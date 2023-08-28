using System;
using System.Collections.Generic;

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
