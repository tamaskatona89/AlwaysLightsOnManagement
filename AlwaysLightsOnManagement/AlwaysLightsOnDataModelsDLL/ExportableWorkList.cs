using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlwaysLightsOnDataModelsDLL
{
    public class ExportableWorkList
    {
        public ExportableWorkList(int workListID, string reportedIssue, string workType, string worker, DateTime fixingDateTime)
        {
            WorkListID = workListID;
            ReportedIssue = reportedIssue;
            WorkType = workType;
            Worker = worker;
            FixingDateTime = fixingDateTime;
        }

        public int WorkListID { get; set; }
        public string ReportedIssue { get; set; }
        public string WorkType { get; set; }
        public string Worker{ get; set; }
        public DateTime FixingDateTime { get; set; }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
