﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Site
    {
        public int SiteId { get; set; }
        public int CampgroundId { get; set; }
        public int SiteNumber { get; set; }
        public int MaxCapacity { get; set; }
        public bool Accessible { get; set; }
        public int MaxRVLength { get; set; }
        public bool Utilities { get; set; }
    }
}
