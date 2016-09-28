using Manager.Repository;
using PagedList;
using System.Linq;
using System.Web.Mvc;

namespace Manager.Controllers
{
    public class CalendarioController : Controller
    {
        private RegionaisRepository _regionaisRepository;
        private DiretoriasRepository _diretoriasRepository;
        private UsuarioseRepository _usuariosRepository;

        public CalendarioController()
        {
            this._diretoriasRepository = new DiretoriasRepository();
            this._regionaisRepository = new RegionaisRepository();
            this._usuariosRepository = new UsuarioseRepository();
        }

        public ActionResult Index(int? pagina, int? idDiretoria, int? idRegional, string coord_lider)
        {
            int paginaTamanho = 10;
            int paginaNumero = (pagina ?? 1);

            //TODO: Executar busca de acordo com filtro de Calendário
            ViewBag.CoordLider = coord_lider;
            ViewBag.Diretorias = this._diretoriasRepository.List();
            ViewBag.Regionais = this._regionaisRepository.List();

            var usuarios = this._usuariosRepository.List().Where(x => x.status);

            pagina = (coord_lider != null ? 1 : 0);
            
            var procurar = from s in usuarios select s;
            
            if (!string.IsNullOrEmpty(coord_lider))
            {
                procurar = usuarios.Where(s => s.usuario.ToLower().Contains(coord_lider.ToLower()));
            }

            return View(procurar.ToPagedList(paginaNumero, paginaTamanho));
        }
    }
}
