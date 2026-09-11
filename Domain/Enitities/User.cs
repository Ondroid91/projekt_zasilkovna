using Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Validations;

namespace Domain.Entities
{
    [Table("user")]
    public class User : Entity<int>
    {
        [Required]
        public string Name { get; set; } = "";
        [EmailAddress]
        public string Email { get; set; } = "";
        [Phone]
        public string Phone { get; set; } = "";
        [Role]
        public string Role { get; set; } = "";
        public int? WorkplaceId { get; set; }
        public string? EmployeeType { get; set; }
    }
}