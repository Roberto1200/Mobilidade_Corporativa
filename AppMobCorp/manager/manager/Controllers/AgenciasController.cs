using Manager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace Manager.Controllers
{
    public class AgenciasController : Controller
    {
        AgenciasRepository Repo = new AgenciasRepository();
        RegionaisRepository RepoRegionais = new RegionaisRepository();

        //
        // GET: /Agencias/

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
            var ListarAgengias = Repo.List();
            int paginaTamanho = 10;
            int paginaNumero = (pagina ?? 1);
            var procurar = from s in ListarAgengias select s;
            ViewBag.Procura = procura;
            if (!String.IsNullOrEmpty(procura))
            {
                procurar = ListarAgengias.Where(s => s.agencia.ToLower().Contains(procura.ToLower()) || s.regional.ToLower().Contains(procura.ToLower()));
            }
            return View(procurar.ToPagedList(paginaNumero, paginaTamanho));
        }

        public ActionResult Edit(int id)
        {

            ViewBag.Regionais = RepoRegionais.List()
                                             .Where(r => r.id_diretoria > 0)
                                             .ToList();

            Models.Agencias Model = Repo.List()
                                        .Where(a => a.id == id)
                                        .FirstOrDefault();

            return View(Model);
        }

        public ActionResult Update(Models.Agencias Model)
        {
            string actionName = "Index";

            if (ModelState.IsValid)
            {
                if (!Repo.InsertUpdate(Model))
                    actionName = string.Format("Edit/{0}", Model.id);
            }
            else
            {
                actionName = string.Format("Edit/{0}", Model.id);
            }

            return RedirectToAction(actionName);
        }

        public ActionResult Delete(int id)
        {
            Repo.Delete(id);
            return RedirectToAction("Index");
        }

    }
}
