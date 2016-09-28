using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Manager.Models;
using System.Linq;
using Manager.Helpers;
using System.Security.Cryptography;
using System.Text.RegularExpressions;


namespace Manager.Repository
{
    public abstract class BaseRepository : IBaseRepository
    {
        private SqlConnection con;
        protected FuncoesDiversas _funcoesDiversas;

        public bool success { get; set; }
        public string message { get; set; }

        public BaseRepository()
        {
            this._funcoesDiversas = new FuncoesDiversas();
        }

        private SqlCommand LoadStoredProcedure(string spName, Dictionary<string, object> parameters = null)
        {
            connection();

            SqlCommand cmd = new SqlCommand(spName, con);

            SetCmdParameters(ref cmd, parameters);

            return cmd;

        }

        private void SetCmdParameters(ref SqlCommand cmd, Dictionary<string, object> parameters = null)
        {
            cmd.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
            {
                foreach (KeyValuePair<string, object> item in parameters)
                {
                    cmd.Parameters.AddWithValue(item.Key, item.Value);
                }
            }
        }

        private Exception CustomException(string nameSP, Dictionary<string, object> parameters = null, Exception innerException = null)
        {

            System.Text.StringBuilder strBuilderError = new System.Text.StringBuilder();

            strBuilderError = strBuilderError.AppendFormat("Erro ao executar Stored Procedure:", nameSP).AppendLine();
            strBuilderError = strBuilderError.AppendFormat("- Nome Stored Procedure = {0}", nameSP).AppendLine();

            if (parameters != null)
            {
                strBuilderError = strBuilderError.AppendLine("- Parâmetros:");
                foreach (var p in parameters)
                {
                    strBuilderError = strBuilderError.AppendFormat("-- {0} = {1}", p.Key, p.Value).AppendLine();
                }
            }
            else
            {
                strBuilderError = strBuilderError.AppendLine("- Parâmetros: VAZIO");
            }

            strBuilderError = strBuilderError.AppendFormat("- Descrição: {0}", innerException.Message).AppendLine();

            return new Exception(strBuilderError.ToString(), innerException);
        }

        private void CloseConnection()
        {
            if (con.State == ConnectionState.Open)
                con.Close();
        }

        protected void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["bradesco_app"].ToString();
            con = new SqlConnection(constr);

        }

        protected DataTable ReadFromStoredProcedure(string nameSP, Dictionary<string, object> parameters = null)
        {

            DataTable dt = new DataTable();

            try
            {
                SqlCommand com = LoadStoredProcedure(nameSP, parameters);
                SqlDataAdapter da = new SqlDataAdapter(com);

                con.Open();
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                throw CustomException(nameSP, parameters, ex);

            }
            finally
            {
                CloseConnection();
            }

            return dt;

        }

        protected int UpdateFromStoredProcedure(string nameSP, Dictionary<string, object> parameters = null)
        {
            int result = 0;

            try
            {
                SqlCommand com = LoadStoredProcedure(nameSP, parameters);
                con.Open();
                result = com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw CustomException(nameSP, parameters, ex);
            }
            finally
            {
                CloseConnection();
            }

            return result;
        }

    }
}