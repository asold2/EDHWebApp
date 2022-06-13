namespace EDHWebApp.Data;
using System.Net;
using System.Net.Mail;
public class EmailSenderImpl : IEmailSender
{
    public void sendAccountRequestEmailToAdmin(string email, string name, string company)
    {
        string messageToSend = "I, " + name + "would like to request an account. I work at " + company;
        try
        {
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress(email);
            message.To.Add(new MailAddress("asoldan1459@gmail.com"));
            message.Subject = "Request Account";
            message.IsBodyHtml = true;
            message.Body = messageToSend;
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com";
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("user@domain.com", "generatedAPPpassword");
            smtp.EnableSsl = true;
            
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(message);
         
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}