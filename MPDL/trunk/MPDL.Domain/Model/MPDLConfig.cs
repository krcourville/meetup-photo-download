using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MPDL.Domain.Model {
    [Serializable]
    public class MPDLConfig {
        [Required]
        public string ApiKey { get; set; }
        [Required]
        internal string MemberId { get; set; }
    }
}
