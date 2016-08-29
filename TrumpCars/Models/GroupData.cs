using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrumpCars.Models
{
    public class GroupData
    {
        public string Name { get; set; }
        public List<PlayerGameData> Players { get; set; }
        public List<TrumpCard> TrumpCards { get; set; }
    }
}