using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Manager.Models;

namespace Manager.Repository
{
    public class ForunsRepository : BaseRepository
    {
        #region CANAIS
        public List<Forum_Canal> ListCanais()
        {
            List<Forum_Canal> EmpList = new List<Forum_Canal>();
            DataTable dt = ReadFromStoredProcedure("PC_SEL_FORUM_CANAL");


            //Bind EmpModel generic list using LINQ 
            EmpList = (from DataRow dr in dt.Rows
                       select ParseCanal(dr)).ToList();

            return EmpList;
        }
        public Forum_Canal LoadCanais(int id)
        {
            List<Forum_Canal> EmpList = new List<Forum_Canal>();
            Dictionary<string, object> dicParameters = new Dictionary<string, object>();
            DataTable dt;
            dicParameters.Add("@id", id);
            dt = ReadFromStoredProcedure("PC_SEL_FORUM_CANAL", dicParameters);

            //Bind EmpModel generic list using LINQ 
            EmpList = (from DataRow dr in dt.Rows
                       select ParseCanal(dr)).ToList();

            return EmpList[0];
        }
        public bool InsertUpdateCanal(Forum_Canal modelo)
        {
            Dictionary<string, object> dicParameters = new Dictionary<string, object>()
            {
                { "@id",modelo.id },
                { "@titulo",modelo.titulo }
            };

            int i = UpdateFromStoredProcedure("PC_IU_FORUM_CANAL", dicParameters);
            if (i >= 1)
            {
                return true;
            }
            return false;
        }
        protected Forum_Canal ParseCanal(DataRow dr)
        {
            return new Forum_Canal()
            {

                id = Convert.ToInt32(dr["id"]),
                titulo = Convert.ToString(dr["titulo"]),
                quantidade = Convert.ToInt32(dr["quantidade"])
            };
        }
        public bool Delete(int id)
        {
            Dictionary<string, object> dicParameters = new Dictionary<string, object>();
            dicParameters.Add("@id", id);

            int i = UpdateFromStoredProcedure("PC_EXCLUIR_CANAL", dicParameters);
            return (i >= 1);
        }
        #endregion
        #region Topicos
        public List<Forum_Canal_Topico> ListTopicos(int id)
        {
            Dictionary<string, object> dicParameters = new Dictionary<string, object>();
            DataTable dt;
            dicParameters.Add("@ID_FORUM", id);
            List<Forum_Canal_Topico> EmpList = new List<Forum_Canal_Topico>();
            dt = ReadFromStoredProcedure("PC_SEL_FORUM_TOPICO",dicParameters);


            //Bind EmpModel generic list using LINQ 
            EmpList = (from DataRow dr in dt.Rows
                       select ParseTopicos(dr)).ToList();

            return EmpList;
        }
        public Forum_Canal_Topico LoadTopicos(int id)
        {
            List<Forum_Canal_Topico> EmpList = new List<Forum_Canal_Topico>();
            Dictionary<string, object> dicParameters = new Dictionary<string, object>();
            DataTable dt;
            dicParameters.Add("@id", id);
            dt = ReadFromStoredProcedure("PC_SEL_FORUM_TOPICO", dicParameters);

            //Bind EmpModel generic list using LINQ 
            EmpList = (from DataRow dr in dt.Rows
                       select ParseTopicos(dr)).ToList();

            return EmpList[0];
        }
        public bool InsertUpdateTopicos(Forum_Canal_Topico modelo)
        {
            Dictionary<string, object> dicParameters = new Dictionary<string, object>()
            {
                { "@id",modelo.id },
                { "@id_canal",modelo.id_canal },
                { "@titulo",modelo.titulo }
            };

            int i = UpdateFromStoredProcedure("PC_IU_FORUM_TOPICO", dicParameters);
            if (i >= 1)
            {
                return true;
            }
            return false;
        }
        public bool DeleteTopicos(int id)
        {
            Dictionary<string, object> dicParameters = new Dictionary<string, object>();
            dicParameters.Add("@id", id);

            int i = UpdateFromStoredProcedure("PC_EXCLUIR_TOPICO", dicParameters);
            return (i >= 1);
        }
        protected Forum_Canal_Topico ParseTopicos(DataRow dr)
        {
            return new Forum_Canal_Topico()
            {

                id = Convert.ToInt32(dr["id"]),
                titulo = Convert.ToString(dr["titulo"]),
                id_canal = Convert.ToInt32(dr["id_canal"]),
                quantidade = Convert.ToInt32(dr["quantidade"])
            };
        }
        #endregion
        #region Posts
        public List<Forum_Canal_Topico_Post> ListPosts(int id)
        {
            Dictionary<string, object> dicParameters = new Dictionary<string, object>();
            DataTable dt;
            dicParameters.Add("@ID_TOPICO", id);
            List<Forum_Canal_Topico_Post> EmpList = new List<Forum_Canal_Topico_Post>();
            dt = ReadFromStoredProcedure("PC_SEL_FORUM_POST", dicParameters);


            //Bind EmpModel generic list using LINQ 
            EmpList = (from DataRow dr in dt.Rows
                       select ParsePosts(dr)).ToList();

            return EmpList;
        }
        public Forum_Canal_Topico_Post LoadPosts(int id)
        {
            List<Forum_Canal_Topico_Post> EmpList = new List<Forum_Canal_Topico_Post>();
            Dictionary<string, object> dicParameters = new Dictionary<string, object>();
            DataTable dt;
            dicParameters.Add("@id", id);
            dt = ReadFromStoredProcedure("PC_SEL_FORUM_POST", dicParameters);

            //Bind EmpModel generic list using LINQ 
            EmpList = (from DataRow dr in dt.Rows
                       select ParsePosts(dr)).ToList();

            return EmpList[0];
        }
        public bool InsertUpdatePosts(Forum_Canal_Topico_Post modelo)
        {
            if (modelo.id > 0)
            {
                modelo.data_edicao = DateTime.Today;
            }
            if (modelo.id == 0)
            {
                modelo.data_inicio = DateTime.Today;
            }
            Dictionary<string, object> dicParameters = new Dictionary<string, object>()
            {
                { "@id",modelo.id },
                { "@id_topico",modelo.id_topico },
                { "@mensagem",modelo.mensagem },
                { "@data_inicio",modelo.data_inicio},
            };
            if (modelo.id > 0)
            {
                dicParameters.Add("@data_edicao", modelo.data_edicao);
                dicParameters.Add("@codigo_funcional", modelo.codigo_funcional);
            }
            int i = UpdateFromStoredProcedure("PC_IU_FORUM_POST", dicParameters);
            if (i >= 1)
            {
                return true;
            }
            return false;
        }
        public bool DeletePosts(int id)
        {
            Dictionary<string, object> dicParameters = new Dictionary<string, object>();
            dicParameters.Add("@id", id);

            int i = UpdateFromStoredProcedure("PC_EXCLUIR_POST", dicParameters);
            return (i >= 1);
        }
        protected Forum_Canal_Topico_Post ParsePosts(DataRow dr)
        {
            return new Forum_Canal_Topico_Post()
            {

                id = Convert.ToInt32(dr["id"]),
                id_topico = Convert.ToInt32(dr["id_topico"]),
                mensagem = Convert.ToString(dr["mensagem"]),
                data_inicio = Convert.ToDateTime(dr["data_inicio"]),
                data_edicao = Convert.ToDateTime(dr["data_edicao"]),
                nome = Convert.ToString(dr["nome"]),
                avatar = Convert.ToString(dr["avatar"])
            };
        }
        #endregion
    }
}