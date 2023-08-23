using System;
/**
* author: KATONA TAMAS - katonatomi@msn.com
* 2023.
*/
namespace AlwaysLightsOnManagement
{
    public class ReportedIssues
    {
        public int Issue_ID { get; set; }
        public int ZIP_CODE { get; set; }
        public string Address { get; set; }
        public DateTime Reported_DateTime { get; set; } = DateTime.Now;
        public bool Is_Fixed { get; set; }

        /// <summary>
        /// Create 1 Issue
        /// </summary>
        /// <param name="zipCode">zipCode</param>
        /// <param name="address">address</param>
        public ReportedIssues(int zipCode, string address)
        {
            ZIP_CODE = zipCode;
            Address = address;
            Reported_DateTime = DateTime.Now;
            Is_Fixed = false;
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
