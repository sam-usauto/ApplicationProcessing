﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ScoringSolutionStaging
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://dvtransaction.com/", ConfigurationName="ScoringSolutionStaging.DataviewServiceSoap")]
    public interface DataviewServiceSoap
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://dvtransaction.com/Ping", ReplyAction="*")]
        System.Threading.Tasks.Task<ScoringSolutionStaging.PingResponse> PingAsync(ScoringSolutionStaging.PingRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://dvtransaction.com/LoadSystem", ReplyAction="*")]
        System.Threading.Tasks.Task<ScoringSolutionStaging.LoadSystemResponse> LoadSystemAsync(ScoringSolutionStaging.LoadSystemRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://dvtransaction.com/ProcessApplication", ReplyAction="*")]
        System.Threading.Tasks.Task<ScoringSolutionStaging.ProcessApplicationResponse> ProcessApplicationAsync(ScoringSolutionStaging.ProcessApplicationRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://dvtransaction.com/ProcessApplicationXML", ReplyAction="*")]
        System.Threading.Tasks.Task<ScoringSolutionStaging.ProcessApplicationXMLResponse> ProcessApplicationXMLAsync(ScoringSolutionStaging.ProcessApplicationXMLRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class PingRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Ping", Namespace="http://dvtransaction.com/", Order=0)]
        public ScoringSolutionStaging.PingRequestBody Body;
        
        public PingRequest()
        {
        }
        
        public PingRequest(ScoringSolutionStaging.PingRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class PingRequestBody
    {
        
        public PingRequestBody()
        {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class PingResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="PingResponse", Namespace="http://dvtransaction.com/", Order=0)]
        public ScoringSolutionStaging.PingResponseBody Body;
        
        public PingResponse()
        {
        }
        
        public PingResponse(ScoringSolutionStaging.PingResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://dvtransaction.com/")]
    public partial class PingResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string PingResult;
        
        public PingResponseBody()
        {
        }
        
        public PingResponseBody(string PingResult)
        {
            this.PingResult = PingResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class LoadSystemRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="LoadSystem", Namespace="http://dvtransaction.com/", Order=0)]
        public ScoringSolutionStaging.LoadSystemRequestBody Body;
        
        public LoadSystemRequest()
        {
        }
        
        public LoadSystemRequest(ScoringSolutionStaging.LoadSystemRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class LoadSystemRequestBody
    {
        
        public LoadSystemRequestBody()
        {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class LoadSystemResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="LoadSystemResponse", Namespace="http://dvtransaction.com/", Order=0)]
        public ScoringSolutionStaging.LoadSystemResponseBody Body;
        
        public LoadSystemResponse()
        {
        }
        
        public LoadSystemResponse(ScoringSolutionStaging.LoadSystemResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://dvtransaction.com/")]
    public partial class LoadSystemResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string LoadSystemResult;
        
        public LoadSystemResponseBody()
        {
        }
        
        public LoadSystemResponseBody(string LoadSystemResult)
        {
            this.LoadSystemResult = LoadSystemResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ProcessApplicationRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ProcessApplication", Namespace="http://dvtransaction.com/", Order=0)]
        public ScoringSolutionStaging.ProcessApplicationRequestBody Body;
        
        public ProcessApplicationRequest()
        {
        }
        
        public ProcessApplicationRequest(ScoringSolutionStaging.ProcessApplicationRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://dvtransaction.com/")]
    public partial class ProcessApplicationRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string XmlRequest;
        
        public ProcessApplicationRequestBody()
        {
        }
        
        public ProcessApplicationRequestBody(string XmlRequest)
        {
            this.XmlRequest = XmlRequest;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ProcessApplicationResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ProcessApplicationResponse", Namespace="http://dvtransaction.com/", Order=0)]
        public ScoringSolutionStaging.ProcessApplicationResponseBody Body;
        
        public ProcessApplicationResponse()
        {
        }
        
        public ProcessApplicationResponse(ScoringSolutionStaging.ProcessApplicationResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://dvtransaction.com/")]
    public partial class ProcessApplicationResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string ProcessApplicationResult;
        
        public ProcessApplicationResponseBody()
        {
        }
        
        public ProcessApplicationResponseBody(string ProcessApplicationResult)
        {
            this.ProcessApplicationResult = ProcessApplicationResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ProcessApplicationXMLRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ProcessApplicationXML", Namespace="http://dvtransaction.com/", Order=0)]
        public ScoringSolutionStaging.ProcessApplicationXMLRequestBody Body;
        
        public ProcessApplicationXMLRequest()
        {
        }
        
        public ProcessApplicationXMLRequest(ScoringSolutionStaging.ProcessApplicationXMLRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://dvtransaction.com/")]
    public partial class ProcessApplicationXMLRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public System.Xml.XmlElement XmlRequest;
        
        public ProcessApplicationXMLRequestBody()
        {
        }
        
        public ProcessApplicationXMLRequestBody(System.Xml.XmlElement XmlRequest)
        {
            this.XmlRequest = XmlRequest;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ProcessApplicationXMLResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ProcessApplicationXMLResponse", Namespace="http://dvtransaction.com/", Order=0)]
        public ScoringSolutionStaging.ProcessApplicationXMLResponseBody Body;
        
        public ProcessApplicationXMLResponse()
        {
        }
        
        public ProcessApplicationXMLResponse(ScoringSolutionStaging.ProcessApplicationXMLResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://dvtransaction.com/")]
    public partial class ProcessApplicationXMLResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public System.Xml.XmlElement ProcessApplicationXMLResult;
        
        public ProcessApplicationXMLResponseBody()
        {
        }
        
        public ProcessApplicationXMLResponseBody(System.Xml.XmlElement ProcessApplicationXMLResult)
        {
            this.ProcessApplicationXMLResult = ProcessApplicationXMLResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    public interface DataviewServiceSoapChannel : ScoringSolutionStaging.DataviewServiceSoap, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    public partial class DataviewServiceSoapClient : System.ServiceModel.ClientBase<ScoringSolutionStaging.DataviewServiceSoap>, ScoringSolutionStaging.DataviewServiceSoap
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public DataviewServiceSoapClient(EndpointConfiguration endpointConfiguration) : 
                base(DataviewServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), DataviewServiceSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public DataviewServiceSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(DataviewServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public DataviewServiceSoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(DataviewServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public DataviewServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ScoringSolutionStaging.PingResponse> ScoringSolutionStaging.DataviewServiceSoap.PingAsync(ScoringSolutionStaging.PingRequest request)
        {
            return base.Channel.PingAsync(request);
        }
        
        public System.Threading.Tasks.Task<ScoringSolutionStaging.PingResponse> PingAsync()
        {
            ScoringSolutionStaging.PingRequest inValue = new ScoringSolutionStaging.PingRequest();
            inValue.Body = new ScoringSolutionStaging.PingRequestBody();
            return ((ScoringSolutionStaging.DataviewServiceSoap)(this)).PingAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ScoringSolutionStaging.LoadSystemResponse> ScoringSolutionStaging.DataviewServiceSoap.LoadSystemAsync(ScoringSolutionStaging.LoadSystemRequest request)
        {
            return base.Channel.LoadSystemAsync(request);
        }
        
        public System.Threading.Tasks.Task<ScoringSolutionStaging.LoadSystemResponse> LoadSystemAsync()
        {
            ScoringSolutionStaging.LoadSystemRequest inValue = new ScoringSolutionStaging.LoadSystemRequest();
            inValue.Body = new ScoringSolutionStaging.LoadSystemRequestBody();
            return ((ScoringSolutionStaging.DataviewServiceSoap)(this)).LoadSystemAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ScoringSolutionStaging.ProcessApplicationResponse> ScoringSolutionStaging.DataviewServiceSoap.ProcessApplicationAsync(ScoringSolutionStaging.ProcessApplicationRequest request)
        {
            return base.Channel.ProcessApplicationAsync(request);
        }
        
        public System.Threading.Tasks.Task<ScoringSolutionStaging.ProcessApplicationResponse> ProcessApplicationAsync(string XmlRequest)
        {
            ScoringSolutionStaging.ProcessApplicationRequest inValue = new ScoringSolutionStaging.ProcessApplicationRequest();
            inValue.Body = new ScoringSolutionStaging.ProcessApplicationRequestBody();
            inValue.Body.XmlRequest = XmlRequest;
            return ((ScoringSolutionStaging.DataviewServiceSoap)(this)).ProcessApplicationAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ScoringSolutionStaging.ProcessApplicationXMLResponse> ScoringSolutionStaging.DataviewServiceSoap.ProcessApplicationXMLAsync(ScoringSolutionStaging.ProcessApplicationXMLRequest request)
        {
            return base.Channel.ProcessApplicationXMLAsync(request);
        }
        
        public System.Threading.Tasks.Task<ScoringSolutionStaging.ProcessApplicationXMLResponse> ProcessApplicationXMLAsync(System.Xml.XmlElement XmlRequest)
        {
            ScoringSolutionStaging.ProcessApplicationXMLRequest inValue = new ScoringSolutionStaging.ProcessApplicationXMLRequest();
            inValue.Body = new ScoringSolutionStaging.ProcessApplicationXMLRequestBody();
            inValue.Body.XmlRequest = XmlRequest;
            return ((ScoringSolutionStaging.DataviewServiceSoap)(this)).ProcessApplicationXMLAsync(inValue);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.DataviewServiceSoap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.DataviewServiceSoap12))
            {
                System.ServiceModel.Channels.CustomBinding result = new System.ServiceModel.Channels.CustomBinding();
                System.ServiceModel.Channels.TextMessageEncodingBindingElement textBindingElement = new System.ServiceModel.Channels.TextMessageEncodingBindingElement();
                textBindingElement.MessageVersion = System.ServiceModel.Channels.MessageVersion.CreateVersion(System.ServiceModel.EnvelopeVersion.Soap12, System.ServiceModel.Channels.AddressingVersion.None);
                result.Elements.Add(textBindingElement);
                System.ServiceModel.Channels.HttpTransportBindingElement httpBindingElement = new System.ServiceModel.Channels.HttpTransportBindingElement();
                httpBindingElement.AllowCookies = true;
                httpBindingElement.MaxBufferSize = int.MaxValue;
                httpBindingElement.MaxReceivedMessageSize = int.MaxValue;
                result.Elements.Add(httpBindingElement);
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.DataviewServiceSoap))
            {
                return new System.ServiceModel.EndpointAddress("http://dv360-uat.dataview360.com/usautosales/dataviewservice.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.DataviewServiceSoap12))
            {
                return new System.ServiceModel.EndpointAddress("http://dv360-uat.dataview360.com/usautosales/dataviewservice.asmx");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        public enum EndpointConfiguration
        {
            
            DataviewServiceSoap,
            
            DataviewServiceSoap12,
        }
    }
}
