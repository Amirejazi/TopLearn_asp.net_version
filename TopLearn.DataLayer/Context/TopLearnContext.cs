using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.DataLayer.Entities.Course;
using TopLearn.DataLayer.Entities.Order;
using TopLearn.DataLayer.Entities.Permissions;
using TopLearn.DataLayer.Entities.User;
using TopLearn.DataLayer.Entities.Wallet;

namespace TopLearn.DataLayer.Context
{
    public class TopLearnContext : DbContext
    {
        public TopLearnContext(DbContextOptions<TopLearnContext> option) : base(option)
        {

        }

        #region User

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserToRole> UserToRoles { get; set; }
        public DbSet<UserDiscountCode> UserDiscountCodes { get; set; }

        #endregion

        #region Wallet

        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<WalletType> WalletTypes { get; set; }

        #endregion

        #region Permission

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RoleToPermission> RoleToPermissions { get; set; }

        #endregion

        #region Course

        public DbSet<CourseGroup> CourseGroups { get; set; }
        public DbSet<CourseLevel> CourseLevels { get; set; }
        public DbSet<CourseStatus> CourseStatus { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseEpisode> CourseEpisodes { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }
        public DbSet<CourseComment> CourseComments { get; set; }
        public DbSet<CourseVote> CourseVotes { get; set; }

        #endregion

        #region Order

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Discount> Discounts { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserToRole>().HasKey(t => new { t.UserId, t.RoleId });
            modelBuilder.Entity<RoleToPermission>().HasKey(t => new { t.RoleId, t.PermissionId });
            modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDelete);
            modelBuilder.Entity<Role>().HasQueryFilter(r => !r.IsDelete);
            modelBuilder.Entity<CourseGroup>().HasQueryFilter(cg => !cg.IsDelete);
            modelBuilder.Entity<Course>().HasQueryFilter(c => !c.IsDelete);


            modelBuilder.Entity<Course>()
                .HasOne<CourseGroup>(f => f.SubGroup)
                .WithMany(g => g.CoursesForSubGroup)
                .HasForeignKey(f => f.SubGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            #region Seed Data WalletTypes

            modelBuilder.Entity<WalletType>().HasData(
                new WalletType()
                {
                    TypeId = 1,
                    TypeTitle = "واریز"
                },
                new WalletType()
                {
                    TypeId = 2,
                    TypeTitle = "برداشت"
                }
            );

            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}
