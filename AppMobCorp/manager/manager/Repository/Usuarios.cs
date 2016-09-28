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
    public class UsuarioseRepository : BaseRepository, IBaseRepository
    {
        //LE UM REGISTRO
        public Usuario Load(int id)
        {
            List<Usuario> EmpList = new List<Usuario>();

            Dictionary<string, object> dicParameters = new Dictionary<string, object>();
            dicParameters.Add("@id", id);

            DataTable dt = ReadFromStoredProcedure("PC_SEL_USUARIOS", dicParameters);

            //Bind EmpModel generic list using LINQ 
            EmpList = (from DataRow dr in dt.Rows
                       select ParseReader(dr)).ToList();

            return EmpList[0];
        }

        private string GetMD5(string strPassword)
        {
            Cripto md5f = new Cripto();
            using (MD5 md5Hash = MD5.Create())
            {
                return md5f.GetMd5Hash(md5Hash, strPassword);
            }
        }

        // Check if E-Mail exists
        public bool EmailExists(string email, int id = 0)
        {
            List<Usuario> listUsers = List();

            return listUsers.Where(u => u.email.Trim().ToLower() == email.Trim().ToLower() && u.id != id).Any();

        }

        public bool CodFuncionalExists(string codfuncional, int id = 0)
        {
            List<Usuario> listUsers = List();

            return listUsers.Where(u => u.codfuncional.Trim().ToLower() == codfuncional.Trim().ToLower() && u.id != id).Any();

        }

        public bool NomeExists(string nome, int id = 0)
        {
            List<Usuario> listUsers = List();

            return listUsers.Where(u => u.nome.Trim().ToLower() == nome.Trim().ToLower() && u.id != id).Any();

        }


        //lista os registros
        public List<Usuario> List()
        {
            List<Usuario> EmpList = new List<Usuario>();

            Dictionary<string, object> dicParameters = new Dictionary<string, object>();
            dicParameters.Add("@id", -1);

            DataTable dt = ReadFromStoredProcedure("PC_SEL_USUARIOS", dicParameters);

            //Bind EmpModel generic list using LINQ 
            EmpList = (from DataRow dr in dt.Rows
                       select ParseReader(dr)).ToList();

            return EmpList;

        }

        //lista os registros
        public bool InsertUpdate(Usuario obj)
        {
            return InsertUpdate(obj, "");
        }
        //atualiza um registro
        public bool InsertUpdate(Usuario obj, string md5)
        {
            string senhamd5 = obj.id == 0 && string.IsNullOrEmpty(obj.senha) ?
                                        LoginUser.GenerateNewPassword() : obj.senha;

            string plainPassword = obj.id == 0 && string.IsNullOrEmpty(obj.senha) ?
                                        LoginUser.GenerateNewPassword() : obj.senha;

            if (plainPassword != null && md5 == "serviço")
            {
                senhamd5 = GetMD5(plainPassword);
            }

            string regionais = "";
            try
            {
                regionais = string.Join(",", obj.idregional);
            }
            catch (Exception Ex)
            { }

            Dictionary<string, object> dicParameters = new Dictionary<string, object>()
            {
                { "@id", obj.id },
                { "@funcional", obj.codfuncional },
                { "@nome", obj.nome },
                { "@email", obj.email },
                { "@diretorias", obj.diretoria },
                { "@regionais", regionais },
                { "@juncao", obj.juncao },
                { "@tipo", obj.tipo },
                { "@avatar", obj.avatar },
                { "@senha", senhamd5 },
                { "@fonecelular", obj.fonecelular }
            };

            int i = UpdateFromStoredProcedure("PC_IU_USUARIOS", dicParameters);

            if (i >= 1)
            {
                if (obj.id == 0)
                {
                    string titulo = "Suas credencias para Bradesco Aplicativo";
                    string mensagem =
                        string.Format("Caro {0},</p>"
                        + "Segue suas credenciais para acessar nosso sistema:<br>"
                        + "Código Funcional: {1}<br>"
                        + "Senha: {2}", obj.nome, obj.codfuncional, plainPassword);
                    Manager.Helpers.SendMails.SendMail(obj.email, mensagem, titulo);
                }
                return true;

            }

            return false;

        }

        public bool SetPrimeiroAcesso(string funcional, string senha)
        {
            Dictionary<string, object> dicParameters = new Dictionary<string, object>()
            {
                { "@funcional", funcional },
                { "@senha", senha }
            };

            int i = UpdateFromStoredProcedure("PC_SETPRIMEIRO_ACESSO", dicParameters);

            return (i >= 1);

        }
        public bool RevPrimeiroAcesso(string funcional, int id)
        {
            string senha = LoginUser.GenerateNewPassword();
            string senhamd5 = GetMD5(senha);

            Dictionary<string, object> dicParameters = new Dictionary<string, object>()
            {
                { "@funcional", funcional },
                { "@senha", senhamd5 }
            };

            int i = UpdateFromStoredProcedure("PC_REV_PRIMEIRO_ACESSO", dicParameters);

            if (i >= 1)
            {
                // Load current 'Usuario' by ID.
                Usuario obj = Load(id);

                string titulo = "Acesso ao Bradesco Aplicativo Inicializado";
                string mensagem =
                    string.Format("Caro {0},</p>"
                    + "Seu acesso ao Bradesco Aplicativo foi Inicializado por um Administrador.<br>"
                    + "Segue suas credenciais para acessar nosso sistema:<br>"
                    + "Código Funcional: {1}<br>"
                    + "Senha: {2}", obj.nome, obj.codfuncional, senha);
                Manager.Helpers.SendMails.SendMail(obj.email, mensagem, titulo);

                return true;

            }

            return false;

        }

        public bool AlterarSenha(string token, string novasenha)
        {

            Cripto md5f = new Cripto();
            string senhamd5;
            using (MD5 md5Hash = MD5.Create())
            {
                senhamd5 = md5f.GetMd5Hash(md5Hash, novasenha);
            }

            Dictionary<string, object> dicParameters = new Dictionary<string, object>()
            {
                { "@token", token },
                { "@novasenha", senhamd5 }
            };

            int i = UpdateFromStoredProcedure("PC_ALTERAR_SENHA", dicParameters);

            return (i >= 1);

        }

        //excluir um registro
        public bool Delete(int id)
        {
            Dictionary<string, object> dicParameters = new Dictionary<string, object>();
            dicParameters.Add("@id", id);

            int i = UpdateFromStoredProcedure("PC_EXCLUIR_USUARIO", dicParameters);

            return (i >= 1);

        }

        protected Usuario ParseReader(DataRow dr)
        {

            int tipo = 1;
            int userID = Convert.ToInt32(dr["id"]);
            List<int> idRegionalList = new List<int>();

            int.TryParse(Convert.ToString(dr["tipo"]), out tipo);

            if (tipo == 1)
            {
                idRegionalList.Add(Convert.ToInt32(dr["unique_regional"])); // Set "regional" id to user "agente"
            }
            else
            {
                // Set "regionais" list to user "coordenador"
                RegionaisRepository repoRegionais = new RegionaisRepository();

                idRegionalList = repoRegionais.List(userID).Select(r => r.id).ToList();

            }

            return new Usuario()
            {
                id = Convert.ToInt32(dr["id"]),
                codfuncional = Convert.ToString(dr["cod_funcional"]),
                nome = Convert.ToString(dr["nome"]),
                email = Convert.ToString(dr["email"]),
                diretoria = Convert.ToString(dr["diretoria"]),
                regional = Convert.ToString(dr["Regional"]),
                fonecelular = Convert.ToString(dr["fonecelular"]),
                txttipo = Convert.ToString(dr["txttipo"]),
                txtdiretoria = Convert.ToString(dr["txtdiretoria"]),
                usuario = Convert.ToString(dr["usuario"]),
                senha = Convert.ToString(dr["senha"]),
                avatar = Convert.ToString(dr["avatar"]),
                token = Convert.ToString(dr["token"]),
                juncao = Convert.ToInt32(dr["juncao"]),
                tipo = Convert.ToInt32(dr["tipo"]),
                unique_regional = tipo == 1 ? Convert.ToInt32(dr["unique_regional"]) : 0,
                idregional = idRegionalList,
                datainicio = Convert.ToDateTime(dr["datainicio"]),
                primeiroacesso = Convert.ToBoolean(dr["primeiro_login_sucedido"])
                //idregional =Convert.ToInt32(dr["idregional"])
            };
        }

        private Usuario GetFromExcel(DataRow dr, string userType)
        {

            int juncao = 0;

            return new Usuario()
            {
                id = 0,
                codfuncional = Convert.ToString(dr["Cód. Funcional"]).Trim().Replace("  ", string.Empty),
                nome = Convert.ToString(dr["Agente da Mobilidade"]).Trim().Replace("  ", string.Empty),
                email = Convert.ToString(dr["Email"]).Trim().Replace("  ", string.Empty).ToLower(),
                txtdiretoria = Convert.ToString(dr["Diretoria"]).Trim().Replace("  ", string.Empty),
                regional = Convert.ToString(dr["Regional"]).Trim().Replace("  ", string.Empty),
                juncao = int.TryParse(Convert.ToString(dr["Junção Agência"]).Trim().Replace("  ", string.Empty), out juncao) ? juncao : 0,
                agencia = Convert.ToString(dr["Agência"]).Trim().Replace("  ", string.Empty),
                txttipo = Convert.ToString(dr["Tipo"]).Replace("  ", string.Empty),
                senha = LoginUser.GenerateNewPassword(),
                token = LoginUser.RandomString(5).ToString(),

            };

        }

        private Usuario GetFromExcelCoordenador(DataRow dr, string userType)
        {
            return new Usuario()
            {
                id = 0,
                codfuncional = Convert.ToString(dr["CÓD. FUNCIONAL"]).Trim().Replace("  ", string.Empty),
                nome = Convert.ToString(dr["COORDENADOR"]).Trim().Replace("  ", string.Empty),
                email = Convert.ToString(dr["Email"]).Trim().Replace("  ", string.Empty),
                txtdiretoria = Convert.ToString(dr["Diretoria"]).Trim().Replace("  ", string.Empty),
                regional = Convert.ToString(dr["Regional"]).Trim().Replace("  ", string.Empty),
                fonecelular = Convert.ToString(dr["Celular"]).Trim().Replace("  ", string.Empty),
                txttipo = "Coordenador",
                senha = GetMD5(LoginUser.GenerateNewPassword()),
                token = LoginUser.RandomString(5).ToString(),
                agencia = string.Empty

            };

        }

        #region Excel Import / Export Private Functions
        private bool NotEmptyRowExcel(DataRow dr, string userType)
        {
            return Convert.ToString(dr["Cód. Funcional"]).Trim() != ""
                && Convert.ToString(dr["Agente da Mobilidade"]).Trim() != ""
                && Convert.ToString(dr["Email"]).Trim() != ""
                && Convert.ToString(dr["Regional"]).Trim() != ""
                && Convert.ToString(dr["Junção Agência"]).Trim() != ""
                && Convert.ToString(dr["Agência"]).Trim() != ""
                && Convert.ToString(dr["Tipo"]).Trim() != "";
        }

        private bool NotEmptyRowExcelCoordenador(DataRow dr, string userType)
        {
            return Convert.ToString(dr["CÓD. FUNCIONAL"]).Trim() != ""
                && Convert.ToString(dr["COORDENADOR"]).Trim() != ""
                && Convert.ToString(dr["Email"]).Trim() != ""
                && Convert.ToString(dr["Diretoria"]).Trim() != ""
                && Convert.ToString(dr["Regional"]).Trim() != ""
                && Convert.ToString(dr["Celular"]).Trim() != "";
        }

        private bool ValidEmail(DataRow dr)
        {
            string emailString = Convert.ToString(dr["Email"]).Trim().Replace("  ", string.Empty);
            string pattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

            return Regex.IsMatch(emailString, pattern, RegexOptions.IgnoreCase);

        }
        #endregion


        private Usuario SetRegionaisID(Usuario newUsuario, List<Regional> listRegionais, string strRegional)
        {

            Regional foundRegional = null;
            string strRegionalID = null;

            if (!string.IsNullOrEmpty(strRegional))
            {

                if (newUsuario != null)
                    foundRegional = listRegionais.Find(r => r.regional.Trim().ToUpper() == strRegional);

                if (foundRegional != null)
                    strRegionalID = foundRegional.id.ToString();

                newUsuario.regional = string.IsNullOrEmpty(newUsuario.regional) || strRegional == newUsuario.regional.Trim().ToUpper() ?
                                        strRegionalID :
                                        string.Concat(newUsuario.regional, ",", strRegionalID);

            }

            return newUsuario;
        }

        private Usuario SetDiretoriasID(Usuario newUsuario, List<Diretoria> listDiretorias, string strDiretoria)
        {

            Diretoria foundDiretoria = null;
            string strDiretoriaID = null;

            if (!string.IsNullOrEmpty(strDiretoria))
            {
                if (newUsuario != null)
                    foundDiretoria = listDiretorias.Find(d => d.diretoria.Trim().ToUpper() == strDiretoria);

                if (foundDiretoria != null)
                    strDiretoriaID = foundDiretoria.id.ToString();

                newUsuario.txtdiretoria = string.IsNullOrEmpty(newUsuario.txtdiretoria)
                                            || strDiretoria == newUsuario.txtdiretoria.Trim().ToUpper() ?
                                            strDiretoriaID :
                                            string.Concat(newUsuario.txtdiretoria, ",", strDiretoriaID);
            }

            return newUsuario;
        }

        private List<Usuario> LoadRegionais(List<Usuario> usuarios, IEnumerable<DataRow> rows)
        {
            RegionaisRepository RepoRegionais = new RegionaisRepository();
            DiretoriasRepository RepoDiretorias = new DiretoriasRepository();

            List<Regional> listRegionais = RepoRegionais.List();
            List<Diretoria> listDiretorias = RepoDiretorias.List();

            Usuario newUsuario = new Usuario();
            int i = 0;

            foreach (DataRow row in rows)
            {
                string strCodFuncional = Convert.ToString(row["CÓD. FUNCIONAL"]).Trim();
                string strRegional = Convert.ToString(row["Regional"]).Trim().ToUpper();
                string strDiretoria = Convert.ToString(row["Diretoria"]).Trim().ToUpper();

                if (!string.IsNullOrEmpty(strCodFuncional))
                {

                    if (newUsuario != null && !string.IsNullOrEmpty(newUsuario.regional))
                    {
                        usuarios[i] = newUsuario;
                        i++;
                    }

                    newUsuario = usuarios.Find(u => u.codfuncional == strCodFuncional);

                }

                newUsuario = SetRegionaisID(newUsuario, listRegionais, strRegional);
                newUsuario = SetDiretoriasID(newUsuario, listDiretorias, strDiretoria);

            }

            return usuarios;
        }



        public List<Usuario> MapFromExcel(IEnumerable<DataRow> rows, string userType)
        {
            List<Usuario> usuarios = new List<Usuario>();

            switch (userType.ToLower())
            {
                case "agente":
                    usuarios = (from DataRow dr in rows
                                where NotEmptyRowExcel(dr, userType)
                                where ValidEmail(dr)
                                select GetFromExcel(dr, userType)).ToList();
                    break;

                case "coord":

                    List<Diretoria> listDir = new List<Diretoria>();

                    usuarios = (from DataRow dr in rows
                                where NotEmptyRowExcelCoordenador(dr, userType)
                                where ValidEmail(dr)
                                select GetFromExcelCoordenador(dr, userType)).ToList();


                    //Map "regionais" to users
                    usuarios = LoadRegionais(usuarios, rows);

                    break;

                default:
                    usuarios = null;
                    break;
            }

            return usuarios;
        }

        //Map User List to Excel, creating a new DataTable to export.
        public DataTable MapToExcel(List<Usuario> listUsers, string sheetName)
        {
            DataTable dt = new DataTable(sheetName);

            // Comment "Agencia" info, until CRUD "Agencia" is completely ready.
            dt.Columns.AddRange(
                new DataColumn[]
                {
                    new DataColumn {ColumnName = "Cód. Funcional", DataType = typeof (string)},
                    new DataColumn {ColumnName = "Agente da Mobilidade", DataType = typeof (string)},
                    new DataColumn {ColumnName = "Email", DataType = typeof (string)},
                    new DataColumn {ColumnName = "Diretoria", DataType = typeof (string)},
                    new DataColumn {ColumnName = "Regional", DataType = typeof (string)},
                    //new DataColumn {ColumnName = "Junção Agência", DataType = typeof (int)},
                    //new DataColumn {ColumnName = "Agência", DataType = typeof (string)},
                    new DataColumn {ColumnName = "Celular", DataType = typeof (string)},
                    new DataColumn {ColumnName = "Tipo", DataType = typeof (string)}
                }
            );

            foreach (var user in listUsers)
            {
                //dt.LoadDataRow(new object[] { user.codfuncional, user.nome, user.email, user.txtdiretoria, user.regional, user.juncao, user.agencia, user.txttipo }, true);
                dt.LoadDataRow(new object[] { user.codfuncional, user.nome, user.email, user.txtdiretoria, user.regional, user.fonecelular, user.txttipo }, true);
            }


            return dt;
        }

        public bool InsertUpdateFromExcel(Usuario obj)
        {
            Dictionary<string, object> dicParameters = new Dictionary<string, object>()
            {
                { "@funcional", obj.codfuncional },
                { "@nome", obj.nome },
                { "@email", obj.email },
                { "@diretoria", obj.txtdiretoria },
                { "@regional", obj.regional },
                { "@agencia", obj.agencia },
                { "@juncao", obj.juncao },
                { "@tipo", obj.txttipo },
                { "@senha", GetMD5(obj.senha) },
                { "@token", obj.token }
            };

            int i = UpdateFromStoredProcedure("PC_XLS_USUARIOS", dicParameters);

            if (i >= 1)
            {
                // Activate E-Mail when deploying.
                /*
                if (obj.id == 0)
                {
                    string titulo = "Suas credencias para Bradesco Aplicativo";
                    string mensagem =
                        string.Format("Caro {0},</p>"
                        + "Segue suas credenciais para acessar nosso sistema:<br>"
                        + "Código Funcional: {1}<br>"
                        + "Senha: {2}", obj.nome, obj.codfuncional, obj.senha);
                    Manager.Helpers.SendMails.SendMail(obj.email, mensagem, titulo);
                }
                */
                return true;

            }
            else
            {

                return false;
            }
        }

    }
}