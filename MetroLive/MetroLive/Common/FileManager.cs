using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLive.Common
{
    public abstract class FileManager
    {
        //default file path
        protected string filePathRoot;

        protected IFileSystem fileSystem;
        //protected string gtfsFileName = "google_transit.zip";

        //constructor
        public FileManager(string mFilePath)
        {
            this.fileSystem = FileSystem.Current;
            this.filePathRoot = mFilePath;
        }

        //will overwrite the existing file if it exists
        public async Task OverwriteFileWithString(string targetFile, string text)
        {
            string completeFilePath = filePathRoot + targetFile;

            IFile activeFile = await fileSystem.GetFileFromPathAsync(completeFilePath);
            //check if file actually exists
            if (activeFile == null)
            {
                //create file
                IFolder root = await fileSystem.GetFolderFromPathAsync("./");
                activeFile = await root.CreateFileAsync(completeFilePath, CreationCollisionOption.FailIfExists);
            }

            //write content
            await activeFile.WriteAllTextAsync(text);
        }

        public async Task<string> ReadStringFromFile(string targetFile)
        {
            string completeFilePath = filePathRoot + targetFile;

            IFile file = await fileSystem.GetFileFromPathAsync(completeFilePath);

            if(file == null)
            {
                return null;
            }

            return await file.ReadAllTextAsync();
        }

        public async Task WriteStreamToArchive(string targetFile, Stream mFileStream)
        {
            string completeFilePath = filePathRoot + targetFile;
            IFile archiveFile = await fileSystem.GetFileFromPathAsync(completeFilePath);

            //check if file actually exists
            if(archiveFile == null)
            {
                //create file
                IFolder root = await fileSystem.GetFolderFromPathAsync("./");
                archiveFile = await root.CreateFileAsync(completeFilePath, CreationCollisionOption.FailIfExists);
            }

            //dump archive into file
            Stream fileStream = await archiveFile.OpenAsync(FileAccess.ReadAndWrite);
            await mFileStream.CopyToAsync(fileStream);
        }

        public async Task<ZipArchive> GetFileFromArchive(string archiveName)
        {
            string completeFilePath = filePathRoot + archiveName;

            IFile file = await fileSystem.GetFileFromPathAsync(completeFilePath);

            if( file == null )
            {
                return null;
            }

            Stream zipStream = await file.OpenAsync(FileAccess.Read);

            ZipArchive catchedArchive = new ZipArchive(zipStream);

            //ZipArchiveEntry compressedFile = catchedArchive.GetEntry(fileName);
            return catchedArchive;

        }

        /*
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

            string timetablePath = FilePathRoot + gtfsFileName;

            IFile file = await fileSystem.GetFileFromPathAsync(timetablePath);
            
            Stream timeTableStream = await file.OpenAsync(FileAccess.Read);

            this.CatchedArchive = new ZipArchive(timeTableStream);

            ZipArchiveEntry compressedFile = CatchedArchive.GetEntry(fileName);
            return compressedFile;
        }
        */
    }
}
