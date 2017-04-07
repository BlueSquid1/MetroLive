using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLive.MetroData;
using MetroLive.SIRI;
using MetroLive.GTFS;
using Xamarin.Forms;

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
            //var db = new SQLiteConnection();
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

        public void AddBusToFavourites(FavouriteStop newFavourite)
        {
            if(favouriteStops == null)
            {
                return;
            }
            favouriteStops.Add(newFavourite);

            //TODO: update storage
        }

        public async Task< List<FavouriteStop> > GetFavStopsAsync()
        {
            if (favouriteStops == null)
            {
                //TODO: load from storage
                favouriteStops = new List<FavouriteStop>();
                //return;
            }

            return favouriteStops;
        }
    }
}
