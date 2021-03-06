﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Manager.Helpers;
using PagedList;

namespace Manager.Controllers
{
    public class DocumentosRegionaisController : Controller
    {
        //
        // GET: /Cartilhas/
        Manager.Repository.DocumentosRegionaisRepository Repo = new Repository.DocumentosRegionaisRepository();

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
            var ListarDocumentos = Repo.List();
            int paginaTamanho = 10;
            int paginaNumero = (pagina ?? 1);
            var procurar = from s in ListarDocumentos select s;
            ViewBag.Procura = procura;
            if (!String.IsNullOrEmpty(procura))
            {
                procurar = ListarDocumentos.Where(s => s.titulo.ToLower().Contains(procura.ToLower()));
            }
            return View(procurar.ToPagedList(paginaNumero, paginaTamanho));
        }


        [HttpPost]
        public ActionResult Update(Manager.Models.DocumentosRegional Modelo)
        {
            FuncoesDiversas diversos = new FuncoesDiversas();
            if (Modelo.id > 0 && Modelo.file == null)
            {
                ModelState.Remove("file");
            }

            if (ModelState.IsValid)
            {

                //EDITANDO COM NOVO ARQUIVO
                if (Modelo.file != null && Modelo.id != 0)
                {
                    var fileName = diversos.RemoverCaracteres(diversos.RemoverAcentuacao(Path.GetFileName(Modelo.file.FileName)));

                    try
                    {
                        System.IO.File.Delete(Server.MapPath("~/Upload/DocumentosRegionais/" + Modelo.arquivo));
                    }
                    catch (Exception Ex)
                    { }

                }

                //INSERINDO UM NOVO REGISTRO
                if (Modelo.file != null && Modelo.file.ContentLength > 0)
                {



                    var fileName = diversos.RemoverCaracteres(diversos.RemoverAcentuacao(Path.GetFileName(Modelo.file.FileName)));
                    string ext = Path.GetExtension(Server.MapPath("~/Upload/DocumentosRegionais/" + fileName));

                    string newname = Server.MapPath("~/Upload/DocumentosRegionais/" + fileName);
                    int cont = 1;
                    while (System.IO.File.Exists(newname))
                    {
                        string n = Path.GetFileNameWithoutExtension(Server.MapPath("~/Upload/DocumentosRegionais/" + fileName));

                        newname = n + "_" + cont.ToString() + ext;
                        cont++;

                    }

                    Modelo.arquivo = fileName.Replace(" ", "_");
                    Modelo.arquivo = diversos.RemoverCaracteres(diversos.RemoverAcentuacao(Modelo.arquivo));
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



                    Modelo.data = System.DateTime.Now.ToShortDateString();
                    fileName = Path.GetFileName(newname);

                    var path = Path.Combine(Server.MapPath("~/Upload/DocumentosRegionais/"), fileName.Replace(" ", "_"));
                    Modelo.file.SaveAs(path);

                }

                if (Modelo.id > 0 || (Modelo.id == 0 && Modelo.file != null))
                {
                    Repo.InsertUpdate(Modelo);
                }
            }
            else
            {
                return View("Edit", Modelo);
            }
            return RedirectToAction("Index");

        }


        public ActionResult Edit(int id)
        {
            Manager.Models.DocumentosRegional Model = new Models.DocumentosRegional();

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
