namespace TrumpCars.Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TrumpCarDbContext : DbContext
    {
        public TrumpCarDbContext()
            : base("name=TrumpCarModel")
        {
        }

        public virtual DbSet<Characteristic> Characteristics { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }
        public virtual DbSet<VehicleCharacteristic> VehicleCharacteristics { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Characteristic>()
                .HasMany(e => e.VehicleCharacteristics)
                .WithRequired(e => e.Characteristic)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Vehicle>()
                .HasMany(e => e.VehicleCharacteristics)
                .WithRequired(e => e.Vehicle)
                .WillCascadeOnDelete(false);
        }
    }
}
