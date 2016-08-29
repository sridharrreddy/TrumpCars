using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrumpCars.Models
{
    public class CarCharacteristic
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public bool IsPicked { get; set; }
    }
}