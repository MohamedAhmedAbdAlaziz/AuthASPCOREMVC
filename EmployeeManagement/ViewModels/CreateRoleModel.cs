using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.ViewModels
{
    public class CreateRoleModel
    {
        [Required]
        public string RoleName {get;set;}
    }
}