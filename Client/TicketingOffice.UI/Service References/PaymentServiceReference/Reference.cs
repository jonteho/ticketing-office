﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30128.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TicketingOffice.UI.PaymentServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.fabrikam.com", ConfigurationName="PaymentServiceReference.IPaymentService")]
    public interface IPaymentService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.fabrikam.com/IPaymentService/PayForOrder", ReplyAction="http://www.fabrikam.com/IPaymentService/PayForOrderResponse")]
        TicketingOffice.PaymentService.Contracts.Payment PayForOrder(System.Guid orderID, System.Guid payingCustomerID, double amount, TicketingOffice.PaymentService.Contracts.PaymentType methodOfPayment, string creditCardNumber);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.fabrikam.com/IPaymentService/Refund", ReplyAction="http://www.fabrikam.com/IPaymentService/RefundResponse")]
        TicketingOffice.PaymentService.Contracts.Payment Refund(long paymentID, System.Guid customerID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.fabrikam.com/IPaymentService/FindPayments", ReplyAction="http://www.fabrikam.com/IPaymentService/FindPaymentsResponse")]
        TicketingOffice.PaymentService.Contracts.Payment[] FindPayments(TicketingOffice.PaymentService.Contracts.PaymentCriteria criteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.fabrikam.com/IPaymentService/FindPayment", ReplyAction="http://www.fabrikam.com/IPaymentService/FindPaymentResponse")]
        TicketingOffice.PaymentService.Contracts.Payment FindPayment(long paymentID);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IPaymentServiceChannel : TicketingOffice.UI.PaymentServiceReference.IPaymentService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PaymentServiceClient : System.ServiceModel.ClientBase<TicketingOffice.UI.PaymentServiceReference.IPaymentService>, TicketingOffice.UI.PaymentServiceReference.IPaymentService {
        
        public PaymentServiceClient() {
        }
        
        public PaymentServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public PaymentServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PaymentServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PaymentServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public TicketingOffice.PaymentService.Contracts.Payment PayForOrder(System.Guid orderID, System.Guid payingCustomerID, double amount, TicketingOffice.PaymentService.Contracts.PaymentType methodOfPayment, string creditCardNumber) {
            return base.Channel.PayForOrder(orderID, payingCustomerID, amount, methodOfPayment, creditCardNumber);
        }
        
        public TicketingOffice.PaymentService.Contracts.Payment Refund(long paymentID, System.Guid customerID) {
            return base.Channel.Refund(paymentID, customerID);
        }
        
        public TicketingOffice.PaymentService.Contracts.Payment[] FindPayments(TicketingOffice.PaymentService.Contracts.PaymentCriteria criteria) {
            return base.Channel.FindPayments(criteria);
        }
        
        public TicketingOffice.PaymentService.Contracts.Payment FindPayment(long paymentID) {
            return base.Channel.FindPayment(paymentID);
        }
    }
}