using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace MetroLive.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        [Preserve]
        void OnButtonClicked(object sender, EventArgs args)
        {
            Console.WriteLine("got here");
        }

        [Preserve]
        void Handle_SearchButtonPressed(object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
