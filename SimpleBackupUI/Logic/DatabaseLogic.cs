using SimpleBackupUI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;


namespace SimpleBackupUI.Logic
{
    public class DatabaseLogic
    {
        public class GetConnection
        {
            public static Tuple<string, string> getCurrentSettings()
            {
                var ConString = ConfigurationManager.ConnectionStrings["BackupConn"].ConnectionString;
                SqlConnection con = new SqlConnection(ConString);
                string query = "select * from dbo.Simplebackup";
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                string source = dt.Rows[0]["SourceDir"].ToString();
                string destination = dt.Rows[0]["Destination"].ToString();
                con.Close();
                return Tuple.Create(source, destination);
            }
        }
        public void UpdateSourceAndDestination([Optional] string newSource, [Optional] string newDestination)
        {
            var ConString = ConfigurationManager.ConnectionStrings["BackupConn"].ConnectionString;
            SqlConnection con = new SqlConnection(ConString);

            string query = " ";

            if (string.IsNullOrEmpty(newSource) && !string.IsNullOrEmpty(newDestination))
            {
                query = $"update Simplebackup set Destination = '{newDestination}'";
            }
            else if (string.IsNullOrEmpty(newDestination) && !string.IsNullOrEmpty(newSource))
            {
                query = $"update Simplebackup set SourceDir = '{newSource}'";
            }
            else if (!string.IsNullOrEmpty(newSource) && !string.IsNullOrEmpty(newDestination))
            {
                query = $"update Simplebackup set Destination = '{newDestination}', SourceDir = '{newSource}'";
            }
            else if (string.IsNullOrEmpty(newDestination) && string.IsNullOrEmpty(newDestination))
            {
                return;
            }

            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.ExecuteNonQuery();
        }

        public List<BackupLogModel> GetBackupLog()
        {
            var ConString = ConfigurationManager.ConnectionStrings["BackupConn"].ConnectionString;
            SqlConnection con = new SqlConnection(ConString);
            string query = "select * from dbo.BackupLog order by BackupDate desc ";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            List<BackupLogModel> backupLog = new List<BackupLogModel>();

            foreach (DataRow row in dt.Rows)
            {
                backupLog.Add(new BackupLogModel
                {
                    FileName = row["FileName"].ToString(),
                    BackupPath = row["BackupPath"].ToString(),
                    BackupDate = (DateTime)row["BackupDate"]

                }); ;   
            }          
            con.Close();

            return backupLog;
        }

        public void SetServiceStatusTo(string status)
        {
            string location = @"C:\Users\Matt\myServices";

            string showTestCommand = " ";

            if (status.ToLower() == "on")
            {
                showTestCommand = "/C Simplebackup.exe start";

            }
            else if(status.ToLower() == "off")
            {
                showTestCommand = "/C Simplebackup.exe stop";
            }

            ProcessStartInfo cmdProcessInfo = new ProcessStartInfo("cmd.exe");

            cmdProcessInfo.WorkingDirectory = location;
            cmdProcessInfo.Arguments = showTestCommand;
            cmdProcessInfo.UseShellExecute = false;
            cmdProcessInfo.CreateNoWindow = true;
            cmdProcessInfo.Verb = "runas";

            Process cmdStart = new Process();
            cmdStart.StartInfo = cmdProcessInfo;

            cmdStart.Start();
        }
    }
}