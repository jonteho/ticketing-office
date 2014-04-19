using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketingOffice.TicketingService.Contracts;


namespace TicketingOffice.TicketingService.BusinessLogic
{
   
    public interface ITicketingManager
	{
        Guid OrderTicket(Order newOrder);

        void UpdateOrder(Order newOrder);

        void DeleteOrder(Guid orderID);

        Order[] FindOrdersByCriteria(OrderCriteria criteria);

        Order FindOrder(Guid OrderID);

        Guid[] GetBestCustomersID(int numOfCustomers);
	} 
}
