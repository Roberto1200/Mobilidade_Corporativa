using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace Manager.Controllers
{
    public class ForumController : Controller
    {
        Manager.Repository.ForunsRepository Repo = new Repository.ForunsRepository();
        //
        // GET: /Forum/

        public ActionResult Index(int? pagina, string procura, string currentFilter)
        {
            ModelState.Clear();
            if (procura != null)
            {
                pagina = 1;
            }
            else
            {
                procura = currentFilter;
            }
            var listarCanais = Repo.ListCanais();
            int paginaTamanho = 10;
            int paginaNumero = (pagina ?? 1);
            var procurar = from s in listarCanais select s;
            ViewBag.Procura = procura;
            if (!String.IsNullOrEmpty(procura))
            {
                procurar = listarCanais.Where(s => s.titulo.ToLower().Contains(procura.ToLower()));
            }
            return View(procurar.ToPagedList(paginaNumero, paginaTamanho));
        }
        public ActionResult IndexTopicos(int id, int? pagina, string procura, string currentFilter)
        {
            ModelState.Clear();
            var ListarTopicos = Repo.ListTopicos(id);
            int paginaTamanho = 10;
            int paginaNumero = (pagina ?? 1);
            var procurar = from s in ListarTopicos select s;
            ViewBag.Procura = procura;
            if (!String.IsNullOrEmpty(procura))
            {
                procurar = ListarTopicos.Where(s => s.titulo.ToLower().Contains(procura.ToLower()));
            }
            return View("Topicos/index",procurar.ToPagedList(paginaNumero, paginaTamanho));
        }
        public ActionResult IndexPosts(int id, int? pagina, string procura, string currentFilter)
        {
            ModelState.Clear();
            var ListarPosts = Repo.ListPosts(id);
            int paginaTamanho = 10;
            int paginaNumero = (pagina ?? 1);
            var procurar = from s in ListarPosts select s;
            ViewBag.Procura = procura;
            if (!String.IsNullOrEmpty(procura))
            {
                procurar = ListarPosts.Where(s => s.mensagem.ToLower().Contains(procura.ToLower()));
            }
            return View("Topicos/Posts/index", procurar.ToPagedList(paginaNumero, paginaTamanho));
        }
        public ActionResult Edit(int id)
        {
            Manager.Models.Forum_Canal Model = new Models.Forum_Canal();

            if (id > 0)
            {
                Model = Repo.LoadCanais(id);
                ViewBag.Operacao = "Editar Registro";
            }
            else
            {
                ViewBag.Operacao = "Inserir Registro";
            }
            return View(Model);
        }
        public ActionResult EditTopicos(int id, int idp)
        {
            Manager.Models.Forum_Canal_Topico Model = new Models.Forum_Canal_Topico();

            if (id > 0)
            {
                Model = Repo.LoadTopicos(id);
                ViewBag.Operacao = "Editar Registro";
            }
            else
            {
                Model.id_canal = idp;
                ViewBag.Operacao = "Inserir Registro";
            }
            return View("Topicos/Edit", Model);
        }
        public ActionResult EditPosts(int id, int idp)
        {
            Manager.Models.Forum_Canal_Topico_Post Model = new Models.Forum_Canal_Topico_Post();

            if (id > 0)
            {
                Model = Repo.LoadPosts(id);
                ViewBag.Operacao = "Editar Registro";
            }
            else
            {
                Model.id_topico = idp;
                ViewBag.Operacao = "Inserir Registro";
            }
            return View("Topicos/Posts/Edit", Model);
        }
        public ActionResult Update(Manager.Models.Forum_Canal Modelo)
        {
            if (ModelState.IsValid)
            {
                Repo.InsertUpdateCanal(Modelo);
                return RedirectToAction("Index");
            }
            else
            {
                return View("Edit", Modelo);
            }
        }
        public ActionResult UpdateTopicos(Manager.Models.Forum_Canal_Topico Modelo)
        {
            if (ModelState.IsValid)
            {
                Repo.InsertUpdateTopicos(Modelo);
                return RedirectToAction("Topicos/"+Modelo.id_canal);
            }
            else
            {
                return View("Topicos/Edit", Modelo);
            }
        }
        public ActionResult UpdatePosts(Manager.Models.Forum_Canal_Topico_Post Modelo)
        {
            if (ModelState.IsValid)
            {
                Repo.InsertUpdatePosts(Modelo);
                return RedirectToAction("Topicos/Posts/Index");
            }
            else
            {
                return View("Topicos/Posts/Edit", Modelo);
            }
        }
        public ActionResult Delete(int id)
        {
            Repo.Delete(id);
            return RedirectToAction("Index");
        }
        public ActionResult DeleteTopicos(int id, int idp)
        {
            Repo.DeleteTopicos(id);
            return RedirectToAction("Topicos/"+idp);
        }
        public ActionResult DeletePosts(int id, int idp)
        {
            Repo.DeletePosts(id);
            return RedirectToAction("Topicos/Posts/"+idp);
        }
    }

}

