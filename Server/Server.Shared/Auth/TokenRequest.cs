namespace DaiPhucVinh.Shared.Auth
{
    public class TokenRequest
    {
        public string Tenant { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string PhoneOs { get; set; }
        public string DeviceToken { get; set; }
        public string LocationCode { get; set; }
    }
}