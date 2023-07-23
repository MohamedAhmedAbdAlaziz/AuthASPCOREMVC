using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EmployeeManagement.Security
{
    public class CanEditOnlyOtherAdminRolesAndClaimsHandler : AuthorizationHandler<ManageAdminRolesAndClaimsRequirment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
         ManageAdminRolesAndClaimsRequirment requirement)
        {
            var authFilterContext=context.Resource as AuthorizationFilterContext;
            if(authFilterContext==null){
                return Task.CompletedTask;
            }
            string loggedInAdminId= context.User.Claims.FirstOrDefault(x=>x.Type== ClaimTypes.NameIdentifier).Value;
            string adminIdBeingEdit=authFilterContext.HttpContext.Request.Query["userId"];
            if(context.User.IsInRole("Admin") &&
             context.User.HasClaim(claim=> claim.Type== "Edit Role" && claim.Value =="true")  &&
            adminIdBeingEdit.ToLower() != loggedInAdminId.ToLower())
            {
             context.Succeed(requirement);
            }
            return Task.CompletedTask;
            
        }
    }
}