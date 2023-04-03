using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleBackupUI.Models
{
    public class DriveModel
    {
        public string Name { get; set; }
        public string DriveType { get; set; }
        public string DriveFormat { get; set; }
        public string AvailableFreeSpace { get; set; }
        public string TotalSize { get; set; }
        public string VolumeLabel { get; set; }

    }
}