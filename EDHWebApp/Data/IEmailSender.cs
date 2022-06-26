namespace EDHWebApp.Data;

public interface IEmailSender
{
    void sendAccountRequestEmailToAdmin(string email, string name, string company);
}