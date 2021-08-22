using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Artigos.Models;
using Microsoft.AspNet.Identity;

namespace Artigos.Controllers
{
    public class ImagemController : Controller
    {
        private readonly Context db = new Context();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int idAr, int idPa, HttpPostedFileBase img)
        {
            if (ValidarImg(img))
            {
                Artigo artigo = db.Artigos.Find(idAr);
                Paragrafo paragrafo = db.Paragrafos.Find(idPa);
                int id = int.Parse(User.Identity.GetUserId());

                if (id == artigo.EscritorId || User.IsInRole("Administrador"))
                {
                    if (paragrafo.ArtigoId == artigo.Id)
                    {
                        BinaryReader imgByte = new BinaryReader(img.InputStream);
                        db.Imagems.Add(new Imagem() 
                        { 
                            ParagrafoId = idPa,
                            Img = imgByte.ReadBytes(img.ContentLength)
                        });
                        db.SaveChanges();

                        return Json("Imagem cadastrada!");
                    }
                }   
            }

            return Json("Selecione uma imagem válida!");
        }

        private Boolean ValidarImg(HttpPostedFileBase img)
        {
            var ex = new string[]
            {
                "image/gif",
                "image/png",
                "image/jpg"
            };

            if (!ex.Contains(img.FileName) && img.ContentLength > 200000)
            {
                return false;
            }

            return true;
        }
    }
}