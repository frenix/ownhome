/**
 * Created by joannarosedelmar on 28/3/15.
 */
myApp.controller('sessionController',
    function ($scope, $window){

        if($window.sessionStorage.token){
            $scope.token = $window.sessionStorage.token;
        }

    });