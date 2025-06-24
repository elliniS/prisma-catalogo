using dotenv.net;
using PrismaCatalogo.Api.Models;
using PrismaCatalogo.Api.Services.Interfaces;
using System.Net;
using System.Net.Mail;

namespace PrismaCatalogo.Api.Services
{
    public class EmailService : IEmailService
    {
        public async Task<string> SendEmail(Email email)
        {
            DotEnv.Load();
            string remetente = Environment.GetEnvironmentVariable("EMAIL");
            string destinatario = email.Destinatario;
            string senhaApp = Environment.GetEnvironmentVariable("SENHA_APP");

            MailMessage mensagem = new MailMessage(remetente, destinatario);
            mensagem.Subject = email.Assunto;
            mensagem.Body = email.Corpo;

            try
            {
                using (SmtpClient cliente = new SmtpClient("smtp.gmail.com", 587))
                {
                    cliente.EnableSsl = true;
                    cliente.Credentials = new NetworkCredential(remetente, senhaApp);
                    cliente.Send(mensagem);
                }
            }
            catch (Exception ex) { 
                return ex.Message;
            }

            return "";
        }
    }
}
