using Microsoft.AspNetCore.Authorization;

namespace EmployeeManagement.Security
{
    public class SuperAdminHandler : AuthorizationHandler<ManageAdminRolesAndClaimsRequirment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageAdminRolesAndClaimsRequirment requirement)
        {
         if(context.User.IsInRole("Super Admin")){
            context.Succeed(requirement);
         }  
         return Task.CompletedTask;
        }
    }
}