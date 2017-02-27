﻿using MetroLive.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MetroLive.Pages
{
    public partial class MainPage : ContentPage
    {
        private MetroLiveCore metroLive;
        //constructor
        public MainPage(MetroLiveCore mMetroLive)
        {
            InitializeComponent();
            metroLive = mMetroLive;

            this.Appearing += MainView_Appearing;
        }

        private async void MainView_Appearing(object sender, EventArgs e)
        {
            bool isTimeTableLocal = await metroLive.TimeTableAvaliableOffline();
            if(isTimeTableLocal == false)
            {
                //download timetable

            }
        }

        private async void OnTripDetail(object sender, EventArgs e)
        {
            StopDetailPage stopDetails = new StopDetailPage(metroLive, txtBusID.Text);
            await this.Navigation.PushAsync(stopDetails);
        }

    }
}
