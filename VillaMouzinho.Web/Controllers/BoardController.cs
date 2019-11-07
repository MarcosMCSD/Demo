using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VillaMouzinho.Web.Controllers
{
    [Authorize]
    public class BoardController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Dashboard";
            return View();
        }

        /// <summary>
        /// Attributes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Attributes()
        {
            ViewBag.Title = "Atributos";
            return View();
        }

        [HttpGet]
        public ActionResult CreateAttribute()
        {
            ViewBag.Title = "Criar atributo";
            return View();
        }
    }
}