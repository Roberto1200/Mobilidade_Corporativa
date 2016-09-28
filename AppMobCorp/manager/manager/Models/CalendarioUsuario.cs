namespace Manager.Models
{
    public class CalendarioUsuario
    {
        public int idDiretoria { get; set; }
        public int idRegional { get; set; }
        public int idUsuarioLider { get; set; }
        public int idUsuario { get; set; }
        public string Diretoria { get; set; }
        public string Regional { get; set; }
        public string Lider { get; set; }
        public string Usuario { get; set; }
    }
}