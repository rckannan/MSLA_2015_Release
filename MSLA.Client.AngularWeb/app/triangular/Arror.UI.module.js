(function() {
    'use strict';

    angular
        .module('Arror.UI', [
            'ngMaterial', 'Arror.UI.themes',
            'Arror.UI.layouts', 'Arror.UI.components',   'Arror.UI.directives', 'Arror.UI.router',
            // 'triangular.profiler',
            // uncomment above to activate the speed profiler
            'ui.router', 'kendo.directives'
        ]);
})();