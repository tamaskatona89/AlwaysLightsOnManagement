using System;
using System.Collections.Generic;

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
