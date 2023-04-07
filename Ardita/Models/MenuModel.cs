using Ardita.Models.DbModels;

namespace Ardita.Models
{
    public class MenuModel
    {
        public IEnumerable<MstMenu> menu { get; set; }
        public IEnumerable<MstSubmenu> subMenu { get; set; }
    }
}
