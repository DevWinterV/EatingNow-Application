using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.FES
{
    [Table("ACL_User")]
    public class ACL_User
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string DomainUserName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public DateTime? LastLogInDate { get; set; }
        public string PasswordHashed { get; set; }
        public string SecretQuestion { get; set; }
        public string SecretAnswerHashed { get; set; }
        public bool Disabled { get; set; }
        public string Description { get; set; }
    }
}
