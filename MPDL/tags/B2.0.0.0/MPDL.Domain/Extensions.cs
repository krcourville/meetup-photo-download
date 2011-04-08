using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPDL.Domain {
    public static class Extensions {
        public static DateTime EpocToLocalDateTime(this string milliseconds){
            var millisecondsValue = double.Parse(milliseconds);
            var epoc = new DateTime(1970,1,1,0,0,0,DateTimeKind.Utc);
            return epoc.AddMilliseconds(millisecondsValue).ToLocalTime();
        }

    }
}
