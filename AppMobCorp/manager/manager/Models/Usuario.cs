using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using FileHelpers;

namespace Manager.Models
{
    public class EnsureMinimumElementsAttribute : ValidationAttribute
    {
        private readonly int _minElements;
        public EnsureMinimumElementsAttribute(int minElements)
        {
            _minElements = minElements;
        }

        public override bool IsValid(object value)
        {
            var list = value as System.Collections.IList;
            if (list != null)
            {
                return list.Count >= _minElements;
            }
            return false;
        }
    }

    public class Usuario
    {
        public bool status { get; set; }

        public int id { get; set; }

        [Required(ErrorMessage = "Campo código funcional obrigatório")]
        [RegularExpression("^[0-9]{1,8}$", ErrorMessage = "Campo código funcional deve ser numérico")]
        [Remote("CheckExistingCodFuncional", "Usuarios", AdditionalFields = "id", ErrorMessage = "Código Funcional já cadastrado")]
        public string codfuncional { get; set; }

        [Required(ErrorMessage = "Campo nome obrigatório")]
        [Remote("CheckExistingNome", "Usuarios", AdditionalFields = "id", ErrorMessage = "Nome já cadastrado")]
        public string nome { get; set; }

        [Required(ErrorMessage = "Campo E-Mail é obrigatório")]
        //[RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Formato de E-mail inválido")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@(bradesco.com.br|pixit.com.br)$", ErrorMessage = "Formato de E-mail inválido")]
        [Remote("CheckExistingEmail", "Usuarios", AdditionalFields ="id", ErrorMessage = "E-Mail já cadastrado")]
        public string email { get; set; }

        [Required(ErrorMessage = "Campo diretoria obrigatório")]
        public string diretoria { get; set; }


        public string regional { get; set; }

        [Required(ErrorMessage = "Campo regional é obrigatório")]
        public int unique_regional { get; set; }

        //[Required(ErrorMessage = "Campo Lista regional é obrigatório")]
        //[EnsureMinimumElements(1, ErrorMessage = "Ao menos um regional deve ser selecionado")]
        public List<int> idregional { get; set; }

        public int juncao { get; set; }

        public string agencia { get; set; }

        [Required(ErrorMessage = "Campo tipo é obrigatório")]
        public int tipo { get; set; }

        public string txttipo { get; set; }

        public string txtdiretoria { get; set; }

        [Required(ErrorMessage = "Campo fone celular obrigatório")]
        [RegularExpression(@"^(\([1-9]{2}\) [0-9]{5}-[0-9]{4})$",
            ErrorMessage = "Formato de telefone inválido.")]
        public string fonecelular { get; set; }

        public string fonecomercial { get; set; }

        // [Required(ErrorMessage = "Campo usuário brigatório")]
        public string usuario { get; set; }

        //[Required(ErrorMessage = "Campo senha obrigatório")]
        [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?=.*[!#$%&()*+,-.:;?@_{|}~]).{8,16}$", 
            ErrorMessage = "A senha deve conter de 8 a 16 caracteres, incluindo números, letras maiúsculas e minúsculas e símbolos (ex.: bRa1234@).")]
        public string senha { get; set; }
        public string senhaAntiga { get; set; }

        public bool primeiroacesso { get; set; }

        public string token { get; set; }

        public string avatar { get; set; }

        public DateTime datainicio { get; set; }


        public Usuario()
        {
            avatar = "";
            tipo = 0;
        }

    }
}