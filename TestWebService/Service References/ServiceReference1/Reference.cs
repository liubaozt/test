﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18444
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestWebService.ServiceReference1 {
    using System.Data;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.DataServiceSoap")]
    public interface DataServiceSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/fun1", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        int fun1(out string _assetheader);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/fun2", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        int fun2(out System.Data.DataTable _table, string _assetExpression);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/fun3", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        int fun3(out string _storeSiteheader);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/fun4", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        int fun4(out System.Data.DataTable _table, string _storeSiteExpression);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/fun5", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        int fun5(out string _labelStyle, int _class);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/fun6", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        int fun6(int _class, string _labelStyle);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface DataServiceSoapChannel : TestWebService.ServiceReference1.DataServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class DataServiceSoapClient : System.ServiceModel.ClientBase<TestWebService.ServiceReference1.DataServiceSoap>, TestWebService.ServiceReference1.DataServiceSoap {
        
        public DataServiceSoapClient() {
        }
        
        public DataServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public DataServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DataServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DataServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public int fun1(out string _assetheader) {
            return base.Channel.fun1(out _assetheader);
        }
        
        public int fun2(out System.Data.DataTable _table, string _assetExpression) {
            return base.Channel.fun2(out _table, _assetExpression);
        }
        
        public int fun3(out string _storeSiteheader) {
            return base.Channel.fun3(out _storeSiteheader);
        }
        
        public int fun4(out System.Data.DataTable _table, string _storeSiteExpression) {
            return base.Channel.fun4(out _table, _storeSiteExpression);
        }
        
        public int fun5(out string _labelStyle, int _class) {
            return base.Channel.fun5(out _labelStyle, _class);
        }
        
        public int fun6(int _class, string _labelStyle) {
            return base.Channel.fun6(_class, _labelStyle);
        }
    }
}