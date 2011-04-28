using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MPDL.Domain.Model;
using System.Net;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MPDL.Domain.Service {
    public interface IMPDLService {
        #region Operations (12)

        bool ConfigIsSet();

        IEnumerable<MeetupPhoto> DownloadHighRes(IEnumerable<MeetupPhoto> selected);

        void DownloadHighResAsync(IEnumerable<MeetupPhoto> selected, Action<IEnumerable<MeetupPhoto>> onComplete);

        IEnumerable<MeetupAlbum> GetAlbums(bool forceDownload, int groupId);

        void GetAlbumsAsync(bool forceDownload, Action<IEnumerable<MeetupAlbum>> onComplete, int groupId);

        IEnumerable<MeetupGroup> GetAllGroups(bool forceDownload);

        void GetAllGroupsAsync(bool forceDownload, Action<IEnumerable<MeetupGroup>> onComplete);

        MPDLConfig GetConfig();

        IEnumerable<MeetupPhoto> GetThumbnails(bool forceDownload, int albumId);

        void GetThumbnailsAsync(bool forceDownload, Action<IEnumerable<MeetupPhoto>> onComplete, int albumId);

        bool HasGroupsCached();

        void SetBusymessageCallback(Action<string> callback);

        void SetErrorMessageCallback(Action<Exception> callback);

        void SetConfig(MPDLConfig mpdlConfig);

        #endregion Operations

        string GetHighResDownloadFolder();
    }


    public class MPDLService : IMPDLService {
        #region Fields (4)

        private Action<string> busyMessageCallback;
        private MPDLConfig config;
        private readonly string groupsPath;
        private readonly string highresDownloadPath;
        private bool isConfigSet;
        private Action<Exception> errorMessageCallback;

        #endregion Fields

        #region Constructors (1)

        public MPDLService() {
            if (!Directory.Exists(Constants.CacheFolder)) {
                Directory.CreateDirectory(Constants.CacheFolder);
            }
            groupsPath = Path.Combine(Constants.CacheFolder, Constants.GroupsFileName);
            highresDownloadPath = Path.Combine(Constants.CacheFolder, "highres");
            if (!Directory.Exists(highresDownloadPath)) {
                Directory.CreateDirectory(highresDownloadPath);
            }


            isConfigSet = false;
            if (File.Exists(Constants.ConfigFileName)) {
                var serializer = new XmlSerializer(typeof(MPDLConfig));
                using (var file = File.OpenRead(Constants.ConfigFileName)) {
                    try {
                        config = serializer.Deserialize(file) as MPDLConfig;
                        isConfigSet = true;
                    } catch {
                        // corrupted config
                        File.Delete(Constants.ConfigFileName);
                    }
                }
            }
        }

        #endregion Constructors

        #region Methods (18)

        // Public Methods (12) 

        public bool ConfigIsSet() {
            return isConfigSet;
        }

        public IEnumerable<MeetupPhoto> DownloadHighRes(IEnumerable<MeetupPhoto> selected) {
            var totalCount = selected.Count();
            var itemCount = 0;
            foreach (var item in selected) {
                itemCount++;
                BusyMessage("Downloading {0} of {1}: {2}", itemCount, totalCount, item.HighResUrl);
                item.HighResUrl = DownloadImage(item.HighResUrl, highresDownloadPath);
            }
            return selected; // TODO: Maybe.. do more with this return value
        }


        public void DownloadHighResAsync(IEnumerable<MeetupPhoto> selected, Action<IEnumerable<MeetupPhoto>> onComplete) {
            IEnumerable<MeetupPhoto> result = null;
            DoWork(() => DownloadHighRes(selected), () => onComplete(result));
        }

        public IEnumerable<MeetupAlbum> GetAlbums(bool forceDownload, int groupId) {
            CheckConfig();
            var result = new List<MeetupAlbum>();
            var url = string.Format(Constants.AlbumQueryTemplate, config.ApiKey, groupId);
            var doc = GetXml(
                forceDownload,
                url,
                Path.Combine(
                    Constants.CacheFolder,
                    string.Format("albums-{0}.xml", groupId)
                    )
                );
            var albumItems = doc.SelectNodes(@"//items/item");
            foreach (XmlNode item in albumItems) {
                result.Add(new MeetupAlbum {
                    Title = item.SelectSingleNode("title").InnerText,
                    AlbumId = int.Parse(item.SelectSingleNode("photo_album_id").InnerText),
                    DateCreated = item.SelectSingleNode("created").InnerText.EpocToLocalDateTime()
                });
            }

            return result;
        }

        public void GetAlbumsAsync(bool forceDownload, Action<IEnumerable<MeetupAlbum>> onComplete, int groupId) {
            IEnumerable<MeetupAlbum> result = null;
            DoWork(() => { result = new List<MeetupAlbum>(GetAlbums(forceDownload, groupId)); },
                   () => onComplete(result));
        }

        public IEnumerable<MeetupGroup> GetAllGroups(bool forceDownload) {
            CheckConfig();
            var url = string.Format(Constants.GroupQueryTemplate, config.ApiKey, config.MemberId);
            var result = new List<MeetupGroup>();
            var doc = GetXml(forceDownload, url, string.Format(groupsPath));
            var groupItems = doc.SelectNodes(@"//items/item");
            foreach (XmlNode item in groupItems) {
                result.Add(new MeetupGroup {
                    Name = item.SelectSingleNode("name").InnerText,
                    UrlName = item.SelectSingleNode("group_urlname").InnerText,
                    GroupId = int.Parse(item.SelectSingleNode("id").InnerText)
                });
            }

            return result;
        }

        public void GetAllGroupsAsync(bool forceDownload, Action<IEnumerable<MeetupGroup>> onComplete) {
            IEnumerable<MeetupGroup> result = null;
            DoWork(() => { result = new List<MeetupGroup>(GetAllGroups(forceDownload)); }, () => onComplete(result));
        }

        public MPDLConfig GetConfig() {
            return this.config ?? new MPDLConfig();
        }

        public IEnumerable<MeetupPhoto> GetThumbnails(bool forceDownload, int albumId) {
            // look for cached folder for album id
            // cache/thumbs-{albumId}
            var cachePath = Path.Combine(Constants.CacheFolder,
                                         string.Format("thumbs-{0}", albumId));

            var photos = GetPhotoUrls(forceDownload, albumId);

            if (!Directory.Exists(cachePath) || forceDownload) {
                if (!Directory.Exists(cachePath)) {
                    Directory.CreateDirectory(cachePath);
                }
                var itemCount = 0;
                var totalCount = photos.Count();
                foreach (var item in photos) {
                    itemCount++;
                    BusyMessage("Downloading {1} of {2}: {0}", item.ThumbUrl, itemCount, totalCount);
                    item.ThumbUrl = DownloadImage(item.ThumbUrl, cachePath);
                }
            }
            return photos;
        }

        public void GetThumbnailsAsync(bool forceDownload, Action<IEnumerable<MeetupPhoto>> onComplete, int albumId) {
            IEnumerable<MeetupPhoto> result = null;
            DoWork(() => { result = new List<MeetupPhoto>(GetThumbnails(forceDownload, albumId)); },
                   () => onComplete(result));
        }

        public bool HasGroupsCached() {
            return File.Exists(groupsPath);
        }

        public void SetBusymessageCallback(Action<string> callback) {
            busyMessageCallback = callback;
        }

        public void SetErrorMessageCallback(Action<Exception> callback) {
            errorMessageCallback = callback;
        }

        public void SetConfig(MPDLConfig mpdlConfig) {
            if (mpdlConfig == null || string.IsNullOrEmpty(mpdlConfig.ApiKey)) {
                return;
            }

            config = mpdlConfig;
            mpdlConfig.ApiKey = mpdlConfig.ApiKey.Trim();
            //
            // Get MemberId via API call
            BusyMessage("Accessing {0}", "Meetup.com profile");
            var url = string.Format(Constants.MemberQueryTemplate, mpdlConfig.ApiKey);
            var webclient = new WebClient();
            try {
                var stream = webclient.OpenRead(url);
                var reader = new StreamReader(stream);
                var jObject = JObject.Parse(reader.ReadToEnd());
                config.MemberId = (string)jObject.SelectToken("results[0].id");
                Debug.WriteLine(config.MemberId, "MemberId");
                stream.Close();
                if (string.IsNullOrEmpty(config.MemberId)) throw new Exception("Unable to location Member Id");
            } catch (Exception ex) {
                ErrorMessage(ex);
            }

            using (var file = File.OpenWrite(Constants.ConfigFileName)) {
                var serializer = new XmlSerializer(typeof(MPDLConfig));
                serializer.Serialize(file, mpdlConfig);
            }

            isConfigSet = true;
        }

        // Private Methods (6) 

        private void BusyMessage(string format, params object[] args) {
            if (busyMessageCallback != null) {
                busyMessageCallback(string.Format(format, args));
            }
        }

        private void ErrorMessage(Exception ex) {
            if (errorMessageCallback != null) {
                errorMessageCallback(ex);
            }
        }

        private void CheckConfig() {
            if (!isConfigSet) {
                throw new InvalidOperationException("Configuration must be set first.");
            }
        }

        private static string DownloadImage(string url, string outputFolder) {
            var client = new WebClient();
            var stream = client.OpenRead(url);
            var bitmap = new Bitmap(stream);
            stream.Flush();

            var outputPath = Path.Combine(outputFolder, Path.GetFileName(url));
            if (File.Exists(outputPath)) {
                File.Delete(outputPath);
            }

            bitmap.Save(outputPath);

            return new FileInfo(outputPath).FullName;
        }

        private void DoWork(Action start, Action complete) {
            var worker = new BackgroundWorker();
            worker.DoWork += (e, p) => start();
            worker.RunWorkerCompleted += (e, p) => {
                if (p.Error == null) {
                    complete();
                } else {
                    ErrorMessage(p.Error);
                }
            };
            worker.RunWorkerAsync();
        }

        private IEnumerable<MeetupPhoto> GetPhotoUrls(bool forceDownload, int albumId) {
            CheckConfig();
            var result = new List<MeetupPhoto>();
            var url = string.Format(Constants.PhotoQueryTemplate, config.ApiKey, albumId);
            var doc = GetXml(
                forceDownload,
                url,
                Path.Combine(Constants.CacheFolder,
                             string.Format("photos-{0}.xml", albumId))
                );
            var albumItems = doc.SelectNodes(@"//items/item");
            foreach (XmlNode item in albumItems) {
                result.Add(new MeetupPhoto {
                    HighResUrl = item.SelectSingleNode("photo_link").InnerText,
                    ThumbUrl = item.SelectSingleNode("thumb_link").InnerText
                });
            }

            return result;
        }

        private XmlDocument GetXml(bool forceDownload, string url, string fileName) {
            var doc = new XmlDocument();
            if (fileName == null || !File.Exists(fileName) || forceDownload) {
                BusyMessage("Accessing {0}", "Meetup.com");
                // TODO: Check for error, suggest configuration re-entry
                doc.Load(url);
                if (fileName != null) {
                    doc.Save(fileName);
                }
            } else {
                BusyMessage("Accessing {0}", "file system");
                doc.Load(fileName);
            }
            return doc;
        }

        #endregion Methods

        public string GetHighResDownloadFolder() {
            return highresDownloadPath;
        }
    }
}