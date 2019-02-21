using System.Web;
using System.Web.Optimization;

namespace ExtranetAppsOmniSerca
{
    public class BundleConfig
    {
        // Para obtener más información acerca de Bundling, consulte http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",//));
                        "~/Scripts/jquery-ui-{version}.js"));
            /*
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));
            */

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                //"//code.jquery.com/ui/1.12.1/themes/smoothness/jquery-ui.css",
                "~/Scripts/jquery-1.8.2.min.js",
                "~/Scripts/jquery-ui-1.8.24.js",
                "~/Scripts/angular-ui/autocomplete.js"
                ));


            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/vendor/bootstrap.js",
                        "~/Scripts/bootstrap-select.js",
                        "~/Scripts/bootstrap-datepicker3.min",
                        "~/Scripts/jquery.maskedinput.js",
                        "~/Scripts/bootstrapValidator.js",
                        "~/Scripts/messenger.js",
                        "~/Scripts/moment.js",
                        "~/Scripts/messenger-theme-future.js",
                        "~/Scripts/jquery.serialize-object.js",
                        "~/Scripts/jquery.maskMoney.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/main").Include(
                        "~/Scripts/site/main.js",
                        "~/Scripts/site/guardias.js",
                        "~/Scripts/site/servicios.js",
                        "~/Scripts/site/potenciales.js",
                        "~/Scripts/site/resumen.js"
                        //"~/Scripts/site/Mensajes/Mensajes.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/opClientes").Include(
                        "~/Scripts/site/main.js",
                        "~/Scripts/site/OperativaClientes/opClientes.js",
                        "~/Scripts/site/OperativaClientes/svEnCurso.js",
                        "~/Scripts/site/OperativaClientes/svFinalizados.js",
                        "~/Scripts/site/OperativaClientes/svDenuncias.js",
                        "~/Scripts/site/OperativaClientes/svErroneos.js"));

            bundles.Add(new ScriptBundle("~/bundles/cuentaCorriente").Include(
                        "~/Scripts/site/main.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/mensajes").Include(
                "~/Scripts/summernote.js",
                "~/Scripts/summernote-ext-rtl.js",
                "~/Scripts/site/Mensajes/Mensajes.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/liquidaciones").Include(
                        "~/Scripts/site/main.js",
                        "~/Scripts/site/liquidacion/liquidaciones2.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                        "~/Scripts/angular.min.js",
                        "~/Scripts/ngMask.js",
                        "~/Scripts/angular-ui/ui-bootstrap-tpls.min.js",
                        "~/Scripts/angular-ui-router.min.js",
                        "~/Scripts/angular-ng-pattern-restrict.js",
                        "~/Scripts/site/angular.module.js",
                        "~/Scripts/signature_pad/signature_pad.min.js",
                        //--------------------------------------------
                        "~/Scripts/site/liquidacion/liquidaciones.js",
                        "~/Scripts/site/EmpleadosLiquidaciones/empleadosLiquidaciones.js",
                        "~/Scripts/site/EmpleadosLiquidaciones/empleadosLiquidaciones2.js",
                        "~/Scripts/site/afiliacion/afiliaciones.js",
                        "~/Scripts/site/afiliacionesPopUp/afiliacionesPopUp.js",
                        "~/Scripts/site/CuentaCorriente/app.js",
                        "~/Scripts/site/CuentaCorriente/lista.js",
                        "~/Scripts/site/CuentaCorriente/listaHijos.js",
                        "~/Scripts/site/CuentaCorriente/nueva.js",
                        "~/Scripts/site/Login/login.js",
                        "~/Scripts/widgets/notify.js",
                        "~/Content/login/ng-password-meter.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqwidgets").Include(
                        "~/Content/jqwidgets/jqxcore.js",
                        "~/Content/jqwidgets/jqxdata.js",
                        "~/Content/jqwidgets/jqxbuttons.js",
                        "~/Content/jqwidgets/jqxscrollbar.js",
                        "~/Content/jqwidgets/jqxmenu.js",
                        "~/Content/jqwidgets/jqxgrid.js",
                        "~/Content/jqwidgets/jqxgrid.pager.js",
                        "~/Content/jqwidgets/jqxgrid.selection.js",
                        "~/Content/jqwidgets/jqxinput.js",
                        "~/Content/jqwidgets/jqxtreegrid.js",
                        "~/Content/jqwidgets/jqxnumberinput.js",
                        "~/Content/jqwidgets/jqxwindow.js",
                        "~/Content/jqwidgets/jqxlistbox.js",
                        "~/Content/jqwidgets/jqxdropdownlist.js",
                        "~/Content/jqwidgets/jqxcheckbox.js",
                        "~/Content/jqwidgets/jqxcombobox.js",
                        "~/Content/jqwidgets/jqxgrid.filter.js",
                        "~/Content/jqwidgets/jqxdatetimeinput.js",
                        "~/Content/jqwidgets/jqxcalendar.js",
                        "~/Content/jqwidgets/jqxgrid.aggregates.js",
                        "~/Content/jqwidgets/jqxtooltip.js",
                        "~/Content/jqwidgets/jqxdata.export.js",
                        "~/Content/jqwidgets/jqxgrid.export.js",
                        "~/Content/jqwidgets/jqxvalidator.js",
                        "~/Content/jqwidgets/globalization/globalize.js",
                        "~/Content/jqwidgets/globalization/globalize.culture.es-AR.js",
                        "~/Content/jqwidgets/jqxgrid.columnsresize.js",
                        "~/Content/jqwidgets/jqxfileupload.js",
                        "~/Content/jqwidgets/jqxangular.js",
                        "~/Content/jqwidgets/jqxdatatable.j",
                        "~/Content/jqwidgets/demos.js",
                        "~/Content/jqwidgets/jqxgrid.edit.js",
                        "~/Content/jqwidgets/jqxfileupload.js",
                        "~/Content/jqwidgets/jqxangular.js"
                        ));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información sobre los formularios. De este modo, estará
            // preparado para la producción y podrá utilizar la herramienta de creación disponible en http://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/styles/droid-font.css",
                        //"~/Content/styles/bootstrap.css",
                        //"~/Content/styles/bootstrap-theme.css",
                        "~/Content/styles/bootstrap-select.css",
                        "~/Content/styles/bootstrapValidator.css",
                        "~/Content/styles/messenger.css",
                        "~/Content/styles/messenger-theme-future.css",
                        "~/Content/styles/messenger-spinner.css",
                        "~/Content/styles/shapes.css",
                        "~/Content/jqwidgets/styles/jqx.base.css",
                        "~/Content/jqwidgets/styles/jqx.arctic.css",
                        "~/Content/jqwidgets/styles/jqx.bootstrap.css",
                        "~/Content/styles/main.css",
                        "~/Content/login/metro-custom.css",
                        "~/Content/login/metro-icons.css",
                        "~/Content/login/ng-password-meter.css",
                        "~/Content/login/login.css"
                        ));

            bundles.Add(new StyleBundle("~/Content/Map/css").Include("~/Content/styles/mapa.css"));

            //bundles.Add(new StyleBundle("~/Content/jquery-ui").Include(
            //    "~/Content/jquery-ui/jquery-ui.min.css",
            //    "~/Content/jquery-ui/jquery-ui.theme.css"
            //    ));
        }
    }
}