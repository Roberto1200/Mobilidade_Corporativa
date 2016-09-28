using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Configuration;


namespace Manager.Helpers
{

    public interface EmailConfigurations
    {
        string Host ();

        int Port ();

        bool DefaultCredencials ();

        string DisplayName ();

        string UserName ();

        string Password();

    }

    public class GmailConfiguration:EmailConfigurations
    {
        public string Host()
        {
            return ConfigurationManager.AppSettings["SMTP"].ToString();
        }

        public int Port()
        {
            return 587;
        }

        public bool DefaultCredencials()
        {
            return false;
        }

        public string DisplayName()
        {
            return ConfigurationManager.AppSettings["fromName"].ToString();

            //return ConfigurationManager
            
        }

        public string UserName()
        {
            return ConfigurationManager.AppSettings["Email"].ToString();
        }

        public string Password()
        {
            return ConfigurationManager.AppSettings["Senha"].ToString();
        }
    }

    public class SendMails
    {        
        public static void SendMail(string emailto,string mensagem, string titulo)
        {

            if (Helpers.Configurations.UseEmailTest == true)
            {
                emailto = Helpers.Configurations.EmailTest;
            }

            GmailConfiguration gc = new GmailConfiguration();

            MailMessage objeto_mail = new MailMessage();
            SmtpClient client = new SmtpClient();
            client.Port = 25;
            
            client.Host = gc.Host();
            client.Timeout = 20000;

            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = gc.DefaultCredencials() ;
            //NECESSARIO PARA UTILIZAÇÂO NO GMAIL APPS
            if (gc.Host().Contains("gmail")){
                client.Credentials = new System.Net.NetworkCredential(gc.UserName(), gc.Password());
                client.EnableSsl = true;
                client.Port = gc.Port();
            }
            objeto_mail.From = new MailAddress(gc.UserName(),gc.DisplayName());
            
            
            objeto_mail.To.Add(emailto);
            objeto_mail.Subject = titulo; //
            objeto_mail.Body = mensagem; //"Utilize este token em sua operação: " + token
            objeto_mail.IsBodyHtml = true;

            try
            {
                client.Send(objeto_mail);
            }
            catch (Exception po)
            { }

        }
    }
}