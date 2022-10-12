using APITools.CommonTools;
using APITools.Domain;
using APITools.Domain.Contracts;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using static EnsimagCafet.Domain.Shared.Identity.RoleConsts;

namespace EnsimagCafet.Domain.Identity
{
    public sealed class Role : Entity<Guid>, IEquatable<Role>
    {
        private Role()
        {
            Name = "";
            ConcurrencyStamp = Guid.NewGuid();
        }

        private Role(Guid id, string name) : base(id)
        {
            Name = name;
            ConcurrencyStamp = Guid.NewGuid();
        }

        public string Name { get; private set; }

        public Guid ConcurrencyStamp { get; private set; }

        public static Result<Role> Instanciate(Guid id, string name)
        {
            var validation = name.NotNullOrWhiteSpace(nameof(name))
                && name.Match(nameof(name), NameRegex);
            if (validation)
            {
                return new Role(id, name);
            }
            return Result.Error<Role>(validation.Exception);
        }



        public bool Equals(Role? other)
        {
            return Equals(other as Entity<Guid>);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Role);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}