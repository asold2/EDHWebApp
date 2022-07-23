// function startVideo(src) {
//     if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
//         navigator.mediaDevices.getUserMedia({ video: true }).then(function (stream) {
//             let video = document.getElementById(src);
//             if ("srcObject" in video) {
//                 video.srcObject = stream;
//             } else {
//                 video.src = window.URL.createObjectURL(stream);
//             }
//             video.onloadedmetadata = function (e) {
//                 video.play();
//             };
//             //mirror image
//             video.style.webkitTransform = "scaleX(-1)";
//             video.style.transform = "scaleX(-1)";
//         });
//     }
// }
//
//
//
// function getFrame(src, dest, dotNetHelper) {
//     let video = document.getElementById(src);
//     let canvas = document.getElementById(dest);
//     canvas.getContext('2d').drawImage(video, 0, 0, 320, 240);
//
//     let dataUrl = canvas.toDataURL("image/jpeg");
//     dotNetHelper.invokeMethodAsync('ProcessImage', dataUrl);
// }
function hideButton(){
    document.getElementById("imgSender").style.visibility="hidden";
    document.getElementById("messageText").style.visibility="hidden";
}
function showButton(){
    document.getElementById("imgSender").style.visibility="visible";
}

function showTextMessage(){

    document.getElementById("messageText").style.color="#28a745";
    document.getElementById("messageText").innerHTML="Picture sent successfully!";
    setTimeout(function(){
        document.getElementById('messageText').style.display = "none";
    },3000);



    document.getElementById("messageText").style.visibility="visible";
}

function refreshDocument(){
    document.getElementById("picture").value="";
   
}