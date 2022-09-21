using EnsimagCafet.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace EnsimagCafet.EntityFrameworkCore.DbMigrator
{
    public sealed class ApplicationDbDataSeeder
    {
        protected readonly ApplicationDbDataSeedingOptions _options;

        public ApplicationDbDataSeeder(ApplicationDbDataSeedingOptions options)
        {
            _options = options;
        }

        public void Seed(ApplicationDbContext context)
        {
            _ = context.Database.EnsureCreated();
            Guid adminRoleId = Guid.Parse("88888888-0001-0001-83d9-bb4152f3299d");
            Guid masterRoleId = Guid.Parse("88888888-0001-0002-a04e-d0da21e72853");
            Guid managerRoleId = Guid.Parse("88888888-0001-0003-8de1-ca156a62546d");
            Guid customerRoleId = Guid.Parse("88888888-0001-0004-90ea-d03576334d72");
            if (_options.ChecksAllRoles)
            {
                if (context.Roles.FirstOrDefault(role => role.Id == adminRoleId) is null)
                {
                    _ = context.Roles.Add(new() { Id = adminRoleId, Name = "Administrator" });
                }
                if (context.Roles.FirstOrDefault(role => role.Id == masterRoleId) is null)
                {
                    _ = context.Roles.Add(new() { Id = masterRoleId, Name = "Master" });
                }
                if (context.Roles.FirstOrDefault(role => role.Id == managerRoleId) is null)
                {
                    _ = context.Roles.Add(new() { Id = managerRoleId, Name = "Manager" });
                }
                if (context.Roles.FirstOrDefault(role => role.Id == customerRoleId) is null)
                {
                    _ = context.Roles.Add(new() { Id = customerRoleId, Name = "Customer" });
                }
            }
            Guid superUserId = Guid.Parse("88888888-0002-0001-bd02-a0500e0cc7f3");
            if (_options.ChecksSuperUser)
            {
                if (_options.ChecksSuperUser)
                {
                    if (context.Users.FirstOrDefault(user => user.Id == superUserId) is null)
                    {
                        PasswordHasher<User> passwordHasher = new();
                        User user = new()
                        {
                            Id = superUserId,
                            UserName = "SuperUser",
                            NormalizedUserName = "SuperUser".ToUpper(),
                            Email = "super@user.com",
                            NormalizedEmail = "super@user.com".ToUpper(),
                            SecurityStamp = string.Concat(Array.ConvertAll(Guid.NewGuid().ToByteArray(), b => b.ToString("X2")))
                        };
                        user.PasswordHash = passwordHasher.HashPassword(user, "Aa123!");
                        _ = context.Users.Add(user);
                        _ = context.UserRoles.Add(new() { UserId = superUserId, RoleId = adminRoleId });
                        _ = context.UserRoles.Add(new() { UserId = superUserId, RoleId = masterRoleId });
                        _ = context.UserRoles.Add(new() { UserId = superUserId, RoleId = managerRoleId });
                        _ = context.UserRoles.Add(new() { UserId = superUserId, RoleId = customerRoleId });
                    }
                }
            }

            _ = context.SaveChanges();
        }
    }
}
