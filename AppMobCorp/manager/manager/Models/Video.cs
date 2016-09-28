using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.Configuration;

namespace Manager.Models
{


    public class VideoWS
    {

        public string id { get; set; }

        public string title { get; set; }

        public string size { get; set; }

        public string duracao { get; set; }

        public string url { get; set; }

        public string urlThumbnail { get; set; }

    }

    public class Video
    {
        public int id { get; set; }

        [AllowHtml]
        [Required(ErrorMessage="O campo título deve ser preenchido")]
        public string titulo { get; set; }
        
        public string imagem { get; set; }

        public string video { get; set; }

        [Required(ErrorMessage = "Deve selecionar um arquivo de imagem")]
        //[ValidateFileAttribute(ErrorMessage = "Arquivo inválido!")]
        public HttpPostedFileBase file { get; set; }

        //[ValidateFileAttribute(ErrorMessage = "Arquivo inválido!")]
        [Required(ErrorMessage = "Deve selecionar um arquivo de video")]
        public HttpPostedFileBase file2 {get;set;}

        public bool status { get; set; }

        public string str_status { get; set; }

        public string app { get; set; }
        public string size { get; set; }
        [Required(ErrorMessage = "O campo duração deve ser preenchido")]
        public string duracao { get; set; }
        public string data { get; set; }

        public Video()
        {
            this.id = 0;
        }

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

                var fileName = Path.GetFileName(file.FileName);

                string ext = Path.GetExtension(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Videos/" + fileName));


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
                                if (img.Height != 360 && img.Width != 640)
                                {
                                    return false;
                                }
                            }
                        }
                        catch { }
                    }
                }
                if(fileSize > 20.0)
                {
                    return false;
                }

                return true;
            }
        }
    }
}