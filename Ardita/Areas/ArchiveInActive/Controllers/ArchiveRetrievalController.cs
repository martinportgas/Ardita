using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Classess;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.ArchiveInActive.Controllers
{
    [CustomAuthorize]
    [Area(GlobalConst.ArchiveInActive)]
    public class ArchiveRetrievalController : BaseController<TrxArchiveRent>
    {
        public ArchiveRetrievalController(
                IArchiveRentService archiveRentService,
            IClassificationSubSubjectService classificationSubSubjectService,
            IArchiveService archiveService,
            IMediaStorageInActiveService mediaStorageInActiveService
            )
        {
            _archiveRentService = archiveRentService;
            _classificationSubSubjectService = classificationSubSubjectService;
            _archiveService = archiveService;
            MediaStorageInActiveService = mediaStorageInActiveService;
        }
        public override async Task<ActionResult> Index() => await base.Index();
        public override async Task<JsonResult> GetData(DataTablePostModel model)
        {
            try
            {
                var result = await _archiveRentService.GetRetrievalList(model);
                return Json(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.ArchiveRetrieval, new { Area = GlobalConst.ArchiveInActive });
    }
}
