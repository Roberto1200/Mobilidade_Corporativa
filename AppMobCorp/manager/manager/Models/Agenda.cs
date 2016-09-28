using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manager.Models
{
    public class Agenda
    {
        public int id { get; set; }
        public string codigo_funcional { get; set; }
        public string tipoevento { get; set; }
        public string codigoagencia { get; set; }
        public string responsavel { get; set; }
        public DateTime data { get; set; }
        public string observacoes { get; set; }
        public string feedback { get; set; }
    }
    public class AgendaTipo
    {
        public int id { get; set; }
        public string tipo { get; set; }
    }
}