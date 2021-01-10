using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tomi_Ionel.Models;

namespace Tomi_Ionel.Data
{
    public class DbInitializer
    {
        public static void Initialize(ShopContext context)
        {
            context.Database.EnsureCreated();
            if (context.Instruments.Any())
            {
                return; // BD a fost creata anterior
            }
            var instruments = new Instrument[]
            {
 new Instrument{Type="Guitar",Brand="Fender",Price=Decimal.Parse("22")},
 new Instrument{Type="Flute",Brand="Yamaha",Price=Decimal.Parse("18")},
 new Instrument{Type="Violin",Brand="Cecilio",Price=Decimal.Parse("27")},
 new Instrument{Type="Saxophone",Brand="Arnolds",Price=Decimal.Parse("45")},
 new Instrument{Type="Piano",Brand="Thomann",Price=Decimal.Parse("38")},
 new Instrument{Type="Xylophone",Brand="Stagg",Price=Decimal.Parse("28")},

            };
            foreach (Instrument s in instruments)
            {
                context.Instruments.Add(s);
            }
            context.SaveChanges();
            var customers = new Customer[]
            {

 new Customer{CustomerID=1050,Name="Moldovan Andrei",BirthDate=DateTime.Parse("1980-07-10")},
 new Customer{CustomerID=1045,Name="Irimescu Ilie",BirthDate=DateTime.Parse("1985-01-08")},

 };
            foreach (Customer c in customers)
            {
                context.Customers.Add(c);
            }
            context.SaveChanges();
            var orders = new Order[]
            {
 new Order{InstrumentID=1,CustomerID=1050, OrderDate=DateTime.Parse("02-25-2020")},
 new Order{InstrumentID=3,CustomerID=1045, OrderDate=DateTime.Parse("09-28-2020")},
 new Order{InstrumentID=1,CustomerID=1045, OrderDate=DateTime.Parse("10-28-2020")},
 new Order{InstrumentID=2,CustomerID=1050, OrderDate=DateTime.Parse("09-28-2020")},
 new Order{InstrumentID=4,CustomerID=1050, OrderDate=DateTime.Parse("09-28-2020")},
 new Order{InstrumentID=6,CustomerID=1050, OrderDate=DateTime.Parse("10-28-2020")},
            };
            foreach (Order e in orders)
            {
                context.Orders.Add(e);
            }
            context.SaveChanges();

            var manufacturers = new Manufacturers[]
            {
                new Manufacturers{ManufacturersName="Andrei Ioan",Adress="Str. Bucegi, nr. 6,Cluj-Napoca"
                },

                new Manufacturers {ManufacturersName = "Popescu Mihai", Adress = "Str. Calea Floresti, nr. 2,Cluj-Napoca"
                },
                };
            foreach (Manufacturers p in manufacturers)
            {
                context.Manufacturers.Add(p);
            }
            context.SaveChanges();

            var manufacturedintruments = new ManufacturedInstruments[]
            {
                new ManufacturedInstruments
                { InstrumentID = instruments.Single(c => c.Type == "Guitar").ID,
                  ManufacturersID = manufacturers.Single(i => i.ManufacturersName == "Pop Andrei" ).ID
                },
                new ManufacturedInstruments
                { InstrumentID = instruments.Single(c => c.Type == "Flute").ID,
                  ManufacturersID = manufacturers.Single(i => i.ManufacturersName == "Popescu Mihai" ).ID
                },
                new ManufacturedInstruments
                { InstrumentID = instruments.Single(c => c.Type == "Violin").ID,
                  ManufacturersID = manufacturers.Single(i => i.ManufacturersName == "Pop Andrei" ).ID
                },
                new ManufacturedInstruments
                { InstrumentID = instruments.Single(c => c.Type == "Saxophone").ID,
                  ManufacturersID = manufacturers.Single(i => i.ManufacturersName == "Pop Andrei" ).ID
                },
                new ManufacturedInstruments
                { InstrumentID = instruments.Single(c => c.Type == "Piano").ID,
                  ManufacturersID = manufacturers.Single(i => i.ManufacturersName == "Popescu Mihai" ).ID
                },
                new ManufacturedInstruments
                { InstrumentID = instruments.Single(c => c.Type == "Xylophone").ID,
                  ManufacturersID = manufacturers.Single(i => i.ManufacturersName == "Popescu Mihai" ).ID
                },
            };
            foreach (ManufacturedInstruments pb in manufacturedintruments)
            {
                context.ManufacturedInstruments.Add(pb);
            }
            context.SaveChanges();
        }
    }
}
