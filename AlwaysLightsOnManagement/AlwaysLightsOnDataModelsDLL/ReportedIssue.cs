﻿using System;
using System.Collections.Generic;

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
    }
}