﻿
function initFacebook(){
        window.fbAsyncInit = function () {
            FB.init({
                appId: '305024333477745',
                cookie: true,
                xfbml: true,
                version: 'V3.2'
            });
      
    //FB.AppEvents.logPageView();

  };

  (function(d, s, id){
     var js, fjs = d.getElementsByTagName(s)[0];
     if (d.getElementById(id)) {return;}
     js = d.createElement(s); js.id = id;
     js.src = "https://connect.facebook.net/en_US/sdk.js#xfbml=1&version=v3.2&appId=305024333477745&autoLogAppEvents=1";
     fjs.parentNode.insertBefore(js, fjs);
   }(document, 'script', 'facebook-jssdk'));
}


function Facebook_LoggedIn() {
    FB.api('/me', { locale: 'en_US', fields: 'id,name,email,birthday,location' },
        function (response) {
            console.log(response);

            FB.getLoginStatus(function (getLoginStatusResponse) {
                console.log(getLoginStatusResponse);
                if (getLoginStatusResponse.status === 'connected') {
                    console.log(getLoginStatusResponse.authResponse.accessToken);


                    $.get("../account/LoginFacebook?token=" + getLoginStatusResponse.authResponse.accessToken);
                      
                }
            })

        });

}

// send http post request

   

    //redirect
    //window.location.href = "http://localhost:64333/Home/MainPageAfterLogin";
