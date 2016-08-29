using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrumpCars.Entities;
using TrumpCars.Models;

namespace TrumpCars.Controllers
{
    public class HomeController : Controller
    {
        private readonly TrumpCarDbContext _context = new TrumpCarDbContext();

        public static GameData GameData = new GameData { MaxPlayers = 2, CardCount = 10, Groups = new List<GroupData>() };

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Game()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public JsonResult InitGame(string groupName)
        {
            var cards = _context.Vehicles.Take(10).Select(v => new TrumpCard
            {
                Id = v.Id,
                Title = v.MakeModel,
                ImageUrl = v.ImageUrl,
                CarCharacteristics = v.VehicleCharacteristics.Select(vc => new CarCharacteristic
                {
                    Name = vc.Characteristic.Name,
                    Value = vc.Value
                }).ToList()
            }).ToList();
            return Json(cards, JsonRequestBehavior.AllowGet);
        }
    }
}