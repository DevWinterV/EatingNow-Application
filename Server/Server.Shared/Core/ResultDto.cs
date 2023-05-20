namespace DaiPhucVinh.Shared.Core
{
    public class ResultDto<T>
    {
        public T Result { get; set; }
        public string TargetUrl { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
        public bool UnAuthorizedRequest { get; set; }
    }
}
