var slider;
function servicio(servicio) {
    checkSessionStorage();
	$("#servicioID").val(servicio);
	$('#servicioModal').modal('show');
	$.getJSON("/facturacion/getUrlImagenes", { servicioId: servicio }, function (data) {
		$(".bxslider").empty();
		for (var i = 0; i < data.length; i++) {
			$(".bxslider").append('<li><img src="' + data[i] + '"/></li>');
		}
	});
	var modal = $('#servicioModal'), dialog = modal.find('.modal-dialog');
	modal.css('display', 'block');
	dialog.css("margin-top", 0);
}
function descargaServicio() {
    checkSessionStorage();
	var servicio = $("#servicioID").val();
	window.location = '/facturacion/getimagenes?servicioid=' + servicio;
}
function descargaPDF() {
	var comprobante = $("#comprobanteID").val();
	window.location = '/facturacion/getpdf?docid='+comprobante;
}
function descargaXLS() {
    checkSessionStorage();
	var comprobante = $("#comprobanteID").val();
	window.location = '/facturacion/GetServiciosXls';
}
function descargaImagenes() {
    checkSessionStorage();
	var comprobante = $("#comprobanteID").val();
	window.location = '/facturacion/GetServiciosImagenes';
}
function descarga(comprobante, tipo, numero) {
    checkSessionStorage();
	$("#comprobanteID").val(comprobante);
	$('#the-canvas').hide();
	$('#comprobanteModal').modal('show');
	$('#comprobanteModal > .modal-dialog > .modal-content > .modal-header > .modal-title').html("Comprobante: "+tipo+"-"+numero);

	PDFJS.getDocument('/facturacion/getpdf?docid=' + comprobante).then(function (pdf) {
		pdf.getPage(1).then(function (page) {
			var scale = 1.5;
			var viewport = page.getViewport(scale);

			var canvas = document.getElementById('the-canvas');
			var context = canvas.getContext('2d');
			canvas.height = viewport.height;
			canvas.width = viewport.width;

			var renderContext = {
				canvasContext: context,
				viewport: viewport
			};
			page.render(renderContext);
			$('#comprobanteModal > .modal-dialog > .modal-content > .modal-body > .row').fadeOut();
			$('#the-canvas').fadeIn();
			var modal = $('#comprobanteModal'), dialog = modal.find('.modal-dialog');
			modal.css('display', 'block');
			dialog.css("margin-top", 0);
		}); 
	});
}
function calculo(servicio, numero) {
    checkSessionStorage();
	$('#calculoModal').modal('show');
	$('#calculoModal > .modal-dialog > .modal-content > .modal-header > .modal-title').html("Apertura Valorización Servicio " + numero);
	var comprobante = $("#comprobanteID").val();

	$("#grdCalculo").jqxGrid(
	{
		width: '100%',
		autoheight: true,
		source: getSourceGridCalculo(comprobante, servicio),
		pageable: true,
		pagesize: 15,
		altrows: true,
		theme: 'arctic',
		columns: colGridCalculo,
		pagesizeoptions: ['15']
	});
}
function detalle(comprobante) {
    checkSessionStorage();
	$('#myModal').modal('show');
	$("#comprobanteID").val(comprobante);
	$("#grdServicios").jqxGrid(
	{
		width: '100%',
		autoheight: true,
		source: getSourceGridServicios(comprobante),
		pageable: true,
		pagesize: 15,
		altrows: true,
		theme: 'arctic',
		showfilterrow: true,
		filterable: true,
		columns: colGridServicios,
		localization: { emptydatastring: 'No se encuentran prestaciones imputadas' },
		pagesizeoptions: ['15']
	});
}
var descargarButton = function (row, columnfield, value, defaulthtml, columnproperties) {
	var data = $("#grdComprobantes").jqxGrid('getrowdata', row)
	return '<div style="margin: 2px;text-align: center;"><a class="btn btn-primary btn-xs" onclick="descarga(' + data.ID + ',\'' + data.TipoComprobante + '\',\'' + data.NroComprobante + '\')" href="#"><i class="fa fa-file-pdf-o"></i></a></div>'
}
var abrirButton = function (row, columnfield, value, defaulthtml, columnproperties) {
	var data = $("#grdComprobantes").jqxGrid('getrowdata', row)
	return '<div style="margin: 2px;text-align: center;"><a class="btn btn-info btn-xs" onclick="detalle(' + data.ID + ')" href="#"><i class="fa fa-list"></i></a></div>'
}
var calculoButton = function (row, columnfield, value, defaulthtml, columnproperties) {
	var data = $("#grdServicios").jqxGrid('getrowdata', row)
	return '<div style="margin: 2px;text-align: center;"><a class="btn btn-primary btn-xs" onclick="calculo(' + data.ID + ',\'' + data.NroInterno + '\')" href="#"><i class="fa fa-calculator"></i></a></div>'
}
var servicioButton = function (row, columnfield, value, defaulthtml, columnproperties) {
	var data = $("#grdServicios").jqxGrid('getrowdata', row)
	return '<div style="margin: 2px;text-align: center;"><a class="btn btn-info btn-xs" onclick="servicio(' + data.ID + ')" href="#"><i class="fa fa-file-code-o"></i></a></div>'
}
function getSourceGridComprobantes() {
    checkSessionStorage();
	var dtFields = [
		{ name: 'ID', type: 'int' },
		{ name: 'FormatedFecha', type: 'string' },
		{ name: 'TipoComprobante', type: 'string' },
		{ name: 'NroComprobante', type: 'string' },
		{ name: 'ImporteExento', type: 'string' },
		{ name: 'ImporteGravado', type: 'string' },
		{ name: 'PorcentajeIva', type: 'string' },
		{ name: 'ImporteIva', type: 'string' },
		{ name: 'PorcentajeARBA', type: 'string' },
		{ name: 'ImporteARBA', type: 'string' },
		{ name: 'PorcentajeAGIP', type: 'string' },
		{ name: 'ImporteAGIP', type: 'string' },
		{ name: 'Importe', type: 'string' },
	];

	var srcGrid = {
		datatype: "json",
		datafields: dtFields,
		url: '/Facturacion/GetComprobantes',
		data: {}
	};

	var dtGrid = new $.jqx.dataAdapter(srcGrid);

	return dtGrid;
}
var colGrid = [
	{ text: 'Id', datafield: 'ID', hidden: true },
	{ text: 'Fecha', datafield: 'FormatedFecha', width: 100, cellsalign: 'center' },
	{ text: 'Tipo', datafield: 'TipoComprobante', width: 50, cellsalign: 'center' },
	{ text: 'Numero', datafield: 'NroComprobante', width: 110, cellsalign: 'left' },
	{ text: '$ Exento', datafield: 'ImporteExento', width: 120, cellsformat: 'c2', cellsalign: 'right' },
	{ text: '$ Gravado', datafield: 'ImporteGravado', width: 120, cellsformat: 'c2', cellsalign: 'right' },
	{ text: '% Iva', datafield: 'PorcentajeIva', width: 50, cellsformat: 'p2', cellsalign: 'right' },
	{ text: '$ Iva', datafield: 'ImporteIva', width: 80, cellsformat: 'c2', cellsalign: 'right' },
	{ text: '% ARBA', datafield: 'PorcentajeARBA', width: 50, cellsformat: 'p2', cellsalign: 'right' },
	{ text: '$ ARBA', datafield: 'ImporteARBA', width: 120, cellsformat: 'c2', cellsalign: 'right' },
	{ text: '% AGIP', datafield: 'PorcentajeAGIP', width: 50, cellsformat: 'p2', cellsalign: 'right' },
	{ text: '$ AGIP', datafield: 'ImporteAGIP', width: 70, cellsformat: 'c2', cellsalign: 'right' },
	{ text: 'Importe', datafield: 'Importe', width: 120, cellsformat: 'c2', cellsalign: 'right' },
	{ text: 'Descargar', datafield: 'ID2', width: 50, cellsalign: 'center', cellsrenderer: descargarButton },
	{ text: 'Abrir', datafield: 'ID3', width: 50, cellsalign: 'center', cellsrenderer: abrirButton }
];
var colGridServicios = [
	{ text: 'Id', datafield: 'ID', hidden: true },
	{ text: 'Incidente', datafield: 'NroIncidente', width: 40, cellsalign: 'center' },
	{ text: 'Fecha', datafield: 'FormatedFecha', width: 80, cellsalign: 'center' },
	{ text: 'Concepto', datafield: 'ConceptoId', width: 40, cellsalign: 'center' },
	{ text: 'Nro Int.', datafield: 'NroInterno', width: 80, cellsalign: 'left' },
	{ text: 'Iva', datafield: 'Iva', width: 36, cellsalign: 'center' },
	{ text: 'Arba', datafield: 'Arba', width: 35, cellsalign: 'center' },
	{ text: 'Agip', datafield: 'Agip', width: 35, cellsalign: 'center' },
	{ text: 'Nro', datafield: 'NroAfiliado', width: 100, cellsalign: 'left' },
	{ text: 'Paciente', datafield: 'FormatedPaciente', width: 100, cellsalign: 'left' },
	{ text: 'Desde', datafield: 'Desde', width: 100, cellsalign: 'left' },
	{ text: 'Hasta', datafield: 'Hasta', width: 100, cellsalign: 'left' },
	{ text: 'Kmt', datafield: 'Kmt', width: 42, cellsalign: 'right' },
	{ text: 'Espera', datafield: 'TpoEspera', width: 60, cellsalign: 'center' },
	{ text: 'Base', datafield: 'ImporteBase', width: 80, cellsformat: 'c2', cellsalign: 'right' },
	{ text: 'Recargos', datafield: 'Recargos', width: 80, cellsformat: 'c2', cellsalign: 'right' },
	{ text: 'Importe', datafield: 'Importe', width: 80, cellsformat: 'c2', cellsalign: 'right' },
	{ text: 'Calculo', datafield: 'ID4', cellsalign: 'center', cellsrenderer: calculoButton },
	{ text: 'Abrir', datafield: 'ID3', cellsalign: 'center', cellsrenderer: servicioButton }
];
var colGridCalculo = [
	{ text: 'Codigo', datafield: 'Codigo', width: 80, cellsalign: 'left' },
	{ text: 'Concepto', datafield: 'Concepto', cellsalign: 'left' },
	{ text: 'Cantidad', datafield: 'Cantidad', width: 80, cellsalign: 'right' },
	{ text: 'Unitario', datafield: 'Unitario', width: 80, cellsformat: 'c2', cellsalign: 'right' },
	{ text: 'Importe', datafield: 'Importe', width: 80, cellsformat: 'c2', cellsalign: 'right' },
];
function getSourceGridCalculo(comprobante, servicio) {
    checkSessionStorage();
	var dtFields = [
		{ name: 'Codigo', type: 'string' },
		{ name: 'Concepto', type: 'string' },
		{ name: 'Cantidad', type: 'numeric' },
		{ name: 'Unitario', type: 'numeric' },
		{ name: 'Importe', type: 'numeric' },
	];
	var srcGrid = {
		datatype: "json",
		datafields: dtFields,
		url: '/Facturacion/GetRenglones',
		data: { comprobante: comprobante, servicio:servicio }
	};

	var dtGrid = new $.jqx.dataAdapter(srcGrid);

	return dtGrid;
}

function getSourceGridServicios(comprobante) {
    checkSessionStorage();
	var dtFields = [
		{ name: 'ID', type: 'int' },
		{ name: 'NroIncidente', type: 'string' },
		{ name: 'FormatedFecha', type: 'string' },
		{ name: 'ConceptoId', type: 'string' },
		{ name: 'NroInterno', type: 'string' },
		{ name: 'Iva', type: 'string' },
		{ name: 'Arba', type: 'string' },
		{ name: 'Agip', type: 'string' },
		{ name: 'NroAfiliado', type: 'string' },
		{ name: 'FormatedPaciente', type: 'string' },
		{ name: 'Desde', type: 'string' },
		{ name: 'Hasta', type: 'string' },
		{ name: 'Kmt', type: 'string' },
		{ name: 'TpoEspera', type: 'string' },
		{ name: 'ImporteBase', type: 'string' },
		{ name: 'Recargos', type: 'numeric' },
		{ name: 'Importe', type: 'numeric' },
	];
	var srcGrid = {
		datatype: "json",
		datafields: dtFields,
		url: '/Facturacion/GetServicios',
		data: {comprobante: comprobante}
	};

	var dtGrid = new $.jqx.dataAdapter(srcGrid);

	return dtGrid;
}
$(document).ready(function () {
	slider = $('.bxslider').bxSlider({ mode: 'fade' });

	$('[data-toggle="tooltip"]').tooltip();
	$("#grdComprobantes").jqxGrid(
	{
		width: '100%',
		autoheight: true,
		source: getSourceGridComprobantes(),
		pageable: true,
		pagesize: 15,
		altrows: true,
		theme: 'arctic',
		columns: colGrid,
		showfilterrow: true,
		filterable: true,
		localization: { emptydatastring: 'No existen comprobantes electrónicos' },
		pagesizeoptions: ['10']
	});
	$("#grdServicios").on("bindingcomplete", function (event) {
		$('#myModal').modal('hide');
		$('.nav-tabs a[href="#servicios"]').tab('show');
	});

	$("#grdComprobantes").on("celldoubleclick", function (event) {
		var args = event.args;
		var data = $("#grdComprobantes").jqxGrid('getrowdata', args.rowindex)
		var uid = data.ID;
		detalle(uid);
	});
	function reposition() {
		var modal = $(this),dialog = modal.find('.modal-dialog');
		modal.css('display', 'block');
		dialog.css("margin-top", Math.max(0, ($(window).height() - dialog.height()) / 2));
	}
	$('.modal').on('show.bs.modal', reposition);
	$('.nav-tabs > li.fright button').hide();
	$(document).on('shown.bs.tab', 'a[data-toggle="tab"]', function (e) {
		if ($(e.target).text() == "Comprobantes") {
			$('.nav-tabs > li.fright button').hide();
		} else {
			$('.nav-tabs > li.fright button').show();
		}

	})
});
