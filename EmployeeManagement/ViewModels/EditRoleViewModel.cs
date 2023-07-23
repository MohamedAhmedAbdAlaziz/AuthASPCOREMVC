using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.ViewModels
{
    public class EditRoleViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage ="Role Name is Required")]
        public string RoleName { get; set; }
        public List<string> users { get; set; }= new List<string>();
    }
}