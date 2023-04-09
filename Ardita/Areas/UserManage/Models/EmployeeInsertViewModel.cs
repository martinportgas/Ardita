using Ardita.Models.DbModels;

namespace Ardita.Areas.UserManage.Models
{
    public class EmployeeInserViewModel
    {
        public IEnumerable<MstPosition> Positions { get; set; }
        public MstEmployee Employee { get; set; }
    } 
}
