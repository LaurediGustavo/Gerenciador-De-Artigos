using Artigos.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Artigos.Controllers
{
    public class ArtigoController : Controller
    {
        private Context db = new Context();

        [HttpGet]
        [Authorize(Roles = "Escritor, Administrador")]
        public ActionResult Index()
        {
            int id = int.Parse(User.Identity.GetUserId());
            var artigos = db.Artigos.Where(a => a.EscritorId == id).ToList();
            return View(artigos);
        }
    }
}