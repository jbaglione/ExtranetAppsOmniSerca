(function (angular) {
    'use strict';
    angular.module('app').component('listaHijos', {
        templateUrl: '/scripts/site/cuentacorriente/listaHijos.html',
        bindings: {
            resolve: '<',
            close: '&'
        },
        controller: function ($http, $uibModal) {
            var ctrl = this;
            ctrl.model = {};

/*
            ctrl.generarCertificado = function (value, tipoCertificado) {
                window.open('/CuentaCorriente/GetCertificados?op=' + value.NroOrdenPago + '&tipoComprobante=' + tipoCertificado.toUpperCase(), '_black');
            }
*/

            this.$onInit = function () {
                ctrl.model = ctrl.resolve.model
            };
        },
    });
})(window.angular);
