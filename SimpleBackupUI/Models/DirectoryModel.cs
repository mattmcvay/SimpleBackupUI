using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SimpleBackupUI.Models
{
    public class DirectoryModel
    {
        public int Id { get; set; }
        public string Source { get; set; }

        [Display(Name = "Backup Location")]
        public string Destination { get; set; }

       

    }
}