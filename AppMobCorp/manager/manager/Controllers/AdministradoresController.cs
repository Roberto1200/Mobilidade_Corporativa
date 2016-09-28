using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using PagedList;

namespace Manager.Controllers
{
    public class AdministradoresController : Controller
    {
        //
        // GET: /Cartilhas/
        Manager.Repository.AdministradoresRepository Repo = new Repository.AdministradoresRepository();

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
            var ListarAdministradores = Repo.List();
            int paginaTamanho = 10;
            int paginaNumero = (pagina ?? 1);
            var procurar = from s in ListarAdministradores select s;
            ViewBag.Procura = procura;
            if (!String.IsNullOrEmpty(procura))
            {
                procurar = ListarAdministradores.Where(s => s.nome.ToLower().Contains(procura.ToLower()) || s.email.ToLower().Contains(procura.ToLower()));
            }
            return View(procurar.ToPagedList(paginaNumero, paginaTamanho));
        }

        [HttpGet]
        public ActionResult CheckExistingEmail(string email, int id = 0)
        {

            bool exists = Repo.EmailExists(email, id);

            return Json(!exists, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckExistingNome(string nome, int id = 0)
        {
            bool exists = Repo.NomeExists(nome, id);

            return Json(!exists, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Update(Manager.Models.Administrador Modelo)
        {

            if (ModelState.IsValid)
            {
                Repo.InsertUpdate(Modelo); 
            }
            else
            {                
                return View("Edit", Modelo);
            }

            return RedirectToAction("Index");

        }

        
        public ActionResult Edit(int id)
        {
            Manager.Models.Administrador Model = new Models.Administrador  ();

            ViewBag.NewPasswordText = string.Empty;
            if (id > 0)
            {
                Model = Repo.Load("", "", id);
                Model.senha = null;
                
                ViewBag.Operacao = "Editar Registro";
                ViewBag.NewPasswordText = "Nova ";
            }
            else
            {
                ViewBag.Operacao = "Inserir Registro";
            }
            return View(Model);
        }

        public ActionResult Delete(int id)
        {
            Repo.Delete(id);
            return RedirectToAction("Index");
        }

    }
}
