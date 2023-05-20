using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.User
{
    public class RolesResponse
    {
        public string SystemName { get; set; }
        public string Display { get; set; }
        public bool Active { get; set; }
        public string Permissons { get; set; }
        public bool? LockEditProduct { get; set; }

    }
}
