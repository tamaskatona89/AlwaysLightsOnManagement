using System;
/**
* author: KATONA TAMAS - katonatomi@msn.com
* 2023.
*/
namespace AlwaysLightsOnManagement
{
    public class WorkTypes
    {
        public int WorkTypeID { get; set; }
        public string WorkTypeDescription { get; set; }

        /// <summary>
        /// Creates 1 WorkType
        /// </summary>
        /// <param name="workTypeDescription">Type of work description</param>
        public WorkTypes(string workTypeDescription)
        {
            WorkTypeDescription = workTypeDescription;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
