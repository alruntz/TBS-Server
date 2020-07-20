using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TBS.Models;

namespace WebAPI.Security
{
    public class RoleAuthorizationRequirement : IAuthorizationRequirement
    {
        public RoleAuthorizationRequirement(UserRole role)
        {
            Role = role;
        }

        public UserRole Role { get; set; }
    }

    public class RoleAuthorizationHandler : AuthorizationHandler<RoleAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleAuthorizationRequirement requirement)
        {
            User user = Authentication.GetUser(context.User).Result;

            if (user == null || user.Role < (int)requirement.Role)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
