using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50,ErrorMessage="Name cannot exeed 50")]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$", ErrorMessage ="inValid Emil")]
        public string Email { get; set; }
        
        [Required]

        public Dept? Department { get; set; }
        public string PHotoPath { get; set; }
       
    }
}
