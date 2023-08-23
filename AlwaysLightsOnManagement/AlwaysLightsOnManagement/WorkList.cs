using System;
/**
* author: KATONA TAMAS - katonatomi@msn.com
* 2023.
*/
namespace AlwaysLightsOnManagement
{
    public  class WorkList
    {
        public int WorkListID { get; set; }
        public int ReportedIssues_IssueID { get; set; }
        public int WorkTypes_WorkTypeID { get; set; }
        public int Worker_WorkerID { get; set; }
        public DateTime Fixing_DateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// Creates a WorkList Entry
        /// </summary>
        /// <param name="reportedIssues_IssueID">reportedIssues_IssueID</param>
        /// <param name="workTypes_WorkTypeID">workTypes_WorkTypeID</param>
        /// <param name="worker_WorkerID">worker_WorkerID</param>
        public WorkList(int reportedIssues_IssueID, int workTypes_WorkTypeID, int worker_WorkerID)
        {
            ReportedIssues_IssueID = reportedIssues_IssueID;
            WorkTypes_WorkTypeID = workTypes_WorkTypeID;
            Worker_WorkerID = worker_WorkerID;
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
