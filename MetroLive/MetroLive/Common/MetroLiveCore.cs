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
        public MetroLiveSettings Settings { get; set; }

        private SiriManager siriMgr { get; set; }
        private GTFSLoader GTFSData { get; set; }
        private List<FavouriteStop> favouriteStops;

        //constructor
        public MetroLiveCore(GTFSLoader gtfsLoader, SiriManager mSiriMgr)
        {
            this.GTFSData = gtfsLoader;
            this.siriMgr = mSiriMgr;
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

        public void AddBusToFavourites(string stopId, string customName)
        {
            favouriteStops.Add(new FavouriteStop(stopId, customName));

            //TODO: update storage
        }

        public async Task GetFavStopsAsync()
        {
            //TODO: load from storage

        }
    }
}
