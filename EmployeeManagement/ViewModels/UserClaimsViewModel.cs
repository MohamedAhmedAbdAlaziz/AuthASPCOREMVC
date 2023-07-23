namespace EmployeeManagement.ViewModels
{
    public class UserClaimsViewModel
    {

     public string userId { get; set; }
     public List<UserClaim> Claims {get;set;}= new List<UserClaim>();   
    }
}