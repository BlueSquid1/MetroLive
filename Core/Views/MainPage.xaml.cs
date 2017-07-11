using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLive.Services.Offline;
using MetroLive.Services.Offline.GTFS;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MetroLive.Views
{
    public partial class MainPage : ContentPage
    {
        GTFSLoader gtfsMgr = null;
        //constructor
        public MainPage(GTFSLoader mGtfsMgr)
        {
            InitializeComponent();
            this.gtfsMgr = mGtfsMgr;
            this.Appearing += MainPage_Appearing;


            //bind the model
            //this.BindingContext = mainPageModel;
        }

        private async void MainPage_Appearing(object sender, EventArgs e)
        {
            //FileManager fileMgr = new FileManager(rootPath);
            //GTFSLoaderAdelaide x = new GTFSLoaderAdelaide(fileMgr);
            //await x.UpdateAsync();
            testLb.Text = "downloading...";
            await gtfsMgr.UpdateAsync();
            testLb.Text = "done";
        }

        public async void OnSearch(object sender, EventArgs e)
        {
            
        }

        public async void OnSelectedItem(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}
