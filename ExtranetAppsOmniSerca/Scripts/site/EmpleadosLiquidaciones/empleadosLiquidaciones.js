app.controller('empleadosLiquidacionesCtrl', function ($scope, $http, $window) {
    //variables
    //$scope.onlyNumber = /^\d+$/; //"/^[0-9]+(\.[0-9]{1,2})?$/";
    checkSessionStorage();
    $scope.solapa = 'liquidaciones';
    _this = this;
    $scope.acceso = 0;
    $scope.filter = {
        LiqId: 0,
        periodo: '',
        estado: '0'
    };
    $scope.ordenServicioSettings = {};
    $scope.ordenServicio = {};
    $scope.data = {
        periodos: [],
        dias: [],
        estados: [],
        revisar: {},
        resumenDetalle: {},
        motivos: [],
        resumen: {}
    };
    $scope.style = {
        Descripcion: {
            'width': '80%',
            'font-size': '13px',
            'padding-top': '4px',
            'padding-bottom': '4px'
        },
        Descripcion2: {
            'width': '60%',
            'font-size': '13px',
            'padding-top': '4px',
            'padding-bottom': '4px'
        },
        Cantidad: {
            'width': '20%',
            'font-size': '13px',
            'padding-top': '4px',
            'padding-bottom': '4px',
            'text-align': 'right'
        },
        HoraDia: {
            'width': '20%',
            'font-size': '13px',
            'padding-top': '4px',
            'padding-bottom': '4px',
            'text-align': 'right'
        }
    };

    
    $scope.data.dias = [
    {
        Descripcion: 'Todos',
        ID: -1
    }
    ];
    $scope.data.estados = [
        {
            Descripcion: 'Todos',
            ID: '0'
        },
        {
            Descripcion: 'Pendientes',
            ID: '1'
        },
        {
            Descripcion: 'Aceptados',
            ID: '2'
        },
        {
            Descripcion: 'Rechazados',
            ID: '3'
        }
    ];

    $scope.Reliquidar = function () {
        checkSessionStorage();
        $http({
            method: 'GET',
            url: '/EmpleadosLiquidaciones/Reliquidar?pLiqId=' + $scope.filter.LiqId
        }).then(function successCallback(response) {
            GetData();
        }, function errorCallback(response) {
            console.log(response);
        });
    }

    $scope.EstadoChange = function () {
        if ($scope.acceso == 3)
            $scope.GetEmpleados();
    };

    $scope.PeriodoChange = function () {
        //if ($scope.acceso == 3)
            $scope.GetEmpleados();
    };

    $scope.EmpleadoChange = function () {
        $scope.GetData();
    };
    //GetData (datos de la primer solapa)
    $scope.GetData = function () {
        checkSessionStorage();
        if ($scope.filter.periodo <= 0) {
            alert('Debe ingresar el periodo.')
            $scope.data.liquidaciones.settings.source = new $.jqx.dataAdapter([{}]);
            return;
        }
        
        if ($scope.filter.LiqId <= 0) {
            alert('Debe ingresar el empleado.')
            $scope.data.liquidaciones.settings.source = new $.jqx.dataAdapter([{}]);
            return;
        }
        $http({
            method: 'GET',
            url: '/EmpleadosLiquidaciones/GetLiquidacionDetalle?pLiqId=' + $scope.filter.LiqId + '&pEstado=' + $scope.filter.estado
        }).then(function successCallback(response) {
            var data = {
                localdata: response.data,
                datafields: [
                    { name: 'ID', type: 'string' },
                    { name: 'pacFecEntrada', type: 'string' },
                    { name: 'pacFecHorEntrada', type: 'dateTime' },
                    { name: 'pacFecHorSalida', type: 'string' },
                    { name: 'MovilPacto', type: 'string' },
                    { name: 'relFecHorEntrada', type: 'string' },
                    { name: 'relFecHorSalida', type: 'string' },
                    { name: 'MovilReal', type: 'string' },
                    { name: 'HoraTrabajado', type: 'string' },
                    { name: 'HoraNocturno', type: 'string' },
                    { name: 'HoraDiurno', type: 'dateTime' },
                    { name: 'HoraFinSemana', type: 'string' },
                    { name: 'HoraIncumplimiento', type: 'string' },
                    { name: 'MinLLegadaTarde', type: 'string' },
                    { name: 'MinRetAnticipado', type: 'string' },
                    { name: 'flgAusenciaSinPremio', type: 'long' },
                    { name: 'cntServicios', type: 'long' },
                    { name: 'TipoNovedad', type: 'string' },
                    { name: 'Status', type: 'string' },
                ],
                datatype: "array"
            }
            $scope.data.liquidaciones.settings.source = new $.jqx.dataAdapter(data);
            $('#liquidacionesGrid').jqxGrid('showcolumn', 'Orden');
            //$('#liquidacionesGrid').jqxGrid((($scope.acceso == '3') ? 'showcolumn' : 'hidecolumn'), 'Orden');
        }, function errorCallback(response) {
            console.log(response);
        });
        $http({
            method: 'GET',
            url: '/EmpleadosLiquidaciones/GetResumen?pMov=' + $scope.filter.LiqId + '&pPer=' + $scope.filter.periodo
        }).then(function successCallback(response) {
            $scope.data.resumen = response.data;
        }, function errorCallback(response) {
            console.log(response);
        });
    }

    $http({
        method: 'GET',
        url: '/EmpleadosLiquidaciones/GetAcceso'
    }).then(function successCallback(response) {
        $scope.acceso = response.data.toString();
    }, function errorCallback(response) {
        console.log(response);
    });

    $http({
        method: 'GET',
        url: '/EmpleadosLiquidaciones/GetMotivosReclamos'
    }).then(function successCallback(response) {
        $scope.data.motivos = response.data;
    }, function errorCallback(response) {
        console.log(response);
    });

    $scope.GetPeriodos = function () {
        checkSessionStorage();
        $http({
            method: 'GET',
            url: '/EmpleadosLiquidaciones/GetPeriodos'
        }).then(function successCallback(response) {
            $scope.data.periodos = response.data;
            $scope.filter.periodo = response.data[0].Periodo;
            $scope.GetEmpleados();
        }, function errorCallback(response) {
            console.log(response);
        });
    }

    //Obtengo el empleados
    $scope.GetEmpleados = function () {
        checkSessionStorage();
        $http({
            method: 'GET',
            url: '/EmpleadosLiquidaciones/GetEmpleados?pPer=' + $scope.filter.periodo + '&pEst=' + $scope.filter.estado
        }).then(function successCallback(response) {
            $scope.data.empleados = response.data;
            if ($scope.data.empleados.length > 0) {
                $scope.filter.LiqId = $scope.data.empleados[0].LiqId;
                $scope.GetData();
            }
        }, function errorCallback(response) {
            console.log(response);
        });
    }
    

    //Cargo los combos
    $scope.GetPeriodos();

    $scope.crOrdenServicio = function (row, columnfield, value, defaulthtml, columnproperties) {
        var icon = (value == 1) ? '' : 'glyphicon-upload verde';
        return '<div style="cursor: pointer;"><span style="margin-left:34%;line-height:33px;" class="glyphicon ' + icon + ' icon-right-margin big-icon"></span></div>'
    }
    $scope.crConfirmacion = function (row, columnfield, value, defaulthtml, columnproperties) {
        var icon = '';
        switch (parseInt(value)) {
            case 0: // --> No esta conforme
                icon = 'glyphicon-remove-circle rojo';
                break;
            case 1: // --> Está conforme
                icon = 'glyphicon-ok-circle verde';
                break;
            case 2: // --> No está conforme y recibió una respuesta
                icon = 'glyphicon-exclamation-sign amarillo';
                break;
            case 3: // --> Reclamo aceptado
                icon = 'glyphicon-ok-circle azul';
                break;
            case 4: // --> Reclamo no aceptado
                icon = 'glyphicon-remove-circle naranja ';
                break;
        }
        return '<div style="cursor: pointer;"><span style="margin-left:34%;line-height:33px;" class="glyphicon ' + icon + ' icon-right-margin big-icon"></span></div>'

    }
    $scope.NroIncidente = function (row, columnfield, value, defaulthtml, columnproperties) {
        return '<div class="jqx-grid-cell-middle-align" style="margin-top: 6px;"><a href="#">' + value + '</a></div>';
    }
    //Encabezados
    $scope.columnrenderer = function (value) {
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
    $scope.FormatearFecha = function (row, columnfield, value, defaulthtml, columnproperties) {
        return '<div class="jqx-grid-cell-middle-align" style="margin-top: 6px;">' + moment(value).format('DD/MM/YYYY') + '</div>';
    }
    $scope.FormatearHora = function (row, columnfield, value, defaulthtml, columnproperties, rowData) {
        var color = '';
        if (columnfield == 'relFecHorEntrada') {
            if ((rowData.MinLLegadaTarde) > 10)
                color = 'color: red;'
        }
        if (columnfield == 'relFecHorSalida') {
            if ((rowData.MinRetAnticipado) > 10)
                color = 'color: red;'
        }
        return '<div class="jqx-grid-cell-middle-align" style="margin-top: 6px;' + color + '">' + ((value == '') ? '' : moment(value)._d.toTimeString().substr(0, 5)) + '</div>';
    }

    $scope.anchoHora = '6%'
    $scope.data.liquidaciones = {};

    $scope.data.liquidaciones.settings =
    {

        altrows: true,
        width: '100%',
        height: 365,
        source: [],
        pageable: true,
        columns: [
                { dataField: 'ID', hidden: true },
                { text: 'Fecha', dataField: 'pacFecEntrada', cellsalign: 'center', width: '7%', renderer: $scope.columnrenderer },
                { text: 'Entrada P|Entrada Pactada', dataField: 'pacFecHorEntrada', width: $scope.anchoHora, cellsalign: 'center', cellsrenderer: $scope.FormatearHora, renderer: $scope.columnrenderer },
                { text: 'Salida P|Salida Pactada', dataField: 'pacFecHorSalida', width: $scope.anchoHora, cellsalign: 'center', cellsrenderer: $scope.FormatearHora, renderer: $scope.columnrenderer },
                { text: 'Movil P|Movil Pactado', dataField: 'MovilPacto', cellsalign: 'center', renderer: $scope.columnrenderer },
                { text: 'Entrada R|Entrada Real', dataField: 'relFecHorEntrada', width: $scope.anchoHora, cellsalign: 'center', cellsrenderer: $scope.FormatearHora, renderer: $scope.columnrenderer },
                { text: 'Salida R|Salida Real', dataField: 'relFecHorSalida', width: $scope.anchoHora, cellsalign: 'center', cellsrenderer: $scope.FormatearHora, renderer: $scope.columnrenderer },
                { text: 'Movil R|Movil Real', dataField: 'MovilReal', cellsalign: 'center', renderer: $scope.columnrenderer },
                { text: 'Horas T|Horas Trabajadas', dataField: 'HoraTrabajado', width: $scope.anchoHora, cellsalign: 'center', renderer: $scope.columnrenderer },
                { text: 'Horas N|Horas Nocturnas', dataField: 'HoraNocturno', width: $scope.anchoHora, cellsalign: 'center', renderer: $scope.columnrenderer },
                { text: 'Horas D|Horas Diurnas', dataField: 'HoraDiurno', width: $scope.anchoHora, cellsalign: 'center', renderer: $scope.columnrenderer },
                { text: 'Horas F|Horas Fin de Semana', dataField: 'HoraFinSemana', width: $scope.anchoHora, cellsalign: 'center', renderer: $scope.columnrenderer },
                { text: 'Horas I|Horas Incumplidas', dataField: 'HoraIncumplimiento', width: $scope.anchoHora, cellsalign: 'center', renderer: $scope.columnrenderer },
                { text: 'Aus. S/P|Ausencia sin premio', dataField: 'flgAusenciaSinPremio', cellsalign: 'center', renderer: $scope.columnrenderer },
                { text: 'Servicios|Cantidad de Servicios', dataField: 'cntServicios', cellsalign: 'center', renderer: $scope.columnrenderer },
                { text: 'Novedad|Tipo de novedad', dataField: 'TipoNovedad', cellsalign: 'center', renderer: $scope.columnrenderer },
                { text: 'Est|Estado', dataField: 'Status', width: '4%', cellsrenderer: $scope.crConfirmacion, renderer: $scope.columnrenderer }
        ],
        cellclick: function (event) {
            var args = event.args;
            var columnIndex = args.columnindex;
            var value = args.value;
            if (columnIndex == 16)//Conformidads
                $scope.Revisar(args.row.bounddata);
        }
    }

    $scope.Revisar = function (row) {
        checkSessionStorage();
        //GetConformidad
        $http({
            method: 'GET',
            url: '/EmpleadosLiquidaciones/GetConformidad?pItmLiq=' + row.ID
        }).then(function successCallback(response) {
            
            $scope.data.revisar = response.data;
            $scope.data.revisar.ID = row.ID;
            $scope.data.revisar.flgConforme = $scope.data.revisar.flgConforme.toString();
            $scope.data.revisar.TerLiqMotivoReclamoId = $scope.data.revisar.TerLiqMotivoReclamoId;

            if (response.data.EntradaReclamo == "")
                $scope.data.revisar.EntradaReclamo = response.data.Entrada;

            $scope.data.revisar.EntradaReclamo = new Date(response.data.EntradaReclamo);
            $scope.data.revisar.EntradaReclamo.setSeconds(0);

            if (response.data.SalidaReclamo == "")
                $scope.data.revisar.SalidaReclamo = response.data.Salida;

            $scope.data.revisar.SalidaReclamo = new Date(response.data.SalidaReclamo);
            $scope.data.revisar.SalidaReclamo.setSeconds(0);
            
            for (var i = 0; i < $scope.data.motivos.length; i++) {
                if ($scope.data.revisar.TerLiqMotivoReclamoId.toString() == $scope.data.motivos[i].ID.toString())
                    $scope.data.revisar.motivoSeleccionado = $scope.data.motivos[i];
                else
                    $scope.data.revisar.motivoSeleccionado = $scope.data.motivos[0];
            }
            $scope.data.revisar.TerLiqMotivoReclamoId = $scope.data.revisar.TerLiqMotivoReclamoId;

            $scope.data.revisar.titulo = 'Fecha: ' + row.pacFecEntrada + ', Horario guardados: ' + moment(row.relFecHorEntrada)._d.toTimeString().substr(0, 5) + ' a ' + moment(row.relFecHorSalida)._d.toTimeString().substr(0, 5);
            $scope.data.revisar.motivo = 'motivo';
            $scope.data.revisar.permiteReclamar = ($scope.acceso == 1 && row.Status == 1);
            $scope.data.revisar.flgRespuesta = $scope.data.revisar.flgRespuesta.toString();
            $scope.data.revisar.conformidad = parseInt(row.Status);
            $scope.data.revisar.deshabilitarEsperado = 0;
            $scope.data.revisar.deshabilitarDiferencia = 0;
            
            $('#popupRevisar').modal('show');
        }, function errorCallback(response) {
            console.log(response);
        });
    }

    $scope.GuardarReclamo = function () {
        
        if ($scope.acceso == 1)
            $scope.SetConformidad();
        else
            $scope.SetReclamo();
    }


    $scope.SetConformidad = function () {
        checkSessionStorage();
        var pItm = $scope.data.revisar.ID;
        var pRpl = 0;
        var pCnf = $scope.data.revisar.flgConforme;
        var pMot = $scope.data.revisar.TerLiqMotivoReclamoId;
        //var pHEnt = $scope.data.revisar.EntradaReclamo.getHours();//Hora ...
        var pHEnt = $(popupGrdEntradaReclamo).val().split(':')[0];
        var pMEnt = $(popupGrdEntradaReclamo).val().split(':')[1];
        var pHSal = $(popupGrdSalidaReclamo).val().split(':')[0];
        var pMSal = $(popupGrdSalidaReclamo).val().split(':')[1];
        var pObs = $scope.data.revisar.Observaciones;
        $http({
            method: 'GET',
        url: '/EmpleadosLiquidaciones/SetConformidad?pItm=' + pItm + '&pCnf=' + pCnf + '&pMot=' + pMot + '&pHEnt=' + pHEnt + '&pMEnt=' + pMEnt + '&pHSal=' + pHSal + '&pMSal=' + pMSal + '&pObs=' + pObs
        }).then(function successCallback(response) {
            if (response.data.Resultado == 1) {
                $('#popupRevisar').modal('toggle');
                $scope.GetData()
            }
            else
                alert(response.data.AlertaError);
        }, function errorCallback(response) {
            console.log(response);
        });
    }
    $scope.SetReclamo = function () {
        checkSessionStorage();
        var pItm = $scope.data.revisar.ID;
        var pSta = $scope.data.revisar.flgRespuesta;
        var pRta = $scope.data.revisar.Respuesta;
        $http({
            method: 'GET',
            url: '/EmpleadosLiquidaciones/SetRespuesta?pItm=' + pItm + '&pSta=' + pSta + '&pRta=' + pRta
        }).then(function successCallback(response) {
            if (response.data.Resultado == 1) {
                $('#popupRevisar').modal('toggle');
                $scope.GetData()
            }
            else
                alert(response.data.AlertaError);
        }, function errorCallback(response) {
            console.log(response);
        });
    }

    $("#popupOrdenServicio").on('hidden.bs.modal', function () { $scope.GetData(); })
});
