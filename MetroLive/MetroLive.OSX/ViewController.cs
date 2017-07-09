using System;

using AppKit;
using Foundation;
using MetroLive.Realtime.SIRI;

namespace MetroLive.OSX
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

		async partial void TestButtonClicked(NSObject sender)
		{
            var x = new SiriMgrAdelaide();
            await x.GetStopDataAsync("11984", new DateTimeOffset(DateTime.Now.Ticks, TimeSpan.FromMinutes(60)));
		}
    }
}
