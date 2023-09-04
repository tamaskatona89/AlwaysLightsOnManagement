/**
 *  Author:           Katona Tamás
 *  E-mail:           katonatomi@msn.com
 *  Course:           CUBIX - C# és .NET fejlesztés alapok, 2023.June - Sept
 *  Project Name:     MINDIG FÉNYES KFT, Company's Working and Issue Management Software
 *  Project Github:   https://github.com/tamaskatona89/AlwaysLightsOnManagement
 *  Project Duration: 2023.08.23....2023.09.06
 */
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
