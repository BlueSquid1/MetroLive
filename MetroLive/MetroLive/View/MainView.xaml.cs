using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MetroLive.View
{
    public partial class MainView : ContentPage
    {
        public MainView()
        {
            InitializeComponent();
        }

        private async void OnTripDetail(object sender, EventArgs e)
        {
            //use NavigationPage instead
            await this.Navigation.PushAsync( new View.StopDetailsView(Convert.ToInt32(txtBusID.Text)));
        }
    }
}
