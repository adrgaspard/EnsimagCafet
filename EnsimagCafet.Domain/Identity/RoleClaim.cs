using APITools.CommonTools;
using APITools.Domain;
using APITools.Domain.Contracts;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Security.Claims;

namespace EnsimagCafet.Domain.Identity
{
    public sealed class RoleClaim : IdentityClaim, IEquatable<RoleClaim>
    {
        private RoleClaim() : base()
        {
        }

        private RoleClaim(Guid id, Guid roleId, string claimType, string claimValue) : base(id, claimType, claimValue)
        {
            RoleId = roleId;
        }

        public static Result<RoleClaim> Instanciate(Guid id, Guid roleId, string claimType, string claimValue)
        {
            var validation = roleId.NotDefault(nameof(roleId)) && claimType.NotNull(nameof(claimType)) && claimValue.NotNull(nameof(claimValue));
            if (validation)
            {
                return new RoleClaim(id, roleId, claimType, claimValue);
            }
            return Result.Error<RoleClaim>(validation.Exception);
        }

        public static Result<RoleClaim> Instanciate(Guid id, Guid roleId, Claim claim)
        {
            return Instanciate(id, roleId, claim.Type, claim.Value);
        }

        public Guid RoleId { get; private set; }

        public bool Equals(RoleClaim? other)
        {
            return Equals(other as Entity<Guid>);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as RoleClaim);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}