using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using TicketingOffice.CurrencyExchange.Contract;

using TicketingOffice.CurrencyExchange.DataAccess;

namespace TicketingOffice.CurrencyExchange.AsmxWebService
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://Fabrikam.com")]
      // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CurrencyExchangeService : System.Web.Services.WebService, IExchangeService
    {
        ExchangeRatesManager manager = new ExchangeRatesManager();
        #region IExchangeService Members
        [WebMethod(CacheDuration = 60*60*24)]
        public double Buy(Currencies currency, double amount)
        {
            return manager.Buy(currency,amount);
        }
        [WebMethod(CacheDuration = 60 * 60 * 24)]
        public double Sell(Currencies currency, double amount)
        {
            return manager.Sell(currency,amount);
        }

        #endregion
    }

    public class ExchangeRatesManager 
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
            return amount / rate * (1 - Commission); ;
        }

        #endregion
    }
   
}