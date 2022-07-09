using EDHWebApi.Model;

namespace EDHWebApi.EmailSender;

public interface EmailSender
{
    void SendRegistrationEmail(RegistrationUser regUser);
    int SendPictureFromUserToCompany(PictureEmail pictureEmail, CompanyUser companyUser, string companyEmail);
    
    
}