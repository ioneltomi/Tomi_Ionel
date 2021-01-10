using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Tomi_Ionel.Models
{
    public class Manufacturers
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Manufacturer Name")]
        [StringLength(50)]
        public string ManufacturersName { get; set; }

        [StringLength(70)]
        public string Adress { get; set; }
        public ICollection<ManufacturedInstruments> ManufacturedInstruments { get; set; }
    }
}
