/**
 *  Author:           Katona Tamás
 *  E-mail:           katonatomi@msn.com
 *  Course:           CUBIX - C# és .NET fejlesztés alapok, 2023.June - Sept
 *  Project Name:     MINDIG FÉNYES KFT, Company's Working and Issue Management Software
 *  Project Github:   https://github.com/tamaskatona89/AlwaysLightsOnManagement
 *  Project Duration: 2023.08.23....2023.09.06
 */
namespace AlwaysLightsOnManagement
{
    public partial class WorkList
    {
        public int WorkListId { get; set; }
        public int IssueId { get; set; }
        public int WorkTypeId { get; set; }
        public int WorkerId { get; set; }
        public DateTime? FixingDateTime { get; set; }

        public virtual ReportedIssue Issue { get; set; } = null!;
        public virtual WorkType WorkType { get; set; } = null!;
        public virtual Worker Worker { get; set; } = null!;
    }
}
