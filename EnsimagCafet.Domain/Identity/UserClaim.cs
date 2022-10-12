using APITools.CommonTools;
using APITools.Domain;
using APITools.Domain.Contracts;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace EnsimagCafet.Domain.Identity
{
    public sealed class UserClaim : IdentityClaim, IEquatable<UserClaim> 
    {
        private UserClaim() : base()
        {
        }

        private UserClaim(Guid id, Guid userId, string claimType, string claimValue) : base(id, claimType, claimValue)
        {
            UserId = userId;
        }

        public Guid UserId { get; private set; }

        public static Result<UserClaim> Instanciate(Guid id, Guid userId, string claimType, string claimValue)
        {
            var validation = userId.NotDefault(nameof(userId)) && claimType.NotNull(claimType) && claimValue.NotNull(claimValue);
            if (validation)
            {
                return new UserClaim(id, userId, claimType, claimValue);
            }
            return Result.Error<UserClaim>(validation.Exception);
        }

        public static Result<UserClaim> Instanciate(Guid id, Guid userId, Claim claim)
        {
            return Instanciate(id, userId, claim.Type, claim.Value);
        }

        public bool Equals(UserClaim? other)
        {
            return Equals(other as Entity<Guid>);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as UserClaim);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}