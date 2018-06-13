using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueTracker
{
    [Authorize]
    [Route("[controller]")]
    public class IssueController : Controller
    {
        public IConfiguration Configuration { get; set; }
        public IssueController(IConfiguration config)
        {
            Configuration = config;
        }

        [AllowAnonymous]
        [HttpPost("gettypes")]
        public IEnumerable<IssueType> gettypes()
        {
            var connectionString = Configuration.GetConnectionString("IssueTypes");

            DataSet ds = new DataSet();
            SqlDataAdapter read = new SqlDataAdapter("SELECT IssueTypeID, IssueName, IssueDescription FROM IssueType ORDER BY IssueDescription", connectionString);
            read.Fill(ds);
            var type = new List<IssueType>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                IssueType listItem = new IssueType
                {
                    IssueTypeID = Int32.Parse(row[0].ToString()),
                    IssueName = row[1].ToString(),
                    IssueDescription = row[2].ToString()
                };

                type.Add(listItem);
            }
            return type;
        }

        [AllowAnonymous]
        [HttpPost("addnewissue")]
        public object addnewissue([FromBody]Issue issue)
        {
            SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("IssueTypes")); conn.Open();
            string sql = "spNewIssue";

            using (SqlCommand insert = new SqlCommand(sql, conn))
            {
                insert.CommandType = CommandType.StoredProcedure;
                insert.Parameters.AddWithValue("@DateAdded", issue.DateAdded);
                insert.Parameters.AddWithValue("@IssueType", issue.IssueTypeID);
                insert.Parameters.AddWithValue("@IssueText", issue.IssueText);

                if (issue.TextRecieved == null || issue.TextWanted == null)
                {
                    insert.Parameters.AddWithValue("@TextGot", DBNull.Value);
                    insert.Parameters.AddWithValue("@TextWanted", DBNull.Value);
                }
                else
                {
                    insert.Parameters.AddWithValue("@TextGot", issue.TextRecieved);
                    insert.Parameters.AddWithValue("@TextWanted", issue.TextWanted);
                }
                
                var newIssue = insert.ExecuteScalar();

                return newIssue;
            }
        }

        [AllowAnonymous]
        [HttpPost("gettickets")]
        public IEnumerable<Issue> gettickets()
        {
            var connectionString = Configuration.GetConnectionString("IssueTypes");

            DataSet ds = new DataSet();
            SqlDataAdapter read = new SqlDataAdapter("exec spGetAllTickets", connectionString);
            read.Fill(ds);
            var tickets = new List<Issue>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                Issue listItem = new Issue
                {
                    IssueID = Int32.Parse(row[0].ToString()),
                    DateAdded = DateTime.Parse(row[1].ToString()),
                    Active = Boolean.Parse(row[2].ToString()),
                    Type = IssueType.Convert(row[3].ToString()),
                    IssueText = row[4].ToString(),
                    TextRecieved = row[5].ToString(),
                    TextWanted = row[6].ToString()
                };

                tickets.Add(listItem);
            };

            return tickets;
        }

        [AllowAnonymous]
        [HttpPost("closeticket")]
        public void closeticket(string issueID)
        {
            var connectionString = Configuration.GetConnectionString("IssueTypes");

            SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("IssueTypes")); conn.Open();
            string sql = "spCloseTicket";

            using (SqlCommand insert = new SqlCommand(sql, conn))
            {
                insert.CommandType = CommandType.StoredProcedure;
                insert.Parameters.AddWithValue("@IssueID", issueID);

                insert.ExecuteNonQuery();
            }
        }
    }
}

