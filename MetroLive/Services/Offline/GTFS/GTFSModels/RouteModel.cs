using System;
namespace MetroLive.Services.Offline.GTFS.GTFSModels
{
    public class RouteModel
    {
        public string route_id { get; set; }

        public string route_short_name { get; set; }
        public string route_long_name { get; set; }
        public string route_desc { get; set; }
        public string route_color { get; set; }
    }
}
