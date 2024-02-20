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
        private readonly string _connectionString;
        public DatabaseLogic()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["BackupConn"].ConnectionString;
        }
        public List<(string, string, int)> getCurrentSettings()
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "select * from dbo.Simplebackup";
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                List<(string, string, int)> SourceAndDestination = new List<(string, string, int)>();

                foreach (DataRow row in dt.Rows)
                {
                    string source = row["SourceDir"].ToString();
                    string destination = row["Destination"].ToString();
                    int id = (int)row["Id"];
                    var pair = Tuple.Create(source, destination, id);
                    SourceAndDestination.Add(pair.ToValueTuple());
                }
                return SourceAndDestination;

            }
        }
        
        public void UpdateSourceAndDestination([Optional] string newSource, [Optional] string newDestination)
        {
            string query = " ";

            if (string.IsNullOrEmpty(newSource) && !string.IsNullOrEmpty(newDestination))
            {
                return;
            }
            else if (string.IsNullOrEmpty(newDestination) && !string.IsNullOrEmpty(newSource))
            {
                return;
            }
            else if (string.IsNullOrEmpty(newSource) && string.IsNullOrEmpty(newDestination))
            {
                return;
            }
            else if (!string.IsNullOrEmpty(newDestination) && !string.IsNullOrEmpty(newDestination))
            {
                query = $"Insert into dbo.Simplebackup (SourceDir, Destination) values('{newSource}', '{newDestination}')"; ;
            }
        

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.ExecuteNonQuery();
            }

        }

        public void DeleteSourceAndDestination(int id)
        {
            string query = $"Delete from dbo.Simplebackup where id = '{id}'";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.ExecuteNonQuery();
            }

        }

        public List<BackupLogModel> GetBackupLog()
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {

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

                return backupLog;
            }
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