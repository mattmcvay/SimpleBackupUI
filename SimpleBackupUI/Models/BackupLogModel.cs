using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SimpleBackupUI.Models
{
    public class BackupLogModel
    {
        [Display(Name = "File Name")]
        public string  FileName { get; set; }

        [Display(Name = "Backup Path")]
        public string BackupPath { get; set; }

        [Display(Name = "Backup Date")]
        public DateTime BackupDate { get; set; }
    }
}