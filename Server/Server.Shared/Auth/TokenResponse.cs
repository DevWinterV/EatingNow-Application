using System;

namespace DaiPhucVinh.Shared.Auth
{
    public class TokenResponse : Falcon.Web.Core.Auth.Ticket
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public int IdUser { get; set; }
        public string Code { get; set; }
        /// <summary>
        /// ExpiresIn in hours
        /// </summary>
        public int ExpiresIn { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public string RefreshToken { get; set; }
        public string TokenType { get; set; }
        public string Role { get; set; }
        public string RoleSystem { get; set; }
        public bool IsError { get; set; }
        public string ErrorDescription { get; set; }
        public string AccessToken { get; set; }
        public string LocationCode { get; set; }
        public double? Long { get; set; }
        public double? Lat { get; set; }
        public string EmloyeeCode { get; set; }
        public string CustomerCode { get; set; }
        public bool? LockEditProduct { get; set; }
    }
}