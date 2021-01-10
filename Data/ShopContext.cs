using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tomi_Ionel.Models;

namespace Tomi_Ionel.Data
{
    public class ShopContext: DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options) :
base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Instrument> Instruments { get; set; }

        public DbSet<Manufacturers> Manufacturers { get; set; }
        public DbSet<ManufacturedInstruments> ManufacturedInstruments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<Instrument>().ToTable("Instrument");
            modelBuilder.Entity<Manufacturers>().ToTable("Manufacturers");
            modelBuilder.Entity<ManufacturedInstruments>().ToTable("ManufacturedIntruments");
            modelBuilder.Entity<ManufacturedInstruments>()
            .HasKey(c => new { c.InstrumentID, c.ManufacturersID });//configureaza cheia primara compusa
        }
    }
}
