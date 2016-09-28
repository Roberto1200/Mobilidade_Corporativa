using Manager.Helpers;
using Manager.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Manager.Repository
{
    public class LembretesRepository : BaseRepository, IBaseRepository
    {
        #region [Public]
        public IEnumerable<Lembretes> Load()
        {
            List<Lembretes> EmpList = new List<Lembretes>();

            DataTable dt = ReadFromStoredProcedure("PC_SEL_LEMBRETES");

            //Bind EmpModel generic list using LINQ 
            EmpList = (from DataRow dr in dt.Rows
                       select ParseReader(dr)).ToList();

            return EmpList;
        }
        public IEnumerable<Lembretes> Load(DateTime? dataInicio, DateTime? dataFim, int? regional, string procura)
        {
            List<Lembretes> EmpList = new List<Lembretes>();

            Dictionary<string, object> dicParameters = new Dictionary<string, object>();
            if(dataInicio.HasValue)
                dicParameters.Add("@dataInicio", base._funcoesDiversas.PrimeiraHora(dataInicio.Value));
            if (dataFim.HasValue)
                dicParameters.Add("@dataFim", base._funcoesDiversas.UltimaHora(dataFim.Value));
            if (regional.HasValue)
                dicParameters.Add("@regional", regional);
            if (!string.IsNullOrWhiteSpace(procura))
                dicParameters.Add("@procura", procura);

            DataTable dt = ReadFromStoredProcedure("PC_SEL_LEMBRETES", dicParameters);

            //Bind EmpModel generic list using LINQ 
            EmpList = (from DataRow dr in dt.Rows
                       select ParseReader(dr)).ToList();

            return EmpList;
        }
        public Lembretes Load(int id)
        {
            List<Lembretes> EmpList = new List<Lembretes>();

            Dictionary<string, object> dicParameters = new Dictionary<string, object>();
            dicParameters.Add("@id", id);

            DataTable dt = ReadFromStoredProcedure("PC_SEL_LEMBRETES", dicParameters);

            //Bind EmpModel generic list using LINQ 
            EmpList = (from DataRow dr in dt.Rows
                       select ParseReader(dr)).ToList();

            return EmpList.FirstOrDefault();
        }

        public bool Delete(int id)
        {
            Dictionary<string, object> dicParameters = new Dictionary<string, object>();
            dicParameters.Add("@id", id);

            int i = UpdateFromStoredProcedure("PC_EXCLUIR_LEMBRETE", dicParameters);

            return (i >= 1);
        }
        #endregion

        #region [Privates]
        protected Lembretes ParseReader(DataRow dr)
        {
            return new Lembretes()
            {
                id = Convert.ToInt32(dr["id"]),
                Data = Convert.ToDateTime(dr["data"]),
                idRegional = Convert.ToInt32(dr["idRegional"]),
                Mensagem = Convert.ToString(dr["mensagem"]),
                Regional = new Regional() { regional = Convert.ToString(dr["regional"]) }
            };
        }
        #endregion
    }
}