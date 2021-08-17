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
            var artigos = (from a in db.Artigos
                       join u in db.Escritores on
                       a.EscritorId equals u.Id
                       select new 
                       {
                            a.Id,
                            a.EscritorId,
                            a.Titulo,
                            a.Capa,
                            a.Ativo,
                            u.Nome,
                            u.Sobrenome
                       }).OrderByDescending(a => a.Id).ToList();

            if (User.IsInRole("Escritor"))
            {
                int id = int.Parse(User.Identity.GetUserId());
                artigos = artigos.Where(a => a.EscritorId == id).ToList();
            }

            if (!String.IsNullOrEmpty(nome))
            {
                artigos = artigos.Where(a => a.Titulo.Contains(nome)).ToList();
            }

            List<ModelViewArtigoEscritor> artigo = new List<ModelViewArtigoEscritor>(); ;
            foreach(var item in artigos)
            {
                artigo.Add(new ModelViewArtigoEscritor()
                {
                    Id = item.Id,
                    EscritorId = item.EscritorId,
                    Titulo = item.Titulo,
                    Capa = item.Capa,
                    Ativo = item.Ativo,
                    Nome = item.Nome,
                    Sobrenome = item.Sobrenome
                });
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);

            return View(artigo.ToPagedList(pageNumber, pageSize));
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
        [ValidateAntiForgeryToken]
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

        [HttpGet]
        [Authorize(Roles = "Escritor, Administrador")]
        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Artigo artigo = this.Artigo(id.Value);
            if (artigo == null)
            {
                return HttpNotFound();
            }

            if (artigo.EscritorId != int.Parse(User.Identity.GetUserId()))
            {
                return HttpNotFound();
            }

            return View(new ModelViewArtigoEdicao() 
            { 
                Id = artigo.Id,
                Titulo = artigo.Titulo,
                Capa = artigo.Capa,
                Ativa = (artigo.Ativo == 1)? true : false,
                Categorias = db.Categorias.Where(c => c.Ativa == 1).ToList()
            });
        }

        public Artigo Artigo(int id)
        {
            Artigo artigo = db.Artigos.Find(id);
            return artigo;
        }

        [HttpPost]
        [Authorize(Roles = "Escritor, Administrador")]
        [ValidateAntiForgeryToken]
        public ActionResult Update([Bind (Include = "Id,Titulo")]ModelViewArtigoEdicao artigo, HttpPostedFileBase Image, bool Ativa)
        {
            Artigo ar = this.Artigo(artigo.Id);
            artigo.Capa = ar.Capa;
            artigo.Categorias = db.Categorias.Where(c => c.Ativa == 1).ToList();

            if (ModelState.IsValid)
            {
                if (Image != null)
                {
                    bool img = this.ValidarImg(Image);

                    if (!img)
                    {
                        ModelState.AddModelError("Image", "As extensões permitidas são: jpg, png, gif e o tamanho máximo é de 2mb");
                        return View(artigo);
                    }

                    BinaryReader binary = new BinaryReader(Image.InputStream);
                    ar.Capa = binary.ReadBytes(Image.ContentLength);
                }

                ar.Titulo = artigo.Titulo;
                ar.Ativo = (Ativa) ? 1 : 0;

                db.Entry(ar).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Update", ar.Id);
            }

            return View(artigo);
        }

        [HttpGet]
        [Authorize(Roles = "Escritor, Administrador")]
        public JsonResult PegarCategorias(int? id)
        {
            if (id == null)
            {
                return Json("Id inválido!");
            }

            Artigo artigo = db.Artigos.Find(id);
            if (artigo == null)
            {
                return Json("Id inválido!");
            }

            if (artigo.EscritorId != int.Parse(User.Identity.GetUserId()))
            {
                return Json("Id inválido!");
            }

            var categorias = (from c in db.Categorias join
                                a in db.ArtigoCategorias on
                                c.Id equals a.CategoriaId where
                                a.ArtigoId == id
                                select new
                                {
                                    c.Id,
                                    c.NomeCategoria
                                }).ToList();

            return Json(categorias, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "Escritor, Administrador")]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateCategoria(int? id, List<Categoria> categorias)
        {
            if (id == null)
            {
                return Json("Selecione os dados corretamente!");
            }

            Artigo artigo = db.Artigos.Find(id);
            if (artigo == null)
            {
                return Json("Selecione os dados corretamente!");
            }

            if (artigo.EscritorId != int.Parse(User.Identity.GetUserId()))
            {
                return Json("Selecione os dados corretamente!");
            }
            else
            {

                this.ValidarCategoria(new ModelViewArtigoEdicaoCategoria() 
                { 
                    Id = id.Value,
                    Categorias = categorias
                });
                return Json("Categorias Atualizadas com sucesso!");
            }
        }

        //Adiciona ou remove as categorias
        private void ValidarCategoria(ModelViewArtigoEdicaoCategoria artigo)
        {
            var categorias = (from c in db.Categorias join
                a in db.ArtigoCategorias on
                c.Id equals a.CategoriaId where
                a.ArtigoId == artigo.Id
                select new
                {
                    c.Id,
                }).ToList();

            if (artigo.Categorias != null)
            {
                bool c;

                foreach (var item in categorias)
                {
                    c = true;
                    foreach (var i in artigo.Categorias)
                    {
                        if (item.Id == i.Id)
                        {
                            c = false;
                            break;
                        }
                    }

                    if (c)
                    {
                        ArtigoCategoria categoria = db.ArtigoCategorias.Where(a => a.ArtigoId == artigo.Id && a.CategoriaId == item.Id).FirstOrDefault();
                        db.ArtigoCategorias.Remove(categoria);
                    }
                }

                foreach (var item in artigo.Categorias)
                {
                    c = true;
                    foreach (var i in categorias)
                    {
                        if (item.Id == i.Id)
                        {
                            c = false;
                            break;
                        }
                    }

                    if (c)
                    {
                        db.ArtigoCategorias.Add(new ArtigoCategoria() 
                        { 
                            ArtigoId = artigo.Id,
                            CategoriaId = item.Id
                        });
                    }
                }

                db.SaveChanges();
            }
        }
    }
}

