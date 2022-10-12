using APITools.CommonTools;
using APITools.Domain;
using APITools.Domain.Contracts;
using static EnsimagCafet.Domain.Shared.Identity.UserConsts;
using Microsoft.AspNetCore.Identity;

namespace EnsimagCafet.Domain.Identity
{
    public sealed class User : Entity<Guid>, IEquatable<User>
    {
        private User()
        {
            UserName = "";
            FirstName = "";
            LastName = "";
            Email = "";
            PasswordHash = "";
            ConcurrencyStamp = Guid.NewGuid();
        }

        private User(Guid id, string userName, string firstName, string lastName, string email, string passwordHash) : base(id)
        {
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            EmailConfirmed = false;
            PasswordHash = passwordHash;
            SecurityStamp = Guid.NewGuid();
            ConcurrencyStamp = Guid.NewGuid();
            TwoFactorEnabled = false;
            LockoutEnd = null;
            LockoutEnabled = false;
            AccessFailedCount = 0;
            Balance = 0;
        }

        [ProtectedPersonalData]
        public string UserName { get; private set; }

        [ProtectedPersonalData]
        public string FirstName { get; private set; }

        [ProtectedPersonalData]
        public string LastName { get; private set; }

        [ProtectedPersonalData]
        public string Email { get; private set; }

        [PersonalData]
        public bool EmailConfirmed { get; private set; }

        public string PasswordHash { get; private set; }

        public Guid SecurityStamp { get; private set; }

        public Guid ConcurrencyStamp { get; private set; }

        public bool TwoFactorEnabled { get; private set; }

        public DateTimeOffset? LockoutEnd { get; private set; }

        public bool LockoutEnabled { get; private set; }

        public int AccessFailedCount { get; private set; }

        public float Balance { get; private set; }

        public static Result<User> Instanciate(Guid id, string userName, string passwordHash)
        {
            var validation = passwordHash.NotNullOrWhiteSpace(nameof(passwordHash), PasswordHashMaxLength, PasswordHashMinLength)
                && userName.NotNullOrWhiteSpace(nameof(userName), UserNameMaxLength, UserNameMinLength)
                && userName.Match(nameof(userName), UserNameRegex);
            if (validation)
            {
                var names = userName.Split('.');
                return new User(id, userName, names[0].FirstUpperThenLowerAndTrim(), names[1].FirstUpperThenLowerAndTrim(), userName + EmailSuffix, passwordHash);
            }
            return Result.Error<User>(validation.Exception);
        }

        public static Result<User> Instanciate(string userName, string passwordHash)
        {
            return Instanciate(Guid.NewGuid(), userName, passwordHash);
        }

        public Result SetPasswordHash(string passwordHash)
        {
            var validation = passwordHash.NotNullOrWhiteSpace(nameof(passwordHash), PasswordHashMaxLength, PasswordHashMinLength);
            if (validation)
            {
                PasswordHash = passwordHash;
            }
            return validation;
        }

        public Result SetBalance(float balance)
        {
            var validation = balance.PositiveOrZero(nameof(balance));
            if (validation)
            {
                Balance = balance;
            }
            return validation;
        }

        public bool Equals(User? other)
        {
            return Equals(other as Entity<Guid>);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as User);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}