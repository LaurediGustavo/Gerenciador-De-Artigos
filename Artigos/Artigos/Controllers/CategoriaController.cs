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
        public ActionResult Index(ModelViewCategoria categoria)
        {
            if (categoria.NomeCategoria == null)
            {
                ModelState.AddModelError("", "Escreva um nome");
            }

            return View(categoria);
        }

        [HttpGet]
        public ActionResult GetAll(string nome, bool ativa)
        {
            var categoria = db.Categorias.ToList();

            if (!String.IsNullOrEmpty(nome))
            {
                categoria = categoria.Where(c => c.NomeCategoria.Contains(nome)).ToList();
            }

            if (!ativa)
            {
                categoria = categoria.Where(c => c.Ativa == 1).ToList();
            }
            else
            {
                categoria = categoria.Where(c => c.Ativa == 0).ToList();
            }

            return Json(categoria, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind (Include = "NomeCategoria, Ativa")] ModelViewCategoria categoria)
        {
            if (ModelState.IsValid)
            {
                int ativa;
                if(categoria.Ativa)
                {
                    ativa = 1;
                }
                else
                {
                    ativa = 0;
                }

                db.Categorias.Add(new Categoria() { NomeCategoria = categoria.NomeCategoria, Ativa = ativa });
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index", categoria);
        }

        [HttpGet]
        public ActionResult Get(int? id)
        {
            var categoria = db.Categorias.Find(id);

            return Json(categoria, JsonRequestBehavior.AllowGet);
        }
    }
}