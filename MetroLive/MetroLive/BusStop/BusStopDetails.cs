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
        public event EventHandler<EventArgs> NewUpdate;

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
        public BusStopDetails( string busStopId )
        {
            this.BusStopId = busStopId;

            IncomingVehicles = new List<VehicleJourney>();
        }

        //populates bus data
        public abstract void FetchDataAsync(DateTimeOffset timeRange);
    }
}
