using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace Manager.Helpers
{
    abstract class ValidateBaseFileAttribute : ValidationAttribute
    {
        protected const string DefaultInvalidExtension = "Extensão do arquivo é inválida";
        protected const string DefaultInvalidSize = "Tamanho de arquivo excedido.";

        protected List<string> AllowedExtensions { get; set; }

        public string UploadFolder { get; set;  }

        public int MaxLenghtAllowedMB { get; set; }

        public ValidateBaseFileAttribute()
        {
            MaxLenghtAllowedMB = 0;
        }

        private double GetFileSizeBytes(int maxLenghtAllowed)
        {
            return maxLenghtAllowed * Math.Pow(2, 20);
        }

        protected bool ValidateFileSize(int currentSize, int maxAllowedSize)
        {
            return maxAllowedSize > 0 || (double)currentSize >= GetFileSizeBytes(maxAllowedSize);
        }

        protected bool IsInvalidExtension(string fileName)
        {
            string fullFileName = string.Format("~/Upload/{0}/{1}", UploadFolder, fileName);
            string ext = Path.GetExtension(System.Web.Hosting.HostingEnvironment.MapPath(fullFileName));

            return ! AllowedExtensions.Select(x => x == ext.ToLower()).Any();
        }
    }

    partial class ValidateImgFileAttribute : ValidateBaseFileAttribute
    {        
        private const string DefaultInvalidDimension = "Dimensão de imagem inválida";
        private const string DefaultErrorMessage = "Arquivo de imagem não deve estar vazio";

        public int Height { get; set; }
        public int Width { get; set; }
               

        public ValidateImgFileAttribute() : base()
        {
            AllowedExtensions = new List<string>() { ".png", ".jpg", ".jpeg" };
            Height = 0;
            Width = 0;
            MaxLenghtAllowedMB = 0;
        }        

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as HttpPostedFileBase;
            if (file == null)
            {
                return new ValidationResult(DefaultErrorMessage);
            }

            string fileName = Path.GetFileName(file.FileName);

            if (IsInvalidExtension(fileName))
            {
                return new ValidationResult(DefaultInvalidExtension);
            }

            if (Helpers.Configurations.BreakImageSize == true)
            {
                try
                {
                    using (var img = Image.FromStream(file.InputStream))
                    {
                        if (img.Height != Height && img.Width != Width && Width > 0 && Height > 0)
                        {
                            return new ValidationResult(string.Format("{0}. A imagem selecionada deve ser de {1}x{2}", DefaultInvalidDimension, Width, Height));
                        }
                    }
                }
                catch (Exception e)
                {
                    return new ValidationResult(e.Message);
                }
            }

            return ValidationResult.Success;
        }
    }

    partial class ValidatePDFAttribute : ValidateBaseFileAttribute
    {        
        private const string DefaultErrorMessage = "Arquivo PDF não deve estar vazio";

        public ValidatePDFAttribute() : base()
        {
            AllowedExtensions = new List<string>() { ".pdf" };            
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as HttpPostedFileBase;
            if (file == null)
            {
                return new ValidationResult(DefaultErrorMessage);
            }

            string fileName = Path.GetFileName(file.FileName);

            if (IsInvalidExtension(fileName))
            {
                return new ValidationResult(DefaultInvalidExtension);
            }

            if ( ! ValidateFileSize (file.ContentLength, MaxLenghtAllowedMB) )
            {
                return new ValidationResult(DefaultInvalidSize);

            }

            return ValidationResult.Success;
        }
    }
}