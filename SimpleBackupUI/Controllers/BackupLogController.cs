using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleBackupUI.Logic;

namespace SimpleBackupUI.Controllers
{
    public class BackupLogController : Controller
    {
        // GET: BackupLog
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BackupLog()
        {
            DatabaseLogic d = new DatabaseLogic();

            var bList = d.GetBackupLog();
       
            return View(bList);
        }
    }
}