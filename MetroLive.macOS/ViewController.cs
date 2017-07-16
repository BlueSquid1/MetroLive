using System;
using System.IO;
using System.IO.Compression;
using AppKit;
using Foundation;
using MetroLive.Services.Offline;

namespace MetroLive.macOS
{
	public partial class ViewController : NSViewController
	{
		public ViewController(IntPtr handle) : base(handle)
		{
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
			string path = System.Environment.CurrentDirectory + '/';
			MetroLive.Services.Offline.FileManager fileMgr = new Services.Offline.FileManager(path);

            MetroLive.Services.Offline.GTFS.GTFSLoaderAdelaide gtfs = new Services.Offline.GTFS.GTFSLoaderAdelaide(fileMgr);

            gtfs.DownloadProg += (sender1, e) => 
            {
                Console.WriteLine(e.Percentage);
            };

            //await gtfs.UpdateAsync();

            Models.BusStopDetails stopDetails = await gtfs.GetStopDataAsync("11981", DateTime.MinValue, DateTime.MaxValue);
            Console.WriteLine(stopDetails.StopId);
		}
	}
}
