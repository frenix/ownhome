'use strict';

/** https://scotch.io/tutorials/angular-routing-using-ui-router **/
var myApp = angular.module('myApp', ['ui.router','ngResource', 'ui.bootstrap']);


myApp.config(function($stateProvider, $urlRouterProvider) {

    $urlRouterProvider.otherwise('/search');

    $stateProvider


        //search dashboard - home page
        .state('search', {
            url: '/search',
            templateUrl: 'search.html'
        })

        .state('signup', {
            url: '/signup',
            templateUrl: 'signup.html'
        })

        .state('login', {
            url: '/login',
            templateUrl: 'login.html'
        })

        .state('logout', {
            url: '/search',
            templateUrl: 'search.html',
            controller: function($window,$rootScope) {
                delete $window.sessionStorage.token;
                delete $rootScope.token ;
                //$rootScope.apply();
            }
        })

  /*      .state('property', {
            url: '/property',
            templateUrl: 'property-list.html',
            controller: function($scope, $window) {
                if($window.sessionStorage.token){
                    $scope.token = $window.sessionStorage.token;
                }
            }
        }) */

        // nested list with custom controller
        .state('my-properties', {
            url: '/myproperties',
            templateUrl: 'property-list-mine.html',
            views : {
                '': { templateUrl: 'property-list-mine.html'
                        /*controller: function($scope, $window) {

                            if($window.sessionStorage.token){
                                $scope.token = $window.sessionStorage.token;
                            }
                        }*/
                },

                // the child views will be defined here (absolutely named)
                'display@my-properties': { templateUrl: 'partials/property-form.html' }
            }

        })

        .state('all-properties', {
            url: '/allproperties',
            templateUrl: 'property-list-public.html'
        })

        .state('unauthorized', {
            url: '/unauthorized',
            templateUrl: 'partials/unauthorized.html'
        })

        // HOME STATES AND NESTED VIEWS ========================================
        .state('home', {
            url: '/home',
            templateUrl: 'partials/partial-home.html'
        })

        // nested list with custom controller
        .state('home.list', {
            url: '/list',
            templateUrl: 'partials/partial-home-list.html',
            controller: function($scope) {
                $scope.dogs = ['Bernese', 'Husky', 'Goldendoodle'];
            }
        })

        // nested list with just some random string data
        //.state('home.paragraph', {
        //    url: '/paragraph',
        //    template: 'I could sure use a drink right now.'
        //})
        .state('home.add', {
            url: '/add',
            templateUrl: 'partials/partial-add.html',
            controller: 'profileController'
        })

        .state('about', {
            url: '/about',
            views: {

                // the main template will be placed here (relatively named)
                '': { templateUrl: 'partials/partial-about.html' },

                // the child views will be defined here (absolutely named)
                'columnOne@about': { template: 'Look I am a column!' },

                // for column two, we'll define a separate controller
                'columnTwo@about': {
                    templateUrl: 'partials/table-data.html',
                    controller: 'scotchController'
                }
            }
        });

}); // closes $myApp.config()

//http://www.toptal.com/web/cookie-free-authentication-with-json-web-tokens-an-example-in-laravel-and-angularjs
/**The $http service of AngularJS allows us to communicate with the backend and make HTTP requests.
 * In our case we want to intercept every HTTP request and inject it with an Authorization header containing our JWT if the user is authenticated.
 * We can also use an interceptor to create a global HTTP error handler.
 * Here is an example of our interceptor that injects a token if it’s available in browser’s local storage
 */

myApp.config(function ($httpProvider) {
$httpProvider.interceptors.push(['$q', '$location','$window', function ($q, $location, $window) {
    return {
        'request': function (config) {
            config.headers = config.headers || {};

            console.log("here in interceptor token=" + $window.sessionStorage.token);

            if ($window.sessionStorage.token) {
                config.headers.Authorization = 'Bearer ' + $window.sessionStorage.token
            }
            return config;
        },
        'responseError': function (response) {
            console.log("ERROR here in interceptor");
            console.log("status=" + response.status)
            if (response.status === 401 || response.status === 403) {
                $location.path('/unauthorized');
            }
            return $q.reject(response);
        }
    };
}]);
});


