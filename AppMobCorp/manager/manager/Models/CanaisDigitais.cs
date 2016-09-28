using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Manager.Models
{
    public class CanaisDigitais
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Campo nome obrigatório")]
        public string nome { get; set; }
        public int subcategoria_padrao { get; set; }
        public string imagem_banner { get; set; }
        [ValidateFileAttribute(ErrorMessage = "Arquivo inválido!")]
        public HttpPostedFileBase file { get; set; }

        public int quantidade { get; set; }

        partial class ValidateFileAttribute : RequiredAttribute
        {
            public override bool IsValid(object value)
            {
                System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                HttpRuntimeSection section = config.GetSection("system.web/httpRuntime") as HttpRuntimeSection;
                var file = value as HttpPostedFileBase;

                if (file == null)
                {
                    return false;
                }

                //get Max upload size in MB                 
                double maxFileSize = Math.Round(section.MaxRequestLength / 1024.0, 1);

                //get File size in MB
                double fileSize = (file.ContentLength / 1024) / 1024.0;

                var fileName = System.IO.Path.GetFileName(file.FileName);

                string ext = Path.GetExtension(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Banners/" + fileName));


                if (ext.ToLower() != ".jpg" && ext.ToLower() != ".png" && ext.ToLower() != ".mp4" && ext.ToLower() != ".mov")
                {
                    return false;
                }

                if (ext.ToLower() == ".jpg" || ext.ToLower() == ".png")
                {
                    if (Manager.Helpers.Configurations.BreakImageSize == true)
                    {
                        try
                        {
                            using (var img = Image.FromStream(file.InputStream))
                            {
                                if (img.Width != 2048 && img.Height != 739)
                                {
                                    return false;
                                }
                            }
                        }
                        catch { }
                    }
                }
                if (fileSize > 20.0)
                {
                    return false;
                }

                return true;
            }
        }
    }
    public class CanaisDigitais_Subcat
    {
        public int id { get; set; }
        public int id_canal_digital { get; set; }
        [Required(ErrorMessage = "O campo Nome deve ser preenchido")]
        public string nome { get; set; }
        //[Required(ErrorMessage = "O Campo chamada deve ser preenchido")]
        public string chamada { get; set; }
        public string imagem { get; set; }
        public int id_tipo_banner_destaque { get; set; }
        [ValidateFileAttribute(ErrorMessage = "Arquivo inválido!")]
        public HttpPostedFileBase file { get; set; }
        public int id_tipo_pagina { get; set; }
        public string data_criacao { get; set; }
        public int quantidade { get; set; }
        [AllowHtml]
        public string conteudo { get; set; }
        public string nome_canal { get; set; }

        partial class ValidateFileAttribute : RequiredAttribute
        {
            public override bool IsValid(object value)
            {
                System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                HttpRuntimeSection section = config.GetSection("system.web/httpRuntime") as HttpRuntimeSection;
                var file = value as HttpPostedFileBase;

                if (file == null)
                {
                    return false;
                }

                //get Max upload size in MB                 
                double maxFileSize = Math.Round(section.MaxRequestLength / 1024.0, 1);

                //get File size in MB
                double fileSize = (file.ContentLength / 1024) / 1024.0;

                var fileName = System.IO.Path.GetFileName(file.FileName);

                string ext = Path.GetExtension(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Banners/" + fileName));


                if (ext.ToLower() != ".jpg" && ext.ToLower() != ".png")
                {
                    return false;
                }

                //if (ext.ToLower() == ".jpg" || ext.ToLower() == ".png")
                //{
                //    if (Manager.Helpers.Configurations.BreakImageSize == true)
                //    {
                //        try
                //        {
                //            using (var img = Image.FromStream(file.InputStream))
                //            {
                //                if ((img.Width != 640 && img.Height != 360) || (img.Width != 760 && img.Height != 670) || (img.Width != 2048 && img.Height != 620) || (img.Width != 760 && img.Height != 335))
                //                {
                //                    return false;
                //                }
                //            }
                //        }
                //        catch { }
                //    }
                //}
                if (fileSize > 20.0)
                {
                    return false;
                }

                return true;
            }
        }
    }
    public class CanaisDitais_subcat_conteudo
    {
        public int id { get; set; }
        public int id_subcat { get; set; }
        [Required(ErrorMessage = "O campo Nome deve ser preenchido")]
        public string nome { get; set; }
        public string banner { get; set; }
        [ValidateFileAttribute(ErrorMessage = "Arquivo inválido!")]
        public HttpPostedFileBase file { get; set; }
        [AllowHtml]
        public string conteudo { get; set; }
        public int quantidade { get; set; }
        public string nome_categoria { get; set; }
        public string nome_canal { get; set; }
        public int id_canal { get; set; }

        partial class ValidateFileAttribute : RequiredAttribute
        {
            public override bool IsValid(object value)
            {
                System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                HttpRuntimeSection section = config.GetSection("system.web/httpRuntime") as HttpRuntimeSection;
                var file = value as HttpPostedFileBase;

                if (file == null)
                {
                    return false;
                }

                //get Max upload size in MB                 
                double maxFileSize = Math.Round(section.MaxRequestLength / 1024.0, 1);

                //get File size in MB
                double fileSize = (file.ContentLength / 1024) / 1024.0;

                var fileName = System.IO.Path.GetFileName(file.FileName);

                string ext = Path.GetExtension(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Banners/" + fileName));


                if (ext.ToLower() != ".jpg" && ext.ToLower() != ".png" && ext.ToLower() != ".mp4" && ext.ToLower() != ".mov")
                {
                    return false;
                }

                if (ext.ToLower() == ".jpg" || ext.ToLower() == ".png")
                {
                    if (Manager.Helpers.Configurations.BreakImageSize == true)
                    {
                        try
                        {
                            using (var img = Image.FromStream(file.InputStream))
                            {
                                if (img.Width != 640 && img.Height != 360)
                                {
                                    return false;
                                }
                            }
                        }
                        catch { }
                    }
                }
                if (fileSize > 20.0)
                {
                    return false;
                }

                return true;
            }
        }
    }
    public class CanaisDicionarioB
    {
        public int id { get; set; }
        public string nome { get; set; }
    }
    public class CanaisDicionarioP
    {
        public int id { get; set; }
        public string nome { get; set; }
    }

}