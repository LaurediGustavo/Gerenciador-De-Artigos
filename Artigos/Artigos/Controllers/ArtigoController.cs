using Artigos.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Artigos.Controllers
{
    [Authorize]
    public class ArtigoController : Controller
    {
        private Context db = new Context();

        [HttpGet]
        [Authorize(Roles = "Escritor, Administrador")]
        public ActionResult Index(int? page, string nome)
        {
            int id = int.Parse(User.Identity.GetUserId());
            var artigos = db.Artigos.OrderByDescending(a => a.Id).Where(a => a.EscritorId == id).ToList();

            if (!String.IsNullOrEmpty(nome))
            {
                artigos = artigos.Where(a => a.Titulo.Contains(nome)).ToList();
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);

            return View(artigos.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        [Authorize(Roles = "Escritor, Administrador")]
        public ActionResult Create()
        {
            ModelViewArtigo artigos = new ModelViewArtigo();
            return View(artigos);
        }

        [HttpPost]
        [Authorize(Roles = "Escritor, Administrador")]
        public ActionResult Create([Bind(Include = "Titulo,Image")] ModelViewArtigo artigo, bool Ativa)
        {
            if (ModelState.IsValid)
            {
                if (this.ValidarImg(artigo.Image))
                {
                    BinaryReader binary = new BinaryReader(artigo.Image.InputStream);

                    db.Artigos.Add(new Artigo()
                    {
                        EscritorId = int.Parse(User.Identity.GetUserId()),
                        Titulo = artigo.Titulo,
                        Capa = binary.ReadBytes(artigo.Image.ContentLength),
                        Ativo = (Ativa) ? 1 : 0
                    });
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Image", "As extensões permitidas são: jpg, png, gif e o tamanho máximo é de 2mb");
                }
            }

            return View(artigo);
        }

        private bool ValidarImg(HttpPostedFileBase Img)
        {
            var ext = new string[]
            {
                "image/gif",
                "image/png",
                "image/jpg"
            };

            if (!ext.Contains(Img.ContentType) || Img.ContentLength > 200000)
            {
                return false;
            }

            return true;
        }
    }
}