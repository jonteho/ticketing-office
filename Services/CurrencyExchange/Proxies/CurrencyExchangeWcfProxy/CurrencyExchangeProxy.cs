using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketingOffice.CurrencyExchange.Contract;
using System.ServiceModel;
using TicketingOffice.Common.Helpers;
using TicketingOffice.Common.Properties;

namespace TicketingOffice.CurrencyExchangeWcfProxy
{
    /// <summary>
    /// A WCF proxy to the currency exchange service implemented using WCF \ ASP.Net ASMX technologies.
    /// </summary>
   
    public class CurrencyExchangeProxy : IExchangeService
    {
        ChannelFactory<IExchangeService> currencyChf;
        ChannelFactory<ICurrencyExchangeAsmxService> currencyAsmxChf;

        IExchangeService currencyProx = null;
        ICurrencyExchangeAsmxService currencyAsmxProx = null;

        public bool UseAsmx { get; set; }

        public CurrencyExchangeProxy()
        {
            currencyChf = new ChannelFactory<IExchangeService>("CurrencyExchangeEP");
            currencyAsmxChf = new ChannelFactory<ICurrencyExchangeAsmxService>("CurrencyExchangeAsmxEP");
        }

        #region IExchangeService Members

        public double Buy(Currencies currency, double amount)
        {        
           
            try
            {
                if (!UseAsmx)
                {
                    // Call the WCF service
                    currencyProx = currencyChf.CreateChannel();
                    return currencyProx.Buy(currency, amount);
                }
                else
                {
                    // Call the WCF service
                    currencyAsmxProx = currencyAsmxChf.CreateChannel();
                    return currencyAsmxProx.Buy(currency, amount);
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, StringsResource.FailedToContactCurrencyExchange + " " + ex.Message);
                throw new FaultException<CurrencyExchangeException>
                    (new CurrencyExchangeException(StringsResource.FailedToContactCurrencyExchange + " " + ex.Message, ex));
            }
            finally
            {
                var channel = currencyProx as ICommunicationObject;
                if ((channel != null) && (channel.State == CommunicationState.Opened))
                    channel.Close();

                channel = currencyAsmxProx as ICommunicationObject;
                if ((channel != null) && (channel.State == CommunicationState.Opened))
                    channel.Close();
            }
        }

        public double Sell(Currencies currency, double amount)
        {         

            try
            {
                if (!UseAsmx)
                {
                    // Call the WCF service
                    currencyProx = currencyChf.CreateChannel();
                    return currencyProx.Sell(currency, amount);
                }
                else
                {
                    // Call the WCF service
                    currencyAsmxProx = currencyAsmxChf.CreateChannel();
                    return currencyAsmxProx.Sell(currency, amount);
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, StringsResource.FailedToContactCurrencyExchange + " " + ex.Message);
                throw new FaultException<CurrencyExchangeException>
                    (new CurrencyExchangeException(StringsResource.FailedToContactCurrencyExchange + " " + ex.Message, ex));
            }
            finally
            {
                var channel = currencyProx as ICommunicationObject;
                if ((channel != null) && (channel.State == CommunicationState.Opened))
                    channel.Close();

                channel = currencyAsmxProx as ICommunicationObject;
                if ((channel != null) && (channel.State == CommunicationState.Opened))
                    channel.Close();
            }
        }

        #endregion
    }
}
