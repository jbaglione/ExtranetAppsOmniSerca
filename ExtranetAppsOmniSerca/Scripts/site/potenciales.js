var vModalidadesPotenciales = [];
var vMedicosPotenciales = [];
var vZonasPotenciales = [];
var vDiasPotenciales = [];
var vGuardiasPotenciales = [];
var dtGridPotenciales = [];

var dtFieldsPotenciales = [
        { name: 'PotencialID', type: 'string' },
        { name: 'ModalidadDescripcion', type: 'string' },
        { name: 'MedicoDescripcion', type: 'string' },
        { name: 'DiaDescripcion', type: 'string' },
        { name: 'ZonaDescripcion', type: 'string' },
        { name: 'Localidad', type: 'string' },
        { name: 'HoraEntrada', type: 'string' },
        { name: 'HoraSalida', type: 'string' },
        { name: 'FechaCarga', type: 'string' },
];
var colGridPotenciales = [
              { text: 'ID', datafield: 'PotencialID', hidden: true },
              { text: 'Modalidad', datafield: 'ModalidadDescripcion', width: '15%' },
              { text: 'Medico', datafield: 'MedicoDescripcion', width: '30.98%' },
              { text: 'Dia', datafield: 'DiaDescripcion', width: '10%', cellsalign: 'center' },
              { text: 'Zona', datafield: 'ZonaDescripcion', width: '10%', cellsalign: 'center' },
              { text: 'Localidad', datafield: 'Localidad', width: '15%', cellsalign: 'center' },
              { text: 'H. Entrada', datafield: 'HoraEntrada', width: '7%', cellsalign: 'center' },
              { text: 'H. Salida', datafield: 'HoraSalida', width: '7%', cellsalign: 'center' },
              { text: 'Inf.', dataField: 'FechaCarga', width: '5%', cellsrenderer: crInfo }
];

function crInfo (row, columnfield, value, defaulthtml, columnproperties) {
    var icon = '';
    var dif = monthDiff(new Date(value), new Date());
    if (dif > 6) {
        icon = 'glyphicon-info-sign azul';
        toolTip = 'Esta Guardia fue cargada hace '+ dif +' meses';
        return '<div style="cursor: pointer;" title="' + toolTip + '"><span style="margin-left:34%;line-height:33px;" class="glyphicon ' + icon + ' icon-right-margin big-icon"></span></div>';
    }

    return '<div></div>';
}

function monthDiff(d1, d2) {
    var months;
    months = (d2.getFullYear() - d1.getFullYear()) * 12;
    months -= d1.getMonth() + 1;
    months += d2.getMonth();
    return months <= 0 ? 0 : months;
}

getSourcePotenciales();

$("#btnConsultarPotenciales").jqxButton({ theme: 'bootstrap', height: '27' });
$("#btnRefrescarPotenciales").jqxButton({ theme: 'bootstrap', height: '27' });

// --> Seteo dropdownlist para seleccionar periodo en la grilla de servicios
// --> Uso mismo source que filtro de periodo de guardias
function loadCombos() {
    $("#ftrModalidadPotenciales").jqxDropDownList({
        selectedIndex: 0, source: vModalidadesPotenciales, displayMember: "Descripcion",
        valueMember: "Id", width: '100%', height: 25, theme: 'bootstrap'
    });
    $("#ftrMedicoPotenciales").jqxDropDownList({
        source: vMedicosPotenciales, displayMember: "Descripcion", selectedIndex: 0,
        valueMember: "Id", width: '100%', height: 25, theme: 'bootstrap'
    });
    $("#ftrZonaPotenciales").jqxDropDownList({
        selectedIndex: 0, source: vZonasPotenciales, displayMember: "Descripcion",
        valueMember: "Id", width: '100%', height: 25, theme: 'bootstrap'
    });
    $("#ftrDiaPotenciales").jqxDropDownList({
        selectedIndex: 0, source: vDiasPotenciales, displayMember: "Descripcion",
        valueMember: "Id", width: '100%', height: 25, theme: 'bootstrap'
    });
};

/*********************************************************************************************************/
// -> Funciones para obtener el periodo y el dia de los filtros en la grilla de guardias

function getSelectedModalidadPot() {
    return $("#ftrModalidadPotenciales").jqxDropDownList('getSelectedItem').value;
}

function getSelectedMedicoPot() {
    var result = $("#ftrMedicoPotenciales").jqxDropDownList('getSelectedItem');
    if (result != null && result != undefined)
        return result.value;
    else null;
}

function getSelectedDiaPot() {
    var result = $("#ftrDiaPotenciales").jqxDropDownList('getSelectedItem');
    if (result != null && result != undefined)
        return result.value;
    else null;
}

function getSelectedZonaPot() {
    var result = $('#ftrZonaPotenciales').jqxDropDownList('getSelectedItem');
    if (result != null && result != undefined)
        return result.value;
    else null;
}

function getDescriptionSelectedModalidadPot() {
    return $("#ftrModalidadPotenciales").jqxDropDownList('getSelectedItem').label;
}

function getDescriptionSelectedMedicoPot() {
    return $("#ftrMedicoPotenciales").jqxDropDownList('getSelectedItem').label;
}

/*********************************************************************************************************/
function getSourceGridPotenciales(vGuardiasPotenciales) {
    //var hardCode = [{ "DiaDescripcion": "XX",
    //    "HoraEntrada": "0:0", "HoraSalida": "0:0",
    //    "Localidad": "XX",
    //    "MedicoDescripcion": "XX",
    //    "ModalidadDescripcion": "XX",
    //    "PotencialID": 0,
    //    "ZonaDescripcion": "XX"
    //}];
    var srcGridPotenciales = {
        datatype: "json",
        datafields: dtFieldsPotenciales,
        localdata: vGuardiasPotenciales.length == 0 ? [{}] : vGuardiasPotenciales,
    };
    var dtGridPotenciales = new $.jqx.dataAdapter(srcGridPotenciales);
    return dtGridPotenciales;
}

function loadGrid() {
    debugger;
    $("#grdPotenciales").jqxGrid(
        {
            width: '100%',
            autoheight: true,
            source: dtGridPotenciales,
            pageable: true,
            pagesize: 9,
            altrows: true,
            theme: 'arctic',
            columns: colGridPotenciales,
            showaggregates: true,
            showstatusbar: false,
            statusbarheight: 25,
            pagesizeoptions: ['9']
        });
}

function getSourcePotenciales() {
    $.ajax({
        url: 'Medicos/GetGuardiasPotenciales',
        dataType: 'json',
        //async: false,
        beforeSend: function () {
            $('#busy').show();
        },
        type: 'GET',
        success: function (guardiasPotencialesSources) {
            vGuardiasPotenciales = guardiasPotencialesSources.GuardiasPotenciales;
            vModalidadesPotenciales = guardiasPotencialesSources.Modalidades;
            vMedicosPotenciales = guardiasPotencialesSources.Medicos;
            vZonasPotenciales = guardiasPotencialesSources.Zonas;
            vDiasPotenciales = guardiasPotencialesSources.Dias;
            loadCombos();
            dtGridPotenciales = getSourceGridPotenciales(vGuardiasPotenciales);
            loadGrid();
            $('#busy').hide();
        }
    });
}
// --> Refrescar grilla cuando filtro informacion

$('#btnConsultarPotenciales').on('click', function () {
    $('#grdPotenciales').jqxGrid({ source: getSourceFiltradosPotenciales() });
});

$('#btnRefrescarPotenciales').on('click', function () {
    getSourcePotenciales();
});
/*********************************************************************************************************/

function getSourceFiltradosPotenciales()
{
    var modalidad = getSelectedModalidadPot();
    var medico = getSelectedMedicoPot();
    var dia = getSelectedDiaPot();
    var zona = getSelectedZonaPot();

    var sourceFiltrado = getSourceGridPotenciales($.grep(vGuardiasPotenciales, function (n, i) {
        var nofiltrado = true;
        if (modalidad != 0)
            nofiltrado = n.Modalidad.Id === modalidad;
        if (medico != 0)
            nofiltrado = nofiltrado && n.Medico.Id === medico;
        if (dia != 0)
            nofiltrado = nofiltrado && n.Dia.Id === dia;
        if (zona != 0)
            nofiltrado = nofiltrado && n.Zona.Id === zona;
        return nofiltrado;
    }));

    return sourceFiltrado;
};


//// --> Seteo alerta mas linda que el clasico alert. Le paso el mensaje y el tipo de alerta.
//function setAlert(msg, tipoMsg) {
//    Messenger().post({
//        message: msg,
//        type: tipoMsg,
//        showCloseButton: true
//    });
//}

