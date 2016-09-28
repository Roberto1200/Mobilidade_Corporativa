using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Manager.Models
{
    public class DocumentosRegionaisWS
    {

        public string id {get;set;}

        public string date {get;set;}        

        public string title {get;set;}

        public string size { get; set; }

        public string urlPDF {get;set;}
                
    }

    public class DocumentosRegional
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

        public DocumentosRegional()
        {
            this.id = 0;
        }

        partial class ValidateFileAttribute : ValidationAttribute
        {

            private const string DefaultInvalidExtension = "Extensão do arquivo é inválida";
            private const string DefaultInvalidSize = "Tamanho de arquivo excedido.";
            private const string DefaultErrorMessage = "Arquivo PDF não deve estar vazio";

            // Set Max File Size allows in MB
            private const int maxLenghtAllowed = 20;
            
            public int EditInsert { get; set; }

            private double GetFileSizeBytes(int maxLenghtAllowed)
            {
                return maxLenghtAllowed * Math.Pow(2, 20);
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var file = value as HttpPostedFileBase;
                if (file == null)
                {
                    return new ValidationResult(DefaultErrorMessage);
                }

                var fileName = Path.GetFileName(file.FileName);

                string ext = Path.GetExtension(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Cartilhas/" + fileName));

                if (ext.ToLower() != ".pdf")
                {
                    return new ValidationResult(DefaultInvalidExtension);
                }

                if ((double)file.ContentLength >= GetFileSizeBytes(maxLenghtAllowed))
                {
                    return new ValidationResult(DefaultInvalidSize);
                    
                }

                return ValidationResult.Success;
            }
        }

    }

   
}