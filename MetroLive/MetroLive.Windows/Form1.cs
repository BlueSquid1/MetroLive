using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroLive.Common;
using MetroLive.GTFS;
using MetroLive.SIRI;
using MetroLive.MetroData;
using Newtonsoft.Json;
using MetroLive.SIRI.Adelaide;

namespace MetroLive.Windows
{
    public partial class Form1 : Form
    {
        private MetroLiveCore metroCore;
        public Form1()
        {
            InitializeComponent();
            
            FileManager fileMgr = new FileManagerWindows();
            SiriManager siriMgr = new SiriMgrAdelaide();
            bool useUncompressedGTFS = true;
            GTFSLoader gtfsLoader = new GTFSLoaderAdelaide(fileMgr, useUncompressedGTFS);
            metroCore = new MetroLiveCore(fileMgr, gtfsLoader, siriMgr);
            
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await metroCore.EarlyStartup();

            if (await metroCore.IsTimeTableUptoDate() == false)
            {
                await metroCore.DownloadTimeTable();
            }
            BusStopMgr stopMgr = metroCore.GetBusStopDetails("11984");
            DateTime now = DateTime.Now;
            DateTime lastMidnight = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            BusStopDetails stopDetailsOffline = await stopMgr.GetOfflineDataAsync(new DateTime(lastMidnight.Ticks), TimeSpan.FromDays(1));
        }
    }
}
