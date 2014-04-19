using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketingOffice.CurrencyExchange.Contract;
using TicketingOffice.CurrencyExchange.DataAccess;

namespace TicketingOffice.CurrencyExchange.BusinessLogic
{
    /// <summary>
    /// Execute exchange logic
    /// </summary>
    public class ExchangeRatesManager : ICurrencyExchangeManager
    {
        
        public double Commission { get; set; }

        /// <summary>
        /// There is a 5% comission
        /// </summary>
        public ExchangeRatesManager()
        {
            Commission = 0.05;
        }
        
        #region IShowsManager Members

        /// <summary>
        /// Buy the Currency
        /// </summary>
        /// <param name="curency"></param>
        /// <param name="amount">The amount in the common currency</param>
        /// <returns>The value in the specified currency</returns>
        public double Buy(Currencies curency, double amount)
        {
           
            ExchangeRateDal dal = new ExchangeRateDal();
            double rate = dal.GetRate(curency.ToString());
            if (rate == 0)
                return amount;

            return amount * rate * (1 - Commission);
        }

        /// <summary>
        /// Sell the currency
        /// </summary>
        /// <param name="curency"></param>
        /// <param name="amount">The amount in the specified currency</param>
        /// <returns>The amount in the common currency</returns>
        public double Sell(Currencies curency, double amount)
        {
            ExchangeRateDal dal = new ExchangeRateDal();
            double rate = dal.GetRate(curency.ToString());
            if (rate == 0)
                return amount;

            return amount / rate * (1 - Commission); ;
        }

        #endregion
    }
}
