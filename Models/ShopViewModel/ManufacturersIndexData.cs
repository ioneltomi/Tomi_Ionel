using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tomi_Ionel.Models.ShopViewModel
{
    public class ManufacturersIndexData
    {
        public IEnumerable<Manufacturers> Manufacturers { get; set; }
        public IEnumerable<Instrument> Instruments { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}
