using AlwaysLightsOnManagement;

namespace ConsoleApp2
{
    public class Program
    {
        public static void Main()
        {
            DBServices dBServices = new();

            //TEST
            //INSERT INTO WorkList(Issue_ID,WorkType_ID,Worker_ID) VALUES(1,2,2);
            //dBServices.AddFinishedWorkToWorkList(1, 2, 2);
            //Console.WriteLine("Finished!\nINSERT INTO WorkList(Issue_ID,WorkType_ID,Worker_ID) VALUES(1,2,2);");

            //List<ReportedIssue>? reportedIssues3333 = dBServices.GetReportedIssuesByZIPCode(3333);
            //List<ReportedIssue>? reportedIssuesBP_VI_District = dBServices.GetBudapestReportedIssuesByDistrict(106);

            List<ReportedIssue>? issuesOlderThan_2 = dBServices.GetReportedIssuesOlderThan(2);





        }
    }
}