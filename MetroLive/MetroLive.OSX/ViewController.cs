using System;
using System.IO;
using System.Net;
using AppKit;
using Foundation;
using MetroLive.Services.Offline;
using MetroLive.Services.Offline.GTFS;

namespace MetroLive.OSX
{
    public partial class ViewController : NSViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
            Console.WriteLine("startup");
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

		async partial void TestButtonClicked(NSObject sender)
		{
            string url = "http://spiderpig1.duckdns.org/public%2Fgoogle_transit.zip";

            string rootPath = Environment.CurrentDirectory + "/";

            FileManager fileMgr = new FileManager(rootPath);
            GTFSLoaderAdelaide x = new GTFSLoaderAdelaide(fileMgr);
            await x.UpdateAsync();

		}
    }
}
