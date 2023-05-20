using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaiPhucVinh.Server.Data.Entity
{
    [Table("z_Role")]
    public class Role
    {
        [MaxLength(128)]
        [Key]
        public string SystemName { get; set; }
        [MaxLength(32)]
        public string Display { get; set; }
        [Required]
        public bool Active { get; set; }
        public string Permissons { get; set; }
        [Required]
        public bool UserCanDeleted { get; set; }
        public bool? LockEditProduct { get; set; }

    }
}
