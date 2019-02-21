/**** SERVICIOS FINALIZADOS ****/

// --> VARIABLES PARA SERVICIOS FINALIZADOS

// --> Filtro de fecha de servicios finalizados
$("#dtServErroneos").jqxDateTimeInput({ width: '100%', height: '25px', theme: 'bootstrap', culture: 'es-AR', selectionMode: 'range' });
$("#dtServErroneos").jqxDateTimeInput('setRange', moment().subtract('days', 31).format('L'), moment().format('L'));

var crCorregir = function (row, columnfield, value, defaulthtml, columnproperties) {
    var glyph = '<div style="text-align:center;margin-top:2px"><a href="#" class="goCorregirErr"><span class="glyphicon glyphicon-pencil big-icon naranja"></span></a></div>';
    return glyph;
}

// --> Seteo las columnas para la grilla de servicios finalizados
var colGridErroneos = [
                        { text: 'ID', datafield: 'ID', hidden: true },
                        { text: 'Fecha', datafield: 'FecIncidente', width: '8%', cellsalign: 'center' },
                        { text: 'Inc', datafield: 'NroIncidente', width: '5%', cellsalign: 'center' },
                        { text: 'Cliente', datafield: 'ClienteID', width: '5%', cellsalign: 'center' },
                        { text: 'Nro. Afiliado', datafield: 'NroAfiliado', width: '8%', cellsalign: 'center' },
                        { text: 'Orden', datafield: 'NroInterno', width: '7%', cellsalign: 'center' },
                        { text: 'Paciente', datafield: 'Nombre', width: '16%' },
                        { text: 'Domicilio', datafield: 'Domicilio', width: '16%' },
                        { text: 'Grado', datafield: 'Grado', width: '5%', cellsalign: 'center' },
                        { text: 'Error', datafield: 'DescError', width: '17%' },
                        { text: 'F/H Proces.', datafield: 'FecHorProcesamiento', width: '8%', cellsalign: 'center' },
                        { text: 'Corregir', width: '5%', cellsalign: 'center', cellsrenderer: crCorregir }
];

// --> Seteo los datafields para la grilla de servicios finalizados
var dtFieldsErroneos = [{ name: 'ID', type: 'number' }, { name: 'FecIncidente', type: 'string' },
                        { name: 'NroIncidente', type: 'string' }, { name: 'ClienteID', type: 'string' },
                        { name: 'NroAfiliado', type: 'string' },
                        { name: 'NroInterno', type: 'string' }, { name: 'Nombre', type: 'string' },
                        { name: 'Domicilio', type: 'string' }, { name: 'DescError', type: 'string' },
                        { name: 'Grado', type: 'string' },
                        { name: 'FecHorProcesamiento', type: 'string' }];

// --> FUNCIONES PARA SERVICIOS FINALIZADOS

// --> Obtengo via ajax los datos de la grilla de servicios finalizados
function getSourceGrdErroneos() {
    checkSessionStorage();
    var objDateTime = $("#dtServErroneos");
    var fd = getRangoFechas(0, objDateTime);
    var fh = getRangoFechas(1, objDateTime);

    var src = {
        datatype: "json",
        datafields: dtFieldsErroneos,
        url: 'OperativaClientes/GetServiciosClientes',
        data:
            {
                fecDesde: fd,
                fecHasta: fh,
                pWS: 3
            }
    };

    var dt = new $.jqx.dataAdapter(src);

    return dt;
}

/**** TERMINA SERVICIOS FINALIZADOS ****/

//$('#popupValidarErroneos').modal('show');

$("#refreshErroneos").click(function () {
    $('#grdServErroneos').jqxGrid({ source: getSourceGrdErroneos() });
})

$('#dtServErroneos').on('valuechanged', function (event) {

    $('#grdServErroneos').jqxGrid({ source: getSourceGrdErroneos() });

    //refrescar source de grilla de servicios finalizados  
});

$('#exportarAExcelErroneos').click(function () {

    var data = $("#grdServErroneos").jqxGrid('exportdata', 'json');
    downloadExcel(data, "servicios_a_validar");

});

$(document).on('click', '.goCorregirErr', function () {

    var rowindex = $('#grdServErroneos').jqxGrid('getselectedrowindex');
    var row = $('#grdServErroneos').jqxGrid('getrowdata', rowindex);
    var idInc = row.ID;
    $('#popTxtPaciente').val(row.Nombre);
    $('#popTxtNroOrden').val(row.NroInterno);
    $('#popTxtNroAfiliado').val(row.NroAfiliado);
    $('#errNroInc').text(row.NroIncidente);
    $('#errIncID').val(idInc);

    $('#popupValidarErroneos').modal('show');

});

$('#btnCorregirErroneo').on('click', function () {
    checkSessionStorage();
    var nOrden = $('#popTxtNroOrden').val();
    var nAfiliado = $('#popTxtNroAfiliado').val();
    var incID = $('#errIncID').val();

    console.log(nOrden + '/' + nAfiliado + '/' + incID);

    if ( (nOrden == "") || (nAfiliado == "")) {

        setAlert('Debe completar Nro. de Orden y Nro. de Afiliado para corregir el servicio','error');

    } else {

        $.ajax({
            type: 'POST',
            cache: false,
            data: {
                'nOrden' : nOrden,
                'nAfiliado': nAfiliado,
                'incID' : incID
            },
            url: 'OperativaClientes/CorregirErroneo',
            success: function (retVal) {

                if (retVal.length > 1) {
                    setAlert('Error: ' + retVal, 'error');
                } else {
                    setAlert('La corrección fue realizada correctamente. <br/> El servicio se encuentra en estado de revalidación', 'success');
                    $('#popupValidarErroneos').modal('hide');
                    $('#grdServErroneos').jqxGrid({ source: getSourceGrdErroneos() });

                }

            },
            error: function(xhr,ajaxOptions, thrownError) {
                setAlert('No se pudo enviar la corrección del servicio','error');
            }

        });
    }
});

// --> EJECUTO CUANDO INICIO

// --> Seteo la grilla de servicios en curso con el source, columnas y datafields


setValuesGrid($('#grdServErroneos'),
              getSourceGrdErroneos(),
              colGridErroneos);




