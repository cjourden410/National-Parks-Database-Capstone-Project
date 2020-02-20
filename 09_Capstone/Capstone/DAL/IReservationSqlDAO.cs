using Capstone.Models;
using System;
using System.Collections.Generic;

namespace Capstone.DAL
{
    public interface IReservationSqlDAO
    {
        int CreateReservation(Reservation newReservation);
        IList<Reservation> Search(int siteId, DateTime fromDate, DateTime endDate);
    }
}