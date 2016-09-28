using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Manager.Models
{
    public class Agencias
    {
        public int id { get; set; }

        [Required(ErrorMessage = "Campo nome é obrigatório")]
        public string agencia { get; set; }

        [Required(ErrorMessage = "Campo Regional é obrigatório")]
        public int id_regional { get; set; }

        public string regional { get; set; }

    }
}