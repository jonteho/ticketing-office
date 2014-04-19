using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketingOffice.HallState.Contracts;

namespace TicketingOffice.HallState.BusinessLogic
{
    public interface IHallStateManager
    {
        SeatIndex[] GetHallState(Guid eventID);
        IAsyncResult BeginGetHallState(Guid eventID, AsyncCallback cb, object state);
        SeatIndex[] EndGetHallState(IAsyncResult ar);
        Guid CreateResrvation(Contracts.Reservation newReservation);
        void DeleteResrvation(Guid resrevationID);
        void UpdateResrvation(Contracts.Reservation newReservation);
        Contracts.Reservation[] FindReservations(ReservationCriteria criteria);       
        Theater FindTheater(string name);
        IAsyncResult BeginFindTheater(string name, AsyncCallback cb, object state);
        Theater EndFindTheater(IAsyncResult ar);
        Theater FindTheater(int ID);
        int CreateTheater(Contracts.Theater newTheater);
        void UpdateTheater(Contracts.Theater newTheater);
        void DeleteTheater(int TheaterID);
    }
}
