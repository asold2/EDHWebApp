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
public class UserView_razor : ComponentBase
{
    private string captionText;
    private string frameUri;

     private readonly IJSRuntime JsRuntime;

    protected override async Task OnInitializedAsync()
    {
        await JsRuntime.InvokeVoidAsync("startVideo", "videoFeed");
        
   
    }
    
    
    private async Task CaptureFrame()
    {
        await JsRuntime.InvokeAsync<String>("getFrame", "videoFeed", "currentFrame", DotNetObjectReference.Create(this));
    }
    [JSInvokable]
    public void ProcessImage(string imageString)
    {
        byte[] imageData = Convert.FromBase64String(imageString);

        //Do image processing here
    }

}