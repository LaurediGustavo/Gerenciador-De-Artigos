using Artigos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Artigos.Controllers
{
    [Authorize]
    public class EscritorController : Controller
    {
        private Context db = new Context();

        //Método para realizar login
        private Boolean Login(string login, string senha)
        {
            //Pega somente o Id e Nome do escritor
            var usuario = (from e in db.Escritores
                           where e.Login == login &&
                           e.Senha == senha
                           select new
                           {
                               e.Id,
                               e.Nome,
                               e.NivelAcesso
                           }).FirstOrDefault();

            if (usuario != null)
            {
                //Adiciona as informações no identity
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuario.Nome),
                    new Claim(ClaimTypes.Role, usuario.NivelAcesso)
                }, "ApplicationCookie");

                //Realiza o Login do escritor
                var ctx = Request.GetOwinContext();
                var auth = ctx.Authentication;
                auth.SignIn(identity);

                return true;
            }

            return false;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            var escritor = new ModelViewEscritorLogin();
            return View(escritor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login([Bind (Include = "Login, Senha")]ModelViewEscritorLogin escritor)
        {
            if (ModelState.IsValid)
            {
                if (this.Login(escritor.Login, escritor.Senha)) {
                    return RedirectToAction("Index", "Artigo");
                }

                ModelState.AddModelError("", "Login ou senha inválida!");
            }

            return View(escritor);
        }

        [HttpGet]
        [Authorize(Roles = "Escritor, Administrador")]
        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var auth = ctx.Authentication;

            auth.SignOut("ApplicationCookie");
            return RedirectToAction("Index", "Home");
        }

        //Retorna os níveis de acesso
        public List<SelectListItem> Roles()
        {
            List<SelectListItem> roles = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Selecione um nível de acesso", Value = ""},
                new SelectListItem { Text = "Escritor", Value = "Escritor" },
                new SelectListItem { Text = "Administrador", Value = "Administrador" }
            };

            return roles;
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public ActionResult Register()
        {
            var escritor = new ModelViewEscritorCadastro();

            escritor.Items = this.Roles();
            return View(escritor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public ActionResult Register([Bind(Include = "Nome, Sobrenome, Login, Senha, Email, NivelAcesso")]ModelViewEscritorCadastro escritor)
        {
            escritor.Items = this.Roles();

            if (ModelState.IsValid)
            {
                string role = escritor.NivelAcesso;
                db.Escritores.Add(new Escritor() { Nome = escritor.Nome, Sobrenome = escritor.Sobrenome, 
                    Login = escritor.Login, Senha = escritor.Senha, Email = escritor.Email, 
                    NivelAcesso = escritor.NivelAcesso});
                db.SaveChanges();

                this.Login(escritor.Login, escritor.Senha);
                return RedirectToAction("Index", "Artigo");
            }

            return View(escritor);
        }

        [HttpGet]
        [Authorize(Roles = "Escritor, Administrador")]
        public ActionResult Update()
        {
            var escritor = db.Escritores.Find(int.Parse(User.Identity.GetUserId()));
            return View(new ModelViewEscritorUpdate() 
            {
                Nome = escritor.Nome,
                Sobrenome = escritor.Sobrenome,
                Login = escritor.Login,
                Email = escritor.Email
            });
        }

        [HttpPost]
        [Authorize(Roles = "Escritor, Administrador")]
        [ValidateAntiForgeryToken]
        public ActionResult Update([Bind(Include = "Nome, Sobrenome, Login, Email")]ModelViewEscritorUpdate escritor)
        {
            if (ModelState.IsValid)
            {
                Escritor es = db.Escritores.Find(int.Parse(User.Identity.GetUserId()));
                es.Nome = escritor.Nome;
                es.Sobrenome = escritor.Sobrenome;
                es.Login = escritor.Login;
                es.Email = escritor.Email;

                db.Entry(es).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return View();
            }

            return View(escritor);
        }

        [HttpPost]
        [Authorize(Roles = "Escritor, Administrador")]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePassword([Bind(Include = "SenhaAntiga, SenhaNova, SenhaConfirma")] EscritorSenha senha)
        {
            if (ModelState.IsValid)
            {
                Escritor escritor = db.Escritores.Find(int.Parse(User.Identity.GetUserId()));
                
                if (escritor.Senha == senha.SenhaAntiga)
                {
                    if (senha.SenhaNova == senha.SenhaConfirma)
                    {
                        escritor.Senha = senha.SenhaConfirma;

                        db.Entry(escritor).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        return Json("");
                    }
                }
            }

            return Json("Preencha os campos corretamente!");
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public ActionResult Index()
        {
            int id = int.Parse(User.Identity.GetUserId());
            var escritor = db.Escritores.Where(e => e.Id != id).ToList();

            return View(escritor);
        }
    }
}