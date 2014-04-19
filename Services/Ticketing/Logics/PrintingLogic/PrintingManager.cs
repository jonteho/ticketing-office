using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketingOffice.TicketingService.Contracts;

namespace TicketingOffice.Printing.BusinessLogic
{
    /// <summary>
    /// The printing manager is responsible of printing tickets.
    /// This is a printing manager that prints to the console.
    /// </summary>
    public class ConsolePrintingManager : IPrinting
    {        
    
        #region IPrinting Members

        public string Print(Order order)
        {
            string result;
            switch (order.State)
            {
                case OrderState.Created:
                    result = "The order is not approved please pay the bill";
                    break;
                case OrderState.Approved:
                     result = order.ToString();
                    break;
                case OrderState.Canceled:
                    result = "The order was canceled";
                    break;
                default:
                    result = "Error: Order is in an undefined state";
                    break;
            }          

            Console.WriteLine(result);

            return result;
        }

        #endregion
        }
}
