using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketingOffice.CrmService.Contracts;
using TicketingOffice.Common.Helpers;
using TicketingOffice.Common.Properties;
using TicketingOffice.Common.Exceptions;
using TicketingOffice.Common;

namespace TicketingOffice.CrmService.DataAccess
{
    /// <summary>
    /// The data access layer for managing orders. 
    /// The OrderDal exposes orders as defined in the contracts assembly.
    /// The dal is based on Entity Framework
    /// </summary>
    public class CustomerDal : IEntityCRUD<Contracts.Customer,Guid>
    {
        /// <summary>
        ///  Retrieve a collection of customers according to criteria parameter.
        /// The criteria is used to create the where statement fot the internal EF query.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public Contracts.Customer[] GetCustomersByCriteria(CustomerCriteria criteria)
        {
            Contracts.Customer[] result;
            try
            {
                using (TicketingOfficeCrmEntities ctx = new TicketingOfficeCrmEntities())
                {
                    IQueryable<Customer> query;
                    query = ctx.Customers;

                    if (criteria.BirthDate != null)
                        query = query.Where(cust => cust.Birthdate == criteria.BirthDate);
                    if (criteria.ReductionCode != null)
                        query = query.Where(cust => cust.ReductionCode == criteria.ReductionCode);
                    if (!string.IsNullOrEmpty(criteria.CellNumber))
                        query = query.Where(cust => cust.CellNumber == criteria.CellNumber);
                    if (!string.IsNullOrEmpty(criteria.Country))
                        query = query.Where(cust => cust.Country == criteria.Country);
                    if (!string.IsNullOrEmpty(criteria.City))
                        query = query.Where(cust => cust.City == criteria.City);
                    if (!string.IsNullOrEmpty(criteria.Email))
                        query = query.Where(cust => cust.Email == criteria.Email);
                    if (!string.IsNullOrEmpty(criteria.PhoneNumber))
                        query = query.Where(cust => cust.PhoneNumber == criteria.PhoneNumber);

                    var tmp  = query.ToArray();

                    result = tmp.Select(cust => MapCustomerObject(cust)).ToArray();                   
                }

            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.RetrieveError, "Customer", ex.Message), ex) { EntityType = typeof(Customer) };
            }

            return result;
        }

        /// <summary>
        /// Helper function. Execute object to object mapping between ORM objects (EF) to contracts business entities
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private Contracts.Customer MapCustomerObject(Customer entity)
        {
            Contracts.Customer newCustomer = new Contracts.Customer()
            {
                ID = entity.ID,
                Address = entity.Address,
                BirthDate = entity.Birthdate,
                CellNumber = entity.CellNumber,
                Country = entity.Country,
                Email = entity.Email,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                PhoneNumber = entity.PhoneNumber,
                City = entity.City,
                ReductionCode = entity.ReductionCode
            };

            return newCustomer;
        }      


        #region IEntityCRUD<Customer,Guid> Members       

        /// <summary>
        /// Retrieve a customer according to its ID
        /// </summary>
        /// <param name="entityID"></param>
        /// <returns></returns>
        public Contracts.Customer GetEntity(Guid entityID)
        {
            Contracts.Customer res = null;
            try
            {
                using (TicketingOfficeCrmEntities ctx = new TicketingOfficeCrmEntities())
                {
                    var tmp = (from cust in ctx.Customers  
                           where cust.ID == entityID
                           select cust).FirstOrDefault();

                    //Object to Object mapping between the EF customer to the customer defined in contracts
                    if (tmp != null) 
                        res = MapCustomerObject(tmp);
                }

            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.RetrieveError, "Customer", ex.Message), ex) { EntityType = typeof(Customer) };
            }

            return res;
        }

        /// <summary>
        /// Insert a new customer
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Guid CreateEntity(Contracts.Customer entity)
        {
            try
            {
                using (TicketingOfficeCrmEntities ctx = new TicketingOfficeCrmEntities())
                {
                    Customer newCustomer = new Customer()
                    {
                         ID = entity.ID,
                         Address = entity.Address,
                         Birthdate = entity.BirthDate,
                         CellNumber = entity.CellNumber,
                         Country = entity.Country,
                         Email = entity.Email,
                         FirstName = entity.FirstName,
                         LastName = entity.LastName,
                         PhoneNumber = entity.PhoneNumber,
                         City = entity.City,
                         ReductionCode = entity.ReductionCode
                    };

                    ctx.Customers.AddObject(newCustomer);
                    ctx.SaveChanges();

                    return newCustomer.ID;
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.InsertError, "Customer", ex.Message), ex) { EntityType = typeof(Customer) };
            }
        }

        /// <summary>
        /// Update a customer
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateEntity(Contracts.Customer entity)
        {
            try
            {
                using (TicketingOfficeCrmEntities ctx = new TicketingOfficeCrmEntities())
                {
                    var customerToUpdate = ctx.Customers.Where(ev => ev.ID == entity.ID).FirstOrDefault();
                    if (customerToUpdate == null)
                    {
                        CreateEntity(entity);
                        return;
                    }
                    customerToUpdate.Address = entity.Address;
                    customerToUpdate.Birthdate = entity.BirthDate;
                    customerToUpdate.CellNumber = entity.CellNumber;
                    customerToUpdate.City = entity.City;
                    customerToUpdate.Country = entity.Country;
                    customerToUpdate.Email = entity.Email;
                    customerToUpdate.FirstName = entity.FirstName;
                    customerToUpdate.LastName = entity.LastName;
                    customerToUpdate.PhoneNumber = entity.PhoneNumber;
                    customerToUpdate.ReductionCode = entity.ReductionCode;
                    
                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.UpdateError, "Customer", ex.Message), ex) { EntityType = typeof(Customer) };
            }
        }

        /// <summary>
        /// Delete a customer
        /// </summary>
        /// <param name="entityID"></param>
        public void DeleteEntity(Guid entityID)
        {
            try
            {
                using (TicketingOfficeCrmEntities ctx = new TicketingOfficeCrmEntities())
                {
                    var customerToDelete = ctx.Customers.Where(ev => ev.ID == entityID).FirstOrDefault();
                    if (customerToDelete == null)
                        return;
                    ctx.Customers.DeleteObject(customerToDelete);
                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.DeleteError, "Customer", ex.Message), ex) { EntityType = typeof(Customer) };
            }
        }

        #endregion
    }
}
