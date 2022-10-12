using APITools.CommonTools;
using APITools.Domain;
using APITools.Domain.Contracts;
using Microsoft.AspNetCore.Identity;

namespace EnsimagCafet.Domain.Identity
{
    public sealed class UserRole : Entity, IEquatable<UserRole>
    {
        private UserRole() : base()
        {

        }

        private UserRole(Guid userId, Guid roleId) : base()
        {
            UserId = userId;
            RoleId = roleId;
        }

        public Guid UserId { get; private set; }

        public Guid RoleId { get; private set; }

        public static Result<UserRole> Instanciate(Guid userId, Guid roleId)
        {
            var validation = userId.NotDefault(nameof(userId)) && roleId.NotDefault(nameof(roleId));
            if (validation)
            {
                return new UserRole(userId, roleId);
            }
            return Result.Error<UserRole>(validation.Exception);
        }

        public bool Equals(UserRole? other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return other is UserRole userRole && UserId == userRole.UserId && RoleId == userRole.RoleId;
        }

        public override bool Equals(Entity? other)
        {
            return Equals(other as UserRole);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as UserRole);
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(GetType());
            hash.Add(UserId);
            hash.Add(RoleId);
            return hash.ToHashCode();
        }
    }
}