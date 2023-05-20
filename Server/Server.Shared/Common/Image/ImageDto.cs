namespace DaiPhucVinh.Shared.Common.Image
{
    public class ImageDto
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string RelativePath { get; set; }
        public string AbsolutePath { get; set; }
        public bool IsExternal { get; set; }
        public string CreatedAt { get; set; }
        public bool IsUsed { get; set; }
        public bool? IsMain { get; set; }
    }
}
