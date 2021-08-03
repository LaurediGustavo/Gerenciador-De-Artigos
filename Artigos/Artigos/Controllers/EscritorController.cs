using Artigos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using System.Net;

namespace Artigos.Controllers
{
    [Authorize]
    public class EscritorController : Controller
    {
        private Context db = new Context();

        //Envia um email com a senha
        private void EnviarEmail(Escritor escritor)
        {
            using (MailMessage mm = new MailMessage("artigosacademicos1@gmail.com", escritor.Email))
            {
                String body =
                "<h1>Cadastro concluido!</h1>" +
                "<p>Olá, " + escritor.Nome + " " + escritor.Sobrenome + " a sua senha de acesso é: " + escritor.Senha + "</p>" +
                "<p></p>" +
                "<a href='https://localhost:44371/Escritor/Login'>Artigos Acadêmicos</a>";

                mm.IsBodyHtml = true;
                mm.Subject = "Senha de acesso";
                mm.Body = body;
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    NetworkCredential NetworkCred = new NetworkCredential("", "");
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                }
            }
        }

        //Cria uma senha aleatória
        private string GerarSenha()
        {
            const string CAIXA_BAIXA = "abcdefghijklmnopqrstuvwxyz";
            const string CAIXA_ALTA = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string NUMEROS = "0123456789";
            const string ESPECIAIS = "@#$%&";
            string senha = "";

            Random random = new Random();
            for (int i = 0; i < 2; i++)
            {
                senha += CAIXA_BAIXA[random.Next(0, CAIXA_BAIXA.Length - 1)];
                senha += CAIXA_ALTA[random.Next(0, CAIXA_ALTA.Length - 1)];
                senha += NUMEROS[random.Next(0, NUMEROS.Length - 1)];
                senha += ESPECIAIS[random.Next(0, ESPECIAIS.Length - 1)];
            }

            return senha;
        }

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
        public ActionResult Register([Bind(Include = "Nome, Sobrenome, Login, Email, NivelAcesso")]ModelViewEscritorCadastro escritor)
        {
            escritor.Items = this.Roles();

            if (ModelState.IsValid)
            {
                Escritor es = new Escritor()
                {
                    Nome = escritor.Nome,
                    Sobrenome = escritor.Sobrenome,
                    Login = escritor.Login,
                    Senha = this.GerarSenha(),
                    Email = escritor.Email,
                    NivelAcesso = escritor.NivelAcesso
                };

                db.Escritores.Add(es);
                db.SaveChanges();

                this.EnviarEmail(es);
                return RedirectToAction("Index");
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
        public ActionResult Index(string nome)
        {
            int id = int.Parse(User.Identity.GetUserId());
            var escritor = db.Escritores.Where(e => e.Id != id).ToList();

            if (!String.IsNullOrEmpty(nome))
            {
                escritor = escritor.Where(e => e.Nome.Contains(nome) || e.Sobrenome.Contains(nome)).ToList();
            }

            return View(escritor);
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public ActionResult Detalhes(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Escritor escritor = db.Escritores.Find(Id);
            if (escritor == null)
            {
                return HttpNotFound();
            }

            ViewBag.Items = this.Roles();
            return View(escritor);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public ActionResult Detalhes([Bind(Include = "Id,NivelAcesso")] ModelViewEscritorAcesso escritor)
        {
            if (ModelState.IsValid)
            {
                if (escritor == null)
                {
                    return Json("Informações Inválidas!");
                }

                Escritor es = db.Escritores.Find(escritor.Id);

                if (escritor == null)
                {
                    return Json("Informações Inválidas!");
                }

                es.NivelAcesso = escritor.NivelAcesso;
                db.Entry(es).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return Json("");
            }

            ViewBag.Items = this.Roles();
            return Json("Preencha os campos corretamente!");
        }
    }
}