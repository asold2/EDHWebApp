using Client.Data.Validation;
using EDHWebApp.Data;
using EDHWebApp.Model;
using Microsoft.AspNetCore.Authorization;

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
public class UserViewRazor : ComponentBase
{
    protected string captionText = "";
    protected string frameUri = "";
    protected byte[] imageData = new byte [10000];
    
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
        _pictureEmail.Picture = imageData;
        _pictureEmail.userId = id;
        // await _jsRuntime.InvokeAsync<String>("getFrame", "videoFeed", "currentFrame", DotNetObjectReference.Create(this));
        await _emailSender.sendPictureToCompaniesEmail(_pictureEmail);
    }
   


}