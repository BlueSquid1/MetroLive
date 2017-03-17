using MetroLive.GTFS;
using MetroLive.SIRI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MetroLive.SIRI.Adelaide;
using Newtonsoft.Json;

namespace MetroLive.MetroData
{
    public class BusStopDetails
    {
        //bus stop reference number
        public string StopRef { get; set; }

        //response timestamp
        public DateTime? RspTimestamp { get; set; }
        
        //valid until timestamp
        public DateTime? ValidUntil { get; set; }

        //e.g. 38 Lower Athelstone Road
        public string StopPointName { get; set; }


        public List<VehicleJourney> IncomingVehicles { get; set; }


        //bus stop coordinates
        public float busStopX { get; set; }
        public float busStopY { get; set; }


        public string Version { get; set; }

        private SiriManager siriMgr;
        private GTFSLoader gtfsLoader;

        //constructor
        public BusStopDetails( string stopRef)
        {
            this.StopRef = stopRef;

            IncomingVehicles = new List<VehicleJourney>();
        }

        public Task FetchscheduledDataAsync(DateTimeOffset timeRange)
        {
            throw new NotImplementedException();
        }
        

        //populates bus data
        public async Task FetchLRealTimeDataAsync(DateTimeOffset timeRange)
        {
            await siriMgr.GetStopDataAsync(timeRange, StopRef);

        }

    }
}
