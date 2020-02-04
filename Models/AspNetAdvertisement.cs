using System;
using System.Collections.Generic;

namespace Noclegi.Model
{
    public partial class AspNetAdvertisement
    {
        public AspNetAdvertisement()
        {
            InverseExchangeAd = new HashSet<AspNetAdvertisement>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal? Price { get; set; }
        public bool Rent { get; set; }
        public bool LookingFor { get; set; }
        public bool Exchange { get; set; }
        public DateTime CreateDate { get; set; }
        public string PropertyType { get; set; }
        public int? Floor { get; set; }
        public int? Rooms { get; set; }
        public int? ExchangeAdId { get; set; }

        public virtual AspNetAdvertisement ExchangeAd { get; set; }
        public virtual ICollection<AspNetAdvertisement> InverseExchangeAd { get; set; }
    }
}
