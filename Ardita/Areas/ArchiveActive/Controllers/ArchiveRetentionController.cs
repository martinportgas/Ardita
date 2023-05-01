using Ardita.Controllers;
using Ardita.Globals;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Ardita.Areas.ArchiveActive.Controllers
{
    [CustomAuthorize]
    [Area(Const.ArchiveActive)]
    public class ArchiveRetentionController : BaseController<TrxArchive>
    {
        #region MEMBER AND CTR
        private readonly IArchiveRetentionService _archiveRetentionService;
        public ArchiveRetentionController(
            IArchiveRetentionService archiveRetentionService)
        {
            _archiveRetentionService = archiveRetentionService;
        }
        #endregion

        #region MAIN ACTION
        public override async Task<ActionResult> Index() => await base.Index();
        public override async Task<JsonResult> GetData(DataTablePostModel model)
        {
            try
            {
                var result = await _archiveRetentionService.GetList(model);

                return Json(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}
