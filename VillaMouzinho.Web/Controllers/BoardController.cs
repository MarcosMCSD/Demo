using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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

        [HttpPost]
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
        public JsonResult CreateUrlFriendly(string title)
        {
            return this.Json(NormalizeStringForUrl(title), JsonRequestBehavior.AllowGet);
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

        /// <summary>
        /// Aux methods
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string NormalizeStringForUrl(string url)
        {
            String normalizedString = url.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder(url.Length);

            foreach (char c in normalizedString)
            {
                switch (CharUnicodeInfo.GetUnicodeCategory(c))
                {
                    case UnicodeCategory.LowercaseLetter:
                    case UnicodeCategory.UppercaseLetter:
                        stringBuilder.Append(Char.ToLower(c));
                        break;
                    case UnicodeCategory.DecimalDigitNumber:
                        stringBuilder.Append(c);
                        break;
                    case UnicodeCategory.SpaceSeparator:
                    case UnicodeCategory.ConnectorPunctuation:
                    case UnicodeCategory.DashPunctuation:
                        stringBuilder.Append('-');
                        break;
                }
            }
            string result = stringBuilder.ToString();
            return String.Join("-", result.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries)); // remove duplicate underscores
        }
    }
}