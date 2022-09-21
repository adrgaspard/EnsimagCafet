using Microsoft.AspNetCore.Authorization;

namespace EnsimagCafet.Web.Authorization
{
    public sealed class SuperUserAuthorizationHandler : AuthorizationHandler<SuperUserRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SuperUserRequirement requirement)
        {
            if (context.User.Identity?.Name == requirement.Identifier && requirement.RequiredRoles.All(role => context.User.IsInRole(role)))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}