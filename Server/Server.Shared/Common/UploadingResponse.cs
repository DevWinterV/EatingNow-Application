namespace DaiPhucVinh.Shared.Common
{
    public class UploadingResponse
    {
        public bool IsSuccess { get; set; }
        public int ImageId { get; set; }
        public string Url { get; set; }
        public string ErrorMessage { get; set; }
    }
}
