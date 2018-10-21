using Microsoft.Win32;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace innovoCAD.Bentley.MicroStationCONNECT.ProjectControl
{
    //Email simple activation
    public class Registration
    {
        MailMessage mail = new MailMessage();
        SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);

        public async void Register()
        {
            await WaitAsynchronouslyAsyncSendEmail();
        }
        // The following method runs asynchronously. The UI thread is not
        // blocked during the delay. You can move or resize the Form1 window 
        // while Task.Delay is running.
        private async Task WaitAsynchronouslyAsyncSendEmail()
        {
            await Task.Delay(10000);
            SendMail();
        }

        private void SendMail()
        {
            try
            {
                RegistryKey Key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\innovoCAD\Visual Studio Tools for MicroStation CONNECT Edition", true);
                mail.From = new MailAddress("annonymous@annoymous.com");
                mail.To.Add("martyrobbins@innovoCAD.com");

                string message =
                    "User Name :" + Environment.GetEnvironmentVariable("USERNAME") +
                    "\nComputer Name :" + Environment.GetEnvironmentVariable("COMPUTERNAME");

                smtp.EnableSsl = true;
                smtp.Credentials = new System.Net.NetworkCredential("innovoCADReg@gmail.com", "Diamonds2000");

                mail.Subject = "Visual Studio Tools for MicroStation CONNECT Edition Registration";
                mail.Body = message;
                smtp.Send(mail);
                Key.SetValue("isRegistered", "True");

            }
            catch (Exception)
            {

            }

        }
    }
}
