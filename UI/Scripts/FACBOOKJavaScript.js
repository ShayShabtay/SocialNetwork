

function initFacebook(){
        window.fbAsyncInit = function () {
            FB.init({
                appId: '505809253241981',
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
      js.src = "https://connect.facebook.net/en_US/sdk.js#xfbml=1&version=v3.2&appId=505809253241981&autoLogAppEvents=1";
     fjs.parentNode.insertBefore(js, fjs);
   }(document, 'script', 'facebook-jssdk'));
}


function Facebook_LoggedIn() {
    console.log(this);
    FB.api('/me', { locale: 'en_US', fields: 'id,name,email,birthday,location' },
        ////נסיון
        //user.setEmail(me.email),
        //user.save(),//סוף נסיון 
        function (response) {
            console.log(response);

            FB.getLoginStatus(function (getLoginStatusResponse) {
                console.log(getLoginStatusResponse);
                if (getLoginStatusResponse.status === 'connected') {
                    console.log(getLoginStatusResponse.authResponse.accessToken);
                    $.post("http://localhost:64333/account/LoginFacebook?token=" + getLoginStatusResponse.authResponse.accessToken);
                    //setCookie(userName, email);
                    //getCookie(userName, email);
                    document.cookie = getLoginStatusResponse.authResponse.email + ";" + ";path=http://localhost:64333";
                    //document.cookie = "id=302534912" + "accessToken=" + getLoginStatusResponse.authResponse.accessToken;// + getLoginStatusResponse.user., "name=" + user.name, "email" + user.email;//, birthday, location;
                    //checkCookie();

                    //redirect
                    window.location.href = "http://localhost:64333/Home/MainPageAfterLogin";

                    ////cookies
                    //FB.api('/me', function (me) {
                    //    user.set("facebook_id", me.id);
                    //    user.set("facebook_link", me.link);
                    //    user.set("firstName", me.first_name);
                    //    user.set("lastName", me.last_name);
                    //    user.setEmail(me.email);
                    //    user.save().then(function () {
                    //        //go to new page
                    //        //redirect
                    //        window.location.href = "http://localhost:64333/Home/MainPageAfterLogin";
                    //    });
                    //});
                    /////// end cookies
                }
            });

        });

}

// send http post request

   
 

//function setCookie(userName, email) {
//    document.cookie = userName + "=" + email + ";" + ";path=/http://localhost:64333";
//}

//function getCookie(userName, email) {
//    var userName = userName + "=";
//    var email = email + "=";
//    var facecookies = userName + email;
//    var decodedCookie = decodeURIComponent(document.cookie);
//    var ca = decodedCookie.split(';');
//    for (var i = 0; i < ca.length; i++) {
//        var c = ca[i];
//        while (c.charAt(0) == ' ') {
//            c = c.substring(1);
//        }
//        if (c.indexOf(facecookies) == 0) {
//            return c.substring(facecookies.length, c.length);
//        }
//    }
//    return "";
//}


//function setCookie(cname, cvalue, exdays) {
//    var d = new Date();
//    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
//    var expires = "expires=" + d.toGMTString();
//    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
//}

//function getCookie(cname) {
//    var name = cname + "=";
//    var decodedCookie = decodeURIComponent(document.cookie);
//    var ca = decodedCookie.split(';');
//    for (var i = 0; i < ca.length; i++) {
//        var c = ca[i];
//        while (c.charAt(0) == ' ') {
//            c = c.substring(1);
//        }
//        if (c.indexOf(name) == 0) {
//            return c.substring(name.length, c.length);
//        }
//    }
//    return "";
//}

//function checkCookie() {
//    var user = getCookie("username");
//    if (user != "") {
//        alert("Welcome again " + user);
//    } else {
//        user = prompt("Please enter your name:", "");
//        if (user != "" && user != null) {
//            setCookie("username", user, 30);
//        }
//    }
//}