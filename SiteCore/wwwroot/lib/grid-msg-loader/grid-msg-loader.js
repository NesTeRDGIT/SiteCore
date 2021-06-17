
(function () {
    'use strict';
    var module = angular.module('ui.grid.loader', ['ng', 'ui.grid']);

    module.directive('uiGridLoader',
        [
            'gridUtil',
            function (gridUtil) {
                return {
                    priority: -200,
                    scope: false,
                    require: '^uiGrid',
                    link: {
                        pre: function ($scope, $elm, $attr, uiGridCtrl) {
                          
                            gridUtil.getTemplate('ui.grid.loader.template').then(function (contents) {
                                    const element = angular.element(contents);
                                    uiGridCtrl.grid.api.Loader = {
                                        Show: function () { element.css('display', 'block'); },
                                        Hide:   function () { element.css('display', 'none'); }
                                    };
                                    $elm.append(element);
                                    uiGridCtrl.innerCompile(element);
                                });
                        }
                    }
                };
            }
        ]);




})();

angular.module('ui.grid.loader').run(['$templateCache', function ($templateCache) {
    'use strict';
    $templateCache.put('ui.grid.loader.template',
        `<div class=\"grid-msg-overlay\">
            <div class=\"msg\">
                <div class=\"msg-content\">
                    <span>Загрузка данных...</span>
                    <div class=\"lds-hourglass\"></div>
                </div>
            </div>
        </div>`
    );
}]);