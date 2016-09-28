using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Manager.Models;
using System.Linq;

namespace Manager.Repository
{
    public class BannerTipoAppRepository
    {
        private SqlConnection con;
        //cria uma instancia da conexao
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["bradesco_app"].ToString();
            con = new SqlConnection(constr);

        }     
        public List<BannerTipoApp> List()
        {
            connection();
            List<BannerTipoApp> EmpList = new List<BannerTipoApp>();
            SqlCommand com = new SqlCommand("PC_SEL_BANNER_TIPO_APP", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            //Bind EmpModel generic list using LINQ 
            EmpList = (from DataRow dr in dt.Rows
                       select new BannerTipoApp()
                       {
                           id = Convert.ToInt32(dr["id"]),
                           tipo_app = Convert.ToString(dr["tipo_app"]),
                       }).ToList();


            return EmpList;


        }
        //atualiza um registro       
    }
}