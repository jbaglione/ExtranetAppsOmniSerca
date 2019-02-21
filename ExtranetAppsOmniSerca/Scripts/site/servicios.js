$("#btnConsultarServicios").jqxButton({ width: '100', theme: 'bootstrap', height: '27' });

// --> Seteo dropdownlist para seleccionar periodo en la grilla de servicios
// --> Uso mismo source que filtro de periodo de guardias

$("#ftrPeriodoServicios").jqxDropDownList({
    selectedIndex: 2, source: setFtrPeriodoGuardias(), displayMember: "Descripcion",
    valueMember: "Periodo", width: '110%', dropDownHeight: 80, height: 25, theme: 'bootstrap'
});

// --> Seteo dropdownlist para seleccionar estado de guardias

$("#ftrEstadoServicios").jqxDropDownList({
    selectedIndex: 0, source: vEstadosGuardias, displayMember: "Descripcion",
    valueMember: "ID", width: '110%', dropDownHeight: 110, height: 25, theme: 'bootstrap'
});

// --> Seteo dropdownlist para seleccionar periodo,guardias en la grilla de servicios
// --> Uso mismo source que filtro de dia,guardias de guardias
$("#ftrDiaServicios").jqxDropDownList({ selectedIndex: 0, source: srcFtrDiaGuardias, width: '110%', height: 25, theme: 'bootstrap' });

$("#ftrMedicoServicios").jqxDropDownList({
    source: getSourceFiltroMedicosServ(), displayMember: "Nombre", selectedIndex: 0,
    valueMember: "UsuarioID", width: '110%', dropDownHeight: 150, dropDownWidth: 320, height: 25, theme: 'bootstrap'
});

$("#ftrCoordServicios").jqxDropDownList({
    selectedIndex: 0, source: setSrcFtrCoordServicios(), displayMember: "Descripcion",
    valueMember: "ID", width: '110%', dropDownHeight: 80,dropDownHeight: 125, height: 25, theme: 'bootstrap'
});

/*********************************************************************************************************/

// --> Seteo dropdownlist para seleccionar medico en la grilla de guardias

function getSourceFiltroMedicosServ() {

    var urlMedicoServ = 'Medicos/getFiltroMedicos?usr_id=' + usr_id_medico +
                            '&selPeriodo=' + getSelectedPeriodoServ() + '&selEstado=' + getSelectedEstadoServ();

    if (tipoAcceso == 1) {
        urlMedicoServ += '&esMedico=1';
    }

    var srcFtrMedicoServ = {
        datatype: "json",
        datafields: [
            { name: 'UsuarioID' },
            { name: 'Nombre' }
        ],
        url: urlMedicoServ,
        async: false
    };

    var dtFtrMedicoServ = new $.jqx.dataAdapter(srcFtrMedicoServ);

    return dtFtrMedicoServ;

}

// --> Seteo dropdownlist para seleccionar coordinacion en la grilla de servicios

function setSrcFtrCoordServicios() {

    var srcFtrCoordServicios = {
        datatype: "json",
        datafields: [
            { name: 'ID' },
            { name: 'Descripcion' }
        ],
        url: 'Medicos/getFiltroCoordinaciones?usr_id=' + getSelectedMedicoServ(),
        async: false
    };

    var dtFtrCoordServicios = new $.jqx.dataAdapter(srcFtrCoordServicios);

    return dtFtrCoordServicios;

}

$('#ftrMedicoServicios').on('select', function (event) {

    $('#ftrCoordServicios').jqxDropDownList({ source: setSrcFtrCoordServicios() });

});

$('#ftrEstadoServicios, #ftrPeriodoServicios').on('select', function (event) {

    $('#ftrMedicoServicios').jqxDropDownList({ source: getSourceFiltroMedicosServ() });
    $('#ftrCoordServicios').jqxDropDownList({ source: setSrcFtrCoordServicios() });

});

/*********************************************************************************************************/

// -> Funciones para obtener el periodo y el dia de los filtros en la grilla de guardias

function getSelectedPeriodoServ() {
    return $("#ftrPeriodoServicios").jqxDropDownList('getSelectedItem').value;
}

function getSelectedDiaServ() {
    return $('#ftrDiaServicios').jqxDropDownList('selectedIndex');
}

function getSelectedCoordServ() {
    var result = $('#ftrCoordServicios').jqxDropDownList('getSelectedItem');
    if (result != null && result != undefined)
        return result.value;
    else null;
}

function getSelectedMedicoServ() {
   
    var result = $("#ftrMedicoServicios").jqxDropDownList('getSelectedItem');
    if (result != null && result != undefined)
        return result.value;
    else null;

}

function getSelectedEstadoServ() {
    return $("#ftrEstadoServicios").jqxDropDownList('getSelectedItem').value;
}

function getDescriptionSelectedPeriodoServ() {
    return $("#ftrPeriodoServicios").jqxDropDownList('getSelectedItem').label;
}

function getDescriptionSelectedMedicoServ() {
    return $("#ftrMedicoServicios").jqxDropDownList('getSelectedItem').label;
}

/*********************************************************************************************************/

// --> Seteo datafields de grilla de servicios

var dtFieldsServicios = [
                        { name: 'IncidenteID', type: 'string' },
                        { name: 'Fecha', type: 'string' },
                        { name: 'NroInc', type: 'string' },
                        { name: 'Iva', type: 'string' },
                        { name: 'Paciente', type: 'string' },
                        { name: 'Localidad', type: 'string' },
                        { name: 'Cdn', type: 'string' },
                        { name: 'Tarifa', type: 'string' },
                        { name: 'Dia', type: 'string' },
                        { name: 'Tur', type: 'string' },
                        { name: 'Grado', type: 'string' },
                        { name: 'CoPago', type: 'number' },
                        { name: 'Importe', type: 'number' },
                        { name: 'MesDia', type: 'string' },
                        { name: 'Estado', type: 'number' }
];

// --> Seteo source grilla de servicios

function getSourceGridServicios() {

    var srcGridServicios = {
        datatype: "json",
        datafields: dtFieldsServicios,
        url: 'Medicos/GetServicios',
        data: {
            periodo: getSelectedPeriodoServ(),
            dia: getSelectedDiaServ(),
            coordinacion: getSelectedCoordServ(),
            medico: getSelectedMedicoServ(),
            estado: getSelectedEstadoServ()
        }
    };

    var dtGridServicios = new $.jqx.dataAdapter(srcGridServicios);

    return dtGridServicios;
}

var dtGridServicios = getSourceGridServicios();

// --> Seteo cellsrenderers de grilla de servicios, para poner en rojo valores dependiendo si llegaron tarde, etc.

var crConfirmacionServicio = function (row, columnfield, value, defaulthtml, columnproperties) {
    var retVal = "";

    switch (value) {
        case 0:
            // --> No esta conforme
            retVal = '<a href="javascript:showPopupServicio()" style="margin-left:34%;line-height:33px"><span class="glyphicon glyphicon-remove-circle rojo icon-right-margin big-icon "></span></a>';
            break;
        case 1:
            // --> Está conforme
            retVal = '<a href="javascript:showPopupServicio()" style="margin-left:34%;line-height:33px"><span class="glyphicon glyphicon-ok-circle verde icon-right-margin big-icon "></span></a>';
            break;
        case 2:
            // --> No está conforme y recibió una respuesta
            retVal = '<a href="javascript:showPopupServicio()" style="margin-left:34%;line-height:33px"><span class="fa fa-exclamation-circle color-yellow icon-right-margin big-icon"></span></a>';
            break;
        case 3:
            // --> Reclamo aceptado
            retVal = '<a href="javascript:showPopupServicio()" style="margin-left:34%;line-height:33px"><span class="glyphicon glyphicon-ok-circle azul icon-right-margin big-icon "></span></a>';
            break;
        case 4:
            // --> Reclamo no aceptado
            retVal = '<a href="javascript:showPopupServicio()" style="margin-left:34%;line-height:33px"><span class="glyphicon glyphicon-remove-circle naranja icon-right-margin big-icon "></span></a>';
            break;
    }

    return retVal;

}


// --> Seteo columnas de la grilla de servicios

var colGridServicios =
             [
              { text: 'ID', datafield: 'IncidenteID', hidden: true },
              { text: 'MesDia', datafield: 'MesDia', hidden: true },
              { text: 'Fecha', datafield: 'Fecha', width: '7%' },
              { text: 'Inc', datafield: 'NroInc', width: '7%', cellsalign: 'center' },
              { text: 'Iva', datafield: 'Iva', width: '7%', cellsalign: 'center' },
              { text: 'Paciente', datafield: 'Paciente', width: '17%', cellsalign: 'center' },
              { text: 'Loc', datafield: 'Localidad', width: '7%', cellsalign: 'center' },
              { text: 'Cdn', datafield: 'Cdn', width: '7%', cellsalign: 'center' },
              { text: 'Tarifa', datafield: 'Tarifa', width: '7%', cellsalign: 'center' },
              { text: 'D&iacute;a', datafield: 'Dia', width: '7%', cellsalign: 'center' },
              { text: 'Tur', datafield: 'Tur', width: '7%', cellsalign: 'center' },
              { text: 'Grado', datafield: 'Grado', width: '7%', cellsalign: 'center' },
              {
                  text: 'CoPago', datafield: 'CoPago', width: '13%', cellsalign: 'right', cellsformat: 'c2', aggregates: ['sum'],
                  aggregatesrenderer: function (aggregates, column, element, summaryData) {
                      var renderstring = "";
                      $.each(aggregates, function (key, value) {
                          var name = key == 'sum' ? 'Total' : 'Avg';
                          var color = 'green';
                          renderstring += '<div style="color:green;font-weight:bold; position: relative; margin: 5px; text-align: right; overflow: hidden;">' + name + ': ' + value + '</div>';
                      });
                      return renderstring;
                  }
              },
              { text: 'Importe', datafield: 'Importe', width: '9%', cellsalign: 'right', cellsformat: 'c2', hidden: true },
              { text: 'Est', datafield: 'Estado', width: '7%', cellsalign: 'center', cellsRenderer: crConfirmacionServicio }
             ];


// --> Seteo objeto de la grilla de servicios con todos los valores

$("#grdServicios").jqxGrid(
{
    width: '100%',
    autoheight: true,
    source: dtGridServicios,
    pageable: true,
    pagesize: 9,
    altrows: true,
    theme: 'arctic',
    columns: colGridServicios,
    showaggregates: true,
    showstatusbar: true,
    statusbarheight: 25,
    pagesizeoptions: ['9']
});

/*********************************************************************************************************/

// --> Refrescar grilla cuando filtro informacion

$('#btnConsultarServicios').on('click', function () {

    $('#grdServicios').jqxGrid({ source: getSourceGridServicios() });
    ejecutoResumen(getSelectedMedicoServ(), getSelectedPeriodoServ(), getSelectedCoordServ());

});

function getHorasMinutosGuardia(hsTrabajadas, idx) {

    var horas = hsTrabajadas.split(":");
    return parseInt(horas[idx]);

}

// --> Seteo idioma en la grilla y totalizadores

$('#grdServicios').on('bindingcomplete', function (event) {
    $grid = $(this);
    $grid.jqxGrid('localizestrings', localizationobj);
    //$grid.jqxGrid('gotonextpage');
    //$grid.jqxGrid('gotoprevpage');

});


/*********************************************************************************************************/

// --> Popup para reclamo de servicios

//$("#popupServDiferencia").maskMoney({ allowZero: true });
//$("#popupServNuestroValor").maskMoney({ allowZero: true });
//$("#popupServValorEsperado").maskMoney({ allowZero: true });
//$('#popupServDiferencia,#popupServNuestroValor,#popupServValorEsperado').maskMoney('mask', 0.00);
//$('#popupServNuestroValor, #popupServValorEsperado').prop('readonly', true);


function showPopupServicio() {


    var rowindex = $('#grdServicios').jqxGrid('getselectedrowindex');
    var row = $('#grdServicios').jqxGrid('getrowdata', rowindex);
    var idInc = row.IncidenteID;

    $.ajax({
        url: 'Medicos/GetEstadoReclamo',
        dataType: 'json',
        beforeSend: function () {
            $('#busy').show();
        },
        data: { id: idInc, pMode: 1 },
        type: 'GET',
        success: function (estadoReclamo) {

            $('#busy').hide();

            // --> Limpio todos los inputs, y saco de la grilla los datos necesarios para ir a buscar al servidor y setear datos en el popup
            limpiarInputsPopupServicio();

            // --> Datos que extraigo de la grilla
            var nroServicio = row.NroInc;
            var mesDiaServicio = row.MesDia;



            // --> Seteo ID del servicio en un campo hidden del form
            $('#ServicioID').val(row.IncidenteID);

            //estadoReclamo.Estado = 0;
            //estadoReclamo.Reclamo = "holis";
            //estadoReclamo.Respuesta = "chauchis";
            //estadoReclamo.Conforme = 1;

            // --> Seteo el radio button dependiendo si esta conforme o no, y hago un disable o enable general de los controles
            $("#formSetReclamoServicio input:radio[name=Conforme][value='" + estadoReclamo.Conforme + "']").prop("checked", true);
            $("#formSetReclamoServicio input:radio[name=Estado][value='" + estadoReclamo.Estado + "']").prop("checked", true);

            hideOrShowPopupServicioElements();
            $('#popupServObservaciones').val(estadoReclamo.Reclamo);
            $('#popupServRespuesta').val(estadoReclamo.Respuesta);


            // --> Seteo titulo del popup con id de servicio y fecha
            $('#titlePopupServicios').text('Conformidad con Copago de Incidente: ' + nroServicio + ' (' + mesDiaServicio + ')');

            switch (parseInt(tipoAcceso)) {

                case 1:
                    enableDisablePopupServRespuesta(true);
                    //si ya respondio el admin.. deshabilitar todo..
                    if (estadoReclamo.Respuesta != "") {
                        enableDisablePopupServicioConformidad(true);
                        enableDisablePopupServObserv(true);
                        $('#btnGuardarReclamoServicio').prop('disabled', true);

                        // --> Si hay una respuesta, entonces muestro el estado de la respuesta.
                        $('#rowEstadoForMedicoServ').show();

                        switch (parseInt(estadoReclamo.Estado)) {
                            // --> Reclamo pendiente de aprobacion
                            case 0:
                                $('#alertRowEstadoForMedicoServ').removeClass().addClass('alert fade in alert-warning');
                                $('#txtRowEstadoForMedicoServ').html('<span class="fa fa-exclamation-circle color-yellow icon-right-margin big-icon"></span> Su reclamo está pendiente de aprobación');
                                break;
                                // --> Reclamo aceptado
                            case 1:
                                $('#alertRowEstadoForMedicoServ').removeClass().addClass('alert fade in alert-success');
                                $('#txtRowEstadoForMedicoServ').html('<span class="glyphicon glyphicon-ok-circle verde icon-right-margin big-icon"></span> Su reclamo ha sido aceptado');
                                break;
                                // --> Reclamo rechazado
                            case 2:
                                $('#alertRowEstadoForMedicoServ').removeClass().addClass('alert fade in alert-danger');
                                $('#txtRowEstadoForMedicoServ').html('<span class="glyphicon glyphicon-remove-circle rojo icon-right-margin big-icon "></span> Su reclamo ha sido rechazado.');
                                break;
                        }

                    } else {

                        // --> Si no hay respuesta, entonces no muestro el estado del reclamo y puedo seguir editando si soy medico
                        $('#rowEstadoForMedicoServ').hide();
                        enableDisablePopupServicioConformidad(false);
                        $('#btnGuardarReclamoServicio').prop('disabled', false);

                    }

                    break;

                case 2:
                    // --> Si es un admin readonly ..
                    disableAllForReadOnly();
                    break;

                case 3:
                    // --> Si es un admin que puede responder ..
                    if (estadoReclamo.Conforme == 1) {
                        disableAllForReadOnlyServ();
                    } else {
                        $('#rowResolucionForAdminServ').show();
                        enableDisablePopupServicioConformidad(true);
                        $('#btnGuardarReclamoServicio').prop('disabled', false);
                        enableDisablePopupServObserv(true);
                        enableDisablePopupServRespuesta(false);
                        enableDisablePopupServRadioRespuesta(false);

                    }

                    break;
            }

            $('#popupRevisarServicio').modal('show');

        }
    });

}

function disableAllForReadOnlyServ() {

    $('#rowResolucionForAdminServ').show();
    enableDisablePopupServObserv(true);
    enableDisablePopupServRespuesta(true);
    enableDisablePopupServicioConformidad(true);
    $('#btnGuardarReclamoServicio').prop('disabled', true);
    enableDisablePopupServRadioRespuesta(true);

}


// --> Enable / Disable el radio button para setear Conforme / No Conforme en el reclamo
function enableDisablePopupServicioConformidad(vBool) {
    $('#formSetReclamoServicio input:radio[name="Conforme"]').each(function () {
        $(this).prop('disabled', vBool);
    });
}

// --> Enable / Disable la observacion del reclamo
function enableDisablePopupServObserv(vBool) {
    $('#popupServObservaciones').prop('readonly', vBool);
}

// --> Enable / Disable la respuesta del reclamo
function enableDisablePopupServRespuesta(vBool) {
    $('#popupServRespuesta').prop('readonly', vBool);
}

// --> Enable / Disable el radio button para setear el tipo de respuesta del admin sobre el reclamo generado por un medico
function enableDisablePopupServRadioRespuesta(vBool) {
    $('#formSetReclamoServicio input:radio[name="Estado"]').each(function () {
        $(this).prop('disabled', vBool);
    });
}

// Evento que muestra o esconde los datos del popup segun si el medico esta conforme o no...
$('#formSetReclamoServicio input:radio[name="Conforme"]').on('change', function () {
    hideOrShowPopupServicioElements();
});

function hideOrShowPopupServicioElements() {

    limpiarInputsPopupServicio();
    enableDisablePopupServRespuesta(true);
    if ($("#formSetReclamoServicio input:radio[name=Conforme][value='0']").is(":checked")) {
        enableDisablePopupServObserv(false);
    } else {
        enableDisablePopupServObserv(true);
    }
}

//Limpio inputs del popup de reclamo de servicio
function limpiarInputsPopupServicio() {

    $('#popupServObservaciones').val('');
    $('#popupServRespuesta').val('');

}


// --> Guardo el reclamo ..
$('#formSetReclamoServicio').on('submit', function (e) {

    // --> Obtengo el objeto del form, lo serializo a un json y lo paso a un querystring. Del lado del server, lo bindeo
    // --> contra un objeto EstadoReclamoGuardia y ya tengo todas las propiedades cargadas.
    var frmReclamo = $(this);
    var jsonReclamoObj = frmReclamo.serializeObject();
    var jsonReclamoStr = JSON.stringify(jsonReclamoObj);

    var urlToPostReclamoServicio = "";
    if (tipoAcceso == 1) {
        // --> Si soy médico, seteo el reclamo del servicio
        urlToPostReclamoServicio = "Medicos/SetReclamoServicio";
    } else {
        // --> Si soy administrador, respondo y seteo la respuesta del reclamo del servicio
        urlToPostReclamoServicio = "Medicos/SetRespuestaReclamoServicio";
    }

    // --> Si el reclamo paso la validacion del lado cliente ..
    if (reclamoValidadoServ(jsonReclamoObj)) {
        $.ajax({
            url: urlToPostReclamoServicio,
            type: 'POST',
            cache: false,
            beforeSend: function () {
                $('#busy').show();
            },
            dataType: 'json',
            data: jsonReclamoStr,
            contentType: 'application/json; charset=utf-8',
            success: function (reclamoOk) {
                // --> Si el reclamo se envio bien..
                if (reclamoOk == 1) {

                    setAlert("Aguarde un instante por favor..", "info");
                    // --> Actualizo la grilla con los ultimos datos cargados
                    updateGridAfterAdminResponseServ();

                } else {
                    setAlert("El reclamo no se pudo enviar. Intente nuevamente por favor", "error");
                    $('#busy').hide();
                }
            },
            error: function (xhr, status, error) {
                var err = eval("(" + xhr.responseText + ")");
                console.log(err.Message);
                setAlert("El reclamo no se pudo enviar. Intente nuevamente por favor", "error");
                $('#busy').hide();
            }
        });
    }

    e.preventDefault();

});

// --> Valido algunos campos del lado cliente
function reclamoValidadoServ(reclamo) {

    var msgError = "";

    if (reclamo.Conforme == 0) {

        if (reclamo.Reclamo == "") {
            msgError = "Debe ingresar el reclamo\n";
            setAlert(msgError, 'error');
        }

        if (msgError != "") {
            return false;
        }
    }

    return true;
}

// --> Seteo alerta mas linda que el clasico alert. Le paso el mensaje y el tipo de alerta.
function setAlert(msg, tipoMsg) {
    Messenger().post({
        message: msg,
        type: tipoMsg,
        showCloseButton: true
    });
}

// --> Como escribo en produccion, y leo del shadow, por un tema de licencias, los datos tienen un pequeño delay. Entonces, para que cuando
// --> modifique o cargue un reclamo, se vea bien en la grilla, meto un delay de 3 segundos para aguantar que el dato pase de produccion al shadow,
// --> y la grilla se vea correctamente.
function updateGridAfterAdminResponseServ() {

    window.setTimeout(function () {
        var dtGridServicios = getSourceGridServicios();
        $('#grdServicios').jqxGrid({ source: dtGridServicios });
        setAlert("El reclamo fue enviado satisfactoriamente.", "info");
        $('#busy').hide();
        $('#popupRevisarServicio').modal('hide');
    }, 3000);

}


