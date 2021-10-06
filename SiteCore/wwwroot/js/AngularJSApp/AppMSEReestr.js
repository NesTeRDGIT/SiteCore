
var myApp = angular.module("myApp", ["ngTouch", "ui.grid", "ui.grid.pagination", "ui.grid.selection", "ui.grid.cellNav", "ui.grid.resizeColumns", "ui.bootstrap", "ui.bootstrap.contextMenu", "ui.grid.autoResize", "Modal", "ui.grid.loader", "ui.select", "ngSanitize"]);
myApp.controller("Grid1", ["$scope", "$http","$q", "uiGridConstants", "i18nService", "$templateCache", "ModalService", "$compile", 
    function ($scope, $http, $q, uiGridConstants, i18nService, $templateCache, ModalService, $compile) {
        $scope.numberPattert = /^[0-9]+(\.[0-9]{1,2})?$/;

        $scope.IsMSEAdmin = false;
        $scope.IsMSESmo = false;
        $scope.OwnerRow = false;
        $scope.CurrentExpertize = {}
      

        $scope.init = function(IsMSEAdmin, IsMSESmo) {
            $scope.IsMSEAdmin = IsMSEAdmin;
            $scope.IsMSESmo = IsMSESmo;
        };

        function CheckOwner() {
            $scope.OwnerRow = $scope.CurrentMSE.SMO === "75" && $scope.IsMSEAdmin || $scope.IsMSESmo;
        }
        $scope.SPR = {
            PROFIL: [],
            MKB: []
        };
        var FillSPR = false;

        function FILL_SPR() {
            var q = Promise.resolve();
            if (!FillSPR) {
                q = $http.get(`GetSPR`);
                q.then(function(response) {
                            const data = response.data;
                            const Result = data.Result;
                            const Value = data.Value;
                            if (Result === true) {
                                Value.PROFIL.forEach((item) => {
                                    item.DATEBEG = item.DATEBEG !== null ? new Date(item.DATEBEG) : null;
                                    item.DATEEND = item.DATEEND !== null ? new Date(item.DATEEND) : null;
                                });
                                Value.F014.forEach((item) => {
                                    item.DATEBEG = item.DATEBEG !== null ? new Date(item.DATEBEG) : null;
                                    item.DATEEND = item.DATEEND !== null ? new Date(item.DATEEND) : null;
                                });
                                Value.MKB.forEach((item) => {
                                    item.DATE_B = item.DATE_B !== null ? new Date(item.DATE_B) : null;
                                    item.DATE_E = item.DATE_E !== null ? new Date(item.DATE_E) : null;
                                });
                                $scope.SPR = Value;

                                FillSPR = true;
                            } else {
                                alert(`Ошибка получения справочников:${Value}`);
                            }
                        },
                        function(response) {
                            alert(`Ошибка получения справочников:${response.status}:${response.statusText}`);
                        })
                    .finally(function() {
                    });;
            }
            return q;
        }
        function FindF014(val, DT) {
           return  FILL_SPR().then(function() {
                const item = $scope.SPR.F014.find(x => x.KOD === val && x.DATEBEG <= DT && (x.DATEEND >= DT || x.DATEEND === null));
                if (item)
                    return item.FullName;
                return "";
            });
        }

      

        $scope.closeModal = function (id) {
            ModalService.Close(id);
        }
        //Страницы
        var paginationOptions = {
            pageNumber: 1,
            pageSize: 100,
            sort: null
        };


        //Опции
        $scope.gridOptions = {
            paginationPageSizes: [100, 200, 300, 1000, 2000],
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
                { displayName: "ЕНП", name: "ENP", enableSorting: false, enableHiding: false, width: "140", enableColumnMenu: false },
                { displayName: "МО", name: "NAM_MOK", enableSorting: false, enableHiding: false, width: "500", enableColumnMenu: false, cellTemplate: "<div class=\"ui-grid-cell-contents ng-binding ng-scope tooltip\" title=\"{{row.entity.CONTACT_INFO}}\">{{row.entity.NAM_MOK}}</div>" },
                { displayName: "ФИО", name: "FIO", enableSorting: false, enableHiding: false, width: "150", enableColumnMenu: false },
                { name: "D_FORM", displayName: "Дата принятия решения о направлении на МСЭ", type: "date", enableSorting: false, cellFilter: "date:'dd.MM.yyyy'", enableHiding: false, width: "100", enableColumnMenu: false },
                { name: "D_PROT", displayName: "Дата направления информации в ТФОМС", type: "date", enableSorting: false, cellFilter: "date:'dd.MM.yyyy'", enableHiding: false, width: "100", enableColumnMenu: false },
                { displayName: "Страхование", name: "SMO", enableSorting: false, enableHiding: false, width: "220", enableColumnMenu: false },
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
                }
            }
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
            DR: null,
            SNILS: null,
            D_FORM_BEGIN : null,
            D_FORM_END : null,
            D_PROT_BEGIN : null,
            D_PROT_END : null,
            CODE_MO : null,
            SMO : null
        };
            

        $scope.ClearFilter = function () {
            $scope.Filter.ENP = null;
            $scope.Filter.FAM = null;
            $scope.Filter.IM = null;
            $scope.Filter.OT = null;
            $scope.Filter.DR = null;
            $scope.Filter.SNILS = null;
            $scope.Filter.D_FORM_BEGIN = null;
            $scope.Filter.D_FORM_END = null;
            $scope.Filter.D_PROT_BEGIN = null;
            $scope.Filter.D_PROT_END = null;
            $scope.Filter.CODE_MO = null;
            $scope.Filter.SMO = null;
                
            $scope.Find();
            setTimeout((function(){
                $("#FilterSMO").val($scope.Filter.SMO ).trigger("change");
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
            url = `GetMSEList?Page=${paginationOptions.pageNumber}&CountOnPage=${paginationOptions.pageSize}`;
         
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
            $scope.IsLoadMSE = true;
            ClearMSE();
            $scope.currentRow = row;
            const MSE_ID = row.entity.MSE_ID;
            const win = "custom-modal-1";
            ModalService.Content(win, "Загрузка...");
            ModalService.Caption(win, `Просмотр записи №${MSE_ID}`);
            ModalService.Open(win);
            const url = `ViewMSEItem`;
            $http.get(url)
                .then(function (response) {
                    FILL_SPR();
                    const data = response.data;
                    const Result = data.Result;
                    const Value = data.Value;
                    if (Result === true) {
                      
                        const view = $compile(Value)($scope);
                        ModalService.Content(win, view);
                        GetMSE(MSE_ID);
                    } else {
                        ModalService.Close(win);
                        alert(Value);
                    }
                }, function (response) {
                    ModalService.Close(win);
                    alert(`Ошибка запроса:${response.status}:${response.statusText}`);
                })
                .finally(function () {

                });;

        }

         function  GetMSE(MSE_ID) {

            const url = `GetMSEItem?MSE_ID=${MSE_ID}`;
            $scope.ErrMSE = "";
            $scope.IsLoadMSE = true;
            $scope.SMODataOK = null;
            $scope.SMODataStatus = null;
         
            $http.get(url)
                .then(function (response) {
                    const data = response.data;
                    const Result = data.Result;
                    const Value = data.Value;

                    if (Result === true) {
                        const item = Value;
                        item.DATE_LOAD = item.DATE_LOAD !== null ? new Date(item.DATE_LOAD) : null;
                        item.D_PROT = item.D_PROT !== null ? new Date(item.D_PROT) : null;
                        item.D_FORM = item.D_FORM !== null ? new Date(item.D_FORM) : null;
                        item.DR = item.DR !== null ? new Date(item.DR) : null;
                        item.SLUCH.forEach(sl => {
                            sl.Expertizes.forEach(exp => {
                                exp.OSN.forEach(osn => {
                                    FindF014(osn.S_OSN, item.DATE_LOAD).then(function(val) {
                                        osn.OSN_NAME = val;
                                    });

                                });
                            });
                        });
                      
                        $scope.CurrentMSE = item;
                    } else {
                        $scope.ErrMSE = Value;
                    }
                }, function (response) {
                    alert(`Ошибка запроса:${response.status}:${response.statusText}`);
                })
                .finally(function () {
                    $scope.IsLoadMSE = false;
                    CheckOwner();
                });;
        }

        $scope.RefreshMSE = function ()
        {
            if ($scope.CurrentMSE.MSE_ID) {
                GetMSE($scope.CurrentMSE.MSE_ID);
            }
        }

        function ClearMSE() {
            $scope.CurrentMSE = {};
            $scope.CurrentMSE.DATE_LOAD = new Date();
            $scope.CurrentMSE.D_PROT = new Date();
            $scope.CurrentMSE.D_FORM = new Date();
            $scope.CurrentMSE.DR = new Date();
        }

        $scope.SetAsMTR =  function() {
            const MSE_ID = $scope.CurrentMSE.MSE_ID;
            if (MSE_ID) {
                if (confirm("Вы уверены что хотите пометить случай как МТР?")) {
                    const url = `SetAsMTR`;
                 
                    $http.post(url, $scope.CurrentMSE.MSE_ID)
                        .then(function(response) {
                                const data = response.data;
                                const Result = data.Result;
                                const Value = data.Value;
                                if (Result === true) {
                                    alert("Успешно");
                                } else {
                                    alert(Value);
                                }
                            },
                            function(response) {
                                alert(`Ошибка запроса:${response.status}:${response.statusText}`);
                            })
                        .finally(function() {
                            GetMSE(MSE_ID);
                        });;
                }
            }
        }

        $scope.ShowEditSluch = function (MSE_SLUCH_ID)
        {
            const win = "custom-modal-2";
            ModalService.Content(win, "Загрузка...");
            ModalService.Caption(win, `Редактор случая`);
            ModalService.Open(win);

            var url = `EditSluch?MSE_ID=${$scope.CurrentMSE.MSE_ID}`;
            if (MSE_SLUCH_ID)
                url += `&&MSE_SLUCH_ID=${MSE_SLUCH_ID}`;
         
            $http.get(url)
                .then(function (response) {
                        
                        const data = response.data;
                        const Result = data.Result;
                        const Value = data.Value;
                        if (Result === true) {
                            ModalService.Content(win, Value);
                        } else {
                            alert(Value);
                        }
                    },
                    function (response) {
                        alert(`Ошибка запроса:${response.status}:${response.statusText}`);
                    })
                .finally(function () {});;
        }

        $scope.CloseModal2 = function () {
            const win = "custom-modal-2";
            ModalService.Close(win);
        }

        $scope.UpdateModal2 = function (data) {
            const win = "custom-modal-2";
            ModalService.Content(win, data);
        }
        $scope.SMODataOK = null;
        $scope.SMODataStatus = null;

        $scope.SaveSMO_DATA = function () {
          
            const url = `SetSMO_DATA`;
           

            $http.post(url, $scope.CurrentMSE.SMO_DATA)
                .then(function (response) {
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
                    function (response) {
                        alert(`Ошибка запроса:${response.status}:${response.statusText}`);
                    })
                .finally(function () {
                });;
        }

        $scope.DeleteSLUCH = function (MSE_SLUCH_ID) {
            const url = `DeleteSluch`;
            $http.post(url, MSE_SLUCH_ID)
                .then(function (response) {
                        const data = response.data;
                        const Result = data.Result;
                        const Value = data.Value;
                        if (Result === true) {
                            $scope.RefreshMSE();
                        } else {
                            alert(`Ошибка: ${Value}`);
                        }
                    },
                    function (response) {
                        alert(`Ошибка запроса:${response.status}:${response.statusText}`);
                    })
                .finally(function () {
                });;
        }

        $scope.NewACT = function (S_TIP, MSE_SLUCH_ID) {
            ClearCurrentExpertize();
            $scope.CurrentExpertize.isNEW = true;
            $scope.CurrentExpertize.S_TIP = S_TIP;
            $scope.CurrentExpertize.MSE_SLUCH_ID = MSE_SLUCH_ID;

            const win = "custom-modal-2";
            ModalService.Content(win, "Загрузка...");
            ModalService.Caption(win, ``);
            ModalService.Open(win);
         
            $http.get(`EditExpertize`)
                .then(function (response) {
                        const data = response.data;
                        const Result = data.Result;
                        const Value = data.Value;
                        if (Result === true) {
                            ModalService.Content(win, $compile(Value)($scope));
                        } else {
                            alert(Value);
                        }
                    },
                    function (response) {
                        alert(`Ошибка запроса:${response.status}:${response.statusText}`);
                    })
                .finally(function () { });;
        }
        $scope.EditACT = function (EXPERTIZE_ID) {
            ClearCurrentExpertize();
            const win = "custom-modal-2";
            ModalService.Content(win, "Загрузка...");
            ModalService.Caption(win, ``);
            ModalService.Open(win);

            $http.get(`EditExpertize`)
                .then(function (response) {
                        
                        const data = response.data;
                        const Result = data.Result;
                        const Value = data.Value;
                        if (Result === true) {
                            ModalService.Content(win, $compile(Value)($scope));
                            GetExpertize(EXPERTIZE_ID);
                        } else {
                            alert(Value);
                        }
                    },
                    function (response) {
                        alert(`Ошибка запроса:${response.status}:${response.statusText}`);
                    })
                .finally(function () { });;
        }

        function GetExpertize(EXPERTIZE_ID) {

            $http.get(`GetExpertize?EXPERTIZE_ID=${EXPERTIZE_ID}`)
                .then(function(response) {
                        const data = response.data;
                        const Result = data.Result;
                        const Value = data.Value;
                        if (Result === true) {
                            Value.DATEACT = Value.DATEACT !== null ? new Date(Value.DATEACT) : null;
                            $scope.CurrentExpertize = Value;
                            $scope.CurrentExpertize.isNEW = false;
                        } else {
                            alert(Value);
                        }
                    },
                    function(response) {
                        alert(`Ошибка запроса:${response.status}:${response.statusText}`);
                    })
                .finally(function() {});;

        }


        $scope.AddOSN = function () {
            if ($scope.CurrentExpertize.OSN === undefined)
                $scope.CurrentExpertize.OSN = [];
            $scope.CurrentExpertize.OSN.push({ S_OSN: null, S_COM: null, S_SUM: null, S_FINE: null });
        }

        $scope.DelOSN = function (OSN) {
            const osn = $scope.CurrentExpertize.OSN;
            const index = osn.indexOf(OSN);
            if (index > -1) {
                osn.splice(index, 1);
            }
        }

        function ClearCurrentExpertize() {
            $scope.CurrentExpertize = {};
            $scope.expERR = [];
        }

        $scope.SaveExpertize = function (form) {
            
            if (form.$valid) {
                $http.post(`EditExpertize`, $scope.CurrentExpertize )
                    .then(function (response) {
                            
                            const data = response.data;
                            const Result = data.Result;
                            const Value = data.Value;
                            if (Result === true) {
                                const win = "custom-modal-2";
                                ModalService.Close(win);
                                $scope.RefreshMSE();
                            } else {
                                if (Array.isArray(Value))
                                    $scope.expERR = Value;
                                else
                                    alert(Value);
                            }
                        },
                        function (response) {
                            alert(`Ошибка запроса:${response.status}:${response.statusText}`);
                        })
                    .finally(function () { });;
            }
        }
        $scope.DeleteExpertize = function (EXPERTIZE_ID) {
            
            $http.post(`DeleteExpertize`, EXPERTIZE_ID)
                .then(function (response) {
                        
                        const data = response.data;
                        const Result = data.Result;
                        const Value = data.Value;
                        if (Result === true) {
                            $scope.RefreshMSE();
                        } else {
                            alert(Value);
                        }
                    },
                    function (response) {
                        alert(`Ошибка запроса:${response.status}:${response.statusText}`);
                    })
                .finally(function () { });;
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

myApp.filter("S_TIP_NAME", function () {
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

myApp.filter("V002Actual", function () {
    return function (input, DT) {
        if (Array.isArray(input)) {
            return input.filter(val => val.DATEBEG <= DT && (val.DATEEND >= DT || val.DATEEND === null));
        }
        return null;
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
myApp.filter("MKBActual", function () {
    return function (input, DT) {
        if (Array.isArray(input)) {
            return input.filter(val => val.DATE_B <= DT && (val.DATE_E >= DT || val.DATE_E === null));
        }
        return null;
    };
});

myApp.filter("MKBActual2", function () {
    return function (input, DT, term, currMKB) {
        var find ;
        if (term === undefined || term === null || term === "") {

            find = null;
        } else {
            find = term.toLowerCase();
        }
        if (Array.isArray(input)) {
            return input.filter(val => {
                var isActive = val.DATE_B <= DT && (val.DATE_E >= DT || val.DATE_E === null);
                var isFind = false;
                if (find != null) {
                    isFind = (val.MKB.toLowerCase().indexOf(find) > -1 || val.NAME.toLowerCase().indexOf(find) > -1);
                }
                var isCurr = val.MKB === currMKB;
                return isActive && (isFind || isCurr);
            });

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

myApp.directive('ngConfirmClick', [
    function () {
        return {
            link: function (scope, element, attr) {
                var msg = attr.ngConfirmClick || "Are you sure?";
                var clickAction = attr.confirmedClick;
                element.bind('click', function (event) {
                    if (window.confirm(msg)) {
                        scope.$eval(clickAction)
                    }
                });
            }
        };
    }]);