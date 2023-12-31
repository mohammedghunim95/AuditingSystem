﻿using AuditingSystem.Entities.AuditProcess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Entities.Setup
{
    public class Year : Base
    {
        [Required(ErrorMessage = "The Company field is required")]
        public int CompanyId { get; set; }
        public virtual Company? Company { get; set; }

        [Required(ErrorMessage = "The Department field is required")]
        public int DepartmentId { get; set; }
        public virtual Department? Department { get; set; }

        //public Year(string name, string description, int companyId, int departmentId) : base(name, description)
        //{
        //    this.CompanyId = companyId;
        //    this.DepartmentId = departmentId;
        //}
    }
}
