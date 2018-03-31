

var settings = {
    authority: "https://local.idm.globusfamily.com.au/core",
    client_id: 'mvcapp',
    redirect_uri: "http://id.jsclient.com",
    post_logout_redirect_uri: "http://id.jsclient.com",
    //silent_redirect_uri: "http://id.jsclient.com",
    //automaticSilentRenew: true,
    response_type: 'id_token token',
    scope: "openid",
    monitorSession: true,
    filterProtocolClaims: true,
    loadUserInfo: true
};



Oidc.Log.logger = console;
Oidc.Log.level = Oidc.Log.ERROR;

var mgr = new Oidc.UserManager(settings);

// OIDC User Manager Events
mgr.events.addAccessTokenExpiring(function () {
    console.log("token expiring");

});

mgr.events.addAccessTokenExpired(function () {
    console.log("token expired");
    removeUserStorage();
    signoutContact();
});

mgr.events.addSilentRenewError(function (e) {
    console.log("silent renew error", e.message);

});

mgr.events.addUserLoaded(function (user) {
    console.log("user loaded", user);
});

mgr.events.addUserUnloaded(function (e) {
    console.log("user unloaded" + e);
});

function getUser() {
    mgr.getUser().then(function (user) {
        console.log("getUser loaded user after userLoaded event fired");
    });
}

// Functions for UI elements
function signinContact() {
    mgr.signinRedirect().then(function () {
        console.log("signinRedirect then");
    }).catch(function (err) {
        console.log(err);
    });
}

function signinContactSilent() {
    mgr.signinSilent().then(function (user) {
        console.log("signinRedirect then");
    }).catch(function (err) {
        console.log(err);
    });
}

function signoutContact() {
    mgr.signoutRedirect().then(function (resp) {
        console.log("signoutRedirect then", resp);
        removeUserStorage();
    }).catch(function (err) {
        console.log(err);
    });
}

function endSigninMainWindow() {

    var myurl = window.location.hash;
    var url = myurl.replace('#/id_token', '#id_token');
    url = window.location.protocol + "//" + window.location.host + url;
    mgr.signinRedirectCallback({ data: url })
        .then(function (user) {
            postSigninRedirect(user);
        })
        .catch(function (err) {
            console.log(err);
        });
}

// Things to do after redirecting from identity signin
function postSigninRedirect(user) {
    console.log("postSigninRedirect", user);
    storeUser(user);
    window.location.replace('/');
}

if (window.location.hash) {
    endSigninMainWindow();
}