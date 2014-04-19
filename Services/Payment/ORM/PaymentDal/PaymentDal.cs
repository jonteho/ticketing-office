using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketingOffice.Common;
using TicketingOffice.Common.Helpers;
using TicketingOffice.Common.Exceptions;
using TicketingOffice.Common.Properties;
using TicketingOffice.PaymentService.Contracts;

namespace TicketingOffice.PaymentService.DataAccess
{
    /// <summary>
    /// The data access layer for managing payments. 
    /// The paymentDal exposes payments as defined in the contracts assembly.
    /// The dal is based on Entity Framework
    /// </summary>
    public class PaymentDal : IEntityCRUD<Contracts.Payment,long>
    {
        /// <summary>
        /// Retrieve a collection of payments according to criteria parameter.
        /// The criteria is used to create the where statement fot the internal EF query. 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public Contracts.Payment[] FindPaymentByCriteria(Contracts.PaymentCriteria criteria)
        {
            
            try
            {
                using (TicketingOfficePaymentEntities ctx = new TicketingOfficePaymentEntities())
                {
                    IQueryable<Payment> query = ctx.Payments;
                    if (criteria.Amount != null)
                        query = query.Where(pay => pay.Amount == criteria.Amount);
                    if (criteria.FromDate != null)
                        query = query.Where(pay => pay.Date >= criteria.FromDate);
                    if (criteria.ToDate != null)
                        query = query.Where(pay => pay.Date <= criteria.ToDate);
                    if (criteria.OrderID != null)
                        query = query.Where(pay => pay.OrderID == criteria.OrderID);
                    if (criteria.PayingCustomerID != null)
                        query = query.Where(pay => pay.CustomerID == criteria.PayingCustomerID);

                   
                    return query.Select(pay => new Contracts.Payment()
                           {
                               ID = pay.ID,
                               Amount = pay.Amount,
                               Date = pay.Date,
                               MethodOfPaymentID = pay.MethodOfPayment,
                               CustomerID = pay.CustomerID,
                               OrderID = pay.OrderID
                           }).ToArray();
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.RetrieveError, "Payment", ex.Message), ex) { EntityType = typeof(Payment) };
            }           

        }

        #region IEntityCRUD<Payment,Guid> Members

        /// <summary>
        /// Retrieve a payment according to its ID
        /// </summary>
        /// <param name="entityID"></param>
        /// <returns></returns>
        public Contracts.Payment GetEntity(long entityID)
        {

            Contracts.Payment res;
            try
            {
                using (TicketingOfficePaymentEntities ctx = new TicketingOfficePaymentEntities())
                {
                    res = (from pay in ctx.Payments
                           where pay.ID == entityID
                           select new Contracts.Payment() 
                           { 
                                ID = pay.ID,
                                Amount = pay.Amount,         
                                Date = pay.Date,
                                MethodOfPaymentID = pay.MethodOfPayment,
                                CustomerID = pay.CustomerID,
                                OrderID = pay.OrderID                           
                           }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.RetrieveError, "Payment", ex.Message), ex) { EntityType = typeof(Payment) };
            }

            return res;
        }

        /// <summary>
        /// Insert a new payment
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public long CreateEntity(Contracts.Payment entity)
        {
            try
            {
                using (TicketingOfficePaymentEntities ctx = new TicketingOfficePaymentEntities())
                {
                    Payment newPayment = new Payment()
                    {
                        Amount = (int)entity.Amount,
                        Date = entity.Date,
                        CustomerID = entity.CustomerID,
                        OrderID = entity.OrderID,
                        MethodOfPayment = (int)entity.MethodOfPayment
                    };

                    ctx.Payments.AddObject(newPayment);
                    ctx.SaveChanges();

                    return newPayment.ID;
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.InsertError, "Payment", ex.Message), ex) { EntityType = typeof(Payment) };
            }
        }

        /// <summary>
        /// Update an existing payment
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateEntity(Contracts.Payment entity)
        {
            try
            {
                using (TicketingOfficePaymentEntities ctx = new TicketingOfficePaymentEntities())
                {
                    var paymrntToUpdate = ctx.Payments.Where(pay => pay.ID == entity.ID).FirstOrDefault();
                    if (paymrntToUpdate == null)
                    {
                        CreateEntity(entity);
                        return;
                    }
                    paymrntToUpdate.Amount = (int)entity.Amount;
                    paymrntToUpdate.Date = entity.Date;
                    paymrntToUpdate.MethodOfPayment = (int)entity.MethodOfPayment;

                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.UpdateError, "Payment", ex.Message), ex) { EntityType = typeof(Payment) };
             }
        }

        /// <summary>
        /// Delete an existing payment
        /// </summary>
        /// <param name="entityID"></param>
        public void DeleteEntity(long entityID)
        {
            try
            {
                using (TicketingOfficePaymentEntities ctx = new TicketingOfficePaymentEntities())
                {
                    var paymentToDelete = ctx.Payments.Where(pay => pay.ID == entityID).FirstOrDefault();
                    if (paymentToDelete == null)
                        return;

                    ctx.Payments.DeleteObject(paymentToDelete);
                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.DeleteError, "Payment", ex.Message), ex) { EntityType = typeof(Payment) };
             }
        }

        #endregion
    }
}
