var fechaInicio = moment().startOf('week');
var CeldaSeleccionada = [0,0];
checkSessionStorage();

$("#btnHorarioCoordinacionConsultar").jqxButton({ width: '100', theme: 'bootstrap', height: '26' });
$("#btnHorarioCoordinacionSiguiente").jqxButton({ width: '100', theme: 'bootstrap', height: '26' });
$("#btnHorarioCoordinacionAnterior").jqxButton({ width: '100', theme: 'bootstrap', height: '26' });
$("#popupCHEntrada").jqxDateTimeInput({ width: '100%', height: '32', formatString: 'HH:mm', showCalendarButton: false, theme: 'bootstrap', textAlign: 'center' });
$("#popupCHSalida").jqxDateTimeInput({ width: '100%', height: '32', formatString: 'HH:mm', showCalendarButton: false, theme: 'bootstrap', textAlign: 'center' });

$('#btnHorarioCoordinacionConsultar').on('click', function () {
	grilla(getDias());
});
$('#btnHorarioCoordinacionAnterior').on('click', function () {
	fechaInicio.subtract(7, 'd');
	grilla(getDias());
});
$('#btnHorarioCoordinacionSiguiente').on('click', function () {
	fechaInicio.add(7, 'd');
	grilla(getDias());
});
$("#ftrCoordinacion").jqxDropDownList({
	selectedIndex: 0, 
	source: new $.jqx.dataAdapter({
		datatype: "json",
		datafields: [
            { name: 'ID' },
            { name: 'Descripcion' }
		],
		url: 'Medicos/GetFiltroHorarioCoordinacionList',
		async: false
	}), 
	displayMember: "Descripcion",
	valueMember: "ID", width: '250px', dropDownHeight: 110, height: 25, theme: 'bootstrap'
});


function getSourceGridHorariosCoordinacion() {
    checkSessionStorage();
	var src = {
		datatype: "json",
		datafields: [
			{ name: 'Legajo', type: 'string' },
			{ name: 'Medico', type: 'string' },
			{ name: 'ID1', type: 'string' },
			{ name: 'Entrada1', type: 'string' },
			{ name: 'Salida1', type: 'string' },
			{ name: 'Movil1', type: 'string' },
			{ name: 'ID2', type: 'string' },
			{ name: 'Entrada2', type: 'string' },
			{ name: 'Salida2', type: 'string' },
			{ name: 'Movil2', type: 'string' },
			{ name: 'ID3', type: 'string' },
			{ name: 'Entrada3', type: 'string' },
			{ name: 'Salida3', type: 'string' },
			{ name: 'Movil3', type: 'string' },
			{ name: 'ID4', type: 'string' },
			{ name: 'Entrada4', type: 'string' },
			{ name: 'Salida4', type: 'string' },
			{ name: 'Movil4', type: 'string' },
			{ name: 'ID5', type: 'string' },
			{ name: 'Entrada5', type: 'string' },
			{ name: 'Salida5', type: 'string' },
			{ name: 'Movil5', type: 'string' },
			{ name: 'ID6', type: 'string' },
			{ name: 'Entrada6', type: 'string' },
			{ name: 'Salida6', type: 'string' },
			{ name: 'Movil6', type: 'string' },
			{ name: 'ID7', type: 'string' },
			{ name: 'Entrada7', type: 'string' },
			{ name: 'Salida7', type: 'string' },
			{ name: 'Movil7', type: 'string' },

		],
		url: 'Medicos/GetHorarioCoordinacion',
		data: {
			coordinacion: getSelectedCoordinacion(),
			fecha: fechaInicio.format("DD/MM/YYYY"),
		}
	};

	var dt = new $.jqx.dataAdapter(src);

	return dt;

}
function grilla(dias) {
	$("#grdHorarioCoordinacion").jqxGrid(
	{
		width: '100%',
		autoheight: true,
		source: getSourceGridHorariosCoordinacion(),
		pageable: true,
		altrows: true,
		pagesize: 8,
		showfilterrow: true,
		filterable: true,
		theme: 'arctic',
		pagesizeoptions: ['8'],
		selectionmode:'singlecell',
		columns: [
			{ text: 'Legajo', datafield: 'Legajo' },
			{ text: 'Medico', datafield: 'Medico', width: '150px' },
			{ text: 'Entrada', datafield: 'Entrada1', columngroup: 'dia1',filterable :false },
			{ text: 'Salida', datafield: 'Salida1', columngroup: 'dia1', filterable: false },
			{ text: 'Móvil', datafield: 'Movil1', columngroup: 'dia1', filterable: false },
			{ text: 'Entrada', datafield: 'Entrada2', columngroup: 'dia2', filterable: false },
			{ text: 'Salida', datafield: 'Salida2', columngroup: 'dia2', filterable: false },
			{ text: 'Móvil', datafield: 'Movil2', columngroup: 'dia2', filterable: false },
			{ text: 'Entrada', datafield: 'Entrada3', columngroup: 'dia3', filterable: false },
			{ text: 'Salida', datafield: 'Salida3', columngroup: 'dia3', filterable: false },
			{ text: 'Móvil', datafield: 'Movil3', columngroup: 'dia3', filterable: false },
			{ text: 'Entrada', datafield: 'Entrada4', columngroup: 'dia4', filterable: false },
			{ text: 'Salida', datafield: 'Salida4', columngroup: 'dia4', filterable: false },
			{ text: 'Móvil', datafield: 'Movil4', columngroup: 'dia4', filterable: false },
			{ text: 'Entrada', datafield: 'Entrada5', columngroup: 'dia5', filterable: false },
			{ text: 'Salida', datafield: 'Salida5', columngroup: 'dia5', filterable: false },
			{ text: 'Móvil', datafield: 'Movil5', columngroup: 'dia5', filterable: false },
			{ text: 'Entrada', datafield: 'Entrada6', columngroup: 'dia6', filterable: false },
			{ text: 'Salida', datafield: 'Salida6', columngroup: 'dia6', filterable: false },
			{ text: 'Móvil', datafield: 'Movil6', columngroup: 'dia6', filterable: false },
			{ text: 'Entrada', datafield: 'Entrada7', columngroup: 'dia7', filterable: false },
			{ text: 'Salida', datafield: 'Salida7', columngroup: 'dia7', filterable: false },
			{ text: 'Móvil', datafield: 'Movil7', columngroup: 'dia7', filterable: false },
		],
		columngroups:
		[
			{ text: dias[0], align: 'center', name: 'dia1' },
			{ text: dias[1], align: 'center', name: 'dia2' },
			{ text: dias[2], align: 'center', name: 'dia3' },
			{ text: dias[3], align: 'center', name: 'dia4' },
			{ text: dias[4], align: 'center', name: 'dia5' },
			{ text: dias[5], align: 'center', name: 'dia6' },
			{ text: dias[6], align: 'center', name: 'dia7' },
		],
	});
}

function getDias() {
	var dias = [];
	dias.push(fechaInicio.format('DD/MM/YYYY'));
	dias.push(fechaInicio.add(1, 'd').format('DD/MM/YYYY'));
	dias.push(fechaInicio.add(1, 'd').format('DD/MM/YYYY'));
	dias.push(fechaInicio.add(1, 'd').format('DD/MM/YYYY'));
	dias.push(fechaInicio.add(1, 'd').format('DD/MM/YYYY'));
	dias.push(fechaInicio.add(1, 'd').format('DD/MM/YYYY'));
	dias.push(fechaInicio.add(1, 'd').format('DD/MM/YYYY'));
	fechaInicio.subtract(6, 'd');

	return dias;
}

function getSelectedCoordinacion() {
	return $("#ftrCoordinacion").jqxDropDownList('getSelectedItem').value;
}

$(function () {
	grilla(getDias());
	$("#grdHorarioCoordinacion").on("celldoubleclick", function (event) {
		var args = event.args;
		if (args.columnindex > 1) {
			var idx = Math.floor((args.columnindex - 2) / 3);
			dias = getDias();
			if (moment(dias[idx], 'DD/MM/YYYY').isBefore()) {
				$("#popupHorarioNotAllowed").modal('show', { backdrop: 'static' });
				return;
			}
			$("#popupCHFecha").val(dias[idx]);
			$("#popupCHFechaSalida").val(dias[idx]);
			var selectedRowData = $('#grdHorarioCoordinacion').jqxGrid('getrowdata', args.rowindex);

			var result = GetRowData(idx, selectedRowData);
			$('#popupCHId').val(result.id);
			$('#popupCHPersonalId').val(selectedRowData.Legajo);
			$('#popupCHEntrada').jqxDateTimeInput('setDate', result.entrada.format());
			$('#popupCHSalida').jqxDateTimeInput('setDate', result.salida.format());
			$('#popupCHMovil').val(result.movil);
			if (result.entrada > result.salida) {
				$("#popupCHFechaSalida").val(moment(dias[idx], 'DD/MM/YYYY').add(1, 'd').format('DD/MM/YYYY'));
			}

			$('#popupEditarHorario').modal('show', { backdrop: 'static' });
		}

	});
	$('#popupCHSalida').on('change', function (event) {
		$("#popupCHFechaSalida").val($("#popupCHFecha").val());
		var entrada= $('#popupCHEntrada').jqxDateTimeInput('getDate'); 
		var salida= $('#popupCHSalida').jqxDateTimeInput('getDate'); 
		if (entrada > salida) {
			$("#popupCHFechaSalida").val(moment($("#popupCHFecha").val(), 'DD/MM/YYYY').add(1, 'd').format('DD/MM/YYYY'));
		}
	});
	var contextMenu = $("#Menu").jqxMenu({ width: 200, height: 58, autoOpenPopup: false, mode: 'popup' });

	$("#grdHorarioCoordinacion").on('contextmenu', function () {
		return false;
	});

	$("#Menu").on('itemclick', function (event) {
		var args = event.args;
		if ($.trim($(args).text()) == "Borrar") {
			dias = getDias();
			var idx = CeldaSeleccionada[1];
			var row = CeldaSeleccionada[0];
			var selectedRowData = $('#grdHorarioCoordinacion').jqxGrid('getrowdata', row);
			console.log(selectedRowData);
			var result = GetRowData(idx, selectedRowData);
			
			$("#popupBCHId").val(result.id);
			$("#popupBCHFecha").val(dias[idx]);
			$("#popupBCHFechaSalida").val(dias[idx]);
			$('#popupBCHEntrada').val(result.entrada.format('HH:mm'));
			$('#popupBCHSalida').val(result.salida.format('HH:mm'));
			$('#popupBCHMovil').val(result.movil);
			if (result.entrada > result.salida) {
				$("#popupBCHFechaSalida").val(moment(dias[idx], 'DD/MM/YYYY').add(1, 'd').format('DD/MM/YYYY'));
			}
			$('#popupBorrarHorario').modal('show', { backdrop: 'static' });
		}
	});

	$("#grdHorarioCoordinacion").on('cellselect', function (event) {
		// event arguments.
		var args = event.args;
		CeldaSeleccionada[1] = parseInt(args.datafield.substr(args.datafield.length - 1)) - 1;
		CeldaSeleccionada[0] = args.rowindex;
	});

	$("#grdHorarioCoordinacion").on('rowclick', function (event) {
		if (event.args.rightclick) {
			dias = getDias();
			var idx = CeldaSeleccionada[1];
			if (moment(dias[idx], 'DD/MM/YYYY').isBefore()) {
				console.log('no se puede');
				return;
			}
			var scrollTop = $(window).scrollTop();
			var scrollLeft = $(window).scrollLeft();
			contextMenu.jqxMenu('open', parseInt(event.args.originalEvent.clientX) + 5 + scrollLeft, parseInt(event.args.originalEvent.clientY) + 5 + scrollTop);
			return false;
		}
	});
	$('#popupCHEntrada').on('change', function () {
		if ($("#popupCHEntrada").val() > $("#popupCHSalida").val()) {
			$("#popupCHFechaSalida").val(moment($("#popupCHFecha").val(), 'DD/MM/YYYY').add(1, 'd').format('DD/MM/YYYY'));
		}

	});
	$('#popupCHSalida').on('change', function () {
	});
	$('#popupCHMovil').on('change', function () {
	    checkSessionStorage();
		$.post("Medicos/ValidateMovil", {
			fecha: $('#popupCHFecha').val(),
			movil: $('#popupCHMovil').val(),
		}
		).done(function (data) {
			$('#popupCHMovilDescripcion').val(data);
		}).fail(function () {
			alert("error");
		});
	});
	$('#popupCHok').on('click', function () {
	    checkSessionStorage();
		$.post("Medicos/SetCoordinacionHorario", {
			Id:$('#popupCHId').val(),
			FecEntrada: $('#popupCHFecha').val(),
			HorEntrada: moment($('#popupCHEntrada').jqxDateTimeInput('getDate')).format('HH:mm'),
			FecSalida: $('#popupCHFechaSalida').val(),
			HorSalida: moment($('#popupCHSalida').jqxDateTimeInput('getDate')).format('HH:mm'),
			MovilId: $('#popupCHMovil').val(),
			PersonalId: $('#popupCHPersonalId').val(),
		}
		).done(function () {
			$('#popupEditarHorario').modal('hide');
			$('#grdHorarioCoordinacion').jqxGrid('showloadelement');
			setTimeout(function () {
				$('#grdHorarioCoordinacion').jqxGrid({ source: getSourceGridHorariosCoordinacion() });
				$('#grdHorarioCoordinacion').jqxGrid('hideloadelement');
			}, 5000);
		}).fail(function () {
  			alert("error");
		});

	});

	$('#popupBCHok').on('click', function () {
	    checkSessionStorage();
		$.post("Medicos/DelCoordinacionHorario", {
			Id: $('#popupBCHId').val()
		}
		).done(function () {
			$('#popupBorrarHorario').modal('hide');
			$('#grdHorarioCoordinacion').jqxGrid('showloadelement');
			setTimeout(function () {
				$('#grdHorarioCoordinacion').jqxGrid({ source: getSourceGridHorariosCoordinacion() });
				$('#grdHorarioCoordinacion').jqxGrid('hideloadelement');
			}, 5000);

		}).fail(function () {
			alert("error");
		});

	});

});

function GetRowData(idx, selectedRowData) {
	var entrada = moment().startOf('day');
	var salida = moment().startOf('day');
	var movil = '';
	var id = 0;
	if (idx == 0) {
		if (selectedRowData.ID1 > 0) {
			entrada = moment(dias[idx] + ' ' + selectedRowData.Entrada1, 'DD/MM/YYYY HH:mm');
			salida = moment(dias[idx] + ' ' + selectedRowData.Salida1, 'DD/MM/YYYY HH:mm');
			movil = selectedRowData.Movil1;
			id = selectedRowData.ID1;
		}
	}
	if (idx == 1) {
		if (selectedRowData.ID2 > 0) {
			entrada = moment(dias[idx] + ' ' + selectedRowData.Entrada2, 'DD/MM/YYYY HH:mm');
			salida = moment(dias[idx] + ' ' + selectedRowData.Salida2, 'DD/MM/YYYY HH:mm');
			movil = selectedRowData.Movil2;
			id = selectedRowData.ID2;
		}
	}
	if (idx == 2) {
		if (selectedRowData.ID3 > 0) {
			entrada = moment(dias[idx] + ' ' + selectedRowData.Entrada3, 'DD/MM/YYYY HH:mm');
			salida = moment(dias[idx] + ' ' + selectedRowData.Salida3, 'DD/MM/YYYY HH:mm');
			movil = selectedRowData.Movil3;
			id = selectedRowData.ID3;
		}
	}
	if (idx == 3) {
		if (selectedRowData.ID4 > 0) {
			entrada = moment(dias[idx] + ' ' + selectedRowData.Entrada4, 'DD/MM/YYYY HH:mm');
			salida = moment(dias[idx] + ' ' + selectedRowData.Salida4, 'DD/MM/YYYY HH:mm');
			movil = selectedRowData.Movil4;
			id = selectedRowData.ID4;
		}
	}
	if (idx == 4) {
		if (selectedRowData.ID5 > 0) {
			entrada = moment(dias[idx] + ' ' + selectedRowData.Entrada5, 'DD/MM/YYYY HH:mm');
			salida = moment(dias[idx] + ' ' + selectedRowData.Salida5, 'DD/MM/YYYY HH:mm');
			movil = selectedRowData.Movil5;
			id = selectedRowData.ID5;
		}
	}
	if (idx == 5) {
		if (selectedRowData.ID6 > 0) {
			entrada = moment(dias[idx] + ' ' + selectedRowData.Entrada6, 'DD/MM/YYYY HH:mm');
			salida = moment(dias[idx] + ' ' + selectedRowData.Salida6, 'DD/MM/YYYY HH:mm');
			movil = selectedRowData.Movil6;
			id = selectedRowData.ID6;
		}
	}
	if (idx == 6) {
		if (selectedRowData.ID7 > 0) {
			entrada = moment(dias[idx] + ' ' + selectedRowData.Entrada7, 'DD/MM/YYYY HH:mm');
			salida = moment(dias[idx] + ' ' + selectedRowData.Salida7, 'DD/MM/YYYY HH:mm');
			movil = selectedRowData.Movil7;
			id = selectedRowData.ID7;
		}
	}
	return {
		entrada:entrada,
		salida: salida,
		movil: movil,
		id:id
	}
}
function getIndex(field) {

}
