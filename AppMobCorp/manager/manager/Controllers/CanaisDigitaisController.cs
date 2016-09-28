using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manager.Helpers;
using PagedList;

namespace Manager.Controllers
{
    public class CanaisDigitaisController : Controller
    {
        Manager.Repository.CanaisDigitaisRepository Repo = new Repository.CanaisDigitaisRepository();
        //
        // GET: /CanaisDigitais/

        #region index de cada categoria
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
                procurar = listarCanais.Where(s => s.nome.ToLower().Contains(procura.ToLower()));
            }
            return View("index", procurar.ToPagedList(paginaNumero, paginaTamanho));
        }
        public ActionResult Subcategorias(int id, int? pagina, string procura, string currentFilter)
        {
            ModelState.Clear();

            var ListarSubcategorias = Repo.ListSubcategoria(id);
            int paginaTamanho = 10;
            int paginaNumero = (pagina ?? 1);
            var procurar = from s in ListarSubcategorias select s;
            ViewBag.Procura = procura;
            if (!String.IsNullOrEmpty(procura))
            {
                procurar = ListarSubcategorias.Where(s => s.chamada.ToLower().Contains(procura.ToLower())
                                                        || s.conteudo.ToLower().Contains(procura.ToLower()));
            }
            return View("Subcategoria/index", procurar.ToPagedList(paginaNumero, paginaTamanho));

        }
        public ActionResult Conteudo(int id, int? pagina, string procura, string currentFilter)
        {
            ModelState.Clear();

            var ListarPosts = Repo.ListConteudo(id);
            int paginaTamanho = 10;
            int paginaNumero = (pagina ?? 1);
            var procurar = from s in ListarPosts select s;
            ViewBag.Procura = procura;
            if (!String.IsNullOrEmpty(procura))
            {
                procurar = ListarPosts.Where(s => s.nome.ToLower().Contains(procura.ToLower())
                                                        || s.conteudo.ToLower().Contains(procura.ToLower()));
            }
            return View("Subcategoria/Conteudo/index", procurar.ToPagedList(paginaNumero, paginaTamanho));
        }
        #endregion
        public ActionResult Edit(int id)
        {
            Manager.Models.CanaisDigitais Model = new Models.CanaisDigitais();

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
        public ActionResult EditSub(int id, int idp)
        {
            Manager.Models.CanaisDigitais_Subcat Model = new Models.CanaisDigitais_Subcat();

            if (id > 0)
            {
                Model = Repo.LoadCategorias(id);
                ViewBag.Operacao = "Editar Registro";
            }
            else
            {
                Model.id_canal_digital = idp;
                ViewBag.Operacao = "Inserir Registro";
            }
            return View("Subcategoria/Edit", Model);
        }
        public ActionResult EditCont(int id, int idp)
        {
            Manager.Models.CanaisDitais_subcat_conteudo Model = new Models.CanaisDitais_subcat_conteudo();

            if (id > 0)
            {
                Model = Repo.LoadConteudo(id);
                ViewBag.Operacao = "Editar Registro";
            }
            else
            {
                Model.id_subcat = idp;
                ViewBag.Operacao = "Inserir Registro";
            }
            return View("Subcategoria/Conteudo/Edit", Model);
        }
        FuncoesDiversas diversos = new FuncoesDiversas();
        [HttpPost]
        public ActionResult Update(Manager.Models.CanaisDigitais Modelo)
        {
            if (Modelo.id > 0 && Modelo.file == null)
            {
                ModelState.Remove("file");
            }
            if (ModelState.IsValid)
            {

                //EDITANDO COM NOVO ARQUIVO
                if (ModelState.IsValid && Modelo.file != null && Modelo.id != 0)
                {
                    var imagemName = diversos.RemoverCaracteres(diversos.RemoverAcentuacao(Path.GetFileName(Modelo.file.FileName)));
                    try
                    {
                        System.IO.File.Delete(Server.MapPath("~/Upload/Banners/" + imagemName));
                    }
                    catch (Exception Ex)
                    { }

                }
                //INSERINDO UM NOVO REGISTRO
                if (Modelo.file != null && Modelo.file.ContentLength > 0)
                {
                    //Pegando o nome da imagem
                    var imageName = diversos.RemoverCaracteres(diversos.RemoverAcentuacao(Path.GetFileName(Modelo.file.FileName)));
                    string extImage = Path.GetExtension(Server.MapPath("~/Upload/Banners/" + imageName));


                    string novoImagem = Server.MapPath("~/Upload/Banners/" + imageName);
                    int cont = 1;
                    while (System.IO.File.Exists(novoImagem))
                    {
                        string n = Path.GetFileNameWithoutExtension(Server.MapPath("~/Upload/Banners/" + imageName));

                        novoImagem = n + "_" + cont.ToString() + extImage;
                        cont++;

                    }

                    Modelo.imagem_banner = imageName.Replace(" ", "_");
                    Modelo.imagem_banner = diversos.RemoverCaracteres(diversos.RemoverAcentuacao(Modelo.imagem_banner));

                    imageName = Path.GetFileName(novoImagem);

                    //Salva imagem
                    var path = Path.Combine(Server.MapPath("~/Upload/Banners/"), imageName.Replace(" ", "_"));
                    Modelo.file.SaveAs(path);
                }

                if (Modelo.id > 0 || (Modelo.id == 0 && Modelo.file != null))
                {

                    Repo.InsertUpdate(Modelo);
                    return RedirectToAction("Index");
                }
                return View("Edit", Modelo);
            }
            else
            {
                return View("Edit", Modelo);
            }


        }
        public ActionResult UpdateCat(Manager.Models.CanaisDigitais_Subcat Modelo)
        {

            if (Modelo.id > 0 && Modelo.file == null)
            {
                ModelState.Remove("file");
            }
            var file = Modelo.file as HttpPostedFileBase;

            if (ModelState.IsValid)
            {

                //EDITANDO COM NOVO ARQUIVO
                if (ModelState.IsValid && Modelo.file != null && Modelo.id != 0)
                {
                    var imagemName = diversos.RemoverCaracteres(diversos.RemoverAcentuacao(Path.GetFileName(Modelo.file.FileName)));
                    try
                    {
                        System.IO.File.Delete(Server.MapPath("~/Upload/Banners/" + imagemName));
                    }
                    catch (Exception Ex)
                    { }

                }
                //INSERINDO UM NOVO REGISTRO
                if (Modelo.file != null && Modelo.file.ContentLength > 0)
                {
                    switch (Modelo.id_tipo_banner_destaque)
                    {
                        case 1:
                            using (var img = Image.FromStream(file.InputStream))
                            {
                                if (img.Width != 760 && img.Height != 335)
                                {
                                    ModelState.AddModelError("", "Arquivo deve estar no formato JPG ou PNG no formato 760x335");
                                    return View("Subcategoria/Edit", Modelo);
                                }
                            }
                            break;
                        case 2:
                            using (var img = Image.FromStream(file.InputStream))
                            {
                                if (img.Width != 760 && img.Height != 670)
                                {
                                    ModelState.AddModelError("", "Arquivo deve estar no formato JPG ou PNG no formato 760x670");
                                    return View("Subcategoria/Edit", Modelo);
                                }
                            }
                            break;
                        case 3:
                            using (var img = Image.FromStream(file.InputStream))
                            {
                                if (img.Width != 2048 && img.Height != 620)
                                {
                                    ModelState.AddModelError("", "Arquivo deve estar no formato JPG ou PNG no formato 2048x620");
                                    return View("Subcategoria/Edit", Modelo);
                                }
                            }
                            break;
                    }
                    //Pegando o nome da imagem
                    var imageName = diversos.RemoverCaracteres(diversos.RemoverAcentuacao(Path.GetFileName(Modelo.file.FileName)));
                    string extImage = Path.GetExtension(Server.MapPath("~/Upload/Banners/" + imageName));


                    string novoImagem = Server.MapPath("~/Upload/Banners/" + imageName);
                    int cont = 1;
                    while (System.IO.File.Exists(novoImagem))
                    {
                        string n = Path.GetFileNameWithoutExtension(Server.MapPath("~/Upload/Banners/" + imageName));

                        novoImagem = n + "_" + cont.ToString() + extImage;
                        cont++;

                    }
                    Modelo.imagem = imageName.Replace(" ", "_");
                    Modelo.imagem = diversos.RemoverCaracteres(diversos.RemoverAcentuacao(Modelo.imagem));

                    imageName = Path.GetFileName(novoImagem);

                    //Salva imagem
                    var path = Path.Combine(Server.MapPath("~/Upload/Banners/"), imageName.Replace(" ", "_"));
                    Modelo.file.SaveAs(path);
                }

                if (Modelo.id > 0 || (Modelo.id == 0 && Modelo.file != null))
                {
                    Repo.InsertUpdateCat(Modelo);
                    return RedirectToAction("Subcategoria/" + Modelo.id_canal_digital);

                }
                return View("Subcategoria/Edit", Modelo);
            }
            else
            {
                return View("Subcategoria/Edit", Modelo);
            }


        }
        public ActionResult UpdateCont(Manager.Models.CanaisDitais_subcat_conteudo Modelo)
        {
            if (Modelo.id > 0 && Modelo.file == null)
            {
                ModelState.Remove("file");
            }
            if (ModelState.IsValid)
            {

                //EDITANDO COM NOVO ARQUIVO
                if (ModelState.IsValid && Modelo.file != null && Modelo.id != 0)
                {
                    var imagemName = diversos.RemoverCaracteres(diversos.RemoverAcentuacao(Path.GetFileName(Modelo.file.FileName)));
                    try
                    {
                        System.IO.File.Delete(Server.MapPath("~/Upload/Banners/" + imagemName));
                    }
                    catch (Exception Ex)
                    { }

                }
                //INSERINDO UM NOVO REGISTRO
                if (Modelo.file != null && Modelo.file.ContentLength > 0)
                {
                    //Pegando o nome da imagem
                    var imageName = diversos.RemoverCaracteres(diversos.RemoverAcentuacao(Path.GetFileName(Modelo.file.FileName)));
                    string extImage = Path.GetExtension(Server.MapPath("~/Upload/Banners/" + imageName));


                    string novoImagem = Server.MapPath("~/Upload/Banners/" + imageName);
                    int cont = 1;
                    while (System.IO.File.Exists(novoImagem))
                    {
                        string n = Path.GetFileNameWithoutExtension(Server.MapPath("~/Upload/Banners/" + imageName));

                        novoImagem = n + "_" + cont.ToString() + extImage;
                        cont++;

                    }

                    Modelo.banner = imageName.Replace(" ", "_");
                    Modelo.banner = diversos.RemoverCaracteres(diversos.RemoverAcentuacao(Modelo.banner));

                    imageName = Path.GetFileName(novoImagem);

                    //Salva imagem
                    var path = Path.Combine(Server.MapPath("~/Upload/Banners/"), imageName.Replace(" ", "_"));
                    Modelo.file.SaveAs(path);
                }

                if (Modelo.id > 0 || (Modelo.id == 0 /*&& Modelo.file != null*/))
                {

                    Repo.InsertUpdateCont(Modelo);
                    return RedirectToAction("Subcategoria/Conteudo/" + Modelo.id_subcat);
                }
                return View("Subcategoria/Conteudo/Edit", Modelo);
            }
            else
            {
                return View("Subcategoria/Conteudo/Edit", Modelo);
            }


        }
        public ActionResult DeleteSub(int id, int idp)
        {
            Repo.DeleteSub(id);
            return RedirectToAction("Subcategoria/" + idp);
        }
        public ActionResult DeleteCont(int id, int idp)
        {
            Repo.DeleteCont(id);
            return RedirectToAction("Subcategoria/Conteudo/" + idp);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Load()
        {

            return Json("false");
        }

    }
}
