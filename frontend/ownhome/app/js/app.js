'use strict';

/** https://scotch.io/tutorials/angular-routing-using-ui-router **/
var myApp = angular.module('myApp', ['ui.router','ngResource']);


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

        .state('myproperties', {
            url: '/myproperties',
            templateUrl: 'partials/partial-home-list.html',
            controller: function($scope) {
                $scope.dogs = ['Bernese', 'Husky', 'Goldendoodle'];
            }
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

/**
 * Reference: http://mikehadlow.blogspot.com/2014/04/json-web-tokens-owin-and-angularjs.html
 * This does two things: on the outbound request it adds an Authorization header ‘Bearer <token>’ if the token is present.
 * This will be decoded by our OWIN middleware to authorize each request. The interceptor also checks the response.
 * If there’s a 401 (unauthorized) response, it simply bumps the user back to the login screen.
 */
/*myApp.factory('authInterceptor', function ($rootScope, $q, $window, $location) {
    return {
        request: function (config) {
            config.headers = config.headers || {};
            if($window.sessionStorage.token) {
                config.headers.Authorization = 'Bearer ' + $window.sessionStorage.token;
            }
            return config;
        },
        responseError: function (response) {
            if(response.status === 401) {
                $location.path('/login');
            }
            return $q.reject(response);
        }
    };
});

myApp.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptor');
});
*/
//http://www.toptal.com/web/cookie-free-authentication-with-json-web-tokens-an-example-in-laravel-and-angularjs
/**The $http service of AngularJS allows us to communicate with the backend and make HTTP requests.
 * In our case we want to intercept every HTTP request and inject it with an Authorization header containing our JWT if the user is authenticated.
 * We can also use an interceptor to create a global HTTP error handler.
 * Here is an example of our interceptor that injects a token if it’s available in browser’s local storage
 */

$httpProvider.interceptors.push(['$q', '$location', '$localStorage', function ($q, $location, $localStorage) {
    return {
        'request': function (config) {
            config.headers = config.headers || {};
            if ($window.sessionStorage.token) {
                config.headers.Authorization = 'Bearer ' + $window.sessionStorage.token
            }
            return config;
        },
        'responseError': function (response) {
            if (response.status === 401 || response.status === 403) {
                $location.path('/login');
            }
            return $q.reject(response);
        }
    };
}]);