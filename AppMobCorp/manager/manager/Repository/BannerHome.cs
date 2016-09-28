using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Manager.Models;
using System.Linq;

namespace Manager.Repository
{
    public class BannerHomeRepository
    {
        private SqlConnection con;
        //cria uma instancia da conexao
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["bradesco_app"].ToString();
            con = new SqlConnection(constr);

        }
        //LE UM REGISTRO
        public BannerHome Load(int id)
        {
            connection();
            List<BannerHome> EmpList = new List<BannerHome>();
            SqlCommand com = new SqlCommand("PC_SEL_BANNERS_HOME", con);
            com.Parameters.AddWithValue("@id", id);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            //Bind EmpModel generic list using LINQ 
            EmpList = (from DataRow dr in dt.Rows
                       select new BannerHome()
                       {
                           id = Convert.ToInt32(dr["id"]),
                           id_tipo_app = Convert.ToInt32(dr["id_tipo_app"]),
                           titulo = Convert.ToString(dr["titulo"]),
                           texto = Convert.ToString(dr["texto"]),
                           status = Convert.ToBoolean(dr["status"]),
                           imagem = Convert.ToString(dr["imagem"]),
                           txt_tipo_app = Convert.ToString(dr["txt_tipo_app"])
                       }).ToList();

            return EmpList[0];


        }
        //lista os registros
        public List<BannerHome> List()
        {
            return List("");
        }
        public List<BannerHome> List(string app)
        {
            connection();
            List<BannerHome> EmpList = new List<BannerHome>();
            SqlCommand com = new SqlCommand("PC_SEL_BANNERS_HOME", con);
            com.Parameters.AddWithValue("@app", app);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            //Bind EmpModel generic list using LINQ 
            EmpList = (from DataRow dr in dt.Rows
                       select new BannerHome()
                       {
                           id = Convert.ToInt32(dr["id"]),
                           id_tipo_app = Convert.ToInt32(dr["id_tipo_app"]),
                           titulo = Convert.ToString(dr["titulo"]),
                           texto =Convert.ToString(dr["texto"]),
                           status = Convert.ToBoolean(dr["status"]),
                           imagem =Convert.ToString(dr["imagem"]),
                           txt_tipo_app =Convert.ToString(dr["txt_tipo_app"]),
                           str_status = Convert.ToString(dr["str_status"])}).ToList();
                        
            return EmpList;


        }
        //atualiza um registro
        public bool InsertUpdate(BannerHome obj)
        {

            connection();
            SqlCommand com = new SqlCommand("[PC_IU_BANNER_HOME]", con);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@id", obj.id);
            com.Parameters.AddWithValue("@id_tipo_app", obj.id_tipo_app);
            com.Parameters.AddWithValue("@titulo", obj.titulo);
            com.Parameters.AddWithValue("@texto", obj.texto);
            com.Parameters.AddWithValue("@status", obj.status);
            com.Parameters.AddWithValue("@imagem", obj.imagem);
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
            SqlCommand com = new SqlCommand("PC_EXCLUIR_BANNER", con);

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
    }
}