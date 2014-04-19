using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using TicketingOffice.Common.Properties;
using TicketingOffice.Common.Helpers;

namespace TicketingOffice.HallState.Contracts
{
    [DataContract(Namespace = @"http://Fabrikam.com")]
    public class SeatIndex : IComparable
    {
        [DataMember]
        public short? Row { get; set; }
        [DataMember]
        public short? Seat { get; set; }


        /// <summary>
        /// Create a string represention of a list of seats. 
        /// </summary>
        /// <param name="seatIndex"></param>
        /// <returns></returns>
        public static string CreateSeatsString(List<SeatIndex> seatIndex)
        {
            StringBuilder result = new StringBuilder();
            foreach (var item in seatIndex)
            {
                result.Append(item.Row);
                result.Append(',');
                result.Append(item.Seat);
                result.Append(',');
            }
            result.Remove(result.Length - 1, 1); // remove the last char (i.e ',')
            return result.ToString();
        }


        /// <summary>
        /// Parse a string to a list of seats
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static List<SeatIndex> ParseSeats(string inputString)
        {
            string[] info = inputString.Split(',');
            if ((info.Length % 2) != 0)
                throw new SeatIndexException(StringsResource.InvalidSeatsString);
            List<SeatIndex> result = new List<SeatIndex>();
            try
            {
                for (int i = 0; i < info.Length ; i++)                
                {                   
                    result.Add(new SeatIndex() { Row = short.Parse(info[i++]), Seat = short.Parse(info[i]) });
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new SeatIndexException(StringsResource.SeatParsingError, ex);
            }

            return result.ToList();
        }

        /// <summary>
        /// Compare two seats collections. 
        /// If there is one or more seat included in both collection the result is true.
        /// </summary>
        /// <param name="group1"></param>
        /// <param name="group2"></param>
        /// <returns></returns>
        public static bool CompareSeates(SeatIndex[] group1, SeatIndex[] group2)
        {
            if (group1 == null || group1.Length == 0)
            {
                if (group2 == null || group2.Length == 0)
                    return true;
                else
                    return false;                
            }

            if (group2 == null || group2.Length == 0)
            {
                if (group1 == null || group1.Length == 0)
                    return true;
                else
                    return false;
            }

            if (group1.Length != group2.Length)
                return false;

          
            foreach (var item in group1)
            {
                if (!group2.Contains(item))
                    return false;
            }

            return true;
        }
        
        /// <summary>
        /// Overriding Equals to compare two seats
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is SeatIndex))
                return false;
            SeatIndex other = (obj as SeatIndex);
            return ((other.Row == this.Row) && (other.Seat == this.Seat));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #region IComparable Members

         public int CompareTo(object obj)
        {
            if (!(obj is SeatIndex))
                throw new ArrayTypeMismatchException();

            SeatIndex other = (obj as SeatIndex);
            if ((other.Seat == this.Seat) && (other.Row == this.Row))
                return 0;

            if (other.Row > this.Row)
                return 1;

            if (other.Row < this.Row)
                return -1;

            if (other.Seat > this.Seat)
                return 1;

            return -1;

        }

        #endregion
    }

    public class SeatIndexException : Exception
    {
        public SeatIndexException() : base() { }

        public SeatIndexException(string message) : base(message) { }

        public SeatIndexException(string message, Exception inner) : base(message, inner) { }

    }
}
