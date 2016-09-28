using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Web.Security;

namespace Manager.Helpers
{
    public class LoginUser
    {
        public class LoginRetorno
        {
            public bool status { get; set; }

            public bool primeiro_acesso { get; set; }
        }
        Repository.UsuarioseRepository RepoUsers = new Repository.UsuarioseRepository();
        public const string connection = "";
        private SqlConnection con;
        //cria uma instancia da conexao
        private void GetConnection()
        {
            string constr = ConfigurationManager.ConnectionStrings["bradesco_app"].ToString();
            con = new SqlConnection(constr);

        }

        //Recebe a senha em MD5
        public LoginRetorno ValidateUser(string username, string password, string cod_funcional)
        {
            string constring = ConfigurationManager.ConnectionStrings["bradesco_app"].ToString();

            using (SqlConnection conn = new SqlConnection(constring))
            using (SqlCommand cmd = conn.CreateCommand())
            {

                cmd.CommandText = "PC_VALIDAR_USUARIO";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("usuario", username);
                cmd.Parameters.AddWithValue("senha", password);
                cmd.Parameters.AddWithValue("funcional", cod_funcional);

                SqlParameter ret = new SqlParameter("@retorno", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ret);
                //var returnParameter = cmd.Parameters.Add("@retorno", SqlDbType.Int);
                //returnParameter.Direction = ParameterDirection.ReturnValue;

                conn.Open();
                cmd.ExecuteNonQuery();
                LoginRetorno tmp = new LoginRetorno();
                tmp.status = false;
                tmp.primeiro_acesso = false;
                int userAffected = Convert.ToInt32(ret.Value);
                if (userAffected > 0)
                {
                    tmp.status = true;
                    tmp.primeiro_acesso = RepoUsers.List().Where(s => s.codfuncional == cod_funcional).FirstOrDefault().primeiroacesso;
                    string criptto = "";
                    if (username == "") criptto = cod_funcional;
                    if (cod_funcional == "") criptto = username;

                    Helpers.Cripto cripto = new Cripto();

                    string email = "";
                    try
                    {
                        email = RepoUsers.List().Where(p => p.codfuncional == cod_funcional).FirstOrDefault().email;
                    }
                    catch (Exception)
                    { }

                    AtualizarToken(null, RandomString(6), username, null, cod_funcional, email, null, ref tmp);

                }
                return tmp;
            }
        }
        public LoginRetorno ValidateCoord(string username, string password, string cod_funcional)
        {
            string constring = ConfigurationManager.ConnectionStrings["bradesco_app"].ToString();

            using (SqlConnection conn = new SqlConnection(constring))
            using (SqlCommand cmd = conn.CreateCommand())
            {

                cmd.CommandText = "PC_VALIDAR_COORD";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("usuario", username);
                cmd.Parameters.AddWithValue("senha", password);
                cmd.Parameters.AddWithValue("funcional", cod_funcional);

                SqlParameter ret = new SqlParameter("@retorno", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ret);
                //var returnParameter = cmd.Parameters.Add("@retorno", SqlDbType.Int);
                //returnParameter.Direction = ParameterDirection.ReturnValue;

                conn.Open();
                cmd.ExecuteNonQuery();
                LoginRetorno tmp = new LoginRetorno();
                tmp.status = false;
                tmp.primeiro_acesso = false;
                int userAffected = Convert.ToInt32(ret.Value);
                if (userAffected > 0)
                {
                    tmp.status = true;
                    tmp.primeiro_acesso = RepoUsers.List().Where(s => s.codfuncional == cod_funcional).FirstOrDefault().primeiroacesso;
                    string criptto = "";
                    if (username == "") criptto = cod_funcional;
                    if (cod_funcional == "") criptto = username;

                    Helpers.Cripto cripto = new Cripto();

                    string email = "";
                    try
                    {
                        email = RepoUsers.List().Where(p => p.codfuncional == cod_funcional).FirstOrDefault().email;
                    }
                    catch (Exception)
                    { }

                    AtualizarToken(null, RandomString(6), username, null, cod_funcional, email, null, ref tmp);

                }
                return tmp;
            }
        }


        public Models.Administrador ValidateUserAdmin(string email, string senha)
        {
            Repository.AdministradoresRepository ua = new Repository.AdministradoresRepository();

            return ua.Load(email, senha);

        }
        public bool AtualizarToken(string tipo, string token, string username, string senha, string funcional, string email, string somenteToken, ref LoginRetorno lr)
        {
            Manager.Repository.AdministradoresRepository repoAdm = new Manager.Repository.AdministradoresRepository();
            Manager.Repository.UsuarioseRepository repoUser = new Manager.Repository.UsuarioseRepository();
            GetConnection();
            SqlParameter ret = new SqlParameter();
            int i = 0;

            string senhamd5 = string.Empty;
            if (!string.IsNullOrEmpty(senha))
            {
                Cripto md5f = new Cripto();
                
                using (MD5 md5Hash = MD5.Create())
                {
                    senhamd5 = md5f.GetMd5Hash(md5Hash, senha);
                }
            }

            if (tipo == "admin")
            {

                SqlCommand com = new SqlCommand("SP_ATUALIZAR_TOKEN_ADMIN", con);

                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@token", token);
                com.Parameters.AddWithValue("@email", email);
                com.Parameters.AddWithValue("@sotoken", somenteToken);
                com.Parameters.AddWithValue("@senha", senhamd5);

                ret = new SqlParameter("@PRIMEIROACESSO", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;
                com.Parameters.Add(ret);

                con.Open();
                i = com.ExecuteNonQuery();
            }
            else
            {
                SqlCommand com = new SqlCommand("SP_ATUALIZAR_TOKEN_USUARIO", con);

                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@token", token);
                com.Parameters.AddWithValue("@usuario", username);
                com.Parameters.AddWithValue("@funcional", funcional);
                com.Parameters.AddWithValue("@ttoken", somenteToken);

                ret = new SqlParameter("@PRIMEIROACESSO", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;
                com.Parameters.Add(ret);

                con.Open();
                i = com.ExecuteNonQuery();
            }
            int userAffected = 0; 

            if (int.TryParse(ret.Value.ToString(), out userAffected) && userAffected > 0 && userAffected < 2)
            {
                if (string.IsNullOrWhiteSpace(somenteToken))
                {
                    lr.primeiro_acesso = repoUser.List().Where(u=> u.codfuncional == funcional).FirstOrDefault().primeiroacesso;
                    lr.status = true;
                }
                else
                {
                    lr.primeiro_acesso = false;
                }
                if (!string.IsNullOrEmpty(email) && somenteToken == "0" && tipo == "admin")
                {
                    
                    List<Manager.Models.Administrador> listAdm = repoAdm.List();
                    var adminName = listAdm.Where(a => a.email == email).Select(a=>a.nome).FirstOrDefault();
                    
                    string titulo = "Seu token de acesso Bradesco Aplicativo";
                    string mensagem =
                        string.Format("Bradesco Aplicativo<p>"
                        + "Caro {0},<br>"
                        + "utilize este token em sua operação: {1}", adminName, token);
                    SendMailUser(email, mensagem, titulo);
                }
                else if(!string.IsNullOrEmpty(email))
                {
                    
                    List<Manager.Models.Usuario> listAdm = repoUser.List();
                    var userName = listAdm.Where(a => a.email == email).Select(a => a.nome).FirstOrDefault();

                    string titulo = "Seu token de acesso Bradesco Aplicativo";
                    string mensagem =
                        string.Format("Bradesco Aplicativo<p>"
                        + "Caro {0},<br>"
                        + "utilize este token em sua operação: {1}", userName, token);
                    SendMailUser(email, mensagem, titulo);
                }
            }
            con.Close();

            return (userAffected > 0);

        }


        public bool ValidarTokenEFuncional(string token, string funcional)
        {
            GetConnection();
            SqlCommand com = new SqlCommand("PC_VALIDAR_TOKEN_EFUNCIONAL", con);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@token", token);
            com.Parameters.AddWithValue("@funcional", funcional);

            SqlParameter ret = new SqlParameter("@STATUS", SqlDbType.Int);
            ret.Direction = ParameterDirection.Output;
            com.Parameters.Add(ret);

            con.Open();
            int i = com.ExecuteNonQuery();
            int userAffected = Convert.ToInt32(ret.Value);
            if (userAffected > 0)
            {
                return true;
            }
            con.Close();
            return false;
        }

        public void SendMailUser(string email, string mensagem, string titulo)
        {
            if (Manager.Helpers.Configurations.AllowEmailTokenLogin)
            {
                Manager.Helpers.SendMails.SendMail(email, mensagem, titulo);
            }
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string GenerateNewPassword()
        {
            return Membership.GeneratePassword(8, 2)
                             .Replace("l", "a")
                             .Replace("I", "x");
        }

    }
}