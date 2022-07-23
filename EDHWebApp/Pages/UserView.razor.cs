using System.Reflection.Metadata;
using Client.Data.Validation;
using EDHWebApp.Data;
using EDHWebApp.Data.TokenData;
using EDHWebApp.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
namespace EDHWebApp.Pages;

using System.IO;
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
    protected string errorMessage = "";
    // protected byte[] imageData = new byte [10000];
    
    protected PictureEmail _pictureEmail = new PictureEmail();
        
    
    [Inject]
    private NavigationManager NavigationManager { get; set; }
     [Inject] private IEmailSender _emailSender { get; set; }
     [Inject] private IUserLogInService _userLogInService { get; set; }
     [Inject] private ITokenManager _tokenManager { get; set; }
     [Inject] private IJSRuntime JsRuntime { get; set; }


     protected string FullName = "";

     protected CompanyUser currentUser = new CompanyUser();
     
     protected override async Task OnInitializedAsync()
     {
        try
        {

            _pictureEmail.PaymentType = "MasterCard";
            _pictureEmail.Picture = "";
            currentUser = await _tokenManager.getUserBasedOnRefreshToken();
            await  JsRuntime.InvokeVoidAsync("hideButton");

            if (currentUser.RefreshToken == "")
            {
                NavigationManager.NavigateTo("/");
            }

            FullName = currentUser.Name + " " + currentUser.Surname;


        }
        catch (Exception e) {
            NavigationManager.NavigateTo("/");
        }

     }
     

     public void OnValueChanged(string value)
     {
         _pictureEmail.PaymentType = value;
     }

     public async Task  CaptureFrame()
    {
        //int id = _userLogInService.getLoggedInId();
        int id = currentUser.UserId;
        _pictureEmail.userId = id;
        try
        {
            await _emailSender.sendPictureToCompaniesEmail(_pictureEmail);
            await JsRuntime.InvokeVoidAsync("showTextMessage");
            // success = "Picture sent successfully!";

        }
        catch (Exception e)
        {
            errorMessage = "An error occured: " + e.Message +
                           "\n the picture might not have loaded completely before sending";
        }

        Thread.Sleep(1000);
       //await JsRuntime.InvokeVoidAsync("refreshDocument ");

        await OnInitializedAsync();
        // NavigationManager.NavigateTo("/UserView");

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
        Thread.Sleep(1500);
        await  JsRuntime.InvokeVoidAsync("showButton");







     }



}