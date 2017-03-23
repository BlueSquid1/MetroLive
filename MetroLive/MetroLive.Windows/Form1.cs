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
            GTFSLoader gtfsLoader = new GTFSLoaderAdelaide(fileMgr);
            metroCore = new MetroLiveCore(gtfsLoader, siriMgr);
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            BusStopMgr stopMgr = metroCore.GetBusStopDetails("13277");
            BusStopDetails y = await stopMgr.GetRealTimeDataAsync(new DateTimeOffset(DateTime.Now.Ticks, TimeSpan.FromMinutes(60)));
        }
    }
}
