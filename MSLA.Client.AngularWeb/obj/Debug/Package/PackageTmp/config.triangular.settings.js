(function() {
    'use strict';

    angular
        .module('Arror')
        .config(translateConfig);

    /* @ngInject */
    function translateConfig(arrSettingsProvider, arrRouteProvider, APP_LANGUAGES) {
        var now = new Date();
        // set app name & logo (used in loader, sidemenu, footer, login pages, etc)
        arrSettingsProvider.setName('Arror UI');
        arrSettingsProvider.setCopyright('&copy;' + now.getFullYear() + ' aeiou.com');
        arrSettingsProvider.setLogo('assets/images/logo.png');
        // set current version of app (shown in footer)
        arrSettingsProvider.setVersion('0.1');
        // set the document title that appears on the browser tab
        arrRouteProvider.setTitle('Arror UI');
        arrRouteProvider.setSeparator('|');

        // setup available languages in triangular
        for (var lang = APP_LANGUAGES.length - 1; lang >= 0; lang--) {
            arrSettingsProvider.addLanguage({
                name: APP_LANGUAGES[lang].name,
                key: APP_LANGUAGES[lang].key
            });
        }
    }
})();