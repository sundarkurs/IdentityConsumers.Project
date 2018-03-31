var app = angular.module("jsApp", []).run(run);;

run.$inject = ['$rootScope'];
function run($rootScope) {

    console.log("run");
    var signinResponse = null;
    if (!isNullorEmpty(localStorage.getItem("SignInResponse"))) {
        signinResponse = JSON.parse(localStorage.getItem("SignInResponse"));
    }

    if (signinResponse) {
        if ($rootScope.accessToken == undefined) {
            $rootScope.accessToken = signinResponse.access_token;
            $rootScope.userLoggedIn = signinResponse.profile.firstname + " " + signinResponse.profile.lastname;
        }
    } else {
        signinContactSilent();
    }
}

app.controller("headerController", ["$scope", "$rootScope", "$http", function ($scope, $rootScope, $http) {

    console.log("header controller");

    setTimeout(function () {

        $rootScope.$apply(function () {
            var signinResponse = null;
            if (!isNullorEmpty(localStorage.getItem("SignInResponse"))) {
                signinResponse = JSON.parse(localStorage.getItem("SignInResponse"));
                console.log(signinResponse);
            }

            if (signinResponse) {
                if ($rootScope.accessToken == undefined) {
                    $rootScope.accessToken = signinResponse.access_token;
                    $rootScope.userLoggedIn = signinResponse.profile.firstname + " " + signinResponse.profile.lastname;

                    $http.defaults.headers.common["Access-Control-Allow-Origin"] = '*';
                    $http.defaults.headers.common["Authorization"] = 'Bearer ' + $rootScope.accessToken;
                }
            }

        });
    }, 1000);

    $scope.login = function () {
        signinContact();
    };

    $scope.signout = function () {
        signoutContact();
    };

}]);


app.controller("indexController",
    [
        "$rootScope", "$scope", "$http", function ($rootScope, $scope, $http) {

            $http.defaults.headers.common["Access-Control-Allow-Origin"] = '*';
            $http.defaults.headers.common["Authorization"] = 'Bearer ' + $rootScope.accessToken;

            console.log($rootScope.accessToken);
            console.log("index controller");

            $scope.welcomeMessage = "Welcome to angular";

            $scope.callApi = function () {
                debugger;
                $http({
                    url: "http://id.webapi.com/api/Values",
                    method: "GET",
                    //data: data,
                    //params: parameters,
                    timeout: 300000
                }).then(function (response) {
                    console.log(response.data);
                }).catch(function (error) {
                    console.log(error);
                });

            }
        }
    ]);

function isNullorEmpty(val) {
    return (val === undefined || val == null || val === "" || val === "undefined") ? true : false;
}