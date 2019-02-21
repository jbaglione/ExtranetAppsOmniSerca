﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ExtranetAppsOmniSerca.WSEmpleadosLiquidaciones {
    using System.Data;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://tempuri.org", ConfigurationName="WSEmpleadosLiquidaciones.EmpleadosLiquidacionesSoap")]
    public interface EmpleadosLiquidacionesSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.EmpleadosLiquidaciones.GetConformidad", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetConformidad(string pItmLiq);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.EmpleadosLiquidaciones.GetEmpleados", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetEmpleados(long pUsr, long pAcc, long pPer, long pEst);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.EmpleadosLiquidaciones.GetLiquidacionDetalle", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetLiquidacionDetalle(long pLiqId, long pEst);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.EmpleadosLiquidaciones.GetMotivosReclamos", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetMotivosReclamos();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.EmpleadosLiquidaciones.GetPeriodos", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetPeriodos();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.EmpleadosLiquidaciones.GetResumen", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetResumen(long pLiqId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.EmpleadosLiquidaciones.GetUsuarioValidacion", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetUsuarioValidacion(long pUsr);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.EmpleadosLiquidaciones.Reliquidar", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        long Reliquidar(long pLiqId, long pUsr);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.EmpleadosLiquidaciones.SetConformidad", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet SetConformidad(string pItm, long pCnf, long pMot, string pHEnt, string pMEnt, string pHSal, string pMSal, string pObs, long pUsr);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.EmpleadosLiquidaciones.SetRespuesta", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet SetRespuesta(string pItm, long pSta, string pRta, long pUsr);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface EmpleadosLiquidacionesSoapChannel : ExtranetAppsOmniSerca.WSEmpleadosLiquidaciones.EmpleadosLiquidacionesSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class EmpleadosLiquidacionesSoapClient : System.ServiceModel.ClientBase<ExtranetAppsOmniSerca.WSEmpleadosLiquidaciones.EmpleadosLiquidacionesSoap>, ExtranetAppsOmniSerca.WSEmpleadosLiquidaciones.EmpleadosLiquidacionesSoap {
        
        public EmpleadosLiquidacionesSoapClient() {
        }
        
        public EmpleadosLiquidacionesSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public EmpleadosLiquidacionesSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EmpleadosLiquidacionesSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EmpleadosLiquidacionesSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Data.DataSet GetConformidad(string pItmLiq) {
            return base.Channel.GetConformidad(pItmLiq);
        }
        
        public System.Data.DataSet GetEmpleados(long pUsr, long pAcc, long pPer, long pEst) {
            return base.Channel.GetEmpleados(pUsr, pAcc, pPer, pEst);
        }
        
        public System.Data.DataSet GetLiquidacionDetalle(long pLiqId, long pEst) {
            return base.Channel.GetLiquidacionDetalle(pLiqId, pEst);
        }
        
        public System.Data.DataSet GetMotivosReclamos() {
            return base.Channel.GetMotivosReclamos();
        }
        
        public System.Data.DataSet GetPeriodos() {
            return base.Channel.GetPeriodos();
        }
        
        public System.Data.DataSet GetResumen(long pLiqId) {
            return base.Channel.GetResumen(pLiqId);
        }
        
        public System.Data.DataSet GetUsuarioValidacion(long pUsr) {
            return base.Channel.GetUsuarioValidacion(pUsr);
        }
        
        public long Reliquidar(long pLiqId, long pUsr) {
            return base.Channel.Reliquidar(pLiqId, pUsr);
        }
        
        public System.Data.DataSet SetConformidad(string pItm, long pCnf, long pMot, string pHEnt, string pMEnt, string pHSal, string pMSal, string pObs, long pUsr) {
            return base.Channel.SetConformidad(pItm, pCnf, pMot, pHEnt, pMEnt, pHSal, pMSal, pObs, pUsr);
        }
        
        public System.Data.DataSet SetRespuesta(string pItm, long pSta, string pRta, long pUsr) {
            return base.Channel.SetRespuesta(pItm, pSta, pRta, pUsr);
        }
    }
}
