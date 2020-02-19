using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;
using Capstone.DAL;
using Capstone.Models;

namespace Capstone.Tests
{
    [TestClass]
    class SiteSqlDAOTests
    {
        private TransactionScope transaction = null;
        private string connectionString = "Server=.\\SQLEXPRESS;Database=npcampground;Trusted_Connection=True;";
        // TODO: private int newCampgroundId; ID NEEDED HERE
        // TODO: private int newParkId; ID NEEDED HERE
        // TODO: private int newReservationId; ID NEEDED HERE
        // TODO: private int newSiteId; ID NEEDED HERE

        [TestInitialize]
        public void SetupDatabase()
        {
            // Start a transaction, so we can roll back when we are finished with this test
            transaction = new TransactionScope();

            // Open Setup.Sql and read in the script to be executed
            string setupSQL;
            using (StreamReader rdr = new StreamReader("Setup.sql"))
            {
                setupSQL = rdr.ReadToEnd();
            }

            // Connect to the database and execute the script
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(setupSQL, conn);
                SqlDataReader rdr = cmd.ExecuteReader();

                // TODO: NEED TO UPDATE THIS PRIOR TO TESTING
                //if (rdr.Read())
                //{
                //    newDepartmentId = Convert.ToInt32(rdr["newDepartmentId"]); // TODO: UPDATE TO PROPER ID
                //}

                //rdr.NextResult();

                //if (rdr.Read())
                //{
                //    newEmployeeId = Convert.ToInt32(rdr["newEmployeeId"]); // TODO: UPDATE TO PROPER ID
                //}

                //rdr.NextResult();

                //if (rdr.Read())
                //{
                //    newProjectId = Convert.ToInt32(rdr["newProjectId"]); // TODO: UPDATE TO PROPER ID
                //}

                //rdr.NextResult();

                //if (rdr.Read())
                //{
                //    newProjectId = Convert.ToInt32(rdr["newProjectId"]); // TODO: UPDATE TO PROPER ID
                //}
            }
        }

        [TestCleanup]
        public void CleanupDatabase()
        {
            // Rollback the transaction to get our good data back
            transaction.Dispose();
        }


        [TestMethod]
        public void TestMethod1()
        {

        }
    }
}
