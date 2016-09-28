using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manager.Models;
using Manager.Repository;
using PagedList;

namespace Manager.Controllers
{
    public class AgendaController : Controller
    {
        Agendas Repo = new Agendas();
        //
        // GET: /Calendario/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IndexTipos(int? pagina, string procura, string currentFilter)
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
            var ListarTipos = Repo.ListTipos();
            int paginaTamanho = 10;
            int paginaNumero = (pagina ?? 1);
            var procurar = from s in ListarTipos select s;
            ViewBag.Procura = procura;
            if (!String.IsNullOrEmpty(procura))
            {
                procurar = ListarTipos.Where(s => s.tipo.ToLower().Contains(procura.ToLower()));
            }
            return View("Tipos/index",procurar.ToPagedList(paginaNumero, paginaTamanho));
        }
        public ActionResult EditTipos(int id)
        {
            AgendaTipo Model = new AgendaTipo();

            if (id > 0)
            {
                Model = Repo.LoadTipos(id);
                ViewBag.Operacao = "Editar Registro";
            }
            else
            {
                ViewBag.Operacao = "Inserir Registro";
            }
            return View("Tipos/Edit",Model);
        }
        public ActionResult UpdateTipos(AgendaTipo modelo)
        {
            if (ModelState.IsValid)
            {
                Repo.InsertUpdateTipos(modelo);
                return RedirectToAction("Tipos/index");
            }
            return View("Tipos/Edit", modelo);
        }
        public ActionResult DeleteTipos(int id)
        {
            Repo.DeleteTipos(id);
            return RedirectToAction("Tipos/index");
        }


    }
}
