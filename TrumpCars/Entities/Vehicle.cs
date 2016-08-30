namespace TrumpCars.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Vehicle")]
    public partial class Vehicle
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vehicle()
        {
            VehicleCharacteristics = new HashSet<VehicleCharacteristic>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string MakeModel { get; set; }

        [Required]
        [StringLength(600)]
        public string ImageUrl { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VehicleCharacteristic> VehicleCharacteristics { get; set; }
    }
}
