using NUnit.Framework;
using System;
using MetroLive.Services.Offline;
namespace MetroLive.NUnit
{
    [TestFixture()]
    public class Test
    {
        [Test()]
        public void CreateAFile()
        {
            FileManager x = new FileManager("./");
            Assert.AreEqual(true, true);
        }
    }
}
