using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Manager.Helpers;

namespace Manager.Models
{

    public class BannerWS
    {

        public string mensagem { get; set; }

        public string urlBanner { get; set; }

    }
    public class files
    {
        public HttpPostedFileBase file { get; set; }
        public int novo { get; set; }
    }
    public class BannerHome
    {
        public string size { get; set; }
        public int id { get; set; }

        public int id_tipo_app { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "O campo título é obrigatório")]
        public string titulo { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "O campo texto é obrigatório")]
        public string texto { get; set; }

        public BannerHome()
        {
            this.id_tipo_app = 1;
            this.id = 0;
        }

        [ValidateImgFileAttribute(Width = 2048, Height = 867, UploadFolder = "Banners")]

        [Required(ErrorMessage = "Imagem de Banner é obrigatório")]
        public HttpPostedFileBase file { get; set; }

        public string imagem { get; set; }

        public string txt_tipo_app { get; set; }

        public bool status { get; set; }
        public string str_status { get; set; }

        
    }
    public class BannerHomeE
    {
        public int id { get; set; }

        public int id_tipo_app { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "O campo título não foi preenchido")]
        public string titulo { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "O campo texto não foi preenchido")]
        public string texto { get; set; }

        public BannerHomeE()
        {
            this.id_tipo_app = 1;
            this.id = 0;
        }
        public HttpPostedFileBase file { get; set; }

        public string imagem { get; set; }

        public string txt_tipo_app { get; set; }

        public bool status { get; set; }
    }


}