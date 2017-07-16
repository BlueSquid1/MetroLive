using System;
using System.Collections.Generic;

namespace MetroLive.Models
{
    public class BusStopDetails
    {
        public int StopId { get; set; }

        //bus stop reference number
        public string StopRef { get; set; }

        //response timestamp
        public DateTime? RspTimestamp { get; set; }

        //valid until timestamp
        //public DateTime? ValidUntil { get; set; }

        //e.g. 38 Lower Athelstone Road
        public string StopPointName { get; set; }


        public List<VehicleJourney> IncomingVehicles { get; set; }


        //bus stop coordinates
        public float busStopX { get; set; }
        public float busStopY { get; set; }


        public string Version { get; set; }

        //constructor
        public BusStopDetails()
        {
            IncomingVehicles = new List<VehicleJourney>();
        }
    }
}
