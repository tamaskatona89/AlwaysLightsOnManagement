using System;
using System.Collections.Generic;

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
