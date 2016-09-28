using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using Manager.Filters;
using Manager.Models;
using System.Web.Http.Routing;
using System.Threading.Tasks;
using System.Net.Http;
using Manager.Helpers;
using System.Security.Cryptography;
using Manager.Repository;

namespace Manager.Controllers
{

    public class WSController : Controller
    {

        [HttpPost]
        [AllowAnonymousAttribute]
        public ActionResult RecAdminCS(ResetPasswordModel model, string senha, string senha2)
        {
            RecAdminSenha(model, "2");
            return View("ok");
        }
        [HttpPost]
        [AllowAnonymousAttribute]
        public ActionResult RecAdminSenha(ResetPasswordModel model, string tipo)
        {

            Manager.Helpers.LoginUser helpLogin = new Helpers.LoginUser();
            Helpers.LoginUser.LoginRetorno tmp = new Helpers.LoginUser.LoginRetorno();
            
            bool success = helpLogin.AtualizarToken("admin", model.Token, model.Email, model.NewPassword, null, model.Email, "2", ref tmp);

            if (success)
            {
                TempData["PasswordChanged"] = true;
                return Redirect("~/Account/Login/");
            }
            else
            {
                return View("Error");
            }

        }

        #region AJAX Password Retrieval workflow 
        [HttpGet]
        [AllowAnonymousAttribute]
        public ActionResult SendPasswordInstructions(string email)
        {

            Manager.Helpers.LoginUser helpLogin = new Helpers.LoginUser();
            Helpers.LoginUser.LoginRetorno tmp = new Helpers.LoginUser.LoginRetorno();
            string novoToken = Helpers.LoginUser.RandomString(5).ToString();

            bool updated =  helpLogin.AtualizarToken("admin", novoToken, email, null, null, email, "0", ref tmp);

            return Json(updated, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowAnonymousAttribute]
        public ActionResult ValidateToken(string email, string token)
        {

            Manager.Helpers.LoginUser helpLogin = new Helpers.LoginUser();
            Helpers.LoginUser.LoginRetorno tmp = new Helpers.LoginUser.LoginRetorno();

            bool updated = helpLogin.AtualizarToken("admin", token, email, null, null, email, "1", ref tmp);

            return Json(updated, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetCartilhas()
        {

            Repository.CartilhasRepository cartilhas = new Repository.CartilhasRepository();
            List<Cartilha> lista = cartilhas.List("a");
            List<CartilhaWS> listaWS = new List<CartilhaWS>();
            foreach (Cartilha c in lista)
            {
                CartilhaWS cw = new CartilhaWS();
                cw.date = c.data;
                cw.id = c.id.ToString();
                cw.title = c.titulo;
                cw.urlPDF = "/Upload/Cartilhas/" + c.arquivo;
                cw.size = c.size;
                listaWS.Add(cw);
            }

            return Json(listaWS, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetDocumentosRegionais()
        {

            Repository.DocumentosRegionaisRepository documentoRegional = new Repository.DocumentosRegionaisRepository();
            List<DocumentosRegional> lista = documentoRegional.List("a");
            List<DocumentosRegionaisWS> listaWS = new List<DocumentosRegionaisWS>();
            foreach (DocumentosRegional c in lista)
            {
                DocumentosRegionaisWS cw = new DocumentosRegionaisWS();
                cw.date = c.data;
                cw.id = c.id.ToString();
                cw.title = c.titulo;
                cw.urlPDF = "/Upload/DocumentosRegionais/" + c.arquivo;
                cw.size = c.size;
                listaWS.Add(cw);
            }

            return Json(listaWS, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetRelatorios()
        {

            Repository.RelatoriosRepository relatorios = new Repository.RelatoriosRepository();
            List<Relatorios> lista = relatorios.List("a");
            List<RelatoriosWS> listaWS = new List<RelatoriosWS>();
            foreach (Relatorios c in lista)
            {
                RelatoriosWS cw = new RelatoriosWS();
                cw.date = c.data;
                cw.id = c.id.ToString();
                cw.title = c.titulo;
                cw.urlPDF = "/Upload/Relatorios/" + c.arquivo;
                cw.size = c.size;
                listaWS.Add(cw);
            }

            return Json(listaWS, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetVideos()
        {

            Repository.VideosRepository videos = new Repository.VideosRepository();
            List<Video> lista = videos.List("a");
            List<VideoWS> listaWS = new List<VideoWS>();
            foreach (Video c in lista)
            {
                VideoWS cw = new VideoWS();
                cw.id = c.id.ToString();
                cw.title = c.titulo;
                cw.urlThumbnail = "/Upload/Videos/" + c.imagem;
                cw.url = "/Upload/Videos/" + c.video;
                cw.size = c.size;
                cw.duracao = c.duracao;
                listaWS.Add(cw);
            }

            return Json(listaWS, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetBannersAgentes()
        {

            Repository.BannerHomeRepository banners = new Repository.BannerHomeRepository();
            List<BannerHome> lista = banners.List("a");
            var listaEnviar = from s in lista select s;
            listaEnviar = lista.Where(b => b.id_tipo_app != 2);
            List<BannerWS> listaWS = new List<BannerWS>();
            foreach (BannerHome c in lista)
            {
                BannerWS cw = new BannerWS();
                cw.mensagem = c.texto;
                cw.urlBanner = "/Upload/Banners/" + c.imagem;
                listaWS.Add(cw);
            }

            return Json(listaWS, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ValidarToken(string token, string funcional)
        {

            Helpers.LoginUser tmp = new Helpers.LoginUser();
            bool status = tmp.ValidarTokenEFuncional(token, funcional);
            return Json(status);
        }


        [AllowAnonymous]
        [HttpGet]
        public ActionResult ValidarToken2(string token, string funcional)
        {

            Helpers.LoginUser tmp = new Helpers.LoginUser();
            bool status = tmp.ValidarTokenEFuncional(token, funcional);
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetBannersCoordenadores()
        {

            Repository.BannerHomeRepository banners = new Repository.BannerHomeRepository();
            List<BannerHome> lista = banners.List("a");
            var listaEnviar = from s in lista select s;
            listaEnviar = lista.Where(b => b.id_tipo_app != 1);
            List<BannerWS> listaWS = new List<BannerWS>();
            foreach (BannerHome c in listaEnviar)
            {
                BannerWS cw = new BannerWS();
                cw.mensagem = c.texto;
                cw.urlBanner = "/Upload/Banners/" + c.imagem;
                listaWS.Add(cw);
            }

            return Json(listaWS, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult Login(string funcional, string password)
        {

            Manager.Helpers.LoginUser wss = new Helpers.LoginUser();

            Usuario user = new Usuario();
            Helpers.LoginUser.LoginRetorno tmp = wss.ValidateUser("", password, funcional);

            if (tmp.status == true)
            {
                Repository.UsuarioseRepository usuarios = new Repository.UsuarioseRepository();
                user = usuarios.List().Where(p => p.codfuncional == funcional).FirstOrDefault();
                user.primeiroacesso = tmp.primeiro_acesso;
                user.status = true;

            }
            else
            {
                user.status = false;
            }
            user.senha = null;
            user.token = null;
            return Json(user, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult GetLogin(string funcional, string password)
        {
            Manager.Helpers.LoginUser wss = new Helpers.LoginUser();

                Usuario user = new Usuario();
            Helpers.LoginUser.LoginRetorno tmp = wss.ValidateUser("", password, funcional);
            if (tmp.status == true)
            {
                Repository.UsuarioseRepository usuarios = new Repository.UsuarioseRepository();
                user = usuarios.List().Where(p => p.codfuncional == funcional).FirstOrDefault();
                user.primeiroacesso = tmp.primeiro_acesso;
                user.status = true;

            }
            else
            {
                user.status = false;
            }
            user.senha = null;
            user.token = null;
            return Json(user);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult GetCoordenador(string funcional, string password)
        {
            Manager.Helpers.LoginUser wss = new Helpers.LoginUser();

            Usuario user = new Usuario();
            Helpers.LoginUser.LoginRetorno tmp = wss.ValidateCoord("", password, funcional);
            if (tmp.status == true)
            {
                Repository.UsuarioseRepository usuarios = new Repository.UsuarioseRepository();
                user = usuarios.List().Where(p => p.codfuncional == funcional).FirstOrDefault();
                user.primeiroacesso = tmp.primeiro_acesso;
                user.status = true;

            }
            else
            {
                user.status = false;
            }

            return Json(user);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult SetLogin(string token, string password)
        {

            Manager.Helpers.LoginUser wss = new Helpers.LoginUser();

            Repository.UsuarioseRepository Repo = new Repository.UsuarioseRepository();

            return Json(Repo.AlterarSenha(token, password));

        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult RecSenha(string codfunc, string tipo)
        {

            Manager.Repository.UsuarioseRepository RepoUsers = new Repository.UsuarioseRepository();
            Helpers.LoginUser.LoginRetorno tmp = new Helpers.LoginUser.LoginRetorno();
            Usuario user = new Usuario();
            Manager.Helpers.LoginUser helpLogin = new Helpers.LoginUser();
            if (Convert.ToInt32(tipo) == 1)
            {
                user = RepoUsers.List().Where(p => p.codfuncional == codfunc).ToList().Where(p => p.tipo == Convert.ToInt32(tipo)).FirstOrDefault();
            }
            else if (Convert.ToInt32(tipo) == 2)
            {
                user = RepoUsers.List().Where(p => p.codfuncional == codfunc).ToList().Where(p => p.tipo == Convert.ToInt32(tipo)).FirstOrDefault();
            }
            if (!(user != null && !string.IsNullOrWhiteSpace(user.codfuncional)))
            {
                return Json(false);
            }
            string novoToken = Helpers.LoginUser.RandomString(5).ToString();
            tmp.status = true;
            tmp.primeiro_acesso = true;
            helpLogin.AtualizarToken(null, novoToken, null, null, user.codfuncional, user.email, "sim", ref tmp);
            user = RepoUsers.List().Where(p => p.codfuncional == codfunc).FirstOrDefault();
            user.status = true;
            user.senha = null;
            user.token = null;
            return Json(user);

        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetCanais()
        {
            var context = System.Web.HttpContext.Current;
            CanaisDigitaisRepository Repo = new CanaisDigitaisRepository();
            List<CanaisDigitais> canais = Repo.ListCanais();
            foreach (CanaisDigitais canaisN in canais)
            {
                if (!string.IsNullOrWhiteSpace(canaisN.imagem_banner))
                {
                    canaisN.imagem_banner = string.Format("{0}://{1}{2}{3}upload/banners/{4}",
                                        context.Request.Url.Scheme,
                                        context.Request.Url.Host,
                                        context.Request.Url.Port == 80
                                            ? string.Empty
                                            : ":" + context.Request.Url.Port,
                                        string.IsNullOrWhiteSpace(context.Request.ApplicationPath) ? string.Empty : context.Request.ApplicationPath + "/", canaisN.imagem_banner);
                }
            }
            return Json(canais, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult GetCanaisCategorias(string id)
        {
            int idInt = Convert.ToInt32(id);
            var context = System.Web.HttpContext.Current;
            CanaisDigitaisRepository Repo = new CanaisDigitaisRepository();
            List<CanaisDigitais_Subcat> categorias = Repo.ListSubcategoria(idInt);
            foreach (CanaisDigitais_Subcat categoriasN in categorias)
            {
                if (!string.IsNullOrWhiteSpace(categoriasN.imagem))
                {
                    categoriasN.imagem = string.Format("{0}://{1}{2}{3}upload/banners/{4}",
                                    context.Request.Url.Scheme,
                                    context.Request.Url.Host,
                                    context.Request.Url.Port == 80
                                        ? string.Empty
                                        : ":" + context.Request.Url.Port,
                                    string.IsNullOrWhiteSpace(context.Request.ApplicationPath) ? string.Empty : context.Request.ApplicationPath + "/", categoriasN.imagem);
                }
            }
            return Json(categorias);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult GetCanaisConteudos(string id)
        {
            int idInt = Convert.ToInt32(id);
            var context = System.Web.HttpContext.Current;
            CanaisDigitaisRepository Repo = new CanaisDigitaisRepository();
            List<CanaisDitais_subcat_conteudo> conteudo = Repo.ListConteudo(idInt);
            foreach (CanaisDitais_subcat_conteudo conteudoN in conteudo)
            {
                if (!string.IsNullOrWhiteSpace(conteudoN.banner))
                {
                    conteudoN.banner = string.Format("{0}://{1}{2}{3}upload/banners/{4}",
                                    context.Request.Url.Scheme,
                                    context.Request.Url.Host,
                                    context.Request.Url.Port == 80
                                        ? string.Empty
                                        : ":" + context.Request.Url.Port,
                                    string.IsNullOrWhiteSpace(context.Request.ApplicationPath) ? string.Empty : context.Request.ApplicationPath + "/", conteudoN.banner);
                }
            }
            return Json(conteudo);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetRegionais(string idDiretoria)
        {
            RegionaisRepository repoRegionais = new RegionaisRepository();
            int id = 0;

            if (int.TryParse(idDiretoria, out id))
                return Json(repoRegionais.ListByDiretoria(id), JsonRequestBehavior.AllowGet);


            return Json(null, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetAgendaEvento(string codFunc)
        {
            Agendas Repo = new Agendas();
            List<Agenda> calendarios = new List<Agenda>();
            calendarios = Repo.List(codFunc);
            return Json(calendarios);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult PostAgendaEvento(Agenda modelo)
        {
            Agendas Repo = new Agendas();
            bool resultado = Repo.InsertUpdate(modelo);
            return Json(resultado);
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetAgendaTipos()
        {
            Agendas Repo = new Agendas();
            List<AgendaTipo> AgendaTipo = new List<AgendaTipo>();
            AgendaTipo = Repo.ListTipos();
            return Json(AgendaTipo, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetForumCanais()
        {
            ForunsRepository Repo = new ForunsRepository();
            List<Forum_Canal> Forumcanal = new List<Forum_Canal>();
            Forumcanal = Repo.ListCanais();
            return Json(Forumcanal, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetForumTopicos(int id)
        {
            ForunsRepository Repo = new ForunsRepository();
            List<Forum_Canal_Topico> forumTopico = new List<Forum_Canal_Topico>();
            forumTopico = Repo.ListTopicos(id);
            return Json(forumTopico);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetForumPosts(int id)
        {
            var context = System.Web.HttpContext.Current;
            ForunsRepository Repo = new ForunsRepository();
            List<Forum_Canal_Topico_Post> Forumcanal = new List<Forum_Canal_Topico_Post>();
            Forumcanal = Repo.ListPosts(id);
            foreach (Forum_Canal_Topico_Post PostsN in Forumcanal)
            {
                if (!string.IsNullOrWhiteSpace(PostsN.avatar))
                {
                    PostsN.avatar = string.Format("{0}://{1}{2}{3}upload/avatar/{4}",
                                        context.Request.Url.Scheme,
                                        context.Request.Url.Host,
                                        context.Request.Url.Port == 80
                                            ? string.Empty
                                            : ":" + context.Request.Url.Port,
                                        string.IsNullOrWhiteSpace(context.Request.ApplicationPath) ? string.Empty : context.Request.ApplicationPath + "/", PostsN.avatar);
                }
            }
            return Json(Forumcanal);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult PostForumTopicos(Forum_Canal_Topico modelo)
        {
            ForunsRepository Repo = new ForunsRepository();
            if (Repo.InsertUpdateTopicos(modelo))
            {
                return Json("true");
            }
            else
            {
                return Json("false");
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult PostForumPosts(Forum_Canal_Topico_Post modelo)
        {
            ForunsRepository Repo = new ForunsRepository();
            if (Repo.InsertUpdatePosts(modelo))
            {
                return Json("true");
            }
            else
            {
                return Json("false");
            }
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            bool checkForAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
            || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
            if (1 == 2)
            {
                base.OnAuthorization(filterContext);
            }


        }

    }
}
