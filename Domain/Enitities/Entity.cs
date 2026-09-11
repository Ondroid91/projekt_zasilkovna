using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public class Entity<Tkey> : IEnity<Tkey>
    {
        public Tkey Id { get; set; }
    }
}

