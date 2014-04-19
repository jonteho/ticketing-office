using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;


namespace TicketingOffice.HallState.Contracts
{
    /// <summary>
    /// The contract of the HallStateService
    /// </summary>
    [ServiceContract(Namespace = "http://Fabrikam.com")] 
    public interface IHallStateService
    {
        /// <summary>
        /// Get the current state of a hall
        /// </summary>
        /// <param name="eventID"></param>
        /// <returns>A list of occupied seats</returns>
        [OperationContract]
        SeatIndex[] GetHallState(Guid eventID);       

        /// <summary>
        /// Find a theater according to its name or ID
        /// </summary>
        /// <param name="name"></param>
        /// <param name="theaterID"></param>
        /// <returns>Theater entity</returns>
        [OperationContract]
        Theater FindTheater(string name, int? theaterID); 

    }


      

    /// <summary>
    /// The contract of the Halls Management service
    /// </summary>
    [ServiceContract(Namespace = "http://Fabrikam.com")]
    public interface IHallManagementService
    {
        [OperationContract]
        int CreateTheater(Contracts.Theater newTheater);

        [OperationContract]
        void DeleteTheater(int theaterID);

        [OperationContract]
        void UpdateTheater(Contracts.Theater newTheater);

    }


    /// <summary>
    /// The contract of the reservation service
    /// </summary>
    [ServiceContract(Namespace = "http://Fabrikam.com")]
    public interface IReservationService
    {    
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        Guid CreateResevation(Reservation reservation);
        
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        void DeleteResrevation(Guid ID);
       
        [OperationContract]   
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        void UpdateReservation(Reservation newReservation);
        
        [OperationContract]
        Reservation FindReservation(string showName, DateTime date, int? HallID, Guid CustomerID);
        
        [OperationContract]
        Reservation[] FindReservations(ReservationCriteria criteria);              
           
    }
}
