namespace TrumpCars.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VehicleCharacteristic
    {
        public int Id { get; set; }

        public int VehicleId { get; set; }

        public int CharacteristicId { get; set; }

        public int Value { get; set; }

        public virtual Characteristic Characteristic { get; set; }

        public virtual Vehicle Vehicle { get; set; }
    }
}
