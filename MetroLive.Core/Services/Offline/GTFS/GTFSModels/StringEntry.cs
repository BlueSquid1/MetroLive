using System;
namespace MetroLive.Services.Offline.GTFS.GTFSModels
{
    public class StringEntry : IComparable<StringEntry>
    {
        public string Entry { get; set; }

        //constructor
        public StringEntry(string mEntry)
        {
            this.Entry = mEntry;
        }

        public int CompareTo(StringEntry other)
        {
            if(other == null)
            {
                return 1;
            }

            int otherValue = int.Parse(other.Entry.Split(',')[0]);
            int curValue = int.Parse(this.Entry.Split(',')[0]);

            if (curValue > otherValue)
            {
                return 1;
            }
            else if (curValue < otherValue)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
