using System;
using System.ComponentModel.DataAnnotations;

namespace Manager.Models
{
    public class Lembretes
    {
        public int id { get; set; }

        [Required(ErrorMessage = "O campo data é obrigatório")]
        public DateTime Data { get; set; }

        public int idRegional { get; set; }
        public Regional Regional { get; set; }

        [Required(ErrorMessage = "O campo mensagem é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo mensagem deve conter mais do que 100 caracteres")]
        public string Mensagem { get; set; }
    }
}