using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Manager.Models;
using System.Linq;
using System.Web;
using Manager.Helpers;
using System.Security.Cryptography;

namespace Manager.Repository
{
    public class CanaisDigitaisRepository
    {
        private SqlConnection con;
        //cria uma instancia da conexao
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["bradesco_app"].ToString();
            con = new SqlConnection(constr);

        }
        #region Classes Load para os Canais
        /******************************************
         * Le registros de acordo com o solicitado*
         ******************************************/
        public CanaisDigitais LoadCanais(int id)
        {
            connection();
            List<CanaisDigitais> EmpList = new List<CanaisDigitais>();
            SqlCommand com = new SqlCommand("PC_SEL_CANAIS", con);
            com.Parameters.AddWithValue("@id", id);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();
            //Bind EmpModel generic list using LINQ 
            EmpList = (from DataRow dr in dt.Rows
                       select ReturnCanais(dr)).ToList();

            return EmpList[0];
        }
        public CanaisDigitais_Subcat LoadCategorias(int id)
        {
            connection();
            List<CanaisDigitais_Subcat> EmpList = new List<CanaisDigitais_Subcat>();
            SqlCommand com = new SqlCommand("PC_SEL_CANAIS_SUBCAT", con);
            com.Parameters.AddWithValue("@id", id);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();
            //Bind EmpModel generic list using LINQ 
            EmpList = (from DataRow dr in dt.Rows
                       select ReturnCategorias(dr)).ToList();

            return EmpList[0];
        }
        public CanaisDitais_subcat_conteudo LoadConteudo(int id)
        {
            connection();
            List<CanaisDitais_subcat_conteudo> EmpList = new List<CanaisDitais_subcat_conteudo>();
            SqlCommand com = new SqlCommand("PC_SEL_CANAIS_SUBCAT_CONTEUDO", con);
            com.Parameters.AddWithValue("@id", id);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();
            //Bind EmpModel generic list using LINQ 
            EmpList = (from DataRow dr in dt.Rows
                       select ReturnConteudo(dr)).ToList();

            return EmpList[0];
        }
        /***********************************************************************************************/
        #endregion

        #region Classes List para os canais
        /*******************************************************
         * lista os registros dentro das categorias dos canais *
         *******************************************************/
        public List<CanaisDigitais> ListCanais()
        {
            connection();
            List<CanaisDigitais> EmpList = new List<CanaisDigitais>();
            SqlCommand com = new SqlCommand("PC_SEL_CANAIS", con);
            com.Parameters.AddWithValue("@id", -1);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            //Bind EmpModel generic list using LINQ 
            EmpList = (from DataRow dr in dt.Rows
                       select ReturnCanais(dr)).ToList();

            return EmpList;
        }
        public List<CanaisDigitais_Subcat> ListSubcategoria(int id)
        {
            connection();
            List<CanaisDigitais_Subcat> EmpList = new List<CanaisDigitais_Subcat>();
            SqlCommand com = new SqlCommand("PC_SEL_CANAIS_SUBCAT", con);
            com.Parameters.AddWithValue("@ID_CANAL_DIGITAL", id);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            //Bind EmpModel generic list using LINQ 
            EmpList = (from DataRow dr in dt.Rows
                       select ReturnCategorias(dr)).ToList();

            return EmpList;
        }
        public List<CanaisDitais_subcat_conteudo> ListConteudo(int id)
        {
            connection();
            List<CanaisDitais_subcat_conteudo> EmpList = new List<CanaisDitais_subcat_conteudo>();
            SqlCommand com = new SqlCommand("PC_SEL_CANAIS_SUBCAT_CONTEUDO", con);
            com.Parameters.AddWithValue("@ID_SUBCAT", id);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            //Bind EmpModel generic list using LINQ 
            EmpList = (from DataRow dr in dt.Rows
                       select ReturnConteudo(dr)).ToList();

            return EmpList;
        }
        #endregion

        #region Classes InsertUpdate para os Canais
        //atualiza um registro
        public bool InsertUpdate(CanaisDigitais obj)
        {
            connection();
            SqlCommand com = new SqlCommand("[PC_IU_CANAIS]", con);


            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@id", obj.id);
            com.Parameters.AddWithValue("@nome", obj.nome);
            com.Parameters.AddWithValue("@subcategoria_padrao", obj.subcategoria_padrao);
            com.Parameters.AddWithValue("@imagem_banner", obj.imagem_banner);

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
        public bool InsertUpdateCat(CanaisDigitais_Subcat obj)
        {
            connection();
            SqlCommand com = new SqlCommand("[PC_IU_CANAIS_SUBCAT]", con);


            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@id", obj.id);
            com.Parameters.AddWithValue("@nome", obj.nome);
            com.Parameters.AddWithValue("@id_canal_digital", obj.id_canal_digital);
            com.Parameters.AddWithValue("@chamada", obj.chamada);
            com.Parameters.AddWithValue("@imagem", obj.imagem);
            com.Parameters.AddWithValue("@id_tipo_banner_destaque", obj.id_tipo_banner_destaque);
            com.Parameters.AddWithValue("@id_tipo_pagina", obj.id_tipo_pagina);
            com.Parameters.AddWithValue("@conteudo", obj.conteudo);

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
        public bool InsertUpdateCont(CanaisDitais_subcat_conteudo obj)
        {
            connection();
            SqlCommand com = new SqlCommand("[PC_IU_CANAIS_SUBCAT_CONTEUDO]", con);


            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@id", obj.id);
            com.Parameters.AddWithValue("@id_subcat", obj.id_subcat);
            com.Parameters.AddWithValue("@nome", obj.nome);
            com.Parameters.AddWithValue("@banner", obj.banner);

            com.Parameters.AddWithValue("@conteudo", obj.conteudo);

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
        #endregion

        //excluir um registro
        public bool DeleteSub(int Id)
        {

            connection();
            SqlCommand com = new SqlCommand("PC_EXCLUIR_CANAIS_SUBCAT", con);

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
        public bool DeleteCont(int Id)
        {

            connection();
            SqlCommand com = new SqlCommand("PC_EXCLUIR_CANAIS_SUBCAT_CONTEUDO", con);

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
        protected CanaisDigitais ReturnCanais(DataRow dr)
        {
            return new CanaisDigitais()
            {
                id = Convert.ToInt32(dr["id"]),
                nome = Convert.ToString(dr["nome"]),
                subcategoria_padrao = Convert.ToInt32(dr["subcategoria_padrao"]),
                quantidade = Convert.ToInt16(dr["quantidade"]),
                imagem_banner = Convert.ToString(dr["imagem_banner"])
            };
        }
        protected CanaisDigitais_Subcat ReturnCategorias(DataRow dr)
        {
            return new CanaisDigitais_Subcat()
            {
                id = Convert.ToInt32(dr["id"]),
                id_canal_digital = Convert.ToInt32(dr["id_canal_digital"]),
                nome = Convert.ToString(dr["nome"]),
                chamada = Convert.ToString(dr["chamada"]),
                imagem = Convert.ToString(dr["imagem"]),
                id_tipo_banner_destaque = Convert.ToInt32(dr["id_tipo_banner_destaque"]),
                id_tipo_pagina = Convert.ToInt32(dr["id_tipo_pagina"]),
                data_criacao = Convert.ToString(dr["data_criacao"]),
                quantidade = Convert.ToInt16(dr["quantidade"]),
                conteudo = Convert.ToString(dr["conteudo"]),
                nome_canal = Convert.ToString(dr["nome_canal"])
            };
        }
        protected CanaisDitais_subcat_conteudo ReturnConteudo(DataRow dr)
        {
            CanaisDitais_subcat_conteudo conteudo = new CanaisDitais_subcat_conteudo();

            conteudo.id = Convert.ToInt32(dr["id"]);
            conteudo.id_subcat = Convert.ToInt32(dr["id_subcat"]);
            conteudo.nome = Convert.ToString(dr["nome"]);
            conteudo.banner = Convert.ToString(dr["banner"]);
            conteudo.conteudo = HttpUtility.HtmlDecode(Convert.ToString(dr["conteudo"]));
            conteudo.nome_categoria = Convert.ToString(dr["nome_categoria"]);
            conteudo.nome_canal = Convert.ToString(dr["nome_canal"]);
            conteudo.id_canal = Convert.ToInt32(dr["id_canal"]);
            return conteudo;
            
        }
    }
}