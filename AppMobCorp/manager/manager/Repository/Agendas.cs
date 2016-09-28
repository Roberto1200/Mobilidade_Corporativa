using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Manager.Models;
using Manager.Helpers;
using System.Data.SqlClient;
using System.Data;

namespace Manager.Repository
{
    public class Agendas : BaseRepository
    {
        public List<Agenda> List()
        {
            return List("");
        }
        public List<Agenda> List(string codFunc)
        {
            List<Agenda> EmpList = new List<Agenda>();

            Dictionary<string, object> dicParameters = new Dictionary<string, object>();
            DataTable dt;
            if (string.IsNullOrWhiteSpace(codFunc))
            {
                dicParameters.Add("@codFunc", string.Empty);
                dt = ReadFromStoredProcedure("PC_SEL_CALENDARIO_EVENTO", dicParameters);
            }
            else
            {
                dicParameters.Add("@codFunc", codFunc);
                dt = ReadFromStoredProcedure("PC_SEL_CALENDARIO_EVENTO", dicParameters);
            }


            //Bind EmpModel generic list using LINQ 
            EmpList = (from DataRow dr in dt.Rows
                       select ParseReader(dr)).ToList();

            return EmpList;


        }
        public AgendaTipo LoadTipos(int id)
        {
            List<AgendaTipo> EmpList = new List<AgendaTipo>();

            Dictionary<string, object> dicParameters = new Dictionary<string, object>();
            DataTable dt;
            dicParameters.Add("@id", id);
            dt = ReadFromStoredProcedure("PC_SEL_AGENDA_TIPO_ITENS",dicParameters);


            //Bind EmpModel generic list using LINQ 
            EmpList = (from DataRow dr in dt.Rows
                       select ParseAgendaTipo(dr)).ToList();

            return EmpList[0];


        }
        public List<AgendaTipo> ListTipos()
        {
            List<AgendaTipo> EmpList = new List<AgendaTipo>();

            Dictionary<string, object> dicParameters = new Dictionary<string, object>();
            DataTable dt;
            dt = ReadFromStoredProcedure("PC_SEL_AGENDA_TIPO_ITENS");


            //Bind EmpModel generic list using LINQ 
            EmpList = (from DataRow dr in dt.Rows
                       select ParseAgendaTipo(dr)).ToList();

            return EmpList;


        }
        public bool InsertUpdate(Agenda modelo)
        {
            Dictionary<string, object> dicParameters = new Dictionary<string, object>() {
                { "@id",modelo.id },
                { "@codFunc",modelo.codigo_funcional },
                { "@tipoevento",modelo.tipoevento },
                { "@codigoagencia",modelo.codigoagencia },
                { "@responsavel", modelo.responsavel },
                { "@data", modelo.data },
                { "@observacoes", modelo.observacoes },
                { "@feedback", modelo.feedback }

            };

            int i = UpdateFromStoredProcedure("PC_IU_CALENDARIO_EVENTO", dicParameters);

            if (i >= 1)
            {
                return true;
            }
            return false;
        }
        public bool InsertUpdateTipos(AgendaTipo modelo)
        {
            Dictionary<string, object> dicParameters = new Dictionary<string, object>() {
                { "@id",modelo.id },
                { "@tipo",modelo.tipo }

            };

            int i = UpdateFromStoredProcedure("PC_IU_AGENDA_TIPO_ITENS", dicParameters);

            if (i >= 1)
            {
                return true;
            }
            return false;
        }
        public bool DeleteTipos(int id)
        {
            Dictionary<string, object> dicParameters = new Dictionary<string, object>() {
                { "@id",id },

            };

            int i = UpdateFromStoredProcedure("PC_EXCLUIR_AGENDA_TIPO_ITENS", dicParameters);

            if (i >= 1)
            {
                return true;
            }
            return false;
        }
        protected Agenda ParseReader(DataRow dr)
        {
            return new Agenda()
            {

                id = Convert.ToInt32(dr["id"]),
                codigo_funcional = Convert.ToString(dr["codigo_funcional"]),
                tipoevento = Convert.ToString(dr["tipoevento"]),
                codigoagencia = Convert.ToString(dr["codigoagencia"]),
                responsavel = Convert.ToString(dr["responsavel"]),
                data = Convert.ToDateTime(dr["data"]),
                observacoes = Convert.ToString(dr["observacoes"]),
                feedback = Convert.ToString(dr["feedback"])
            };
        }
        protected AgendaTipo ParseAgendaTipo(DataRow dr)
        {
            return new AgendaTipo()
            {

                id = Convert.ToInt32(dr["id"]),
                tipo = Convert.ToString(dr["tipo"])
            };
        }
    }
}