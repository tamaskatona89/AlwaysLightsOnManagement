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
    public partial class Worker
    {
        public Worker()
        {
            WorkLists = new HashSet<WorkList>();
        }

        public int WorkerId { get; set; }
        public string FullName { get; set; } = null!;

        public virtual ICollection<WorkList> WorkLists { get; set; }

        public override string? ToString()
        {
            return $"{WorkerId}, {FullName}";
        }
    }
}
