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

        [HttpPost] // Ouriço para a demo vai só com 2 valores.
        public ActionResult CreateAttribute(string name, string value1, string description1, string value2, string description2)
        {
            return RedirectToAction("Attributes", "Board", new { Message = "Atributo criado com sucesso" });
        }

        [HttpGet]
        public ActionResult UpdateAttribute()
        {
            ViewBag.Title = "Editar atributo";
            return View();
        }

        [HttpPost]
        public ActionResult UpdateAttribute(int id, string name, int idvalue1, string value1, string description1,
            int idvalue2, string value2, string descriptionvalue2)
        {
            return RedirectToAction("Attributes", "Board", new { Message = "Atributo editado com sucesso" });
        }

        /// <summary>
        /// CMS Content
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreatePage()
        {
            ViewBag.Title = "Criar página";
            return View();
        }

        [HttpGet]
        public ActionResult GetModule(int id)
        {
            if (id == 1)
            {
                ViewBag.RefModule = Guid.NewGuid();
                return PartialView("~/Views/Board/Modules/Create/_Content.cshtml");
            }
            else
            {
                return PartialView("~/Views/Board/Modules/Create/_Content.cshtml");
            }
        }
    }
}