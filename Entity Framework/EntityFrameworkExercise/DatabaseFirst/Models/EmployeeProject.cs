﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftUni.Models
{
    public partial class EmployeeProject
    {
        [Column("EmployeeID")]
        public int EmployeeId { get; set; }
        [Column("ProjectID")]
        public int ProjectId { get; set; }

        [ForeignKey("EmployeeId")]
        [InverseProperty("EmployeesProjects")]
        public virtual Employee Employee { get; set; }
        [ForeignKey("ProjectId")]
        [InverseProperty("EmployeesProjects")]
        public virtual Project Project { get; set; }
    }
}
