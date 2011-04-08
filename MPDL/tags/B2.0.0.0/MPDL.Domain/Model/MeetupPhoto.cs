using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPDL.Domain.Model {
    [Serializable]
    public class MeetupPhoto {
        public string HighResUrl { get; set; }
        public string ThumbUrl { get; set; }
        public override string ToString() {
            return ThumbUrl;
        }
    }

}
