app.controller('liquidacionesCtrl', function ($scope, $http, $window) {

    //variables
    //$scope.onlyNumber = /^\d+$/; //"/^[0-9]+(\.[0-9]{1,2})?$/";
    $scope.solapa = 'prestaciones';
    _this = this;
    $scope.esEmpresa = 0;
    $scope.acceso= 0;
    $scope.filter = {
        pLiqId: 0,
        periodo: '',
        dia: '0',
        estado: '0',
        tipo: '0'
    };
    $scope.ordenServicioSettings = {};
    $scope.ordenServicio = {};
    $scope.data = {
        periodos: [],
        dias: [],
        estados: [],
        moviles: [],
        revisar: {},
        resumenDetalle: {},
        motivos: [],
        calculo: {},
        detalle: {
            Importe: 0,
            CoPago: 0,
            Sexo: '',
            Edad: ''
        },
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
        Importe: {
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
        Descripcion: 'Reclamados',
        ID: '1'
    },
    {
        Descripcion: 'Pendientes',
        ID: '2'
    },
    {
        Descripcion: 'Resueltos',
        ID: '3'
    }
    ];

    $scope.data.tipos = [
    {
        Descripcion: 'Todas',
        ID: '0'
    },
    {
        Descripcion: 'Local',
        ID: '1'
    },
    {
        Descripcion: 'Interior',
        ID: '2'
    }
    ];


    $scope.Reliquidar = function () {
        $http({
            method: 'GET',
            url: '/liquidaciones/Reliquidar?pLiqId=' + $scope.filter.pLiqId
        }).then(function successCallback(response) {
            GetData();
        }, function errorCallback(response) {
            console.log(response);
        });
    }
    $scope.EstadoChange = function () {
        $scope.GetMoviles();
    };
    $scope.TipoChange = function () {
        $scope.GetMoviles();
    };
    $scope.PeriodoChange = function () {
        $scope.GetDias();
        $scope.GetMoviles($("#selMoviles option:selected" ).text());
    };
    //GetData (datos de la primer solapa)
    $scope.GetData = function () {
        checkSessionStorage();
        if ($scope.filter.periodo <= 0) {
            alert('Debe ingresar el periodo.')
            return;
        }
        if ($scope.filter.pLiqId <= 0) {
            alert(($scope.esEmpresa) ? 'Debe ingresar la empresa' : 'Debe ingresar el móvil.')
            return;
        }
        $http({
            method: 'GET',
            url: '/liquidaciones/GetIncidentes?pMov=' + $scope.filter.pLiqId + '&pPer=' + $scope.filter.periodo + '&pDia=' + $scope.filter.dia + '&pEst=' + $scope.filter.estado
        }).then(function successCallback(response) {
            var data = {
                localdata: response.data,
                datafields: [
                    { name: 'IncidenteId', type: 'string' },
                    { name: 'ID', type: 'string' },
                    { name: 'FecIncidente', type: 'string' },
                    { name: 'NroIncidente', type: 'string' },
                    { name: 'Iva', type: 'float' },
                    { name: 'IIBB', type: 'string' },
                    { name: 'Nombre', type: 'string' },
                    { name: 'LocalidadDesde', type: 'string' },
                    { name: 'LocalidadHasta', type: 'string' },
                    { name: 'Kilometros', type: 'string' },
                    { name: 'Retorno', type: 'string' },
                    { name: 'Turno', type: 'string' },
                    { name: 'TpoEspera', type: 'string' },
                    { name: 'ConceptoFacturacionId', type: 'string' },
                    { name: 'CoPago', type: 'float' },
                    { name: 'Deriva', type: 'string' },
                    { name: 'Importe', type: 'float' },
                    { name: 'Rem', type: 'string' },
                    { name: 'Conf', type: 'string' },
                    { name: 'Rev', type: 'string' },
                    { name: 'ArchivoOrden', type: 'string' }
                ],
                datatype: "array"
            }
            $scope.data.incidentes.settings.source = new $.jqx.dataAdapter(data);
            $('#incidentesGrid').jqxGrid((($scope.esEmpresa) ? 'showcolumn' : 'hidecolumn'), 'Orden');
        }, function errorCallback(response) {
            console.log(response);
        });
        $http({
            method: 'GET',
            url: '/liquidaciones/GetAsistencia?pMov=' + $scope.filter.pLiqId + '&pPer=' + $scope.filter.periodo
        }).then(function successCallback(response) {
            var data = {
                localdata: response.data,
                datafields: [
                    { name: 'FecMovimiento', type: 'string' },
                    { name: 'DiaSemana', type: 'string' },
                    { name: 'pacFecHorInicio', type: 'string' },
                    { name: 'pacFecHorFinal', type: 'string' },
                    { name: 'HorasPactadas', type: 'int' },
                    { name: 'relFecHorInicio', type: 'string' },
                    { name: 'minTarde', type: 'int' },
                    { name: 'Tarde', type: 'string' },
                    { name: 'relFecHorFinal', type: 'string' },
                    { name: 'minAnticipado', type: 'int' },
                    { name: 'Anticipado', type: 'string' },
                    { name: 'MotivoDescuento', type: 'string' },
                    { name: 'virEvlDescontable', type: 'int' },
                    { name: 'virTpoDescontable', type: 'string' },
                    { name: 'ConceptoFacturacionId', type: 'string' },
                    { name: 'HorasTrabajadas', type: 'int' }
                ],
                datatype: "array"
            }
            $scope.data.asistencia.settings.source = new $.jqx.dataAdapter(data);
        }, function errorCallback(response) {
            console.log(response);
        });
        $http({
            method: 'GET',
            url: '/liquidaciones/GetResumen?pMov=' + $scope.filter.pLiqId + '&pPer=' + $scope.filter.periodo
        }).then(function successCallback(response) {
            $scope.data.resumen = response.data;
        }, function errorCallback(response) {
            console.log(response);
        });
    }
    $http({
        method: 'GET',
        url: '/liquidaciones/EsEmpresa'
    }).then(function successCallback(response) {
        $scope.esEmpresa = (response.data.toString() == "1");
    }, function errorCallback(response) {
        console.log(response);
    });
    $http({
        method: 'GET',
        url: '/liquidaciones/GetAcceso'
    }).then(function successCallback(response) {
        $scope.acceso = response.data.toString();
        if ($scope.acceso == 0) {
            sessionStorage.currentUser = undefined;
            console.log(response);
            setTimeout(function () {
                window.location.href = '/login';
            }, 1000);
        }
        else //Cargo los combos
            $scope.GetPeriodos();
            
    }, function errorCallback(response) {
        console.log(response);
    });
    
    $http({
        method: 'GET',
        url: '/liquidaciones/GetMotivosReclamos'
    }).then(function successCallback(response) {
        $scope.data.motivos = response.data;
    }, function errorCallback(response) {
        console.log(response);
    });
    //Obtengo los periodos para el combo
    $scope.GetPeriodos = function () {
        debugger;
        var date = new Date();
        $scope.data.periodos = [];

        var meses = $scope.acceso == 3 ? 6 : 4;

        for (var i = 0; i < meses; i++) {
            //TODO: Fix porque no hay datos previos, eliminar en 2020
            if (date.getMonth() > 2 && date.getFullYear() > 2018) {
                $scope.data.periodos.push({
                    Descripcion: (date.getMonth() < 9 ? '0' : '') + (date.getMonth() + 1).toString() + '/' + date.getFullYear().toString(),
                    ID: date.getFullYear().toString() + (date.getMonth() < 9 ? '0' : '') + (date.getMonth() + 1).toString()
                });
            }
            date.setMonth(date.getMonth() - 1);
        }
    }
    //Obtengo los Móviles
    $scope.GetMoviles = function (pPeriodoDesOriginal) {
        $http({
            method: 'GET',
            url: '/liquidaciones/GetMoviles?pPer=' + $scope.filter.periodo + '&pEst=' + $scope.filter.estado + '&pTip=' + $scope.filter.tipo
        }).then(function successCallback(response) {
            $scope.data.moviles = response.data;
            if (pPeriodoDesOriginal != null) {
                var found = false;
                for (var i = 0; i < $scope.data.moviles.length; i++) {
                    if ($scope.data.moviles[i].Descripcion == pPeriodoDesOriginal) {
                        found = true;
                        $scope.filter.pLiqId = $scope.data.moviles[i].ID;
                        break;
                    }
                }
                if (!found)
                    $scope.filter.pLiqId = response.data[0].ID;
            }
            else
                $scope.filter.pLiqId = response.data[0].ID;
            //si obtengo los móviles puedo realizar la consulta.
            $scope.GetData();
        }, function errorCallback(response) {
            console.log(response);
        });
    }

    $scope.CargarDiferencia = function (campoEntrada, e) {
        if (campoEntrada == 'DIFERENCIA') 
            $scope.data.revisar.Importe = (parseFloat($scope.data.revisar.virImpLiquidado) + parseFloat($scope.data.revisar.virImpDiferencia)).toFixed(2);
        if (campoEntrada == 'ESPERADO') 
            $scope.data.revisar.virImpDiferencia = (parseFloat($scope.data.revisar.Importe) - parseFloat($scope.data.revisar.virImpLiquidado)).toFixed(2);
    }
    //Obtengo los días para el combo
    //Cargo el combo de dias segun el mes seleccionado, si es el mes actual ingreso hasta el día de la fecha.
    $scope.GetDias = function () {
        $scope.data.dias = [
            {
                Descripcion: 'Todos',
                ID: '0'
            }
        ];
        var periodoSeleccionado = $scope.filter.periodo;
        var date = new Date();
        var maxDay = 0;
        if (date.getMonth() != parseInt(periodoSeleccionado.substr(4, 2)) - 1) {
            date.setDate(1);
            date.setMonth(parseInt(periodoSeleccionado.substr(4, 2)));
            date.setDate(date.getDate() - 1);
        }
        maxDay = date.getDate();
        for (var i = 1; i <= maxDay; i++) {
            $scope.data.dias.push(
                {
                    Descripcion: (i < 10 ? '0' : '') + i.toString(),
                    ID: i.toString()
                }
            );
        }
    };

    //$scope.filter.periodo = $scope.data.periodos[0].ID;
    $scope.filter.periodo = new Date().getFullYear().toString() + ((new Date().getMonth() < 9) ? '0' : '') + (new Date().getMonth() + 1).toString(); // $scope.data.periodos[0].ID;
    $scope.GetDias();
    $scope.GetMoviles();
    if ($scope.data.moviles.length > 0)
        $scope.filter.movil = $scope.data.moviles[0].ID;

    $scope.crOrdenServicio = function (row, columnfield, value, defaulthtml, columnproperties) {
        var icon = (value==1) ? '' : 'glyphicon-upload verde';
        return '<div style="cursor: pointer;"><span style="margin-left:34%;line-height:33px;" class="glyphicon ' + icon + ' icon-right-margin big-icon"></span></div>'
    }
    $scope.crConfirmacion = function (row, columnfield, value, defaulthtml, columnproperties) {
        var icon = '';
        //return '<div style="' + (columnfield == 'Facturado' ? '' : 'cursor: pointer;') + '"><span style="margin-left:34%;line-height:33px;" class="' + icon + ' ' + color + ' icon-right-margin big-icon"></span></div>';

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
    $scope.FormatearHora = function (row, columnfield, value, defaulthtml, columnproperties) {
        var color = '';
        if (columnfield == 'virTpoDescontable' || columnfield == 'Anticipado' || columnfield == 'Tarde') {
            if (moment(value)._d.getHours() > 0 || moment(value)._d.getMinutes() > 15)
                color = 'color: red;'
        }
        return '<div class="jqx-grid-cell-middle-align" style="margin-top: 6px;' + color + '">' + ((value == '') ? '' : moment(value)._d.toTimeString().substr(0, 5)) + '</div>';
    }
    $scope.data.asistencia = {};
    $scope.anchoHora = '6%'
    $scope.data.asistencia.settings =
    {
        altrows: true,
        width: '100%',
        height: 365,
        source: [],
        pageable: true,
        columns: [
            { text: 'Fecha', dataField: 'FecMovimiento', width: '7%', cellsalign: 'center', cellsrenderer: $scope.FormatearFecha, renderer: $scope.columnrenderer },
            { text: 'Día', dataField: 'DiaSemana', width: '3%', cellsalign: 'center', renderer: $scope.columnrenderer },
            { text: 'Inicio Pac.|Inicio Pactado', dataField: 'pacFecHorInicio', width: $scope.anchoHora, cellsalign: 'center', cellsrenderer: $scope.FormatearHora, renderer: $scope.columnrenderer },
            { text: 'Final Pac.', dataField: 'pacFecHorFinal', width: $scope.anchoHora, cellsalign: 'center', cellsrenderer: $scope.FormatearHora, renderer: $scope.columnrenderer },
            { text: 'Horas Pac.', dataField: 'HorasPactadas', cellsformat: 'd2', width: '7%', cellsalign: 'right', renderer: $scope.columnrenderer },
            { text: 'Inicio Rea.', dataField: 'relFecHorInicio', width: $scope.anchoHora, cellsalign: 'center', cellsrenderer: $scope.FormatearHora, renderer: $scope.columnrenderer },
            { dataField: 'minTarde', hidden: true },
            { text: 'Tarde', dataField: 'Tarde', width: $scope.anchoHora, cellsalign: 'center', cellsrenderer: $scope.FormatearHora, renderer: $scope.columnrenderer },
            { text: 'Final Rea.', dataField: 'relFecHorFinal', width: $scope.anchoHora, cellsalign: 'center', cellsrenderer: $scope.FormatearHora, renderer: $scope.columnrenderer },
            { dataField: 'minAnticipado', hidden: true },
            { text: 'Anticip', dataField: 'Anticipado', width: $scope.anchoHora, cellsalign: 'center', cellsrenderer: $scope.FormatearHora, renderer: $scope.columnrenderer },
            { text: 'Condicional', dataField: 'MotivoDescuento', cellsalign: 'center', renderer: $scope.columnrenderer },
            { dataField: 'virEvlDescontable', hidden: true },
            { text: 'Tiempo', dataField: 'virTpoDescontable', width: $scope.anchoHora, cellsalign: 'center', cellsrenderer: $scope.FormatearHora, renderer: $scope.columnrenderer },
            { text: 'Horas Tra.', dataField: 'HorasTrabajadas', cellsformat: 'd2', width: '7%', cellsalign: 'right', renderer: $scope.columnrenderer }
        ],
        cellclick: function (event) {
            var args = event.args;
            var columnindex = args.columnindex;
            var value = args.value;
            if (columnindex == 3)
                $scope.Detalle(args.row.bounddata);
            if (columnindex == 18)
                $scope.Revisar(args.row.bounddata);
        }
    }

    $scope.data.incidentes = {};
    $scope.data.incidentes.settings =
    {
        altrows: true,
        width: '100%',
        height: 365,
        source: [],
        pageable: true,
        columns: [
            { dataField: 'ID', hidden: true },
            { dataField: 'IncidenteId', hidden: true },
            { text: 'Fecha', dataField: 'FecIncidente', width: '5%', cellsalign: 'center', renderer: $scope.columnrenderer },
            { text: 'Incid', dataField: 'NroIncidente', width: '4%', cellsalign: 'center', cellsrenderer: $scope.NroIncidente, renderer: $scope.columnrenderer },
            { text: 'IVA', dataField: 'Iva', width: '4%', cellsalign: 'center', renderer: $scope.columnrenderer },
            { text: 'IIBB', dataField: 'IIBB', width: '4%', cellsalign: 'center', renderer: $scope.columnrenderer },
            { text: 'Nombre', dataField: 'Nombre', cellsalign: 'left', renderer: $scope.columnrenderer },
            { text: 'Desde', dataField: 'LocalidadDesde', width: '5%', cellsalign: 'center', renderer: $scope.columnrenderer },
            { text: 'Hasta', dataField: 'LocalidadHasta', width: '5%', cellsalign: 'center', renderer: $scope.columnrenderer },
            { name:'Kilometros', text: 'KM', dataField: 'Kilometros', width: '5%', cellsalign: 'center', renderer: $scope.columnrenderer },
            { text: 'Ret|Retorno', dataField: 'Retorno', width: '4%', cellsalign: 'center', renderer: $scope.columnrenderer },
            { text: 'Tur|Turno', dataField: 'Turno', width: '4%', cellsalign: 'center', renderer: $scope.columnrenderer },
            { text: 'Esp|Espera', dataField: 'TpoEspera', width: '4%', cellsalign: 'center', renderer: $scope.columnrenderer },
            { text: 'Concepto', dataField: 'ConceptoFacturacionId', width: '7%', cellsalign: 'center', renderer: $scope.columnrenderer },
            { text: 'CoPago', dataField: 'CoPago', cellsformat: 'c2', width: '7%', cellsalign: 'right', renderer: $scope.columnrenderer },
            { text: 'Deriva', dataField: 'Deriva', width: '5%', cellsalign: 'center', renderer: $scope.columnrenderer },
            { text: 'Importe', dataField: 'Importe', cellsformat: 'c2', width: '8%', cellsalign: 'right', renderer: $scope.columnrenderer },
            { text: 'Rem|Remito', dataField: 'Rem', width: '4%', cellsalign: 'center', renderer: $scope.columnrenderer },
            { text: 'Est|Estado', dataField: 'Conf', width: '4%', cellsrenderer: $scope.crConfirmacion, renderer: $scope.columnrenderer },
            { text: 'Orden', dataField: 'Orden', width: '4%', cellsrenderer: $scope.crOrdenServicio, renderer: $scope.columnrenderer },
            { dataField: 'Rev', hidden: true }
        ],
        cellclick: function (event) {
            var args = event.args;
            var columnIndex = args.columnindex;
            var value = args.value;
            if (columnIndex == 3)//Detalle
                $scope.Detalle(args.row.bounddata);
            if (columnIndex == 18)//Conformidad
                $scope.Revisar(args.row.bounddata);
            if (columnIndex == 19) //Subir Orden Servicio
                $scope.OrdenServicio(args.row.bounddata);
        }
    }
    $scope.ordenServicioSettings = {
        width: '90%',
        uploadUrl: '/liquidaciones/UploadOrdenServicio?id=' + $scope.ordenServicio.ArchivoOrden,
        autoUpload: true,
        fileInputName: 'file',
        uploadEnd: function (event) {
            if (event.args.response.includes("false")) {
                alert("Debe seleccionar una imagen jpg valida.");
                return;
            }
            checkSessionStorage();
            $http({
                method: 'GET',
                url: '/liquidaciones/GetSession?pVar=Error'
            }).then(function successCallback(response) {
                //Obtener el nombre de archivo (GetArchivoOrden)
                if (response.data == "") {
                    $http({
                        method: 'GET',
                        url: '/liquidaciones/GetSession?pVar=ArchivoOrden'
                    }).then(function successCallback(response) {
                        $scope.ordenServicio.rutaArchivo = 'liquidacionesImagenes\\' + response.data + '?time=' + new Date().getTime();
                        $scope.ordenServicio.nombre = event.args.file;
                        $scope.ordenServicio.existeArchivo = true;
                    }, function errorCallback(response) {
                        console.log(response);
                    });
                }
                else {
                    alert(response.data);
                }
            }, function errorCallback(response) {
                console.log(response);
            });
        }
    };
    $scope.OrdenServicio = function (row) {
        checkSessionStorage();
        $scope.ordenServicio = {};
        $scope.ordenServicio.IncidenteId = row.IncidenteId;
        $scope.ordenServicio.ArchivoOrden = row.ArchivoOrden;
        //$scope.ordenServicioSettings.fileInputName = 'tst.t'
        $scope.ordenServicioSettings.uploadUrl = '/liquidaciones/UploadOrdenServicio?ArchivoOrden=' + $scope.ordenServicio.ArchivoOrden + '&pLiqId=' + $scope.filter.pLiqId + '&IncidenteId=' + row.IncidenteId;
        $('#fileUpload').jqxFileUpload({
            width: '90%',
            uploadUrl: $scope.ordenServicioSettings.uploadUrl,
            autoUpload: true,
            fileInputName: 'file',
            accept: '.jpg',
            browseTemplate: 'primary'
        });
        $('#fileUpload').jqxFileUpload('render');
        //GetConformidad
        $scope.ordenServicio.rutaArchivo = '/liquidacionesImagenes/' + $scope.ordenServicio.ArchivoOrden + '?time=' + new Date().getTime();
        //Verifico si existe el archivo
        if ($scope.ordenServicio.ArchivoOrden == "") {
            $scope.ordenServicio.existeArchivo = false;
        }
        else {
            var http = new XMLHttpRequest();
            http.open('HEAD', $scope.ordenServicio.rutaArchivo, false);
            http.send();
            $scope.ordenServicio.existeArchivo = (http.status != 404);
        }
        //Abro formulario
        $('#popupOrdenServicio').modal('show');
    }
    $scope.Revisar = function (row) {
        checkSessionStorage();
        $http({
            method: 'GET',
            url: '/liquidaciones/GetConformidad?pItmLiq=' + row.ID
        }).then(function successCallback(response) {
            $scope.data.revisar = response.data;
            //sino lo convierto a string no lo reconoce el radio
            if ($scope.data.revisar.virImpLiquidado == 0)
                $scope.data.revisar.virImpLiquidado = parseFloat(row.Importe).toFixed(2);
            $scope.data.revisar.ID = row.ID;
            $scope.data.revisar.flgConforme = $scope.data.revisar.flgConforme.toString();
            $scope.data.revisar.TerLiqMotivoReclamoId = $scope.data.revisar.TerLiqMotivoReclamoId;
            for (var i = 0; i < $scope.data.motivos.length; i++) {
                if ($scope.data.revisar.TerLiqMotivoReclamoId.toString() == $scope.data.motivos[i].ID.toString())
                    $scope.data.revisar.motivoSeleccionado = $scope.data.motivos[i];
            }
            $scope.data.revisar.TerLiqMotivoReclamoId = $scope.data.revisar.TerLiqMotivoReclamoId;

            $scope.data.revisar.titulo = 'Conformidad con incidente: ' + row.NroIncidente + ' (' + row.FecIncidente + ')';
            $scope.data.revisar.motivo = 'motivo';
            $scope.data.revisar.permiteReclamar = ($scope.acceso == 1 && row.Conf == 1);
            $scope.data.revisar.flgRespuesta = $scope.data.revisar.flgRespuesta.toString();
            $scope.data.revisar.conformidad = parseInt(row.Conf);
            $scope.data.revisar.deshabilitarEsperado = 0;
            $scope.data.revisar.deshabilitarDiferencia = 0;

            $('#popupRevisar').modal('show');
        }, function errorCallback(response) {
            console.log(response);
        });
    }

    //POPUP DETALLE
    $scope.Detalle = function (row) {
        checkSessionStorage();
        $http({
            method: 'GET',
            url: '/liquidaciones/GetIncidenteDetalle?pItmLiq=' + row.ID + '&pLiqId=' + $scope.filter.pLiqId + '&pInc=' + row.IncidenteId
        }).then(function successCallback(response) {
            $scope.data.detalle = response.data;
            $scope.data.detalle.FecIncidente = moment(response.data.FecIncidente).utc().format('DD/MM/YYYY');
            $scope.data.detalle.HorDespacho = moment(response.data.HorDespacho).utc().format('HH:mm');
            $scope.data.detalle.HorLlegada = moment(response.data.HorLlegada).utc().format('HH:mm');
            $('#popupDetalle').modal('show');
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
        var pMot = $scope.data.revisar.TerLiqMotivoReclamoId
        var pDif = $scope.data.revisar.virImpDiferencia;
        var pLiq = $scope.data.revisar.virImpLiquidado;
        var pNue = $scope.data.revisar.Importe;
        var pObs = $scope.data.revisar.Observaciones;
        $http({
            method: 'GET',
            url: '/liquidaciones/SetConformidad?pItm= ' + pItm + '&pRpl=' + pRpl + '&pCnf=' + pCnf + '&pMot=' + pMot + '&pDif=' + pDif + '&pLiq=' + pLiq + '&pNue=' + pNue + '&pObs=' + pObs
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
        $http({//(string pItm, long pSta, string pRta)
            method: 'GET',
            url: '/liquidaciones/SetRespuesta?pItm= ' + pItm + '&pSta=' + pSta + '&pRta=' + pRta
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
    $scope.ResumenDetalle = function (item) {
        checkSessionStorage();
        $http({
            method: 'GET',
            url: '/liquidaciones/GetResumenDetalle?pLiqId=' + $scope.filter.pLiqId + '&pLiqMovId=' + item.LiquidacionMovilId + '&link=' + item.Link
        }).then(function successCallback(response) {
            $scope.data.resumenDetalle.list = response.data;
            $scope.data.resumenDetalle.item = item;
            $('#popupResumenDetalle').modal('show');
        }, function errorCallback(response) {
            console.log(response);
        });
    }
    $scope.importeOnBlur = function (campoEntrada) {
        if (campoEntrada == 'DIFERENCIA') 
            $scope.data.revisar.virImpDiferencia = parseFloat($scope.data.revisar.virImpDiferencia).toFixed(2)
        if (campoEntrada == 'ESPERADO') 
            $scope.data.revisar.Importe = parseFloat($scope.data.revisar.Importe).toFixed(2)
    }
    $("#popupOrdenServicio").on('hidden.bs.modal', function () { $scope.GetData(); })

    $('#exportarAExcel').click(function () {
        //Funcion en main.js
        var data = $("#incidentesGrid").jqxGrid('exportdata', 'json');
        downloadExcel(data, "prestaciones");

    });
});
