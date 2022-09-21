using EnsimagCafet.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using static EnsimagCafet.Domain.Shared.Identity.RoleConsts;
using static EnsimagCafet.Domain.Shared.Identity.UserConsts;

namespace EnsimagCafet.EntityFrameworkCore.DbMigrator
{
    public sealed class ApplicationDbDataSeeder
    {
        private readonly ApplicationDbDataSeedingOptions _options;

        public ApplicationDbDataSeeder(ApplicationDbDataSeedingOptions options)
        {
            _options = options;
        }

        public void Seed(ApplicationDbContext context)
        {
            _ = context.Database.EnsureCreated();
            if (_options.ChecksAllRoles)
            {
                if (context.Roles.FirstOrDefault(role => role.Id == AdminRoleId) is null)
                {
                    _ = context.Roles.Add(new() { Id = AdminRoleId, Name = AdminRoleName });
                }
                if (context.Roles.FirstOrDefault(role => role.Id == MasterRoleId) is null)
                {
                    _ = context.Roles.Add(new() { Id = MasterRoleId, Name = MasterRoleName });
                }
                if (context.Roles.FirstOrDefault(role => role.Id == ManagerRoleId) is null)
                {
                    _ = context.Roles.Add(new() { Id = ManagerRoleId, Name = ManagerRoleName });
                }
                if (context.Roles.FirstOrDefault(role => role.Id == CustomerRoleId) is null)
                {
                    _ = context.Roles.Add(new() { Id = CustomerRoleId, Name = CustomerRoleName });
                }
            }
            if (_options.ChecksSuperUser)
            {
                if (_options.ChecksSuperUser)
                {
                    if (context.Users.FirstOrDefault(user => user.Id == SuperUserId) is null)
                    {
                        PasswordHasher<User> passwordHasher = new();
                        User user = new()
                        {
                            Id = SuperUserId,
                            UserName = SuperUserUserName,
                            NormalizedUserName = SuperUserUserName.ToUpper(),
                            Email = SuperUserEmail,
                            NormalizedEmail = SuperUserEmail.ToUpper(),
                            SecurityStamp = string.Concat(Array.ConvertAll(Guid.NewGuid().ToByteArray(), b => b.ToString("X2")))
                        };
                        user.PasswordHash = passwordHasher.HashPassword(user, _options.SuperUserDefaultPassword);
                        _ = context.Users.Add(user);
                        _ = context.UserRoles.Add(new() { UserId = SuperUserId, RoleId = AdminRoleId });
                        _ = context.UserRoles.Add(new() { UserId = SuperUserId, RoleId = MasterRoleId });
                        _ = context.UserRoles.Add(new() { UserId = SuperUserId, RoleId = ManagerRoleId });
                        _ = context.UserRoles.Add(new() { UserId = SuperUserId, RoleId = CustomerRoleId });
                    }
                }
            }
            _ = context.SaveChanges();
        }
    }
}
