using System;
using System.Collections.Generic;
using System.Text;

namespace Travel.Domain.Entities
{
    public class TourList
    {
        public int Id { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string About { get; set; }

        public IList<TourPackage> TourPackages { get; set; }

        public TourList()
        {
            TourPackages = new List<TourPackage>();
        }
    }
}
