using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Manager.Models;
using System.Linq;

namespace Manager.Repository
{
    public class UsuariosTipoAppRepository
    {
        private SqlConnection con;
        //cria uma instancia da conexao
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["bradesco_app"].ToString();
            con = new SqlConnection(constr);

        }     
        public List<UsuarioTipo> List()
        {
            connection();
            List<UsuarioTipo> EmpList = new List<UsuarioTipo>();
            SqlCommand com = new SqlCommand("SEL_USUARIOS_TIPO", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            //Bind EmpModel generic list using LINQ 
            EmpList = (from DataRow dr in dt.Rows
                       select new UsuarioTipo()
                       {
                           id = Convert.ToInt32(dr["id"]),
                           tipo = Convert.ToString(dr["tipo"]),
                       }).ToList();


            return EmpList;


        }
        //atualiza um registro       
    }
}