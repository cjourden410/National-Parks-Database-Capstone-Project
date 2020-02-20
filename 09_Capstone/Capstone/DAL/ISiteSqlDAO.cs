using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAL
{
    public interface ISiteSqlDAO
    {
        IList<Site> GetAllSites(int site_id);
    }
}