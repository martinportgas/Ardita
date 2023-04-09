using Ardita.Models.DbModels;
using Ardita.Models.ViewModels.Pages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ardita.Areas.UserManage.Models
{
    public class PageInsertViewModel
    {
        public MstPage page { get; set; }
        public IEnumerable<SelectListItem> MenuTypes { get; set; }
        public IEnumerable<SelectListItem> subMenuTypes { get; set; }
    }
  

}
