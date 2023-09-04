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
    public partial class ReportedIssue
    {
        public ReportedIssue()
        {
            WorkLists = new HashSet<WorkList>();
        }

        public int IssueId { get; set; }
        public int ZipCode { get; set; }
        public string Address { get; set; } = null!;
        public DateTime? ReportedDateTime { get; set; }
        public bool? IsFixed { get; set; }

        public virtual ICollection<WorkList> WorkLists { get; set; }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string? ToString()
        {
            return $"{IssueId}, {ZipCode}, {Address}, {ReportedDateTime}, {IsFixed}";
        }
    }
}
