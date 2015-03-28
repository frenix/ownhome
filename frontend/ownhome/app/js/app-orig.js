'use strict';

// Declare app level module which depends on filters, and services
//var migrationApp = angular.module('migrationApp',[]);
var migrationApp = angular.module('migrationApp',[]);

/** for the sample router
 * https://scotch.io/tutorials/angular-routing-using-ui-router
 * **/
// app.js
var routerApp = angular.module('routerApp', ['ui.router','ngResource']);




routerApp.config(function($stateProvider, $urlRouterProvider) {

    $urlRouterProvider.otherwise('/home');

    $stateProvider

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

}); // closes $routerApp.config()


