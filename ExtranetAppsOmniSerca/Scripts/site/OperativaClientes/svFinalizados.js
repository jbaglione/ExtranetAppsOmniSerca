/**** SERVICIOS FINALIZADOS ****/

// --> VARIABLES PARA SERVICIOS FINALIZADOS

// --> Filtro de fecha de servicios finalizados
$("#dtServFinalizados").jqxDateTimeInput({ width: '100%', height: '25px', theme: 'bootstrap', culture: 'es-AR', selectionMode: 'range' });

$("#toggleFilterRow").click(function () {
   
    var bRow = $('#grdServFinalizados').jqxGrid('showfilterrow') == true ? false : true;
    $('#grdServFinalizados').jqxGrid({ showfilterrow: bRow });

});

$("#ftrClientesFinalizados").jqxDropDownList({
    source: getSrcClientesFinalizados(), displayMember: "Codigo", selectedIndex: 0,
    valueMember: "ID", width: '100%', dropDownHeight: 150, dropDownWidth: 320, height: 25, theme: 'bootstrap'
});

//$('#rowHeaderFinalizados').show();

// --> Seteo las columnas para la grilla de servicios finalizados
var colGridFinalizados = [
                        { text: 'ID', datafield: 'ID', hidden: true },
                        { text: 'Fecha', datafield: 'FecIncidente', width: '7%', cellsalign: 'center' },
                        { text: 'Inc', datafield: 'NroIncidente', width: '3%', cellsalign: 'center' },
                        //{ text: 'Cliente', datafield: 'ClienteID', width: '5%', cellsalign: 'center' },
                        //{ text: 'Llamada', datafield: 'Llamada', width: '3%', cellsalign: 'center' },
                        { text: 'Nro. Int.', datafield: 'NroInterno', width: '5%'},
                        { text: 'Nro. Afiliado', datafield: 'IntegranteID', width: '8%', cellsalign: 'center' },
                        { text: 'Paciente', datafield: 'Nombre', width: '13%' },
                        { text: 'Domicilio', datafield: 'Domicilio', width: '11%' },
                        { text: 'Loc', datafield: 'Localidad', width: '9%' },
                        { text: 'Síntoma', datafield: 'Sintoma', width: '12%' },
                        { text: 'Grado', datafield: 'Grado', width: '5%', cellsalign: 'center' },
                        { text: 'Arribo', datafield: 'Arribo', width: '4%', cellsalign: 'center' },
                        { text: 'Diagnóstico', datafield: 'Diagnostico', width: '13%' },
                        { text: 'Deriva', datafield: 'Deriva', width: '6%', cellsalign: 'center' },
                        { text: 'Final', datafield: 'Final', width: '4%', cellsalign: 'center' }
];

// --> Seteo los datafields para la grilla de servicios finalizados
var dtFieldsFinalizados = [{ name: 'ID', type: 'number' }, { name: 'FecIncidente', type: 'string' },
                        { name: 'NroIncidente', type: 'string' },
                        //{ name: 'ClienteID', type: 'string' },
                        //{ name: 'Llamada', type: 'string' },
                        { name : 'NroInterno', type: 'string'},
                        { name: 'IntegranteID', type: 'string' },
                        { name: 'Nombre', type: 'string' }, { name: 'Domicilio', type: 'string' },
                        { name: 'Localidad', type: 'string' }, { name: 'Sintoma', type: 'string' },
                        { name: 'Grado', type: 'string' },
                        { name: 'Arribo', type: 'string' },
                        { name: 'Diagnostico', type: 'string' }, { name: 'Deriva', type: 'string' },
                        { name: 'Final', type: 'string' }];

// --> FUNCIONES PARA SERVICIOS FINALIZADOS

function getSelectedCliente() {
    return $("#ftrClientesFinalizados").jqxDropDownList('getSelectedItem').value;
}

function getSrcClientesFinalizados() {
    checkSessionStorage();
    var src = {
        datatype: "json",
        datafields: [
            { name: 'ID' },
            { name: 'Codigo' }
        ],
        url: 'OperativaClientes/getServiciosClientes',
        data: { pWS : 4},
        async: false
    };

    var dt = new $.jqx.dataAdapter(src);

    return dt;

}

// --> Obtengo via ajax los datos de la grilla de servicios finalizados
function getSourceGrdFinalizados() {
    checkSessionStorage();
    var objDatetime = $('#dtServFinalizados');
    var fd = getRangoFechas(0, objDatetime);
    var fh = getRangoFechas(1, objDatetime);

    var src = {
        datatype: "json",
        datafields: dtFieldsFinalizados,
        url: 'OperativaClientes/GetServiciosClientes',
        data:
            {
                fecDesde: fd,
                fecHasta: fh,
                pWS: 2,
                pCli: getSelectedCliente()
            }
    };

    var dt = new $.jqx.dataAdapter(src);

    return dt;
}

function getRangoFechas(idx, objDateTime) {
    var jsDate = objDateTime.jqxDateTimeInput('getText');
    var vDate = jsDate.split("-");
    var dateCalculate = vDate[idx].trim();
    var vMesAnioDia = dateCalculate.split("/");
    return vMesAnioDia[2] + vMesAnioDia[1] + vMesAnioDia[0];
}


/**** TERMINA SERVICIOS FINALIZADOS ****/

function setValuesGrid(objGrid, src, columnas) {

    objGrid.jqxGrid(
        {
            width: '100%',
            autoheight: true,
            source: src,
            pageable: true,
            pagesize: 15,
            altrows: true,
            theme: 'arctic',
            filterable: true,
            columns: columnas,
            pagesizeoptions: ['15']
        });

}

$('#exportarAExcel').click(function () {

    var data = $("#grdServFinalizados").jqxGrid('exportdata', 'json');
    downloadExcel(data, "servicios_finalizados");

});

$('#dtServFinalizados').on('valuechanged', function (event) {

    $('#grdServFinalizados').jqxGrid({ source: getSourceGrdFinalizados() });

    //refrescar source de grilla de servicios finalizados  
});

$('#ftrClientesFinalizados').on('select', function (event) {

    $('#grdServFinalizados').jqxGrid({ source: getSourceGrdFinalizados() });

});

// --> EJECUTO CUANDO INICIO

// --> Seteo la grilla de servicios en curso con el source, columnas y datafields

setValuesGrid($('#grdServFinalizados'),
              getSourceGrdFinalizados(),
              colGridFinalizados);


