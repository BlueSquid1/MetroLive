using System;
namespace MetroLive.Services.Offline.GTFS.GTFSModels
{
    public class TripTimesModel
    {
        public int stop_id { get; set; }
        public string departure_time { get; set; }
        public int trip_id { get; set; }
    }
}
