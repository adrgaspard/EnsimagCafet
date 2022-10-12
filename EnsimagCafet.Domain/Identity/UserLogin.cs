using APITools.CommonTools;
using APITools.Domain;
using APITools.Domain.Contracts;
using Microsoft.AspNetCore.Identity;

namespace EnsimagCafet.Domain.Identity
{
    public sealed class UserLogin : Entity, IEquatable<UserLogin>
    {
        private UserLogin() : base()
        {
            LoginProvider = "";
            ProviderKey = "";
            ProviderDisplayName = "";
        }

        private UserLogin(Guid userId, string loginProvider, string providerKey, string providerDisplayName) : base()
        {
            UserId = userId;
            LoginProvider = loginProvider;
            ProviderKey = providerKey;
            ProviderDisplayName = providerDisplayName;
        }

        public Guid UserId { get; private set; }

        public string LoginProvider { get; private set; }

        public string ProviderKey { get; private set; }

        public string ProviderDisplayName { get; private set; }

        public static Result<UserLogin> Instanciate(Guid userId, string loginProvider, string providerKey, string providerDisplayName)
        {
            var validation = userId.NotDefault(nameof(userId)) && loginProvider.NotNull(nameof(loginProvider))
                && providerKey.NotNull(nameof(providerKey)) && providerDisplayName.NotNull(nameof(providerDisplayName));
            if (validation)
            {
                return new UserLogin(userId, loginProvider, providerKey, providerDisplayName);
            }
            return Result.Error<UserLogin>(validation.Exception);
        }

        public bool Equals(UserLogin? other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return other is UserLogin login && UserId == login.UserId && LoginProvider == login.LoginProvider
                && ProviderKey == login.ProviderKey && ProviderDisplayName == login.ProviderDisplayName;
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
            hash.Add(ProviderKey);
            hash.Add(ProviderDisplayName);
            return hash.ToHashCode();
        }
    }
}