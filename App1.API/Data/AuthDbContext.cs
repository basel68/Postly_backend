using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace App1.API.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //Role Ids
            var readerRoleId = "2346f9f9-53b0-4500-8a2b-f30b086d58d9";
            var writerRoleId = "a408b087-5dd7-4be4-90de-4fa6aee70135";

            var roles = new List<IdentityRole> {
                new IdentityRole()
                {
                    Id = readerRoleId,
                    Name = "Reader",
                    ConcurrencyStamp=readerRoleId,
                    NormalizedName="Reader".ToUpper()

                },
                new IdentityRole()
                {
                    Id=writerRoleId,
                    Name="Writer",
                    ConcurrencyStamp=writerRoleId,
                    NormalizedName="Writer".ToUpper()
                }
            };
            //seed the roles
            builder.Entity<IdentityRole>().HasData( roles );

            //Admin user
            var adminUserId = "c372591a-e973-4e14-820d-c2a4bc3bc9e7";
            var adminUser = new IdentityUser
            {
                Id = adminUserId,
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                NormalizedEmail = "admin@gmail.com".ToUpper(),
                NormalizedUserName = "admin@gmail.com".ToUpper()
            };
            var adminPassword = new PasswordHasher<IdentityUser>().HashPassword(adminUser, "beso");
            //seed admin user
            builder.Entity<IdentityUser>().HasData( adminUser );

            //give roles to admin
            var adminRoles = new List<IdentityUserRole<string>>
            {
                new()
                {
                    RoleId=readerRoleId,
                    UserId=adminUserId,
                },
                new()
                {
                    RoleId = writerRoleId,
                    UserId=adminUserId,
                }

            };
            builder.Entity<IdentityUserRole<string>>().HasData( adminRoles );
        }
    }
}
