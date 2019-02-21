//var mensajeid = 0;
//var source =
//            {
//                datatype: "json",
//                datafields: [
//                    { name: 'Nombre' },
//                    { name: 'Id' },
//                ],
//                url: "mensajes/GetUsuarios",
//                data: {}
//            };
//var dataAdapter = new $.jqx.dataAdapter(source,
//    {
//        formatData: function (data) {
//            var item = $("#grupos").jqxComboBox('getSelectedItem');
//            console.log(item);
//            data.GrupoId = item.value;
//            return data;
//        }
//    }
//);

//function getSourceGridMensajesRecibidos() {
//    checkSessionStorage();

//    var dtFields = [{ name: 'Id', type: 'int' }, { name: 'FechaConFormato', type: 'string' }, { name: 'Mensaje', type: 'string' }];

//    var srcGrid = {
//        datatype: "json",
//        datafields: dtFields,
//        url: 'Mensajes/GetRecibidos',
//        data: {}
//    };

//    var dtGrid = new $.jqx.dataAdapter(srcGrid);

//    return dtGrid;
//}
//function getSourceGridMensajesEnviados() {
//    checkSessionStorage();
//    var dtFields = [{ name: 'Id', type: 'int' }, { name: 'FechaConFormato', type: 'string' }, { name: 'Mensaje', type: 'string' }, { name: 'MailText', type: 'string' }];

//    var srcGrid = {
//        datatype: "json",
//        datafields: dtFields,
//        url: 'Mensajes/GetEnviados',
//        data: {}
//    };

//    var dtGrid = new $.jqx.dataAdapter(srcGrid);

//    return dtGrid;
//}
//$(document).ready(function () {
//    checkSessionStorage();
//    $("#btnNuevoMensaje").jqxButton({ width: '100', theme: 'bootstrap', height: '26' });
//    $("#btnBorrarMensaje").jqxButton({ width: '100', theme: 'bootstrap', height: '26' });
//    $('#btnBorrarMensaje').on('click', function () {
//        var selectedrowindex = $('#grdNotificacionesEnviadas').jqxGrid('selectedrowindex');
//        if (selectedrowindex < 0)
//            alert('Debe seleccionar un mensaje');
//        var data = $("#grdNotificacionesEnviadas").jqxGrid('getrowdata', selectedrowindex);
//        var uid = data.Id;
//        $.get('/mensajes/borrar?id=' + uid, function (html) {
//            checkSessionStorage();
//            $("#grdNotificacionesEnviadas").jqxGrid('updatebounddata', 'cells');
//        });
//    });
//    $('#btnNuevoMensaje').on('click', function () {
//        checkSessionStorage();
//        $('#titlePopupMensajeEdicion').html('Nuevo mensaje');
//        mensajeid = 0;
//        padreDescripcion = $('#btnNuevoMensaje').attr("padre");
//        $.get('/mensajes/edicion?id=0' + '&padre=' + padreDescripcion, function (html) {
//            $('#popupEdicion .modal-body').html(html);
//            $('#popupEdicion').modal('show', { backdrop: 'static' });
//        });
//    });
//    var colGrid = [{ text: 'Id', datafield: 'Id', hidden: true },
//                    { text: 'Fecha', datafield: 'FechaConFormato', width: '12%', cellsalign: 'center' },
//                    { text: 'Mensaje', datafield: 'Mensaje', width: '83%', cellsalign: 'left' },
//                    { text: 'Mail', datafield: 'MailText', width: '5%', cellsalign: 'center' }];

//    var colGrid2 = [{ text: 'Id', datafield: 'Id', hidden: true },
//                    { text: 'Fecha', datafield: 'FechaConFormato', width: '12%', cellsalign: 'center' },
//                    { text: 'Mensaje', datafield: 'Mensaje', width: '88%', cellsalign: 'left' }];


//    $("#grdNotificacionesRecibidas").jqxGrid(
//    {
//        width: '99%',
//        autoheight: true,
//        source: getSourceGridMensajesRecibidos(),
//        pageable: true,
//        pagesize: 8,
//        altrows: true,
//        theme: 'arctic',
//        columns: colGrid2,
//        pagesizeoptions: ['8']
//    });

//    $("#grdNotificacionesEnviadas").jqxGrid(
//    {
//        width: '99%',
//        autoheight: true,
//        source: getSourceGridMensajesEnviados(),
//        pageable: true,
//        pagesize: 8,
//        altrows: true,
//        theme: 'arctic',
//        columns: colGrid,
//        pagesizeoptions: ['8']
//    });
//    $("#grdNotificacionesRecibidas").on("celldoubleclick", function (event) {
//        var args = event.args;
//        var data = $("#grdNotificacionesRecibidas").jqxGrid('getrowdata', args.rowindex)
//        var uid = data.Id;
//        $.get('/mensajes/ver?id=' + uid, function (html) {
//            $('#popupVer .modal-body').html(html);
//            $('#popupVer').modal('show', { backdrop: 'static' });
//        });
//    });
//    $("#grdNotificacionesEnviadas").on("celldoubleclick", function (event) {
//        var args = event.args;
//        var data = $("#grdNotificacionesEnviadas").jqxGrid('getrowdata', args.rowindex)
//        mensajeid = data.Id;

//        $('#titlePopupMensajeEdicion').html('Modificar mensaje');
//        padreDescripcion = $('#btnNuevoMensaje').attr("padre");
//        $.get('/mensajes/edicion?id=' + mensajeid + '&padre=' + padreDescripcion, function (html) {
//            checkSessionStorage();
//            $('#popupEdicion .modal-body').html(html);
//            $('#popupEdicion').modal('show', { backdrop: 'static' });
//        });
//    });

//    $('body').on('click', '#btn1', function (event) {
//        var usuarios = $("#usuarios").jqxListBox('getItems');
//        for (var i = 0; i < usuarios.length; i++) {
//            var item = $("#seleccionado").jqxListBox('getItemByValue', usuarios[i].value);
//            if (!item) {
//                $("#seleccionado").jqxListBox('addItem', usuarios[i]);
//            }
//        }
//    });
//    $('body').on('click', '#btn2', function (event) {
//        var usuarios = $("#usuarios").jqxListBox('getSelectedItems');
//        for (var i = 0; i < usuarios.length; i++) {
//            var item = $("#seleccionado").jqxListBox('getItemByValue', usuarios[i].value);
//            if (!item) {
//                $("#seleccionado").jqxListBox('addItem', usuarios[i]);
//            }
//        }
//    });
//    $('body').on('click', '#btn3', function (event) {
//        var usuarios = $("#seleccionado").jqxListBox('getSelectedItems');
//        for (var i = 0; i < usuarios.length; i++) {
//            var attrListBoxTable = usuarios[i].element.getElementsByTagName("table")[0].attributes;
//            if (parseInt(attrListBoxTable.attrappopen["value"]) == 0 &&
//                parseInt(attrListBoxTable.attrmailopen["value"]) == 0) {
//                $("#seleccionado").jqxListBox('removeItem', usuarios[i]);
//            }
//        }
//    });
//    $('body').on('click', '#btn4', function (event) {
//        eliminarTodos();
//        eliminarTodos();
//    });

//    var eliminarTodos = function () {
//        var usuarios = $("#seleccionado").jqxListBox('getItems');
//        for (var i = 0; i < usuarios.length; i++) {
//            var attrListBoxTable = usuarios[i].element.getElementsByTagName("table")[0].attributes;
//            if (parseInt(attrListBoxTable.attrappopen["value"]) == 0 &&
//                parseInt(attrListBoxTable.attrmailopen["value"]) == 0) {
//                $("#seleccionado").jqxListBox('removeItem', usuarios[i]);
//            }
//        }
//    }
//    $('body').on('select', '#grupos', function (event) {
//        var args = event.args;
//        if (args != undefined) {
//            var item = event.args.item;
//            if (item != null) {
//                dataAdapter.dataBind();
//                $("#usuarios").jqxListBox({ width: '100%', height: 150, source: dataAdapter, displayMember: "Nombre", valueMember: "Id" });
//            }
//        }
//    });
//    $('body').on('click', '#guardarSinUsuarios', function () {
//        guardar();
//    });

//    $('body').on('click', '#popupMensajeEdicionok', function () {
//        var usuarios = $("#seleccionado").jqxListBox('getItems');
//        if (usuarios) {
//            guardar();
//        } else {
//            $("#popupConfirmacion").modal('show');
//        }
//    });

//});

//function guardar() {

//    //var sessionUsrId = parseInt($("#body_layout").data('value'));
//    //if (sessionUsrId == 0)
//    //{
//    //    $('#labelErrores').html("Sesion invalida");
//    //    $('#popupErrores').modal('show', { backdrop: 'static' });
//    //    return;
//    //}

//    $('#popupProcesando').modal('show', { backdrop: 'static' });

//    var mensaje = {};
//    var ok = true;
//    var mensajeEnriquecido = $('.textarea-editor').summernote('code');
//    if (mensajeEnriquecido.substr(0, 3) != "<p>")
//        mensajeEnriquecido = "<p>" + mensajeEnriquecido + "</p>";
//    var mensajeEnriquecidoCodificado = window.btoa(mensajeEnriquecido);

//    mensaje.Id = mensajeid;
//    mensaje.Mensaje = $(mensajeEnriquecido).text();
//    var usuarios = $("#seleccionado").jqxListBox('getItems');
//    mensaje.Usuarios = [];
//    mensaje.Mail = 0;
//    console.log($("#mail"));
//    if ($("#mail").prop('checked')) {
//        mensaje.Mail = 1;
//    }

//    if (usuarios) {
//        for (var i = 0; i < usuarios.length; i++) {
//            mensaje.Usuarios.push(usuarios[i].value);
//        }
//    }
//    var datos = JSON.stringify(mensaje);
//    $.post('/mensajes/guardar', { valor: datos, mensajeEnriquecido: mensajeEnriquecidoCodificado }, function (data, status, xhr) {
//        checkSessionStorage();
//        if (data == "OK") {
//            $('#popupProcesando').modal('hide');
//            $('#popupEdicion').modal('hide');
//            $("#grdNotificacionesEnviadas").jqxGrid('updatebounddata', 'cells');
//        } else {
//            $('#popupProcesando').modal('hide');
//            $('#labelErrores').html(data);
//            $('#popupErrores').modal('show', { backdrop: 'static' });
//        }
//    });
//}