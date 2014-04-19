using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketingOffice.TicketingService.Contracts;

namespace TicketingOffice.Printing.BusinessLogic
{
    public interface IPrinting
    {
        string Print(Order order);
    }
}
