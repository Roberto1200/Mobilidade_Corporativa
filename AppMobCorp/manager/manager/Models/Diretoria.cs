using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Manager.Models
{
    public class Diretoria
    {
        public int id { get; set; }

        [Required(ErrorMessage = "Campo nome é obrigatório")]
        public string diretoria { get; set; }

        public List<Regional> regionais { get; set; }

        public List<string> id_regionais { get; set; }

    }
}