using Domain.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("storage")]
    public class Storage : Entity<int>
    {
        public string Name { get; set; }
        public string Type { get; set; } = "warehouse";
        public int WarehouseId { get; set; }
        [ForeignKey(nameof(WarehouseId))]
        public Warehouse? Warehouse { get; set; }
    }
}
