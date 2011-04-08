using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPDL.Domain.Model {
    [Serializable]
    public class MeetupAlbum {
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public DateTime DateCreated { get; set; }


        public override string ToString() {
            return string.Format("{0:yyyy-MM-dd} {1}", DateCreated, Title);
        }
    }

}
