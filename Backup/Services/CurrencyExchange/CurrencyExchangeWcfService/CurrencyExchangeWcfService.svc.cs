using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TicketingOffice.CurrencyExchange.Contract;
using TicketingOffice.CurrencyExchange.BusinessLogic;

namespace TicketingOffice.CurrencyExchange.Wcf
{
    /// <summary>
    /// The currency exchange is a simple service to translate one currency to another using the buy and sell operations.
    /// </summary>
    public class CurrencyExchangeService : IExchangeService
    {
        ICurrencyExchangeManager manager = new ExchangeRatesManager();
        #region IExchangeService Members

        public double Buy(Currencies currency, double amount)
        {
            return manager.Buy(currency, amount);
        }

        public double Sell(Currencies currency, double amount)
        {
            return manager.Sell(currency, amount);
        }

        #endregion
    }
}
