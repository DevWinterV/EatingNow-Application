using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaiPhucVinh.Server.Data.Entity
{
    [Table("z_UserClaim")]
    public class UserClaim
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [MaxLength(32)]
        public string ClaimName { get; set; }
        public string ClaimValue { get; set; }
        public bool? Deleted { get; set; }
    }
}
