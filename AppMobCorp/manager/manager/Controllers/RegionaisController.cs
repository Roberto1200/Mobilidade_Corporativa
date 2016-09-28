using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manager.Repository;
using PagedList;

namespace Manager.Controllers
{    
    public class RegionaisController : Controller
    {

        RegionaisRepository Repo = new RegionaisRepository();
        DiretoriasRepository RepoDiretorias = new DiretoriasRepository();

        //
        // GET: /Regionais/

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
            var ListarRegionais = Repo.List();
            int paginaTamanho = 10;
            int paginaNumero = (pagina ?? 1);
            var procurar = from s in ListarRegionais select s;
            ViewBag.Procura = procura;
            if (!String.IsNullOrEmpty(procura))
            {
                procurar = ListarRegionais.Where(s => s.regional.ToLower().Contains(procura.ToLower()) || s.diretoria.ToLower().Contains(procura.ToLower()));
            }
            return View(procurar.ToPagedList(paginaNumero, paginaTamanho));
        }

        public ActionResult Edit(int id)
        {

            ViewBag.Diretorias = RepoDiretorias.List();

            Models.Regional Model = Repo.List()
                                        .Where(r => r.id == id)
                                        .FirstOrDefault();

            return View(Model);
        }

        public ActionResult Update(Models.Regional Model)
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
