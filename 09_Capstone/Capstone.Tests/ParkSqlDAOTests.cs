﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class ParkSqlDAOTests
    {
        private TransactionScope transaction = null;
        private string connectionString = "Server=.\\SQLEXPRESS;Database=npcampground;Trusted_Connection=True;";
        private int newCampgroundId;
        private int newParkId;
        private int newReservationId;
        private int newSiteId;


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

                if (rdr.Read())
                {
                    newParkId = Convert.ToInt32(rdr["newParkId"]);
                }

                rdr.NextResult();

                if (rdr.Read())
                {
                    newCampgroundId = Convert.ToInt32(rdr["newCampgroundId"]);
                }

                rdr.NextResult();

                if (rdr.Read())
                {
                    newSiteId = Convert.ToInt32(rdr["newSiteId"]);
                }

                rdr.NextResult();

                if (rdr.Read())
                {
                    newReservationId = Convert.ToInt32(rdr["newReservationId"]);
                }
            }
        }

        [TestCleanup]
        public void CleanupDatabase()
        {
            // Rollback the transaction to get our good data back
            transaction.Dispose();
        }


        [TestMethod]
        public void GetAllParksTest()
        {
            ParkSqlDAO dao = new ParkSqlDAO(connectionString);

            IList<Park> parks = dao.GetParks();

            Assert.AreEqual(1, parks.Count);
        }
    }
}
