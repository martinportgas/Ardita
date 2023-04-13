using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Models.ViewModels.Pages
{
    public class PageListViewDetailModel
    {
        public Guid PageId { get; set; }
        public string PageName { get; set; }
        public string PagePath { get; set; }
        public bool PageIsActive { get; set; }
        public Guid SubMenuId { get; set; }
        public string SubMenuName { get; set; }
        public string SubMenuPath { get; set; }
        public Guid MenuId { get; set; }
        public string MenuName { get; set; }
        public string MenuPath { get; set; }
        public bool SubmIsActive { get; set; }
    }
}
