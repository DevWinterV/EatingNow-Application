namespace DaiPhucVinh.Shared.Common
{
    public class UploadFileDto
    {
        //Kq tra ve
        public bool IsSuccess { get; set; }
        public int ImageId { get; set; }
        public string Url { get; set; }
        public string ErrorMessage { get; set; }
        //
        public string Mime { get; set; }
        public byte[] Content { get; set; }
    }
}
