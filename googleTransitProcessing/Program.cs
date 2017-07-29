using System;
using System.IO;

namespace googleTransitProcessing
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            if(args.Length <= 1)
            {
                Console.WriteLine("please: 1. pass the csv file to proccess 2. pass the name of the output file");
            }

            string csvStopPath = args[0];
            string outputCsv = args[1];

            FileInfo stopTimesFile = new FileInfo(csvStopPath);

            using(FileStream outputStream = File.Open(outputCsv, FileMode.Create))
            using(FileStream stopStream = stopTimesFile.Open( FileMode.Open, FileAccess.Read))
            {
                StreamWriter stopWriter = new StreamWriter(outputStream);
                StreamReader stopReader = new StreamReader(stopStream);

                //header line
                int lineCount = 0;
                stopReader.ReadLine();

                //find the number of entries
                int totalCount = 0;
                while (stopReader.EndOfStream == false)
				{
                    stopReader.ReadLine();
                    totalCount++;
				}
                //start reading for begining again
                stopReader.BaseStream.Seek(0, SeekOrigin.Begin);
                string header = stopReader.ReadLine();
                header = header + ',' + totalCount.ToString();
                stopWriter.WriteLine(header);
                while (stopReader.EndOfStream == false)
                {
                    string line = stopReader.ReadLine();
                    lineCount++;

                    string[] entry = line.Split(',');
                    string stop_id = entry[0];

                    stop_id = stop_id.PadLeft(7, '0');

                    string departure_time = entry[1];
                    string trip_id = entry[2];

                    trip_id = trip_id.PadLeft(7, '0');

                    string[] timeParts = departure_time.Split(':');
                    if( timeParts.Length < 3 )
                    {
                        Console.WriteLine("line:" + lineCount.ToString() + " has an invalid length.");
                        continue;
                    }

                    string hour = timeParts[0];
                    string minute = timeParts[1];
                    string second = timeParts[2];

                    //make sure the times are fix length
                    hour = hour.PadLeft(2,'0');
                    minute = minute.PadLeft(2);
                    second = second.PadLeft(2);

                    departure_time = hour + ":" + minute + ":" + second;

                    string outputEntry = stop_id + "," + departure_time + "," + trip_id;

                    //make sure output has the right length
                    if(outputEntry.Length != 24)
                    {
                        Console.WriteLine("line:" + lineCount.ToString() + " has an invalid length of: " + outputEntry.Length.ToString());
						continue;
                    }

                    stopWriter.WriteLine(outputEntry);
                }
                stopWriter.Flush();
            }

            Console.WriteLine("Done");

        }
    }
}
