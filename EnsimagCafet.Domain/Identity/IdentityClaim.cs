using APITools.CommonTools;
using APITools.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EnsimagCafet.Domain.Identity
{
    public abstract class IdentityClaim : Entity<Guid>, IEquatable<IdentityClaim>
    {
        protected IdentityClaim()
        {
            ClaimType = "";
            ClaimValue = "";
        }

        protected IdentityClaim(Guid id, string claimType, string claimValue) : base(id)
        {
            ClaimType = claimType;
            ClaimValue = claimValue;
        }

        public string ClaimType { get; protected set; }

        public string ClaimValue { get; protected set; }

        public Claim ToClaim()
        {
            return new(ClaimType, ClaimValue);
        }

        public Result SetClaim(Claim other)
        {
            var validation = other.Type.NotNull(nameof(other.Type)) && other.Value.NotNull(nameof(other.Value));
            if (validation)
            {
                ClaimType = other.Type;
                ClaimValue = other.Value;
            }
            return validation;
        }

        public bool Equals(IdentityClaim? other)
        {
            return Equals(other as Entity<Guid>);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as IdentityClaim);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
