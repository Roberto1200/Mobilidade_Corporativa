using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Manager.Helpers;

namespace Manager.Models
{
    public class CartilhaWS
    {

        public string id {get;set;}

        public string date {get;set;}        

        public string title {get;set;}

        public string size { get; set; }

        public string urlPDF {get;set;}
                
    }

    public class Cartilha
    {
        public int id { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "O campo título não foi preenchido")]
        public string titulo { get; set; }

        [ValidatePDFAttribute(UploadFolder ="Cartilhas", MaxLenghtAllowedMB = 20)]

        [Required(ErrorMessage = "Arquivo PDF não foi selecionado")]
        public HttpPostedFileBase file { get; set; }

        public string arquivo { get; set; }

        public string size { get; set; } //pega dinamicamente na chamada do webservice

        public string data { get; set; }

        public bool status { get; set; }
        public string str_status { get; set; }

        public Cartilha()
        {
            this.id = 0;
        }

        

    }

   
}