using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLive.Pages.StopDetails
{
    public class BusViewModel
    {
        //for identification purposes only
        public string BusId { get; set; }

        public string LineRef { get; set; }
        public string ExpectArrival { get; set; }
        public string ExpectedUncertainty { get; set; }
        public string TimeDiff { get; set; }
        public string TimeTillArrival { get; set; }
    }

    public class StopDetailsModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Title"));
            }
        }
        private string title;

        public List<BusViewModel> BusCollection
        {
            get
            {
                return busCollection;
            }
            set
            {
                busCollection = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BusCollection"));
            }
        }
        private List<BusViewModel> busCollection;


        //constructor
        public StopDetailsModel()
        {
            this.title = "";
            this.busCollection = new List<BusViewModel>();
        }
    }
}
