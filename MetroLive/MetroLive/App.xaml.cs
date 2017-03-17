using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MetroLive.Pages;

using Xamarin.Forms;
using MetroLive.GTFS;
using MetroLive.SIRI;
using MetroLive.Common;
using MetroLive.SIRI.Adelaide;

namespace MetroLive
{
    public partial class App : Application
    {
        private MetroLiveCore metroLive;

        public App(FileManager fileMgr)
        {
            InitializeComponent();

            GTFSLoader gtfsLoader = new GTFSLoaderAdelaide(fileMgr);
            SiriManager siriMgr = new SiriMgrAdelaide();

            metroLive = new MetroLiveCore(gtfsLoader, siriMgr);
            this.MainPage = new NavigationPage(new MainPage(metroLive));
        }

        protected override void OnStart()
        {
            // Handle when your app starts
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
