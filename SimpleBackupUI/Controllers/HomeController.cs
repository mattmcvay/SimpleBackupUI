using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleBackupUI.Models;
using SimpleBackupUI.Logic;
using System.Threading;
using System.Diagnostics;

namespace SimpleBackupUI.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {        
            return View();
        }
        public ActionResult LoadDirectories()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            List<DriveModel> driveList = new List<DriveModel>();

            foreach (DriveInfo d in allDrives)
            {
                if (d.IsReady == true)
                {
                    var convertedFreeSpace = FormatBytesToHumanReadable(d.AvailableFreeSpace);
                    var convertedTotalSize = FormatBytesToHumanReadable(d.TotalSize);

                    driveList.Add(new DriveModel
                    {
                        Name = d.Name,
                        DriveType = d.DriveType.ToString(),
                        DriveFormat = d.DriveFormat,
                        AvailableFreeSpace = convertedFreeSpace.ToString(),
                        TotalSize = convertedTotalSize.ToString(),
                        VolumeLabel = d.VolumeLabel.ToString()
                    }); 
                }

            }

            return View(driveList); 
        }

        public ActionResult LoadCurrentSourceAndDestination()
        {
            Tuple<string, string> con = DatabaseLogic.GetConnection.getCurrentSettings();
            string sourceDir = con.Item1;
            string destination = con.Item2;

            List<DirectoryModel> directories = new List<DirectoryModel>();

            directories.Add(new DirectoryModel
            {
                Source = sourceDir,
                Destination = destination
            });

            return View(directories);
        }

        public ActionResult UpdateSourceAndDestination(DirectoryModel directory)
        {
            var newSource = directory.Source;
            var newDestination = directory.Destination;

            DatabaseLogic logic = new DatabaseLogic();

            logic.UpdateSourceAndDestination(newSource, newDestination);

            Thread.Sleep(1000);
            //return View("/Index.cshtml");
            return RedirectToAction("Index", "Home");
        }

        private static string FormatBytesToHumanReadable(long bytes)
        {
            if (bytes > 1073741824)
                return Math.Ceiling(bytes / 1073741824M).ToString("#,### GB");
            else if (bytes > 1048576)
                return Math.Ceiling(bytes / 1048576M).ToString("#,### MB");
            else if (bytes >= 1)
                return Math.Ceiling(bytes / 1024M).ToString("#,### KB");
            else if (bytes < 0)
                return "";
            else
                return bytes.ToString("#,### B");
        }

        public ActionResult UpdateServceStatus(ServiceStatusModel status)
        {
            DatabaseLogic updateService = new DatabaseLogic();
            
            updateService.SetServiceStatusTo(status.ServiceStatus.ToString());

            return RedirectToAction("Index", "Home");
        }

        public ActionResult LoadServiceStatus()
        {
            // new code, may not use.
            string serviceName = "SimpleBackupService"; // the name of the service you want to check
            string arguments = "query " + serviceName;

            Process process = new Process();
            process.StartInfo.FileName = "sc.exe";
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            bool isServiceRunning = output.Contains("STATE              : 4  RUNNING");

            string status = string.Empty;

            if(isServiceRunning == true)
            {
                status = "On";
            }
            else if(isServiceRunning == false)
            {
                status = "Off";
            }

            IsServiceRunningModel model = new IsServiceRunningModel()
            {
               IsServiceRunning = isServiceRunning,
               Status = status
            };

            return View(model);
        }
    }
}