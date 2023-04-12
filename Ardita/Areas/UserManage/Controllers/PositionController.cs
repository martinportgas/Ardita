﻿using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Ardita.Services.Classess;
using Ardita.Models.ViewModels;
using Ardita.Areas.UserManage.Models;
using Ardita.Models.DbModels;
using Ardita.Areas.UserManage.Models;

namespace Ardita.Areas.UserManage.Controllers
{
    [CustomAuthorizeAttribute]
    [Area("UserManage")]
    public class PositionController : Controller
    {
        private readonly IPositionService _positionService;
        public PositionController(IPositionService positionService)
        {
            _positionService = positionService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetData()
        {
            try
            {
                var model = new DataTableModel();

                model.draw = Request.Form["draw"].FirstOrDefault();
                model.start = Request.Form["start"].FirstOrDefault();
                model.length = Request.Form["length"].FirstOrDefault();
                model.sortColumn = Request.Form["columns[" + Request.Form["order[1][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                model.sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                model.searchValue = Request.Form["search[value]"].FirstOrDefault();
                model.pageSize = model.length != null ? Convert.ToInt32(model.length) : 0;
                model.skip = model.start != null ? Convert.ToInt32(model.start) : 0;
                model.recordsTotal = 0;

                var result = await _positionService.GetListPosition(model);

                var jsonResult = new
                {
                    draw = result.draw,
                    recordsFiltered = result.recordsFiltered,
                    recordsTotal = result.recordsTotal,
                    data = result.data
                };
                return Json(jsonResult);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IActionResult Add()
        {
            return View();
        }
        public async Task<IActionResult> Update(Guid Id)
        {
            var data = await _positionService.GetById(Id);
            if (data.Count() > 0)
            {
                var model = new MstPosition();
                model.PositionId = data.FirstOrDefault().PositionId;
                model.Code = data.FirstOrDefault().Code;
                model.Name = data.FirstOrDefault().Name;
                model.IsActive = data.FirstOrDefault().IsActive;
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Position", new { Area = "UserManage" });
            }

        }
        public async Task<IActionResult> Remove(Guid Id)
        {
            var data = await _positionService.GetById(Id);
            if (data.Count() > 0)
            {
                var model = new MstPosition();
                model.PositionId = data.FirstOrDefault().PositionId;
                model.Code = data.FirstOrDefault().Code;
                model.Name = data.FirstOrDefault().Name;
                model.IsActive = data.FirstOrDefault().IsActive;
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Position", new { Area = "UserManage" });
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(MstPosition model)
        {
            int result = 0;
            if (model != null)
            {
                if (model.PositionId != Guid.Empty)
                {
                    model.UpdateBy = User.FindFirst("UserId").Value.ToString();
                    model.UpdateDate = DateTime.Now;
                    result = await _positionService.Update(model);
                }

                else 
                {
                    model.CreatedBy = User.FindFirst("UserId").Value.ToString();
                    model.CreatedDate = DateTime.Now;
                    result = await _positionService.Insert(model);
                }
                    

            }
            return RedirectToAction("Index", "Position", new { Area = "UserManage" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(MstPosition model)
        {
            int result = 0;
            if (model != null && model.PositionId != Guid.Empty)
            {
                model.UpdateBy = User.FindFirst("UserId").Value.ToString();
                model.UpdateDate = DateTime.Now;
                result = await _positionService.Delete(model);
            }
            return RedirectToAction("Index", "Position", new { Area = "UserManage" });
        }
    }
}