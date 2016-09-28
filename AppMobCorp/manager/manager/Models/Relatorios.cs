using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Manager.Models
{
    public class RelatoriosWS
    {

        public string id {get;set;}

        public string date {get;set;}        

        public string title {get;set;}

        public string size { get; set; }

        public string urlPDF {get;set;}
                
    }

    public class Relatorios
    {
        public int id { get; set; }

        [AllowHtml]
        [Required(ErrorMessage="O campo título não foi preenchido")]
        public string titulo { get; set; }

        [Required(ErrorMessage = "Deve selecionar um arquivo")]
        [ValidateFileAttribute(ErrorMessage = "Arquivo inválido!")]
        public HttpPostedFileBase file { get; set; }

        public string arquivo { get; set; }

        public string size { get; set; } //pega dinamicamente na chamada do webservice

        public string data { get; set; }

        public bool status { get; set; }
        public string str_status { get; set; }

        public Relatorios()
        {
            this.id = 0;
        }

        partial class ValidateFileAttribute : RequiredAttribute
        {
            public override bool IsValid(object value)
            {
                if (value == null) return false;
                var file = value as HttpPostedFileBase;
                if (file == null)
                {
                    return false;
                }

                var fileName = Path.GetFileName(file.FileName);

                string ext = Path.GetExtension(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Relatorios/" + fileName));


                if (ext.ToLower() != ".pdf" )
                {
                    return false;
                }               

                return true;
            }
        }
        
    }

   
}