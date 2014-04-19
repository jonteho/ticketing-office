using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketingOffice.Common.Helpers;
using TicketingOffice.Common.Exceptions;
using TicketingOffice.Common.Properties;

namespace TicketingOffice.CurrencyExchange.DataAccess
{
    public class ExchangeRateDal
    {
        public double GetRate(string currency)
        {
            try 
        	{
                using (TicketingOfficeExchangeEntities ctx = new TicketingOfficeExchangeEntities())
                {
                    var res = (from r in ctx.ExchangeRates
                             where r.Currency == currency
                             select r.Rate).FirstOrDefault();

                    return res;
                }
        	}
        	catch (Exception ex)
        	{
        		LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.RetrieveError, "ExchangeRate", ex.Message), ex) { EntityType = typeof(ExchangeRate) };
  
        	}    
        }


        public void SetRate(string currency, double rate)
        {
            try
            {
                using (TicketingOfficeExchangeEntities ctx = new TicketingOfficeExchangeEntities())
                {
                    ExchangeRate cur = (from r in ctx.ExchangeRates
                                           where r.Currency == currency
                                           select r).FirstOrDefault();

                    if (cur != null)
                        cur.Rate = rate;
                    else
                        ctx.ExchangeRates.AddObject(new ExchangeRate() { Currency = currency, Rate = rate });

                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.RetrieveError, "ExchangeRate", ex.Message), ex) { EntityType = typeof(ExchangeRate) };

            }
        }
    }
}
