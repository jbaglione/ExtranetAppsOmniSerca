/**** DENUNCIAS ****/

// --> Filtro de fecha de denuncias
$("#dtDenuncias").jqxDateTimeInput({ width: '100%', height: '25px', theme: 'bootstrap', culture: 'es-AR', selectionMode: 'range' });
$("#dtDenuncias").jqxDateTimeInput('setRange', moment().subtract('days', 31).format('L'), moment().format('L'));

// --> Seteo las columnas para la grilla de servicios finalizados
var colGridDenuncias = [
                        { text: 'Nro', datafield: 'NroDenuncia', width: '4%', cellsalign: 'right' },
                        { text: 'Fecha/Hora', datafield: 'FecHorDenuncia', width: '7%', cellsalign: 'center' },
                        { text: 'Receptor', datafield: 'Receptor', width: '7%', cellsalign: 'center' },
                        { text: 'Empresa', datafield: 'Empresa', width: '7%', cellsalign: 'center' },
                        { text: 'CUIT', datafield: 'CUIT', width: '7%', cellsalign: 'center' },
                        { text: 'Empleado', datafield: 'Empleado', width: '9%', cellsalign: 'center' },
                        { text: 'CUIL', datafield: 'CUIL', width: '7%' },
                        { text: 'TipoAccidente', datafield: 'TipoAccidente', width: '7%' },
                        { text: 'Denunciante', datafield: 'Denunciante', width: '7%', cellsalign: 'center' },
                        { text: 'Telefono', datafield: 'Telefono', width: '7%' },
                        { text: 'Domicilio', datafield: 'Domicilio', width: '7%', cellsalign: 'center' },
                        { text: 'FecHorAccidente', datafield: 'FecHorAccidente', width: '7%', cellsalign: 'center' },
                        { text: 'Hecho', datafield: 'Hecho', width: '7%', cellsalign: 'center' },
                        { text: 'Observaciones', datafield: 'Observaciones', width: '7%', cellsalign: 'center' },
                        { text: 'Super', datafield: 'Super', width: '7%', cellsalign: 'center' },
                     ];

// --> Seteo los datafields para la grilla de servicios finalizados
var dtFieldsDenuncias = [{ name: 'NroDenuncia', type: 'string' }, { name: 'FecHorDenuncia', type: 'string' },
                        { name: 'Receptor', type: 'string' }, { name: 'Empresa', type: 'string' },
                        { name: 'CUIT', type: 'string' },
                        { name: 'Empleado', type: 'string' }, { name: 'CUIL', type: 'string' },
                        { name: 'TipoAccidente', type: 'string' }, { name: 'Denunciante', type: 'string' },
                        { name: 'Telefono', type: 'string' },
                        { name: 'Domicilio', type: 'string' }, { name: 'FecHorAccidente', type: 'string' },
                        { name: 'Hecho', type: 'string' }, { name: 'Observaciones', type: 'string' },
                        { name: 'Super', type: 'string' }
                        ];

// --> FUNCIONES PARA SERVICIOS FINALIZADOS

// --> Obtengo via ajax los datos de la grilla de servicios finalizados
function getSourceGrdDenuncias() {
    checkSessionStorage();
    var objDateTime = $("#dtDenuncias");
    var fd = getRangoFechas(0, objDateTime);
    var fh = getRangoFechas(1, objDateTime);

    var src = {
        datatype: "json",
        datafields: dtFieldsDenuncias,
        url: 'OperativaClientes/GetServiciosClientes',
        data:
            {
                fecDesde: fd,
                fecHasta: fh,
                pWS: 5
            }
    };

    var dt = new $.jqx.dataAdapter(src);

    return dt;
}

/**** TERMINA SERVICIOS FINALIZADOS ****/

//$('#popupValidarErroneos').modal('show');

$('#dtDenuncias').on('valuechanged', function (event) {

    $('#grdDenuncias').jqxGrid({ source: getSourceGrdDenuncias() });

    //refrescar source de grilla de servicios finalizados  
});

$('#exportarAExcelDenuncias').click(function () {

    var data = $("#grdDenuncias").jqxGrid('exportdata', 'json');
    downloadExcel(data, "denuncias");

});



// --> EJECUTO CUANDO INICIO

// --> Seteo la grilla de servicios en curso con el source, columnas y datafields


setValuesGrid($('#grdDenuncias'),
              getSourceGrdDenuncias(),
              colGridDenuncias);




