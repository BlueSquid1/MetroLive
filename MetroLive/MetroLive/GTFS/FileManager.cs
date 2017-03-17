using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLive.GTFS
{
    public abstract class FileManager
    {
        protected IFileSystem fileSystem;
        protected string DefaultFilePath;
        protected string gtfsFileName;
        protected ZipArchive CatchedArchive;

        //constructor
        public FileManager(string mFilePath = "./")
        {
            this.fileSystem = FileSystem.Current;
            this.DefaultFilePath = mFilePath;
            this.gtfsFileName = "google_transit.zip";


        }

        public async Task WriteArchiveToDiskAsync(Stream mFileStream)
        {
            bool archiveExists = await DoesArchiveExist();

            IFile archiveFile;

            if (!archiveExists)
            {
                //create a folder to store the file
                IFolder root = await fileSystem.GetFolderFromPathAsync("./");
                if (await root.CheckExistsAsync(DefaultFilePath) == ExistenceCheckResult.NotFound)
                {
                    //create folder
                    await root.CreateFolderAsync(DefaultFilePath, CreationCollisionOption.FailIfExists);
                }

                IFolder tempFolder = await fileSystem.GetFolderFromPathAsync(DefaultFilePath);
                archiveFile = await tempFolder.CreateFileAsync(gtfsFileName, CreationCollisionOption.ReplaceExisting);
            }
            else
            {
                //get file from disk
                archiveFile = await fileSystem.GetFileFromPathAsync(DefaultFilePath + gtfsFileName);
            }

            Stream fileStream = await archiveFile.OpenAsync(FileAccess.ReadAndWrite);

            await mFileStream.CopyToAsync(fileStream);
        }

        public async Task<bool> DoesArchiveExist()
        {
            string timetablePath = DefaultFilePath + gtfsFileName;

            IFolder root = await fileSystem.GetFolderFromPathAsync("./");
            if (await root.CheckExistsAsync(timetablePath) == ExistenceCheckResult.NotFound)
            {
                return false;
            }
            return true;
        }

        public async Task<ZipArchiveEntry> GetArchiveEntryAsync(string fileName)
        {
            //check if zip archive is already in RAM
            if(CatchedArchive != null)
            {
                ZipArchiveEntry cachedEntry = CatchedArchive.GetEntry(fileName);
                return cachedEntry;
            }

            //not in RAM check if its on Disk
            bool archiveExist = await DoesArchiveExist();

            if(!archiveExist)
            {
                //no archive to explore
                return null;
            }

            string timetablePath = DefaultFilePath + gtfsFileName;

            IFile file = await fileSystem.GetFileFromPathAsync(timetablePath);
            
            Stream timeTableStream = await file.OpenAsync(FileAccess.Read);

            this.CatchedArchive = new ZipArchive(timeTableStream);

            ZipArchiveEntry compressedFile = CatchedArchive.GetEntry(fileName);
            return compressedFile;
        }
        

    }
}
