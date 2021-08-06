﻿using Artigos.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
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
        public ActionResult Index()
        {
            int id = int.Parse(User.Identity.GetUserId());
            var artigos = db.Artigos.Where(a => a.EscritorId == id).ToList();
            return View(artigos);
        }

        [HttpGet]
        [Authorize(Roles = "Escritor, Administrador")]
        public ActionResult Create()
        {
            ModelViewArtigo artigos = new ModelViewArtigo();
            artigos.Categorias = db.Categorias.Where(c => c.Ativa == 1).ToList();

            return View(artigos);
        }

        [HttpPost]
        [Authorize(Roles = "Escritor, Administrador")]
        public ActionResult Create([Bind(Include = "Titulo,Capa")] ModelViewArtigo artigo)
        {
            if (ModelState.IsValid)
            {
                //artigo.EscritorId = int.Parse(User.Identity.GetUserId());

                //db.Artigos.Add(artigo);
                //db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(artigo);
        }
    }
}