using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace MetroLive
{
    public partial class App : Application
    {
        public static IList<string> PhoneNumbers { get; set; }

        public App()
        {
            PhoneNumbers = new List<string>();
            PhoneNumbers.Add("test");
            PhoneNumbers.Add("test2");

            InitializeComponent();

           this.MainPage = new MetroLive.View.StopDetailsView();
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
