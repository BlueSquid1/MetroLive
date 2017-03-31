using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLive.Pages.StopDetails
{
    public class StopDetailsModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Surname
        {
            get
            {
                return surname;
            }
            set
            {
                surname = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Surname"));
            }
        }

        private string surname = "Smith";

    }
}
