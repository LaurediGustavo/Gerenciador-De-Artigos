using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Artigos.Models;
using Microsoft.AspNet.Identity;

namespace Artigos.Controllers
{
    [Authorize(Roles = "Administrador, Escritor")]
    public class ParagrafoController : Controller
    {
        private readonly Context db = new Context();
        
        [HttpGet]
        public ActionResult Index(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        [HttpGet]
        public ActionResult GetAll(int id)
        {
            int idUser = int.Parse(User.Identity.GetUserId().ToString());
            Artigo artigo = db.Artigos.Find(id);

            if(idUser == artigo.EscritorId || User.IsInRole("Administrador"))
            {
                var artigosId = (from p in db.Paragrafos
                                 where p.ArtigoId == id
                                 select new
                                 {
                                     p.Id,
                                     p.ArtigoId
                                 }).ToList();

                return Json(artigosId, JsonRequestBehavior.AllowGet);
            }

            return Json("");
        }

        [HttpGet]
        public ActionResult Details(int idPa, int idAr)
        {
            int idUser = int.Parse(User.Identity.GetUserId().ToString());
            Artigo artigo = db.Artigos.Find(idAr);
            Paragrafo paragrafo = db.Paragrafos.Find(idPa);

            if ((paragrafo.ArtigoId == artigo.Id && idUser == artigo.EscritorId) || User.IsInRole("Administrador"))
            {
                return Json(paragrafo, JsonRequestBehavior.AllowGet);
            }

            return Json("");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, string texto)
        {
            int idUser = int.Parse(User.Identity.GetUserId().ToString());
            Artigo artigo = db.Artigos.Find(id);

            if ((idUser == artigo.EscritorId || User.IsInRole("Administrador")) && !String.IsNullOrEmpty(texto))
            {
                db.Paragrafos.Add(new Paragrafo() 
                {  
                    ArtigoId = id,
                    Texto = texto
                });
                db.SaveChanges();

                return Json("Paragrafo adicionado!");
            }

            return Json("");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int idPa, int idAr, string texto)
        {
            int idUser = int.Parse(User.Identity.GetUserId().ToString());
            Artigo artigo = db.Artigos.Find(idAr);
            Paragrafo paragrafo = db.Paragrafos.Find(idPa);

            if (!String.IsNullOrEmpty(texto))
            {
                if ((paragrafo.ArtigoId == artigo.Id && idUser == artigo.EscritorId) || User.IsInRole("Administrador"))
                {
                    paragrafo.Texto = texto;

                    db.Entry(paragrafo).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    return Json("Paragrafo atualizado!");
                }
            }

            return Json("");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int idPa, int idAr)
        {
            int idUser = int.Parse(User.Identity.GetUserId().ToString());
            Artigo artigo = db.Artigos.Find(idAr);
            Paragrafo paragrafo = db.Paragrafos.Find(idPa);

            if ((paragrafo.ArtigoId == artigo.Id && idUser == artigo.EscritorId) || User.IsInRole("Administrador"))
            {
                foreach (var item in paragrafo.Imagems)
                {
                    db.Imagems.Remove(item);
                }

                db.Paragrafos.Remove(paragrafo);
                db.SaveChanges();

                return Json("Paragrafo excluido!");
            }

            return Json("");
        }
    }
}