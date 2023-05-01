using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Globals;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Classess;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.ArchiveActive.Controllers
{
    [CustomAuthorize]
    [Area(Const.ArchiveActive)]
    public class ArchiveApprovalController : BaseController<VwArchiveApproval>
    {
        #region MEMBER AND CTR
        public ArchiveApprovalController(
            IArchiveApprovalService archiveApprovalService)
        {
            _archiveApprovalService = archiveApprovalService;
        }
        #endregion
        #region MAIN ACTION
        public override async Task<ActionResult> Index() => await base.Index();
        public override async Task<JsonResult> GetData(DataTablePostModel model)
        {
            try
            {
                model.EmployeeId = AppUsers.CurrentUser(User).EmployeeId;
                var result = await _archiveApprovalService.GetList(model);

                return Json(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
        #region HELPER
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.ArchiveApproval, new { Area = Const.ArchiveActive });
        #endregion
    }
}
