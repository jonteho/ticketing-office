using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using TicketingOffice.CurrencyExchange.Contract;

namespace TicketingOffice.Pricing.Contracts
{
    /// <summary>
    /// The contract of the pricing service
    /// </summary>
    [ServiceContract(Namespace = @"http://Fabrikam.com")]  
    public interface IPricingService
    {
        /// <summary>
        /// Calculate a final price
        /// </summary>
        /// <param name="reductionCode">This code is used to classify customes and to find pricing rules appropriate for different kind of customers</param>
        /// <param name="policyName">The relevant group of rules</param>
        /// <param name="listPrice">The base price if the event</param>
        /// <param name="numberOfOrders">Numbers of tickes ordered</param>
        /// <param name="currency">Currency in which the price us shown to the client</param>
        /// <returns>The calculated price</returns>
        [OperationContract]
        double CalculatePrice(int reductionCode, string policyName, int listPrice, int numberOfTickets,Currencies? currency);
    }
}
