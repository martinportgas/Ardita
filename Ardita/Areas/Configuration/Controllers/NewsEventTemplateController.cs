using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Ardita.Areas.Configuration.Controllers
{
    [CustomAuthorize]
    [Area(GlobalConst.Configuration)]
    public class NewsEventTemplateController : BaseController<MstTemplateSetting>
    {
        #region MEMBER AND CTR
        public NewsEventTemplateController(
            ITemplateSettingService templateSettingService,
            IHostingEnvironment hostingEnvironment)
        {
            _templateSettingService = templateSettingService;
            _hostingEnvironment = hostingEnvironment;
        }
        #endregion
        #region MAIN ACTION
        public override async Task<ActionResult> Index()
        {
            return await base.Index();
        }
        public override async Task<JsonResult> GetData(DataTablePostModel model)
        {
            try
            {
                model.IsArchiveActive = true;
                var result = await _templateSettingService.GetList(model);

                return Json(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public override async Task<IActionResult> Update(Guid Id)
        {
            var model = await _templateSettingService.GetById(Id);
            if (model != null)
            {
                ViewBag.ListViews = await BindViews();
                if (!string.IsNullOrEmpty(model.Path))
                {
                    string path = Path.Combine(_hostingEnvironment.WebRootPath, model.Path);
                    ViewBag.Data = String.Format("data:application/pdf;base64,{0}", Convert.ToBase64String(Global.WordToPdf(path)));
                }
                return View(GlobalConst.Form, model);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Detail(Guid Id)
        {
            var model = await _templateSettingService.GetById(Id);
            if (model != null)
            {
                ViewBag.ListViews = await BindViews();
                if (!string.IsNullOrEmpty(model.Path))
                {
                    string path = Path.Combine(_hostingEnvironment.WebRootPath, model.Path);
                    ViewBag.Data = String.Format("data:application/pdf;base64,{0}", Convert.ToBase64String(Global.WordToPdf(path)));
                }
                return View(GlobalConst.Form, model);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public async Task<JsonResult> BindPreview(string data)
        {
            var result = new
            {
                data = ""
            };
            try
            {
                await Task.Delay(0);
                var bytes = Convert.FromBase64String(data.Split(',')[1]);

                result = new
                {
                    data = String.Format("data:application/pdf;base64,{0}", Convert.ToBase64String(Global.ConvertToPdf(bytes)))
                };
            }
            catch(Exception ex)
            {

            }

            return Json(result);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Save(MstTemplateSetting model)
        {
            int result = 0;
            if (model != null)
            {
                IFormFile file = Request.Form.Files[0];

                var variable = Request.Form["variableName[]"].ToArray();
                var type = Request.Form["variableType[]"].ToArray();
                var data = Request.Form["variableData[]"].ToArray();
                var detail = new Tuple<string[], string[], string[]>(variable, type, data);
                if (model.TemplateSettingId != Guid.Empty)
                {
                    model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                    model.UpdatedDate = DateTime.Now;
                    result = await _templateSettingService.Update(model, file, detail);
                }
                else
                {
                    model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                    model.CreatedDate = DateTime.Now;
                    model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                    model.UpdatedDate = DateTime.Now;
                    result = await _templateSettingService.Insert(model, file, detail);
                }
            }
            return RedirectToIndex();
        }
        #endregion
        #region HELPER
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.NewsEventTemplate, new { Area = GlobalConst.Configuration });
        #endregion
    }
}

