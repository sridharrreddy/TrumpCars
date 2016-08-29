using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrumpCars.Models
{
    public class PlayerGameData
    {
        public string PlayerId { get; set; }
        public bool IsPlayersTurn { get; set; }
        public List<TrumpCard> TrumpCards { get; set; }
        public int Score { get; set; }
    }
}