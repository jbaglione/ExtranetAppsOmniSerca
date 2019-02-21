using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Web;

namespace ExtranetAppsOmniSerca.Helpers
{
    public class SoapHelper
    {
    }
    public class MyFaultLogger : IEndpointBehavior, IClientMessageInspector
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        // agregar a la referencia del ws
        // this.Endpoint.Behaviors.Add(new MyFaultLogger());
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(this);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            ServiceHelper.UltimaRespuesta = reply.ToString();
            logger.Debug(reply.ToString());
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            logger.Debug(request.ToString());
            return null;
        }
    }
}