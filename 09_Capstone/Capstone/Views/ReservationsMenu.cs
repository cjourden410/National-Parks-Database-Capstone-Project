using Capstone.DAL;
using Capstone.Models;
using System;
using System.Globalization;
using System.Collections.Generic;
using Capstone.Extensions;

namespace Capstone.Views
{
    public class ReservationsMenu : MainMenu
    {
        // Store any private variables, including DAOs here....
        private Park selectedPark;
        private string menuNumber;

        /// <summary>
        /// Constructor adds items to the top-level menu
        /// </summary>
        public ReservationsMenu(Park selectedPark, ICampgroundSqlDAO campgroundDAO, IParkSqlDAO parkDAO, IReservationSqlDAO reservationDAO, ISiteSqlDAO siteDAO) :
            base(campgroundDAO, parkDAO, reservationDAO, siteDAO)
        {
            this.selectedPark = selectedPark;
        }

        protected override void SetMenuOptions()
        {
            //this.menuOptions.Add("1", "Search for Available Reservation");
            //this.menuOptions.Add("2", "Return to Previous Screen");
            //this.quitKey = "2";
            Console.Clear();
            BeforeDisplayMenu();
            SearchForAvailableReservations();
            Console.WriteLine();
        }

        /// <summary>
        /// The override of ExecuteSelection handles whatever selection was made by the user.
        /// This is where any business logic is executed.
        /// </summary>
        /// <param name="choice">"Key" of the user's menu selection</param>
        /// <returns></returns>
        //protected override bool ExecuteSelection(string choice)
        //{
        //    switch (choice)
        //    {
        //        case "1": // Go to new SubMenu for Reservations
        //            //WriteError("Not yet implemented");
        //            //ReservationsMenu rm = new ReservationsMenu(selectedPark, campgroundDAO, parkDAO, reservationDAO, siteDAO);
        //            //rm.Run();
        //            //Pause("");
        //            return true;
        //    }
        //    return true;
        //}

        private void SearchForAvailableReservations()
        {
            IList<Campground> campgrounds = campgroundDAO.GetCampgroundsByParkId(selectedPark.ParkId);
            string pickCampground = CLIMenu.GetString("Which campground would you like (enter 0 to cancel)?");
            if (pickCampground == "0")
            {
                CampgroundsMenu cm = new CampgroundsMenu(selectedPark, campgroundDAO, parkDAO, reservationDAO, siteDAO);
                cm.Run();
                Pause("");
            }
            else if ((Convert.ToInt32(pickCampground)) > campgrounds.Count)
            {
                Console.WriteLine("Please choose a valid campground");
                Pause("");
                ReservationsMenu rm = new ReservationsMenu(selectedPark, campgroundDAO, parkDAO, reservationDAO, siteDAO);
                rm.Run();
                Pause("");
            }
            else
            {
                
                string arrivalDate = CLIMenu.GetDateTime("What is the arrival date?");
                string departureDate = CLIMenu.GetDateTime("What is the departure date?");
                IList<Site> sites = siteDAO.ViewAvailableReservations(pickCampground, arrivalDate, departureDate);
                if (sites.Count > 0)
                {
                    decimal campgroundCost = campgroundDAO.GetCampgroundCost(pickCampground);
                    Console.WriteLine("Results Matching Your Search Criteria");
                    Console.WriteLine($"{"Site No.", -10} {"Max Occupancy", 10} {"Accessible?", 14} {"Max RV Length", 18} {"Utility", 11} {"Cost", 6}");

                    foreach (Site site in sites)
                    {
                        Console.WriteLine($"{site.SiteId, -10} {site.MaxOccupancy, -16} {site.Accessible.ToYesNo(), -16} {site.MaxRVLength.ToLengthNA(), -17} {site.Utilities.ToYesNA(), -9} {campgroundCost.ToString("C"), 0}");
                        
                    }
                    Console.WriteLine();
                    string siteToReserve = CLIMenu.GetString("Which site should be reserved (enter 0 to cancel)");
                    if (siteToReserve == "0")
                    {
                        ReservationsMenu rm = new ReservationsMenu(selectedPark, campgroundDAO, parkDAO, reservationDAO, siteDAO);
                        rm.Run();
                        Pause("");
                    }
                    //else if ((Convert.ToInt32(siteToReserve)) > sites.Count)
                    //{
                    //    Console.WriteLine("Please choose a valid site");
                    //    Pause("");
                    //}
                    bool siteExistsInList = false;

                    foreach (Site site in sites)
                    {
                        if (site.SiteId.ToString() == siteToReserve)
                        {
                            siteExistsInList = true;
                        }
                    }

                    if (!siteExistsInList)
                    {
                        Console.WriteLine("We're sorry, that Site ID is not in our list. Please try again.");
                        return;
                    }

                    string name = CLIMenu.GetString("What name should the reservation be made under?");
                    MakeReservation(siteToReserve, name, arrivalDate, departureDate);
                }
                else
                {
                    Console.WriteLine("We're sorry, there are no campsites available. Please try again with different dates.");
                    return;
                }
         
            }
        }

        private void MakeReservation(string siteToReserve, string name, string arrivalDate, string departureDate)
        {
            int reservationId = reservationDAO.CreateReservation(siteToReserve, name, arrivalDate, departureDate);
            if (reservationId == 0)
            {
                Console.WriteLine("We apologize, but unfortunately there was an error during the reservation process.");
                Pause("");
            }
            else
            {
                Console.WriteLine($"The reservation has been made and the confirmation id is {reservationId}");
                Pause("");
            }
        }


        protected override void BeforeDisplayMenu()
        {
            PrintHeader();
            SetColor(ConsoleColor.Red);
            IList<Campground> campgrounds = campgroundDAO.GetCampgroundsByParkId(selectedPark.ParkId);
            int sum = 0;
            Console.WriteLine($"      {"Name",-10} {"Open",11} {"Close",11} {"Daily Fee",16}");
            foreach (Campground campground in campgrounds)
            {
                sum += 1;
                menuNumber = Convert.ToString(sum);
                string openFromMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(campground.OpenFromMonth);
                string openToMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(campground.OpenToMonth);
                Console.WriteLine($"{"#"}{menuNumber}    {campground.Name,-17} {openFromMonth,-10} {openToMonth,-12} {campground.DailyFee.ToString("C"),0}");

            }
            ResetColor();
        }

        protected override void AfterDisplayMenu()
        {
            base.AfterDisplayMenu();
            SetColor(ConsoleColor.Green);
            //Console.WriteLine("Display some data here, AFTER the sub-menu is shown....");
            ResetColor();
        }

        private void PrintHeader()
        {
            SetColor(ConsoleColor.Magenta);
            Console.WriteLine(Figgle.FiggleFonts.Standard.Render("Reservations"));
            ResetColor();
        }
    }
}