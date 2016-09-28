using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Manager.Models;
using System.Linq;

namespace Manager.Repository
{
    public class AgenciasRepository : BaseRepository, IBaseRepository
    {
        //le um registro
        public Agencias Load(int id)
        {
            List<Agencias> EmpList = new List<Agencias>();

            Dictionary<string, object> dicParameters = new Dictionary<string, object>();
            dicParameters.Add("@id", id);

            DataTable dt = ReadFromStoredProcedure("PC_SEL_AGENCIAS", dicParameters);

            //Bind EmpModel generic list using LINQ 
            EmpList = (from DataRow dr in dt.Rows
                       select ParseReader(dr)).ToList();

            return EmpList[0];


        }
        //adiciona um registro
        public List<Agencias> List()
        {
            List<Agencias> EmpList = new List<Agencias>();

            Dictionary<string, object> dicParameters = new Dictionary<string, object>();

            DataTable dt = ReadFromStoredProcedure("PC_SEL_AGENCIAS");

            //Bind EmpModel generic list using LINQ 
            EmpList = (from DataRow dr in dt.Rows
                       select ParseReader(dr)).ToList();

            return EmpList;
        }
        //atualiza um registro
        public bool InsertUpdate(Agencias obj)
        {

            Dictionary<string, object> dicParameters = new Dictionary<string, object>()
            {
                { "@id", obj.id },
                { "@agencia", obj.agencia },
                { "@id_regional", obj.id_regional }
            };

            int i = UpdateFromStoredProcedure("PC_IU_AGENCIAS", dicParameters);

            return (i >= 1);

        }
        //excluir um registro
        public bool Delete(int id)
        {
            Dictionary<string, object> dicParameters = new Dictionary<string, object>();
            dicParameters.Add("@id", id);

            int i = UpdateFromStoredProcedure("PC_EXCLUIR_AGENCIA", dicParameters);

            return (i >= 1);

        }

        protected Agencias ParseReader(DataRow dr)
        {
            return new Agencias()
            {
                id = Convert.ToInt32(dr["id"]),
                agencia = Convert.ToString(dr["agencia"]),
                id_regional  = Convert.ToInt32(dr["id_regional"]),
                regional = Convert.ToString(dr["regional"])
            };
        }
    }
}