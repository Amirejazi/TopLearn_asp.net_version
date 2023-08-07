using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.DataLayer.Context
{
    public class TopLearnContext : DbContext
    {
        public TopLearnContext(DbContextOptions<TopLearnContext> option) : base(option)
        {

        }

        #region User

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserToRole> UserToRoles { get; set; }

        #endregion


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserToRole>().HasKey(t => new { t.UserId, t.RoleId });


            base.OnModelCreating(modelBuilder);
        }
    }
}
