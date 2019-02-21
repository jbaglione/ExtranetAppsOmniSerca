app.controller('loginCtrl', function ($scope, $http, $window, Base64, $rootScope) {
    $scope.data = {};
    $scope.error = '';
    $scope.currentDateStr = (new Date()).toLocaleDateString('es-AR', { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' });
    $scope.currentYear = (new Date()).getFullYear();
    $scope.labelCliente = 'Cliente';
    $scope.homeWelcome = "Bienvenidos a Extranet";
    $rootScope.mostrarError = function (title, errorMessage) {
        $.Notify({
            caption: title,
            content: errorMessage,
            type: 'alert',
            icon: "<span class='mif-cross'></span>"
        })
    };
    $rootScope.mostrarSuccess = function (title, message) {
        $.Notify({
            caption: title,
            content: message,
            type: 'success',
            icon: "<span class='mif-checkmark'></span>"
        })
    };
    $scope.logout = function () {
        debugger;
        delete sessionStorage.currentUser;
        $scope.data.enableSites = false;
    };

    //$scope.getSession = function () {
    //    $http({
    //        method: 'POST',
    //        url: '/login/GetSession'
    //    }).then(function successCallback(response) {
    //        if (parseInt(response.data) <= 0)
    //            $scope.logout();
    //    }, function errorCallback(response) {
    //        $scope.logout();
    //    });
    //}

    $scope.getSessionStorageIsValid = function () {
        if (sessionStorage.currentUser != null && sessionStorage.currentUser != undefined && sessionStorage.currentUser != 'undefined') {
                $scope.data.enableSites = true;
                $http({
                    method: 'POST',
                    url: '/login/GetSession'
                }).then(function successCallback(response) {
                    if (parseInt(response.data) > 0) {
                        $rootScope.currentUser = JSON.parse(sessionStorage.currentUser);
                        if ($rootScope.currentUser.sessionData.UsuarioExtranetId != null && $rootScope.currentUser.sessionData.UsuarioExtranetId != undefined &&
                        $rootScope.currentUser.sessionData.UsuarioExtranetId > 0) {
                            $scope.data.acciones = $rootScope.currentUser.acciones;
                            $scope.data.enableSites = true;
                        }
                    }
                    else{
                        $scope.logout();
                    }
                }, function errorCallback(response) {
                    $scope.logout();
                });
        } else $scope.data.enableSites = false;
    };

    $scope.getSessionStorageIsValid();

    //$scope.data.enableSites = $scope.getSessionStorageIsValid();

    $scope.login = function () {
        $http({
            method: 'POST',
            url: '/login/Login?pUsr=' + $scope.username + '&pPass=' + $scope.password
        }).then(function successCallback(response) {
            //$rootScope.dataLoading = false;
            $scope.data.acciones = response.data.LoginUsuarios;
            if (Object.keys($scope.data.acciones).length > 0) {
                $scope.data.acciones.forEach(function (accion) {
                    accion.Icono = $scope.GetIcon(accion.Jerarquia);
                });
                $scope.SetearCredenciales($scope.username, $scope.password, $scope.data.acciones, response.data.MySessionData);

                $scope.error = ''
                $scope.data.enableSites = true;
                $scope.ObtenerAlertas();
            }
            else {
                $scope.error = '¡Datos de acceso inválidos!';
                $scope.data.enableSites = false;
            }
            //CreateLink(response);
        }, function errorCallback(response) {
            $scope.error = '¡Datos de acceso inválidos!';
            //alert($scope.error);
            //$rootScope.mostrarError('¡Datos de acceso inválidos!', response.message);
            console.log(response);
        });
    }
    var authdata = "";
    $scope.SetearCredenciales = function (username, password, acciones, sessData) {
        
        authdata = Base64.encode(sessData.Nombre + ':' + password);
        $rootScope.currentUser = { username: username, userfullname: sessData.Nombre, authdata: authdata, acciones: acciones || [], sessionData: sessData || [] };//
        $http.defaults.headers.common['Authorization'] = 'Basic' + authdata;
        sessionStorage.setItem("currentUser", JSON.stringify($rootScope.currentUser));
        $scope.currentemail = $rootScope.currentUser.sessionData.Email;
    };

    $scope.ObtenerAlertas = function () {
        $http({
            method: 'POST',//GetAlertas(long pUsrExtId)
            url: '/login/GetAlertas'
        }).then(function successCallback(response) {
            
            //if (response.status && response.status === "OK") {
                for (var i in response.data) {
                    $.Notify({
                        caption: "Alerta",
                        content: response.data[i].DescripcionAlerta,
                        type: 'info',
                        icon: "<span class='mif-info'></span>",
                        keepOpen: true
                    })
                }
            //}
        }, function errorCallback(response) {
            $.Notify({
                caption: "Alerta",
                content: "Error al cargar las alertas.",
                type: 'Error',
                icon: "<span class='mif-error'></span>",
                keepOpen: true
            })
            console.log(response);
        });
    }

    $scope.GetIcon = function (jerarquia) {
        var iconos = ['arrow-right', 'books', 'profile', 'apps'];
        var idx = 0;
        if (jerarquia < 200) {
            idx = 1;
        }
        if (jerarquia >= 200 && jerarquia < 300) {
            idx = 2;
        }
        if (jerarquia >= 300) {
            idx = 3;
        }
        return iconos[idx];
    }

    $scope.redireccionarAccion = function (accion) {        
        $window.location.href = accion.URL;
    }

    $scope.forgot = function () {
        $scope.errorForgot = '';
        $scope.okForgot = '';
        $('#popupForgot').modal('show');
    }

    $scope.forgotPassword = function (email) {
        $scope.errorForgot = '';
        $scope.okForgot = '';
        $http({
            method: 'POST',
            url: '/login/ForgotPassword?pIde=' + email
        }).then(function successCallback(response) {
            $scope.okForgot = 'Hemos enviado la nueva clave a su E-mail.';
        }, function errorCallback(response) {
            $scope.errorForgot = 'Email no registrado.';
        });
    }

    //<--ChangePassword
    $scope.currentpass = "";
    $scope.newpass = "";
    $scope.confirmnewpass = "";
    $scope.showChangePassword = function () {
        $('#popupChangePassword').modal('show');
    }
    $scope.validarSeguridadPassword = function () {
        var segura = jQuery(".pass-meter .pass-meter-col").hasClass("good") || jQuery(".pass-meter .pass-meter-col").hasClass("strong");
        var distinta = ($scope.currentpass !== $scope.newpass);
        return (segura && distinta);
    };

    $scope.changePassword = function () {
        var authdataToVerify = Base64.encode($rootScope.currentUser.userfullname + ':' + $scope.currentpass);
        if ($rootScope.currentUser.authdata == authdataToVerify) {
            if ($scope.validarSeguridadPassword()) {
                if ($scope.confirmnewpass == $scope.newpass) {
                    $http({
                        method: 'POST',
                        url: '/login/ChangePassword?pUsrExtId=' + $rootScope.currentUser.sessionData.UsuarioExtranetId + '&pOld=' + $scope.currentpass + '&pNew=' + $scope.newpass
                    }).then(function successCallback(response) {
                        if (response.data.Resultado === "1") {
                            $scope.mostrarSuccess('¡Contraseña cambiada!', 'Contraseña cambiada correctamente!');
                            
                            $scope.logout();
                            setTimeout(function () {
                                $window.location.reload();
                            }, 3000);
                        } else {
                            $rootScope.mostrarError('¡No se pudo cambiar la contraseña!', response.data.DescripcionError);
                        }
                    });
                } else {
                    $rootScope.mostrarError('¡Error en Contraseña!', 'Las contraseñas no coinciden (Confirmar Nueva Contraseña)');
                }
            } else {
                $rootScope.mostrarError('¡Error en Contraseña!', 'Por favor ingrese una contraseña distinta a la anterior o más segura (nivel mínimo requerido: Buena)');
            }
        } else {
            $rootScope.mostrarError('¡Error de Validación!', 'La contraseña ingresada (Contraseña Actual) es incorrecta');
        }
        //$location.path('/home');
    }
    //ChangePassword-->

    //<--ChangeSettings
    $scope.showChangeSettings = function () {
        $('#popupChangeSettings').modal('show');
    }

    $scope.newemail = "";
    $scope.changeSettings = function () {
         
        if ($scope.newemail !== "") {
            $http({
                method: 'POST',
                url: '/login/ChangeSettings?pUsrExtId=' + $rootScope.currentUser.sessionData.UsuarioExtranetId + '&pEmail=' + $scope.newemail
            }).then(function successCallback(response) {
                if (response.data.Resultado === "1") {
                    $scope.mostrarSuccess('¡Preferencias cambiadas!', 'Las preferencias fueron cambiada correctamente!');
                    $rootScope.currentUser.sessionData.Email = $scope.newemail;
                    $scope.currentemail = $scope.newemail;
                    $('#popupChangeSettings').modal('hide');
                } else {
                    $rootScope.mostrarError('¡No se pudieron cambiar las preferencias!', response.message);
                }
            });
        }
    }
    //ChangeSettings-->

    $scope.register = function () {

        $scope.errorRegister = '';
        $scope.okRegister = '';
        $('#popupRegister').modal('show');
    } 

    $scope.RelacionChange = function () {

        switch ($scope.relacion) {
            case 'Cliente':
                $scope.labelCliente = 'Código Cliente';
                break;
            case 'EmpresaPrestadora':
                $scope.labelCliente = 'Código Empresa Serca';
                break;
            case 'PrestadorDeMovil':
                $scope.labelCliente = 'Moviles';
                break;
            case 'Personal':
                $scope.labelCliente = 'Nro Legajo';
                break;
            case 'Otros':
                $scope.labelCliente = 'Referencias';
                break;
            default:
                $scope.labelCliente = 'Cliente';
                break;
        }
    };

    $scope.registerUser = function () {
        $scope.errorRegister = '';
        $scope.okRegister = '';

        var nombre = $scope.nombre;
        var relacion = $scope.relacion;
        var emailCliente = $scope.emailCliente;
        var labelCliente = $scope.labelCliente;
        labelCliente = '';
        var cliente = $scope.cliente;

        $http({
            method: 'POST',
            url: '/login/SendMailRegisterUser?nombre=' + nombre + '&relacion=' + $scope.relacion + '&emailCliente=' + $scope.emailCliente + '&labelCliente=' + $scope.labelCliente + '&cliente=' + $scope.cliente
        }).then(function successCallback(response) {
            $scope.okRegister = '¡Solicitud enviada! Pronto lo estaremos contactando.';    
        }, function errorCallback(response) {
            $scope.errorRegister = '¡Error procesando su solicitud!';
            ////$rootScope.mostrarError('¡Datos de acceso inválidos!', response.message);
            console.log(response);
        });
    }
});