using System.Reflection.Metadata;
using Client.Data.Validation;
using EDHWebApp.Data;
using EDHWebApp.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;

namespace EDHWebApp.Pages;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;



using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.Fonts;
using System.Net.Http;
using System.IO;

[Authorize(Policy = "IsVerified")]
[Authorize]
public class UserViewRazor : ComponentBase
{
    protected string image = "";
    protected string captionText = "";
    protected string frameUri = "";
    

    protected string success = "";
    // protected byte[] imageData = new byte [10000];
    
    protected PictureEmail _pictureEmail = new PictureEmail();
        
    
     [Inject] private IEmailSender _emailSender { get; set; }
     [Inject] private IUserLogInService _userLogInService { get; set; }


  

     

 

     public void OnValueChanged(string value)
     {
         _pictureEmail.PaymentType = value;
     }

     public async Task  CaptureFrame()
    {
        int id = _userLogInService.getLoggedInId();
        _pictureEmail.userId = id;
        await _emailSender.sendPictureToCompaniesEmail(_pictureEmail);
        success = "Picture sent successfully!";

    }

     public async Task GetPicture(InputFileChangeEventArgs e)
     {
         var imageFile = e.File;

         var path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString());
         await using var fs = new FileStream(path, FileMode.Create);
         await imageFile.OpenReadStream(imageFile.Size).CopyToAsync(fs);
         var bytes = new byte[imageFile.Size];
         fs.Position = 0;
         await fs.ReadAsync(bytes);
         fs.Close();
         File.Delete(path);
         string image = Convert.ToBase64String(bytes);
         _pictureEmail.Picture = image;
         





     }



}