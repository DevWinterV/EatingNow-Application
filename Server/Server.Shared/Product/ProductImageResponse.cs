namespace DaiPhucVinh.Shared.Product
{
    public class ProductImageResponse
    {
        public int Id { get; set; }
        public string ItemCode { get; set; }
        public string ImageName { get; set; }
        public byte[] Image { get; set; }
        public string Description { get; set; }
        public bool? IsMain { get; set; }
        public bool? IsHiden { get; set; }
    }
}
