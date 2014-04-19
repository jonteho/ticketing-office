using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using TicketingOffice.PaymentService.Contracts;
using TicketingOffice.HallState.Contracts;
using TicketingOffice.CurrencyExchange.Contract;
using System.ServiceModel.Channels;

namespace TicketingOffice.TicketingService.Contracts
{
/// <summary>
/// The Contract for the ticketing service 
/// This is a request response contract
/// </summary>
[ServiceContract(Namespace = @"http://Fabrikam.com")]
public interface ITicketingService
{
    // TODO: Ex3 - Decorate the ITicketingService methods with the TransactionFlow attribute

    [OperationContract]
    [TransactionFlow(TransactionFlowOption.Mandatory)]
    Guid OrderTicket(Contracts.Order newOrder, SeatIndex[] seats);

    [OperationContract]
    [TransactionFlow(TransactionFlowOption.Mandatory)]
    Payment PayForTicket(Guid orderID, Guid payingCustomerID, double amount, PaymentType methodOfPayment, Currencies? currency, string creditCard);

    [OperationContract]
    [TransactionFlow(TransactionFlowOption.Mandatory)]
    Payment CancelTicket(Guid orderID, Guid payingCustomerID, string creditCard);

    [OperationContract]
    Order FindOrder(Guid orderID);

    [OperationContract]
    Order[] FindOrders(OrderCriteria criteria);

    [OperationContract]
    Guid[] FindBestCustomersIds(int numberOfCustomers);

    [OperationContract]
    [TransactionFlow(TransactionFlowOption.Mandatory)]
    string PrintTicket(Guid orderID);
}


// TODO: Ex2 – Examine the one-way service contract 

/// <summary>
/// The Contract for the ticketing service 
/// This is a One-Way contract
/// </summary>
[ServiceContract(Namespace = @"http://Fabrikam.com")]
public interface ITicketingServiceOneWay
{
    // TODO: Ex3 - Decorate the ITicketingServiceOneWay methods with the TransactionFlow attribute

    [OperationContract(IsOneWay = true)]
    [TransactionFlow(TransactionFlowOption.Allowed)]
    void OrderTicket(Contracts.Order newOrder, SeatIndex[] seats, Guid callID);

    [OperationContract(IsOneWay = true)]
    [TransactionFlow(TransactionFlowOption.Allowed)]
    void PayForTicket(Guid orderID, Guid payingCustomerID, double amount, PaymentType methodOfPayment, Currencies? currency, Guid callID, string creditCard);

    [OperationContract(IsOneWay = true)]
    [TransactionFlow(TransactionFlowOption.Allowed)]
    void CancelTicket(Guid orderID, Guid payingCustomerID, Guid callID, string creditCard);

    [OperationContract(IsOneWay = true)]
    void FindOrder(Guid orderID, Guid callID);

    [OperationContract(IsOneWay = true)]
    void FindOrders(OrderCriteria criteria, Guid callID);

    [OperationContract(IsOneWay = true)]
    void FindBestCustomersIds(int numberOfCustomers, Guid callID);

    [OperationContract(IsOneWay = true)]
    [TransactionFlow(TransactionFlowOption.Allowed)]
    void PrintTicket(Guid orderID, Guid callID);

}

/// <summary>
/// A callback conract for returning Ticketing results.
/// This contract is often used in conjunction with the one-way ticketing contract
/// </summary>
[ServiceContract(Namespace = @"http://Fabrikam.com")]
public interface ITicketingCallBack
{
    [OperationContract(IsOneWay = true)]
    void PaymentArrived(Payment result, Guid callID);

    [OperationContract(IsOneWay = true)]
    void OrderArrived(Order result, Guid callID);

    [OperationContract(IsOneWay = true)]
    void OrdersArrived(Order[] result, Guid callID);

    [OperationContract(IsOneWay = true)]
    void IDArrived(Guid result, Guid callID);

    [OperationContract(IsOneWay = true)]
    void IDsArrived(Guid[] result, Guid callID);

    [OperationContract(IsOneWay = true)]
    void MessageArrived(string result, Guid callID);
}

}
