using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Manager.Models;
using System.Linq;

namespace Manager.Repository
{
    public class DiretoriasRepository : BaseRepository, IBaseRepository
    {
        //le um registro
        
        public Diretoria Load(int id)
        {
            Dictionary<string, object> dicParameters = new Dictionary<string, object>();
            dicParameters.Add("@id", id);

            List<Diretoria> listDiretoria = new List<Diretoria>();

            DataTable dt = ReadFromStoredProcedure("PC_SEL_DIRETORIAS", dicParameters);

            //Bind EmpModel generic list using LINQ 
            listDiretoria = (from DataRow dr in dt.Rows
                             select ParseReader(dr)).ToList();

            return listDiretoria.FirstOrDefault();
        }

        //adiciona um registro
        public List<Diretoria> List()
        {
            List<Diretoria> listDiretoria = new List<Diretoria>();

            Dictionary<string, object> dicParameters = new Dictionary<string, object>();
            
            DataTable dt = ReadFromStoredProcedure("PC_SEL_DIRETORIAS");

            //Bind EmpModel generic list using LINQ 
            listDiretoria = (from DataRow dr in dt.Rows
                      select ParseReader(dr)).ToList();
                        
            return listDiretoria;


        }
        //atualiza um registro
        public bool InsertUpdate(Diretoria obj)
        {
            var id_regionais = obj.id_regionais != null ?
                                string.Join(",", obj.id_regionais)
                              : null;

            Dictionary<string, object> dicParameters = new Dictionary<string, object>()
            {
                { "@id", obj.id },
                { "@diretoria", obj.diretoria },
                { "@id_regionais", id_regionais }
            };

            int i = UpdateFromStoredProcedure("PC_IU_DIRETORIAS", dicParameters);

            return (i >= 1);

        }
        //excluir um registro
        public bool Delete(int id)
        {
            Dictionary<string, object> dicParameters = new Dictionary<string, object>();
            dicParameters.Add("@id", id);
            
            int i = UpdateFromStoredProcedure("PC_EXCLUIR_DIRETORIA", dicParameters);
            
            return (i >= 1);

        }
        
        protected Diretoria ParseReader(DataRow dr)
        {
            RegionaisRepository regionaisRepo = new RegionaisRepository();

            return new Diretoria()
            {
                id = Convert.ToInt32(dr["id"]),
                diretoria = Convert.ToString(dr["diretoria"]),
                regionais = regionaisRepo.ListByDiretoria(Convert.ToInt32(dr["id"]))
            };
        }
    }
}