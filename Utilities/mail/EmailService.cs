using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace Utilities.mail
{
    public static class EmailService
    {
        public static void SendMail()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("sebastian Olaya", "jsolayareyes@gmail.com"));
            message.To.Add(new MailboxAddress("Admin", "encarte1817@gamil.com"));
            message.Subject = "BankConsole - Usuarios nuevos";
            
            message.Body = new TextPart("plain")
            {
                Text = GetEmailText()
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("jsolayareyes@gmail.com", "pankuxnjupdaywyl");
                client.Send(message);
                client.Disconnect(true);
            }

        }

        private static string GetEmailText()
        {
            List<UserData> newUsers = StorageState.GetNewUsers();

            if (newUsers.Count == 0)
                return "No hay nuevos usuarios registrados.";

            string emailText = "usuarios agregados:\n";

            foreach (UserData in newUsers)
            {
                emailText +- = "\t+ " + userData.GetAllUsersAsync() + "\n";

                return emailText;
            }


        }
    }
}