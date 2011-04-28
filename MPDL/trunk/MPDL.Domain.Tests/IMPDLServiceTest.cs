using MPDL.Domain.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MPDL.Domain.Model;
using System.Collections.Generic;

namespace MPDL.Domain.Tests
{
    
    
    /// <summary>
    ///This is a test class for IMPDLServiceTest and is intended
    ///to contain all IMPDLServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class IMPDLServiceTest {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext {
            get {
                return testContextInstance;
            }
            set {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        internal virtual IMPDLService CreateIMPDLService() {
            // TODO: Instantiate an appropriate concrete class.
            IMPDLService target = new MPDLService();
            return target;
        }

        /// <summary>
        ///A test for ConfigIsSet
        ///</summary>
        [TestMethod()]
        public void ConfigIsSetTest() {
            IMPDLService target = CreateIMPDLService(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.ConfigIsSet();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DownloadHighRes
        ///</summary>
        [TestMethod()]
        public void DownloadHighResTest() {
            IMPDLService target = CreateIMPDLService(); // TODO: Initialize to an appropriate value
            IEnumerable<MeetupPhoto> selected = null; // TODO: Initialize to an appropriate value
            IEnumerable<MeetupPhoto> expected = null; // TODO: Initialize to an appropriate value
            IEnumerable<MeetupPhoto> actual;
            actual = target.DownloadHighRes(selected);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DownloadHighResAsync
        ///</summary>
        [TestMethod()]
        public void DownloadHighResAsyncTest() {
            IMPDLService target = CreateIMPDLService(); // TODO: Initialize to an appropriate value
            IEnumerable<MeetupPhoto> selected = null; // TODO: Initialize to an appropriate value
            Action<IEnumerable<MeetupPhoto>> onComplete = null; // TODO: Initialize to an appropriate value
            target.DownloadHighResAsync(selected, onComplete);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for GetAlbums
        ///</summary>
        [TestMethod()]
        public void GetAlbumsTest() {
            IMPDLService target = CreateIMPDLService(); // TODO: Initialize to an appropriate value
            bool forceDownload = false; // TODO: Initialize to an appropriate value
            int groupId = 0; // TODO: Initialize to an appropriate value
            IEnumerable<MeetupAlbum> expected = null; // TODO: Initialize to an appropriate value
            IEnumerable<MeetupAlbum> actual;
            actual = target.GetAlbums(forceDownload, groupId);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetAlbumsAsync
        ///</summary>
        [TestMethod()]
        public void GetAlbumsAsyncTest() {
            IMPDLService target = CreateIMPDLService(); // TODO: Initialize to an appropriate value
            bool forceDownload = false; // TODO: Initialize to an appropriate value
            Action<IEnumerable<MeetupAlbum>> onComplete = null; // TODO: Initialize to an appropriate value
            int groupId = 0; // TODO: Initialize to an appropriate value
            target.GetAlbumsAsync(forceDownload, onComplete, groupId);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for GetAllGroups
        ///</summary>
        [TestMethod()]
        public void GetAllGroupsTest() {
            IMPDLService target = CreateIMPDLService(); // TODO: Initialize to an appropriate value
            bool forceDownload = false; // TODO: Initialize to an appropriate value
            IEnumerable<MeetupGroup> expected = null; // TODO: Initialize to an appropriate value
            IEnumerable<MeetupGroup> actual;
            actual = target.GetAllGroups(forceDownload);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetAllGroupsAsync
        ///</summary>
        [TestMethod()]
        public void GetAllGroupsAsyncTest() {
            IMPDLService target = CreateIMPDLService(); // TODO: Initialize to an appropriate value
            bool forceDownload = false; // TODO: Initialize to an appropriate value
            Action<IEnumerable<MeetupGroup>> onComplete = null; // TODO: Initialize to an appropriate value
            target.GetAllGroupsAsync(forceDownload, onComplete);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for GetConfig
        ///</summary>
        [TestMethod()]
        public void GetConfigTest() {
            IMPDLService target = CreateIMPDLService(); // TODO: Initialize to an appropriate value
            MPDLConfig expected = null; // TODO: Initialize to an appropriate value
            MPDLConfig actual;
            actual = target.GetConfig();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetHighResDownloadFolder
        ///</summary>
        [TestMethod()]
        public void GetHighResDownloadFolderTest() {
            IMPDLService target = CreateIMPDLService(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.GetHighResDownloadFolder();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetThumbnails
        ///</summary>
        [TestMethod()]
        public void GetThumbnailsTest() {
            IMPDLService target = CreateIMPDLService(); // TODO: Initialize to an appropriate value
            bool forceDownload = false; // TODO: Initialize to an appropriate value
            int albumId = 0; // TODO: Initialize to an appropriate value
            IEnumerable<MeetupPhoto> expected = null; // TODO: Initialize to an appropriate value
            IEnumerable<MeetupPhoto> actual;
            actual = target.GetThumbnails(forceDownload, albumId);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetThumbnailsAsync
        ///</summary>
        [TestMethod()]
        public void GetThumbnailsAsyncTest() {
            IMPDLService target = CreateIMPDLService(); // TODO: Initialize to an appropriate value
            bool forceDownload = false; // TODO: Initialize to an appropriate value
            Action<IEnumerable<MeetupPhoto>> onComplete = null; // TODO: Initialize to an appropriate value
            int albumId = 0; // TODO: Initialize to an appropriate value
            target.GetThumbnailsAsync(forceDownload, onComplete, albumId);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for HasGroupsCached
        ///</summary>
        [TestMethod()]
        public void HasGroupsCachedTest() {
            IMPDLService target = CreateIMPDLService(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.HasGroupsCached();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SetBusymessageCallback
        ///</summary>
        [TestMethod()]
        public void SetBusymessageCallbackTest() {
            IMPDLService target = CreateIMPDLService(); // TODO: Initialize to an appropriate value
            Action<string> callback = null; // TODO: Initialize to an appropriate value
            target.SetBusymessageCallback(callback);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SetConfig
        ///</summary>
        [TestMethod()]
        public void SetConfigTest() {
            var target = CreateIMPDLService(); // TODO: Initialize to an appropriate value
            var config = new MPDLConfig { ApiKey = "" }; // TODO: Add API Key here; remove after testing
            target.SetConfig(config);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SetErrorMessageCallback
        ///</summary>
        [TestMethod()]
        public void SetErrorMessageCallbackTest() {
            IMPDLService target = CreateIMPDLService(); // TODO: Initialize to an appropriate value
            Action<Exception> callback = null; // TODO: Initialize to an appropriate value
            target.SetErrorMessageCallback(callback);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
