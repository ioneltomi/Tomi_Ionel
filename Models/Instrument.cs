using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tomi_Ionel.Models
{
    public class Instrument
    {
        
            public int ID { get; set; }
            public string Type { get; set; }
            public string Brand { get; set; }
            [Column(TypeName = "decimal(6, 2)")]
            public decimal Price { get; set; }
            public ICollection<Order> Orders { get; set; }
        public ICollection<ManufacturedInstruments> ManufacturedInstruments { get; set; }
    }
}
