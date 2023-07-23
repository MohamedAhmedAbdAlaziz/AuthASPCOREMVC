using System.ComponentModel.DataAnnotations;
using Core.Models;

namespace EmployeeManagement.ViewModels
{
    //RegisterViewModel
    public class EmployeeCreateViewModel
    {
             
        [Required]
        [MaxLength(50,ErrorMessage="Name cannot exeed 50")]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$", ErrorMessage ="inValid Emil")]
        public string Email { get; set; }
        
        [Required]

        public Dept? Department { get; set; }
        public List<IFormFile> Photos{ get; set; }
        
        public IFormFile Photo{ get; set; }
    }
}