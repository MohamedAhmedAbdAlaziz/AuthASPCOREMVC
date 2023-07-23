using Microsoft.AspNetCore.Identity;

namespace Core.Models
{
    public class ApplicationUser:IdentityUser
    {
     public string City {get ;set;}   
    }
}