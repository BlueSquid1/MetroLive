using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MetroLive.Services.Offline;
using MetroLive.Services.Offline.GTFS;
using MetroLive.Views;

using Xamarin.Forms;

namespace MetroLive
{
    public partial class App : Application
    {
        //FileManager mFileMgr = null;
        public App(FileManager fileMgr)
        {
            InitializeComponent();
			GTFSLoader gtfsMgr = new GTFSLoaderAdelaide(fileMgr);
            this.MainPage = new NavigationPage(new MainPage(gtfsMgr));
        }

        protected override void OnStart()
        {

		}

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
