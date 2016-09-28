using Manager.Repository;
using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;
using Manager.Models;
using System.Collections.Generic;

namespace Manager.Controllers
{
    public class LembretesController : Controller
    {
        private LembretesRepository _lembreteRepository;
        private RegionaisRepository _regionaisRepository;
        private DiretoriasRepository _diretoriasRepository;

        public LembretesController()
        {
            this._lembreteRepository = new LembretesRepository();
            this._regionaisRepository = new RegionaisRepository();
            this._diretoriasRepository = new DiretoriasRepository();

            ViewBag.Regionais = this.ToListItem(this._regionaisRepository.List());
        }
        
        public ActionResult Index(int? pagina, DateTime? data_inicio, DateTime? data_fim, int? regional, string procura)
        {
            ModelState.Clear();

            DateTime dataInicio = DateTime.MinValue;
            DateTime dataFim = DateTime.MinValue;

            DateTime.TryParse(data_inicio.ToString(), out dataInicio);
            DateTime.TryParse(data_fim.ToString(), out dataFim);

            var listarLembretes = this._lembreteRepository.Load(data_inicio, data_fim, regional, procura);

            int paginaTamanho = 10;
            int paginaNumero = (pagina ?? 1);
            var procurar = from s in listarLembretes select s;
            ViewBag.Procura = procura;

            if (!String.IsNullOrEmpty(procura))
            {
                procurar = listarLembretes.Where(s => s.Mensagem.ToLower().Contains(procura.ToLower()));
            }
            return View(procurar.ToPagedList(paginaNumero, paginaTamanho));
        }
        public ActionResult Delete(int id)
        {
            this._lembreteRepository.Delete(id);
            return RedirectToAction("Index");
        }
        public ActionResult Visualizar(int id)
        {
            ViewBag.ErrorMessage = "Houve um erro ao processar as informações, tente novamente.";

            if(id > 0)
            {
                var lembrete = this._lembreteRepository.Load(id);

                if (lembrete != null) return View("Visualizar", lembrete);

                ViewBag.ErrorMessage = "Lembrete não encontrado.";
            }
            else
                ViewBag.ErrorMessage = "Inválido ou não existe.";

            return View("Index", ViewBag.ErrorMessage);
        }

        public ActionResult Edit(int id)
        {
            var Model = new Lembretes();
            if (id > 0)
            {
                Model = this._lembreteRepository.Load(id);
                ViewBag.Operacao = "Editar Registro";
            }
            else
            {
                ViewBag.DirRegional = this._diretoriasRepository.List();
                ViewBag.Operacao = "Inserir Registro";
            }
            return View(Model);
        }

        private IEnumerable<SelectListItem> ToListItem(List<Regional> list)
        {
            if(list.Count > 0)
            {
                var lst = new List<SelectListItem>();
                lst.Add(new SelectListItem() { Text = "Selecione uma Regional", Value = null, Selected = true });

                foreach (var item in list)
                    lst.Add(new SelectListItem() { Text = item.regional, Value = item.id.ToString() });

                return lst;
            }

            return new List<SelectListItem>() { new SelectListItem() { Text = "Não existem regionais cadastradas", Value = "-1" } };
        }
    }
}
