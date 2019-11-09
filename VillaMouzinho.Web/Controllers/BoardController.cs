using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VillaMouzinho.Business.DB;
using VillaMouzinho.Web.Models;

namespace VillaMouzinho.Web.Controllers
{
    [Authorize]
    public class BoardController : Controller
    {
        private dbEntities db = new dbEntities();

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
            var model = new AttributesModel();
            model.Attributes = db.attributes.ToList();

            return View(model);
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
            var attribute = new attributes() {
                NAME = name
            };
            db.attributes.Add(attribute);
            db.SaveChanges();

            var attributeValue1 = new attributes_values()
            {
                ATTRIBUTE_ID = attribute.ID,
                DESCRIPTION = description1,
                VALUE = value1
            };
            db.attributes_values.Add(attributeValue1);
            db.SaveChanges();

            var attributeValue2 = new attributes_values()
            {
                ATTRIBUTE_ID = attribute.ID,
                DESCRIPTION = description2,
                VALUE = value2
            };
            db.attributes_values.Add(attributeValue2);
            db.SaveChanges();

            return RedirectToAction("Attributes", "Board", new { Message = "Atributo criado com sucesso" });
        }

        [HttpGet]
        public ActionResult UpdateAttribute(int id)
        {
            ViewBag.Title = "Editar atributo";

            var model = new AttributesModel();
            model.Attribute = db.attributes.FirstOrDefault(x => x.ID == id);

            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateAttribute(int id, string name, int idvalue1, string value1, string description1,
            int idvalue2, string value2, string description2)
        {
            var attribute = db.attributes.FirstOrDefault(x => x.ID == id);

            if(attribute != null)
            {
                attribute.NAME = name;
                db.SaveChanges();

                var value1DB = db.attributes_values.FirstOrDefault(x => x.ID == idvalue1);

                if(value1DB != null)
                {
                    value1DB.VALUE = value1;
                    value1DB.DESCRIPTION = description1;
                    db.SaveChanges();
                }

                var value2DB = db.attributes_values.FirstOrDefault(x => x.ID == idvalue2);

                if (value2DB != null)
                {
                    value2DB.VALUE = value2;
                    value2DB.DESCRIPTION = description2;
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Attributes", "Board", new { Message = "Atributo editado com sucesso" });
        }
    }
}