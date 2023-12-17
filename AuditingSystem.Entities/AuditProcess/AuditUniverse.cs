using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AuditingSystem.Entities.AuditProcess
{
    public class AuditUniverse:Base
    {
        public int IndustryId { get; set; }
        public int CompanyId { get; set; }
        public int DepartmentId { get; set; }
        public int FunctionId { get; set; }
        public int ActivityId { get; set; }
        public int ObjectiveId { get; set; }
        public int TaskId { get; set; }
        public int PracticeId { get; set; }

        public virtual Industry? Industry { get; set; }
        public virtual Company? Company { get; set; }
        public virtual Department? Department { get; set; }
        public virtual Function? Function { get; set; }
        public virtual Activity? Activity { get; set; }
        public virtual Objective? Objective { get; set; }
        public virtual Tasks? Task { get; set; }
        public virtual Practice? Practice { get; set; }

        //public AuditUniverse(string name, string description,int industryId, int companyId, 
        //                    int departmentId, int functionId, int activityId, int objectiveId,
        //                    int taskId, int practiceId) : base(name, description)
        //{
        //    this.IndustryId = industryId;
        //    this.CompanyId = companyId;
        //    this.DepartmentId = departmentId;
        //    this.FunctionId = functionId;
        //    this.ActivityId = activityId;
        //    this.ObjectiveId = objectiveId;
        //    this.TaskId = taskId;
        //    this.PracticeId = practiceId;
        //}
    }
}
