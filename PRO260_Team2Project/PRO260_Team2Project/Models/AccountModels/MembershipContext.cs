using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PRO260_Team2Project.Models.AccountModels
{
    public class MembershipContext : DbContext
    {
        public DbSet<User> Users { get; set; }
    }

}