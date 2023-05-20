using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.Auth
{
    public class RoleDto
    {
        public string SystemName { get; set; }
        public string Display { get; set; }
        public bool Active { get; set; } = true;
        public string Permissons { get; set; }
        public bool UserCanDeleted { get; set; } = true;
        public List<string> lstPermissons { get; set; }
    }
    public class PermissonsDto {
        public string Permissons{ get; set; }
    }
}
