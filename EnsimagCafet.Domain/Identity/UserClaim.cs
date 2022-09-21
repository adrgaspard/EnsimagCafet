using APITools.Domain.Contracts;
using Microsoft.AspNetCore.Identity;

namespace EnsimagCafet.Domain.Identity
{
    public class UserClaim : IdentityUserClaim<Guid>, IEntity<int>
    {
    }
}