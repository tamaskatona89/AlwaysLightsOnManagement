using System;
/**
* author: KATONA TAMAS - katonatomi@msn.com
* 2023.
*/
namespace AlwaysLightsOnManagement
{
    public class ReportedIssues
    {
        public int IssueID { get; set; }
        public int ZIPCODE { get; set; }
        public string Address { get; set; }
        public DateTime ReportedDateTime { get; set; } = DateTime.Now;
        public bool IsFixed { get; set; }

        /// <summary>
        /// Create 1 Issue
        /// </summary>
        /// <param name="zipCode">zipCode</param>
        /// <param name="address">address</param>
        public ReportedIssues(int zipCode, string address)
        {
            ZIPCODE = zipCode;
            Address = address;
            ReportedDateTime = DateTime.Now;
            IsFixed = false;
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
