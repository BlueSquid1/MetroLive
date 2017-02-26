using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLive.BusStop
{
    public abstract class BusStopDetails
    {
        //triggered when new information about the stop is discovered
        public event EventHandler<EventArgs> NewInfo;

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

        //constructor
        public BusStopDetails( string stopRef)
        {
            this.StopRef = stopRef;

            IncomingVehicles = new List<VehicleJourney>();
        }

        public abstract Task StartListeningAsyc();

        public abstract Task FetchscheduledDataAsync(DateTimeOffset timeRange);

        //populates bus data
        public abstract Task FetchLiveDataAsync(DateTimeOffset timeRange);
    }
}
