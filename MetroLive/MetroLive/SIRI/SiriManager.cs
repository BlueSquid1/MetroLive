using MetroLive.MetroData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLive.SIRI
{
    public abstract class SiriManager
    {
        public abstract Task<BusStopDetails> GetStopDataAsync(string StopRef, DateTimeOffset timeRange);
    }
}
