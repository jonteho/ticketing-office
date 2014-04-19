using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketingOffice.ShowsService.DataAccess;

namespace TicketingOffice.ShowsService.BusinessLogic
{
    /// <summary>
    /// Execute show related logic. In this simple example there is only call to data access logic.
    /// </summary>
    public class ShowManager : IShowsManager
    {
        #region IShowsManager Members

        public Contracts.Show[] FindShows(Contracts.ShowCriteria showCriteria)
        {
            ShowDal dal = new ShowDal();
            return dal.GetShowsByCriteria(showCriteria);
        }

        public void CreateShow(Contracts.Show showToCreate)
        {
            ShowDal dal = new ShowDal();
            dal.CreateEntity(showToCreate);
        }

        public void DeleteShow(int showId)
        {
            ShowDal dal = new ShowDal();
            dal.DeleteEntity(showId);
        }

        public Contracts.Show FindShow(int showID)
        {
            ShowDal dal = new ShowDal();
            return dal.GetEntity(showID);
        }

        int IShowsManager.CreateShow(Contracts.Show showToCreate)
        {
            ShowDal dal = new ShowDal();
            return dal.CreateEntity(showToCreate);
        }

        public void UpdateShow(Contracts.Show showToUpdate)
        {
            ShowDal dal = new ShowDal();
            dal.UpdateEntity(showToUpdate);
        }

        #endregion
    }
}
