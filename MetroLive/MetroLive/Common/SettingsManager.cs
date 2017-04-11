using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLive.Common
{
    public class SettingsManager
    {
        public struct SettingsStruct
        {
            //the earliest bus trip to be fetched from the current time
            public TimeSpan GTFSStart;

            //the earilest bus trip to be fetched from the current time
            public TimeSpan SIRIStart;
            public TimeSpan SIRIPreviewInterval;

            //favourite bus stops
            public List<FavouriteStop> FavStops { get; set; }
        }

        public SettingsStruct Settings { get; set; }
        private FileManager fileMgr;
        private string SettingsfileTarget;

        //constructor
        public SettingsManager(FileManager mFileMgr)
        {
            this.fileMgr = mFileMgr;

            this.SettingsfileTarget = "Settings.json";

            InitalizeSettings();
        }

        private void InitalizeSettings()
        {
            SettingsStruct initSettings = new SettingsStruct();
            initSettings.GTFSStart = TimeSpan.FromMinutes(-15);
            initSettings.SIRIStart = TimeSpan.FromMinutes(0);
            initSettings.SIRIPreviewInterval = TimeSpan.FromMinutes(60);
            initSettings.FavStops = new List<FavouriteStop>();
            this.Settings = initSettings;
        }

        public async Task SaveSettingToDisk()
        {
            //convert class to json
            string settingsText = await JsonSeralizer.SerializeObject(Settings);

            //write string to file
            await fileMgr.OverwriteFileWithString(SettingsfileTarget, settingsText);
        }

        public async Task LoadSettingFromDisk()
        {
            string settingsText = await fileMgr.ReadStringFromFile(SettingsfileTarget);

            //check if no file was found
            if( settingsText == null )
            {
                //create a dummy file
                string defaultSettings = await JsonSeralizer.SerializeObject(Settings);
                await fileMgr.OverwriteFileWithString(SettingsfileTarget, defaultSettings);
                return;
            }

            this.Settings = await JsonSeralizer.DeserializeObject<SettingsStruct>(settingsText);
        }
    }
}
