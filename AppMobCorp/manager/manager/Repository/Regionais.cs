using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Manager.Models;
using System.Linq;

namespace Manager.Repository
{
    public class RegionaisRepository: BaseRepository, IBaseRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userid">0 retornar tudo, caso contrario retorna do userid</param>
        /// <returns></returns>
        public List<Regional> List(int userid = 0)
        {
            List<Regional> listRegionais = new List<Regional>();

            Dictionary<string, object> dicParameters = new Dictionary<string, object>();
            dicParameters.Add("@userid", userid);

            DataTable dt = ReadFromStoredProcedure("SEL_REGIONAIS", dicParameters);

            //Bind listRegionais generic list using LINQ 
            listRegionais = (from DataRow dr in dt.Rows
                       select new Regional()
                       {
                           id = Convert.ToInt32(dr["id"]),
                           regional = Convert.ToString(dr["regional"]),
                           id_diretoria = Convert.ToInt32(dr["id_diretoria"]),
                           diretoria = Convert.ToString(dr["diretoria"]),
                       }).ToList();

            return listRegionais;


        }
        //atualiza um registro

        public List<Regional> ListByDiretoria(int iddiretoria)
        {
            List<Regional> regionais = new List<Regional>();
            
            Dictionary<string, object> dicParameters = new Dictionary<string, object>();
            dicParameters.Add("@id", iddiretoria);

            DataTable dt = ReadFromStoredProcedure("PC_SEL_REGIONAIS_BY_DIRETORIA", dicParameters);

            //Bind regionais generic list using LINQ 
            regionais = (from DataRow dr in dt.Rows
                         select new Regional()
                         {
                             id = Convert.ToInt32(dr["id"]),
                             regional = Convert.ToString(dr["regional"]).Trim(),
                             id_diretoria = Convert.ToInt32(dr["id_diretoria"]),
                             diretoria = Convert.ToString(dr["diretoria"]).Trim(),
                         }).ToList();

            return regionais;

        }

        public bool InsertUpdate(Regional obj)
        {
            Dictionary<string, object> dicParameters = new Dictionary<string, object>()
            {
                { "@id", obj.id },
                { "@regional", obj.regional },
                { "@id_diretoria", obj.id_diretoria }
            };

            int i = UpdateFromStoredProcedure("PC_IU_REGIONAIS", dicParameters);

            return (i >= 1);
        }

        //excluir um registro
        public bool Delete(int id)
        {
            Dictionary<string, object> dicParameters = new Dictionary<string, object>();
            dicParameters.Add("@id", id);

            int i = UpdateFromStoredProcedure("PC_EXCLUIR_REGIONAL", dicParameters);

            return (i >= 1);

        }
    }
}