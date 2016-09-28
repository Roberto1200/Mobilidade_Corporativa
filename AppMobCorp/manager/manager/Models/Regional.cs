using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Manager.Models
{
    public class Regional
    {

        public int id { get; set; }

        [Required(ErrorMessage = "Campo nome é obrigatório")]
        public string regional { get; set; }

        [Required(ErrorMessage = "Campo diretoria é obrigatório")]
        public int id_diretoria { get; set; }

        public string diretoria { get; set; }

        public string diretoria_regional
        {
            get { return string.Concat(diretoria, " - ", regional); }
        }

        public List<Agencias> agencias { get; set; }

    }
}