using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tomi_Ionel.Models
{
    public class ManufacturedInstruments
    {
        public int ManufacturersID { get; set; }
        public int InstrumentID { get; set; }
        public Manufacturers Manufacturers { get; set; }
        public Instrument Instrument { get; set; }
    }
}
