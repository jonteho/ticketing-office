using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketingOffice.CurrencyExchange.Contract;


namespace TicketingOffice.CurrencyExchange.BusinessLogic
{
    public interface ICurrencyExchangeManager
    {
        double Buy(Currencies curency, double amount);

        double Sell(Currencies curency, double amount);
    }
}
