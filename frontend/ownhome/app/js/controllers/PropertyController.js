/**
 * Created by joannarosedelmar on 29/3/15.
 */
myApp.controller('propertyController',
    function ($scope){

        if($window.sessionStorage.token){
            $scope.token = $window.sessionStorage.token;
        }



    });

