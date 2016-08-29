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

        public Dictionary<string, List<TrumpCard>> gameData = new Dictionary<string, List<TrumpCard>>();

        public List<string> userQueue = new List<string>();

        public string roomName;

        public ActionResult Index()
        {
            var groupName = roomName;
            //If roomName is null, create a new room and keep this user in that room
            if (string.IsNullOrEmpty(roomName))
            {
                roomName = Guid.NewGuid().ToString();
                groupName = roomName;
            }
            else
            {
                roomName = string.Empty;
            }
            ViewData["RoomName"] = groupName;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

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
            if (gameData.ContainsKey(groupName))
            {
                gameData[groupName].AddRange(cards);
            }
            else
            {
                gameData.Add(groupName, cards);
            }
            return Json(cards, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCards()
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