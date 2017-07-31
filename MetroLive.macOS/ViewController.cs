using System;
using System.IO;
using System.IO.Compression;
using AppKit;
using Foundation;
using MetroLive.Services.Offline;
using MetroLive.Services.Realtime;

namespace MetroLive.macOS
{
	public partial class ViewController : NSViewController
	{
        IOffline gtfs;
        IRealtime siri;
		public ViewController(IntPtr handle) : base(handle)
		{
			string path = System.Environment.CurrentDirectory + '/';
			FileManager fileMgr = new FileManager(path);
            gtfs = new Services.Offline.GTFS.GTFSLoaderAdelaide(fileMgr);
            siri = new Services.Realtime.SIRI.SiriMgrAdelaide();
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Do any additional setup after loading the view.
		}

		public override NSObject RepresentedObject
		{
			get
			{
				return base.RepresentedObject;
			}
			set
			{
				base.RepresentedObject = value;
				// Update the view, if already loaded.
			}
		}
		async partial void StartTest(NSObject sender)
		{
            gtfs.DownloadProg += (sender1, e) => 
            {
                Console.WriteLine(e.Percentage);
            };

            //await gtfs.UpdateAsync();

            //Models.BusStopDetails stopDetails = await gtfs.GetStopDataAsync("11981", DateTime.MinValue, DateTime.MaxValue);
			
            Models.BusStopDetails stopDetailsOffline = await gtfs.GetStopDataAsync("11981", new DateTime(2017, 04, 28, 18, 00, 0), new DateTime(2017, 04, 29, 18, 0, 0));
            Models.BusStopDetails stopDetailsOnline =  await siri.GetStopDataAsync("11981", new TimeSpan(2,0,0));

            stopDetailsOnline.MergeStopDetails(stopDetailsOffline);
		}
	}
}
