using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MetroLive.Common;

namespace MetroLive.Droid
{
    public class FileManagerDroid : FileManager
    {
        public FileManagerDroid() : base(Environment.GetFolderPath(Environment.SpecialFolder.Personal))
        {
        }
    }
}