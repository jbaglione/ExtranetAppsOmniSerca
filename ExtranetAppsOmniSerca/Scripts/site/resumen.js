$("#btnConsultarResumen").jqxButton({ width: '100', theme: 'bootstrap', height: '27' });

// --> Seteo dropdownlist para seleccionar periodo en la grilla de servicios
// --> Uso mismo source que filtro de periodo de guardias

$("#ftrPeriodoResumen").jqxDropDownList({
    selectedIndex: 2, source: setFtrPeriodoGuardias(), displayMember: "Descripcion",
    valueMember: "Periodo", width: '110%', dropDownHeight: 80, height: 25, theme: 'bootstrap'
});

// --> Seteo dropdownlist para seleccionar medicos en la grilla de servicios
// --> Uso mismo source que filtro de medicos de guardias

$("#ftrMedicoResumen").jqxDropDownList({
    source: getSourceFiltroMedicos(), displayMember: "Nombre", selectedIndex: 0,
    valueMember: "UsuarioID", width: '110%', dropDownHeight: 150, dropDownWidth: 320, height: 25, theme: 'bootstrap'
});

function setSrcFtrCoordResumen() {

    var srcFtrCoordResumen = {
        datatype: "json",
        datafields: [
            { name: 'ID' },
            { name: 'Descripcion' }
        ],
        url: 'Medicos/getFiltroCoordinaciones?usr_id=' + getSelectedMedicoResumen(),
        async: false
    };

    var dtFtrCoordResumen = new $.jqx.dataAdapter(srcFtrCoordResumen);

    return dtFtrCoordResumen;

}

$("#ftrCoordResumen").jqxDropDownList({
    selectedIndex: 0, source: setSrcFtrCoordResumen(), displayMember: "Descripcion",
    valueMember: "ID", width: '110%', dropDownHeight: 80, height: 25, theme: 'bootstrap'
});

$('#ftrMedicoResumen').on('select', function (event) {

    $('#ftrCoordResumen').jqxDropDownList({ source: setSrcFtrCoordResumen() });

});

function getSelectedPeriodoResumen() {
    return $("#ftrPeriodoResumen").jqxDropDownList('getSelectedItem').value;
}

function getSelectedPeriodoResumenDesc() {
    return $("#ftrPeriodoResumen").jqxDropDownList('getSelectedItem').label;
}

function getSelectedCoordResumen() {
    var result =$('#ftrCoordResumen').jqxDropDownList('getSelectedItem');
    if (result != null && result != undefined)
        return result.value;
    else null;
}

function getSelectedMedicoResumen() {
    var result = $("#ftrMedicoResumen").jqxDropDownList('getSelectedItem');
    if (result != null && result != undefined)
        return result.value;
    else null;
}

/*********************************************************************************************************/


// --> Seteo datafields de grilla de resumen

var dtFieldsResumen = [{ name: 'Item', type: 'string' }, { name: 'Importe', type: 'number' }];
var dtFieldsHorarios = [{ name: 'DiaDeLaSemana', type: 'string' }, { name: 'Entrada1', type: 'string' },
                        { name: 'Salida1', type: 'string' }, { name: 'Movil1', type: 'string' },
                        { name: 'Entrada2', type: 'string' }, { name: 'Salida2', type: 'string' },
                        { name: 'Movil2', type: 'string' }, { name: 'Disponibilidad', type: 'string' },
                        { name: 'DiaNumero', type: 'string' }];

var ccnHorarios = function (row, column, value, data) {
	if (data.Disponibilidad === 0)
		return 'blanco';
	else
	    return 'amarillo';
}

var crHorarios = function (row, columnfield, value, defaulthtml, columnproperties, data) {
	if (data.Disponibilidad === 1) {
		return '<a href="javascript:delHorarioDisponible(' + data.DiaNumero + ')" style="margin-left:34%;line-height:33px"><span class="glyphicon glyphicon-remove-circle rojo icon-right-margin big-icon "></span></a>';
	} else {
		if (data.Salida1 === null) {
			return '<a href="javascript:setHorarioDisponible(' + data.DiaNumero + ')" style="margin-left:34%;line-height:33px"><span class="glyphicon glyphicon-plus green icon-right-margin big-icon "></span></a>';
		} else {
			return '';
		}
	}
}
var crItem = function (row, columnfield, value, defaulthtml, columnproperties) {
    var item = $("#grdResumen").jqxGrid('getrowdata', row).Item;
    if ((item == "Total a Facturar") || (item == "Anticipos Cobrados")) {
        return '<div style="text-align:center"><span style="line-height:25px;font-weight: bold">' + value + '</span></div>';
    }
}

var crImporte = function (row, columnfield, value, defaulthtml, columnproperties) {
    var item = $("#grdResumen").jqxGrid('getrowdata', row).Item;
    var importe = $("#grdResumen").jqxGrid('getrowdata', row).Importe.toString();
    if ((item == "Total a Facturar") || (item == "Anticipos Cobrados")) {

        if (importe.indexOf("-") == -1) {
            return '<div style="text-align:right"><span style="line-height:25px;font-weight: bold">' + '$' + numberWithCommas(parseFloat(importe).toFixed(2)) + '</span></div>';
        } else {
            importe = importe.replace("-", " ");
            return '<div style="text-align:right"><span style="line-height:25px;font-weight: bold">' + '($' + numberWithCommas(parseFloat(importe).toFixed(2)) + ')' + '</span></div>';
        }
    }
}


// --> Seteo source grilla de resumen

function getSourceGridResumen() {

    var srcGridResumen = {
        datatype: "json",
        datafields: dtFieldsResumen,
        url: 'Medicos/GetResumenLiquidacion',
        data: {
            periodo: getSelectedPeriodoResumen(),
            coordinacion: getSelectedCoordResumen(),
            medico: getSelectedMedicoResumen()
        }
    };

    var dtGridResumen = new $.jqx.dataAdapter(srcGridResumen);

    $('#txtResumenPeriodo').text('Resumen Liquidación: ' + getSelectedPeriodoResumenDesc());

    return dtGridResumen;
}

function getSourceGridHorarios() {

    var srcGridHorarios = {
        datatype: "json",
        datafields: dtFieldsHorarios,
        url: 'Medicos/GetHorarios',
        data: {
            periodo: getSelectedPeriodoResumen(),
            medico: getSelectedMedicoResumen()
        }
    };

    var dtGridHorarios = new $.jqx.dataAdapter(srcGridHorarios);

    return dtGridHorarios;
}



// --> Seteo columnas de la grilla de guardias

var colGridResumen = [
                        { text: 'Item', datafield: 'Item', width:'50%', cellsrenderer: crItem, cellsalign: 'center' },
                        { text: 'Importe', datafield: 'Importe', width: '50%',cellsrenderer: crImporte, cellsalign: 'right', cellsformat: 'c2' },
];

var colGridHorarios = [
                        { text: 'D&iacute;a', datafield: 'DiaDeLaSemana', width: '16%', cellsalign: 'center', cellclassname: ccnHorarios },
                        { text: 'Entrada 1', datafield: 'Entrada1', width: '16%', cellsalign: 'center', cellclassname: ccnHorarios },
                        { text: 'Salida 1', datafield: 'Salida1', width: '14%', cellsalign: 'center', cellclassname: ccnHorarios },
                        { text: 'M&oacute;vil 1', datafield: 'Movil1', width: '9%', cellsalign: 'center', cellclassname: ccnHorarios },
                        { text: 'Entrada 2', datafield: 'Entrada2', width: '16%', cellsalign: 'center', cellclassname: ccnHorarios },
                        { text: 'Salida 2', datafield: 'Salida2', width: '14%', cellsalign: 'center', cellclassname: ccnHorarios },
                        { text: 'M&oacute;vil 2', datafield: 'Movil2', width: '9%', cellsalign: 'center', cellclassname: ccnHorarios },
						{ text: '', width: '6%', cellsrenderer: crHorarios, cellclassname: ccnHorarios }
];

// --> Seteo objeto de la grilla de guardias con todos los valores

$("#grdResumen").jqxGrid(
{
    width: '99%',
    autoheight: true,
    source: getSourceGridResumen(),
    pageable: true,
    pagesize: 4,
    altrows: true,
    theme: 'arctic',
    columns: colGridResumen,
    pagesizeoptions: ['4']
});

$("#grdHorarios").jqxGrid(
{
    width: '99%',
    autoheight: true,
    source: getSourceGridHorarios(),
    pageable: true,
    pagesize: 7,
    altrows: true,
    theme: 'arctic',
    columns: colGridHorarios,
    pagesizeoptions: ['7']
});

$('#grdResumen').on('bindingcomplete', function (event) {
    $grid = $(this);
    $grid.jqxGrid('localizestrings', localizationobj);
    $grid.jqxGrid('gotopage', 1);
    $grid.jqxGrid('gotopage', 0);
});

$('#grdHorarios').on('bindingcomplete', function (event) {
    $grid = $(this);
    $grid.jqxGrid('localizestrings', localizationobj);
});

$('#btnConsultarResumen').on('click', function () {

    setSourceResumen();

});

function setSourceResumen() {

    $('#grdResumen').jqxGrid({ source: getSourceGridResumen() });

    $('#grdHorarios').jqxGrid({ source: getSourceGridHorarios() });

}

function delHorarioDisponible(diaNumero) {
	$.post("Medicos/delHorarioDisponible", { DiaNumero: diaNumero }
	).done(function () {
		setTimeout(function () {
			$('#grdHorarios').jqxGrid({ source: getSourceGridHorarios() });
		}, 5000);
	})
	  .fail(function () {
	  	alert("error");
	  });
}

function setHorarioDisponible(diaNumero) {
	$('#diaNumero').val(diaNumero);
	$('#popupHorario').modal('show');

	//$('#popupHorario').jqxWindow('open');
}

//$('#popupHorario').jqxWindow({
//	maxHeight: 200, maxWidth: 280, minHeight: 30, minWidth: 250, height: 200, width: 270,
//	resizable: false, isModal: true, modalOpacity: 0.3, autoOpen: false,
//	okButton: $('#popupHorariook'), cancelButton: $('#popupHorariocancel'),
//	initContent: function () {
//		$('#popupHorariook').jqxButton({ width: '65px' });
//		$('#popupHorariocancel').jqxButton({ width: '65px' });
//		$('#popupHorariook').focus();
//	}
//});
$("#popupHorariook").on('click', function () {
	var desde = $("#popupHorarioEntrada").jqxDateTimeInput('getDate');
	var hasta = $("#popupHorarioSalida").jqxDateTimeInput('getDate');
	var desdeText = desde.getHours() + ':' + desde.getMinutes();
	var hastaText = hasta.getHours() + ':' + hasta.getMinutes();
	$('#popupHorario').modal('hide');
	$.post("Medicos/setHorarioDisponible", { DiaNumero: $('#diaNumero').val(), Entrada1: desdeText, Salida1: hastaText }
	).done(function () {
		$('#grdHorarios').jqxGrid('showloadelement');
		setTimeout(function () {
			$('#grdHorarios').jqxGrid({ source: getSourceGridHorarios() });
			$('#grdHorarios').jqxGrid('hideloadelement');
		}, 5000);
	})
	  .fail(function () {
	  	alert("error");
	  });
});
$("#popupHorarioEntrada").jqxDateTimeInput({ width: '100%', height: '32', formatString: 'HH:mm', showCalendarButton: false, theme: 'bootstrap', textAlign: 'center' });
$("#popupHorarioSalida").jqxDateTimeInput({ width: '100%', height: '32', formatString: 'HH:mm', showCalendarButton: false, theme: 'bootstrap', textAlign: 'center' });