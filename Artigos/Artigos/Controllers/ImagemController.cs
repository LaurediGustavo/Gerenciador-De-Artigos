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
        public ActionResult Create(int idAr, int idPa)
        {
            HttpPostedFileBase img = Request.Files[0];

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
            if (img == null)
            {
                return false;
            }

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

        [HttpGet]
        public ActionResult GetImg(int id)
        {
            var imagem = (from i in db.Imagems
                          where i.ParagrafoId == id
                          select new 
                          { 
                             i.Id,
                             i.ParagrafoId
                          });

            return Json(imagem, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDetailsImg(int id)
        {
            Imagem imagem = db.Imagems.Find(id);

            var base64 = Convert.ToBase64String(imagem.Img);
            var img = String.Format("data:image/gif;base64,{0}", base64);

            var i = new { Id = imagem.Id, Image = img };
            return Json(i, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int Id)
        {
            Imagem imagem = db.Imagems.Find(Id);

            if (imagem != null)
            {
                db.Imagems.Remove(imagem);
                db.SaveChanges();
            }

            return Json("");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int Id)
        {
            HttpPostedFileBase img = Request.Files[0];
            Imagem imagem = db.Imagems.Find(Id);

            if (imagem != null && img != null)
            {
                if (ValidarImg(img))
                {
                    BinaryReader binary = new BinaryReader(img.InputStream);
                    imagem.Img = binary.ReadBytes(img.ContentLength);

                    db.Entry(imagem).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }

            return Json("");
        }
    }
}