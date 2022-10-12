using APITools.CommonTools;
using APITools.Domain;
using APITools.Domain.Contracts;
using Microsoft.AspNetCore.Identity;

namespace EnsimagCafet.Domain.Identity
{
    public sealed class UserToken : Entity, IEquatable<UserToken>
    {
        private UserToken() : base()
        {
            LoginProvider = "";
            Name = "";
        }

        private UserToken(Guid userId, string loginProvider, string name, string? value) : base()
        {
            UserId = userId;
            LoginProvider = loginProvider;
            Name = name;
            Value = value;
        }

        public static Result<UserToken> Instanciate(Guid userId, string loginProvider, string name, string? value)
        {
            var validation = userId.NotDefault(nameof(userId)) && loginProvider.NotNull(nameof(loginProvider))
                && name.NotNull(nameof(name));
            if (validation)
            {
                return new UserToken(userId, loginProvider, name, value);
            }
            return Result.Error<UserToken>(validation.Exception);
        }

        public Guid UserId { get; private set; }

        public string LoginProvider { get; private set; }

        public string Name { get; private set; }

        [ProtectedPersonalData]
        public string? Value { get; private set; }

        public bool Equals(UserToken? other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return other is UserToken token && UserId == token.UserId && LoginProvider == token.LoginProvider
                && Name == token.Name && Value == token.Value;
        }

        public override bool Equals(Entity? other)
        {
            return Equals(other as UserLogin);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as UserLogin);
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(GetType());
            hash.Add(UserId);
            hash.Add(LoginProvider);
            hash.Add(Name);
            hash.Add(Value ?? "");
            return hash.ToHashCode();
        }
    }
}