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
            //update favourites list
            mainPageModel.FavStops = await this.metroLive.GetFavStopsAsync();
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
