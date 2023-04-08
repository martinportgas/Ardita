using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Models.ViewModels.Pages
{
    public class MenuWithSubMenu
    {
        public Guid MenuId { get; set; }
        public Guid SubMenuId { get; set; }
    }
    public class SubMenuTypes
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("MenuTypes")]
        public Guid MenuId { get; set; }
        public MenuTypes menuTypes { get; set; }
    }
    public class MenuTypes
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<SubMenuTypes> SubMenuTypes { get; set; }
    }
}
