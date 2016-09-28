using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manager.Repository;
using PagedList;

namespace Manager.Controllers
{
    public class DiretoriasController : Controller
    {

        DiretoriasRepository Repo = new DiretoriasRepository();
        RegionaisRepository RepoRegionais = new RegionaisRepository();

        //
        // GET: /Diretoria/

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
            var ListarDiretorias = Repo.List();
            int paginaTamanho = 10;
            int paginaNumero = (pagina ?? 1);
            var procurar = from s in ListarDiretorias select s;
            ViewBag.Procura = procura;
            if (!String.IsNullOrEmpty(procura))
            {
                procurar = ListarDiretorias.Where(s => s.diretoria.ToLower().Contains(procura.ToLower()));
            }
            return View(procurar.ToPagedList(paginaNumero, paginaTamanho));
        }

        public ActionResult Edit(int id)
        {
            Manager.Models.Diretoria Model = new Models.Diretoria();

            if (id > 0)
            {
                Model = Repo.Load(id);
                
                ViewBag.Operacao = "Editar Registro";
                
            }
            else
            {
                ViewBag.Operacao = "Inserir Registro";
            }

            string options = "";

            List<Models.Regional> listRegionais = RepoRegionais.List();

            foreach (Models.Regional r in listRegionais)
            {
                string t = "";

                if (id > 0)
                {
                    if (Model.regionais.Where(p => p.id == r.id).Any())
                    {
                        t = "selected";
                    }
                }
                options += "<option " + t + "  value='" + r.id.ToString() + "' >" + r.regional + "</option>";
            }

            ViewBag.RegionaisList = options;

            return View(Model);
        }

        public ActionResult Update(Models.Diretoria Model)
        {
            string actionName = "Index";

            if (ModelState.IsValid)
            {
                if(!Repo.InsertUpdate(Model))
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
