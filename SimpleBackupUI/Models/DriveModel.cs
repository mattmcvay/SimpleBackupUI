using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SimpleBackupUI.Models
{
    public class DriveModel
    {     
        public string Name { get; set; }

        [Display(Name = "Drive Type")]
        public string DriveType { get; set; }

        [Display(Name = "Drive Format")]
        public string DriveFormat { get; set; }

        [Display(Name = "Available Free Space")]
        public string AvailableFreeSpace { get; set; }

        [Display(Name = "Total Size")]
        public string TotalSize { get; set; }

        [Display(Name = "Volume Label")]
        public string VolumeLabel { get; set; }

    }
}