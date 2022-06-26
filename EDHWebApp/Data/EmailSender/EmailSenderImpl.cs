using System.Text;
using System.Text.Json;
using EDHWebApp.Model;

namespace EDHWebApp.Data;
using System.Net;
using System.Net.Mail;
public class EmailSenderImpl : IEmailSender
{
    private string uri = "https://localhost:7213";
    private readonly HttpClient HttpClient;

    public EmailSenderImpl()
    {
        HttpClient = new HttpClient();
    }


    // We use a company email to send emails when users want to register.
    // The email created by the developer is:
    // edhtech2022@gmail.com
    // In line 27, we use an App Password, to bypass the authentication required by SMTP. 
    // This way, the email specified in line 19, will receive requests for registration
    // public void sendAccountRequestEmailToAdmin(string email, string name, string company)
    // {
    //     string messageToSend = "I, " + name + ", would like to request an account. I work at " + company + ". My email is " + email;
    //     try
    //     {
    //         MailMessage message = new MailMessage();
    //         SmtpClient smtp = new SmtpClient();
    //         message.From = new MailAddress("edhtech2022@gmail.com");
    //         message.To.Add(new MailAddress("asoldan1459@gmail.com"));
    //         message.Subject = "Request Account";
    //         message.IsBodyHtml = true;
    //         message.Body = messageToSend;
    //         smtp.Port = 587;
    //         smtp.Host = "smtp.gmail.com";
    //         smtp.UseDefaultCredentials = false;
    //         smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
    //         smtp.EnableSsl = true;
    //         smtp.Credentials = new NetworkCredential("edhtech2022@gmail.com", "qlxsksjccfqtskyj");
    //         
    //         
    //         smtp.Send(message);
    //      
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e);
    //     }
    // }


    public async  Task sendAccountRequestEmailToAdmin(RegistrationUser regUser)
    {
        string regUserAsJson = JsonSerializer.Serialize(regUser, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        HttpContent content = new StringContent(
            regUserAsJson,
            Encoding.UTF8,
            "application/json"
        );
        await HttpClient.PostAsync(uri + "/reg/email", content);
        Console.WriteLine("Sent registration request to API " + regUserAsJson);
    }
}