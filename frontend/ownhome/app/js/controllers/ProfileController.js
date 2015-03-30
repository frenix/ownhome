myApp.controller('profileController',
    function ($scope , $http, $resource, $window, $location, $rootScope){

        var backendUrl = "http://192.168.254.104:8088";

        //default values for the profile form
        $scope.profile = {
            fname: "",
            lname: "",
            email: "",
            cemail: "",
            password: "",
            cpassword: ""
        }

        //test for hash authentication
        $scope.authentication = {
            APIId: "4d53bce03ec34c0a911182d4c228ee6c",
            APIKey: "A93reRTUJHsCuQSHR+L3GxqOJyDmQpCgps102ciuabc=",
            user: $window.sessionStorage.user,
            exp:""
        };

        $scope.login = function(){
            console.log("here in login");

            var dataObject = {
                email: $scope.profile.email,
                password: $scope.profile.password,
            };

            //var url = backendUrl + "/login"
            var url = backendUrl + "/auth" //test for authentication
            var responsePromise = $http.post(url, dataObject, {});
            responsePromise.success(function(data, status, headers, config) {
                console.log("success=" + JSON.stringify(data));

                //test
                //sample: http://mikehadlow.blogspot.com/2014/04/json-web-tokens-owin-and-angularjs.html
                //<header>.<payload>.<hash>
                //header: { "typ": "JWT", "alg": "HMACSHA256" }
                //payload: data:{"user":"mike","exp":123456789}

                /**var data = {
                    token: "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyIjoibWlrZSIsImV4cCI6MTIzNDU2Nzg5fQ.KG-ds05HT7kK8uGZcRemhnw3er_9brQSF1yB2xAwc_E"
                    //sample, generated from server
                };**/

                $window.sessionStorage.token = data.token;

                var payload = angular.fromJson($window.atob(data.token.split('.')[1]));
                $window.sessionStorage.user = payload.user;

                $window.sessionStorage.email = payload.email;
                $window.sessionStorage.userId = payload.userId;

                console.log("data:" + JSON.stringify(payload));
                //data:{"user":"mike","exp":123456789}
                console.log("data decrypt:" + JSON.stringify( $window.sessionStorage ));
                //token
                console.log("token=" + $window.sessionStorage.token);


                //redirect: read up, comment first
                //$rootScope.$emit("LoginController.login");
                $rootScope.token = data.token;
                $location.path('/myproperties');

            });

            //debug using failed message first
            responsePromise.error(function(data, status, headers, config) {
                console.log("Submitting form failed!");

                /** simulation **/
                var data = {
                    token: "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyIjoibWlrZSIsImV4cCI6MTIzNDU2Nzg5fQ.KG-ds05HT7kK8uGZcRemhnw3er_9brQSF1yB2xAwc_E"
                    //sample, generated from server
                };

                $window.sessionStorage.token = data.token;
                var payload = angular.fromJson($window.atob(data.token.split('.')[1]));
                console.log("data decrypt:" + JSON.stringify( $window.sessionStorage ));                //token
                console.log("token=" + $window.sessionStorage.token);
                $rootScope.token = data.token;
                $location.path('/myproperties');


                // Erase the token if the user fails to login
                // delete $window.sessionStorage.token;

                // $scope.message = 'Error: Invalid email or password';

            });
        },

        /** Note: Connection with backend service working as of 26 March 2015
         *  POST -> working send signup details
         *  TODO: authentication
         */
        $scope.signup = function(){
            //send the information to localhost:3000

            console.log("--> Submitting form");

            //if($scope.profileForm.$valid){

                var dataObject = {
                    FirstName: $scope.profile.fname,
                    LastName: $scope.profile.lname,
                    EmailAddress: $scope.profile.email,
                    Password: $scope.profile.password,
                    AuthKey: ""
                };

                console.log("dataObject:" + JSON.stringify(dataObject));

                //var url = "http://localhost:3000/persons";
                var url = backendUrl + "/Agents"
                var responsePromise = $http.post(url, dataObject, {});
                responsePromise.success(function(dataFromServer, status, headers, config) {
                    console.log("success=" + dataFromServer);
                });
                responsePromise.error(function(data, status, headers, config) {
                    console.log("Submitting form failed!");
                });
            //}


//note: follow this => http://jasonwatmore.com/post/2015/03/10/AngularJS-User-Registration-and-Login-Example.aspx








        }//end of add


}//end of controller function


);//end of controller