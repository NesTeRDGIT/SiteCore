
    var myApp = angular.module("myApp", ["ngTouch", "ui.grid", "ui.grid.pagination", "ui.grid.selection", "ui.grid.cellNav", "ui.grid.resizeColumns", "ui.bootstrap", "ui.bootstrap.contextMenu", "ui.grid.autoResize", "Modal", "ui.grid.loader"]);
myApp.controller("Grid1", ["$scope", "$http", "uiGridConstants", "i18nService", "$templateCache", "ModalService", "$compile", 
    function ($scope, $http, uiGridConstants, i18nService, $templateCache, ModalService, $compile) {

           
        $scope.closeModal = function (id) {
            ModalService.Close(id);
        }
        //Страницы
        var paginationOptions = {
            pageNumber: 1,
            pageSize: 100,
            sort: null
        };

        $scope.SPR = {
            PROFIL: [],
            MKB: []
        };
        var FillSPR = false;

        function FILL_SPR() {
            var q = Promise.resolve();
            if (!FillSPR) {
                q = $http.get(`GetSPR`);
                q.then(function (response) {
                    const data = response.data;
                    const Result = data.Result;
                    const Value = data.Value;
                    if (Result === true) {
                        Value.F014.forEach((item) => {
                            item.DATEBEG = item.DATEBEG !== null ? new Date(item.DATEBEG) : null;
                            item.DATEEND = item.DATEEND !== null ? new Date(item.DATEEND) : null;
                        });
                        $scope.SPR = Value;
                        FillSPR = true;
                    } else {
                        alert(`Ошибка получения справочников:${Value}`);
                    }
                },
                    function (response) {
                        alert(`Ошибка получения справочников:${response.status}:${response.statusText}`);
                    })
                    .finally(function () {
                    });;
            }
            return q;
        }

        FILL_SPR();

        $scope.IsTFOMS = function () {
            return  $scope.CurrentTMK.SMO === "75";
        }

        $scope.init = function (IsTMKUser, IsTMKAdmin, IsTMKReader, IsTMKSMO, CODE_MO) {
            $scope.IsTMKUser = IsTMKUser;
            $scope.IsTMKAdmin = IsTMKAdmin;
            $scope.IsTMKReader = IsTMKReader;
            $scope.IsTMKSMO = IsTMKSMO;
            $scope.CODE_MO = CODE_MO;
        }


        //Опции
        $scope.gridOptions = {
            paginationPageSizes: [100, 200, 300],
            paginationPageSize: 100,
            virtualizationThreshold: 300,
            useExternalPagination: true,
            useExternalSorting: true,
            enableRowSelection: true,
            enableRowHeaderSelection: false,
            enableVerticalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
            multiSelect: true,
            modifierKeysToMultiSelect: true,
            noUnselect: true,
            enableColumnResizing: true,
            rowTemplate: rowTemplate(),
            //Колонки


            columnDefs: [
                { displayName: "", name: "STATUS", enableHiding: false, enableSorting: false, width: "40", cellTemplate: "<div style=\"position: relative; float: left;\"><img height=\"30px\" ng-src=\"{{grid.appScope.GetImageStatus(row, col)}}\" title=\"{{row.entity.STATUS_COM}}\" > <img height=\"30px\" ng-src=\"{{grid.appScope.GetImageStatusEXP(row, col)}}\" ng-hide=\"!row.entity.isEXP\" title=\"{{row.entity.STATUS_COM}}\" style=\"position: absolute; left: 0;\"/> </div> ", enableColumnMenu: false },
                { displayName: "№", name: "TMK_ID", enableSorting: false, enableHiding: false, width: "60", enableColumnMenu: false },
                { displayName: "ЕНП", name: "ENP", enableSorting: false, enableHiding: false, width: "140", enableColumnMenu: false },
                { displayName: "МО", name: "NAM_MOK", enableSorting: false, enableHiding: false, width: "180", enableColumnMenu: false, cellTemplate: "<div class=\"ui-grid-cell-contents ng-binding ng-scope tooltip\" title=\"{{row.entity.CONTACT_INFO}}\">{{row.entity.NAM_MOK}}</div>" },
                { displayName: "ФИО", name: "FIO", enableSorting: false, enableHiding: false, width: "150", enableColumnMenu: false },
                { name: "DATE_B", displayName: "Дата начала лечения", type: "date", enableSorting: false, cellFilter: "date:'dd.MM.yyyy'", enableHiding: false, width: "100", enableColumnMenu: false },
                { name: "DATE_QUERY", displayName: "Дата оформления запроса на ТМК", type: "date", enableSorting: false, cellFilter: "date:'dd.MM.yyyy'", enableHiding: false, width: "100", enableColumnMenu: false },
                { name: "DATE_PROTOKOL", displayName: "Дата получения протокола", type: "date", enableSorting: false, cellFilter: "date:'dd.MM.yyyy'", enableHiding: false, width: "100", enableColumnMenu: false },
                { name: "DATE_TMK", displayName: " Дата проведения очной консультации \\ консилиума", type: "date", enableSorting: false, cellFilter: "date:'dd.MM.yyyy'", enableHiding: false, width: "120", enableColumnMenu: false },
                { displayName: "Страхование", name: "SMO", enableSorting: false, enableHiding: false, width: "110", enableColumnMenu: false },
                { displayName: "Вид медицинской документации", name: "VID_NHISTORY", enableSorting: false, enableHiding: false, width: "150", enableColumnMenu: false },
                { displayName: "Признак оплаты", name: "OPLATA", enableSorting: false, enableHiding: false, width: "150", enableColumnMenu: false },
                { displayName: "Дата МЭК", name: "DATE_MEK", enableSorting: false, enableHiding: false, width: "90", enableColumnMenu: false },
                { displayName: "Дефекты МЭК", name: "DEF_MEK", enableSorting: false, enableHiding: false, width: "200", enableColumnMenu: false },
                { displayName: "Дата МЭЭ", name: "DATE_MEE", enableSorting: false, enableHiding: false, width: "90", enableColumnMenu: false },
                { displayName: "Дефекты МЭЭ", name: "DEF_MEE", enableSorting: false, enableHiding: false, width: "200", enableColumnMenu: false },
                { displayName: "Дата ЭКМП", name: "DATE_EKMP", enableSorting: false, enableHiding: false, width: "90", enableColumnMenu: false },
                { displayName: "Дефекты ЭКМП", name: "DEF_EKMP", enableSorting: false, enableHiding: false, width: "200", enableColumnMenu: false }
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
            },
            appScopeProvider: {
                onDblClick: function (row) {
                    GetDetail(row);
                },
                contextmenuOptions: function (row) {
                    if (row && !row.isSelected) {
                        $scope.gridApi.selection.clearSelectedRows();
                        row.setSelected(true);
                    }

                    const contextMenuData = [];
                    $scope.rightClickedRow = row;
                      
                    const rows =  $scope.gridApi.selection.getSelectedRows();
                    var isOpen = false;
                    var isClosed = false;
                    var isError = false;
                    var IsMy = false;

                    var isOpenALL = false;
                    var isClosedALL = false;
                    var isErrorALL = false;
                    var IsMyALL = false;
                    if (rows.length !== 0) {
                        isOpen = rows[0].STATUS === "Open";
                        isClosed = rows[0].STATUS === "Closed";
                        isError = rows[0].STATUS === "Error";
                        IsMy = rows[0].CODE_MO === $scope.CODE_MO;

                        isOpenALL = true;
                        isClosedALL = true;
                        isErrorALL = true;
                        IsMyALL = true;
                        for (let i = 0; i < rows.length; i++) {
                            const x = rows[i];
                            if (x.STATUS === "Open") {
                                isClosedALL = false;
                                isErrorALL = false;
                            }
                            if (x.STATUS === "Closed") {
                                isOpenALL = false;
                                isErrorALL = false;
                            }
                            if (x.STATUS === "Error") {
                                isClosedALL = false;
                                isOpenALL = false;
                            }
                              
                            if (x.CODE_MO !== $scope.CODE_MO) {
                                IsMyALL = false;
                            }
                        }
                    }
                        
                    contextMenuData.push(
                        {
                            text: '<span  style=\"font-weight: bold\">Просмотр</span>',
                            enabled: function () { return row; },
                            click: function ($itemScope, $event, modelValue, text, $li) {
                                GetDetail(row);
                            }
                        });

                  
                 

                    if ($scope.IsTMKUser) {
                        contextMenuData.push(
                            {
                                text: "Новая запись",
                                enabled: function () { return isOpen && IsMy; },
                                click: function ($itemScope, $event, modelValue, text, $li) {
                                    AddTMK();
                                }
                            });

                        contextMenuData.push(
                            {
                                text: "Редактировать",
                                enabled: function () { return isOpen && IsMy; },
                                click: function ($itemScope, $event, modelValue, text, $li) {
                                    const TMK_ID = $scope.gridApi.selection.getSelectedRows().map(function (x) { return x.TMK_ID })[0];
                                    EditTMK(TMK_ID);
                                }
                            });
                        contextMenuData.push(
                            {
                                text: "Удалить",
                                enabled: function () { return isOpenALL && IsMyALL; },
                                click: function ($itemScope, $event, modelValue, text, $li) {
                                    const TMK_ID = rows.map(function (x) { return x.TMK_ID });
                                    if (TMK_ID && TMK_ID.length !== 0) {
                                        if (!confirm(`Вы уверены что ходите удалить ${TMK_ID.length} записей`)) return;
                                        $http.post("DeleteTmkReestr", TMK_ID).then(function (response) {
                                                const data = response.data;
                                                const Result = data.Result;
                                                const Value = data.Value;
                                            if (Result !== true) {
                                                alert(`Ошибка:${Value}`);
                                            }
                                        },
                                            function (response) {
                                                alert(`Ошибка запроса:${response.status}: ${statusText}`);
                                            }).finally(function () { $scope.getPage() });
                                    } else {
                                        alert(`Не выбрано не одного элемента!`);
                                    }
                                }
                            });
                    }

                    if ($scope.IsTMKAdmin) {
                        contextMenuData.push(
                            {
                                text: "Изменить статус",
                                click: function ($itemScope, $event, modelValue, text, $li) {
                                    const TMK_ID = rows.map(function (x) { return x.TMK_ID });
                                    if (TMK_ID && TMK_ID.length !== 0) {
                                        if (!confirm(`Вы уверены что ходите изменить статус ${TMK_ID.length} записей`)) return;
                                        $http.post("ChangeTmkReestrStatus", TMK_ID).then(function (response) {
                                                const data = response.data;
                                                const Result = data.Result;
                                                const Value = data.Value;
                                            if (Result !== true) {
                                                alert(`Ошибка:${Value}`);
                                            } 
                                        },
                                            function (response) {
                                                alert(`Ошибка запроса:${response.status}:${response.statusText}`);
                                            }).finally(function () { $scope.getPage() });
                                    } else {
                                        alert(`Не выбрано не одного элемента!`);
                                    }
                                }
                            });
                    }

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
                    if (row.entity.STATUS === "Closed")
                        return "../Image/GreenIndicator.png";
                    if (row.entity.STATUS === "Open")
                        return "../Image/YelowIndicator.png";
                    if (row.entity.STATUS === "Error")
                        return "../Image/RedIndicator.png";
                    return "";
                },
                GetImageStatusEXP(row, col) {
                    if (row.entity.isEXP === true) {
                        return "../Image/InExpertIcon.png";
                    }
                    return "";
                }


            }
        };

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
            return [this.getFullYear(),(mm > 9 ? "" : "0") + mm,(dd > 9 ? "" : "0") + dd].join("-");
        };

        function serialize(obj, prefix) {
               
            var str = [],
                p;
            for (p in obj) {
                if (obj.hasOwnProperty(p)) {
                    var k = prefix ? prefix + "[" + p + "]" : p,
                        v = obj[p] instanceof Date ? obj[p].yyyymmdd() : obj[p];

                    if (typeof v === "string")
                        v = v.replace(" ", "%20");
                     
                    if (v !== null && v !== "" && !(isNaN(v) && typeof v === "number")) {
                        var s = (typeof v === "object") ? serialize(v, k) : encodeURIComponent(k) + "=" + v;
                        if(s!=="")
                            str.push(s);
                    }
                }
            }
            return str.join("&");
        }

        $scope.Filter =
        { 
            ENP : null,
            FAM : null,
            IM : null,
            OT : null,
            DR : null,
            DATE_B_BEGIN : null,
            DATE_B_END : null,
            DATE_QUERY_BEGIN : null,
            DATE_QUERY_END : null,
            DATE_PROTOKOL_BEGIN : null,
            DATE_PROTOKOL_END : null,
            DATE_TMK_BEGIN : null,
            DATE_TMK_END : null,
            CODE_MO: null,
            SMO: null,
            VID_NHISTORY: null,
            OPLATA: null
        };
            

        $scope.ClearFilter = function () {
            $scope.Filter.ENP = null;
            $scope.Filter.FAM = null;
            $scope.Filter.IM = null;
            $scope.Filter.OT = null;
            $scope.Filter.DR = null;
            $scope.Filter.DATE_B_BEGIN = null;
            $scope.Filter.DATE_B_END = null;
            $scope.Filter.DATE_QUERY_BEGIN = null;
            $scope.Filter.DATE_QUERY_END = null;
            $scope.Filter.DATE_PROTOKOL_BEGIN = null;
            $scope.Filter.DATE_PROTOKOL_END = null;
            $scope.Filter.DATE_TMK_BEGIN = null;
            $scope.Filter.DATE_TMK_END = null;
            $scope.Filter.CODE_MO = null;
            $scope.Filter.SMO = null;
            $scope.Filter.VID_NHISTORY = null;
            $scope.Filter.OPLATA = null;
                
            $scope.Find();
            setTimeout((function(){
                $("#FilterCODE_MO").trigger("change");
                $("#FilterSMO").val($scope.Filter.SMO ).trigger("change");
                $("#FilterVID_NHISTORY").trigger("change");
                $("#FilterOPLATA").trigger("change");

            }),100);
            
                 
        }

        $scope.getXLS= function () {
            var url;
            url = `GetTMKReestrFile?Page=${paginationOptions.pageNumber}&CountOnPage=${paginationOptions.pageSize}`;
            const Fillter = serialize($scope.Filter);
            if (Fillter)
                url += `&${Fillter}`;
            const downloadLink = window.document.createElement("a");
            downloadLink.setAttribute("download", "true");
            downloadLink.href = url;
            document.body.appendChild(downloadLink);
            downloadLink.click();
            document.body.removeChild(downloadLink);
        };

        $scope.Find = function () {
               
            if (paginationOptions.pageNumber!==1)
                $scope.gridApi.pagination.seek(1);
            else
                $scope.getPage();
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
        $scope.getPage = function () {

            ShowLoader();
            var url;
            url = `GetTMKList?Page=${paginationOptions.pageNumber}&CountOnPage=${paginationOptions.pageSize}`;
         
            var Fillter = serialize($scope.Filter);
            if (Fillter)
                url += "&"+Fillter;
            $scope.gridOptions.data = [];

            $http.get(url)
                .then(function (response) {
                    const data = response.data;
                    const Result = data.Result;
                    const Value = data.Value;
                    if (Result === true) {
                        $scope.gridOptions.totalItems = Value.count;
                        $scope.gridOptions.data = Value.items;
                    } else {
                        alert(Value);
                    }
                }, function (response) {
                    alert(`Ошибка запроса:${response.status}:${response.statusText}`);
                })
                .finally(function () {
                    HideLoader();
                });;
        };
          
        function GetDetail(row) {
            $scope.currentRow = row;
            const TMK_ID = row.entity.TMK_ID;
            var win = "custom-modal-1";
            ModalService.Content(win, "Загрузка...");
            ModalService.Caption(win, `Просмотр записи = ${TMK_ID}`);
            ModalService.Open(win);
            const url = `View?TMK_ID=${TMK_ID}`;
            $http.get(url)
                .then(function (response) {
                    const data = $compile(response.data)($scope);
                    ModalService.Content(win, data);
                    GetTMK(TMK_ID);
                    UpdateListExpertize();
                }, function (response) {
                  
                    alert(`Ошибка запроса:${response.status}:${response.statusText}`);
                })
                .finally(function () {

                });;
        }


        $scope.AddTMK = function() {
            AddTMK();
        }

        function AddTMK() {
            var win = "custom-modal-1";
            ModalService.Content(win, "Загрузка...");
            ModalService.Caption(win, "Создание записи");
            ModalService.Open(win);
            const url = "EditTMKReestr";
            $http.get(url)
                .then(function (response) {
                    const data = $compile(response.data)($scope);
                    ModalService.Content(win, data);
                    ClearTMK();
                }, function (response) {
                    alert(`Ошибка запроса:${response.status}:${response.statusText}`);
                })
                .finally(function () {
                });;
        }

        function EditTMK(TMK_ID) {
            var win = "custom-modal-1";
            ModalService.Content(win, "Загрузка...");
            ModalService.Caption(win, "Редактирование записи №" + TMK_ID);
            ModalService.Open(win);
            const url = "EditTMKReestr";
            $http.get(url)
                .then(function (response) {
                    const data = $compile(response.data)($scope);
                    ModalService.Content(win, data);
                    GetTMK(TMK_ID);
                }, function (response) {
                    alert(`Ошибка запроса:${response.status}:${response.statusText}`);
                })
                .finally(function () {
                });;
        }


        $scope.AddExpertizeDialog = function (type) {
            var row = $scope.currentRow.entity;
            var win = "custom-modal-2";
            ModalService.Caption(win, `Добавление экспертизы для записи №${row.TMK_ID}`);
            ModalService.Content(win, "Загрузка...");
            ModalService.Open(win);
            const url = `EditExpertize?TMK_ID=${row.TMK_ID}&&ExpertTip=${type}`;
            $http.get(url)
                .then(function (response) {
                    const data = $compile(response.data)($scope);
                    NewCurrentExpertize(type, row.TMK_ID);
                    ModalService.Content(win, data);
                }, function (response) {
                    alert(`Ошибка запроса:${response.status}:${response.statusText}`);
                })
                .finally(function () {
                });;
        }

        $scope.SaveExpertize = function () {
            
            var win = "custom-modal-2";            
            $http.post("EditExpertize", prePost($scope.CurrentExpertize))
                .then(function (response) {
                    const data = response.data;
                    const Result = data.Result;
                    const Value = data.Value;
                    if (Result === true) {
                        ModalService.Close(win);
                        UpdateListExpertize();
                    } else {
                        $scope.ErrExpertise = Value;
                    }
                }, function (response) {
                    alert(`Ошибка запроса:${response.status}:${response.statusText}`);
                })
                .finally(function () {
                });;
        }

       
        $scope.SaveTMK = function () {
            var win = "custom-modal-1";
          
            $http.post("EditTmkReestr", prePost($scope.CurrentTMK))
                .then(function (response) {
                    const data = response.data;
                    const Result = data.Result;
                    const Value = data.Value;
                    if (Result === true) {
                        ModalService.Close(win);
                        $scope.getPage();
                    } else {
                        $scope.ErrTMK = Value;
                    }
                }, function (response) {
                    alert(`Ошибка запроса:${response.status}:${response.statusText}`);
                })
                .finally(function () {
                });;
        }

        $scope.DeleteTMK = function () {
            var win = "custom-modal-1";
            $http.post("DeleteTmkReestr", [$scope.CurrentTMK.TMK_ID])
                .then(function (response) {
                    const data = response.data;
                    const Result = data.Result;
                    const Value = data.Value;
                    if (Result === true) {
                        ModalService.Close(win);
                        $scope.getPage();
                    } else {
                        $scope.ErrTMK = Value;
                    }
                }, function (response) {
                    alert(`Ошибка запроса:${response.status}:${response.statusText}`);
                })
                .finally(function () {
                });;
        }


        $scope.EditExpertizeDialog = function (EXPERTIZE_ID) {

            var win = "custom-modal-2";
            ModalService.Caption(win, `Редактирование экспертизы №${EXPERTIZE_ID}`);
            ModalService.Content(win, "Загрузка...");
            ModalService.Open(win);
            const url = `EditExpertize?EXPERTIZE_ID=${EXPERTIZE_ID}`;
            $http.get(url)
                .then(function (response) {
                    const data = $compile(response.data)($scope);
                    SetCurrentExpertize(EXPERTIZE_ID);
                    ModalService.Content(win, data);
                }, function (response) {
                    alert(`Ошибка запроса:${response.status}:${response.statusText}`);
                })
                .finally(function () {
                });;
        }

        $scope.DeleteExpertize = function(EXPERTIZE_ID) {
            if (confirm("Вы уверены что хотите удалить экспертизу?")) {
                const url = `DeleteExpertize?EXPERTIZE_ID=${EXPERTIZE_ID}`;
                $http.post(url)
                    .then(function (response) {
                            const data = response.data;
                            const Result = data.Result;
                            const Value = data.Value;
                            if (Result === true) {
                                UpdateListExpertize();
                            } else {
                                alert(Value);
                            }

                        },
                        function(response) {
                            alert(`Ошибка запроса:${response.status}:${response.statusText}`);
                        })
                    .finally(function() {
                    });;
            }
        }

        $scope.SMODataOK = null;
        $scope.SMODataStatus = null;
        $scope.SaveSMOData = function() {
            const url = `SetOPLATAandVID_NHISTORY`;
          const json = JSON.stringify({
                TMK_ID: $scope.CurrentTMK.TMK_ID,
                VID_NHISTORY: $scope.CurrentTMK.VID_NHISTORY,
                OPLATA: $scope.CurrentTMK.OPLATA,
                SMO_COM: $scope.CurrentTMK.SMO_COM
            });
          
            $http.post(url, json)
                .then(function(response) {
                        const data = response.data;
                        const Result = data.Result;
                        const Value = data.Value;
                        if (Result === true) {
                            $scope.SMODataStatus = "Изменения приняты";
                            $scope.SMODataOK = true;
                        } else {
                            $scope.SMODataOK = false;
                            $scope.SMODataStatus = `Ошибка: ${Value}`;
                        }
                    },
                    function(response) {
                        alert(`Ошибка запроса:${response.status}:${response.statusText}`);
                    })
                .finally(function() {
                });;
        }


        $scope.SetAsMTR = function() {
            const url = `SetAsMTR?TMK_ID=${$scope.CurrentTMK.TMK_ID}`;
            $http.post(url)
                .then(function(response) {
                        const data = response.data;
                        const Result = data.Result;
                        const Value = data.Value;
                        if (Result === true) {
                            GetDetail($scope.currentRow);
                        } else {
                            alert(Value);
                        }
                    },
                    function(response) {
                        alert(`Ошибка запроса:${response.status}:${response.statusText}`);
                    })
                .finally(function() {
                });;
        }

       

      

        function GetTMK(TMK_ID) {
            const url = `GetTmkReestr?TMK_ID=${TMK_ID}`;
            $scope.ErrTMK = {};
            $scope.CurrentTMK = {}
            $scope.IsLoadTMK = true;
            $scope.SMODataOK = null;
            $scope.SMODataStatus = null;
            $http.get(url)
                .then(function (response) {
                    const data = response.data;
                    const Result = data.Result;
                    const Value = data.Value;

                    if (Result===true) {
                        const item = Value;
                        item.DATE_TMK = item.DATE_TMK!==null ? new Date(item.DATE_TMK) :null;
                        item.DATE_PROTOKOL = item.DATE_PROTOKOL!==null ? new Date(item.DATE_PROTOKOL):null;
                        item.DATE_QUERY = item.DATE_QUERY!==null ? new Date(item.DATE_QUERY):null;
                        item.DATE_B = item.DATE_B!==null ? new Date(item.DATE_B):null;
                        item.DR = item.DR!==null ? new Date(item.DR):null;
                        item.DR_P = item.DR_P !== null ? new Date(item.DR_P) : null;
                        $scope.CurrentTMK = item;
                        
                    } else {
                        alert(Value);
                    }

                }, function (response) {
                    alert(`Ошибка запроса:${response.status}:${response.statusText}`);
                })
                .finally(function () {
                    $scope.IsLoadTMK = false;
                });;
        }

      

        function ClearTMK() {
            $scope.IsLoadTMK = true;
            $scope.CurrentTMK = {};
            $scope.CurrentTMK.TMK_ID = 0;
            $scope.IsLoadTMK = false;
        }


        function UpdateListExpertize() {
            const url = `GetExpertize?TMK_ID=${$scope.currentRow.entity.TMK_ID}`;
            $scope.IsLoadExpertize = true;
            $http.get(url)
                .then(function(response) {
                        const data = response.data;
                        const Result = data.Result;
                        const Value = data.Value;
                        if (Result === true) {
                            const exp = Value.map(function(item) {
                                item.DATEACT = item.DATEACT !== null ? new Date(item.DATEACT) : null;
                                return item;
                            });
                            $scope.Expertizes = exp;
                        } else {
                            alert(Value);
                        }
                    },
                    function(response) {
                        alert(`Ошибка запроса:${response.status}:${response.statusText}`);
                    })
                .finally(function() {
                    $scope.IsLoadExpertize = false;
                });;
        }


        function NewCurrentExpertize(type, TMK_ID) {
            $scope.CurrentExpertize = {};
            const CurrentExpertize = $scope.CurrentExpertize;
            CurrentExpertize.EXPERTIZE_ID = -1;
            CurrentExpertize.TMK_ID = TMK_ID;
            CurrentExpertize.OSN = [];
            CurrentExpertize.S_TIP = type;
            $scope.ErrExpertise = {};
        }


        function SetCurrentExpertize(EXPERTIZE_ID) {
            $scope.CurrentExpertize = {};
            const CurrentExpertize = $scope.CurrentExpertize;
            $scope.ErrExpertise = {};
            $scope.IsLoadExpertize = true;
            $http.get(`GetExpertize?EXPERTIZE_ID=${EXPERTIZE_ID}`)
                .then(function (response) {
                    const data = response.data;
                    const Result = data.Result;
                    const Value = data.Value;
                    if (Result===true) {
                        const exps = Value.map(function(item) {
                            item.DATEACT = item.DATEACT !== null ? new Date(item.DATEACT) : null;
                            return item;
                        });
                        const exp = exps[0];
                        CurrentExpertize.NUMACT = exp.NUMACT;
                        CurrentExpertize.DATEACT = exp.DATEACT;
                        CurrentExpertize.FIO = exp.FIO;
                        CurrentExpertize.FULL = exp.FULL;
                        CurrentExpertize.ISCOROLLARY = exp.ISCOROLLARY;
                        CurrentExpertize.ISNOTRECOMMEND = exp.ISNOTRECOMMEND;
                        CurrentExpertize.CELL = exp.CELL;
                        CurrentExpertize.ISOSN = exp.ISOSN;
                        CurrentExpertize.ISRECOMMENDMEDDOC = exp.ISRECOMMENDMEDDOC;
                        CurrentExpertize.NOTPERFORM = exp.NOTPERFORM;
                        CurrentExpertize.N_EXP = exp.N_EXP;
                        CurrentExpertize.S_TIP = exp.S_TIP;
                        CurrentExpertize.EXPERTIZE_ID = exp.EXPERTIZE_ID;
                        CurrentExpertize.TMK_ID = exp.TMK_ID;
                        CurrentExpertize.OSN = exp.OSN;
                    } else {
                        alert(response.data.Data);
                    }
                }, function (response) {
                    alert(`Ошибка запроса:${response.status}:${response.statusText}`);
                })
                .finally(function () {
                    $scope.IsLoadExpertize = false;
                });;
        }

        $scope.ClearExpertize = function ()
        {
            $scope.CurrentExpertize = {};
            $scope.Expertizes = {};
        }
        $scope.IsLoadExpertize = false;
        $scope.IsLoadTMK = false;
        $scope.ErrExpertise = {};
        $scope.CurrentExpertize = {}
        $scope.CurrentTMK = {}
        $scope.ErrTMK = {};



        $scope.addOSN= function() {
            $scope.CurrentExpertize.OSN.push({ S_OSN: null, S_COM: null, S_SUM: null, S_FINE: null });
        }
        $scope.deleteOSN = function (OSN) {
            const osn = $scope.CurrentExpertize.OSN;
            const index = osn.indexOf(OSN);
            if (index > -1) {
                osn.splice(index, 1);
            }
        }
        $scope.getPage();

        function rowTemplate() {
            return "<div ng-dblclick=\"grid.appScope.onDblClick(row)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell context-menu=\"grid.appScope.contextmenuOptions(row)\"  ></div>";
        }
        //Шаблон панели строк + контекстное меню
        $templateCache.put("ui-grid/uiGridViewport",
            "<div role=\"rowgroup\" class=\"ui-grid-viewport\" ng-style=\"colContainer.getViewportStyle()\" context-menu=\"grid.appScope.contextmenuOptions(row)\"><!-- tbody --><div class=\"ui-grid-canvas\"><div ng-repeat=\"(rowRenderIndex, row) in rowContainer.renderedRows track by $index\" class=\"ui-grid-row\" ng-style=\"Viewport.rowStyle(rowRenderIndex)\"><div role=\"row\" ui-grid-row=\"row\" row-render-index=\"rowRenderIndex\"></div></div></div></div>"
        );
    }]);

myApp.filter("S_OSN_NAME", function () {
    return function (S_OSN) {
        switch (S_OSN) {
            case 1:
                return "МЭК";
            case 2:
                return "МЭЭ";
            case 3:
                return "ЭКМП";
            default:
                return S_OSN;
        }
    };
});
myApp.filter("YesNo", function () {
    return function (input) {
        switch (input) {
            case true:
                return "Да";
            case false:
                return "Нет";
            default:
                return input;
        }
    };
});
myApp.filter("ToOSN", function () {
    return function (input) {
        switch (input) {
            case true:
                return "Обоснованно";
            case false:
                return "Необоснованно";
            default:
                return input;
        }
    };
});
myApp.filter("ToCDate", function () {
    return function (input) {
        if (input != null) {
            return new Date(parseInt(input.substr(6)));
        }
        return null;
    };
});

myApp.directive("convertToNumber", function () {
    return {
        require: "ngModel",
        link: function (scope, element, attrs, ngModel) {
            ngModel.$parsers.push(function (val) {
                return parseInt(val, 10);
            });
            ngModel.$formatters.push(function (val) {
                if (val!==null && val!==undefined)
                    return "" + val;
                return "";

            });
        }
    };
});


myApp.directive("select2", function ($timeout, $parse) {
    return {
        link: function (scope, element, attrs) {
            $timeout(function () {
                $(element).select2();
            });
        }
    };
});


myApp.filter("F014Actual", function () {
    return function (input, DT) {
        if (Array.isArray(input)) {
            return input.filter(val => val.DATEBEG <= DT && (val.DATEEND >= DT || val.DATEEND === null));
        }
        return null;
    };
});