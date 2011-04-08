using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPDL.Domain.Model {
    [Serializable]
    public class MeetupGroup {
        public string Name { get; set; }
        public string UrlName { get; set; }
        public int GroupId { get; set; }

        public override string ToString() {
            return Name;
        }
    }
}
