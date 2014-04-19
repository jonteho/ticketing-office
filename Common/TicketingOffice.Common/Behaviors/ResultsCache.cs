using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TicketingOffice.Common.Helpers
{
    public class ResultsCache
    {
        protected Dictionary<Guid, ResultPackage> Results { get; set; }

        public ResultsCache()
        {
            Results = new Dictionary<Guid, ResultPackage>();
        }


        public void SaveResult(Guid Id, ResultPackage result)
        {
            Results.Add(Id, result);
        }

        public object GetResult(Guid id)
        {
            if (Results.ContainsKey(id))
                return Results[id].Result;

            return null;
        }

        public ResultPackage GetResultPack(Guid id)
        {
            if (Results.ContainsKey(id))
                return Results[id];

            return null;
        }

        public void SetPackage(Guid id, ResultPackage package)
        {
            Results[id] = package;
        }

        public void ClearResult(Guid id)
        {
            if (Results.ContainsKey(id))
                Results[id] = null;
        }

        public static ResultsCache Current
        {  
            get
            {
                if (_current == null)
                {
                    lock (typeof(ResultsCache))
                    {
                        if (_current == null)
                        {
                            _current = new ResultsCache();
                        }
                    }
                }
                return _current;
            }
        
        }

        private static ResultsCache _current;

    }

    public class ResultPackage
    {
        public AutoResetEvent ResultArrived  { get; set; }
        public object Result { get; set; }        
    }



}
