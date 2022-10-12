using EnsimagCafet.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EnsimagCafet.EntityFrameworkCore
{
    public sealed class ApplicationDbContext : DbContext
    {
        private class PersonalDataConverter : ValueConverter<string, string>
        {
            public PersonalDataConverter(IPersonalDataProtector protector) : base(str => protector.Protect(str), str => protector.Unprotect(str), default) { }
        }

#pragma warning disable CS8618
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {      
        }
#pragma warning restore CS8618

        public DbSet<User> Users { get; private set; }

        public DbSet<UserClaim> UserClaims { get; private set; }

        public DbSet<UserLogin> UserLogins { get; private set; }

        public DbSet<UserToken> UserTokens { get; private set; }

        public DbSet<UserRole> UserRoles { get; private set; }

        public DbSet<Role> Roles { get; private set; }

        public DbSet<RoleClaim> RoleClaims { get; private set; }

        private StoreOptions? GetStoreOptions()
        {
            return this.GetService<IDbContextOptions>().Extensions.OfType<CoreOptionsExtension>().FirstOrDefault()?
            .ApplicationServiceProvider?.GetService<IOptions<IdentityOptions>>()?.Value?.Stores;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            ConfigureIdentity(builder);
        }

        private void ConfigureIdentity(ModelBuilder modelBuilder)
        {
            var storeOptions = GetStoreOptions();
            var maxKeyLength = storeOptions?.MaxLengthForKeys ?? 0;
            var encryptPersonalData = storeOptions?.ProtectPersonalData ?? false;
            var converter = new PersonalDataConverter(this.GetService<IPersonalDataProtector>());
            modelBuilder.Entity<User>(builder =>
            {
                builder.ToTable("AspNetUsers");
                builder.HasKey(u => u.Id);
                builder.HasIndex(u => u.UserName).HasDatabaseName("UserNameIndex").IsUnique();
                builder.HasIndex(u => u.Email).HasDatabaseName("EmailIndex").IsUnique();
                builder.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();
                builder.Property(u => u.UserName).HasMaxLength(256);
                builder.Property(u => u.Email).HasMaxLength(256);
                if (encryptPersonalData)
                {
                    EncryptPersonalData(converter, builder);
                }
                builder.HasMany<UserClaim>().WithOne().HasForeignKey(userClaim => userClaim.UserId).IsRequired();
                builder.HasMany<UserLogin>().WithOne().HasForeignKey(userLogin => userLogin.UserId).IsRequired();
                builder.HasMany<UserToken>().WithOne().HasForeignKey(userToken => userToken.UserId).IsRequired();
                builder.HasMany<UserRole>().WithOne().HasForeignKey(userRole => userRole.UserId).IsRequired();
            });
            modelBuilder.Entity<UserClaim>(builder =>
            {
                builder.ToTable("AspNetUserClaims");
                builder.HasKey(userClaim => userClaim.Id);
            });
            modelBuilder.Entity<UserLogin>(builder =>
            {
                builder.ToTable("AspNetUserLogins");
                builder.HasKey(userLogin => new { userLogin.LoginProvider, userLogin.ProviderKey });
                if (maxKeyLength > 0)
                {
                    builder.Property(userLogin => userLogin.LoginProvider).HasMaxLength(maxKeyLength);
                    builder.Property(userLogin => userLogin.ProviderKey).HasMaxLength(maxKeyLength);
                }
            });
            modelBuilder.Entity<UserToken>(builder =>
            {
                builder.ToTable("AspNetUserTokens");
                builder.HasKey(userToken => new { userToken.UserId, userToken.LoginProvider, userToken.Name });
                if (maxKeyLength > 0)
                {
                    builder.Property(userToken => userToken.LoginProvider).HasMaxLength(maxKeyLength);
                    builder.Property(userToken => userToken.Name).HasMaxLength(maxKeyLength);
                }
                if (encryptPersonalData)
                {
                    EncryptPersonalData(converter, builder);
                }
            });
            modelBuilder.Entity<Role>(builder =>
            {
                builder.ToTable("AspNetRoles");
                builder.HasKey(role => role.Id);
                builder.HasIndex(role => role.Name).HasDatabaseName("RoleNameIndex").IsUnique();
                builder.Property(role => role.ConcurrencyStamp).IsConcurrencyToken();
                builder.Property(role => role.Name).HasMaxLength(256);
                builder.HasMany<UserRole>().WithOne().HasForeignKey(userRole => userRole.RoleId).IsRequired();
                builder.HasMany<RoleClaim>().WithOne().HasForeignKey(roleClaim => roleClaim.RoleId).IsRequired();
            });
            modelBuilder.Entity<RoleClaim>(builder =>
            {
                builder.ToTable("AspNetRoleClaims");
                builder.HasKey(rc => rc.Id);
            });
            modelBuilder.Entity<UserRole>(builder =>
            {
                builder.ToTable("AspNetUserRoles");
                builder.HasKey(r => new { r.UserId, r.RoleId });
            });
        }

        private static void EncryptPersonalData<TEntity>(PersonalDataConverter converter, EntityTypeBuilder<TEntity> builder) where TEntity : class
        {
            var personalDataProps = typeof(TEntity).GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(ProtectedPersonalDataAttribute)));
            foreach (var property in personalDataProps)
            {
                if (property.PropertyType != typeof(string))
                {
                    throw new InvalidOperationException("Only strings can be protected !");
                }
                builder.Property(typeof(string), property.Name).HasConversion(converter);
            }
        }
    }
}