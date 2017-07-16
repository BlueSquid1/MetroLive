using System;
namespace MetroLive.Services.Offline.GTFS.GTFSModels
{
    public class StopModel
    {
        public int stop_id { get; set; }
        public string stop_code { get; set; }
        public string stop_name { get; set; }
        public float stop_lat { get; set; }
        public float stop_lon { get; set; }
    }
}
