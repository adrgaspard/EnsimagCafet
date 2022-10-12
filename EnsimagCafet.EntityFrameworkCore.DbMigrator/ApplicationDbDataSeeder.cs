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
                    _ = context.Roles.Add(Role.Instanciate(AdminRoleId, AdminRoleName).Value);
                }
                if (context.Roles.FirstOrDefault(role => role.Id == MasterRoleId) is null)
                {
                    _ = context.Roles.Add(Role.Instanciate(MasterRoleId, MasterRoleName).Value);
                }
                if (context.Roles.FirstOrDefault(role => role.Id == ManagerRoleId) is null)
                {
                    _ = context.Roles.Add(Role.Instanciate(ManagerRoleId, ManagerRoleName).Value);
                }
                if (context.Roles.FirstOrDefault(role => role.Id == CustomerRoleId) is null)
                {
                    _ = context.Roles.Add(Role.Instanciate(CustomerRoleId, CustomerRoleName).Value);
                }
            }
            if (_options.ChecksSuperUser)
            {
                if (_options.ChecksSuperUser)
                {
                    if (context.Users.FirstOrDefault(user => user.Id == SuperUserId) is null)
                    {
                        PasswordHasher<User> passwordHasher = new();
                        User user = User.Instanciate(SuperUserId, SuperUserUserName, "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX").Value;
                        user.SetPasswordHash(passwordHasher.HashPassword(user, _options.SuperUserDefaultPassword));
                        _ = context.Users.Add(user);
                        _ = context.UserRoles.Add(UserRole.Instanciate(SuperUserId, AdminRoleId).Value);
                        _ = context.UserRoles.Add(UserRole.Instanciate(SuperUserId, MasterRoleId).Value);
                        _ = context.UserRoles.Add(UserRole.Instanciate(SuperUserId, ManagerRoleId).Value);
                        _ = context.UserRoles.Add(UserRole.Instanciate(SuperUserId, CustomerRoleId).Value);
                    }
                }
            }
            _ = context.SaveChanges();
        }
    }
}