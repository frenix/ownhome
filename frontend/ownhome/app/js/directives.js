'use strict';

/* Directives */
//reference: http://stackoverflow.com/questions/17475595/how-can-i-make-a-directive-in-angularjs-to-validate-email-or-password-confirmati
myApp.directive('match', function($parse) {
    return {
        require: 'ngModel',
        link: function(scope, elem, attrs, ctrl) {
            scope.$watch(function() {
                return $parse(attrs.match)(scope) === ctrl.$modelValue;
            }, function(currentValue) {
                ctrl.$setValidity('mismatch', currentValue);
            });
        }
    };
});
