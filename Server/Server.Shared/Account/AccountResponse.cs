using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.Account
{
    public class AccountResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string StoreName { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public bool Status { get; set; }
        public int AccountId { get; set; }

        public string AccessToken { get; set; }
    }
}
