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
        public List<BusStopDetails> FavouriteStops { get; set; }
        public MetroLiveSettings Settings { get; set; }

        private SiriManager siriMgr { get; set; }
        private GTFSLoader GTFSData { get; set; }

        //protected 

        //constructor
        public MetroLiveCore(GTFSLoader gtfsLoader, SiriManager mSiriMgr)
        {
            this.GTFSData = gtfsLoader;
            this.siriMgr = mSiriMgr;
            this.FavouriteStops = new List<BusStopDetails>();

            LoadFavStops();
        }

        public async Task<bool> isTimeTableAvaliableOffline()
        {
            return await GTFSData.TimeTableUptoDate();
        }

        public async Task<bool> UpdateTimeTable()
        {
            return await GTFSData.UpdateTimeTable();
        }


        public BusStopDetails GetBusStopDetails(string busId)
        {
            //Convert.ToInt32()
            BusStopDetails busStop = new BusStopDetails(busId, siriMgr);
            return busStop;
        }

        //TODO: load favourite stops
        private void LoadFavStops()
        {
            
        }
    }
}
