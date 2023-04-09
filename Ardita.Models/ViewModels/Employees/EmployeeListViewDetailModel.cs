using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Models.ViewModels.Employees
{
    public class EmployeeListViewDetailModel
    {
        public Guid EmployeeId { get; set; }
        public string EmployeeNIK { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeeGender { get; set; }
        public string EmployeePlaceOfBirth { get; set; }
        public DateTime? EmployeeDateOfBirth { get; set; }
        public string EmployeeAddress { get; set; }
        public string EmployeePhone { get; set; }
        public string EmployeeProfilePict { get; set; }
        public string EmployeeLevel { get; set; }
        public Guid PositionId { get; set; }
        public string PositionName { get; set; }
    }
}
