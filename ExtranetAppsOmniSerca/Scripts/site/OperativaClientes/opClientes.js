// --> Inicializo opciones generales para el alerta que uso en vez del alert de javascript comun
Messenger().options = {
    extraClasses: 'messenger-fixed messenger-on-bottom messenger-on-right',
    theme: 'flat',
};

// --> Seteo alerta mas linda que el clasico alert. Le paso el mensaje y el tipo de alerta.
function setAlert(msg, tipoMsg) {
    Messenger().post({
        message: msg,
        type: tipoMsg,
        showCloseButton: true
    });
}

// --> Me fijo que tabs tengo habilitadas, dependiendo del usuario
// --> Si no está habilitado, escondo la tab
if (habCurso == 0) {
    $('#liEnCurso').remove();
    $('#enCurso').css("display", "none");
}

if (habFinaliz == 0) {
    $('#liFinalizados').remove();
    $('#finalizados').css("display", "none");
}

if (habErroneos == 0) {
    $('#liErroneos').remove();
    $('#validErroneos').css("display", "none");
}

if (habDenuncias == 0) {
    $('#liDenuncias').remove();
    $('#denuncias').css("display", "none");
}

// --> Me fijo por prioridad, si está en curso, esa está en active.. sino la siguiente.. y así

if ((habCurso > 0) || (habFinaliz > 0) || (habErroneos > 0)) {

    if ($('#liEnCurso').length > 0) {

        setActiveIn($('#enCurso'));
        setActiveIn($('#liEnCurso'));

    } else if ($('#liFinalizados').length > 0) {

        setActiveIn($('#finalizados'));
        setActiveIn($('#liFinalizados'));

    } else if ($('#liErroneos').length > 0) {

        setActiveIn($('#validErroneas'));
        setActiveIn($('#liErroneos'));

    } else {

        setActiveIn($('#denuncias'));
        setActiveIn($('#liDenuncias'));

    }

    // --> le paso la tab y la seteo active - in
}



function setActiveIn(obj) {

    obj.addClass("active");
    obj.addClass("in");

}

$('#displayTabs').css("display", "block");






