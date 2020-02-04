using System.ComponentModel.DataAnnotations;

namespace Noclegi.Model
{
    public partial class AspNetAdress
    {
        [Key]
        public int AdvertisementId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }

        public virtual AspNetAdvertisement Advertisement { get; set; }
    }
}
