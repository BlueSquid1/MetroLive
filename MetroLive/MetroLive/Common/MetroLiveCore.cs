using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLive.MetroData;
using MetroLive.SIRI;
using MetroLive.GTFS;

namespace MetroLive.Common
{
    //main interface for all logic operations
    public class MetroLiveCore
    {
        private SiriManager siriMgr { get; set; }
        private GTFSLoader scheduleData { get; set; }


        public MetroLiveSettings Settings { get; set; }

        //protected 

        //constructor
        public MetroLiveCore(GTFSLoader gtfsLoader, SiriManager mSiriMgr)
        {
            this.scheduleData = gtfsLoader;
            this.siriMgr = mSiriMgr;
        }

        
        public BusStopDetails GetBusStopDetails(string busId)
        {
            //Convert.ToInt32()
            BusStopDetails busStop = new BusStopDetails(busId);
            return busStop;
        }
        


        public virtual async Task<bool> TimeTableAvaliableOffline()
        {
            return await scheduleData.TimeTableAvaliableOffline();
        }
    }
}
