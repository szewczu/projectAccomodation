namespace Noclegi.Model
{
    public partial class AspNetPicture
    {

        public int AdvertisementId { get; set; }
        public string Picture1 { get; set; }
        public string Picture2 { get; set; }
        public string Picture3 { get; set; }
        public string Picture4 { get; set; }
        public string Picture5 { get; set; }
        public string Picture6 { get; set; }

        public virtual AspNetAdvertisement Advertisement { get; set; }
    }
}
