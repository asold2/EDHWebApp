fetch('https://localhost:7213/claims', {
    method: 'POST',
    credentials: 'include',
    headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
        'Authorization': 'Basic ' + btoa(inst.user.username + ':' + inst.user.password)
    }
})
    .then(
        function(response) {
            if (response.status !== 200) {
                console.log('Looks like there was a problem. Status Code: ' +
                    response.status);
                return;
            }
        }
    )
    .catch(function(err) {
        console.log('Fetch Error :-S', err);
    });  

// function setCookie(){
//    
// }