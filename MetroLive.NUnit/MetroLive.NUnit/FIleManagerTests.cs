using NUnit.Framework;
using System;
using MetroLive.Services.Offline;
using System.Text;
using System.IO;

namespace MetroLive.NUnit
{
    [TestFixture()]
    public class Test
    {
		//enter the path to /MetroLive/MetroLive.NUnit/MetroLive.NUnit/
		private string rootPath { get; } = @"/Users/clintonpage/Desktop/MyGit/MetroLive/MetroLive.NUnit/MetroLive.NUnit/";

        [Test()]
        public async void CreateAFileAtRoot()
        {
            FileManager fileMgr = new FileManager(rootPath);

            //write file
            string fileName = "Test.txt";
            string fileMessage = "This is a test to see if file write works. !@#$";

            byte[] fileData = Encoding.UTF8.GetBytes(fileMessage);
            await fileMgr.WriteBytesToFileAsync(fileName, fileData);

            //read file
            FileInfo fileStream = fileMgr.GetFile(fileName);
            Assert.AreEqual(true, true);
        }
    }
}
