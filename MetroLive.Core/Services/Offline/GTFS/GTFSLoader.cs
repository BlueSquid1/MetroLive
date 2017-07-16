using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Text;
using System.Diagnostics;
using MetroLive.Models;
using MetroLive.Services.Offline.GTFS.GTFSModels;

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
	public class GTFSLoader //: IOffline
	{
		public event EventHandler<DownloadProgEventArgs> DownloadProg;

		private string GTFSBaseUrl;
		private FileManager fileMgr;
        private string archiveFileName;
        private List<OpenArchive> bufferedArchives;
        private List<StopModel> stopsList = null;
        private List<TripModel> tripList = null;
        private List<RouteModel> routeList = null;

		//constructor
		public GTFSLoader(FileManager mFileMgr, string baseUrl)
		{
            this.bufferedArchives = new List<OpenArchive>();
			this.fileMgr = mFileMgr;
			this.GTFSBaseUrl = baseUrl;
			this.archiveFileName = "GTFSLoader.zip";
		}

        //assume file length is less than 4Gb
        async private Task<byte[]> DownloadGTFS()
        {
            byte[] downloadFile;

            DownloadProg?.Invoke(this, new DownloadProgEventArgs(0, 0));

			using (var httpClient = new HttpClient())
			using (var dataStream = await httpClient.GetStreamAsync(GTFSBaseUrl))
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
                    DownloadProg?.Invoke(this, new DownloadProgEventArgs(receivedBytes,fileLen));
				}
			}
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

		private byte[] ReadBytesAsync(Stream dataStream, int bytesToRead)
		{
			Byte[] steamContent = new Byte[bytesToRead];
			int numBytesProcessed = 0;
			while (numBytesProcessed < bytesToRead)
			{
				//ReadAsync() can return less then intSize therefore keep on looping until intSize is reached
				byte[] dataBuffer = new Byte[bytesToRead - numBytesProcessed];
				int bytesRead = dataStream.Read(dataBuffer, 0, bytesToRead - numBytesProcessed);
				Array.Copy(dataBuffer, 0, steamContent, numBytesProcessed, bytesRead);
				numBytesProcessed += bytesRead;
			}
			return steamContent;
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
                TimeSpan tripArrivalTime = GetTimeSpanFromString(trip.departure_time);
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

            return stopDetails;
		}

		protected StopModel GetStopFromStopRef(OpenArchive gtfsArchive, string stopRef)
        {
            if (stopsList == null)
            {
                using (Stream stopsStream = gtfsArchive.zip.GetEntry("google_transit/stops.csv").Open())
                {
                    stopsList = CsvSerializer.DeserializeFromStream<List<StopModel>>(stopsStream);
                }
            }

            StopModel stopObj = stopsList.FirstOrDefault(o => o.stop_code == stopRef);
            return stopObj;
        }

        protected List<TripTimesModel> GetTripsFromStopId(OpenArchive gtfsArchive, int stopId, DateTime timeStart, DateTime timeEnd)
        {
            List<TripTimesModel> trips;
            using (Stream timeStream = gtfsArchive.zip.GetEntry("google_transit/stop_times.csv").Open())
			{
                trips = CsvSerializer.DeserializeFromStream<List<TripTimesModel>>(timeStream);
			}
            List<TripTimesModel> tripsAtStop = trips.Where(o => o.stop_id == stopId).ToList();
            return tripsAtStop;
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
            TimeSpan time = new TimeSpan(int.Parse(timeParts[0]), int.Parse(timeParts[1]), int.Parse(timeParts[2]));
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

		/*
        public async Task<bool> IsTimeTableUptoDate()
        {
            if (this.useUncompressedGTFS == false)
            {
                ZipArchive archive = await this.GetZipFile(archiveFilePath);

                if (archive == null)
                {
                    return false;
                }
            }
            else
            {
                return await fileMgr.DoesFolderExist("./GTFSLoader");
            }

            return true;
        }
        */
	}
}
