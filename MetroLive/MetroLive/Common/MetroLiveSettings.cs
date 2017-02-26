using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLive.Common
{
    public class MetroLiveSettings
    {
        //the earliest bus trip to be fetched from the current time
        public TimeSpan GTFSStart = TimeSpan.FromMinutes(-15);

        //the earilest bus trip to be fetched from the current time
        public TimeSpan SIRIStart = TimeSpan.FromMinutes(0);
        public TimeSpan SIRIPreviewInterval = TimeSpan.FromMinutes(60);
    }
}
