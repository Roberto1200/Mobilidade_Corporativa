using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Manager.Models;
using System.Linq;

namespace Manager.Repository
{
    public class VideosRepository
    {
        private SqlConnection con;
        //cria uma instancia da conexao
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["bradesco_app"].ToString();
            con = new SqlConnection(constr);

        }

        //LE UM REGISTRO
        public Video Load(int id)
        {
            connection();
            List<Video> EmpList = new List<Video>();
            SqlCommand com = new SqlCommand("PC_SEL_VIDEOS", con);
            com.Parameters.AddWithValue("@id", id);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            //Bind EmpModel generic list using LINQ 
            EmpList = (from DataRow dr in dt.Rows
                       select ParseReader(dr)).ToList();

            return EmpList[0];


        }

        //lista os registros
        public List<Video> List()
        {
            return List("");
        }
        public List<Video> List(string app)
        {
            connection();
            List<Video> EmpList = new List<Video>();
            SqlCommand com = new SqlCommand("PC_SEL_VIDEOS", con);
            com.Parameters.AddWithValue("@app", app);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            //Bind EmpModel generic list using LINQ 
            EmpList = (from DataRow dr in dt.Rows
                       select ParseReader(dr)).ToList();
                        
            return EmpList;


        }

        //atualiza um registro        
        public bool InsertUpdate(Video obj)
        {

            connection();
            SqlCommand com = new SqlCommand("[PC_IU_VIDEO]", con);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@id", obj.id);
            com.Parameters.AddWithValue("@titulo", obj.titulo);            
            com.Parameters.AddWithValue("@imagem", obj.imagem);
            com.Parameters.AddWithValue("@video", obj.video);
            com.Parameters.AddWithValue("@status", obj.status);
            com.Parameters.AddWithValue("@size", obj.size);
            com.Parameters.AddWithValue("@duracao", obj.duracao);
            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {
                return true;
            }
            
            else
            {
                return false;
            }

        }

        //excluir um registro
        public bool Delete(int Id)
        {

            connection();
            SqlCommand com = new SqlCommand("PC_EXCLUIR_VIDEO", con);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@id", Id);

            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {

                return true;

            }
            else
            {

                return false;
            }


        }

        protected Video ParseReader(DataRow dr)
        {
            return new Video()
                       {
                           id = Convert.ToInt32(dr["id"]),
                           titulo = Convert.ToString(dr["titulo"]),
                           imagem = Convert.ToString(dr["imagem"]),
                           video = Convert.ToString(dr["video"]),
                           status = Convert.ToBoolean(dr["status"]),
                           str_status = Convert.ToString(dr["str_status"]),
                           data = Convert.ToString(dr["data"]),
                           duracao = Convert.ToString(dr["duracao"]),
                           size = Convert.ToString(dr["size"])
            };
        }
    }

}