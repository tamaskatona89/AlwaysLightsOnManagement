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
    public partial class WorkType
    {
        public WorkType()
        {
            WorkLists = new HashSet<WorkList>();
        }

        public int WorkTypeId { get; set; }
        public string WorkTypeDescription { get; set; } = null!;

        public virtual ICollection<WorkList> WorkLists { get; set; }
    }
}
