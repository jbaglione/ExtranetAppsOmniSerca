/**** SERVICIOS EN CURSO ****/

var crReclamarEnCurso = function (row, columnfield, value, defaulthtml, columnproperties) {
    var glyph = '<div style="text-align:center;margin-top:2px"><a href="#" class="goRecEnCurso"><span class="glyphicon glyphicon-exclamation-sign big-icon naranja"></span></a></div>';
    return glyph;
}

// --> Seteo las columnas para la grilla de servicios en curso
var colGridOpEnCurso = [
                        { text: 'ID', datafield: 'ID', hidden: true },
                        { text: 'Inc', datafield: 'NroInc', width: '5%', cellsalign: 'center' },
                        { text: 'Orden', datafield: 'NroInterno', width: '10%', cellsalign: 'center' },
                        { text: 'Paciente', datafield: 'Nombre', width: '22%' },
                        { text: 'Domicilio', datafield: 'Domicilio', width: '20%' },
                        { text: 'Loc', datafield: 'Localidad', width: '5%', cellsalign: 'center' },
                        { text: 'Grado', datafield: 'Grado', width: '5%', cellsalign: 'center' },
                        { text: 'Móvil', datafield: 'WebMovil', width: '5%', cellsalign: 'center' },
                        { text: 'Arribo', datafield: 'Arribo', width: '5%', cellsalign: 'center' },
                        { text: 'Estado', datafield: 'WebEstado', width: '16%', cellsalign: 'center' },
                        { text: 'Reclamar', width: '7%', cellsalign: 'center', cellsrenderer: crReclamarEnCurso }
];

// --> Seteo los datafields para la grilla de servicios en curso
var dtFieldsOpEnCurso = [{ name: 'ID', type: 'number' }, { name: 'NroInc', type: 'string' },
                        { name: 'NroInterno', type: 'string' }, { name: 'Nombre', type: 'string' },
                        { name: 'Domicilio', type: 'string' }, { name: 'Localidad', type: 'string' },
                        { name: 'Grado', type: 'string' }, { name: 'WebMovil', type: 'string' },
                        { name: 'Arribo', type: 'string' }, { name: 'WebEstado', type: 'string' }];

// --> Obtengo via ajax los datos de la grilla de servicios en curso
function getSourceGrdOpEnCurso() {
    checkSessionStorage();
    var src = {
        datatype: "json",
        datafields: dtFieldsOpEnCurso,
        url: 'OperativaClientes/GetServiciosClientes',
        data: {
            pWS: 1
        }
    };

    var dt = new $.jqx.dataAdapter(src);

    return dt;
}

// --> Seteo la grilla de servicios en curso con el source, columnas y datafields
$("#grdOperativaEnCurso").jqxGrid(
{
    width: '100%',
    autoheight: true,
    source: getSourceGrdOpEnCurso(),
    pageable: true,
    pagesize: 15,
    altrows: true,
    theme: 'arctic',
    columns: colGridOpEnCurso,
    pagesizeoptions: ['15']
});

// --> Función para refrescar la grilla de servicios en curso cada 20 segundos
function refreshOpEnCurso() {
    $('#grdOperativaEnCurso').jqxGrid({ source: getSourceGrdOpEnCurso() });
    setActualTime($('#horarioActualEnCurso'));
}

// --> Función para setear el horario actual
function setActualTime(objHorario) {
    objHorario.text(moment().format('HH:mm:ss'));
}

// --> Apenas inicializo, seteo el horario actual
setActualTime($('#horarioActualEnCurso'));

// --> Apenas inicializo, seteo el timer para que refresque la grilla de servicios en curso cada 15 segs
setInterval("refreshOpEnCurso()", 20000);

$(document).on('click', '.goRecEnCurso', function () {
    checkSessionStorage();
    $('#ecObservaciones').val('');
    var rowindex = $('#grdOperativaEnCurso').jqxGrid('getselectedrowindex');
    var row = $('#grdOperativaEnCurso').jqxGrid('getrowdata', rowindex);
    var idInc = row.ID;

    $.ajax({
        type: 'GET',
        url: 'OperativaClientes/IsReclamado',
        cache: false,
        data: { 'idInc': idInc },
        success: function (retVal) {

            retVal = parseInt(retVal);
            if (retVal == 0) {

                $('#ecNroInc').val(row.NroInc);
                $('#ecNroOrden').val(row.NroInterno);
                $('#ecPaciente').val(row.Nombre);
                $('#ecEstado').val(row.WebEstado);
                $('#ecLblNroInc').text(row.NroInc);
                $('#ecIncID').val(idInc);
                $('#popupReclamarEnCurso').modal('show');

            } else if (retVal == 1) {
                setAlert('El incidente ya fue reclamado.', 'error');
            } else {
                setAlert('Error, no se pudo comprobar el estado del reclamo.', 'error');
            }

        },
        error: function (xhr, ajaxOptions, thrownError) {
            setAlert('Error, no se pudo comprobar el estado del reclamo', 'error');
        }
    });
});

$('#popupReclamarEnCurso').on('shown.bs.modal', function (e) {
    $('#ecObservaciones').focus();
});

$('#btnReclamarEnCurso').on('click', function () {
    checkSessionStorage();
    var idInc = $('#ecIncID').val();
    var observ = $('#ecObservaciones').val();

    if (observ == "") {
        setAlert('Debe completar el campo observaciones para enviar el reclamo', 'error');
    } else {

        $.ajax({
            type: 'POST',
            cache: false,
            data:
                {
                    'idInc': idInc,
                    'observ': observ
                },
            url: 'OperativaClientes/SetReclamoEnCurso',
            success: function (retVal) {
                if (retVal.length > 1) {
                    setAlert('Error: ' + retVal, 'error');
                } else {
                    setAlert('El reclamo fue realizado satisfactoriamente', 'success');
                    $('#popupReclamarEnCurso').modal('hide');
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                setAlert('No se pudo enviar el reclamo', 'error');
            }
        });

    }

});


/**** TERMINA SERVICIOS EN CURSO ****/