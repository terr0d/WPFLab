using System;
using System.Collections.Generic;

namespace AdaptationManagement
{
    public class AdaptationModule
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public List<string> Developers { get; set; }
        public List<string> Approvers { get; set; }
        public string MainApprover { get; set; }
        public string Position { get; set; }
        public DateTime DeadlineDate { get; set; }

        public AdaptationModule()
        {
            Developers = new List<string>();
            Approvers = new List<string>();
        }
    }
}