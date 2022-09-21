using Microsoft.AspNetCore.Authorization;
using System.Collections.ObjectModel;

namespace EnsimagCafet.Web.Authorization
{
    public sealed class SuperUserRequirement : IAuthorizationRequirement
    {
        public SuperUserRequirement(string identifier, IEnumerable<string> requiredRoles)
        {
            ArgumentNullException.ThrowIfNull(identifier, nameof(identifier));
            Identifier = identifier;
            RequiredRoles = new ReadOnlyCollection<string>(requiredRoles.ToList());
        }

        public string Identifier { get; }

        public IReadOnlyList<string> RequiredRoles { get; }
    }
}
