using MetroLive.Common;
using MetroLive.MetroData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;


namespace MetroLive.Pages
{
    public partial class VehicleDetailPage : ContentPage
    {
        private MetroLiveCore metroLive;
        private VehicleJourney vehicleDetails;

        public VehicleDetailPage(MetroLiveCore mMetroLive, VehicleJourney mVehicleDetails)
        {
            InitializeComponent();
            this.metroLive = mMetroLive;
            this.vehicleDetails = mVehicleDetails;

            this.Appearing += VehicleDetail_Appearing;
        }

        private async void VehicleDetail_Appearing(object sender, EventArgs e)
        {
            BusId.Text = vehicleDetails.VehicleRef;
            LineRef.Text = vehicleDetails.LineRef;
            
        }
    }
}
