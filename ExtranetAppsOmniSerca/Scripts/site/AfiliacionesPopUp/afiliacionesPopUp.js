(function (window) {
    'use strict';
    angular.module("ngLocale", [], ["$provide", function ($provide) {
        $provide.value("$locale", {
            "DATETIME_FORMATS": {
                "AMPMS": [
                  "a.m.",
                  "p.m."
                ],
                "DAY": [
                  "domingo",
                  "lunes",
                  "martes",
                  "mi\u00e9rcoles",
                  "jueves",
                  "viernes",
                  "s\u00e1bado"
                ],
                "ERANAMES": [
                  "Antes de Cristo",
                  "Despues de Cristo"
                ],
                "ERAS": [
                  "AC",
                  "DC"
                ],
                "FIRSTDAYOFWEEK": 6,
                "MONTH": [
                  "enero",
                  "febrero",
                  "marzo",
                  "abril",
                  "mayo",
                  "junio",
                  "julio",
                  "agosto",
                  "septiembre",
                  "octubre",
                  "noviembre",
                  "diciembre"
                ],
                "SHORTDAY": [
                  "dom",
                  "lun",
                  "mar",
                  "mi\u00e9",
                  "jue",
                  "vie",
                  "s\u00e1b"
                ],
                "SHORTMONTH": [
                  "ene",
                  "feb",
                  "mar",
                  "abr",
                  "may",
                  "jun",
                  "jul",
                  "ago",
                  "sep",
                  "oct",
                  "nov",
                  "dic"
                ],
                "STANDALONEMONTH": [
                    "Enero",
                    "Febrero",
                    "Marzo",
                    "Abril",
                    "Mayo",
                    "Junio",
                    "Julio",
                    "Agosto",
                    "Septiembre",
                    "Octubre",
                    "Noviembre",
                    "Diciembre"
                ],
                "WEEKENDRANGE": [
                  5,
                  6
                ],
                "fullDate": "EEEE, d 'de' MMMM 'de' y",
                "longDate": "d 'de' MMMM 'de' y",
                "medium": "dd/MM/yyyy HH:mm:ss",
                "mediumDate": "dd/MM/yyyy",
                "mediumTime": "HH:mm:ss",
                "short": "dd/MM/yy HH:mm",
                "shortDate": "dd/MM/yy",
                "shortTime": "HH:mm"
            }

        });
    }]);

    angular.module('signature', []);


})(window);

app.controller('afiliacionesPopUpCtrl', function ($scope, $http, $window) {

    _this = this;

    //TODO: extraer
    //checkSessionStorage();

    $scope.boundingBox = {
        width: 700,
        height: 300
    };

    $scope.filter = {
        tipoCliente: 0,
        razonSocial: '',
    };

    

    //GetAccesoDesc
    $http({
        method: 'GET',
        url: '/AfiliacionesPopUp/GetAccesoDesc'
    }).then(function successCallback(response) {
        $scope.acceso = response.data.toString();
        if ($scope.acceso == "SinAcceso" ||
            sessionStorage.currentUser == undefined || sessionStorage.currentUser == 'undefined') {
            $scope.mostrarError('Error', 'Sesion Finalizada');
            //$scope.limpiarCredenciales();
            sessionStorage.currentUser = undefined;
            setTimeout(function () {
                window.location.href = '/login';
            }, 2000);
        }
        if ($scope.acceso == "Administrador") {
            $scope.GetVendedores();
        }
        if ($scope.data.tipoClientes.length > 0) {
            $scope.filter.tipoCliente = $scope.acceso == "Administrador" ? $scope.data.tipoClientes[2].ID : $scope.data.tipoClientes[1].ID;
        }
    }, function errorCallback(response) {
        console.log(response);
    });

    $scope.GetVendedores = function () {
        //TODO: Extraer.
        return;
        //checkSessionStorage();
        //$http({
        //    method: 'GET',
        //    url: '/AfiliacionesPopUp/GetVendedores'
        //}).then(function successCallback(response) {
        //    $scope.data.vendedores = response.data;

        //    if ($scope.data.vendedores.length > 0) {
        //        $scope.filter.UsuarioId = $scope.data.vendedores[0].UsuarioId;
        //        $scope.GetData();
        //    }
        //}, function errorCallback(response) {
        //    console.log(response);
        //});
    }

    $scope.mostrarError = function (title, errorMessage) {
        $.Notify({
            caption: title,
            content: errorMessage,
            type: 'alert',
            icon: "<span class='mif-cross'></span>"
        })
    };

    $scope.mostrarErrorLong = function (title, errorMessage) {
        $.Notify({
            caption: title,
            content: errorMessage,
            timeout: 5000,
            type: 'alert',
            icon: "<span class='mif-cross'></span>"
        })
    };

    $scope.mostrarSuccess = function (title, message) {
        $.Notify({
            caption: title,
            content: message,
            type: 'success',
            icon: "<span class='mif-checkmark'></span>"
        })
    };

    $scope.data = {
        tipoClientes: [],
    };

    $scope.data.constantesFacturacion = {
        debitoAutomatico: 'DEBITO AUTOMATICO',
        pagoMisCuentas: 'PAGO MIS CUENTAS',
        tarjetaDeCredito: 'TARJETA DE CREDITO',
    }

    $scope.setCoordinatesClienteColor = function (color) {
        angular.element(document.querySelector('#lblMapaClienteLat')).css('color', color);
        angular.element(document.querySelector('#lblMapaClienteLong')).css('color', color);

        if ($scope.data.solicitudDigitalizada.AreaProtegida.FlgDomicilioLegalCobertura)
            $scope.setCoordinatesAreaColor(color)
    };
    $scope.setCoordinatesAreaColor = function (color) {
        angular.element(document.querySelector('#lblMapaAreaLat')).css('color', color);
        angular.element(document.querySelector('#lblMapaAreaLong')).css('color', color);
    };
    //$scope.bloquear.Firma = true;
    //$scope.data.tipoClientes = [
    //    {
    //        Descripcion: 'Todos',
    //        ID: '0'
    //    },
    //    {
    //        Descripcion: 'Activos',
    //        ID: '1'
    //    },
    //    {
    //        Descripcion: 'Potenciales',
    //        ID: '2'
    //    },
    //    {
    //        Descripcion: 'Inactivos',
    //        ID: '3'
    //    }
    //];
    //Todos = 0, potencial = 1, preparado = 2, activo = 3, inactivo = 4
    const clientePotencial = 1;
    const clientePreparado = 2;
    const clienteActivo = 3;
    const clienteInactivo = 4;
    $scope.data.tipoClientes =
        [
            { Descripcion: 'Todos', ID: '0' },
            { Descripcion: 'Potenciales', ID: '1' },
            { Descripcion: 'Preparados', ID: '2' },
            { Descripcion: 'Activos', ID: '3' },
            { Descripcion: 'Inactivos', ID: '4' }
        ];

    $scope.setEsPreparado = function (acceptFirm) {
        $scope.data.solicitudDigitalizada.EntidadCliente.Estado = document.getElementById('chkEsPreparado').checked ? clientePreparado : clientePotencial;

        if ($scope.data.solicitudDigitalizada.EntidadCliente.Estado == clientePreparado) {
            var errores = "";
            if ($scope.ValidarGeo()) {
                if ($scope.data.solicitudDigitalizada.EntidadCliente.Rubro == null ||
                    !StringHasValue($scope.data.solicitudDigitalizada.EntidadCliente.Rubro.ID))
                    errores += "Cliente - Rubro, ";
                if ($scope.data.solicitudDigitalizada.EntidadCliente.CondicionIVA == null ||
                    !StringHasValue($scope.data.solicitudDigitalizada.EntidadCliente.CondicionIVA.ID))
                    errores += "Cliente - Condicion ante el IVA, ";

                if (!StringHasValue($scope.data.solicitudDigitalizada.EntidadCliente.Cuit))
                    errores += "Cliente - CUIT, ";
                else if (!$scope.validarCuitDigitoVerificador($scope.data.solicitudDigitalizada.EntidadCliente.Cuit))
                    errores += "Cliente - CUIT (no es un CUIT real), ";
                //else if (!$scope.validarCuitServicio($scope.data.solicitudDigitalizada.EntidadCliente.Cuit, false))
                //    errores += "Cliente - CUIT (es invalido), ";

                //StringHasValue(x)
                if ($scope.data.solicitudDigitalizada.DatosAdministrativos.ImporteMensual == null ||
                    $scope.data.solicitudDigitalizada.DatosAdministrativos.ImporteMensual <= 0) {
                    errores += "Al menos un producto con valor mayor a cero, ";
                }
                //TODO: Corregir funcionamiento de la firma
                if (acceptFirm)
                    $scope.firma = $scope.accept();
                $scope.data.solicitudDigitalizada.DatosAdministrativos.Firmado = !$scope.firma.isEmpty && StringHasValue($scope.firma.dataUrl);
                if (!$scope.data.solicitudDigitalizada.DatosAdministrativos.Firmado) {
                    errores += "Firma, ";
                }
                if (errores.length > 0) {
                    $scope.mostrarErrorLong("Datos Incompletos", "Los siguiente campos son obligatorios para marcar como preparado: " + errores.slice(0, -2));
                    $scope.data.solicitudDigitalizada.EntidadCliente.Estado = clientePotencial;
                }
                else {
                    $scope.data.solicitudDigitalizada.EntidadCliente.Estado = clientePreparado;
                }
            }
        }

        document.getElementById('chkEsPreparado').checked = $scope.data.solicitudDigitalizada.EntidadCliente.Estado == clientePreparado;
    }

    $scope.data.clientes = {};
    $scope.data.productos = {};
    $scope.data.informacionVisual = {};

    $scope.data.bloquear = {};
    $scope.data.habilitar = {};




    

    $scope.filter.tipoCliente = '';
    $scope.filter.descripcion = '';

    $scope.copyAdress = function () {
        if ($scope.data.solicitudDigitalizada.AreaProtegida.FlgDomicilioLegalCobertura) {
            //$scope.data.solicitudDigitalizada.AreaProtegida = angular.copy($scope.data.solicitudDigitalizada.EntidadCliente);
            $scope.data.solicitudDigitalizada.AreaProtegida.Calle = $scope.data.solicitudDigitalizada.EntidadCliente.Calle
            $scope.data.solicitudDigitalizada.AreaProtegida.Altura = $scope.data.solicitudDigitalizada.EntidadCliente.Altura;
            $scope.data.solicitudDigitalizada.AreaProtegida.Localidad = $scope.data.solicitudDigitalizada.EntidadCliente.Localidad;
            $scope.data.solicitudDigitalizada.AreaProtegida.Provincia = $scope.data.solicitudDigitalizada.EntidadCliente.Provincia;
            $scope.data.solicitudDigitalizada.AreaProtegida.Pais = $scope.data.solicitudDigitalizada.EntidadCliente.Pais;
            $scope.data.solicitudDigitalizada.AreaProtegida.Longitud = $scope.data.solicitudDigitalizada.EntidadCliente.Longitud;
            $scope.data.solicitudDigitalizada.AreaProtegida.Latitud = $scope.data.solicitudDigitalizada.EntidadCliente.Latitud;
            $scope.data.solicitudDigitalizada.AreaProtegida.CP = $scope.data.solicitudDigitalizada.EntidadCliente.CP;
            $scope.data.solicitudDigitalizada.AreaProtegida.Piso = $scope.data.solicitudDigitalizada.EntidadCliente.Piso;
            $scope.data.solicitudDigitalizada.AreaProtegida.Depto = $scope.data.solicitudDigitalizada.EntidadCliente.Depto;
            $scope.data.solicitudDigitalizada.AreaProtegida.Referencia = $scope.data.solicitudDigitalizada.EntidadCliente.Referencia;
            $scope.data.solicitudDigitalizada.AreaProtegida.EntreCalle1 = $scope.data.solicitudDigitalizada.EntidadCliente.EntreCalle1;
            $scope.data.solicitudDigitalizada.AreaProtegida.EntreCalle2 = $scope.data.solicitudDigitalizada.EntidadCliente.EntreCalle2;

            $scope.setCoordinatesAreaColor('black');
        }
    };

    $scope.crAfiliar = function (row, columnfield, value, defaulthtml, columnproperties) {
        var icon = '';
        switch (parseInt(value)) {
            case 1: // --> potencial
                icon = 'glyphicon-exclamation-sign amarillo';
                break;
            case 2: // --> Preparado
                icon = 'glyphicon-ok-circle azul';
                break;
            case 3: // --> activo
                icon = 'glyphicon-ok-circle verde';
                break;
            case 4: // --> inactivo
                icon = 'glyphicon-remove-circle rojo';
                break;
        }
        return '<div style="cursor: pointer;"><span style="margin-left:34%;line-height:33px;" class="glyphicon ' + icon + ' icon-right-margin big-icon"></span></div>'
    }

    $scope.crRubro = function (row, columnfield, value, defaulthtml, columnproperties) {
        return '<div class="jqx-grid-cell-left-align" style="margin-top: 6px;" id="' + value.ID + '">' + value.Descripcion + '</div>'
    }

    $scope.crCondicionIVA = function (row, columnfield, value, defaulthtml, columnproperties) {
        return '<div class="jqx-grid-cell-left-align" style="margin-top: 6px;" id="' + value.ID + '">' + value.Descripcion + '</div>'
    }

    $scope.crLocalidad = function (row, columnfield, value, defaulthtml, columnproperties) {
        return '<div class="jqx-grid-cell-left-align" style="margin-top: 6px;" id="' + value.value + '">' + value.label + '</div>'
    }

    $scope.crContrato = function (row, columnfield, value, defaulthtml, columnproperties, rowData) {
        return rowData.Estado != 3 ? defaulthtml : '<div style="cursor: pointer;"><span style="margin-left:34%;line-height:33px;" class="glyphicon glyphicon-download  verde icon-right-margin big-icon"></span></div>';
    }

    $scope.GetData = function () {
        //TODO: Extraer.
        return;

        //checkSessionStorage();
        //var usId = 0;
        //if ($scope.filter.UsuarioId != null)
        //    usId = $scope.filter.UsuarioId;
        //$http({
        //    method: 'GET',
        //    url: '/AfiliacionesPopUp/GetClientesSegunEstado?pTipoCliente=' + $scope.filter.tipoCliente + '&pDescripcion=' + $scope.filter.descripcion + '&pUsuarioId=' + usId,
        //}).then(function successCallback(response) {

        //    var data = {
        //        localdata: response.data,
        //        datafields: [
        //            { name: 'ClienteId', type: 'long' },
        //            { name: 'NombreComercial', type: 'string' },
        //            { name: 'Rubro', type: 'string' },
        //            { name: 'RazonSocial', type: 'string' },
        //            { name: 'Cuit', type: 'long' },
        //            { name: 'Domicilio', type: 'string' },
        //            { name: 'Localidad', type: 'object' },
        //            { name: 'Estado', type: 'int' },
        //            { name: 'CredencialID', type: 'int' }
        //        ],
        //        datatype: "json"
        //    }
        //    $scope.data.clientes.settings.source = new $.jqx.dataAdapter(data);

        //}, function errorCallback(response) {

        //    console.log(response);
        //});

        //$scope.$watch('');
    }


    $scope.data.clientes.settings =
    {
        altrows: true,
        width: '100%',
        height: 365,
        source: [],
        pageable: true,
        columns: [
            { dataField: 'ClienteId', hidden: true },
            { text: 'Nombre Comercial', dataField: 'NombreComercial', width: '17%', minwidth: '149px', cellsalign: 'left', renderer: $scope.columnrenderer },
            { text: 'Rubro', dataField: 'Rubro', width: '11%', minwidth: '98px', cellsalign: 'left', renderer: $scope.columnrenderer, cellsrenderer: $scope.crRubro },
            { text: 'Razon Social', dataField: 'RazonSocial', width: '17%', minwidth: '149px', cellsalign: 'left', renderer: $scope.columnrenderer },
            { text: 'Cuit', dataField: 'Cuit', width: '9%', minwidth: '94px', cellsalign: 'left', renderer: $scope.columnrenderer },
            { text: 'Domicilio', dataField: 'Domicilio', width: '22%', minwidth: '170px', cellsalign: 'left', renderer: $scope.columnrenderer },
            { text: 'Localidad', dataField: 'Localidad', width: '10%', minwidth: '98px', cellsalign: 'center', renderer: $scope.columnrenderer, cellsrenderer: $scope.crLocalidad },
            { text: 'Nro de Cliente', dataField: 'CredencialID', width: '9%', minwidth: '84px', cellsalign: 'right', renderer: $scope.columnrenderer },
            { text: 'Estado', dataField: 'Estado', width: '5%*', minwidth: '45px', cellsrenderer: $scope.crAfiliar, renderer: $scope.columnrenderer },
            { text: 'Contrato', dataField: '', width: '5%*', minwidth: '45px', cellsrenderer: $scope.crContrato, renderer: $scope.columnrenderer, hidden: false }//$scope.acceso != "Administrador"
        ],

        cellclick: function (event) {
            var args = event.args;
            var columnIndex = args.columnindex;
            var value = args.value;
            if (columnIndex == 8) {
                $scope.GetSolicitudDigitalizada(args.row.bounddata.ClienteId);
            }
            //if (columnIndex == 9 && $scope.acceso == "Administrador" && args.row.bounddata.Estado == 3) {
            //    $scope.GetContrato(args.row.bounddata.ClienteId);
            //}
        }
    }

    //TODO: Sin uso, por indefinicion de precio unitario
    $scope.CalularFinalProducto = function (producto) {
        var valorReal = (producto.PrecioLista * producto.Cantidad);
        var descuento = (valorReal * producto.Bonificacion) / 100;
        producto.Final = valorReal - descuento;
    }

    $scope.ActualizarFinal = function (producto) {
        var impTotal = 0;
        $scope.data.solicitudDigitalizada.Productos.forEach(function (productos) {
            impTotal += parseFloat(productos.Final);
        });
        $scope.data.solicitudDigitalizada.DatosAdministrativos.ImporteMensual = impTotal;
    }
    //$("#productosGrid").jqxGrid('begincelledit', 0, 'Bonificacion');
    $scope.GetContrato = function () {
        //TODO: extraer
        //checkSessionStorage();
        //if (columnIndex == 9 && $scope.acceso == "Administrador" && args.row.bounddata.Estado == 3) {
            window.open('/AfiliacionesPopUp/GetContrato?pClienteId=' + $scope.GetDirectClienteId, '_black');
        //}
    }
    
    $scope.GetSolicitudDigitalizada = function (clienteId) {
        debugger;
        //TODO: extraer
        //checkSessionStorage();
        $http({
            method: 'GET',
            url: '/AfiliacionesPopUp/GetSolicitudDigitalizada?pClienteId=' + clienteId
        }).then(function successCallback(response) {
            if (response.data.Success == true) {
                $scope.SolicitudDigitalizada(response.data.Solicitud);
            }
            else {
                $scope.mostrarError("Error inesperado", response.data.Message);
            }
        }, function errorCallback(response) {
            console.log(response);
        });
    }

    $scope.SolicitudDigitalizada = function (data) {
        //FIX: Autocomplete not working binding data.
        $("#txtLocalidadCliente").val(null);
        $("#txtLocalidadArea").val(null);

        ////TODO: REVISAR.
        //$scope.clean();//Borra pad firma.

        $scope.data.solicitudDigitalizada = data;
        $scope.ConfigurarCalendario();


        if ($scope.data.solicitudDigitalizada.EntidadCliente == null) {
            $scope.CreateDefaultSolicitudDigitalizada();
            $scope.data.bloquear.CamposGrales = false;
            $scope.data.bloquear.CamposCliente = false;
            $scope.data.bloquear.solicitud = false;
            $scope.data.bloquear.Firma = true;
        }
        else {
            //Solo datos entidad Cliente.
            $scope.SetearCombosCliente();
            $scope.SetearAutocompleteCliente();
            $scope.data.bloquear.CamposGrales = false;
            $scope.data.bloquear.CamposCliente = false;//True
            $scope.data.bloquear.Firma = false;

            //Completar datos Extras.
            //if ($scope.data.solicitudDigitalizada.EntidadCliente.Estado == clientePotencial) {
            //$scope.SetearCombosDatosAdministrativos();
            debugger;
            $scope.SetearAutocompleteAreaProtegida();
            //$scope.data.bloquear.CamposGrales = true;
            //$scope.data.bloquear.CamposCliente = true;

            $scope.dt = new Date($scope.data.solicitudDigitalizada.SolicitudDigitalizadaBasico.FechaIngreso.match(/\d+/)[0] * 1);
            //}
            //else
            //    $scope.CreateDefaultSolicitudDigitalizada();

            $scope.data.habilitar.afiliar = $scope.data.solicitudDigitalizada.EntidadCliente.Estado == clientePreparado && $scope.acceso == "Administrador";
            $scope.data.bloquear.solicitud = $scope.data.solicitudDigitalizada.EntidadCliente.Estado == clienteActivo || $scope.data.solicitudDigitalizada.EntidadCliente.Estado == clienteInactivo;

            //FIX
            $scope.CargarFirma();
            document.getElementById('chkEsPreparado').checked = $scope.data.solicitudDigitalizada.EntidadCliente.Estado == clientePreparado;
            $scope.setEsPreparado();
        }

        //$scope.SetearCombosCliente();
        $scope.SetearCombosDatosAdministrativos();

        if ($scope.data.solicitudDigitalizada.EntidadCliente.Latitud == null ||
            $scope.data.solicitudDigitalizada.EntidadCliente.Latitud == "" ||
            $scope.data.solicitudDigitalizada.EntidadCliente.Longitud == "" ||
            $scope.data.solicitudDigitalizada.EntidadCliente.Longitud == null) {
            $scope.data.solicitudDigitalizada.EntidadCliente.Latitud = 0;
            $scope.data.solicitudDigitalizada.EntidadCliente.Longitud = 0;
            $scope.setCoordinatesClienteColor('red');
        }

        if ($scope.data.solicitudDigitalizada.AreaProtegida.Latitud == null || $scope.data.solicitudDigitalizada.AreaProtegida.Longitud == null) {
            $scope.data.solicitudDigitalizada.AreaProtegida.Latitud = 0;
            $scope.data.solicitudDigitalizada.AreaProtegida.Longitud = 0;
            $scope.setCoordinatesAreaColor('red');
        }
        //$scope.SetMaskMoneyForInputs();

        $scope.data.informacionVisual.Titulo = 'Solicitud Digitalizada';
        $scope.data.informacionVisual.Descripcion = 'Solicito la incorporación al sistema de cobertura Plan AREA PROTEGIDA, tomando conocimiento de las especificaciones de los servicios comprendidos y aceptando de total conformidad lo indicado al dorso del presente.';

        document.getElementById('chkFlgDomicilioLegalCobertura').checked = $scope.data.solicitudDigitalizada.AreaProtegida.FlgDomicilioLegalCobertura == 1;

        $scope.copyAdress();
        $scope.ActualizarFinal();

        $('#popupRevisar').modal('show');

        //TODO: Fix, si bindeo los dos input a la misma propiedad 'mask' recorta el valor en la carga.
        if ($scope.data.solicitudDigitalizada.formaDePagoSeleccionado.Descripcion === $scope.data.constantesFacturacion.tarjetaDeCredito
            && $scope.data.solicitudDigitalizada.DatosAdministrativos.TarjetaDeCredito.TipoDeTarjeta.MascaraIngreso == '9999-999999-99999')
            $scope.data.solicitudDigitalizada.DatosAdministrativos.TarjetaDeCredito.NumeroAmerican = $scope.data.solicitudDigitalizada.DatosAdministrativos.TarjetaDeCredito.Numero;
    }

    $scope.CreateDefaultSolicitudDigitalizada = function () {

        if ($scope.data.solicitudDigitalizada == null || $scope.data.solicitudDigitalizada == "")
            $scope.data.solicitudDigitalizada = new Object();
        if ($scope.data.solicitudDigitalizada.SolicitudDigitalizadaBasico == null) {
            $scope.data.solicitudDigitalizada.SolicitudDigitalizadaBasico = new Object();
            $scope.dt = new Date();
        }

        if ($scope.data.solicitudDigitalizada.EntidadCliente == null) {

            $scope.data.solicitudDigitalizada.EntidadCliente = new Object();
            $scope.data.solicitudDigitalizada.EntidadCliente.Estado = clientePotencial;
        }
        //if ($scope.data.solicitudDigitalizada.EntidadCliente.Latitud == null)
        //    $scope.data.solicitudDigitalizada.EntidadCliente.Latitud = 0;
        //if ($scope.data.solicitudDigitalizada.EntidadCliente.Longitud == null)
        //    $scope.data.solicitudDigitalizada.EntidadCliente.Longitud = 0;
        if ($scope.data.solicitudDigitalizada.ClienteContacto == null)
            $scope.data.solicitudDigitalizada.ClienteContacto = new Object();
        $scope.data.solicitudDigitalizada.AreaProtegida = new Object();
        $scope.data.solicitudDigitalizada.AreaProtegida.FlgDomicilioLegalCobertura = false;
        document.getElementById('chkEsPreparado').checked = false;
        $scope.data.solicitudDigitalizada.DatosAdministrativos = new Object();
        $scope.data.solicitudDigitalizada.DatosAdministrativos.DebitoEnCuenta = new Object();
        $scope.data.solicitudDigitalizada.DatosAdministrativos.FormaDePago = new Object();
        $scope.data.solicitudDigitalizada.DatosAdministrativos.FormaDePago.ID = 1;
        //$scope.data.solicitudDigitalizada.DatosAdministrativos.DebitoEnCuenta.TipoCuenta = 1;
        $scope.data.solicitudDigitalizada.formaDePagoSeleccionado = new Object();
    }

    

    $scope.ConfigurarCalendario = function () {
        //https://angular-ui.github.io/bootstrap/
        $scope.today = function () {
            $scope.dt = new Date();
        };
        $scope.today();
        $scope.clear = function () {
            $scope.dt = null;
        };
        $scope.inlineOptions = {
            customClass: getDayClass,
            minDate: new Date(),
            showWeeks: true
        };
        $scope.dateOptions = {
            dateDisabled: disabled,
            formatYear: 'yy',
            maxDate: new Date(2020, 5, 22),
            minDate: new Date(),
            startingDay: 1
        };
        // Disable weekend selection
        function disabled(data) {
            var date = data.date,
              mode = data.mode;
            return mode === 'day' && (date.getDay() === 0 || date.getDay() === 6);
        }
        $scope.open1 = function () {
            $scope.popup1.opened = true;
        };
        $scope.setDate = function (year, month, day) {
            $scope.dt = new Date(year, month, day);
        };

        $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];
        $scope.format = $scope.formats[0];
        $scope.altInputFormats = ['M!/d!/yyyy'];
        $scope.popup1 = {
            opened: false
        };
        function getDayClass(data) {
            var date = data.date,
              mode = data.mode;
            if (mode === 'day') {
                var dayToCheck = new Date(date).setHours(0, 0, 0, 0);

                for (var i = 0; i < $scope.events.length; i++) {
                    var currentDay = new Date($scope.events[i].date).setHours(0, 0, 0, 0);
                    if (dayToCheck === currentDay) {
                        return $scope.events[i].status;
                    }
                }
            }
            return '';
        }
    }

    $scope.SetearCombosCliente = function () {

        if ($scope.data.rubros != null)
            for (var i = 0; i < $scope.data.rubros.length; i++) {
                if ($scope.data.solicitudDigitalizada.EntidadCliente.Rubro.ID.toString() == $scope.data.rubros[i].ID.toString())
                    $scope.data.solicitudDigitalizada.rubroSeleccionado = $scope.data.rubros[i];
            }

        if ($scope.data.condicionIVAs != null)
            for (var i = 0; i < $scope.data.condicionIVAs.length; i++) {
                if ($scope.data.solicitudDigitalizada.EntidadCliente.CondicionIVA.ID == $scope.data.condicionIVAs[i].ID)
                    $scope.data.solicitudDigitalizada.condicionIVASeleccionado = $scope.data.condicionIVAs[i];
            }
    }
    $scope.SetearAutocompleteCliente = function () {

        if ($scope.data.localidades != null)
            for (var i = 0; i < $scope.data.localidades.length; i++) {
                if ($scope.data.solicitudDigitalizada.EntidadCliente.Localidad.value == $scope.data.localidades[i].value)
                    $("#txtLocalidadCliente").val($scope.data.localidades[i].label);
            }
    }
    $scope.SetearCombosDatosAdministrativos = function () {

        for (var i = 0; i < $scope.data.formaDePagos.length; i++) {
            if ($scope.data.solicitudDigitalizada.DatosAdministrativos.FormaDePago.ID.toString() == $scope.data.formaDePagos[i].ID.toString())
                $scope.data.solicitudDigitalizada.formaDePagoSeleccionado = $scope.data.formaDePagos[i];
        }

        if ($scope.data.solicitudDigitalizada.formaDePagoSeleccionado.Descripcion == $scope.data.constantesFacturacion.tarjetaDeCredito &&
                $scope.data.solicitudDigitalizada.DatosAdministrativos.TarjetaDeCredito != null) {

            for (var i = 0; i < $scope.data.tipoDeTarjetas.length; i++) {
                if ($scope.data.solicitudDigitalizada.DatosAdministrativos.TarjetaDeCredito.TipoDeTarjeta.ID.toString() == $scope.data.tipoDeTarjetas[i].ID.toString())
                    $scope.data.solicitudDigitalizada.tipoDeTarjetaSeleccionado = $scope.data.tipoDeTarjetas[i];
            }
        }

        if ($scope.data.solicitudDigitalizada.formaDePagoSeleccionado.Descripcion == $scope.data.constantesFacturacion.debitoAutomatico) {
            if ($scope.data.solicitudDigitalizada.DatosAdministrativos.DebitoEnCuenta.Banco != null)
                for (var i = 0; i < $scope.data.bancos.length; i++) {
                    if ($scope.data.solicitudDigitalizada.DatosAdministrativos.DebitoEnCuenta.Banco.ID.toString() == $scope.data.bancos[i].ID.toString())
                        $scope.data.solicitudDigitalizada.bancoSeleccionado = $scope.data.bancos[i];
                }
            if ($scope.data.solicitudDigitalizada.DatosAdministrativos.DebitoEnCuenta.TipoCuenta != null)
                for (var i = 0; i < $scope.data.tipoCuentas.length; i++) {
                    if ($scope.data.solicitudDigitalizada.DatosAdministrativos.DebitoEnCuenta.TipoCuenta.ID.toString() == $scope.data.tipoCuentas[i].ID.toString())
                        $scope.data.solicitudDigitalizada.tipoCuentaSeleccionado = $scope.data.tipoCuentas[i];
                }
        }
    }
    $scope.SetearAutocompleteAreaProtegida = function () {
        for (var i = 0; i < $scope.data.localidades.length; i++) {

            if ($scope.data.solicitudDigitalizada.AreaProtegida.Localidad.value == $scope.data.localidades[i].value)
                $("#txtLocalidadArea").val($scope.data.localidades[i].label);
        }
    }

    $scope.openmapaCliente = function () {
        $("#btnCerrarPopupRevisar").attr("disabled", "disabled");
        var loc = "";
        if ($scope.data.solicitudDigitalizada.EntidadCliente.Localidad != null && $scope.data.solicitudDigitalizada.EntidadCliente.Localidad.label != null)
            loc = $scope.data.solicitudDigitalizada.EntidadCliente.Localidad.label.replace(/ *\([^)]*\) */g, "")

        initMap($scope.data.solicitudDigitalizada.EntidadCliente.Calle,
            $scope.data.solicitudDigitalizada.EntidadCliente.Altura,
            loc,
            "Buenos Aires",
            $scope.data.solicitudDigitalizada.EntidadCliente.Latitud,
            $scope.data.solicitudDigitalizada.EntidadCliente.Longitud,
            "mapaCliente");

        $('#popupMapa').modal('show');
    };


    $scope.openmapaArea = function () {
        $("#btnCerrarPopupRevisar").attr("disabled", "disabled");
        var loc = "";
        if ($scope.data.solicitudDigitalizada.AreaProtegida.Localidad != null && $scope.data.solicitudDigitalizada.AreaProtegida.Localidad.label != null)
            loc = $scope.data.solicitudDigitalizada.AreaProtegida.Localidad.label.replace(/ *\([^)]*\) */g, "")

        initMap($scope.data.solicitudDigitalizada.AreaProtegida.Calle,
            $scope.data.solicitudDigitalizada.AreaProtegida.Altura,
            loc,
            "Buenos Aires",
            $scope.data.solicitudDigitalizada.AreaProtegida.Latitud,
            $scope.data.solicitudDigitalizada.AreaProtegida.Longitud,
            "mapaArea");

        $('#popupMapa').modal('show');
    };

    $("#popupMapa").on('hidden.bs.modal', function () {

        $("#btnCerrarPopupRevisar").removeAttr("disabled");

        if (direccionDelForm != null) {

            FormatearFecha(direccionDelForm,
            function (result) {
                if (result.source == "mapaCliente") {
                    $scope.data.solicitudDigitalizada.EntidadCliente.Calle = result.Calle;
                    $scope.data.solicitudDigitalizada.EntidadCliente.Altura = result.Altura;
                    $scope.data.solicitudDigitalizada.EntidadCliente.Latitud = result.Latitud;
                    $scope.data.solicitudDigitalizada.EntidadCliente.Longitud = result.Longitud;
                    if (result.LocalidadDesc != null && result.LocalidadId != null && (
                        $scope.data.solicitudDigitalizada.EntidadCliente.Localidad == null ||
                            $scope.data.solicitudDigitalizada.EntidadCliente.Localidad.PartidoId != result.PartidoId)) {
                        var localidadItem = { label: result.LocalidadDesc, value: result.LocalidadId };
                        $scope.data.solicitudDigitalizada.EntidadCliente.Localidad = localidadItem;
                        $("#txtLocalidadCliente").val(localidadItem.label);
                    }
                    if (result.CodPostal != null)
                        $scope.data.solicitudDigitalizada.EntidadCliente.CP = result.CodPostal;
                    $scope.setCoordinatesClienteColor('black');
                }
                else if (result.source == "mapaArea") {
                    $scope.data.solicitudDigitalizada.AreaProtegida.Calle = result.Calle;
                    $scope.data.solicitudDigitalizada.AreaProtegida.Altura = result.Altura;
                    $scope.data.solicitudDigitalizada.AreaProtegida.Latitud = result.Latitud;
                    $scope.data.solicitudDigitalizada.AreaProtegida.Longitud = result.Longitud;
                    if (result.LocalidadDesc != null && result.LocalidadId != null && (
                        $scope.data.solicitudDigitalizada.EntidadCliente.Localidad == null ||
                            $scope.data.solicitudDigitalizada.AreaProtegida.Localidad.PartidoId != result.PartidoId)) {
                        var localidadItem = { label: result.LocalidadDesc, value: result.LocalidadId };
                        $scope.data.solicitudDigitalizada.AreaProtegida.Localidad = localidadItem;
                        $("#txtLocalidadArea").val(localidadItem.label);
                    }
                    if (result.CodPostal != null)
                        $scope.data.solicitudDigitalizada.AreaProtegida.CP = result.CodPostal;
                    $scope.setCoordinatesAreaColor('black');
                }
                $scope.$apply();
            });
        }
        $scope.$apply();
    });

    $scope.configFirmaInvalida = function () {
        //TODO: Corregir funcionamiento de la firma
        if ($scope.data.solicitudDigitalizada.EntidadCliente.Estado == clientePreparado) {
            $scope.data.solicitudDigitalizada.EntidadCliente.Estado = clientePotencial;
        }

        $scope.data.solicitudDigitalizada.DatosAdministrativos.Firmado = false;
        document.getElementById('chkEsPreparado').checked = false;
    }

    $scope.guardarFirma = function () {
        $scope.firma = $scope.accept();
        if ($scope.data.solicitudDigitalizada.EntidadCliente.ClienteId != 0) {
            var url = "";
            if ($scope.firma.isEmpty)
                $scope.configFirmaInvalida();
            else {
                url = $scope.firma.dataUrl;
                $scope.data.solicitudDigitalizada.DatosAdministrativos.Firmado = true;
            }


            $http.post('/AfiliacionesPopUp/GuardarFirma', { pFirma: url, pCli: $scope.data.solicitudDigitalizada.EntidadCliente.ClienteId })
               .then(function (response) {
                   if (response.data.Success == true) {
                       //$scope.mostrarSuccess("Firma guardada", response.data.Message);
                   }
                   else {
                       $scope.configFirmaInvalida();
                       $scope.mostrarError("Error al guardada la firma ", response.data.Message);
                   }

               }, function (error) {
                   $scope.configFirmaInvalida();
                   $scope.mostrarError("Error al guardada la firma ", "");
                   console.log(response.data.Message);
               });
            //}
            //else
            //    $scope.configFirmaInvalida();

        }
        else {
            $scope.configFirmaInvalida();
            $scope.mostrarError("Error al guardar la Firma", "Cliente invalido, pruebe guardando solo el cliente primero.");
        }
    };


    $scope.CargarFirma = function () {
        $scope.clean();
        if ($scope.data.solicitudDigitalizada.DatosAdministrativos.Firmado &&
            $scope.data.solicitudDigitalizada.DatosAdministrativos.FirmaBase64 != null) {

            //var canvas = document.getElementById("SignatureCanvas");
            //var ctx = canvas.getContext("2d");
            //var image = new Image();
            //image.src = $scope.data.solicitudDigitalizada.DatosAdministrativos.FirmaBase64;
            //$scope.clean();
            //image.onload = function () {
            //    ctx.drawImage(image, 0, 0);
            //};

            $scope.firma = $scope.accept($scope.data.solicitudDigitalizada.DatosAdministrativos.FirmaBase64);
            //$scope.setFirmaBase64($scope.data.solicitudDigitalizada.DatosAdministrativos.FirmaBase64);
            //var sp = $("#signature-pad");
            //sp.attr("dataurl", $scope.data.solicitudDigitalizada.DatosAdministrativos.FirmaBase64);
            //var canvas = document.getElementById("SignatureCanvas");
            //var ctx = canvas.getContext("2d");
            //
            //var image = new Image();

            //image.src = $scope.data.solicitudDigitalizada.DatosAdministrativos.FirmaBase64;

            //$scope.clean();

            //image.onload = function () {
            //    ctx.drawImage(image, 0, 0);
            //};
            //$scope.endDrawing();
            //$scope.newFunctionn();
            //$scope.$emit('updateModel', args);
            //$scope.loadSign($scope.data.solicitudDigitalizada.DatosAdministrativos.FirmaBase64);
            //$scope.firma = new Object();
            //$scope.firma.dataUrl = $scope.data.solicitudDigitalizada.DatosAdministrativos.FirmaBase64;
            //$scope.firma.dataurl = $scope.data.solicitudDigitalizada.DatosAdministrativos.FirmaBase64;
            //$scope.dataurl = $scope.data.solicitudDigitalizada.DatosAdministrativos.FirmaBase64;
            //$scope.firma.isEmpty = false;
            //$scope.$apply();
            //$scope.updateModel();
        }
    };

    //$scope.$watch('chkFlgDomicilioLegalCobertura', function (newvalue, oldvalue) {
    //    if (newvalue !== oldvalue) {
    //        if(newvalue == "checked"){
    //            $scope.data.solicitudDigitalizada.AreaProtegida.FlgDomicilioLegalCobertura = 1;
    //            $scope.copyAdress();
    //        }
    //        else
    //            $scope.data.solicitudDigitalizada.AreaProtegida.FlgDomicilioLegalCobertura = 0;
    //    }
    //});
    $scope.ValidarDatosAdministrativos = function () {
        if ($scope.data.solicitudDigitalizada.formaDePagoSeleccionado.Descripcion === $scope.data.constantesFacturacion.tarjetaDeCredito) {
            if ($scope.data.solicitudDigitalizada.tipoDeTarjetaSeleccionado.MascaraIngreso == '9999-999999-99999') {
                if ($scope.data.solicitudDigitalizada.DatosAdministrativos.TarjetaDeCredito.Numero.length != 17) {
                    $scope.mostrarError("Error Datos Administrativos", "Número de tarjeta invalido");
                    return false;
                }
            }
            else {
                if ($scope.data.solicitudDigitalizada.DatosAdministrativos.TarjetaDeCredito.Numero.length != 19) {
                    $scope.mostrarError("Error Datos Administrativos", "Número de tarjeta invalido");
                    return false;
                }
            }
            if ($scope.data.solicitudDigitalizada.DatosAdministrativos.TarjetaDeCredito.Vencimiento.length != 5) {
                $scope.mostrarError("Error Datos Administrativos", "Fecha de vencimiento de tarjeta invalida");
                return false;
            }
        }
        else if ($scope.data.solicitudDigitalizada.formaDePagoSeleccionado.Descripcion === $scope.data.constantesFacturacion.debitoAutomatico) {
            if ($scope.data.solicitudDigitalizada.bancoSeleccionado.ID != $scope.data.solicitudDigitalizada.DatosAdministrativos.DebitoEnCuenta.CBU.substring(0, 3)) {
                $scope.mostrarError("Error Datos Administrativos", "El CBU no corresponde al banco seleccionado");
                return false;
            }
        }
        if ($scope.data.solicitudDigitalizada.DatosAdministrativos.ProximoPago != null &&
            $scope.data.solicitudDigitalizada.DatosAdministrativos.ProximoPago.length != 7 &&
            $scope.data.solicitudDigitalizada.DatosAdministrativos.ProximoPago.length != 0) {
            $scope.mostrarError("Error Datos Administrativos", "Fecha de proximo pago invalida");
            return false;
        }
        return true;
    }
    $scope.TarjetaVencimientoChange = function () {
        if ($scope.data.solicitudDigitalizada.DatosAdministrativos.TarjetaDeCredito != null && $scope.data.solicitudDigitalizada.DatosAdministrativos.TarjetaDeCredito.Vencimiento != null)
            if ($scope.data.solicitudDigitalizada.DatosAdministrativos.TarjetaDeCredito.Vencimiento.length == 5) {
                var month = $scope.data.solicitudDigitalizada.DatosAdministrativos.TarjetaDeCredito.Vencimiento.substring(0, 2)
                if (parseInt(month) > 12) {
                    $scope.data.solicitudDigitalizada.DatosAdministrativos.TarjetaDeCredito.Vencimiento =
                        $scope.data.solicitudDigitalizada.DatosAdministrativos.TarjetaDeCredito.Vencimiento.replace(month + "/", "12/");
                }
            }
    }

    $scope.ProximoPagoChange = function () {
        if ($scope.data.solicitudDigitalizada.DatosAdministrativos.ProximoPago != null)
            if ($scope.data.solicitudDigitalizada.DatosAdministrativos.ProximoPago.length == 7) {
                var month = $scope.data.solicitudDigitalizada.DatosAdministrativos.ProximoPago.substring(0, 2)
                if (parseInt(month) > 12) {
                    $scope.data.solicitudDigitalizada.DatosAdministrativos.ProximoPago =
                        $scope.data.solicitudDigitalizada.DatosAdministrativos.ProximoPago.replace(month + "/", "12/");
                }
            }
    }

    $scope.RefrescarEstado = function () {
        //FIX: Checkbox no dispara setEsPreparado la primera vez (si viene en true)
        if ($scope.data.solicitudDigitalizada.EntidadCliente.Estado == clientePreparado || $scope.data.solicitudDigitalizada.EntidadCliente.Estado == clientePotencial) {
            $scope.data.solicitudDigitalizada.EntidadCliente.Estado = document.getElementById('chkEsPreparado').checked ? clientePreparado : clientePotencial;
        }
    }
    //ANGULAR JS => $http.post  
    $scope.GuardarSolicitud = function () {
        $scope.SetValuesForm();

        //var canvas = document.getElementById("SignatureCanvas");
        //var signaturePad = new SignaturePad(canvas);
        //var upUrl = signaturePad.toDataURL();
        //$scope.firma = $scope.accept(upUrl);

        //TODO: Fix, si bindeo los dos input a la misma propiedad 'mask' recorta el valor en la carga.
        if ($scope.data.solicitudDigitalizada.formaDePagoSeleccionado.Descripcion === $scope.data.constantesFacturacion.tarjetaDeCredito
            && $scope.data.solicitudDigitalizada.DatosAdministrativos.TarjetaDeCredito.TipoDeTarjeta.MascaraIngreso == '9999-999999-99999')
            $scope.data.solicitudDigitalizada.DatosAdministrativos.TarjetaDeCredito.Numero = $scope.data.solicitudDigitalizada.DatosAdministrativos.TarjetaDeCredito.NumeroAmerican;

        $scope.firma = $scope.accept();
        $scope.data.solicitudDigitalizada.DatosAdministrativos.Firmado = !$scope.firma.isEmpty;

        $scope.RefrescarEstado();


        if (!$scope.validarCuitDigitoVerificador($scope.data.solicitudDigitalizada.EntidadCliente.Cuit)) {
            $scope.mostrarError("Error al guardar el cliente", "CUIT invalido.");
            return;
        }

        if ($scope.ValidarGeo() && $scope.ValidarDatosAdministrativos()) {
            $http.post('/AfiliacionesPopUp/GuardarSolicitud', { Solicitud: $scope.data.solicitudDigitalizada })
               .then(function (response) {
                   if (response.data.Success == true) {
                       $scope.data.solicitudDigitalizada.EntidadCliente.ClienteId = response.data.ClienteId;
                       $scope.guardarFirma();


                       //var errorElement = document.getElementById("errorElementId");
                       //
                       $scope.mostrarSuccess("Cliente guardado", response.data.Message);
                       //$scope.bloquear.Firma = false;
                       //FIX
                       if ($scope.data.solicitudDigitalizada.EntidadCliente.Estado == clientePreparado)
                           document.getElementById('chkEsPreparado').checked = true;

                       $scope.GetData();
                   }
                   else
                       $scope.mostrarError("Error al guardar el cliente", response.data.Message);
               }, function (error) {

                   $scope.mostrarError("Error al guardar el cliente", "");
               });
        }
    }

    $scope.validarCuitDigitoVerificador = function (Cuit) {

        //var m = document.getElementById("matricula").value;
        var expreg = /\b(20|23|24|27|30|33|34)(\D)?[0-9]{8}(\D)?[0-9]/;

        if (expreg.test(Cuit)) {

            var cuitsplit = Cuit.split("-");
            var num = cuitsplit[0] + cuitsplit[1];
            var verif = cuitsplit[2];
            var multiplicadores = [5, 4, 3, 2, 7, 6, 5, 4, 3, 2];
            var calculo = 0;

            /*
            * Recorro el arreglo y el numero de document_number para
            * realizar las multiplicaciones.
            */
            for (var i = 0; i < 10; i++) {
                calculo += (parseInt(num.charAt(i)) * multiplicadores[i]);
            }

            // Calculo el resto.
            var resto = (parseInt(calculo)) % 11;
            resto = 11 - resto;

            if (resto == 11) resto = 0;
            if (resto == 10) resto = 9;

            if (resto == verif)
                return true;
            else
                return false;
        }
        else
            return false;
    }

    //Lo Valida el Guardar.
    $scope.validarCuitServicio = function (Cuit, mostrarMensaje) {
        var ClienteId = parseInt($scope.data.solicitudDigitalizada.EntidadCliente.ClienteId) || 0;
        $http.post('/AfiliacionesPopUp/ValidateCUIT', { cuit: Cuit, clienteId: ClienteId })
        .then(function successCallback(response) {
            if (response.data.Success == true) {
                if (mostrarMensaje)
                    $scope.mostrarSuccess("CUIT valido", response.data.Message);
                return true;
            }
            else {
                if (mostrarMensaje)
                    $scope.mostrarError("CUIT invalido", response.data.Message);
                return false;
            }
        }, function errorCallback(response) {
            if (mostrarMensaje)
                $scope.mostrarError("Error", "Error al consultar CUIT.");
            console.log(response);
            return false;
        });
    };

    $scope.validarCuit = function (Cuit, mostrarMensaje) {
        if (!StringHasValue(Cuit)) {
            if (mostrarMensaje)
                $scope.mostrarError("CUIT invalido", "No puede ser vacio.");
            return false;
        }
        if (!$scope.validarCuitDigitoVerificador(Cuit)) {
            if (mostrarMensaje)
                $scope.mostrarError("CUIT invalido", "No es un CUIT real.");
            return false;
        }
        return $scope.validarCuitServicio(Cuit, mostrarMensaje);
    }

    $scope.ValidarGeo = function () {
        if (!StringHasValue($scope.data.solicitudDigitalizada.EntidadCliente.Longitud) ||
            $scope.data.solicitudDigitalizada.EntidadCliente.Longitud == "0" ||
           !StringHasValue($scope.data.solicitudDigitalizada.EntidadCliente.Longitud ||
            $scope.data.solicitudDigitalizada.EntidadCliente.Longitud == "0")) {
            $scope.mostrarError("Coordenadas Vacias", "Debe establecer la ubicacion del cliente utilizando el Mapa.")
            return false;
        }
        if (!StringHasValue($scope.data.solicitudDigitalizada.AreaProtegida.Longitud) ||
            $scope.data.solicitudDigitalizada.AreaProtegida.Longitud == "0" ||
            !StringHasValue($scope.data.solicitudDigitalizada.AreaProtegida.Longitud) ||
            $scope.data.solicitudDigitalizada.AreaProtegida.Longitud == "0") {
            if (!StringHasValue($scope.data.solicitudDigitalizada.AreaProtegida.Calle) &&
                !StringHasValue($scope.data.solicitudDigitalizada.AreaProtegida.Altura) &&
                !StringHasValue($scope.data.solicitudDigitalizada.AreaProtegida.Localidad)) {
                $scope.copyAdress(true);
                return true;
            }
            else {
                $scope.mostrarError("Coordenadas Vacias", "Debe establecer la ubicacion del Area Protegida utilizando el Mapa.")
                return false;
            }
        }
        return true;
    }

    $scope.Afiliar = function () {
        if ($scope.ValidarDatosAdministrativos()) {
            var email = "";
            if (document.getElementById('chkEnviarEmailContrato').checked)
                email = $scope.data.solicitudDigitalizada.ClienteContacto.Email;
            $http.post('/AfiliacionesPopUp/Afiliar', { pCli: $scope.data.solicitudDigitalizada.EntidadCliente.ClienteId, pEmail: email })
            .then(function (response) {

                if (response.data.Success == true) {
                    $scope.mostrarSuccess('¡Cliente Afiliado!', response.data.Message);
                    $('#popupRevisar').modal('toggle');
                    $scope.GetData();
                }
                else {
                    $scope.mostrarError("Error", response.data.Message);
                }
            }, function (error) {
                $scope.mostrarError("Error", "No se pudo afiliar al Cliente.");
                console.log('¡No se pudo afiliado el cliente!', response.data.Message);
            });
        }
    }

    $scope.GetData();

    $scope.SetValuesForm = function () {
        //$scope.GetCurrencyValues();//Prueba commit.
        if ($scope.data.solicitudDigitalizada.formaDePagoSeleccionado.Descripcion == "PAGO MIS CUENTAS") {
            $scope.data.solicitudDigitalizada.DatosAdministrativos.Pagomiscuentas = true;
        }
        //if ($scope.data.solicitudDigitalizada.DatosAdministrativos.FormaDePago.Descripcion == $scope.data.constantesFacturacion.debitoAutomatico
        //    && $scope.data.solicitudDigitalizada.DatosAdministrativos.DebitoEnCuenta != null)
        //    $scope.data.solicitudDigitalizada.DatosAdministrativos.DebitoEnCuenta.TipoCuenta = $('input[name=TipoDeCuenta]:checked').val();

        $scope.data.solicitudDigitalizada.SolicitudDigitalizadaBasico.FechaIngreso = $scope.dt;
    }

    //$('.togglebutton').data("flag", 1);
    //$('.togglebutton').click(function () {
    //    var $t = $(this);
    //    if ($t.data("flag") == 1) {
    //        $t.removeClass("glyphicon glyphicon-chevron-right");
    //        $t.addClass("glyphicon glyphicon-chevron-down");
    //        $t.data("flag", 0);
    //    } else {
    //        $t.removeClass("glyphicon glyphicon-chevron-down");
    //        $t.addClass("glyphicon glyphicon-chevron-right");
    //        $t.data("flag", 1);
    //    }
    //})

    //Helper
    var StringHasValue = function (strValue) {
        if ($.trim(strValue) != "" && $.trim(strValue) != null && $.trim(strValue) != undefined)
            return true;
        else
            return false;
    };

    Array.prototype.slice.call(document.querySelectorAll('.Accordion')).forEach(function (accordion) {

        // Allow for multiple accordion sections to be expanded at the same time
        var allowMultiple = accordion.hasAttribute('data-allow-multiple');
        // Allow for each toggle to both open and close individually
        var allowToggle = (allowMultiple) ? allowMultiple : accordion.hasAttribute('data-allow-toggle');

        // Create the array of toggle elements for the accordion group
        var triggers = Array.prototype.slice.call(accordion.querySelectorAll('.Accordion-trigger'));
        var panels = Array.prototype.slice.call(accordion.querySelectorAll('.Accordion-panel'));

        accordion.addEventListener('click', function (event) {
            var target = event.target;
            //
            if (target.classList.contains('Accordion-trigger')) {
                // Check if the current toggle is expanded.
                var isExpanded = target.getAttribute('aria-expanded') == 'true';
                var active = accordion.querySelector('[aria-expanded="true"]');

                // without allowMultiple, close the open accordion
                if ((!allowMultiple && active && active !== target) || isExpanded) {
                    // Set the expanded state on the triggering element
                    active.setAttribute('aria-expanded', 'false');
                    // Hide the accordion sections, using aria-controls to specify the desired section
                    document.getElementById(active.getAttribute('aria-controls')).setAttribute('hidden', '');

                    // When toggling is not allowed, clean up disabled state
                    if (!allowToggle) {
                        active.removeAttribute('aria-disabled');
                    }
                }

                if (!isExpanded) {
                    // Set the expanded state on the triggering element
                    target.setAttribute('aria-expanded', 'true');
                    // Hide the accordion sections, using aria-controls to specify the desired section
                    document.getElementById(target.getAttribute('aria-controls')).removeAttribute('hidden');

                    // If toggling is not allowed, set disabled state on trigger
                    if (!allowToggle) {
                        target.setAttribute('aria-disabled', 'true');
                    }
                }
                else if (allowToggle && isExpanded) {
                    // Set the expanded state on the triggering element
                    target.setAttribute('aria-expanded', 'false');
                    // Hide the accordion sections, using aria-controls to specify the desired section
                    document.getElementById(target.getAttribute('aria-controls')).setAttribute('hidden', '');
                }

                event.preventDefault();
            }
        });

        // Bind keyboard behaviors on the main accordion container
        accordion.addEventListener('keydown', function (event) {
            var target = event.target;
            if (event.which != null) {
                var key = event.which.toString();
                // 33 = Page Up, 34 = Page Down
                var ctrlModifier = (event.ctrlKey && key.match(/33|34/));

                // Is this coming from an accordion header?
                if (target.classList.contains('Accordion-trigger')) {
                    // Up/ Down arrow and Control + Page Up/ Page Down keyboard operations
                    // 38 = Up, 40 = Down
                    if (key.match(/38|40/) || ctrlModifier) {
                        var index = triggers.indexOf(target);
                        var direction = (key.match(/34|40/)) ? 1 : -1;
                        var length = triggers.length;
                        var newIndex = (index + length + direction) % length;

                        triggers[newIndex].focus();

                        event.preventDefault();
                    }
                    else if (key.match(/35|36/)) {
                        // 35 = End, 36 = Home keyboard operations
                        switch (key) {
                            // Go to first accordion
                            case '36':
                                triggers[0].focus();
                                break;
                                // Go to last accordion
                            case '35':
                                triggers[triggers.length - 1].focus();
                                break;
                        }

                        event.preventDefault();
                    }
                }
                else if (ctrlModifier) {
                    // Control + Page Up/ Page Down keyboard operations
                    // Catches events that happen inside of panels
                    panels.forEach(function (panel, index) {
                        if (panel.contains(target)) {
                            triggers[index].focus();

                            event.preventDefault();
                        }
                    });
                }
            }
        });

        // Minor setup: will set disabled state, via aria-disabled, to an
        // expanded/ active accordion which is not allowed to be toggled close
        if (!allowToggle) {
            // Get the first expanded/ active accordion
            var expanded = accordion.querySelector('[aria-expanded="true"]');

            // If an expanded/ active accordion is found, disable
            if (expanded) {
                expanded.setAttribute('aria-disabled', 'true');
            }
        }

    });
    $scope.GetDirectSolicitudDigitalizada = function () {
        if ($scope.data.localidades == null) {
            return;
        }
        else if ($scope.data.rubros == null) {
            return;
        }
        else if ($scope.data.condicionIVAs == null) {
            return;
        }
        else if ($scope.data.formaDePagos == null) {
            return;
        }
        else if ($scope.data.tipoDeTarjetas == null) {
            return;
        }
        else if ($scope.data.bancos == null) {
            return;
        }
        else if ($scope.data.tipoCuentas == null) {
            return;
        }
        else
        {
            $scope.GetSolicitudDigitalizada($scope.GetDirectClienteId);
        }
            
    }

    $scope.findGetParameter = function (parameterName) {
        debugger;
        var result = null,
            tmp = [];
        var items = location.search.substr(1).split("&");
        for (var index = 0; index < items.length; index++) {
            tmp = items[index].split("=");
            if (tmp[0] === parameterName) result = decodeURIComponent(tmp[1]);
        }
        return result;
    }
    
    $scope.GetDirectSolicitud = $scope.findGetParameter('GetDirectSolicitud') == 'true';
    $scope.GetDirectClienteId = $scope.findGetParameter('GetDirectClienteId');
    $scope.ShowGetContrato = $scope.findGetParameter('Acceso') == '3' && $scope.findGetParameter('Estado') == '3';
    //GetLocalidades
    $http({
        method: 'GET',
        url: '/AfiliacionesPopUp/GetLocalidades'
    }).then(function successCallback(response) {
        $scope.data.localidades = response.data,
        $("#txtLocalidadCliente").autocomplete({
            source: response.data,
            select: function (event, ui) {
                $scope.data.solicitudDigitalizada.EntidadCliente.Localidad = ui.item;
                $("#txtLocalidadCliente").val(ui.item.label);
                return false;
            },
        });
        $("#txtLocalidadArea").autocomplete({
            source: response.data,
            select: function (event, ui) {

                $scope.data.solicitudDigitalizada.AreaProtegida.Localidad = ui.item;
                $("#txtLocalidadArea").val(ui.item.label);
                return false;
            },
        });
        if ($scope.GetDirectSolicitud)
            $scope.GetDirectSolicitudDigitalizada();
    }, function errorCallback(response) {
        console.log(response);
    });

    //GetRubros
    $http({
        method: 'GET',
        url: '/AfiliacionesPopUp/GetRubros'
    }).then(function successCallback(response) {
        $scope.data.rubros = response.data;
        if ($scope.GetDirectSolicitud)
            $scope.GetDirectSolicitudDigitalizada();
    }, function errorCallback(response) {
        console.log(response);
    });

    //GetCondicionIVAs
    $http({
        method: 'GET',
        url: '/AfiliacionesPopUp/GetCondicionIVAs'
    }).then(function successCallback(response) {
        $scope.data.condicionIVAs = response.data;
        if ($scope.GetDirectSolicitud)
            $scope.GetDirectSolicitudDigitalizada();
    }, function errorCallback(response) {
        console.log(response);
    });

    //GetFormaDePagos
    $http({
        method: 'GET',
        url: '/AfiliacionesPopUp/GetFormaDePagos'
    }).then(function successCallback(response) {
        $scope.data.formaDePagos = response.data;
        if ($scope.GetDirectSolicitud)
            $scope.GetDirectSolicitudDigitalizada();
    }, function errorCallback(response) {
        console.log(response);
    });

    //GetTipoDeTarjetas
    $http({
        method: 'GET',
        url: '/AfiliacionesPopUp/GetTipoDeTarjetas'
    }).then(function successCallback(response) {
        $scope.data.tipoDeTarjetas = response.data;
        if ($scope.GetDirectSolicitud)
            $scope.GetDirectSolicitudDigitalizada();
    }, function errorCallback(response) {
        console.log(response);
    });

    //GetBancos
    $http({
        method: 'GET',
        url: '/AfiliacionesPopUp/GetBancos'
    }).then(function successCallback(response) {
        $scope.data.bancos = response.data;
        if ($scope.GetDirectSolicitud)
            $scope.GetDirectSolicitudDigitalizada();
    }, function errorCallback(response) {
        console.log(response);
    });

    //GetTipoCuentas
    $http({
        method: 'GET',
        url: '/AfiliacionesPopUp/GetTipoCuentas'
    }).then(function successCallback(response) {
        $scope.data.tipoCuentas = response.data;
        if ($scope.GetDirectSolicitud)
            $scope.GetDirectSolicitudDigitalizada(23982);
    }, function errorCallback(response) {
        console.log(response);
    });
});
