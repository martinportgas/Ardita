﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Models.ViewModels.Users
{
    public class GetLoginModel
    {
        public string? Username { get; set; }
        public Guid? UserId { get; set; }
        public Guid? RoleId { get; set; }
        public string? RoleCode { get; set; }
        public string? RoleName { get; set; }
        public string? EmployeeNIK { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeMail { get; set; }
        public string? EmployeePhone { get; set; }
        public Guid? PositionId { get; set; }
        public Guid? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public Guid? EmployeeId { get; set; }
        public string? ArchiveUnitId { get; set; }
        public string? ArchiveUnitName { get; set; }
        public string? CreatorId { get; set; }
        public string? CreatorName { get; set; }
    }
}