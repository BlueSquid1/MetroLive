using System;
namespace MetroLive.Services.Offline.GTFS.GTFSModels
{
    public class FeedInfo
    {
        public string feed_publisher_name { get; set; }
        public string feed_publisher_url { get; set; }
        public string feed_lang { get; set; }
        public string feed_start_date { get; set; }
        public string feed_end_date { get; set; }
        public int feed_version { get; set; }
    }
}
