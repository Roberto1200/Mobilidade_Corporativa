using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Manager.Models;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using Manager.Helpers;

namespace Manager.Repository
{
    public class AdministradoresRepository
    {
        private SqlConnection con;
        //cria uma instancia da conexao
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["bradesco_app"].ToString();
            con = new SqlConnection(constr);

        }
        //le um registro
        public Administrador Load(string email, string senha,int id=0)
        {
            try
            {
                connection();
                List<Administrador> EmpList = new List<Administrador>();
                SqlCommand com = new SqlCommand("PC_SEL_USUARIO_ADMIN", con);
                com.Parameters.AddWithValue("@email", email);
                com.Parameters.AddWithValue("@senha", senha);
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
            catch (Exception)
            {
                return null;
            }


        }
        //adiciona um registro
        public List<Administrador> List()
        {
            connection();
            List<Administrador> EmpList = new List<Administrador>();
            SqlCommand com = new SqlCommand("PC_SEL_USUARIO_ADMIN", con);
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

        // Check if E-Mail exists
        public bool EmailExists(string email, int id = 0)
        {
            List<Administrador> listAdmins = List();

            return listAdmins.Where(u => u.email.Trim().ToLower() == email.Trim().ToLower() && u.id != id).Any();

        }

        // Check if Nome exists
        public bool NomeExists(string nome, int id = 0)
        {
            List<Administrador> listAdmins = List();

            return listAdmins.Where(u => u.nome.Trim().ToLower() == nome.Trim().ToLower() && u.id != id).Any();

        }
        //atualiza um registro
        public bool InsertUpdate(Administrador obj)
        {
            Cripto md5f = new Cripto();
            string senhamd5 = null;
            
            string plainPassword = obj.id == 0 && string.IsNullOrEmpty(obj.senha) ?
                                        LoginUser.GenerateNewPassword() :
                                        obj.senha;

            if (!string.IsNullOrEmpty(plainPassword))
            {
                using (MD5 md5Hash = MD5.Create())
                {
                    // Create complex password automatically when adding new user and password is empty.
                    senhamd5 = md5f.GetMd5Hash(md5Hash, plainPassword);
                }
            }           

            connection();
            SqlCommand com = new SqlCommand("PC_IU_USUARIOS_ADMIN", con);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@id", obj.id);
            com.Parameters.AddWithValue("@nome", obj.nome);
            com.Parameters.AddWithValue("@email", obj.email);
            com.Parameters.AddWithValue("@senha", senhamd5);
            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {
                if(obj.id == 0)
                {
                    var context = HttpContext.Current;
                    string titulo = "Suas credencias para Bradesco Aplicativo";
                    string mensagem =
                        string.Format("Caro {0},</p>"
                        + "Segue suas credenciais para acessar nosso sistema:<br>"
                        + "Login: {1}<br>"
                        + "Senha: {2}<br>"
                        + "Endereço para acesso: <a href=\"{3}://{4}{5}{6}\">{3}://{4}{5}{6}</a>",obj.nome, obj.email, plainPassword,
                                    context.Request.Url.Scheme,
                                    context.Request.Url.Host,
                                    context.Request.Url.Port == 80
                                        ? string.Empty
                                        : ":" + context.Request.Url.Port,
                                    context.Request.ApplicationPath);
                    Manager.Helpers.SendMails.SendMail(obj.email, mensagem, titulo);
                }
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
            SqlCommand com = new SqlCommand("PC_EXCLUIR_USUARIO_ADMIN", con);

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
        public bool Bloqueio(string email, string senha, bool bloqueio, string data)
        {
            connection();
            SqlCommand com = new SqlCommand("PC_PROTECAO_LOGIN", con);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@EMAIL", email);
            com.Parameters.AddWithValue("@SENHA", senha);
            com.Parameters.AddWithValue("@BLOQUEIO", bloqueio);
            com.Parameters.AddWithValue("@DATA", data);

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

        protected Administrador ParseReader(DataRow dr)
        {
            return new Administrador()
                       {
                           id = Convert.ToInt32(dr["id"]),
                           nome = Convert.ToString(dr["nome"]),
                           email= Convert.ToString(dr["email"]),
                           senha= Convert.ToString(dr["senha"]),    
                           bloqueado = Convert.ToBoolean(dr["bloqueado"]),
                           data_liberacao = Convert.ToString(dr["data_liberacao"])
                       };
        }
    }
}