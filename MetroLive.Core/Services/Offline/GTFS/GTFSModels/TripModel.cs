using System;
namespace MetroLive.Services.Offline.GTFS.GTFSModels
{
    public class TripModel
    {
        public string route_id { get; set; }
        public int service_id { get; set; }
        public int trip_id { get; set; }
        public string trip_headsign { get; set; }
        public int direction_id { get; set; }
        public int? wheelchair_accessible { get; set; }
    }
}
