
var myApp = angular.module("myApp", ["ngTouch", "ui.grid", "ui.grid.pagination", "ui.grid.selection", "ui.grid.cellNav", "ui.grid.resizeColumns", "ui.bootstrap", "ui.bootstrap.contextMenu", "ui.grid.loader"]);
myApp.controller("Grid1", ["$scope", "$http", "uiGridConstants", "i18nService", "$templateCache",
    function ($scope, $http, uiGridConstants, i18nService, $templateCache) {

        const prePost = (obj) => {
            const copy = angular.copy(obj);

            for (let prop in copy) {
                if (Object.prototype.hasOwnProperty.call(copy, prop)) {
                    if (copy[prop] instanceof Date) {
                        copy[prop] = copy[prop].yyyymmdd();
                    }
                }
            }
            return copy;
        };


        Date.prototype.yyyymmdd = function () {
            const mm = this.getMonth() + 1; // getMonth() is zero-based
            const dd = this.getDate();
            return [this.getFullYear(), (mm > 9 ? "" : "0") + mm, (dd > 9 ? "" : "0") + dd].join("-");
        };
        //Локализация
        const vm = this;
        vm.langs = i18nService.getAllLangs();
        vm.lang = "ru";
      
        $scope.currentCSListItem = undefined;
            
        //Страницы
        var paginationOptions = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };
        //Опции
        $scope.gridOptions = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            useExternalPagination: true,
            useExternalSorting: true,
            enableRowSelection: true,
            enableRowHeaderSelection: false,
            multiSelect: true,
            modifierKeysToMultiSelect: true,
            noUnselect: true,
            rowHeight: 30,
            enableColumnResizing: true,
            rowTemplate: rowTemplate(),
            //Колонки
            columnDefs: [
                { displayName: "Статус", name: "STATUS", enableHiding: false, enableSorting: false, width: "100", cellTemplate: "<img height=\"30px\" ng-src=\"{{grid.appScope.GetImageStatus(row, col)}}\" title=\"{{row.entity.STATUS_RUS}}\" style=\"display: block;margin-left: auto;margin-right: auto\"  >", enableColumnMenu: false },
                { displayName: "МО", name: "CODE_MO", enableSorting: false, enableHiding: false, width: "100", enableColumnMenu: false },
                { displayName: "Наименование", name: "CAPTION", enableSorting: false, enableHiding: false, width: "*", enableColumnMenu: false },
                { name: "DATE_CREATE", displayName: "Дата", type: "date", enableSorting: false, cellFilter: "date:'dd.MM.yyyy'", enableHiding: false, width: "100", enableColumnMenu: false }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                //Сортировка - серверная
                $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                    if (sortColumns.length === 0) {
                        paginationOptions.sort = null;
                    } else {
                        paginationOptions.sort = sortColumns[0].sort.direction;
                    }
                    $scope.getPage();
                });

                //Страницы
                gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    paginationOptions.pageNumber = newPage;
                    paginationOptions.pageSize = pageSize;
                    $scope.getPage();
                });
                //Изменения текущего
                gridApi.selection.on.rowSelectionChanged($scope, function (row, evt) {
                    $scope.currentCSListItem = row.entity;
                    $scope.$broadcast("Grid1_rowSelectionChanged", row.entity);
                });

            },
            appScopeProvider: {
                onDblClick: function (row) {
                    $scope.$broadcast("Grid1_DblClick", row.entity);
                },
                contextmenuOptions: function (row) {
                    if (row && !row.isSelected) {
                        $scope.gridApi.selection.clearSelectedRows();
                        row.setSelected(true);
                    }
                      
                    var contextMenuData = [];
                    $scope.rightClickedRow = row;
                    var isSend = false;
                    var isSetNew = false;

                    var rows =  $scope.gridApi.selection.getSelectedRows();

                    for (let i = 0; i < rows.length; i++) {
                        const x = rows[i];
                        if (x.STATUS === "New")
                            isSend = true;
                        if (x.STATUS === "Answer" || x.STATUS === "Error")
                            isSetNew = true;
                    }


                    contextMenuData.push(
                        {
                            text: "Новый",
                            click: function($itemScope, $event, modelValue, text, $li) {
                                $http.get("NewCSList")
                                    .then(function(response) {
                                            $("#ModalAjaxContent").html(response.data);
                                            showModal();
                                        },
                                        function(response) {
                                            alert(`Ошибка запроса:${response.status}:${response.xhrStatus} ${response
                                                .statusText}`);
                                        });
                            }
                        });

                    contextMenuData.push(
                        {
                            text: "Отправить",
                            enabled: function () { return isSend; },
                            click: function($itemScope, $event, modelValue, text, $li) {
                                const CS_LIST_ID = $scope.gridApi.selection.getSelectedRows().map(function (x) { return x.CS_LIST_ID });
                                if (CS_LIST_ID && CS_LIST_ID.length !== 0) {
                                    if (!confirm(`Вы уверены что ходите отправить ${CS_LIST_ID.length} записей`)) return;
                                    $http.post("SendCSList", CS_LIST_ID).then(function (response) {
                                            const data = response.data.Value;
                                            const Result = response.data.Result;
                                        if (Result === false) { alert(`Ошибка:${data}`); }
                                        },
                                        function (response) {
                                            alert(`Ошибка запроса:${response.status}:${response.xhrStatus} ${response.statusText}`);
                                        }).finally(function () { $scope.getPage() });
                                } else {
                                    alert(`Не выбрано не одного элемента!`);
                                }
                            }
                        });



                    contextMenuData.push(
                        {
                            text: 'Отметить как "Новый"',
                            enabled: function(){return isSetNew}, 
                            click: function ($itemScope, $event, modelValue, text, $li) {
                                const CS_LIST = $scope.gridApi.selection.getSelectedRows();
                                const CS_LIST_ID = CS_LIST.map(function (x) { return x.CS_LIST_ID });
                              
                                if (CS_LIST_ID && CS_LIST_ID.length !== 0) {
                                    if (!confirm(`Вы уверены что ходите открыть ${CS_LIST_ID.length} записей`)) return;
                                    $http.post("OpenCSList", CS_LIST_ID).then(function (response) {
                                            const data = response.data.Value;
                                            const Result = response.data.Result;
                                            if (Result === false) { alert(`Ошибка:${data}`); }
                                        },
                                        function (response) {
                                            alert(`Ошибка запроса:${response.status}:${response.xhrStatus} ${response.statusText}`);
                                        }).finally(function () {
                                            $scope.CheckStatusByID(CS_LIST_ID);
                                    
                                    });
                                } else {
                                    alert(`Не выбрано не одного элемента!`);
                                }
                            }
                        });

                      

                    contextMenuData.push(
                        {
                            text: "Удалить",
                            click: function($itemScope, $event, modelValue, text, $li) {
                                const CS_LIST_ID = $scope.gridApi.selection.getSelectedRows()
                                    .map(function(x) { return x.CS_LIST_ID });
                                if (CS_LIST_ID && CS_LIST_ID.length !== 0) {
                                    if (!confirm(`Вы уверены что ходите удалить ${CS_LIST_ID.length} записей`)) return;
                                    $http.post("DeleteCSList", CS_LIST_ID).then(function(response) {
                                            const data = response.data.Value;
                                            const Result = response.data.Result;
                                            if (Result === true) {
                                                $scope.$broadcast("Grid1_onDelete", CS_LIST_ID);
                                            } else {
                                                alert(`Ошибка:${data}`);
                                            }
                                        },
                                        function(response) {
                                            alert(`Ошибка запроса:${response.status}:${response.xhrStatus} ${response.statusText}`);
                                        }).finally(function() { $scope.getPage() });
                                } else {
                                    alert(`Не выбрано не одного элемента!`);
                                }
                            }
                        });
                           
                    contextMenuData.push(
                        {
                            text: "Обновить",
                            click: function($itemScope, $event, modelValue, text, $li) {
                                $scope.getPage();
                            }
                        });
                       
                    return contextMenuData;
                },
                GetImageStatus(row, col) {
                    if (row.entity.STATUS === "New")
                        return "../Image/documentedit.png";
                    if (row.entity.STATUS === "OnSend")
                        return "../Image/Onsend.png";
                    if (row.entity.STATUS === "Send")
                        return "../Image/Send.png";
                    if (row.entity.STATUS === "FLK")
                        return "../Image/sendandOK.png";
                    if (row.entity.STATUS === "Answer")
                        return "../Image/IconOK.png";
                    if (row.entity.STATUS === "Error")
                        return "../Image/IconERROR.png";
                    return "";
                }

               
            }
        };

        //Получить лог
        $scope.GetServiceLog= function() {
            $http.get("GetServiceLog")
                .then(function(response) {
                        $("#ModalAjaxContent").html(response.data);
                        showModal();
                    },
                    function(response) {
                        alert(`Ошибка запроса:${response.status}:${response.xhrStatus} ${response.statusText}`);
                    });
        }

        //Показать инструкцию
        $scope.ShowInstruction = function() {
            $http.get("GetInstruction")
                .then(function(response) {
                        $("#ModalAjaxContent").html(response.data);
                        showModal();
                    },
                    function(response) {
                        alert(`Ошибка запроса:${response.status}:${response.xhrStatus} ${response.statusText}`);
                    });
        }


        //Проверка доступности сервиса
        $scope.IsCheckServiceAv = true;
        $scope.CheckServiceAv = function() {
            $scope.IsCheckServiceAv = null;
            $http.get("GetServiceState")
                .then(function(response) {
                        const data = response.data.Value;
                        const Result = response.data.Result;
                        $scope.IsCheckServiceAv = data;
                    },
                    function(response) {
                        alert(`Ошибка запроса:${response.status}:${response.xhrStatus} ${response.statusText}`);
                    });
        }

        function ShowLoader() {
            if ($scope.gridApi) {
                $scope.gridApi.Loader.Show();
            }
        }
        function HideLoader() {
            if ($scope.gridApi) {
                $scope.gridApi.Loader.Hide();
            }
        }

        //получение данных
        $scope.getPage = function() {
            ShowLoader();
            var url;
            switch (paginationOptions.sort) {
            case uiGridConstants.ASC:
                url = `GetCSList?Page=${paginationOptions.pageNumber}&CountOnPage=${paginationOptions.pageSize}`;
                break;
            case uiGridConstants.DESC:
                url = `GetCSList?Page=${paginationOptions.pageNumber}&CountOnPage=${paginationOptions.pageSize}`;
                break;
            default:
                url = `GetCSList?Page=${paginationOptions.pageNumber}&CountOnPage=${paginationOptions.pageSize}`;
                break;
            }
            $scope.gridOptions.data = [];

            $http.get(url)
                .then(function(response) {
                        const data = response.data.Value;
                        const Result = response.data.Result;
                        if (Result === true) {
                            $scope.gridOptions.totalItems = data.count;
                            $scope.gridOptions.data = data.items;
                        } else {
                            alert(`Ошибка запроса:${data}`);
                        }
                    },
                    function(response) {
                        alert(`Ошибка запроса:${response.status}:${response.xhrStatus}`);
                    })
                .finally(function() {
                    HideLoader();
                });;
        };
        $scope.getPage();
        $scope.CheckServiceAv();
       
        

        $scope.CheckStatusByID = function (ID) {
            const  url = `GetCSListByID?CS_LIST_ID=${ID.join("&CS_LIST_ID=")}`;
            $http.get(url)
                .then(function (response) {
                    const data = response.data.Value;
                    const Result = response.data.Result;
                    if (Result===true) {
                        data.items.forEach(x => {
                            var OLDItem = FindDataByID(x.CS_LIST_ID);
                            if (OLDItem) {
                                if (OLDItem.STATUS !== x.STATUS) {
                                    OLDItem.STATUS = x.STATUS;
                                    OLDItem.STATUS_RUS = x.STATUS_RUS;
                                    $scope.$broadcast("Grid1_onChangeStatus", OLDItem);
                                }
                            }
                            else {
                                alert(`Ошибка запроса:${data}`);
                            }
                        });
                    }
                    
                }, function (response) {
                    console.writeln(`Ошибка запроса:${response.data}`);
                })
                .finally(function () {
                  
                });;
        }


        function FindDataByID(CS_LIST_ID) {
            return $scope.gridOptions.data.find(x => x.CS_LIST_ID === CS_LIST_ID);
        }

           

        function rowTemplate() {
            return "<div ng-dblclick=\"grid.appScope.onDblClick(row)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell context-menu=\"grid.appScope.contextmenuOptions(row)\"  ></div>";
        }
        //Шаблон панели строк + контекстное меню
        $templateCache.put("ui-grid/uiGridViewport",
            "<div role=\"rowgroup\" class=\"ui-grid-viewport\" ng-style=\"colContainer.getViewportStyle()\" context-menu=\"grid.appScope.contextmenuOptions(row)\"><!-- tbody --><div class=\"ui-grid-canvas\"><div ng-repeat=\"(rowRenderIndex, row) in rowContainer.renderedRows track by $index\" class=\"ui-grid-row\" ng-style=\"Viewport.rowStyle(rowRenderIndex)\"><div role=\"row\" ui-grid-row=\"row\" row-render-index=\"rowRenderIndex\"></div></div></div></div>"
        );
    }]);

myApp.controller("Grid2", ["$scope", "$http", "uiGridConstants", "i18nService","$templateCache",
    function ($scope, $http, uiGridConstants, i18nService, $templateCache) {
        //Локализация
        const vm = this;
        vm.langs = i18nService.getAllLangs();
        vm.lang = "ru";
     
          
        //Страницы
        var paginationOptions = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };
        //Опции

        $scope.gridOptions = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            useExternalPagination: true,
            useExternalSorting: true,
            enableRowSelection: true,
            enableRowHeaderSelection: false,
            multiSelect: true,
            modifierKeysToMultiSelect: true,
            noUnselect: true,
            rowHeight: 30,
            enableColumnResizing: true,
            rowTemplate:rowTemplate() ,
            //Колонки
            columnDefs: [
                { displayName: "Статус", name: "STATUS", enableHiding: false, enableSorting: false, width: "100", cellTemplate: "<img height=\"30px\" ng-src=\"{{grid.appScope.GetImageStatus(row, col)}}\" title=\"{{row.entity.STATUS_RUS}}\" style=\"display: block;margin-left: auto;margin-right: auto\"  >", enableColumnMenu: false },
                { displayName: "Фамилия", name: "FAM", enableSorting: false, enableHiding: false, width: "200", enableColumnMenu: false },
                { displayName: "Имя", name: "IM", enableSorting: false, enableHiding: false, width: "200", enableColumnMenu: false },
                { displayName: "Отчество", name: "OT", enableSorting: false, enableHiding: false, width: "200", enableColumnMenu: false },
                { displayName: "Дата рождения", name: "DR", type: "date", cellFilter: "date:'dd.MM.yyyy'", enableHiding: false, enableSorting: false, width: "200", enableColumnMenu: false },
                { displayName: "Полис", name: "POLIS", enableSorting: false, enableHiding: false, width: "400", enableColumnMenu: false },
                { displayName: "Документ", name: "DOC", enableSorting: false, enableHiding: false, width: "400", enableColumnMenu: false },
                { displayName: "Текущая СМО", name: "CURRENT_SMO", enableSorting: false, enableHiding: false, width: "400", enableColumnMenu: false }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;


                //Сортировка - серверная
                $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                    if (sortColumns.length === 0) {
                        paginationOptions.sort = null;
                    } else {
                        paginationOptions.sort = sortColumns[0].sort.direction;
                    }
                    $scope.getPage();
                });

                //Страницы
                gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    paginationOptions.pageNumber = newPage;
                    paginationOptions.pageSize = pageSize;
                    $scope.getPage();
                });
                //Изменения текущего
                gridApi.selection.on.rowSelectionChanged($scope, function (row, evt) {
                     
                });

            },
            appScopeProvider: {
                onDblClick: function (row) {
                    if (row) {
                        $scope.showSMO(row.entity.CS_LIST_IN_ID);
                    }
                },
                contextmenuOptions: function (row) {
                    var contextMenuData = [];
                    if (row && !row.isSelected) {
                        $scope.gridApi.selection.clearSelectedRows();
                        row.setSelected(true);
                    }

                    $scope.rightClickedRow = row;
                    var isNew = false;
                    if ($scope.currentCSListItem) {
                        isNew = $scope.currentCSListItem.STATUS === "New";
                    }
                    var isEdit = false;
                    if ($scope.currentCSListItem) {
                        if (row) {
                            isEdit = $scope.currentCSListItem.STATUS === "New";
                        }
                    }
                       
                    contextMenuData.push(
                        {
                            text: "Новый",
                            enabled: function() { return isNew; },
                            click: function($itemScope, $event, modelValue, text, $li) {
                                if ($scope.currentCSListItem.CS_LIST_ID) {
                                    $http.get(`NewCSItem?CS_LIST_ID=${$scope.currentCSListItem.CS_LIST_ID}`)
                                        .then(function(response) {
                                                const data = response.data.Value;
                                                const Result = response.data.Result;
                                                if (Result === true) {
                                                    $("#ModalAjaxContent").html(data);
                                                    showModal();
                                                } else {
                                                    alert(`Ошибка запроса:${data}`);
                                                }
                                            },
                                            function(response) {
                                                alert(`Ошибка запроса:${response.status}:${response.xhrStatus} ${response.statusText}`);
                                            });
                                } else {
                                    alert(`Не выбран не один список для добавления`);
                                }
                            }
                        });
                    

                    contextMenuData.push(
                        {
                            text: "Редактировать",
                            enabled: function() { return isEdit; },
                            click: function($itemScope, $event, modelValue, text, $li) {
                                if ($scope.currentCSListItem.CS_LIST_ID) {

                                    $http.get(`NewCSItem?CS_LIST_ID=${$scope.currentCSListItem.CS_LIST_ID}&CS_LIST_IN_ID=${row.entity.CS_LIST_IN_ID}`)
                                        .then(function(response) {
                                                const data = response.data.Value;
                                                const Result = response.data.Result;
                                                if (Result === true) {
                                                    $("#ModalAjaxContent").html(data);
                                                    showModal();
                                                } else {
                                                    alert(`Ошибка запроса:${data}`);
                                                }
                                            },
                                            function(response) {
                                                alert(`Ошибка запроса:${response.status}:${response.xhrStatus} ${response.statusText}`);
                                            });
                                } else {
                                    alert(`Не выбран не один список для добавления`);
                                }
                            }
                        });




                    contextMenuData.push(
                        {
                            text: "Просмотр",
                            enabled: function() { if (row) { return row.entity.STATUS === true; } else return false; },
                            click: function($itemScope, $event, modelValue, text, $li) {
                                if ($scope.currentCSListItem.CS_LIST_ID) {
                                    $scope.showSMO(row.entity.CS_LIST_IN_ID);
                                } else {
                                    alert(`Не выбран не один элемент`);
                                }
                            }
                        });

                    contextMenuData.push(
                        {
                            text: "Удалить",
                            enabled: function() { return isNew; },
                            click: function($itemScope, $event, modelValue, text, $li) {
                                const CS_LIST_IN_ID = $scope.gridApi.selection.getSelectedRows()
                                    .map(function(x) { return x.CS_LIST_IN_ID });
                                if (CS_LIST_IN_ID && CS_LIST_IN_ID.length !== 0) {
                                    if (!confirm(`Вы уверены что ходите удалить ${CS_LIST_IN_ID.length} записей`))
                                        return;
                                    $http.post("DeleteCSItem", CS_LIST_IN_ID).then(function(response) {
                                            const data = response.data.Value;
                                            const Result = response.data.Result;
                                            if (Result === true) {
                                                alert("Успешно");
                                            } else {
                                                alert(`Ошибка:${data}`);
                                            }
                                        },
                                        function(response) {
                                            alert(`Ошибка запроса:${response.status}:${response.xhrStatus} ${response
                                                .statusText}`);
                                        }).finally(function() { $scope.getPage() });
                                } else {
                                    alert(`Не выбрано не одного элемента!`);
                                }
                            }
                        });


                    contextMenuData.push(
                        {
                            text: "Обновить",
                            enabled: function () { return $scope.currentCSListItem != null; },
                            click: function ($itemScope, $event, modelValue, text, $li) {
                                $scope.getPage();
                            }
                        });

                    return contextMenuData;
                },
                GetImageStatus(row, col) {
                      
                    if (row.entity.STATUS === true)
                        return "../Image/IconOK.png";
                    if (row.entity.STATUS === false)
                        return "../Image/IconERROR.png"; 
                    return "../Image/question.png";
                }

            }
        };
   

        $scope.showSMO= function(CS_LIST_IN_ID) {
            $http.get(`ViewResult?CS_LIST_IN_ID=${CS_LIST_IN_ID}`)
                .then(function(response) {
                        const data = response.data.Value;
                        const Result = response.data.Result;
                        if (Result === true) {
                            $("#ModalAjaxContent").html(data);
                            showModal();
                        } else {
                            alert(`Ошибка:${data}`);
                        }
                    },
                    function(response) {
                        alert(`Ошибка запроса:${response.data}`);
                    });
        }
        $scope.$on("Grid1_rowSelectionChanged", function (event, data) {
             
             
        });
        $scope.$on("Grid1_onChangeStatus", function (event, data) {
       
            if ($scope.currentCSListItem) {
                if ($scope.currentCSListItem.CS_LIST_ID === data.CS_LIST_ID) {
                    $scope.currentCSListItem.STATUS = data.STATUS;
                    $scope.getPage();
                }
            }
        });

        $scope.$on("Grid1_onDelete", function (event, data) {
        
            if ($scope.currentCSListItem) {
                if (data.includes($scope.currentCSListItem.CS_LIST_ID)) {
                    $scope.currentCSListItem = null;
                    $scope.getPage();
                }
            }
        });
           

        $scope.$on("Grid1_DblClick", function (event, data) {
            $scope.currentCSListItem = data;
            $scope.getPage();
        });

       

        $scope.currentCSListItem = null;


        function ShowLoader() {
            if ($scope.gridApi) {
                $scope.gridApi.Loader.Show();
            }
        }
        function HideLoader() {
            if ($scope.gridApi) {
                $scope.gridApi.Loader.Hide();
            }
        }
        //получение данных
        $scope.getPage = function() {
         $scope.gridOptions.data = [];
            ShowLoader();
            if ($scope.currentCSListItem) {
                const CS_LIST_ID = $scope.currentCSListItem.CS_LIST_ID;
                const url = `GetCSListIN?CS_LIST_ID=${CS_LIST_ID}&Page=${paginationOptions.pageNumber}&CountOnPage=${paginationOptions.pageSize}`;

                $http.get(url)
                    .then(function(response) {
                            const data = response.data.Value;
                            const Result = response.data.Result;
                            if (Result === true) {
                                $scope.gridOptions.totalItems = data.count;
                                $scope.gridOptions.data = data.items;
                            } else {
                                alert(`Ошибка запроса :${data}`);
                            }
                        },
                        function(response) {
                            alert(`Ошибка запроса:${response.status}:${response.xhrStatus} ${response.statusText}`);
                        })
                    .finally(function() {
                        HideLoader();
                    });;
            };
        };

        function rowTemplate() {
            return "<div ng-dblclick=\"grid.appScope.onDblClick(row)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell context-menu=\"grid.appScope.contextmenuOptions(row)\"  ></div>";
        }
        //Шаблон панели строк + контекстное меню
        $templateCache.put("ui-grid/uiGridViewport",
            "<div role=\"rowgroup\" class=\"ui-grid-viewport\" ng-style=\"colContainer.getViewportStyle()\" context-menu=\"grid.appScope.contextmenuOptions(row)\"><!-- tbody --><div class=\"ui-grid-canvas\"><div ng-repeat=\"(rowRenderIndex, row) in rowContainer.renderedRows track by $index\" class=\"ui-grid-row\" ng-style=\"Viewport.rowStyle(rowRenderIndex)\"><div role=\"row\" ui-grid-row=\"row\" row-render-index=\"rowRenderIndex\"></div></div></div></div>"
        );
    }]);
