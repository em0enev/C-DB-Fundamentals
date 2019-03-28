using System.ComponentModel.DataAnnotations;

namespace FastFood.Web.ViewModels.Employees
{
    public class RegisterEmployeeInputModel
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [Range(16,65)]
        public int Age { get; set; }

        [Required]
        public string PositionName { get; set; }

        [Required]
        [MinLength(3)]
        public string Address { get; set; }
    }
}
