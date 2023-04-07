using Ardita.Models.DbModels;

namespace Ardita.Areas.Employee.Models
{
    public class EmployeeInserViewModel
    {
        public IEnumerable<MstPosition> Positions { get; set; }
        public MstEmployee Employee { get; set; }
    } 
}
