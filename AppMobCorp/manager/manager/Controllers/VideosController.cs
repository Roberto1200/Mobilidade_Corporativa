using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Manager.Helpers;
using PagedList;

namespace Manager.Controllers
{
    public class VideosController : Controller
    {
        //
        // GET: /Videos/

        Manager.Repository.VideosRepository Repo = new Repository.VideosRepository();

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
            var listaVideos = Repo.List();
            int paginaTamanho = 10;
            int paginaNumero = (pagina ?? 1);
            var procurar = from s in listaVideos select s;
            ViewBag.Procura = procura;
            if (!String.IsNullOrEmpty(procura))
            {
                procurar = listaVideos.Where(s => s.titulo.ToLower().Contains(procura.ToLower())
                                       || s.video.ToLower().Contains(procura.ToLower())
                                       || s.imagem.ToLower().Contains(procura.ToLower())
                                       || s.str_status.ToLower().Contains(procura.ToLower()));
            }
            return View(procurar.ToPagedList(paginaNumero, paginaTamanho));
        }

        [HttpPost]
        public ActionResult Update(Manager.Models.Video Modelo)
        {
            FuncoesDiversas diversos = new FuncoesDiversas();
            if (Modelo.id > 0 && Modelo.file2 == null && Modelo.file == null)
            {
                ModelState.Remove("file");
                ModelState.Remove("file2");
            }
            else if (Modelo.id > 0 && Modelo.file == null)
            {
                ModelState.Remove("file");

            }
            else if (Modelo.id > 0 && Modelo.file2 == null)
            {
                ModelState.Remove("file2");
            }
            
            if (ModelState.IsValid)
            {

                //EDITANDO COM NOVO ARQUIVO
                if (ModelState.IsValid && Modelo.file != null && Modelo.id != 0)
                {
                    var imageName = diversos.RemoverCaracteres(diversos.RemoverAcentuacao(Path.GetFileName(Modelo.file.FileName)));
                    try
                    {
                        System.IO.File.Delete(Server.MapPath("~/Upload/Videos/" + imageName));
                    }
                    catch (Exception Ex)
                    { }

                }
                if (ModelState.IsValid && Modelo.file2 != null && Modelo.id != 0)
                {
                    var videoName = diversos.RemoverCaracteres(diversos.RemoverAcentuacao(Path.GetFileName(Modelo.file2.FileName)));
                    try
                    {
                        System.IO.File.Delete(Server.MapPath("~/Upload/Videos/" + videoName));
                    }
                    catch (Exception Ex)
                    { }

                }
                //INSERINDO UM NOVO REGISTRO
                if (Modelo.file != null && Modelo.file.ContentLength > 0)
                {
                    //Pegando o nome da imagem
                    var imageName = diversos.RemoverCaracteres(diversos.RemoverAcentuacao(Path.GetFileName(Modelo.file.FileName)));
                    string extImage = Path.GetExtension(Server.MapPath("~/Upload/Videos/" + imageName));


                    string novoImagem = Server.MapPath("~/Upload/Videos/" + imageName);
                    int cont = 1;
                    while (System.IO.File.Exists(novoImagem))
                    {
                        string n = Path.GetFileNameWithoutExtension(Server.MapPath("~/Upload/Videos/" + imageName));

                        novoImagem = n + "_" + cont.ToString() + extImage;
                        cont++;

                    }

                    Modelo.imagem = imageName.Replace(" ", "_");
                    Modelo.imagem = diversos.RemoverCaracteres(diversos.RemoverAcentuacao(diversos.RemoverAcentuacao(Modelo.imagem)));

                    imageName = Path.GetFileName(novoImagem);

                    //Salva imagem
                    var path = Path.Combine(Server.MapPath("~/Upload/Videos/"), imageName.Replace(" ", "_"));
                    Modelo.file.SaveAs(path);
                }
                if (Modelo.file2 != null && Modelo.file2.ContentLength > 0)
                {
                    //Pegando o nome do video
                    var videoName = diversos.RemoverCaracteres(diversos.RemoverAcentuacao(Path.GetFileName(Modelo.file2.FileName)));
                    string extVideo = Path.GetExtension(Server.MapPath("~/Upload/Videos/" + videoName));

                    string novoVideo = Server.MapPath("~/Upload/Videos/" + videoName);
                    int cont = 1;
                    while (System.IO.File.Exists(novoVideo))
                    {
                        string n = Path.GetFileNameWithoutExtension(Server.MapPath("~/Upload/Videos/" + videoName));

                        novoVideo = n + "_" + cont.ToString() + extVideo;
                        cont++;

                    }

                    Modelo.video = videoName.Replace(" ", "_");
                    Modelo.video = diversos.RemoverCaracteres(diversos.RemoverAcentuacao(diversos.RemoverAcentuacao(Modelo.video)));
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

                    videoName = Path.GetFileName(novoVideo);

                    //Salva video
                    var path2 = Path.Combine(Server.MapPath("~/Upload/Videos/"), videoName.Replace(" ", "_"));
                    Modelo.file2.SaveAs(path2);
                }

                if (Modelo.id > 0 || (Modelo.id == 0 && Modelo.file != null && Modelo.file2 != null))
                {
                    Repo.InsertUpdate(Modelo);
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View("Edit", Modelo);
            }



        }

        public ActionResult Edit(int id)
        {
            Manager.Models.Video Model = new Models.Video();

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
