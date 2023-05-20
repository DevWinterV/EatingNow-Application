using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaiPhucVinh.Server.Data.Entity
{
    [Table("z_Setting")]
    public class Setting
    {
        //public z_Setting(string name, string value);
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        //public override string ToString();
    }
}
