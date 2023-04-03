using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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

        public void UpdateSource(string newSource)
        {
            var ConString = ConfigurationManager.ConnectionStrings["BackupConn"].ConnectionString;
            SqlConnection con = new SqlConnection(ConString);
            string query = $"update Simplebackup set SourceDir = '{newSource}'";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.ExecuteNonQuery();
        }

        public void UpdateDestination(string newDestination)
        {
            var ConString = ConfigurationManager.ConnectionStrings["BackupConn"].ConnectionString;
            SqlConnection con = new SqlConnection(ConString);
            string query = $"update Simplebackup set Destination = '{newDestination}'";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.ExecuteNonQuery();

        }

    }
}