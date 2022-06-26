using EDHWebApp.Model;

namespace EDHWebApp.Data;

public interface IEmailSender
{
    Task sendAccountRequestEmailToAdmin(RegistrationUser regUser);
}