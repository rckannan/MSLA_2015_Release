(function() {
    'use strict';

    angular
        .module('Arror.UI.directives')
        .directive('paletteBackground', paletteBackground);

    /* @ngInject */
    function paletteBackground(arrTheming) {
        // Usage:
        // ```html
        // <div palette-background="green:500">Coloured content</div>
        // ```
        //
        // Creates:
        //
        var directive = {
            bindToController: true,
            link: link,
            restrict: 'A'
        };
        return directive;

        function link($scope, $element, attrs) {
            var splitColor = attrs.paletteBackground.split(':');
            var color = arrTheming.getPaletteColor(splitColor[0], splitColor[1]);

            if(angular.isDefined(color)) {
                $element.css({
                    'background-color': arrTheming.rgba(color.value),
                    'border-color': arrTheming.rgba(color.value),
                    'color': arrTheming.rgba(color.contrast)
                });
            }
        }
    }
})();