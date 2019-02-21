app = angular.module('app', [
    'ui.router',
    'ui.bootstrap',
    'jqwidgets',
    'ngPatternRestrict',
    'ngMask',
    'ui.autocomplete',
    'signature',
    'ngPasswordMeter'
]);
app.constant('appConfig', { 'apiUrl': '/api/', })
app.run(['$rootScope', function ($rootScope) { }]);

app.directive('jqdatepicker', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {
            $(element).datepicker({
                dateFormat: 'yy-mm-dd',

                onSelect: function (date) {
                    var ngModelName = this.attributes['ng-model'].value;

                    // if value for the specified ngModel is a property of 
                    // another object on the scope
                    if (ngModelName.indexOf(".") != -1) {
                        var objAttributes = ngModelName.split(".");
                        var lastAttribute = objAttributes.pop();
                        var partialObjString = objAttributes.join(".");
                        var partialObj = eval("scope." + partialObjString);

                        partialObj[lastAttribute] = date;
                    }
                        // if value for the specified ngModel is directly on the scope
                    else {
                        scope[ngModelName] = date;
                    }
                    scope.$apply();
                }

            });
        }
    };
});
app.directive('numbersOnly', numbersOnly);

numbersOnly.$inject = [];
 function numbersOnly() {
 
    var directive = {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, element, attr, ngModelCtrl) {
            function fromUser(text) {
                if (text) {
                    var transformedInput = text.replace(/[^0-9\.]/g, '');
                    if (transformedInput !== text) {
                        ngModelCtrl.$setViewValue(transformedInput);
                        ngModelCtrl.$render();
                    }
                    return transformedInput;
                }
                return undefined;
            }
            ngModelCtrl.$parsers.push(fromUser);
        }
    };
    return directive;
 };

 app.directive('selectOnClick', ['$window', function ($window) {
     return {
         restrict: 'A',
         link: function (scope, element, attrs) {
             element.on('click', function () {
                 if (!$window.getSelection().toString()) {
                     // Required for mobile Safari
                     this.setSelectionRange(0, this.value.length)
                 }
             });
         }
     };
 }]);

 app.directive('noDirty', function () {
     return {
         require: 'ngModel',
         link: function (scope, element, attrs, ngModelCtrl) {
             // override the $setDirty method on ngModelController
             ngModelCtrl.$setDirty = angular.noop;

         }
     }
 });

 app.directive('price', [function () {
     return {
         require: 'ngModel',
         link: function (scope, element, attrs, ngModel) {
             attrs.$set('ngTrim', "false");

             var formatter = function (str, isNum) {
                 str = String(Number(str || 0) / (isNum ? 1 : 100));
                 str = (str == '0' ? '0.0' : str).split('.');
                 str[1] = str[1] || '0';
                 return str[0].replace(/(\d)(?=(\d\d\d)+(?!\d))/g, '$1,') + '.' + (str[1].length == 1 ? str[1] + '0' : str[1]);
             }
             var updateView = function (val) {
                 scope.$applyAsync(function () {
                     ngModel.$setViewValue(val || '');
                     ngModel.$render();
                 });
             }
             var parseNumber = function (val) {
                 var modelString = formatter(ngModel.$modelValue, true);
                 var sign = {
                     pos: /[+]/.test(val),
                     neg: /[-]/.test(val)
                 }
                 sign.has = sign.pos || sign.neg;
                 sign.both = sign.pos && sign.neg;

                 if (!val || sign.has && val.length == 1 || ngModel.$modelValue && Number(val) === 0) {
                     var newVal = (!val || ngModel.$modelValue && Number() === 0 ? '' : val);
                     if (ngModel.$modelValue !== newVal)
                         updateView(newVal);

                     return '';
                 }
                 else {
                     var valString = String(val || '');
                     var newSign = (sign.both && ngModel.$modelValue >= 0 || !sign.both && sign.neg ? '-' : '');
                     var newVal = valString.replace(/[^0-9]/g, '');
                     var viewVal = newSign + formatter(angular.copy(newVal));

                     if (modelString !== valString)
                         updateView(viewVal);

                     return (Number(newSign + newVal) / 100) || 0;
                 }
             }
             var formatNumber = function (val) {
                 if (val) {
                     var str = String(val).split('.');
                     str[1] = str[1] || '0';
                     val = str[0] + '.' + (str[1].length == 1 ? str[1] + '0' : str[1]);
                 }
                 return parseNumber(val);
             }

             ngModel.$parsers.push(parseNumber);
             ngModel.$formatters.push(formatNumber);
         }
     };
 }]);

 app.directive('signaturePad', ['$interval', '$timeout', '$window',
   function ($interval, $timeout, $window) {
       'use strict';

       var signaturePad, element, EMPTY_IMAGE = 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAjgAAADcCAQAAADXNhPAAAACIklEQVR42u3UIQEAAAzDsM+/6UsYG0okFDQHMBIJAMMBDAfAcADDATAcwHAAwwEwHMBwAAwHMBzAcAAMBzAcAMMBDAcwHADDAQwHwHAAwwEMB8BwAMMBMBzAcADDATAcwHAADAcwHADDAQwHMBwAwwEMB8BwAMMBDAfAcADDATAcwHAAwwEwHMBwAAwHMBzAcAAMBzAcAMMBDAcwHADDAQwHwHAAwwEwHMBwAMMBMBzAcAAMBzAcwHAADAcwHADDAQwHMBwAwwEMB8BwAMMBDAfAcADDATAcwHAAwwEwHMBwAAwHMBzAcCQADAcwHADDAQwHwHAAwwEMB8BwAMMBMBzAcADDATAcwHAADAcwHMBwAAwHMBwAwwEMBzAcAMMBDAfAcADDAQwHwHAAwwEwHMBwAAwHMBzAcAAMBzAcAMMBDAcwHADDAQwHwHAAwwEMB8BwAMMBMBzAcADDATAcwHAADAcwHMBwAAwHMBwAwwEMB8BwAMMBDAfAcADDATAcwHAAwwEwHMBwAAwHMBzAcAAMBzAcAMMBDAcwHADDAQwHwHAAwwEMB8BwAMMBMBzAcADDkQAwHMBwAAwHMBwAwwEMBzAcAMMBDAfAcADDAQwHwHAAwwEwHMBwAMMBMBzAcAAMBzAcwHAADAcwHADDAQwHMBwAwwEMB8BwAMMBMBzAcADDATAcwHAADAcwHMBwAAwHMBwAwwEMBzAcAMMBDAegeayZAN3dLgwnAAAAAElFTkSuQmCC';

       return {
           restrict: 'EA',
           replace: true,
           template: '<div class="signature" style="width: 100%; max-width:{{width}}px; height: 100%; max-height:{{height}}px;"><canvas id="SignatureCanvas" style="display: block; margin: 0 auto;" ng-mouseup="onMouseup()" ng-mousedown="notifyDrawing({ drawing: true })"></canvas></div>',
           scope: {
               accept: '=?',
               clear: '=?',
               clean: '=?',
               disabled: '=?',
               dataurl: '=?',
               height: '@',
               width: '@',
               notifyDrawing: '&onDrawing',
           },
           controller: [
             '$scope',
             function ($scope) {
                 $scope.accept = function (dataurlnew) {
                     
                     if (dataurlnew != null) {
                        $scope.dataurl = dataurlnew;
                        $scope.setDataUrl(dataurlnew);
                        $scope.updateModel();
                    }
                     $timeout(5000);
                     return {
                             isEmpty: $scope.dataurl === EMPTY_IMAGE,
                             dataUrl: $scope.dataurl                        
                     };
                 };

                 $scope.onMouseup = function () {
                     
                     $scope.updateModel();
                     
                     // notify that drawing has ended
                     $scope.notifyDrawing({ drawing: false });
                 };

                 $scope.updateModel = function () {
                     /*
                      defer handling mouseup event until $scope.signaturePad handles
                      first the same event
                      */
                     $timeout().then(function () {
                         $scope.dataurl = $scope.signaturePad.isEmpty() ? EMPTY_IMAGE : $scope.signaturePad.toDataURL();
                     });
                 };

                 $scope.clear = function () {
                     
                     $scope.signaturePad.clear();
                     $scope.dataurl = EMPTY_IMAGE;
                 };

                 $scope.clean = function () {
                     
                     $scope.signaturePad.clear();
                     $scope.dataurl = EMPTY_IMAGE;
                 };

                 $scope.$watch("dataurl", function (dataUrl) {
                     if (!dataUrl || $scope.signaturePad.toDataURL() === dataUrl) {
                         return;
                     }

                     $scope.setDataUrl(dataUrl);
                 });
             }
           ],
           link: function (scope, element, attrs) {
               var canvas = element.find('canvas')[0];
               var parent = canvas.parentElement;
               var scale = 0;
               var ctx = canvas.getContext('2d');

               var width = parseInt(scope.width, 10);
               var height = parseInt(scope.height, 10);

               canvas.width = width;
               canvas.height = height;

               scope.signaturePad = new SignaturePad(canvas);

               scope.setDataUrl = function(dataUrl) {
                   var ratio = Math.max(window.devicePixelRatio || 1, 1);

                   ctx.setTransform(1, 0, 0, 1, 0, 0);
                   ctx.scale(ratio, ratio);

                   scope.signaturePad.clear();
                   scope.signaturePad.fromDataURL(dataUrl);

                   $timeout().then(function() {
                       ctx.setTransform(1, 0, 0, 1, 0, 0);
                       ctx.scale(1 / scale, 1 / scale);
                   });
               };

               scope.$watch('disabled', function (val) {
                   val ? scope.signaturePad.off() : scope.signaturePad.on();
               });
        
               var calculateScale = function() {
                   var scaleWidth = Math.min(parent.clientWidth / width, 1);
                   var scaleHeight = Math.min(parent.clientHeight / height, 1);

                   var newScale = Math.min(scaleWidth, scaleHeight);

                   if (newScale === scale) {
                       return;
                   }

                   var newWidth = width * newScale;
                   var newHeight = height * newScale;
                   canvas.style.height = Math.round(newHeight) + "px";
                   canvas.style.width = Math.round(newWidth) + "px";

                   scale = newScale;
                   ctx.setTransform(1, 0, 0, 1, 0, 0);
                   ctx.scale(1 / scale, 1 / scale);
               };

               var resizeIH = $interval(calculateScale, 200);
               scope.$on('$destroy', function () {
                   $interval.cancel(resizeIH);
                   resizeIH = null;
               });

               angular.element($window).bind('resize', calculateScale);
               scope.$on('$destroy', function () {
                   angular.element($window).unbind('resize', calculateScale);
               });

               calculateScale();

               element.on('touchstart', onTouchstart);
               element.on('touchend', onTouchend);

               function onTouchstart(event) {
                   scope.$apply(function () {
                       // notify that drawing has started
                       scope.notifyDrawing({ drawing: true });
                   });
                   event.preventDefault();
               }

               function onTouchend(event) {
                   scope.$apply(function () {
                       // updateModel
                       scope.updateModel();

                       // notify that drawing has ended
                       scope.notifyDrawing({ drawing: false });
                   });
                   event.preventDefault();
               }
           }
       };
   }
 ]);
//angular.module('app.services', [])
 app.factory('Base64', function () {
     var keyStr = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=';

     return {
         encode: function (input) {
             var output = "";
             var chr1, chr2, chr3 = "";
             var enc1, enc2, enc3, enc4 = "";
             var i = 0;

             do {
                 chr1 = input.charCodeAt(i++);
                 chr2 = input.charCodeAt(i++);
                 chr3 = input.charCodeAt(i++);

                 enc1 = chr1 >> 2;
                 enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
                 enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
                 enc4 = chr3 & 63;

                 if (isNaN(chr2)) {
                     enc3 = enc4 = 64;
                 } else if (isNaN(chr3)) {
                     enc4 = 64;
                 }

                 output = output +
                     keyStr.charAt(enc1) +
                     keyStr.charAt(enc2) +
                     keyStr.charAt(enc3) +
                     keyStr.charAt(enc4);
                 chr1 = chr2 = chr3 = "";
                 enc1 = enc2 = enc3 = enc4 = "";
             } while (i < input.length);

             return output;
         },
         decode: function (input) {
             var output = "";
             var chr1, chr2, chr3 = "";
             var enc1, enc2, enc3, enc4 = "";
             var i = 0;

             var base64test = /[^A-Za-z0-9\+\/\=]/g;
             if (base64test.exec(input)) {
                 window.alert("There were invalid base64 characters in the input text.\n" +
                     "Valid base64 characters are A-Z, a-z, 0-9, '+', '/',and '='\n" +
                     "Expect errors in decoding.");
             }
             input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");

             do {
                 enc1 = keyStr.indexOf(input.charAt(i++));
                 enc2 = keyStr.indexOf(input.charAt(i++));
                 enc3 = keyStr.indexOf(input.charAt(i++));
                 enc4 = keyStr.indexOf(input.charAt(i++));

                 chr1 = (enc1 << 2) | (enc2 >> 4);
                 chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
                 chr3 = ((enc3 & 3) << 6) | enc4;

                 output = output + String.fromCharCode(chr1);

                 if (enc3 != 64) {
                     output = output + String.fromCharCode(chr2);
                 }
                 if (enc4 != 64) {
                     output = output + String.fromCharCode(chr3);
                 }

                 chr1 = chr2 = chr3 = "";
                 enc1 = enc2 = enc3 = enc4 = "";

             } while (i < input.length);

             return output;
         }
     };
 });
