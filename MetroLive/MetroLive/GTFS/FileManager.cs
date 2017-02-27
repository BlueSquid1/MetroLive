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

        //constructor
        public FileManager(string mFilePath = "./")
        {
            this.fileSystem = FileSystem.Current;
            this.DefaultFilePath = mFilePath;
            this.gtfsFileName = "google_transit.zip";
        }

        public async Task WriteToFileAsync(Stream mFileStream)
        {
            //create a folder to store the file
            IFolder root = await fileSystem.GetFolderFromPathAsync("./");
            if (await root.CheckExistsAsync(DefaultFilePath) == ExistenceCheckResult.NotFound)
            {
                //create folder
                await root.CreateFolderAsync(DefaultFilePath, CreationCollisionOption.FailIfExists);
            }

            IFolder tempFolder = await fileSystem.GetFolderFromPathAsync(DefaultFilePath);
            IFile newFile = await tempFolder.CreateFileAsync(gtfsFileName, CreationCollisionOption.ReplaceExisting);
            Stream fileStream = await newFile.OpenAsync(FileAccess.ReadAndWrite);

            await mFileStream.CopyToAsync(fileStream);
        }

        public async Task ReadFileAsync(string fileName)
        {
            ZipArchive zipArchive = new ZipArchive(GTFSCompressed);
            foreach (ZipArchiveEntry archive in zipArchive.Entries)
            {
                string name = archive.FullName;
            }

        }

    }
}
