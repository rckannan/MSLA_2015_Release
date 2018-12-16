(function() {
    'use strict';

    angular
        .module('Arror.UI')
        .run(runFunction);

    /* @ngInject */
    function runFunction($rootScope, $window) {
        // add a class to the body if we are on windows
        if($window.navigator.platform.indexOf('Win') !== -1) {
            $rootScope.bodyClasses = ['os-windows'];
        }
    }
})(); 

(function() {
    "use strict";

    function e(e, t, a, n, l, i, o, s, k) {
        function r() {
            i(function() {
                var e = o.title;
                angular.forEach(m.crumbs, function(t) {
                    e += " " + o.separator + " " + n("translate")(t.name);
                }), t.document.title = e;
            });
        }

        k.fillAuthData();

        function d() {
            r();
        }

        var m = s.breadcrumbs;
        d();
        var c = e.$on("$stateChangeSuccess", function() {
            r();
        });
        e.$on("$destroy", function() {
            c();
        });
    }

    angular.module("Arror.UI").run(e), e.$inject = ["$rootScope", "$window", "$state", "$filter", "$translate", "$timeout", "arrRoute", "arrBreadcrumbsService", 'arrAuthService'];
})();
