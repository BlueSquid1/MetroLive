using MetroLive.Common;
using MetroLive.Pages.StopDetails;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLive.Pages.MainPage
{
    class MainPageModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public List<FavouriteStop> FavStops
        {
            get
            {
                return favStops;
            }
            set
            {
                favStops = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FavStops"));
            }
        }
        private List<FavouriteStop> favStops;

        //constructor
        public MainPageModel()
        {
            favStops = new List<FavouriteStop>();
            //favStops.Add(new FavouriteStop("test", "", new Xamarin.Forms.Color()));
        }
    }
}
