using DaiPhucVinh.Server.Data.FES;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Services.Database
{
    public class FesContext : DbContext
    {
        public FesContext() : base("FesContext")
        {
        }

        
        public DbSet<ACL_User> ACL_User { get; set; }
    }
}
