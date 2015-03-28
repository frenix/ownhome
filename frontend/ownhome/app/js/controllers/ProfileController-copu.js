routerApp.controller('profileController',
    function ($scope , $http, $resource){

        $scope.messsage='test';

        //default values
        $scope.myform = {
            firstname: "Joanna Rose",
            lastname: "Del Mar",
            description: "Pretty woman walking down the street"
        };

        $scope.add = function(){
            console.log("Test 123");

        }//end of add


        $scope.add = function(item, event){
            //send the information to localhost:3000

            console.log("--> Submitting form");
            var dataObject = {
                firstname: $scope.myform.firstname ,
                lastname: $scope.myform.lastname,
                description: $scope.myform.description
            };
            
            //test for neil
            var url ="https://jira.atlassian.com/rest/api/latest/issue/JRA-9?expand=names,renderedFields&callback=JSON_CALLBACK"  ;

            /*$http({ method: "JSONP", url: url}).
                    success(function(data, status) {
                     console.log("SUCCESS STATUS => " + status);
                     console.log("SUCCESS STATUS => " + data);
                    }).
                    error(function(data, status) {
                      console.log( "ERROR Request failed ERROR=>"  + data);
                      console.log( "ERROR Status=>"  + status);
                  });
                };    */


        var req = {
             callback: "JSON_CALLBACK",
             method: 'JSONP',
             url: url,
             headers: {
               'Content-Type': 'application/json'
             },
             data: { }
                };
            $http(req).success(function(dataFromServer, status, headers, config) {
                  console.log(dataFromServer);
              });
             $http(req).error(function(data, status, headers, config) {
                  alert("Submitting form failed!");
              });

             /*var BackendAPI = $resource(url,
                { callback: "JSON_CALLBACK"},
                { get: { method: "JSONP" ,
                    headers: {

                        "Accept" : "application/jsonp; charset=utf-8",
                        "Content-Type": "application/javascript; charset=utf-8",
                        }
                }
                });
            //BackendAPI.get();
            
            /*var responsePromise = $http.post("http://localhost:3000/persons", dataObject, {});
            responsePromise.success(function(dataFromServer, status, headers, config) {
                console.log(dataFromServer);
            });
            responsePromise.error(function(data, status, headers, config) {
                alert("Submitting form failed!");
            });*/

           /*BackendAPI.get().$promise.then(
                function (data){
                    console.log( data.toString() );
                },
                function (error){
                    console.log("Submitting form failed! ERROR=" + error);
                }

            );*/









        }//end of add


}//end of controller function


);//end of controller