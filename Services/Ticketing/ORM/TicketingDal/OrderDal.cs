using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketingOffice.Common;
using TicketingOffice.Common.Properties;
using TicketingOffice.Common.Exceptions;
using TicketingOffice.Common.Helpers;

namespace TicketingOffice.TicketingService.DataAccess
{
    /// <summary>
    /// The data access layer for managing orders. 
    /// The OrderDal exposes orders as defined in the contracts assembly.
    /// The dal is based on Entity Framework
    /// </summary>
    public class OrderDal : IEntityCRUD<Contracts.Order,Guid>
    {
        /// <summary>
        /// Retrieve a collection of orders according to criteria parameter.
        /// The criteria is used to create the where statement fot the internal EF query. 
        /// </summary>
        /// <param name="criteria">The "where" criteria to retrieve entities</param>
        /// <returns></returns>
        public Contracts.Order[] GetOrdersByCriteria(Contracts.OrderCriteria criteria)
        {
            Contracts.Order[] res;
            try
            {
                using (TicketingOfficeOrderEntities ctx = new TicketingOfficeOrderEntities())
                {
                    IQueryable<Order> query;
                    query = ctx.Orders;

                    if (criteria.Date != null)
                        query = query.Where(ord => ord.DateOfPurchase == criteria.Date);
                    if (criteria.CustomerID != null)
                        query = query.Where(ord => ord.CustomerID == criteria.CustomerID);
                    if (criteria.EventID != null)
                        query = query.Where(ord => ord.EventID == criteria.EventID);
                    if ((criteria.FromDate != null) && (criteria.ToDate != null))
                        query = query.Where(ord => ((ord.DateOfPurchase >= criteria.FromDate) &&
                                                       (ord.DateOfPurchase <= criteria.ToDate)));

                    var tmp = query.ToArray();
                    res = tmp.Select(ord => MapOrder(ord)).ToArray();
                }               
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.RetrieveError, "Order", ex.Message), ex) { EntityType = typeof(Order) };
            }
            return res;
        }

        /// <summary>
        /// Helper to map ORM odrers (EF class) to a business entity Contracts.Order
        /// </summary>
        /// <param name="ord"></param>
        /// <returns></returns>
        private Contracts.Order MapOrder(Order ord)
        {
            return new Contracts.Order()
            {
                ID = ord.ID,
                CustomerInfo = new Contracts.Customer(){ ID = ord.CustomerID},
                EventInfo = new ShowsService.Contracts.Event(){ EventID =ord.EventID},               
                DateOfPurchase = DateTime.Now,
                State = (Contracts.OrderState)Enum.Parse(typeof(Contracts.OrderState), ord.State),
                ReservationID = ord.ReservationID,
                Remarks = ord.Remarks,
                TotalPrice = (double)ord.TotalPrice
            };
        }


        /// <summary>
        /// Retrieve a collection of customer IDs with the largest number of orders. 
        /// </summary>
        /// <param name="numOfCustomers">Numbers of customers to return</param>
        /// <returns></returns>
        public Guid[] GetBestCustomersID(int numOfCustomers)
        {           
            using (TicketingOfficeOrderEntities ctx = new TicketingOfficeOrderEntities())
            {
                var q = from ord in ctx.Orders
                        group ord by ord.CustomerID into g
                        orderby g.Count() descending
                        select new { CustomerID = g.Key };

                var customersArr = q.ToArray();
               
                int length = (numOfCustomers < customersArr.Length)? numOfCustomers : customersArr.Length;

                Guid[] results = new Guid[length];

                for (int i = 0; i < length; i++)
                {
                    results[i] = customersArr[i].CustomerID;
                }

                return results;
            }
        }



      #region IEntityCRUD<Order,Guid> Members

        /// <summary>
        /// Retrieve an Order using its ID
        /// </summary>
        /// <param name="entityID"></param>
        /// <returns></returns>
        public Contracts.Order GetEntity(Guid entityID)
        {
            Contracts.Order res = null;
            try
            {
                using (TicketingOfficeOrderEntities ctx = new TicketingOfficeOrderEntities())
                {
                    var tmp = (from ord in ctx.Orders
                                where ord.ID == entityID
                                select ord).FirstOrDefault();
                    if (tmp != null)
                        res = MapOrder(tmp);
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.RetrieveError, "Order", ex.Message), ex) { EntityType = typeof(Order) };
            }
            return res;
        }

        /// <summary>
        /// Insert a new Order
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Guid CreateEntity(Contracts.Order entity)
        {
            try
            {
                using (TicketingOfficeOrderEntities ctx = new TicketingOfficeOrderEntities())
                {
                    Order newOrder = new Order()
                    {
                        DateOfPurchase = entity.DateOfPurchase,
                        ID = entity.ID,
                        ReservationID  = entity.ReservationID,
                        State = entity.State.ToString(),
                        Remarks = entity.Remarks,
                        TotalPrice = (decimal)entity.TotalPrice,
                        CustomerID = entity.CustomerInfo.ID,
                        EventID = entity.EventInfo.EventID
                    };

                    ctx.Orders.AddObject(newOrder);
                    ctx.SaveChanges();

                    return newOrder.ID;
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.InsertError, "Order", ex.Message), ex) { EntityType = typeof(Order) };
           
            }
        }
     
        /// <summary>
        /// Update an existing Order. If the order does not exist it will be created
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateEntity(Contracts.Order entity)
        {
            try
            {
                using (TicketingOfficeOrderEntities ctx = new TicketingOfficeOrderEntities())
                {
                    var orderToUpdate = ctx.Orders.Where(ord => ord.ID == entity.ID).FirstOrDefault();
                    if (orderToUpdate == null)
                    {
                        CreateEntity(entity);
                        return;
                    }
                    orderToUpdate.DateOfPurchase = entity.DateOfPurchase;
                    orderToUpdate.ReservationID = entity.ReservationID;
                    orderToUpdate.State = entity.State.ToString();
                    orderToUpdate.Remarks = entity.Remarks;
                    orderToUpdate.TotalPrice = (decimal)entity.TotalPrice;

                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.UpdateError, "Order", ex.Message), ex) { EntityType = typeof(Order) };
            }
        }


        /// <summary>
        /// Delete an Order
        /// </summary>
        /// <param name="entityID"></param>
        public void DeleteEntity(Guid entityID)
        {
            try
            {
                using (TicketingOfficeOrderEntities ctx = new TicketingOfficeOrderEntities())
                {
                    var orderToDelete = ctx.Orders.Where(ord => ord.ID == entityID).FirstOrDefault();
                    if (orderToDelete == null)
                        return;

                    ctx.Orders.DeleteObject(orderToDelete);
                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.DeleteError, "Order", ex.Message), ex) { EntityType = typeof(Order) };
            }
        }

        #endregion
    }
}
