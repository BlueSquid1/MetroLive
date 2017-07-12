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
namespace MetroLive.Pages.MainPage
{
    public partial class MainPage : ContentPage
    {
        private MetroLiveCore metroLive;

        private MainPageModel mainPageModel;

        //constructor
        public MainPage(MetroLiveCore mMetroLive)
        {
            InitializeComponent();
            this.Appearing += MainPage_Appearing;

            this.mainPageModel = new MainPageModel();

            this.metroLive = mMetroLive;

            //bind the model
            this.BindingContext = mainPageModel;
        }

        private async void MainPage_Appearing(object sender, EventArgs e)
        {
            await metroLive.EarlyStartup();

            mainPageModel.FavStops = this.metroLive.GetFavStops();

            //check if GTFS exists locally
            bool isAvaliable = await metroLive.IsTimeTableUptoDate();

            if (isAvaliable == false)
            {
                //download timetable
                mainPageModel.ShowOverlay = true;
                bool downloadSuccess = false;
                while (!downloadSuccess)
                {
                    downloadSuccess = await metroLive.DownloadTimeTable();
                    if (downloadSuccess == false)
                    {
                        await DisplayAlert("Download Failed", "failed to download the latest timetable. Press Ok to try again", "Ok");
                        return;
                    }
                }
                mainPageModel.ShowOverlay = false;
            }
        }

        public async void OnSearch(object sender, EventArgs e)
        {
            SearchBar searchView = (SearchBar)sender;
            StopDetailPage stopDetails = new StopDetailPage(metroLive, searchView.Text);
            await this.Navigation.PushAsync(stopDetails);
        }

        public async void OnSelectedItem(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}
