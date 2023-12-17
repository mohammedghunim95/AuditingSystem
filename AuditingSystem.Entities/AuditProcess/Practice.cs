using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Entities.AuditProcess
{
    public class Practice:Base
    {
        public int TaskId { get; set; }
        public virtual Tasks? Task { get; set; }

        //public Practice(string name, string description, int taskId) : base(name, description)
        //{
        //    this.TaskId = taskId;
        //}
    }
}
