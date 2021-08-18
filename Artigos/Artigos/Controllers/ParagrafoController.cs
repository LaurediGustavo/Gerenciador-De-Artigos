using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Artigos.Models;

namespace Artigos.Controllers
{
    [Authorize(Roles = "Administrador, Escritor")]
    public class ParagrafoController : Controller
    {
        private readonly Context db = new Context();
        public ActionResult Index()
        {
            return View();
        }
    }
}