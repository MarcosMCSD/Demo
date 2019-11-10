using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using VillaMouzinho.Business.DB;
using VillaMouzinho.Web.Controllers.Core;
using VillaMouzinho.Web.Models;
using VillaMouzinho.Web.Models.Board.CMS;

namespace VillaMouzinho.Web.Controllers
{
    [Authorize]
    public class BoardController : CoreController
    {
        private dbEntities db = new dbEntities();

        [HttpGet]
        public ActionResult Index()
        {
            FullBreadcrumb.Add("Home|Board/Index");
            FullBreadcrumb.Add("Dashboard|Board/Index");
            BuildFastBreadCrumb(FullBreadcrumb, 1);

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
            FullBreadcrumb.Add("Home|Board/Index");
            FullBreadcrumb.Add("Atributos|Board/Attributes");
            BuildFastBreadCrumb(FullBreadcrumb, 1);

            ViewBag.Title = "Atributos";
            var model = new AttributesModel();
            model.Attributes = db.attributes.ToList();

            return View(model);
        }

        [HttpGet]
        public ActionResult CreateAttribute()
        {
            FullBreadcrumb.Add("Home|Board/Index");
            FullBreadcrumb.Add("Atributos|Board/Attributes");
            FullBreadcrumb.Add("Criar atributo|Board/CreateAttribute");
            BuildFastBreadCrumb(FullBreadcrumb, 1);

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

            FullBreadcrumb.Add("Home|Board/Index");
            FullBreadcrumb.Add("Atributos|Board/Attributes");
            FullBreadcrumb.Add("Editar atributo " + model.Attribute.NAME + "|Board/UpdateAttribute/" + id);
            BuildFastBreadCrumb(FullBreadcrumb, 1);

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

            FullBreadcrumb.Add("Home|Board/Index");
            FullBreadcrumb.Add("Conteúdos|#");
            FullBreadcrumb.Add("Criar Página|Board/CreatePage");
            BuildFastBreadCrumb(FullBreadcrumb, 1);

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreatePage(CreatePageModel model)
        {
            var page = new cms_page_header();
            page.ID = !string.IsNullOrEmpty(model.pageHeader.id) ? model.pageHeader.id : Guid.NewGuid().ToString();
            page.TITLE = model.pageHeader.title;
            page.STATUS = model.pageHeader.status;
            page.TYPE = model.pageHeader.type;            
            page.ACTIVE = true;
            page.CREATION_DATE = DateTime.Now;
            if (!string.IsNullOrEmpty(model.pageHeader.location))
            {
                var parent = db.cms_page_header.Where(x => x.URL == "/pt" + model.pageHeader.location).FirstOrDefault();
                page.PARENT_ID = parent.ID;
                page.PARENT_PATH = parent.PARENT_PATH + "|" + parent.ID;
                page.URL = "/pt" + parent.URL + "/" + model.pageHeader.url;
            }
            else
            {
                page.URL = "/pt" + model.pageHeader.url;
            }

            // Check unique URL
            var uniqueUrl = db.cms_page_header.Where(x => x.URL == page.URL).FirstOrDefault();
            if (uniqueUrl != null && string.IsNullOrEmpty(model.pageHeader.id))
            {
                return Json(new { result = "error-url" }, JsonRequestBehavior.AllowGet);
            }

            db.cms_page_header.Add(page);
            db.SaveChanges();

            var thisPage = db.cms_page_header.OrderByDescending(x => x.IID).FirstOrDefault();

            if (model.modules != null && model.modules.Any())
            {
                var counter = 0;
                foreach (var item in model.modules)
                {
                    counter++;
                    if (item.type == "content-module")
                    {
                        var contentModule = new cms_page_module_content();
                        contentModule.TITLE = item.title;
                        contentModule.DESCRIPTION = item.description;
                        if (!string.IsNullOrEmpty(item.image))
                        {
                            contentModule.IMAGE_NAME = item.imageName;
                            contentModule.IMAGE = Convert.FromBase64String(item.image.Split(',')[1]);
                            contentModule.EXTENSION = "." + item.image.Split('/')[1].Split(';')[0];
                        }

                        db.cms_page_module_content.Add(contentModule);
                        db.SaveChanges();

                        var thisContentModuleId = db.cms_page_module_content.OrderByDescending(x => x.ID).FirstOrDefault().ID;

                        var contentModuleMapping = new cms_page_module_mapping();
                        contentModuleMapping.PAGE_HEADER_ID = thisPage.ID;
                        contentModuleMapping.PAGE_HEADER_IID = thisPage.IID;
                        contentModuleMapping.TYPE = item.type;
                        contentModuleMapping.MODULE_ID = thisContentModuleId;
                        contentModuleMapping.ORDER = counter;
                        db.cms_page_module_mapping.Add(contentModuleMapping);
                        db.SaveChanges();
                    }
                    else if(item.type == "gallery-module" && item.images != null && item.images.Any())
                    {
                        var galleryModule = new cms_page_module_gallery();
                        galleryModule.CREATION_DATE = DateTime.Now;
                        db.cms_page_module_gallery.Add(galleryModule);
                        db.SaveChanges();

                        var thisGalleryModuleId = db.cms_page_module_gallery.OrderByDescending(x => x.ID).FirstOrDefault().ID;

                        foreach (var image in item.images)
                        {
                            var galleryImage = new cms_page_module_gallery_items();
                            galleryImage.IMAGE_NAME = image.name;
                            galleryImage.IMAGE = Convert.FromBase64String(image.image.Split(',')[1]);
                            galleryImage.EXTENSON = "." + image.image.Split('/')[1].Split(';')[0];
                            galleryImage.GALLERY_ID = thisGalleryModuleId;
                            db.cms_page_module_gallery_items.Add(galleryImage);                            
                        }
                        db.SaveChanges();

                        var galleryModuleMapping = new cms_page_module_mapping();
                        galleryModuleMapping.PAGE_HEADER_ID = thisPage.ID;
                        galleryModuleMapping.PAGE_HEADER_IID = thisPage.IID;
                        galleryModuleMapping.TYPE = item.type;
                        galleryModuleMapping.MODULE_ID = thisGalleryModuleId;
                        galleryModuleMapping.ORDER = counter;
                        db.cms_page_module_mapping.Add(galleryModuleMapping);
                        db.SaveChanges();
                    }
                    else if(item.type == "room-module")
                    {
                        var roomModule = new cms_page_module_room();
                        roomModule.TITLE = item.title;
                        roomModule.DESCRIPTION = item.description;
                        if (!string.IsNullOrEmpty(item.image))
                        {
                            roomModule.IMAGE_NAME = item.imageName;
                            roomModule.IMAGE = Convert.FromBase64String(item.image.Split(',')[1]);
                            roomModule.EXTENSION = "." + item.image.Split('/')[1].Split(';')[0];
                        }

                        if (!string.IsNullOrEmpty(item.higherPrice))
                        {
                            try
                            {
                                roomModule.HIGHER_PRICE = decimal.Parse(item.higherPrice.Replace('.', ','));
                            }
                            catch (Exception)
                            {                                
                            }
                        }

                        if (!string.IsNullOrEmpty(item.lowerPrice))
                        {
                            try
                            {
                                roomModule.LOWER_PRICE = decimal.Parse(item.lowerPrice.Replace('.', ','));
                            }
                            catch (Exception)
                            {
                            }
                        }

                        db.cms_page_module_room.Add(roomModule);
                        db.SaveChanges();

                        var thisRoomModuleId = db.cms_page_module_room.OrderByDescending(x => x.ID).FirstOrDefault().ID;

                        if (item.attributes != null && item.attributes.Any())
                        {
                            foreach (var attr in item.attributes)
                            {
                                var roomAttribute = new cms_page_module_room_attributes();
                                roomAttribute.ROOM_ID = thisRoomModuleId;
                                roomAttribute.ATTRIBUTE_ID = attr.id;
                                roomAttribute.ATTRIBUTE_VALUE_ID = attr.valueId;
                                db.cms_page_module_room_attributes.Add(roomAttribute);
                            }
                            db.SaveChanges();
                        }

                        var roomModuleMapping = new cms_page_module_mapping();
                        roomModuleMapping.PAGE_HEADER_ID = thisPage.ID;
                        roomModuleMapping.PAGE_HEADER_IID = thisPage.IID;
                        roomModuleMapping.TYPE = item.type;
                        roomModuleMapping.MODULE_ID = thisRoomModuleId;
                        roomModuleMapping.ORDER = counter;
                        db.cms_page_module_mapping.Add(roomModuleMapping);
                        db.SaveChanges();
                    }
                }
            }

            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CreateUrlFriendly(string title)
        {
            return this.Json(NormalizeStringForUrl(title), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetModule(int id)
        {
            ViewBag.RefModule = Guid.NewGuid();
            if (id == 1)
            {               
                return PartialView("~/Views/Board/Modules/Create/_Content.cshtml");
            }
            else if (id == 2)
            {
                var Model = new AttributesModel();
                Model.Attributes = db.attributes.ToList();

                return PartialView("~/Views/Board/Modules/Create/_Room.cshtml", Model);
            }
            else
            {
                return PartialView("~/Views/Board/Modules/Create/_Gallery.cshtml");
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