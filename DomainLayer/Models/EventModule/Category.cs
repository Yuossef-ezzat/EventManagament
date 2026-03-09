using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.EventModule
{
    public class Category : BaseEntity<int>
    {
        public string Name { get; set; } = null!;
        public List<Event> Events { get; set; }
    }
}
