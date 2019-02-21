(function (angular) {
    'use strict';
    angular.module('app').component('lista', {
        templateUrl: '/scripts/site/cuentacorriente/lista.html',
        bindings: {},
        controller: function ($scope, $http, $uibModal) {
            var ctrl = this;
            checkSessionStorage();
            $http({
                method: 'GET',
                url: '/CuentaCorriente/GetAccesoPlus'
            }).then(function successCallback(response) {
                if (response.data.toString() != 0) {
                    var responseArray = response.data.toString().split(";");
                    $scope.acceso = responseArray[0];
                    $scope.FacturaSinOP = responseArray[1];
                    $scope.ProveedorTangoDefaultId = responseArray[2];
                    if ($scope.acceso == 3) {
                        $scope.GetProveedores();
                        $("#gridCuentas").jqxGrid('hidecolumn', ['nueva']);
                    }
                }
            }, function errorCallback(response) {
                console.log(response);
            });
            $scope.data = {};
            $scope.filter = {};
            $scope.GetProveedores = function () {
                checkSessionStorage();
                $http({
                    method: 'GET',
                    url: '/CuentaCorriente/GetProveedores'
                }).then(function successCallback(response) {
                    
                    ctrl.proveedoresctrl = response.data;
                    $scope.data.proveedores = response.data;
                    if ($scope.data.proveedores.length > 0) {
                        $scope.filter.UsuarioId = $scope.data.proveedores[0].UsuarioId;
                    }
                }, function errorCallback(response) {
                    
                    console.log(response);
                });
            }
            
            ctrl.url = '/CuentaCorriente/getcuentacorriente';

            ctrl.cellsrenderer = function (row, columnfield, value, defaulthtml, columnproperties) {
                if (value) {
                    if (columnfield == 'NroComprobante') {
                        var tieneHijos = value.indexOf('...') > 0;
                        if (tieneHijos)
                            return '<div class="jqx-grid-cell-middle-align" style="margin-top: 6px;text-align:left; margin-left:7px;cursor: pointer;" title="Haga click para ver todos los comprobantes">' + value.replace('...', '<span style="text-align: center;margin-left:5px;" class="fa fa-plus-circle verde icon-right-margin big-icon">') + '</div>';
                        else
                            return '<div class="jqx-grid-cell-middle-align" style="margin-top: 6px;text-align:left; margin-left:7px">' + value + '</div>';
                    } else if (columnfield == "Novedad") {
                        if (value)
                            return '<div style="cursor: pointer;" title="Orden de Pago generada recientemente sin facturar"><span style="width: 100%;line-height:33px;text-align: center;" class="glyphicon glyphicon-exclamation-sign rojo icon-right-margin big-icon"></span></div>'
                    }
                    else {
                        var icon = (columnfield == 'Facturado') ? 'fa fa-check-circle-o' : 'glyphicon glyphicon-download';
                        var color = (columnfield == 'Arba') ? 'amarillo' : (columnfield == 'Agip') ? 'rojo' : (columnfield == 'Ganancias') ? 'verde' : (columnfield == 'CajaPrevisional') ? 'naranja' : (columnfield == 'Iva') ? 'azul' : 'verde';
                        return '<div style="' + (columnfield == 'Facturado' ? '' : 'cursor: pointer;') + '"><span style="width: 100%;line-height:33px;text-align: center;" class="' + icon + ' ' + color + ' icon-right-margin big-icon"></span></div>';
                    }
                }
                else {
                    if (columnfield == 'nueva')
                        return '<div class="jqx-grid-cell-middle-align" style="margin-top: 6px;text-align:center; cursor: pointer;" title="Haga click para agregar comprobantes a la OP"><span style="text-align: center;margin-left:5px;" class="glyphicon glyphicon-upload verde icon-right-margin big-icon"></div>';
                    else
                        return '<span style="margin-left:34%;line-height:33px"></span>';
                }
            }

            ctrl.uploadcellsrenderer = function (row, columnfield, value, defaulthtml, columnproperties) {
                return '<div style="cursor: pointer;"><span style="margin-left:34%;line-height:33px;" class="glyphicon glyphicon-upload verde icon-right-margin big-icon"></span></div>';
            }

            ctrl.columnrenderer = function (value) {
                var toolTip = '';
                var titulo = '';
                if (value.indexOf('|') > 0) {
                    titulo = value.substr(0, value.indexOf('|'))
                    toolTip = value.substr(value.indexOf('|') + 1)
                }
                else {
                    titulo = value;
                    toolTip = value;
                }

                return '<div style="text-align: center; margin-top: 9px;" title="' + toolTip + '" >' + titulo + '</div>';
            }

            ctrl.settings =
            {
                altrows: true,
                width: '100%',
                height: 370,
                source: [],
                pageable: true,
                columns: [
                    { dataField: 'ID', hidden: true },
                    { text: 'Nov', dataField: 'Novedad', width: '4%', cellsrenderer: ctrl.cellsrenderer, renderer: ctrl.columnrenderer },
                    { text: 'Fecha', dataField: 'FormatedFecPago', width: '8%', cellsalign: 'center', renderer: ctrl.columnrenderer },
                    { text: 'OP', dataField: 'NroOrdenPago', width: '10%', cellsalign: 'center', renderer: ctrl.columnrenderer },
                    { text: 'Importe OP', dataField: 'ImporteOP', width: '9%', cellsformat: 'c2', cellsalign: 'right', renderer: ctrl.columnrenderer },
                    { text: 'Tipo', dataField: 'TipoComprobante', width: '4%', cellsalign: 'center', renderer: ctrl.columnrenderer },
                    { text: 'Nro Comprobante', dataField: 'NroComprobante', width: '13%', cellsrenderer: ctrl.cellsrenderer, cellsalign: 'left', renderer: ctrl.columnrenderer },
                    { text: 'Referencias', dataField: 'Referencias', cellsalign: 'center', renderer: ctrl.columnrenderer },
                    { text: 'Arba', dataField: 'Arba', width: '8%', cellsrenderer: ctrl.cellsrenderer, renderer: ctrl.columnrenderer },
                    { text: 'Agip', dataField: 'Agip', width: '8%', cellsrenderer: ctrl.cellsrenderer, renderer: ctrl.columnrenderer },
                    { text: 'Ganancias', dataField: 'Ganancias', width: '8%', cellsrenderer: ctrl.cellsrenderer, renderer: ctrl.columnrenderer },
                    { text: 'Caja', dataField: 'CajaPrevisional', width: '8%', cellsrenderer: ctrl.cellsrenderer, renderer: ctrl.columnrenderer },
                    { text: 'Iva', dataField: 'Iva', width: '8%', cellsrenderer: ctrl.cellsrenderer, renderer: ctrl.columnrenderer },
                    { text: '', dataField: 'nueva', width: '4%', cellsrenderer: ctrl.cellsrenderer, renderer: ctrl.columnrenderer }
                    ],
                cellclick: function (event) {
                    var args = event.args;
                    //var columnindex = args.columnindex;
                    var datafield = args.datafield;
                    //var value = args.value;
                    if (datafield == 'NroComprobante')
                        ctrl.MostrarListaHijos(args.row.bounddata);
                    if (datafield == 'Arba' ||datafield == 'Agip' ||datafield == 'Ganancias' ||datafield == 'CajaPrevisional' ||datafield == 'Iva')
                        ctrl.generarCertificado(args.row.bounddata, args.datafield);
                    if (datafield == 'nueva' && $scope.acceso != 3)
                        ctrl.nueva(args.row.bounddata);
                }
            }

            ctrl.generarCertificado = function (value, tipoCertificado) {
                checkSessionStorage();
                window.open('/CuentaCorriente/GetCertificados?op=' + value.NroOrdenPago + '&tipoComprobante=' + tipoCertificado.toUpperCase(), '_black');
            }

            ctrl.nueva = function (value) {
                
                if (value == undefined){
                    value = null;
                    value = new Object();
                    value.title = 'Nueva Factura a cuenta';
                    value.ProveedorTangoId = $scope.ProveedorTangoDefaultId;
                }
                else {
                    value.title = 'Nueva Factura para la Orden de Pago Nro';
                }
                    
                var modalInstance = $uibModal.open({
                    component: 'nueva',
                    windowClass: 'app-modal-window',
                    backdrop: 'static',
                    keyboard: false,
                    resolve: {
                        model: value
                    }
                });
            }

            ctrl.MostrarListaHijos = function (value) {
                if (value.NroComprobante.indexOf('...') > 0) {
                    var modalInstance = $uibModal.open({
                        component: 'listaHijos',
                        windowClass: 'app-modal-window50',
                        backdrop: 'static',
                        keyboard: false,
                        resolve: {
                            model: value
                        }
                    });
                }
            }

            ctrl.GetData = function () { getData(); }
            function getData() {
                $http.get(ctrl.url + '?usr_id=' + $scope.filter.UsuarioId)
                    .then(function (response) {
                        ctrl.data = response.data;
                        var data = {
                            localdata: response.data,
                            datafields:
                            [
                                { name: 'ID', type: 'string' },
                                { name: 'Novedad', tyoe: 'bool' },
                                { name: 'FecPago', type: 'string' },
                                { name: 'FormatedFecPago', type: 'string' },
                                { name: 'NroOrdenPago', type: 'string' },
                                { name: 'TipoComprobante', type: 'string' },
                                { name: 'NroComprobante', type: 'string' },
                                { name: 'ImporteOP', type: 'float' },
                                { name: 'Arba', type: 'bool' },
                                { name: 'Agip', type: 'bool' },
                                { name: 'Ganancias', type: 'bool' },
                                { name: 'Iva', type: 'bool' },
                                { name: 'CajaPrevisional', type: 'bool' },
                                { name: 'CuentaCorrientes', type: 'array' },
                                { name: 'ProveedorTangoId', type: 'string' },
                                { name: 'Referencias', type: 'string' }
                        ],
                            id: 'ID',
                            datatype: "json"
                        };
                        var dataAdapter = new $.jqx.dataAdapter(data);
                        ctrl.settings.source = dataAdapter;
                        ctrl.facturasAdeudadas = 0;
                        for (var i = 0; i < response.data.length; i++) {
                            if (!response.data[i].NroComprobante || response.data[i].NroComprobante == '')
                                ctrl.facturasAdeudadas++;
                        }
                    }, function (error) {
                        ctrl.data = [];
                    });
            }

            this.$onInit = function () {
                getData();
            };
        },
    });
})(window.angular);
