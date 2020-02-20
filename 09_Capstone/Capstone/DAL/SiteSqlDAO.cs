using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class SiteSqlDAO : ISiteSqlDAO
    {
        private string connectionString;

        public SiteSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public IList<Site> GetAllSites(int site_id) // TODO: Reference Assign EmployeeToProject ProjectCLI
        {
            // Declare the result variable
            List<Site> list = new List<Site>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Open the connection
                    conn.Open();

                    // Create the command for sql statement
                    string sql = "Select * From site Where site_id = @site_id";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@site_id", site_id);

                    // Execute the query and get the result set in a reader
                    SqlDataReader rdr = cmd.ExecuteReader();

                    // For each row, create a new department and add it to the list
                    while (rdr.Read())
                    {
                        Site site = RowToObject(rdr);

                        list.Add(site);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occured reading campgrounds by park.");
                Console.WriteLine(ex.Message);
                throw;
            }

            return list;
        }

        private static Site RowToObject(SqlDataReader rdr)
        {
            Site site = new Site();
            site.SiteId = Convert.ToInt32(rdr["site_id"]);
            site.CampgroundId = Convert.ToInt32(rdr["campground_id"]);
            site.SiteNumber = Convert.ToInt32(rdr["site_number"]);
            site.MaxCapacity = Convert.ToInt32(rdr["max_occupancy"]);
            site.Accessible = Convert.ToBoolean(rdr["accessible"]);
            site.MaxRVLength = Convert.ToInt32(rdr["max_rv_length"]);
            site.Utilities = Convert.ToBoolean(rdr["utilities"]);
            return site;
        }
    }
}
