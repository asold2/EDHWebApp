using System.Text;
using System.Text.Json;
using EDHWebApp.Model;
using Microsoft.AspNetCore.Mvc;

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
    }

    public async  Task sendPictureToCompaniesEmail(PictureEmail pictureEmail)
    {

        string pictureEmailAsJson = JsonSerializer.Serialize(pictureEmail, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        HttpContent content = new StringContent(
            pictureEmailAsJson,
            Encoding.UTF8,
            "application/json"
        );
        await HttpClient.PostAsync(uri + "/picture", content);

       
    }
}