using EDHWebApi.Model;

namespace EDHWebApi.EmailSender;

public interface EmailSender
{
    void SendRegistrationEmail(RegistrationUser regUser);
    void SendPictureFromUserToCompany(PictureEmail pictureEmail);
    
    
}