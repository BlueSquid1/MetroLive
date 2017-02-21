using System;
using GTFS;
using GTFS.Exceptions;
using GTFS.IO;
namespace MetroLive
{
	public class BusStopMgr
	{
		private GTFSReader<GTFSFeed> Reader;

		//constructor
		public BusStopMgr(int busStopId)
		{
			Reader = new GTFSReader<GTFSFeed>();
			//GTFSDirectorySource x = new GTFSDirectorySource();
		}
	}
}
