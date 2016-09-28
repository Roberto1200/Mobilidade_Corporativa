using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Manager.Models
{
    public class Forum_Canal
    {
        public int id { get; set; }
        [Required(ErrorMessage = "O campo título deve ser preenchido")]
        public string titulo { get; set; }
        public int quantidade { get; set; }
    }
    public class Forum_Canal_Topico
    {
        public int id { get; set; }
        public int id_canal { get; set; }
        [Required(ErrorMessage = "O campo título deve ser preenchido")]
        public string titulo { get; set; }
        public int id_ultimo_post { get; set; }
        public int quantidade { get; set; }
    }
    public class Forum_Canal_Topico_Post
    {
        public int id { get; set; }
        public int id_topico { get; set; }
        public string codigo_funcional { get; set; }
        [Required(ErrorMessage = "O campo Mensagem deve ser preenchido")]
        public string mensagem { get; set; }
        public DateTime data_inicio { get; set; }
        public DateTime data_edicao { get; set; }
        public string nome { get; set; }
        public string avatar { get; set; }
    }
}