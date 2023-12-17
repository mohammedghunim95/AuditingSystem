﻿using AuditingSystem.Entities.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Entities.AuditProcess
{
    public class Company : Base
    {
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }

        public int IndustryId { get; set; }
        public virtual Industry? Industry { get; set; }

        public virtual IEnumerable<User>? User { get; set; }
        public virtual IEnumerable<Department>? Departments { get; set; }

        //public Company(string name, string description, string address, string contactNo, string email, int industryId) : base(name, description)
        //{
        //    this.Address = address;
        //    this.ContactNo = contactNo; // تم تصحيح الاسم هنا
        //    this.Email = email;
        //    this.IndustryId = industryId;
        //}
    }
}
