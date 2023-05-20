using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaiPhucVinh.Server.Data.Entity
{
    [Table("z_User")]
    public class User
    {

        [Key]
        public int Id { get; set; }
        //[Index(IsUnique = true)]
        [MaxLength(64)]
        [Required]
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string AvatarUrl { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Salt { get; set; }
        public bool Active { get; set; }
        [Required]
        public string Roles { get; set; }
        public string RoleSystem { get; set; }
        [ForeignKey("RoleSystem")]
        public virtual Role Role { get; set; }
        public virtual ICollection<UserClaim> UserClaims { get; set; }
        public bool Deleted { get; set; }
    }
}
