using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using Manager.Helpers;
using PagedList;

namespace Manager.Controllers
{
    public class BannersController : Controller
    {
        //
        // GET: /Banners/

        Manager.Repository.BannerHomeRepository Repo = new Repository.BannerHomeRepository();
        Manager.Repository.BannerTipoAppRepository RepoBannerTipoApp = new Repository.BannerTipoAppRepository();

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
            var ListarBanners = Repo.List();
            int paginaTamanho = 10;
            int paginaNumero = (pagina ?? 1);
            var procurar = from s in ListarBanners select s;
            ViewBag.Procura = procura;
            if (!String.IsNullOrEmpty(procura))
            {
                procurar = ListarBanners.Where(s => s.titulo.ToLower().Contains(procura.ToLower()) || s.txt_tipo_app.ToLower().Contains(procura.ToLower()) || s.texto.ToLower().Contains(procura.ToLower()));
            }
            return View(procurar.ToPagedList(paginaNumero, paginaTamanho));
        }
        [HttpPost]
        public ActionResult Update(Models.BannerHome Modelo)
        {
            FuncoesDiversas diversos = new FuncoesDiversas();
            if (Modelo.id > 0 && Modelo.file == null)
            {
                ModelState.Remove("file");
            }

            //ModelState.Clear();

            if (ModelState.IsValid)
            {

                //EDITANDO COM NOVO ARQUIVO
                if (Modelo.file != null && Modelo.id != 0)
                {
                    var fileName = diversos.RemoverCaracteres(diversos.RemoverAcentuacao(Path.GetFileName(Modelo.file.FileName)));

                    try
                    {
                        System.IO.File.Delete(Server.MapPath("~/Upload/Banners/" + Modelo.imagem));
                    }
                    catch (Exception Ex)
                    { }

                }

                //INSERINDO UM NOVO REGISTRO
                if (Modelo.file != null && Modelo.file.ContentLength > 0)
                {



                    var fileName = diversos.RemoverCaracteres(diversos.RemoverAcentuacao(Path.GetFileName(Modelo.file.FileName)));
                    string ext = Path.GetExtension(Server.MapPath("~/Upload/Banners/" + fileName));

                    string newname = Server.MapPath("~/Upload/Banners/" + fileName);
                    int cont = 1;
                    while (System.IO.File.Exists(newname))
                    {
                        string n = Path.GetFileNameWithoutExtension(Server.MapPath("~/Upload/Banners/" + fileName));

                        newname = n + "_" + cont.ToString() + ext;
                        cont++;

                    }

                    Modelo.imagem = fileName.Replace(" ", "_");
                    Modelo.imagem = diversos.RemoverCaracteres(diversos.RemoverAcentuacao(Modelo.imagem));
                    try
                    {
                        Modelo.size = Convert.ToInt32((Modelo.file.ContentLength / 1024)).ToString();
                        if (Convert.ToInt32(Modelo.size) > 1000)
                        {
                            Modelo.size += "MB";
                        }
                        else
                        {
                            Modelo.size += "KB";
                        }
                    }
                    catch (Exception Exp)
                    { }

                    fileName = Path.GetFileName(newname);

                    var path = Path.Combine(Server.MapPath("~/Upload/Banners/"), fileName.Replace(" ", "_"));
                    Modelo.file.SaveAs(path);

                }

                if (Modelo.id > 0 || (Modelo.id == 0 && Modelo.file != null))
                {
                    Repo.InsertUpdate(Modelo);
                }
            }
            else
            {
                ViewBag.BannerTipoApp = RepoBannerTipoApp.List();
                return View("Edit", Modelo);
            }
            return RedirectToAction("Index");

        }

        public ActionResult Edit(int id)
        {


            Manager.Models.BannerHome Model = new Models.BannerHome();
            ViewBag.BannerTipoApp = RepoBannerTipoApp.List();

            if (id > 0)
            {
                Model = Repo.Load(id);
                ViewBag.Operacao = "Editar Registro";
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
