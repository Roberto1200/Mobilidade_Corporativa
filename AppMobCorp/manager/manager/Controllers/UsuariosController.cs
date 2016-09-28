using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileHelpers;
using System.Data;
using Manager.Repository;
using Manager.Helpers;
using PagedList;
using System.Text;

namespace Manager.Controllers
{
    public class UsuariosController : Controller
    {
        //
        // GET: /Banners/

        UsuarioseRepository Repo = new Repository.UsuarioseRepository();

        UsuariosTipoAppRepository RepoTipos = new UsuariosTipoAppRepository();

        RegionaisRepository RepoRegionais = new RegionaisRepository();

        DiretoriasRepository RepoDiretorias = new DiretoriasRepository();

        ExcelImportExport excel = new ExcelImportExport();

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

            ViewBag.CurrentFilter = procura;
            ViewBag.ImportedMessage = string.Empty;
            var listaUsers = Repo.List();
            int paginaTamanho = 10;
            int paginaNumero = (pagina ?? 1);
            var procurar = from s in listaUsers select s;
            ViewBag.Procura = procura;

            if (TempData["ImportedMessage"] != null && !string.IsNullOrEmpty(TempData["ImportedMessage"].ToString()))
            {
                ViewBag.ImportedSuccess = (bool)TempData["ImportedSuccess"] ? "alert-success" : "alert-danger";
                ViewBag.ImportedMessage = TempData["ImportedMessage"].ToString();
            }

            if (!String.IsNullOrEmpty(procura))
            {
                procurar = listaUsers.Where(s => s.nome.ToLower().Contains(procura.ToLower())
                                       || s.codfuncional.ToLower().Contains(procura.ToLower())
                                       || s.txtdiretoria.ToLower().Contains(procura.ToLower())
                                       || s.juncao.ToString().ToLower().Contains(procura.ToLower())
                                       || s.txttipo.ToString().ToLower().Contains(procura.ToLower()));
            }
            return View(procurar.ToPagedList(paginaNumero, paginaTamanho));
        }

        #region AJAX validations
        [HttpGet]
        public ActionResult CheckExistingEmail(string email, int id = 0)
        {
            bool exists = Repo.EmailExists(email, id);

            return Json(!exists, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckExistingCodFuncional(string codfuncional, int id = 0)
        {
            bool exists = Repo.CodFuncionalExists(codfuncional, id);

            return Json(!exists, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckExistingNome(string nome, int id = 0)
        {
            bool exists = Repo.NomeExists(nome, id);

            return Json(!exists, JsonRequestBehavior.AllowGet);
        }
        #endregion



        [HttpPost]
        public ActionResult Update(Manager.Models.Usuario Modelo)
        {

            string actionName = "";

            if (string.IsNullOrWhiteSpace(Modelo.senha))
            {
                Modelo.senha = Modelo.senhaAntiga;
            }
            if (Modelo.avatar == null)
            {
                Modelo.avatar = "";
            }
            if (Modelo.tipo == 1)
            {
                Modelo.idregional = new List<int>();
                Modelo.idregional.Add(Modelo.unique_regional);
            }
            else
            {
                Modelo.unique_regional = 0;
                ModelState["diretoria"].Errors.Clear();
                ModelState["unique_regional"].Errors.Clear();
            }
            if (ModelState.IsValid)
            {
                Repo.InsertUpdate(Modelo, "serviço");
                actionName = "Index";

            }
            else
            {
                actionName = string.Format("Edit/{0}", Modelo.id) ;
            }
            return RedirectToAction(actionName);

        }
        [HttpPost]
        public ActionResult RevPrimeiro(Manager.Models.Usuario Modelo)
        {
            bool sucess = Repo.RevPrimeiroAcesso(Modelo.codfuncional, Modelo.id);

            TempData["MsgRevokedAccess"] = sucess ?
                                            "Primeiro acesso revogado com sucesso!"
                                            : "Erro ao revogar acesso!";

            return RedirectToAction(string.Format("Edit/{0}", Modelo.id));
        }

        public ActionResult Edit(int id)
        {
            Manager.Models.Usuario Model = new Models.Usuario();

            ViewBag.UsuarioTipoList = RepoTipos.List();

            ViewBag.Diretorias = RepoDiretorias.List();

            ViewBag.Regionais = RepoRegionais.List();


            string options = "";

            int unique_regional = 0;

            // Filter Regionais by Diretoria
            int idDiretoria = 0;
            bool validDiretoria = false;

            if (id > 0)
            {
                Model = Repo.Load(id);

                //Model.unique_regional = unique_regional;

                Model.senhaAntiga = Model.senha;
                Model.senha = null;

                // Filter Regionais by Diretoria
                validDiretoria = (!string.IsNullOrEmpty(Model.diretoria) && int.TryParse(Model.diretoria, out idDiretoria));
                if (validDiretoria)
                    ViewBag.Regionais = RepoRegionais.ListByDiretoria(idDiretoria);

                ViewBag.Operacao = "Editar Registro";
            }
            else
            {
                Model.avatar = "";

                ViewBag.Operacao = "Inserir Registro";
            }

            #region Regionais list combo for multiple selection

            // Return all "Regionais" with their bounded "Diretorias"
            List<Models.Regional> listRegionais = RepoRegionais.List().Where(r => r.id_diretoria > 0).ToList();

            foreach (Models.Regional r in listRegionais)
            {
                string t = "";


                if (id > 0 && RepoRegionais.List(id).Where(p => p.id == r.id).Count() > 0)
                {
                    //unique_regional = r.id;
                    t = "selected";
                }

                options += "<option " + t + "  value='" + r.id.ToString() + "' >" + string.Concat(r.diretoria, " - ", r.regional) + "</option>";
            }

            ViewBag.RegionaisList = options;
            #endregion

            ViewBag.MsgRevokedAccess = null;

            if (TempData["MsgRevokedAccess"] != null)
                ViewBag.MsgRevokedAccess = TempData["MsgRevokedAccess"];

            return View(Model);
        }

        public ActionResult Delete(int id)
        {
            Repo.Delete(id);
            return RedirectToAction("Index");
        }
        public ActionResult ExportarUsuariosTXT()
        {
            var engine = new FileHelperEngine<Manager.Models.Usuario>();

            var customers = new List<Manager.Models.Usuario>();
            var usuarios = Repo.List();
            foreach (var usuario in usuarios)
            {
                customers.Add(usuario);
            }
            engine.WriteFile("Output.Txt", customers);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImportarExcel(HttpPostedFileBase excelFile)
        {

            int qtdImported = 0, qtdTotal = 0, qtdError = 0;
            bool success = true, successSummary = true;
            StringBuilder s = new StringBuilder();

            try
            {
                string path = Server.MapPath(string.Format("~/Upload/Excel/{0}", excelFile.FileName));
                string ext = System.IO.Path.GetExtension(path);
                string new_file = Server.MapPath(string.Format("~/Upload/Excel/Agentes_{0}{1}", DateTime.Now.ToString("yyyyMMdd-HHmmss"), ext));

                var excelUser = excel.Open(excelFile);

                if (excelUser != null)
                {
                    var modelUsers = Repo.MapFromExcel(excelUser, Request.Form["optUserType"]);

                    foreach (var user in modelUsers)
                    {
                        success = Repo.InsertUpdateFromExcel(user);

                        successSummary = successSummary && success; // Check if there any wrong importing records.

                        if (success)
                            qtdImported++;

                        qtdTotal++;
                    }

                    qtdError = qtdTotal - qtdImported; //Get Qtd Wrong Records

                    excelFile.SaveAs(new_file);

                    s.Append("<h4 class=\"alert-heading\">");
                    s.Append(successSummary ? "Arquivo importado com sucesso" : "Arquivo Excel fora do padrão.");
                    s.Append("</h4>").AppendLine();
                    s.Append("Resumo de importacão:").AppendLine("<br />");
                    s.AppendFormat("Total Registros Lidos e importados: {0}", qtdTotal).AppendLine("<br />");

                    if (!successSummary)
                    {
                        s.Replace("Total Registros Lidos e importados:", "Total Registros Lidos:")
                         .Replace("Arquivo importado com sucesso", "Arquivo importado com alguns erros na importação.").AppendLine("<br />");

                        s.AppendFormat("Total Registros Importados: {0}", qtdImported).AppendLine("<br />");
                        s.AppendFormat("Total Registros com erros: {0}", qtdError);
                    }
                }

            }
            catch (Exception ex)
            {

                s.Append("<h4 class=\"alert-heading\">Erro na importação do arquivo</h4>").AppendLine();
                s.Append("Arquivo Excel fora do padrão. Verifique a estrutura do arquivo e tente novamente.").AppendLine("<br />");
                successSummary = false;
            }
            finally
            {
                TempData["ImportedMessage"] = s.ToString();
                TempData["ImportedSuccess"] = successSummary;
            }

            return RedirectToAction("Index");

        }

        public ActionResult ExportarExcel(string excelFilter)
        {
            string dataName = "Usuarios";

            var listUsers = Repo.List()
                                .Where(s => string.IsNullOrEmpty(excelFilter) 
                                       || s.nome.ToLower().Contains(excelFilter.ToLower())
                                       || s.codfuncional.ToLower().Contains(excelFilter.ToLower())
                                       || s.txtdiretoria.ToLower().Contains(excelFilter.ToLower())
                                       || s.juncao.ToString().ToLower().Contains(excelFilter.ToLower())
                                       || s.txttipo.ToString().ToLower().Contains(excelFilter.ToLower())
                                      ).ToList();

            DataTable dtUsers = Repo.MapToExcel(listUsers, dataName);

            excel.Download(dtUsers, dataName);

            return RedirectToAction("Index");
        }

    }
}
