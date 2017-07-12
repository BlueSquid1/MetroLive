﻿using MetroLive.GTFS;
using MetroLive.SIRI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLive.MetroData
{
    public class BusStopMgr
    {
        //for buffering purposes
        public BusStopDetails BusStopData { get; set; }

        //plateform/area specific code
        private GTFSLoader gtfsLoader;
        private SiriManager sirMgr;

        public string BusStopId { get; set; }


        //constructor
        public BusStopMgr(string mBusStopId, GTFSLoader mGTFSLoader, SiriManager mSiriManager)
        {
            this.BusStopId = mBusStopId;
            this.BusStopData = null;
            this.gtfsLoader = mGTFSLoader;
            this.sirMgr = mSiriManager;
        }

        public async Task<BusStopDetails> GetOfflineDataAsync(DateTime startTime, TimeSpan timeLength )
        {
            BusStopDetails stopDataTemp = null;
            stopDataTemp = await gtfsLoader.GetBusStopData(BusStopId, startTime, timeLength);
            //populate bus stop from GTFS data

            //dump into offiline buffer
            BusStopData = stopDataTemp;
            return BusStopData;
        }

        public async Task<BusStopDetails> GetRealTimeDataAsync(DateTimeOffset timeInterval)
        {
            BusStopDetails newStopData = new BusStopDetails();
            try
            {
                newStopData = await sirMgr.GetStopDataAsync(BusStopId, timeInterval);
            }
            catch
            {

            }
            //add newStopData to the previous results
            BusStopData = newStopData;
            return BusStopData;
        }
    }
}