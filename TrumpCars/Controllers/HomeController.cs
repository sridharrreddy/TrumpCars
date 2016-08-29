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
        public static GameData GameData = new GameData { MaxPlayers = 2, CardCount = 5, Groups = new List<GroupData>() };

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Game()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
    }
}