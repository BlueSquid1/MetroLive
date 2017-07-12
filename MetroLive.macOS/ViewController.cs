using System;

using AppKit;
using Foundation;

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
        partial void StartTest(NSObject sender)
        {
            MetroLive.Services.Offline.FileManager x = new Services.Offline.FileManager("./");
        }
    }
}
