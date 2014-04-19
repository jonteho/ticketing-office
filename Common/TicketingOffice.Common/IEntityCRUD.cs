using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicketingOffice.Common
{
    /// <summary>
    /// Implement CRUD for business entities
    /// </summary>
    /// <typeparam name="T">The type of the business entity</typeparam>
    /// <typeparam name="U">The type of the ID</typeparam>
    public interface IEntityCRUD<T,U>
    {
        /// <summary>
        /// Retrieves business entities
        /// </summary>       
        /// <param name="entityID">The entity id</param>
        /// <returns>The business entity</returns>
        T GetEntity(U entityID);
        /// <summary>
        /// Insert a new entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The entityID</returns>
        U CreateEntity(T entity);
        /// <summary>
        /// Update an existing entity
        /// </summary>
        /// <param name="entity"></param>
        void UpdateEntity(T entity);
        /// <summary>
        /// Delete an existing entity
        /// </summary>
        /// <param name="entityID">The entity id</param>
        void DeleteEntity(U entityID);
    }
}
