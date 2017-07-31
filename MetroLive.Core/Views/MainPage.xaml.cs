using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace MetroLive.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        void OnButtonClicked(object sender, EventArgs args)
        {
            Console.WriteLine("got here");
        }
    }
}
