﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VillaMouzinho.Web.Controllers
{
    [Authorize]
    public class BoardController : Controller
    {
        // GET: Board
        public ActionResult Index()
        {
            return View();
        }
    }
}