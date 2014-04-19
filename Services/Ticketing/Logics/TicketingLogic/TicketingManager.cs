using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketingOffice.TicketingService.Contracts;
using TicketingOffice.TicketingService.DataAccess;

namespace TicketingOffice.TicketingService.BusinessLogic
{
    /// <summary>
    /// Execute the ticketing logic.
    /// In this simple example there are only calls to the data access layer
    /// </summary>
    public class TicketingManager : ITicketingManager
    {
        #region ITicketingManager Members



        public Guid OrderTicket(Contracts.Order newOrder)
        {
            OrderDal dal = new OrderDal();
            return dal.CreateEntity(newOrder);
        }

        public void UpdateOrder(Contracts.Order newOrder)
        {
            OrderDal dal = new OrderDal();
            dal.UpdateEntity(newOrder);
        }

        public void DeleteOrder(Guid orderID)
        {
            OrderDal dal = new OrderDal();
            dal.DeleteEntity(orderID);
        }
       
     
        public Contracts.Order[] FindOrdersByCriteria(Contracts.OrderCriteria criteria)
        {
            OrderDal dal = new OrderDal();
            return dal.GetOrdersByCriteria(criteria);
        }

        public Contracts.Order FindOrder(Guid OrderID)
        {
            OrderDal dal = new OrderDal();
            return dal.GetEntity(OrderID);
        }

        public Guid[] GetBestCustomersID(int numOfCustomers)
        {
            OrderDal dal = new OrderDal();
            return dal.GetBestCustomersID(numOfCustomers);
        }

        #endregion
    }
}
