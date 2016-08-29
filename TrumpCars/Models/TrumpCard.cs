using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrumpCars.Models
{
    public class TrumpCard
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public List<CarCharacteristic> CarCharacteristics { get; set; }
        public bool Finished { get; set; }
        public bool IsActive { get; set; }
        public bool Win { get; set; }
    }
}