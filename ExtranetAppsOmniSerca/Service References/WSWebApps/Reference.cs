﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ExtranetAppsOmniSerca.WSWebApps {
    using System.Data;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://tempuri.org", ConfigurationName="WSWebApps.WebAppsSoap")]
    public interface WebAppsSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.WebApps.ChangePassword", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet ChangePassword(long pUsrExtId, string pOld, string pNew);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.WebApps.ForgotPassword", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet ForgotPassword(string pIde);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.WebApps.GetAlertas", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetAlertas(long pUsrExtId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.WebApps.GetSessionData", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetSessionData(string pIde);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.WebApps.Login", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet Login(string pIde, string pPsw);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.WebApps.LoginMobileGerencial", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet LoginMobileGerencial(string pIde, string pPsw);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.WebApps.SetPersonals", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet SetPersonals(long pUsrExtId, string pEmail);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface WebAppsSoapChannel : ExtranetAppsOmniSerca.WSWebApps.WebAppsSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WebAppsSoapClient : System.ServiceModel.ClientBase<ExtranetAppsOmniSerca.WSWebApps.WebAppsSoap>, ExtranetAppsOmniSerca.WSWebApps.WebAppsSoap {
        
        public WebAppsSoapClient() {
        }
        
        public WebAppsSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WebAppsSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WebAppsSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WebAppsSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Data.DataSet ChangePassword(long pUsrExtId, string pOld, string pNew) {
            return base.Channel.ChangePassword(pUsrExtId, pOld, pNew);
        }
        
        public System.Data.DataSet ForgotPassword(string pIde) {
            return base.Channel.ForgotPassword(pIde);
        }
        
        public System.Data.DataSet GetAlertas(long pUsrExtId) {
            return base.Channel.GetAlertas(pUsrExtId);
        }
        
        public System.Data.DataSet GetSessionData(string pIde) {
            return base.Channel.GetSessionData(pIde);
        }
        
        public System.Data.DataSet Login(string pIde, string pPsw) {
            return base.Channel.Login(pIde, pPsw);
        }
        
        public System.Data.DataSet LoginMobileGerencial(string pIde, string pPsw) {
            return base.Channel.LoginMobileGerencial(pIde, pPsw);
        }
        
        public System.Data.DataSet SetPersonals(long pUsrExtId, string pEmail) {
            return base.Channel.SetPersonals(pUsrExtId, pEmail);
        }
    }
}
