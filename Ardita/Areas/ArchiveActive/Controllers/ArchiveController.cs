using Ardita.Controllers;
using Ardita.Globals;
using Ardita.Models.DbModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.ArchiveActive.Controllers;

[CustomAuthorize]
[Area(Const.ArchiveActive)]
public class ArchiveController : BaseController<TrxArchive>
{
    public ArchiveController(
        IArchiveService archiveService, 
        IGmdService gmdService,
        IClassificationSubSubjectService classificationSubSubjectService,
        ISecurityClassificationService securityClassificationService,
        IArchiveCreatorService archiveCreatorService)
    {
        _archiveService = archiveService;
        _gmdService = gmdService;
        _classificationSubSubjectService = classificationSubSubjectService;
        _securityClassificationService = securityClassificationService;
        _archiveCreatorService = archiveCreatorService;
    }

    public override async Task<ActionResult> Index() => await base.Index();

    public override async Task<IActionResult> Add()
    {
        ViewBag.listGmd = await BindGmds();
        //ViewBag.listSubSubjectClasscification = await BindSubSubjectClasscifications();
        ViewBag.listSecurityClassification = await BindSecurityClassifications();
        ViewBag.listArchiveCreator = await BindArchiveCreators();

        return View(Const.Form, new TrxArchive());
    }
}
