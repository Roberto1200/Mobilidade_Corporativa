using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Manager.Models
{
    public class Administrador
    {

        public int id { get; set; }

        [Required(ErrorMessage = "O campo nome é obrigatório")]
        [Remote("CheckExistingNome", "Administradores", AdditionalFields = "id", ErrorMessage = "Nome já cadastrado")]
        public string nome { get; set; }

        [Required(ErrorMessage = "O campo email é obrigatório")]
        //[RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Formato de E-mail inválido")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@(bradesco.com.br|pixit.com.br)$", ErrorMessage = "Formato de E-mail inválido")]
        [Remote("CheckExistingEmail", "Administradores", AdditionalFields = "id", ErrorMessage = "E-Mail já esta registrado")]
        public string email { get; set; }

        //[Required(ErrorMessage = "O campo senha é obrigatório")]
        [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?=.*[!#$%&()*+,-.:;?@_{|}~]).{8,16}$", 
            ErrorMessage = "A senha deve conter de 8 a 16 caracteres, incluindo números, letras maiúsculas e minúsculas e símbolos (ex.: bRa1234@).")]
        public string senha { get; set; }

        public bool bloqueado { get; set; }
        public string data_liberacao { get; set; }

    }
}