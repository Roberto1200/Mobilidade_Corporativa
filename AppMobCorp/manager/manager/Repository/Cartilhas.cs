using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Manager.Models;
using System.Linq;

namespace Manager.Repository
{
    public class CartilhasRepository
    {
        private SqlConnection con;
        //cria uma instancia da conexao
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["bradesco_app"].ToString();
            con = new SqlConnection(constr);

        }
        //le um registro
        public Cartilha Load(int id)
        {
            connection();
            List<Cartilha> EmpList = new List<Cartilha>();
            SqlCommand com = new SqlCommand("PC_SEL_CARTILHAS", con);
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
        //adiciona um registro
        public List<Cartilha> List()
        {
            return List("");
        }
        public List<Cartilha> List(string app)
        {
            connection();
            List<Cartilha> EmpList = new List<Cartilha>();
            SqlCommand com = new SqlCommand("PC_SEL_CARTILHAS", con);
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
        public bool InsertUpdate(Cartilha obj)
        {

            connection();
            SqlCommand com = new SqlCommand("[PC_IU_CARTILHAS]", con);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@id", obj.id);
            com.Parameters.AddWithValue("@titulo", obj.titulo);
            com.Parameters.AddWithValue("@data", obj.data);
            com.Parameters.AddWithValue("@size", obj.size);
            com.Parameters.AddWithValue("@arquivo", obj.arquivo);
            com.Parameters.AddWithValue("@status", obj.status);
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
            SqlCommand com = new SqlCommand("PC_EXCLUIR_CARTILHAS", con);

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

        protected Cartilha ParseReader(DataRow dr)
        {
            if (Convert.ToBoolean(dr["status"])) {
 }
            return new Cartilha()
                       {
                           id = Convert.ToInt32(dr["id"]),
                           titulo = Convert.ToString(dr["titulo"]),
                           data = Convert.ToString(dr["data"]),
                           size = Convert.ToString(dr["size"]),
                           arquivo = Convert.ToString(dr["arquivo"]),
                           status = Convert.ToBoolean(dr["status"]),
                           str_status = Convert.ToString(dr["str_status"])
            };
        }
    }
}