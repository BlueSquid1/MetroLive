using MetroLive.Common;
using MetroLive.Pages.StopDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
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
        }

        public async void OnSearch(object sender, EventArgs e)
        {
            SearchBar searchView = (SearchBar)sender;
            StopDetailPage stopDetails = new StopDetailPage(metroLive, searchView.Text);
            await this.Navigation.PushAsync(stopDetails);
        }
    }
}
