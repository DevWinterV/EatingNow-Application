using DaiPhucVinh.Shared.Common;

namespace DaiPhucVinh.Shared.Store
{
    public class StoreResponse
    {
        public int UserId { get; set; }
        public string AbsoluteImage { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public string OpenTime { get; set; }
        public string Province { get; set; }
        public int ProvinceId { get; set; } 
        public string Cuisine { get; set; }
        public int   CuisineId { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string OwnerName { get; set; }
        public string Phone { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Status { get; set; }
        public double Distance { get; set; }
        public double Time { get; set; }
        public double similarity { get; set; }
    }
}
