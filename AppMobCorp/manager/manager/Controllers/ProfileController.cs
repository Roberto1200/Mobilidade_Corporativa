using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Manager.Controllers
{
    public class ProfileController : Controller
    {

        public ActionResult Index()
        {
            return Content("ok");
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UploadAvatar(string funcional)
        {

            int arquivosSalvos = 0;
              for (int i = 0; i < Request.Files.Count; i++)
              {
                  HttpPostedFileBase arquivo = Request.Files[i];
   
                  //Suas validações ......
   
                  //Salva o arquivo
                  if (arquivo.ContentLength > 0)
                  {
                      var uploadPath = Server.MapPath("~/Upload/Avatar");
                      string caminhoArquivo = Path.Combine(@uploadPath,funcional + ".jpg");
                      //Path.GetFileName(arquivo.FileName)
                      arquivo.SaveAs(caminhoArquivo);
                      arquivosSalvos++;
                  }
              }

              try
              {
                  if (Request.Files.Count > 0)
                  {
                      Repository.UsuarioseRepository ur = new Repository.UsuarioseRepository();
                      Models.Usuario tmp = ur.List().Where(p => p.codfuncional == funcional).FirstOrDefault();

                      tmp.avatar = funcional + ".jpg";

                      try
                      {
                          ur.InsertUpdate(tmp);
                          return Content("ok");
                      }
                      catch (Exception Ex)
                      {
                          return Content(Ex.Message);
                      }

                  }
                  else
                  {
                      return Content("Arquivo não enviado");
                  }
              }
              catch (Exception po)
              {
                  return Content(po.Message);
              }
              
          }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdatePassword(string funcional, string newpass, string oldpass)
        {
            Repository.UsuarioseRepository ur = new Repository.UsuarioseRepository();
            var existe = ur.List().Where(p => p.senha == oldpass).Where(p => p.codfuncional == funcional).FirstOrDefault();
            if (string.IsNullOrWhiteSpace(oldpass))
            {


                try
                {
                    ur.SetPrimeiroAcesso(funcional, newpass);
                }
                catch (Exception)
                { Content("OI"); }
                return Content("true");
            }
            if(existe == null)
            {
                return Content("false");
            }
            try
            {
                ur.SetPrimeiroAcesso(funcional, newpass);
            }
            catch (Exception)
            { Content("OI"); }
            return Content("true");

        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdateUser(HttpPostedFileBase file, string nome, string email, string telefone, string senha,string codfuncional, string tipoA)
        {

            if (string.IsNullOrEmpty(codfuncional))
            {
                return Content("Informe o código funcional!");
            }

            int arquivosSalvos = 0;
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase arquivo = Request.Files[i];

                //Suas validações ......

                //Salva o arquivo
                if (arquivo.ContentLength > 0)
                {
                    var uploadPath = Server.MapPath("~/Upload/Avatar");
                    string caminhoArquivo = Path.Combine(@uploadPath, codfuncional + ".jpg");
                    //Path.GetFileName(arquivo.FileName)
                    arquivo.SaveAs(caminhoArquivo);
                    
                    arquivosSalvos++;
                }
            }

            try
            {
                Repository.UsuarioseRepository ur = new Repository.UsuarioseRepository();
                Models.Usuario tmp = ur.List().Where(p => p.codfuncional == codfuncional).FirstOrDefault();

                if (Request.Files.Count > 0)
                {
                    tmp.avatar = codfuncional + ".jpg";
                }
                tmp.nome = nome;
                tmp.email = email;
                if(tipoA == "1")
                {
                    tmp.senha = tmp.senha;
                    tmp.status = true;
                }
                else
                {
                    tmp.senha = senha;
                }
                tmp.fonecelular = telefone;

                try
                {
                    ur.InsertUpdate(tmp);
                    return Content("ok");
                }
                catch (Exception Ex)
                {
                    return Content(Ex.Message);
                }
            }
            catch (Exception po)
            { return Content(po.Message); }
            return Content("ok");
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            bool checkForAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
            || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
            if (1 == 2)
            {
                base.OnAuthorization(filterContext);
            }


        }

    }
}

