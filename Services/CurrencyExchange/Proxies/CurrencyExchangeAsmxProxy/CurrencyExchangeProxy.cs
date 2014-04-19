using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicketingOffice.CurrencyExchange.Contract;
using TicketingOffice.Common.Helpers;
using TicketingOffice.Common.Properties;

namespace TicketingOffice.CurrencyExchangeAsmxProxy
{
    public class CurrencyExchangeProxy :IExchangeService
    {
       #region IExchangeService Members
        AsmxExchange.CurrencyExchangeService prox;

        public double Buy(Currencies currency, double amount)
        {
            double res;
            try
            {
                using (prox = new AsmxExchange.CurrencyExchangeService())
                {
                    res = (prox.Buy(MapCurrency(currency), amount));
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, StringsResource.FailedToContactCurrencyExchange + " " + ex.Message);
                throw new CurrencyExchangeException(StringsResource.FailedToContactCurrencyExchange + " " + ex.Message, ex);
            }
           
            return res;
           
        }

        public double Sell(Currencies currency, double amount)
        {
            double res;
            try
            {
                using (prox = new AsmxExchange.CurrencyExchangeService())
                {
                    res = (prox.Sell(MapCurrency(currency), amount));
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, StringsResource.FailedToContactCurrencyExchange + " " + ex.Message);
                throw new CurrencyExchangeException(StringsResource.FailedToContactCurrencyExchange + " " + ex.Message, ex);
            }
           
            return res;           
        }

        #endregion

        private AsmxExchange.Currencies MapCurrency(Currencies currency)
        {
            return (AsmxExchange.Currencies)Enum.Parse(typeof(AsmxExchange.Currencies),currency.ToString());   
        }
    }
}