using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TicketingOffice.ShowsService.BusinessLogic
{
    public interface IShowsManager
    {
        Contracts.Show[] FindShows(Contracts.ShowCriteria showCriteria);
        Contracts.Show FindShow(int showID);
        int CreateShow(Contracts.Show showToCreate);
        void UpdateShow(Contracts.Show showToUpdate);
        void DeleteShow(int showId);
    }
}
