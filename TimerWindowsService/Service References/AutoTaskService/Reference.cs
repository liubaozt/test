﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18444
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace TimerWindowsService.AutoTaskService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="AutoTaskService.AutoTaskSoap")]
    public interface AutoTaskSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/StartTask", ReplyAction="*")]
        int StartTask();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ExcuteAutoBackUp", ReplyAction="*")]
        int ExcuteAutoBackUp();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface AutoTaskSoapChannel : TimerWindowsService.AutoTaskService.AutoTaskSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class AutoTaskSoapClient : System.ServiceModel.ClientBase<TimerWindowsService.AutoTaskService.AutoTaskSoap>, TimerWindowsService.AutoTaskService.AutoTaskSoap {
        
        public AutoTaskSoapClient() {
        }
        
        public AutoTaskSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public AutoTaskSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AutoTaskSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AutoTaskSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public int StartTask() {
            return base.Channel.StartTask();
        }
        
        public int ExcuteAutoBackUp() {
            return base.Channel.ExcuteAutoBackUp();
        }
    }
}