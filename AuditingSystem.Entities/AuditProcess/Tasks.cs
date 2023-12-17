using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Entities.AuditProcess
{
    public class Tasks:Base
    {
        public int ObjectiveId { get; set; }
        public virtual Objective? Objective { get; set; }
        public virtual IEnumerable<Practice>? Practices { get; set; }

        //public Tasks(string name, string description, int objectiveId) : base(name, description)
        //{
        //    this.ObjectiveId = objectiveId;
        //}
    }
}
