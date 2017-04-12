using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLive.MetroData;
using MetroLive.SIRI;
using MetroLive.GTFS;
using Xamarin.Forms;
using MetroLive.Common.EventArgs;

namespace MetroLive.Common
{
    //main interface for all logic operations
    public class MetroLiveCore
    {
        public event EventHandler<DownloadProgEventArgs> downloadProg;

        public SettingsManager SettingsMgr { get; set; }

        private SiriManager siriMgr { get; set; }
        private GTFSLoader GTFSData { get; set; }
        private FileManager fileMgr;

        //constructor
        public MetroLiveCore(FileManager mFileMgr, GTFSLoader gtfsLoader, SiriManager mSiriMgr)
        {
            this.GTFSData = gtfsLoader;
            this.siriMgr = mSiriMgr;
            this.fileMgr = mFileMgr;

            GTFSData.downloadProg += GTFSData_downloadProg;

            this.SettingsMgr = new SettingsManager(mFileMgr);
        }

        private void GTFSData_downloadProg(object sender, DownloadProgEventArgs e)
        {
            downloadProg?.Invoke(this, e);
        }

        public async Task StartUp()
        {
            await SettingsMgr.LoadSettingFromDisk();
        }

        public BusStopMgr GetBusStopDetails(string busId)
        {
            return new BusStopMgr(busId, GTFSData, siriMgr);
        }

        public async Task AddBusToFavourites(FavouriteStop newFavourite)
        {
            SettingsMgr.Settings.FavStops.Add(newFavourite);
            await SettingsMgr.SaveSettingToDisk();
        }

        public List<FavouriteStop> GetFavStops()
        {
            return SettingsMgr.Settings.FavStops;
        }

        public async Task<bool> IsTimeTableUptoDate()
        {
            return await GTFSData.IsTimeTableUptoDate();
        }

        public async Task<bool> DownloadTimeTable()
        {
            return await GTFSData.DownloadTimeTable();
        }
    }
}
