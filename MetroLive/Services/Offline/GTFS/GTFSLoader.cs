using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MetroLive.Models;
using MetroLive.Services.Offline.GTFS.GTFSModels;
using ServiceStack.Text;

namespace MetroLive.Services.Offline.GTFS
{
    public class OpenArchive
    {
		public string completePath;
		public ZipArchive zip;

        //constructor
        public OpenArchive( string mCompletePath, ZipArchive mZip)
        {
            this.completePath = mCompletePath;
            this.zip = mZip;
        }
    }

	public class GTFSLoader : IOffline
	{
		public event EventHandler<DownloadProgEventArgs> DownloadProg;

		private string GTFSBaseUrl;
        private string FeedInfoUrl;
		private FileManager fileMgr;
        private string archiveFileName;
        private List<OpenArchive> bufferedArchives;
        private List<StopModel> stopsList = null;
        private List<TripModel> tripList = null;
        private List<StringEntry> timeStringList = null;
        private List<RouteModel> routeList = null;

		//constructor
		public GTFSLoader(FileManager mFileMgr, string mGTFSBaseUrl, string mFeedInfoUrl)
		{
            this.bufferedArchives = new List<OpenArchive>();
			this.fileMgr = mFileMgr;
			this.GTFSBaseUrl = mGTFSBaseUrl;
            this.FeedInfoUrl = mFeedInfoUrl;
			this.archiveFileName = "GTFSLoader.zip";
		}

        async private Task<byte[]> DownloadGTFS()
        {
            byte[] downloadFile = await this.DownloadFile(this.GTFSBaseUrl, true);
            return downloadFile;
        }

		public async Task<bool> UpdateAsync()
		{
			try
			{
                Console.WriteLine("downloading GTFS");
                byte[] gtfsFile = await this.DownloadGTFS();
				await fileMgr.WriteBytesToFileAsync(archiveFileName, gtfsFile);
				Console.WriteLine("done writing file");
				return true;
			}
			catch
			{
                Console.WriteLine("Failed to update GTFS.");
				return false;
			}
		}


		public async Task<BusStopDetails> GetStopDataAsync(string StopRef, DateTime timeStart, DateTime timeEnd, bool forceRefresh = false)
		{
            //check if its in the buffer
            OpenArchive gtfsArchive = GetArchiveFromBuffer(this.archiveFileName);
            if (gtfsArchive == null)
            {
                string completePath = this.fileMgr.FilePathRoot + this.archiveFileName;
                ZipArchive zipFile = this.fileMgr.GetZipFile(this.archiveFileName);
                gtfsArchive = new OpenArchive(completePath, zipFile);
                this.bufferedArchives.Add(gtfsArchive);
            }

            //get stop id
            //find stop id from stop name or stop code
            StopModel stopObj = GetStopFromStopRef(gtfsArchive, StopRef);


            //get expected trips at stop
            List<TripTimesModel> trips = GetTripsFromStopId(gtfsArchive,stopObj.stop_id, timeStart, timeEnd);


			//fill in the details
			BusStopDetails stopDetails = new BusStopDetails();

            stopDetails.StopId = stopObj.stop_id;
            stopDetails.StopRef = stopObj.stop_code;
            stopDetails.StopPointName = stopObj.stop_name;
            stopDetails.busStopX = stopObj.stop_lat;
            stopDetails.busStopY = stopObj.stop_lon;

            foreach(TripTimesModel trip in trips)
            {
                //details from trip times
                VehicleJourney vehicleJourney = new VehicleJourney();
                TimeSpan startTime = timeStart.TimeOfDay;
                TimeSpan endTime = timeEnd.TimeOfDay;
                TimeSpan tripArrivalTime = trip.departure_time;
                DateTime startOfDate = timeStart.Date;
                startOfDate = startOfDate.Add(tripArrivalTime);

                vehicleJourney.TripId = trip.trip_id;
                vehicleJourney.AimedArrival = startOfDate;
                vehicleJourney.ConfidenceLevel = 3; //not real time


				//get more details from trip.csv
				TripModel tripDetails = GetTripFromTripId(gtfsArchive, trip.trip_id);
                vehicleJourney.DirrectionAway = tripDetails.direction_id == 1;

				//get more details from routes.csv
				RouteModel routeDetails = GetRouteFromRouteId( gtfsArchive, tripDetails.route_id);
                vehicleJourney.LineRef = routeDetails.route_short_name;

                stopDetails.IncomingVehicles.Add(vehicleJourney);
            }
            stopDetails.IncomingVehicles.Sort();
            return stopDetails;
		}

		async public Task<bool> IsUpdateAvaliable()
		{
            try
            {
                //check if its in the buffer
                OpenArchive gtfsArchive = GetArchiveFromBuffer(this.archiveFileName);
                if (gtfsArchive == null)
                {
                    string completePath = this.fileMgr.FilePathRoot + this.archiveFileName;
                    ZipArchive zipFile = this.fileMgr.GetZipFile(this.archiveFileName);
                    gtfsArchive = new OpenArchive(completePath, zipFile);
                    this.bufferedArchives.Add(gtfsArchive);
                }

                //done by checking if the feed version number is different
                List<FeedInfo> feedInfos = null;
                using (Stream infoStream = gtfsArchive.zip.GetEntry("google_transit/feed_info.csv").Open())
                {
                    feedInfos = CsvSerializer.DeserializeFromStream<List<FeedInfo>>(infoStream);
                }

                FeedInfo feedInfo = feedInfos.First();
                int localVersion = feedInfo.feed_version;

                //compare this local version with the online version
                byte[] feedFile = await DownloadFile(this.FeedInfoUrl);
                string feedString = Encoding.UTF8.GetString(feedFile);
                List<FeedInfo> onlineFeedInfos = CsvSerializer.DeserializeFromString<List<FeedInfo>>(feedString);
                FeedInfo onlineFeedInfo = onlineFeedInfos.First();
                int onlineVersion = onlineFeedInfo.feed_version;

                return onlineVersion != localVersion;
            }
            catch
            {
                //failed somewhere with reading local file
                //return true so caller will update local version
                return true;
            }
		}

        private async Task<Byte[]> DownloadFile(string url, bool verbose = false)
        {
            if(verbose == true)
            {
                DownloadProg?.Invoke(this, new DownloadProgEventArgs(0, 0));
            }

            byte[] downloadFile = null;
			using (var httpClient = new HttpClient())
			using (var dataStream = await httpClient.GetStreamAsync(this.FeedInfoUrl))
			{
				int fileLen = (int)dataStream.Length;
				int receivedBytes = 0;
				byte[] buffer = new byte[fileLen];

				downloadFile = new byte[fileLen];

				while (receivedBytes < fileLen)
				{
					int bytesRead = await dataStream.ReadAsync(buffer, 0, buffer.Length);
					Array.Copy(buffer, 0, downloadFile, receivedBytes, bytesRead);
					receivedBytes += bytesRead;
                    if (verbose == true)
                    {
                        DownloadProg?.Invoke(this, new DownloadProgEventArgs(receivedBytes, fileLen));
                    }
				}
			}
            return downloadFile;
        }

		protected StopModel GetStopFromStopRef(OpenArchive gtfsArchive, string stopRef)
        {
            if (stopsList == null)
            {
                using (Stream stopsStream = gtfsArchive.zip.GetEntry("google_transit/stops.csv").Open())
                {
                    this.stopsList = CsvSerializer.DeserializeFromStream<List<StopModel>>(stopsStream);
                }
            }

            StopModel stopObj = stopsList.FirstOrDefault(o => o.stop_code == stopRef);
            return stopObj;
        }

        protected List<TripTimesModel> GetTripsFromStopId(OpenArchive gtfsArchive, int stopId, DateTime timeStart, DateTime timeEnd)
        {
            if( timeStart > timeEnd )
            {
                throw new ArgumentException("The end time must be greater than or equal to the start time");
            }
            else if(timeEnd.Subtract(timeStart).TotalDays > 1.0)
            {
                throw new ArgumentException("The start and end dates must be less than 1 day appart");
            }
            if (this.timeStringList == null)
            {
                this.timeStringList = new List<StringEntry>();
                using (Stream timeStream = gtfsArchive.zip.GetEntry("google_transit/stop_times.csv").Open())
                {
					StreamReader timeReader = new StreamReader(timeStream);
                    //skip header
                    timeReader.ReadLine();
					while (timeReader.EndOfStream == false)
					{
						string timeLine = timeReader.ReadLine();
                        this.timeStringList.Add( new StringEntry(timeLine) );
					}
                }
            }

            //binary search this.timeStringList for the valid entries
            StringEntry stopKey = new StringEntry(stopId.ToString());
            int index = this.timeStringList.BinarySearch( stopKey );

            if(index < 0 || index > this.timeStringList.Count())
            {
                throw new KeyNotFoundException("Failed to find a entry for the stop id:" + stopId.ToString() + " in stop_times.csv");
            }

            //find the start of this section
            while(this.timeStringList[index - 1].CompareTo(stopKey) == 0)
            {
                --index;
            }

            List<TripTimesModel> tripsAtStop = new List<TripTimesModel>();

			//populate the return entries
			while (this.timeStringList[index].CompareTo(stopKey) == 0)
            {
                TripTimesModel tripTime = new TripTimesModel();
                string entry = this.timeStringList[index].Entry;
                string[] entries = entry.Split(',');
                tripTime.stop_id = int.Parse(entries[0]);
                tripTime.departure_time = GetTimeSpanFromString(entries[1]);
                tripTime.trip_id = int.Parse(entries[2]);
                bool isInRange = IsTimeInRange(ref tripTime, timeStart, timeEnd);
                if(isInRange)
                {
                    IsTimeInRange(ref tripTime, timeStart, timeEnd);
                    tripsAtStop.Add(tripTime);
                }
                ++index;
            }

            return tripsAtStop;
        }

        private bool IsTimeInRange(ref TripTimesModel trip, DateTime timeStart, DateTime timeEnd)
        {
            DateTime tempEnd = timeEnd;

            bool multiDays = false;
            DateTime secBeforeMidnight = timeStart.Date.Add(new TimeSpan(23,59,59));
            if(timeEnd > secBeforeMidnight)
            {
                multiDays = true;
                tempEnd = secBeforeMidnight;
            }

            if (trip.departure_time >= timeStart.TimeOfDay && trip.departure_time <= tempEnd.TimeOfDay )
            {
                return true;
            }

            if(multiDays)
            {
                if(trip.departure_time <= timeEnd.TimeOfDay)
                {
                    trip.departure_time = trip.departure_time.Add(new TimeSpan(1,0,0,0));
                    return true;
                }
            }

            return false;
        }

        protected TripModel GetTripFromTripId(OpenArchive gtfsArchive, int tripId)
        {
            if(tripList == null)
            {
                using (Stream tripStream = gtfsArchive.zip.GetEntry("google_transit/trips.csv").Open())
				{
					tripList = CsvSerializer.DeserializeFromStream<List<TripModel>>(tripStream);
				}
            }

            TripModel tripDetails = tripList.FirstOrDefault(o => o.trip_id == tripId);
			return tripDetails;
        }

        protected RouteModel GetRouteFromRouteId(OpenArchive gtfsArchive, string routeId)
        {
			if (routeList == null)
			{
				using (Stream routeStream = gtfsArchive.zip.GetEntry("google_transit/routes.csv").Open())
				{
					routeList = CsvSerializer.DeserializeFromStream<List<RouteModel>>(routeStream);
				}
			}

            RouteModel routeDetails = routeList.FirstOrDefault(o => o.route_id == routeId);
			return routeDetails;
        }

		protected TimeSpan GetTimeSpanFromString(string timeString)
        {
            //timeString has the format
            //"13:26:38"
            List<string> timeParts = timeString.Split(':').ToList();
            int hour = int.Parse(timeParts[0]);
            int minute = int.Parse(timeParts[1]);
            int second = int.Parse(timeParts[2]);

            //for some reason 12:00am is 24:00 and not 00:00
            if(hour == 24)
            {
                hour = 0;
            }

            TimeSpan time = new TimeSpan(hour, minute, second);
            return time;
		}

        private OpenArchive GetArchiveFromBuffer(string fileArchive)
        {
            string completePath = this.fileMgr.FilePathRoot + fileArchive;
			//check if archive file is in buffer
			foreach (OpenArchive zipArchive in this.bufferedArchives)
			{
				if (zipArchive.completePath == completePath)
				{
					return zipArchive;
				}
			}
            //failed to find archive in buffer
            return null;
        }
    }
}
