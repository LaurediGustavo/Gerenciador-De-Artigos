using Artigos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Artigos.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class CategoriaController : Controller
    {
        private Context db = new Context();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}