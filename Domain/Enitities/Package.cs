using Domain.Entities;
using Domain.Interfaces;
using Domain.Validations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("package")]
    public class Package : Entity<int>
    {
        public string Name { get; set; } = null!;
        [NonNegative]
        public decimal Weight { get; set; }
        public string Status { get; set; } = "created";

        public int SenderId { get; set; }
        [ForeignKey(nameof(SenderId))]
        public User Sender { get; set; } = null!;

        public int? MessengerId { get; set; }
        [ForeignKey(nameof(MessengerId))]
        public User? Messenger { get; set; }

        public int? WarehousemanId { get; set; }
        [ForeignKey(nameof(WarehousemanId))]
        public User? Warehouseman { get; set; }

        public int PickupAddressId { get; set; }
        [ForeignKey(nameof(PickupAddressId))]
        public Address PickupAddress { get; set; } = null!;

        public int DeliveryAddressId { get; set; }
        [ForeignKey(nameof(DeliveryAddressId))]
        public Address DeliveryAddress { get; set; } = null!;

        public int? StoragesId { get; set; }
        [ForeignKey(nameof(StoragesId))]
        public Storage? Storage { get; set; }

        public int? WarehouseId { get; set; }
        [ForeignKey(nameof(WarehouseId))]
        public Warehouse? Warehouse { get; set; }

        public int? StorageAddressId { get; set; }
        [ForeignKey(nameof(StorageAddressId))]
        public Address? StorageAddress { get; set; }
    }
}
