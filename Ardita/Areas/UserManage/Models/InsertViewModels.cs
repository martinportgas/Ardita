using Ardita.Models.DbModels;
using dbModel = Ardita.Models.DbModels;

namespace Ardita.Areas.UserManage.Models
{
    public class InsertViewModels
    {
        public IEnumerable<MstEmployee> Employees { get; set; }
        public MstUser User { get; set; }
    }
}
