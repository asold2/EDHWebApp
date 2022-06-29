using System.Net;
using System.Net.Mail;
using EDHWebApi.Model;

namespace EDHWebApi.EmailSender;

public class EmailSenderImpl : EmailSender
{
    public void SendRegistrationEmail(RegistrationUser regUser)
    {
        string messageToSend = "I, " + regUser.FullName + ", would like to request an account. I work at " + regUser.Company + ".\n My email is " + regUser.Email;
        try
        {
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress("edhtech2022@gmail.com");
            message.To.Add(new MailAddress("asoldan1459@gmail.com"));
            message.Subject = "Request Account";
            message.IsBodyHtml = true;
            message.Body = messageToSend;
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com";
            smtp.UseDefaultCredentials = false;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential("edhtech2022@gmail.com", "qlxsksjccfqtskyj");
            
            
            smtp.Send(message);
         
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public void SendPictureFromUserToCompany(PictureEmail pictureEmail)
    {
        
        
    }
}