﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ExtranetAppsOmniSerca.WSProduccionOperativaClientes {
    using System.Data;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://tempuri.org", ConfigurationName="WSProduccionOperativaClientes.ClientesOperativosSoap")]
    public interface ClientesOperativosSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.ClientesOperativos.GetClientesUsuario", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetClientesUsuario(long pUsr);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.ClientesOperativos.GetDenuncias", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetDenuncias(long pUsr, string pDes, string pHas);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.ClientesOperativos.GetErroresAutorizacion", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetErroresAutorizacion(long pUsr, long pDes, long pHas);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.ClientesOperativos.GetFinalizados", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetFinalizados(long pUsr, long pDes, long pHas, long pCli);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.ClientesOperativos.GetOperativaEnCurso", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetOperativaEnCurso(long pUsr);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.ClientesOperativos.GetUsuarioValidacion", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetUsuarioValidacion(long pUsr);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.ClientesOperativos.IsReclamado", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet IsReclamado(long pInc);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.ClientesOperativos.SetCorreccion", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet SetCorreccion(long pInc, string pOrd, string pAfl, long pUsr);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WebServices.ClientesOperativos.SetReclamo", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet SetReclamo(long pInc, string pObs, long pUsr);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ClientesOperativosSoapChannel : ExtranetAppsOmniSerca.WSProduccionOperativaClientes.ClientesOperativosSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ClientesOperativosSoapClient : System.ServiceModel.ClientBase<ExtranetAppsOmniSerca.WSProduccionOperativaClientes.ClientesOperativosSoap>, ExtranetAppsOmniSerca.WSProduccionOperativaClientes.ClientesOperativosSoap {
        
        public ClientesOperativosSoapClient() {
        }
        
        public ClientesOperativosSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ClientesOperativosSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ClientesOperativosSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ClientesOperativosSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Data.DataSet GetClientesUsuario(long pUsr) {
            return base.Channel.GetClientesUsuario(pUsr);
        }
        
        public System.Data.DataSet GetDenuncias(long pUsr, string pDes, string pHas) {
            return base.Channel.GetDenuncias(pUsr, pDes, pHas);
        }
        
        public System.Data.DataSet GetErroresAutorizacion(long pUsr, long pDes, long pHas) {
            return base.Channel.GetErroresAutorizacion(pUsr, pDes, pHas);
        }
        
        public System.Data.DataSet GetFinalizados(long pUsr, long pDes, long pHas, long pCli) {
            return base.Channel.GetFinalizados(pUsr, pDes, pHas, pCli);
        }
        
        public System.Data.DataSet GetOperativaEnCurso(long pUsr) {
            return base.Channel.GetOperativaEnCurso(pUsr);
        }
        
        public System.Data.DataSet GetUsuarioValidacion(long pUsr) {
            return base.Channel.GetUsuarioValidacion(pUsr);
        }
        
        public System.Data.DataSet IsReclamado(long pInc) {
            return base.Channel.IsReclamado(pInc);
        }
        
        public System.Data.DataSet SetCorreccion(long pInc, string pOrd, string pAfl, long pUsr) {
            return base.Channel.SetCorreccion(pInc, pOrd, pAfl, pUsr);
        }
        
        public System.Data.DataSet SetReclamo(long pInc, string pObs, long pUsr) {
            return base.Channel.SetReclamo(pInc, pObs, pUsr);
        }
    }
}
