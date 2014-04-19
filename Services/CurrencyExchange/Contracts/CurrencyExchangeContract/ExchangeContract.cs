using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace TicketingOffice.CurrencyExchange.Contract
{
    [ServiceContract(Namespace = @"http://Fabrikam.com")]   
    public interface IExchangeService
    {
        /// <summary>
        /// Buy the currency
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        [OperationContract]
        double Buy(Currencies currency, double amount);
        /// <summary>
        /// Sell the currency
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        [OperationContract]
        double Sell(Currencies currency, double amount);
    }


    //The same contract tuned for interoperability with an asmx service
    [ServiceContract(Namespace = "http://Fabrikam.com")]
    public interface ICurrencyExchangeAsmxService
    {

        [System.ServiceModel.OperationContractAttribute(Action = "http://Fabrikam.com/Buy", ReplyAction = "*")]
        double Buy(Currencies currency, double amount);

        [System.ServiceModel.OperationContractAttribute(Action = "http://Fabrikam.com/Sell", ReplyAction = "*")]
        double Sell(Currencies currency, double amount);
    }

    [DataContract(Namespace = @"http://Fabrikam.com")]
    public enum Currencies
    {
        [EnumMember]
        Dollar,
        [EnumMember]
        Euro,
        [EnumMember]
        Yen,
        [EnumMember]
        Pound,
        [EnumMember]
        Dinar
    }
}
