using Domain.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("warehouse")]
    public class Warehouse : Entity<int>
    {
        public string Name { get; set; } = null!;

        public int AddressId { get; set; }
        [ForeignKey(nameof(AddressId))]
        public Address? Address { get; set; }

        public ICollection<Storage> Storages { get; set; } = new List<Storage>();
    }
}
