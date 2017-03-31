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
        public List<FavouriteStop> FavouriteStops { get; set; }
        public MetroLiveSettings Settings { get; set; }

        private SiriManager siriMgr { get; set; }
        private GTFSLoader GTFSData { get; set; }

        //protected 

        //constructor
        public MetroLiveCore(GTFSLoader gtfsLoader, SiriManager mSiriMgr)
        {
            this.GTFSData = gtfsLoader;
            this.siriMgr = mSiriMgr;
            this.FavouriteStops = new List<FavouriteStop>();

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


        public BusStopMgr GetBusStopDetails(string busId)
        {
            return new BusStopMgr(busId, GTFSData, siriMgr);
        }

        //TODO: load favourite stops
        private void LoadFavStops()
        {
            
        }
    }
}
