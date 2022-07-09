using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using EDHWebApi.Model;
using Microsoft.Extensions.DependencyInjection;

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

    public int SendPictureFromUserToCompany(PictureEmail pictureEmail, CompanyUser companyUser, string companyEmail)
    {
        // String messageToSend = "Hi!\n My name is " + user.Name + " " + user.Surname+ ". \n Here is a receipt from me: \n";
        string body = "Hi!\n My name is " + companyUser.Name + " " + companyUser.Surname+ ". \n Here is a receipt from me: \n \n \n \n \n <img src=\"cid:picture\" width=\"900\" height=\"900\" />";
        byte[] image = ConvertStringToByteArrayImage(pictureEmail.Picture);
        MemoryStream image1 = new MemoryStream(image);
        AlternateView av =
            AlternateView.CreateAlternateViewFromString(body, null, System.Net.Mime.MediaTypeNames.Text.Html);
        LinkedResource picture = new LinkedResource(image1, System.Net.Mime.MediaTypeNames.Image.Jpeg);
        picture.ContentId = "picture";
        picture.ContentType = new ContentType("image/jpg");
        av.LinkedResources.Add(picture);
      
        

        try
        {
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress("edhtech2022@gmail.com");
            message.To.Add(new MailAddress(companyEmail));
            message.Subject = "My Receipt:";
            message.IsBodyHtml = true;
            message.AlternateViews.Add(av);
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com";
            smtp.UseDefaultCredentials = false;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential("edhtech2022@gmail.com", "qlxsksjccfqtskyj");


            smtp.Send(message);
            return 1;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return 2;
        }
        // }

    }

    public byte[] ConvertStringToByteArrayImage(string picture)
    {
        byte[] imageBytes = Convert.FromBase64String(picture);
        Console.WriteLine(imageBytes);
        return imageBytes;
    }


}