! function() {
        "use strict";

        function e(e, t) {
            -1 !== t.navigator.platform.indexOf("Win") && (e.bodyClasses = ["os-windows"]);
        }

        angular.module("triangular").run(e), e.$inject = ["$rootScope", "$window"];
    }(), //Set the settings
    function() {
        "use strict";

        function e() {
            function e(e) {
                i.languages.push(e);
            }

            function t(e) {
                i.logo = e;
            }

            function a(e) {
                i.name = e;
            }

            function n(e) {
                i.copyright = e;
            }

            function l(e) {
                i.version = e;
            }

            var i = {
                languages: [],
                name: "",
                logo: "",
                copyright: "",
                version: ""
            };
            this.addLanguage = e, this.setLogo = t, this.setName = a, this.setCopyright = n, this.setVersion = l, this.$get = function() {
                return {
                    languages: i.languages,
                    name: i.name,
                    copyright: i.copyright,
                    logo: i.logo,
                    version: i.version,
                    defaultSkin: i.defaultSkin
                }
            }
        }

        angular.module("triangular").provider("triSettings", e);
    }(),
    function() {
        "use strict";

        function e(e) {
            e.state("triangular", {
                "abstract": !0,
                templateUrl: "app/triangular/layouts/default/default.tmpl.html",
                controller: "DefaultLayoutController",
                controllerAs: "layoutController"
            }).state("triangular-no-scroll", {
                "abstract": !0,
                templateUrl: "app/triangular/layouts/default/default-no-scroll.tmpl.html",
                controller: "DefaultLayoutController",
                controllerAs: "layoutController"
            }).state("triangular.admin-default", {
                "abstract": !0,
                views: {
                    sidebarLeft: {
                        templateUrl: "app/triangular/components/menu/menu.tmpl.html",
                        controller: "MenuController",
                        controllerAs: "vm"
                    },
                    sidebarRight: {
                        templateUrl: "app/triangular/components/notifications-panel/notifications-panel.tmpl.html",
                        controller: "NotificationsPanelController",
                        controllerAs: "vm"
                    },
                    toolbar: {
                        templateUrl: "app/triangular/components/toolbars/toolbar.tmpl.html",
                        controller: "DefaultToolbarController",
                        controllerAs: "vm"
                    },
                    content: {
                        template: '<div id="admin-panel-content-view" class="{{layout.innerContentClass}}" flex ui-view></div>'
                    },
                    belowContent: {
                        template: '<div ui-view="belowContent"></div>'
                    }
                }
            }).state("triangular.admin-default-no-scroll", {
                "abstract": !0,
                views: {
                    sidebarLeft: {
                        templateUrl: "app/triangular/components/menu/menu.tmpl.html",
                        controller: "MenuController",
                        controllerAs: "vm"
                    },
                    sidebarRight: {
                        templateUrl: "app/triangular/components/notifications-panel/notifications-panel.tmpl.html",
                        controller: "NotificationsPanelController",
                        controllerAs: "vm"
                    },
                    toolbar: {
                        templateUrl: "app/triangular/components/toolbars/toolbar.tmpl.html",
                        controller: "DefaultToolbarController",
                        controllerAs: "vm"
                    },
                    content: {
                        template: '<div flex ui-view layout="column" class="overflow-hidden"></div>'
                    }
                }
            });
        }

        angular.module("triangular").config(e), e.$inject = ["$stateProvider"];
    }();