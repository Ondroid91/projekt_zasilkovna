using Domain.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace Domain.Entities
{
    [Table("address")]
    public class Address : Entity<int>
    {
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Zipcode { get; set; } = null!;
    }
}
