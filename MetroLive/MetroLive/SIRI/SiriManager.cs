using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLive.SIRI
{
    public abstract class SiriManager
    {
        public abstract Task GetStopDataAsync(DateTimeOffset timeRange, string StopRef);
    }
}
