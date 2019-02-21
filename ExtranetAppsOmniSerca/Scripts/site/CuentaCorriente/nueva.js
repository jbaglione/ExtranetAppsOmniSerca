(function (angular) {
    'use strict';
    //'ngPatternRestrict''ui.bootstrap'
    angular.module('app').component('nueva', {
        templateUrl: '/scripts/site/cuentacorriente/nueva.html',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        controller: function ($http, $scope) {
            var ctrl = this;
            ctrl.model = {};
            ctrl.url = '/CuentaCorriente';
            ctrl.enviar = function () {
                checkSessionStorage();
                if (!isNaN(ctrl.model.sucursal) || ctrl.model.sucursal.lenght < 5 || ctrl.model.sucursal.substr(1, 5) < 1) {
                    Messenger().post({
                        message: "Primera sección del Nro. de comprobante inválida. Debe especificar el tipo de factura (A / B / C) y el Nro (ej.: A0001)",
                        type: 'error',
                        showCloseButton: true
                    });
                    return;
                }
                else if (isNaN(ctrl.model.numero) ||ctrl.model.numero.lenght < 8 || ctrl.model.numero.substr(1, 8) < 1) {
                    Messenger().post({
                        message: "Segunda sección del Nro. de comprobante inválida (ej.: 00000001)",
                        type: 'error',
                        showCloseButton: true
                    });
                    return;
                }
                if (!ctrl.model.pdf || ctrl.model.pdf=='') {
                    Messenger().post({
                        message: "El archivo pdf es obligatorio",
                        type: 'error',
                        showCloseButton: true
                    });
                    return;
                }
                $http.post(ctrl.url + '/guardarV2', ctrl.model, ctrl.model.ProveedorTangoId)
                    .then(function (response) {
                        ctrl.close({});
                    }, function (error) {
                    });
/*
                $http.post(ctrl.url + '/guardar', ctrl.model)
                    .then(function (response) {
                        ctrl.close({});
                    }, function (error) {
                    });
*/
            };
            ctrl.cerrar = function () {
                checkSessionStorage();
                ctrl.close({});
            };
            this.$onInit = function () {
                //$('#txtFechaEmision').datepicker();
                $('#txtFechaEmision').datepicker({
                    format:'dd/mm/yyyy'
                });
                $('#ui-datepicker-div').css('background-color', 'white');
                $.datepicker.regional['es'] = {
                    closeText: 'Cerrar',
                    prevText: '< Ant',
                    nextText: 'Sig >',
                    currentText: 'Hoy',
                    monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                    monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                    dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
                    dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
                    dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
                    weekHeader: 'Sm',
                    dateFormat: 'dd/mm/yy',
                    firstDay: 1,
                    isRTL: false,
                    showMonthAfterYear: false,
                    yearSuffix: ''
                };
                $.datepicker.setDefaults($.datepicker.regional['es']);
                $(function () {
                    $("#fecha").datepicker();
                });


                ctrl.model.tipo = 'Factura';
                ctrl.model.monto = ctrl.resolve.model.Importe;
                ctrl.model.periodo = ctrl.resolve.model.FormatedPeriodo;
                ctrl.model.id = ctrl.resolve.model.ID;
                ctrl.model.NroOP = ctrl.resolve.model.NroOrdenPago;
                ctrl.model.ProveedorTangoId = ctrl.resolve.model.ProveedorTangoId;
                ctrl.pdfSettings = {
                    uploadUrl: '/CuentaCorriente/uploadPdf?id='+ ctrl.model.ProveedorTangoId,
                    autoUpload: true,
                    fileInputName: 'file',
                    uploadEnd: function (event) {
                        var args = event.args;
                        ctrl.model.pdf = args.file;
                    }
                }

                ctrl.cabeceraSettings = {
                    uploadUrl: '/CuentaCorriente/uploadCabecera?id=' + ctrl.model.ProveedorTangoId,
                    autoUpload: true,
                    fileInputName: 'file',
                    uploadEnd: function (event) {
                        var args = event.args;
                        ctrl.model.cabecera = args.file;
                    }
                }

                ctrl.detalleSettings = {
                    uploadUrl: '/CuentaCorriente/uploadDetalle?id=' + ctrl.model.ProveedorTangoId,
                    autoUpload: true,
                    fileInputName: 'file',
                    uploadEnd: function (event) {
                        var args = event.args;
                        ctrl.model.detalle = args.file;
                    }
                }

                ctrl.ventaSettings = {
                    uploadUrl: '/CuentaCorriente/uploadVenta?id=' + ctrl.model.ProveedorTangoId,
                    autoUpload: true,
                    fileInputName: 'file',
                    uploadEnd: function (event) {
                        var args = event.args;
                        ctrl.model.venta = args.file;
                    }
                }
            };
        }
    });
})(window.angular);
