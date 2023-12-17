using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Entities
{
    public abstract class Base
    {
        public int Id { get; set; }

         
        public string Name { get; set; }


        public string Description { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string CreatedBy { get; set; } = "Admin";
        public DateTime CreationDate { get; set; }
        public string UpdatedBy { get; set; } = "Admin";
        public DateTime UpdatedDate { get; set; }

        //public Base(string name, string description)
        //{
        //    this.Name = name;
        //    this.Description = description;
        //}
    }
}
