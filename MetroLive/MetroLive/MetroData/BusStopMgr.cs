using MetroLive.GTFS;
using MetroLive.SIRI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLive.MetroData
{
    public class BusStopMgr
    {
        //for buffering purposes
        private BusStopDetails offlineBusStopData;

        //plateform/area specific code
        private GTFSLoader gtfsLoader;
        private SiriManager sirMgr;

        private string busStopId;


        //constructor
        public BusStopMgr(string mBusStopId, GTFSLoader mGTFSLoader, SiriManager mSiriManager)
        {
            this.busStopId = mBusStopId;
            this.offlineBusStopData = null;
            this.gtfsLoader = mGTFSLoader;
            this.sirMgr = mSiriManager;
        }

        public async Task<BusStopDetails> GetOfflineDataAsync(DateTimeOffset timeInterval )
        {
            //overide previous offline data (maybe?)
                        
            return offlineBusStopData;
        }

        public async Task<BusStopDetails> GetRealTimeDataAsync(DateTimeOffset timeInterval)
        {
            //add to the previous results
            offlineBusStopData = await sirMgr.GetStopDataAsync(busStopId, timeInterval);
            return offlineBusStopData;
        }
    }
}
