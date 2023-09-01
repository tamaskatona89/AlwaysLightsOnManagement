using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AlwaysLightsOnDataModelsDLL
{
    [Serializable]
    public class ExportableWorkList
    {
        public ExportableWorkList() 
        {
            
        }
        public ExportableWorkList(int workListID, string reportedIssue, string workType, string worker, DateTime fixingDateTime)
        {
            WorkListID = workListID;
            ReportedIssue = reportedIssue;
            WorkType = workType;
            Worker = worker;
            FixingDateTime = fixingDateTime;
        }

        [XmlElement("WorkListID")]
        public int WorkListID { get; set; }

        [XmlElement("ReportedIssue")]
        public string ReportedIssue { get; set; }

        [XmlElement("WorkType")]
        public string WorkType { get; set; }

        [XmlElement("Worker")]
        public string Worker{ get; set; }

        [XmlElement("FixingDateTime")]
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
