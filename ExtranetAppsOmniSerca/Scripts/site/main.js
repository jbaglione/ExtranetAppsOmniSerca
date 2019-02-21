// --> Seteo tooltip de salir en boton salir, arriba a la derecha en el header
$('#navbar-top-logout').tooltip({
    title: 'Salir',
    placement: 'right'
});

// --> Seteo localization obj para traducir grillas de ingles

var localizationobj = {};
localizationobj.pagergotopagestring = "Ir a p&aacute;gina:";
localizationobj.pagershowrowsstring = "Mostrar Filas:";
localizationobj.pagerrangestring = " de ";
localizationobj.currencysymbol = "$";

function JSON2CSV(objArray) {
    var array = typeof objArray != 'object' ? JSON.parse(objArray) : objArray;

    var str = '';
    var line = '';


    str = "sep=,";
    str += '\r\n';


    var head = array[0];

    for (var index in array[0]) {
        var value = index + "";
        line += '"' + value.replace(/"/g, '""') + '",';
    }


    line = line.slice(0, -1);
    str += line + '\r\n';


    for (var i = 0; i < array.length; i++) {
        var line = '';


        for (var index in array[i]) {
            var value = array[i][index] + "";
            line += '"' + value.replace(/"/g, '""') + '",';
        }


        line = line.slice(0, -1);
        str += line + '\r\n';
    }
    return str;

}

function downloadExcel(jsonData, fileName) {
    var json = $.parseJSON(jsonData);
    var csv = JSON2CSV(json);

    var downloadLink = document.createElement("a");
    var blob = new Blob(["\ufeff", csv]);
    var url = URL.createObjectURL(blob);
    downloadLink.href = url;
    downloadLink.download = fileName + ".xls";

    document.body.appendChild(downloadLink);
    downloadLink.click();
    document.body.removeChild(downloadLink);

}

















