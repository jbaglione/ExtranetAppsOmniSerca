﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ExtranetAppsOmniSerca.WSTercerosLiquidaciones {
    using System.Data;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://tempuri.org", ConfigurationName="WSTercerosLiquidaciones.TercerosLiquidacionesSoap")]
    public interface TercerosLiquidacionesSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.TercerosLiquidaciones.GetAsistencia", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetAsistencia(long pLiqId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.TercerosLiquidaciones.GetConformidad", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetConformidad(string pItmLiq);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.TercerosLiquidaciones.GetEmpresas", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetEmpresas(long pUsr, long pAcc, long pPer, long pEst, long pTip);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.TercerosLiquidaciones.GetIncidenteCalculo", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetIncidenteCalculo(long pLiqId, long pInc);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.TercerosLiquidaciones.GetIncidenteDetalle", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetIncidenteDetalle(string pItmLiq);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.TercerosLiquidaciones.GetIncidentes", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetIncidentes(long pLiqId, long pPer, long pDia, long pEst);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.TercerosLiquidaciones.GetMotivosReclamos", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetMotivosReclamos();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.TercerosLiquidaciones.GetMoviles", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetMoviles(long pUsr, long pAcc, long pPer, long pEst);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.TercerosLiquidaciones.GetResumen", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetResumen(long pLiqId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.TercerosLiquidaciones.GetResumenInsumos", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetResumenInsumos(long pLiqId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.TercerosLiquidaciones.GetResumenOtrosDescuentos", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetResumenOtrosDescuentos(long pLiqId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.TercerosLiquidaciones.GetResumenPremios", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetResumenPremios(long pLiqMovId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.TercerosLiquidaciones.GetResumenProductividad", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetResumenProductividad(long pLiqMovId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.TercerosLiquidaciones.GetUsuarioValidacion", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetUsuarioValidacion(long pUsr, long pEmp);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.TercerosLiquidaciones.Reliquidar", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        long Reliquidar(long pLiqId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.TercerosLiquidaciones.SetConformidad", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet SetConformidad(long pLiqId, long pFec, string pNro, long pRpl, long pCnf, long pMot, decimal pDif, decimal pLiq, decimal pNue, string pObs, long pUsr);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.TercerosLiquidaciones.SetOrdenServicio", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet SetOrdenServicio(long pLiqId, long pInc, long pUsr);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.TercerosLiquidaciones.SetRespuesta", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet SetRespuesta(long pLiqId, long pFec, string pNro, long pSta, string pRta, long pUsr);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface TercerosLiquidacionesSoapChannel : ExtranetAppsOmniSerca.WSTercerosLiquidaciones.TercerosLiquidacionesSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class TercerosLiquidacionesSoapClient : System.ServiceModel.ClientBase<ExtranetAppsOmniSerca.WSTercerosLiquidaciones.TercerosLiquidacionesSoap>, ExtranetAppsOmniSerca.WSTercerosLiquidaciones.TercerosLiquidacionesSoap {
        
        public TercerosLiquidacionesSoapClient() {
        }
        
        public TercerosLiquidacionesSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public TercerosLiquidacionesSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TercerosLiquidacionesSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TercerosLiquidacionesSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Data.DataSet GetAsistencia(long pLiqId) {
            return base.Channel.GetAsistencia(pLiqId);
        }
        
        public System.Data.DataSet GetConformidad(string pItmLiq) {
            return base.Channel.GetConformidad(pItmLiq);
        }
        
        public System.Data.DataSet GetEmpresas(long pUsr, long pAcc, long pPer, long pEst, long pTip) {
            return base.Channel.GetEmpresas(pUsr, pAcc, pPer, pEst, pTip);
        }
        
        public System.Data.DataSet GetIncidenteCalculo(long pLiqId, long pInc) {
            return base.Channel.GetIncidenteCalculo(pLiqId, pInc);
        }
        
        public System.Data.DataSet GetIncidenteDetalle(string pItmLiq) {
            return base.Channel.GetIncidenteDetalle(pItmLiq);
        }
        
        public System.Data.DataSet GetIncidentes(long pLiqId, long pPer, long pDia, long pEst) {
            return base.Channel.GetIncidentes(pLiqId, pPer, pDia, pEst);
        }
        
        public System.Data.DataSet GetMotivosReclamos() {
            return base.Channel.GetMotivosReclamos();
        }
        
        public System.Data.DataSet GetMoviles(long pUsr, long pAcc, long pPer, long pEst) {
            return base.Channel.GetMoviles(pUsr, pAcc, pPer, pEst);
        }
        
        public System.Data.DataSet GetResumen(long pLiqId) {
            return base.Channel.GetResumen(pLiqId);
        }
        
        public System.Data.DataSet GetResumenInsumos(long pLiqId) {
            return base.Channel.GetResumenInsumos(pLiqId);
        }
        
        public System.Data.DataSet GetResumenOtrosDescuentos(long pLiqId) {
            return base.Channel.GetResumenOtrosDescuentos(pLiqId);
        }
        
        public System.Data.DataSet GetResumenPremios(long pLiqMovId) {
            return base.Channel.GetResumenPremios(pLiqMovId);
        }
        
        public System.Data.DataSet GetResumenProductividad(long pLiqMovId) {
            return base.Channel.GetResumenProductividad(pLiqMovId);
        }
        
        public System.Data.DataSet GetUsuarioValidacion(long pUsr, long pEmp) {
            return base.Channel.GetUsuarioValidacion(pUsr, pEmp);
        }
        
        public long Reliquidar(long pLiqId) {
            return base.Channel.Reliquidar(pLiqId);
        }
        
        public System.Data.DataSet SetConformidad(long pLiqId, long pFec, string pNro, long pRpl, long pCnf, long pMot, decimal pDif, decimal pLiq, decimal pNue, string pObs, long pUsr) {
            return base.Channel.SetConformidad(pLiqId, pFec, pNro, pRpl, pCnf, pMot, pDif, pLiq, pNue, pObs, pUsr);
        }
        
        public System.Data.DataSet SetOrdenServicio(long pLiqId, long pInc, long pUsr) {
            return base.Channel.SetOrdenServicio(pLiqId, pInc, pUsr);
        }
        
        public System.Data.DataSet SetRespuesta(long pLiqId, long pFec, string pNro, long pSta, string pRta, long pUsr) {
            return base.Channel.SetRespuesta(pLiqId, pFec, pNro, pSta, pRta, pUsr);
        }
    }
}
