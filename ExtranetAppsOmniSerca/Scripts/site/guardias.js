
// --> Inicializo algunos componentes jqwidgets
$("#btnConsultarGuardias").jqxButton({ width: '100', theme: 'bootstrap', height: '27' });
$("#popupGrdEntrada").jqxDateTimeInput({ width: '100%', height: '32', formatString: 'HH:mm', disabled: true, showCalendarButton: false, theme: 'bootstrap', textAlign: 'center' });
$("#popupGrdSalida").jqxDateTimeInput({ width: '100%', height: '32', formatString: 'HH:mm', disabled: true, showCalendarButton: false, theme: 'bootstrap', textAlign: 'center' });

if (tipoAcceso == 1) {
    $('#colMedicoGuardias').hide();
    $('#colMedicoServicios').hide();
    $('#colMedicoResumen').hide();
    $('#colEstadoGuardias').hide();
}

// --> Inicializo opciones generales para el alerta que uso en vez del alert de javascript comun
Messenger().options = {
    extraClasses: 'messenger-fixed messenger-on-bottom messenger-on-right',
    theme: 'flat',
};

// --> Seteo dropdownlist para seleccionar estado de guardias

var vEstadosGuardias = [];
var vEstadosDesc = ['Todos', 'Pendientes', 'Aceptados', 'Rechazados'];

for (var j = 0; j < vEstadosDesc.length; j++) {
    var obj = { ID: j, Descripcion: vEstadosDesc[j] };
    vEstadosGuardias.push(obj);
}

// --> Set dropdownlist estado de guardias

$("#ftrEstadoGuardias").jqxDropDownList({
    selectedIndex: 0, source: vEstadosGuardias, displayMember: "Descripcion",
    valueMember: "ID", width: '110%', dropDownHeight: 110, height: 25, theme: 'bootstrap'
});

// --> Set dropdownlist periodo de guardias

$("#ftrPeriodoGuardias").jqxDropDownList({
    selectedIndex: 2, source: setFtrPeriodoGuardias(), displayMember: "Descripcion",
    valueMember: "Periodo", width: '110%', dropDownHeight: 80, height: 25, theme: 'bootstrap'
});

// --> Set dropdownlist medicos de guardias

$("#ftrMedicoGuardias").jqxDropDownList({
    source: getSourceFiltroMedicos(), displayMember: "Nombre", selectedIndex: 0,
    valueMember: "UsuarioID", width: '110%', dropDownHeight: 150, dropDownWidth: 320, height: 25, theme: 'bootstrap'
});

$("#ftrCoordGuardias").jqxDropDownList({
    selectedIndex: 0, source: setSrcFtrCoordGuardias(), displayMember: "Descripcion",
    valueMember: "ID", width: '110%', dropDownHeight: 125, height: 25, theme: 'bootstrap'
});

function setFtrPeriodoGuardias() {

    // --> Seteo dropdownlist para seleccionar periodo en la grilla de guardias

    var srcFtrPeriodoGuardias = {
        datatype: "json",
        datafields: [
            { name: 'Periodo' },
            { name: 'Descripcion' }
        ],
        url: 'Medicos/getFiltroPeriodos',
        async: false
    };

    var dtFtrPeriodoGuardias = new $.jqx.dataAdapter(srcFtrPeriodoGuardias);

    return dtFtrPeriodoGuardias;
}

/*************************************************************************************************/

// --> Seteo dropdownlist para seleccionar medico en la grilla de guardias

function getSourceFiltroMedicos() {

    var urlMedicoGuardias = 'Medicos/getFiltroMedicos?usr_id=' + usr_id_medico +
                            '&selPeriodo=' + getSelectedPeriodo() + '&selEstado=' + getSelectedEstado();

    if (tipoAcceso == 1) {
        urlMedicoGuardias += '&esMedico=1';
    }

    var srcFtrMedicoGuardias = {
        datatype: "json",
        datafields: [
            { name: 'UsuarioID' },
            { name: 'Nombre' }
        ],
        url: urlMedicoGuardias,
        async: false
    };

    var dtFtrMedicoGuardias = new $.jqx.dataAdapter(srcFtrMedicoGuardias);

    return dtFtrMedicoGuardias;

}

// --> Cuando selecciono medico, actualizo el dropdownlist de las coordinaciones

$('#ftrMedicoGuardias').on('select', function (event) {

    $('#ftrCoordGuardias').jqxDropDownList({ source: setSrcFtrCoordGuardias() });

});

$('#ftrEstadoGuardias, #ftrPeriodoGuardias').on('select', function (event) {

    $('#ftrMedicoGuardias').jqxDropDownList({ source: getSourceFiltroMedicos() });
    $('#ftrCoordGuardias').jqxDropDownList({ source: setSrcFtrCoordGuardias() });

});

/*********************************************************************************************************/

// --> Seteo dropdownlist para seleccionar coordinacion en la grilla de guardias

function setSrcFtrCoordGuardias() {

    var srcFtrCoordGuardias = {
        datatype: "json",
        datafields: [
            { name: 'ID' },
            { name: 'Descripcion' }
        ],
        url: 'Medicos/getFiltroCoordinaciones?usr_id=' + getSelectedMedico(),
        async: false
    };

    var dtFtrCoordGuardias = new $.jqx.dataAdapter(srcFtrCoordGuardias);

    return dtFtrCoordGuardias;

}

/*********************************************************************************************************/

// --> Seteo dropdownlist para seleccionar periodo en la grilla de guardias

var srcFtrDiaGuardias = ["Seleccione D&iacute;a"];

for (var k = 1; k <= 31; k++) {
    srcFtrDiaGuardias.push(k);
}

$("#ftrDiaGuardias").jqxDropDownList({ selectedIndex: 0, source: srcFtrDiaGuardias, width: '110%', height: 25, theme: 'bootstrap' });

/*********************************************************************************************************/

// -> Funciones para obtener el periodo y el dia de los filtros en la grilla de guardias

function getSelectedPeriodo() {
    return $("#ftrPeriodoGuardias").jqxDropDownList('getSelectedItem').value;
}

function getSelectedDia() {
    return $('#ftrDiaGuardias').jqxDropDownList('selectedIndex');
}

function getSelectedCoord() {
    var result = $('#ftrCoordGuardias').jqxDropDownList('getSelectedItem');
    if (result != null && result != undefined)
        return result.value;
    else null;
}

function getSelectedMedico() {
    var result = $("#ftrMedicoGuardias").jqxDropDownList('getSelectedItem');
    if (result != null && result != undefined)
        return result.value;
    else null;
}

function getSelectedEstado() {
    return $("#ftrEstadoGuardias").jqxDropDownList('getSelectedItem').value;
}

function getDescriptionSelectedPeriodo() {
    return $("#ftrPeriodoGuardias").jqxDropDownList('getSelectedItem').label;
}

function getDescriptionSelectedMedico() {
    return $("#ftrMedicoGuardias").jqxDropDownList('getSelectedItem').label;
}

/*********************************************************************************************************/

// --> Seteo datafields de grilla de guardias

var dtFieldsGuardias = [{ name: 'ID', type: 'string' },
                        { name: 'FecMovimiento', type: 'string' },
                        { name: 'DiaDeLaSemana', type: 'string' },
                        { name: 'Tarifa', type: 'string' },
                        { name: 'HorarioEntrada', type: 'string' },
                        { name: 'MinutosLlegadaTarde', type: 'number' },
                        { name: 'HorarioSalida', type: 'string' },
                        { name: 'MinutosRetiroAnticipado', type: 'number' },
                        { name: 'Movil', type: 'string' },
                        { name: 'HorasTrabajadas', type: 'string' },
                        { name: 'Rojos', type: 'number' },
                        { name: 'Amarillos', type: 'number' },
                        { name: 'Verdes', type: 'number' },
                        { name: 'TrasladosProgramados', type: 'number' },
                        { name: 'ImpTotalHoras', type: 'number' },
                        { name: 'ImpEspecialidad', type: 'number' },
                        { name: 'ImpPrestacion', type: 'number' },
                        { name: 'ImpPrestacionExcedente', type: 'number' },
                        { name: 'ImpAnticipo', type: 'number' },
                        { name: 'ImpFinal', type: 'number' },
                        { name: 'Estado', type: 'number' }];

// --> Seteo source grilla de guardias

function getSourceGridGuardias() {

    var srcGridGuardias = {
        datatype: "json",
        datafields: dtFieldsGuardias,
        url: 'Medicos/GetGuardias',
        data: {
            periodo: getSelectedPeriodo(),
            dia: getSelectedDia(),
            coordinacion: getSelectedCoord(),
            medico: getSelectedMedico(),
            estado: getSelectedEstado()
        }
    };

    var dtGridGuardias = new $.jqx.dataAdapter(srcGridGuardias);

    return dtGridGuardias;
}

// --> Seteo cellsrenderers de grilla de guardias, para poner en rojo valores dependiendo si llegaron tarde, etc.

var crHorEntradaGuardia = function (row, columnfield, value, defaulthtml, columnproperties) {
    var minLlegadaTarde = $("#grdGuardias").jqxGrid('getrowdata', row).MinutosLlegadaTarde;
    if (minLlegadaTarde > 0) {
        return '<div style="text-align:center"><span style="line-height:25px;color: red;font-weight: bold">' + value + '</span></div>';
    }
}

var crHorSalidaGuardia = function (row, columnfield, value, defaulthtml, columnproperties) {
    var minRetiroAnticipado = $("#grdGuardias").jqxGrid('getrowdata', row).MinutosRetiroAnticipado;
    if (minRetiroAnticipado > 0) {
        return '<div style="text-align:center"><span style="line-height:25px;color: red;font-weight: bold">' + value + '</span></div>';
    }
}

var crAnticipoGuardia = function (row, columnfield, value, defaulthtml, columnproperties) {
    return '<div style="text-align:right"><span style="line-height:25px">' + '($' + numberWithCommas(parseFloat(value).toFixed(2)) + ')' + '</span></div>';
}


var crConfirmacionGuardia = function (row, columnfield, value, defaulthtml, columnproperties) {
    var retVal = "";
    switch (value) {
        case 0:
            // --> No esta conforme
            retVal = '<a href="javascript:showPopupGuardia()" style="margin-left:34%;line-height:33px"><span class="glyphicon glyphicon-remove-circle rojo icon-right-margin big-icon "></span></a>';
            break;
        case 1:
            // --> Está conforme
            retVal = '<a href="javascript:showPopupGuardia()" style="margin-left:34%;line-height:33px"><span class="glyphicon glyphicon-ok-circle verde icon-right-margin big-icon "></span></a>';
            break;
        case 2:
            // --> No está conforme y recibió una respuesta
            retVal = '<a href="javascript:showPopupGuardia()" style="margin-left:34%;line-height:33px"><span class="fa fa-exclamation-circle color-yellow icon-right-margin big-icon"></span></a>';
            break;
        case 3:
            // --> Reclamo aceptado
            retVal = '<a href="javascript:showPopupGuardia()" style="margin-left:34%;line-height:33px"><span class="glyphicon glyphicon-ok-circle azul icon-right-margin big-icon "></span></a>';
            break;
        case 4:
            // --> Reclamo no aceptado
            retVal = '<a href="javascript:showPopupGuardia()" style="margin-left:34%;line-height:33px"><span class="glyphicon glyphicon-remove-circle naranja icon-right-margin big-icon "></span></a>';
            break;
    }

    return retVal;

}


// --> Seteo columnas de la grilla de guardias

var colGridGuardias =
             [
              { text: 'ID', datafield: 'ID', hidden: true },
              { text: 'FecMovimiento', datafield: 'FecMovimiento', hidden: true },
              { text: 'D&iacute;a', datafield: 'DiaDeLaSemana', width: '5%', cellsalign: 'center' },
              { text: 'Tar', datafield: 'Tarifa', width: '4%', cellsalign: 'center' },
              { text: 'H.Ent', datafield: 'HorarioEntrada', width: '5%', cellsalign: 'center', cellsrenderer: crHorEntradaGuardia },
              { text: 'H.Sal', datafield: 'HorarioSalida', width: '5%', cellsalign: 'center', cellsRenderer: crHorSalidaGuardia },
              { text: 'Mov', datafield: 'Movil', width: '4%', cellsalign: 'center' },
              { text: 'T.Hs', datafield: 'HorasTrabajadas', width: '5%', cellsalign: 'center' },
              { text: 'Ro', datafield: 'Rojos', width: '5%', cellsalign: 'center' },
              { text: 'Am', datafield: 'Amarillos', width: '5%', cellsalign: 'center' },
              { text: 'Ve', datafield: 'Verdes', width: '5%', cellsalign: 'center' },
              { text: 'Tp', datafield: 'TrasladosProgramados', width: '5%', cellsalign: 'center' },
              { text: '$Hs', datafield: 'ImpTotalHoras', width: '8%', cellsalign: 'right', cellsformat: 'c2' },
              { text: '$Esp', datafield: 'ImpEspecialidad', width: '8%', cellsalign: 'right', cellsformat: 'c2' },
              { text: '$Inc', datafield: 'ImpPrestacion', width: '8%', cellsalign: 'right', cellsformat: 'c2' },
              { text: '$Exc', datafield: 'ImpPrestacionExcedente', width: '8%', cellsalign: 'right', cellsformat: 'c2' },
              { text: '$Ant', datafield: 'ImpAnticipo', width: '8%', cellsalign: 'right', cellsRenderer: crAnticipoGuardia },
              { text: '$Tot', datafield: 'ImpFinal', width: '8%', cellsalign: 'right', cellsformat: 'c2' },
              { text: 'Est', datafield: 'Estado', width: '4%', cellsalign: 'center', cellsRenderer: crConfirmacionGuardia }
             ];


// --> Seteo objeto de la grilla de guardias con todos los valores

$("#grdGuardias").jqxGrid(
{
    width: '100%',
    autoheight: true,
    source: getSourceGridGuardias(),
    pageable: true,
    altrows: true,
    pagesize: 10,
    theme: 'arctic',
    columns: colGridGuardias,
    pagesizeoptions: ['10']
});

/*********************************************************************************************************/

// --> Seteo idioma en la grilla y totalizadores

$('#grdGuardias').on('bindingcomplete', function (event) {

    var $grid = $(this);

    var $rows = $grid.jqxGrid('getrows');
    var cantGuardias = $rows.length;
    var acumServicios = 0;
    var acumImporteTotal = 0;
    var acumHoras = 0;
    var acumMinutos = 0;

    for (var i = 0; i < $rows.length; i++) {
        var row = $rows[i];
        acumServicios += row.Rojos + row.Amarillos + row.Verdes + row.TrasladosProgramados;
        acumImporteTotal += row.ImpFinal;
        acumHoras += getHorasMinutosGuardia(row.HorasTrabajadas, 0);
        acumMinutos += getHorasMinutosGuardia(row.HorasTrabajadas, 1);
    }

    if (acumMinutos > 60) {
        acumHoras = acumHoras + parseInt((acumMinutos / 60));
        acumMinutos = acumMinutos % 60;
    }

    var totalHorasTrabajadas = acumHoras + " hs. " + acumMinutos + " min.";

    $('#cantServicios').text(acumServicios);
    $('#cantGuardias').text(cantGuardias);
    $('#impLiquidado').text(parseFloat(acumImporteTotal).toFixed(2));
    $('#totalHoras').text(totalHorasTrabajadas);
    $('#titPeriodo').text(getDescriptionSelectedPeriodo());
    $('#titMedico').text(getDescriptionSelectedMedico());

    $grid.jqxGrid('localizestrings', localizationobj);
    //$grid.jqxGrid('gotonextpage');
    //$grid.jqxGrid('gotoprevpage');

});

function getHorasMinutosGuardia(hsTrabajadas, idx) {

    var horas = hsTrabajadas.split(":");
    return parseInt(horas[idx]);

}

/*********************************************************************************************************/

// --> Refrescar grilla cuando filtro informacion

$('#btnConsultarGuardias').on('click', function () {

    $('#grdGuardias').jqxGrid({ source: getSourceGridGuardias() });
    ejecutoResumen(getSelectedMedico(), getSelectedPeriodo(), getSelectedCoord());

});

function ejecutoResumen(valMedico, valPeriodo, valCoord) {

    var itemMed = $("#ftrMedicoResumen").jqxDropDownList('getItemByValue', valMedico);
    $('#ftrMedicoResumen').jqxDropDownList('selectItem', itemMed);

    var itemPer = $("#ftrPeriodoResumen").jqxDropDownList('getItemByValue', valPeriodo);
    $('#ftrPeriodoResumen').jqxDropDownList('selectItem', itemPer);

    var itemCoord = $("#ftrCoordResumen").jqxDropDownList('getItemByValue', valCoord);
    $('#ftrCoordResumen').jqxDropDownList('selectItem', itemCoord);

    setSourceResumen();

}


/*********************************************************************************************************/

// --> Popup para reclamo de guardias


// Set datasource para el filtro de motivo de reclamo, el dropDownList en el popup de reclamo de guardia
var srcFtrMotivoReclamoGuardias = {
    datatype: "json",
    datafields: [
        { name: 'ID' },
        { name: 'Descripcion' }
    ],
    url: 'Medicos/getFiltroMotivoReclamo',
    data: { flgTipoReclamo: 2 },
    async: false
};

var dtFtrMotivoReclamoGuardias = new $.jqx.dataAdapter(srcFtrMotivoReclamoGuardias);

$("#ftrMotivoReclamoGuardias").jqxDropDownList({
    source: dtFtrMotivoReclamoGuardias, displayMember: "Descripcion", placeHolder: "Por favor seleccione:",
    valueMember: "ID", width: 266, dropDownHeight: 150, dropDownWidth: 265, height: 32, theme: 'bootstrap'
});

//Cuando selecciono un motivo de reclamo de guardia, veo si es relacionado a un horario. Solo si es así, le saco el disabled
// a los campos horario entrada y horario salida.
$('#ftrMotivoReclamoGuardias').on('select', function (event) {

    var args = event.args;
    var item = args.item;
    var vReclamo = item.value.split("/");
    var idReclamo = parseInt(vReclamo[0]);
    var difIngreso = parseInt(vReclamo[1]);
    if (difIngreso == 0) {
        enableDisablePopupGuardiaHorarios(true);
    } else {
        enableDisablePopupGuardiaHorarios(false);
    }

});

// Muestro popup de reclamo de guardia
function showPopupGuardia() {

    var rowindex = $('#grdGuardias').jqxGrid('getselectedrowindex');
    var row = $('#grdGuardias').jqxGrid('getrowdata', rowindex);
    var idGuardia = row.ID;

    $.ajax({
        url: 'Medicos/GetEstadoReclamo',
        dataType: 'json',
        beforeSend: function () {
            $('#busy').show();
        },
        data: { id: idGuardia, pMode: 0 },
        type: 'GET',
        success: function (estadoReclamo) {

            $('#busy').hide();

            // --> Limpio todos los inputs, y saco de la grilla los datos necesarios para ir a buscar al servidor y setear datos en el popup
            limpiarInputsPopupGuardia();

            // --> Datos que extraigo de la grilla
            var fechaGuardia = row.FecMovimiento;
            var movil = row.Movil;

            // --> Seteo ID de la guardia en un campo hidden del form
            $('#GuardiaID').val(row.ID);

            // --> Seteo el radio button dependiendo si esta conforme o no, y hago un disable o enable general de los controles
            $("#formSetReclamo input:radio[name=Conforme][value='" + estadoReclamo.Conforme + "']").prop("checked", true);
            $("#formSetReclamo input:radio[name=Estado][value='" + estadoReclamo.Estado + "']").prop("checked", true);
            hideOrShowPopupGuardiaElements();


            // --> Datos que extraigo del servidor
            $('#popupGrdObservaciones').val(estadoReclamo.Reclamo);
            $('#popupGrdRespuesta').val(estadoReclamo.Respuesta);

            if (estadoReclamo.Entrada != "") {
                $('#popupGrdEntrada').jqxDateTimeInput('setDate', new Date(2014, 1, 1, estadoReclamo.Entrada.split(":")[0], estadoReclamo.Entrada.split(":")[1]));
                $('#popupGrdSalida').jqxDateTimeInput('setDate', new Date(2014, 1, 1, estadoReclamo.Salida.split(":")[0], estadoReclamo.Salida.split(":")[1]));
            }

            // --> Seteo titulo del popup con movil y fecha de la guardia
            $('#titlePopupGuardias').text('Confirmación de la guardia del ' + fechaGuardia + ' en el móvil ' + movil);

            // Toda esta movida la hago porque no puedo poner 2 values en el dropdown de motivo de reclamo. Entonces tengo
            // id de motivo reclamo / si acepta diferencia horaria
            var motivo = estadoReclamo.MotivoId + "/0";
            var itemMotivo = $("#ftrMotivoReclamoGuardias").jqxDropDownList('getItemByValue', motivo);
            if (itemMotivo == null) {
                itemMotivo = $("#ftrMotivoReclamoGuardias").jqxDropDownList('getItemByValue', estadoReclamo.MotivoId + "/1");
            }
            $("#ftrMotivoReclamoGuardias").jqxDropDownList('selectItem', itemMotivo);

            // --> Dependiendo el tipo de acceso, voy a permitir o no diferentes cosas
            switch (parseInt(tipoAcceso)) {
                case 1:
                    // --> Si es un médico, y la respuesta del reclamo ya fue hecha, no puede tocar nada del popup. Todo deshabilitado.
                    if (estadoReclamo.Respuesta != "") {
                        enableDisablePopupGuardiaGeneral(true, true, true, true);
                        enableDisablePopupGuardiaConformidad(true);
                        enableDisablePopupGuardiaBotonGuardar(true);

                        // --> Si hay una respuesta, entonces muestro el estado de la respuesta.
                        $('#rowEstadoForMedico').show();

                        switch (parseInt(estadoReclamo.Estado)) {
                            // --> Reclamo pendiente de aprobacion
                            case 0:
                                $('#alertRowEstadoForMedico').removeClass().addClass('alert fade in alert-warning');
                                $('#txtRowEstadoForMedico').html('<span class="fa fa-exclamation-circle color-yellow icon-right-margin big-icon"></span> Su reclamo está pendiente de aprobación');
                                break;
                                // --> Reclamo aceptado
                            case 1:
                                $('#alertRowEstadoForMedico').removeClass().addClass('alert fade in alert-success');
                                $('#txtRowEstadoForMedico').html('<span class="glyphicon glyphicon-ok-circle verde icon-right-margin big-icon"></span> Su reclamo ha sido aceptado');
                                break;
                                // --> Reclamo rechazado
                            case 2:
                                $('#alertRowEstadoForMedico').removeClass().addClass('alert fade in alert-danger');
                                $('#txtRowEstadoForMedico').html('<span class="glyphicon glyphicon-remove-circle rojo icon-right-margin big-icon "></span> Su reclamo ha sido rechazado.');
                                break;
                        }
                    } else {
                        // --> Si no hay respuesta, entonces no muestro el estado del reclamo y puedo seguir editando si soy medico
                        $('#rowEstadoForMedico').hide();
                        enableDisablePopupGuardiaConformidad(false);
                        enableDisablePopupGuardiaBotonGuardar(false);
                    }
                    break;
                case 2:
                    // --> Si es un admin readonly ..
                    disableAllForReadOnly();
                    break;
                case 3:
                    // --> Si es un admin que puede responder ..
                    if (estadoReclamo.Conforme == 1) {
                        disableAllForReadOnly();
                    } else {

                        $('#rowResolucionForAdmin').show();
                        enableDisablePopupGuardiaConformidad(true);
                        enableDisablePopupGuardiaGeneral(true, false, true, true);
                        enableDisablePopupGuardiaBotonGuardar(false);
                        enableDisablePopupGuardiaRadioRespuesta(false);
                    }
                    break;

            }

            // --> Mostrar popup de guardia
            $('#popupRevisarGuardias').modal('show');
        },
        error: function () {
            setAlert('Error, no se pudo recuperar la guardia seleccionada', 'error');
            $('#busy').hide();
        }
    });
}

function disableAllForReadOnly() {

    $('#rowResolucionForAdmin').show();
    enableDisablePopupGuardiaGeneral(true, true, true, true);
    enableDisablePopupGuardiaConformidad(true);
    enableDisablePopupGuardiaBotonGuardar(true);
    enableDisablePopupGuardiaRadioRespuesta(true);
}

// --> Enable / Disable el radio button para setear Conforme / No Conforme en el reclamo
function enableDisablePopupGuardiaConformidad(vBool) {
    $('#formSetReclamo input:radio[name="Conforme"]').each(function () {
        $(this).prop('disabled', vBool);
    });
}

// --> Enable / Disable el radio button para setear el tipo de respuesta del admin sobre el reclamo generado por un medico
function enableDisablePopupGuardiaRadioRespuesta(vBool) {
    $('#formSetReclamo input:radio[name="Estado"]').each(function () {
        $(this).prop('disabled', vBool);
    });
}

// --> Enable / Disable la observacion del reclamo
function enableDisablePopupGuardiaObserv(vBool) {
    $('#popupGrdObservaciones').prop('readonly', vBool);
}

// --> Enable / Disable la respuesta del reclamo
function enableDisablePopupGuardiaRta(vBool) {
    $('#popupGrdRespuesta').prop('readonly', vBool);
}

// --> Enable / Disable el dropdownlist del motivo del reclamo
function enableDisablePopupGuardiaMotivo(vBool) {
    $('#ftrMotivoReclamoGuardias').jqxDropDownList({ disabled: vBool });
}

// --> Enable / Disable los campos de horario del reclamo (solo se usa si el reclamo tiene diferencia horaria)
function enableDisablePopupGuardiaHorarios(vBool) {
    $('.popupGrdHorario').each(function (event) {
        $ctrl = $(this);
        $ctrl.jqxDateTimeInput({ disabled: vBool });
    });
}

// --> Enable / Disable boton guardar reclamo
function enableDisablePopupGuardiaBotonGuardar(vBool) {
    $('#btnGuardarReclamoGuardia').prop('disabled', vBool);
}

// --> Agrupo varios enable / disable juntos para mayor claridad
function enableDisablePopupGuardiaGeneral(bObserv, bRta, bMotivo, bHorarios) {

    enableDisablePopupGuardiaMotivo(bMotivo);
    enableDisablePopupGuardiaObserv(bObserv);
    enableDisablePopupGuardiaRta(bRta);
    enableDisablePopupGuardiaHorarios(bHorarios);

}

//Limpio inputs del popup de reclamo de guardias
function limpiarInputsPopupGuardia() {

    $('#popupGrdObservaciones').val('');
    $('#popupGrdRespuesta').val('');
    $('.popupGrdHorario').each(function (event) {
        $(this).jqxDateTimeInput('setDate', new Date(2014, 1, 1, 00, 00));
    });
    $('#ftrMotivoReclamoGuardias').jqxDropDownList('clearSelection');
}

// Evento que muestra o esconde los datos del popup segun si el medico esta conforme o no...
$('#formSetReclamo input:radio[name="Conforme"]').on('change', function () {
    hideOrShowPopupGuardiaElements();
});

function hideOrShowPopupGuardiaElements() {

    limpiarInputsPopupGuardia();
    $radio = $('#formSetReclamo input:radio[name="Conforme"]');

    if ($("#formSetReclamo input:radio[name=Conforme][value='0']").is(":checked")) {
        enableDisablePopupGuardiaGeneral(false, true, false, true);
    } else {
        enableDisablePopupGuardiaGeneral(true, true, true, true);
    }
}


// --> Guardo el reclamo ..
$('#formSetReclamo').on('submit', function (e) {

    // --> Obtengo el objeto del form, lo serializo a un json y lo paso a un querystring. Del lado del server, lo bindeo
    // --> contra un objeto EstadoReclamoGuardia y ya tengo todas las propiedades cargadas.
    var frmReclamo = $(this);
    var jsonReclamoObj = frmReclamo.serializeObject();
    var jsonReclamoStr = JSON.stringify(jsonReclamoObj);


    var urlToPostReclamoGuardia = "";
    if (tipoAcceso == 1) {
        // --> Si soy médico, seteo el reclamo de la guardia
        urlToPostReclamoGuardia = "Medicos/SetReclamoGuardia";
    } else {
        // --> Si soy administrador, respondo y seteo la respuesta del reclamo de la guardia
        urlToPostReclamoGuardia = "Medicos/SetRespuestaReclamoGuardia";
    }

    // --> Si el reclamo paso la validacion del lado cliente ..
    if (reclamoValidado(jsonReclamoObj)) {
        $.ajax({
            url: urlToPostReclamoGuardia,
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
                    updateGridAfterAdminResponse();

                } else {
                    setAlert("El reclamo no se pudo enviar. Intente nuevamente por favor", "error");
                    $('#busy').hide();
                }
            },
            error: function (error) {
                setAlert("El reclamo no se pudo enviar. Intente nuevamente por favor", "error");
                $('#busy').hide();
            }
        });
    }

    e.preventDefault();

});

// --> Como escribo en produccion, y leo del shadow, por un tema de licencias, los datos tienen un pequeño delay. Entonces, para que cuando
// --> modifique o cargue un reclamo, se vea bien en la grilla, meto un delay de 3 segundos para aguantar que el dato pase de produccion al shadow,
// --> y la grilla se vea correctamente.
function updateGridAfterAdminResponse() {

    window.setTimeout(function () {
        var dtGridGuardias = getSourceGridGuardias();
        $('#grdGuardias').jqxGrid({ source: dtGridGuardias });
        setAlert("El reclamo fue enviado satisfactoriamente.", "info");
        $('#busy').hide();
        $('#popupRevisarGuardias').modal('hide');
    }, 3000);

}

// --> Valido algunos campos del lado cliente
function reclamoValidado(reclamo) {

    var msgError = "";

    if (reclamo.Conforme == 0) {

        if (reclamo.Reclamo == "") {
            msgError = "Debe ingresar el reclamo\n";
            setAlert(msgError, 'error');
        }
        if (reclamo.MotivoId == "") {
            msgError = "Debe ingresar el motivo del reclamo\n";
            setAlert(msgError, 'error');
        }
        if (tipoAcceso == 3) {
            if (reclamo.Respuesta == "") {
                msgError = "Debe ingresar la respuesta al reclamo";
                setAlert(msgError, 'error');
            }
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

function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}









