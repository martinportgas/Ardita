using Ardita.Models.DbModels;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Ardita.Models.ViewModels.Users;
using Ardita.Models.ViewModels;
using Ardita.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Services.Classess
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPositionRepository _positionRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly ISubMenuRepository _subMenuRepository;
        private readonly IPageRepository _pageRepository;
        private readonly IRolePageRepository _rolePageRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserArchiveUnitRepository _userArchiveUnitRepository;
        private readonly IArchiveUnitRepository _archiveUnitRepository;
        private readonly IArchiveCreatorRepository _archiveCreatorRepository;

        public UserService(IUserRepository userRepository,
            IRoleRepository roleRepository,
            IUserRoleRepository userRoleRepository,
            IEmployeeRepository employeeRepository,
            IPositionRepository positionRepository,
            IMenuRepository menuRepository,
            ISubMenuRepository subMenuRepository,
            IPageRepository pageRepository,
            IRolePageRepository rolePageRepository,
            ICompanyRepository companyRepository,
            IUserArchiveUnitRepository userArchiveUnitRepository,
            IArchiveUnitRepository archiveUnitRepository,
            IArchiveCreatorRepository archiveCreatorRepository
            )
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _employeeRepository = employeeRepository;
            _positionRepository = positionRepository;
            _menuRepository = menuRepository;
            _subMenuRepository = subMenuRepository;
            _pageRepository = pageRepository;
            _rolePageRepository = rolePageRepository;
            _companyRepository = companyRepository;
            _userArchiveUnitRepository = userArchiveUnitRepository;
            _archiveUnitRepository = archiveUnitRepository;
            _archiveCreatorRepository = archiveCreatorRepository;
        }
        public async Task<int> Delete(MstUser model)
        {
            return await _userRepository.Delete(model);
        }

        public async Task<IEnumerable<IdxUserArchiveUnit>> GetIdxUserArchiveUnitByUserId(Guid id)
        {
            return await _userArchiveUnitRepository.GetByUserId(id);
        }

        public async Task<IEnumerable<MstUser>> GetAll()
        {
            return await _userRepository.GetAll();
        }

        public async Task<MstUser> GetById(Guid id)
        {
            var result = await _userRepository.GetById(id);
            result.Password = Global.Decode(result.Password);
            return result;
        }
        public async Task<DataTableResponseModel<object>> GetListUsers(DataTablePostModel model)
        {
            try
            {
                var filterData = new DataTableModel();

                filterData.sortColumn = model.columns[model.order[0].column].name;
                filterData.sortColumnDirection = model.order[0].dir;
                filterData.searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value;
                filterData.pageSize = model.length;
                filterData.skip = model.start;

                var results = await _userRepository.GetByFilterModel(filterData);

                var responseModel = new DataTableResponseModel<object>();
                var dataCount = await _userRepository.GetCount(filterData);

                responseModel.draw = model.draw;
                responseModel.recordsTotal = dataCount;
                responseModel.recordsFiltered = dataCount;
                responseModel.data = results.ToList();

                return responseModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Claim>> GetLogin(string username, string password)
        {       
            var user = await _userRepository.GetAll();
            var role = await _roleRepository.GetAll();
            var userRole = await _userRoleRepository.GetAll();
            var employee = await _employeeRepository.GetAll();
            var position = await _positionRepository.GetAll();
            var company = await _companyRepository.GetAll();
            var archiveUnit = await _archiveUnitRepository.GetAll();
            var creator = await _archiveCreatorRepository.GetAll();
            var userArchiveUnit = await _userArchiveUnitRepository.GetAll();

            var result = (from usr in user
                          join ur in userRole on usr.UserId equals ur.UserId
                          join r in role on ur.RoleId equals r.RoleId
                          join au in archiveUnit on ur.ArchiveUnitId equals au.ArchiveUnitId into aus
                          from aust in aus.DefaultIfEmpty()
                          join cr in creator on ur.CreatorId equals cr.CreatorId into crs
                          from crst in crs.DefaultIfEmpty()
                          join e in employee on usr.EmployeeId equals e.EmployeeId
                          join p in position on e.PositionId equals p.PositionId
                          join c in company on e.CompanyId equals c.CompanyId
                          where usr.Username == username && usr.Password == password
                          && usr.IsActive == true && r.IsActive == true && e.IsActive == true && ur.IsPrimary == true
                          select new
                          {
                              Username = usr.Username,
                              UserId = usr.UserId,
                              RoleId = r.RoleId,
                              RoleCode = r.Code,
                              RoleName = r.Name,
                              EmployeeNIK = e.Nik,
                              EmployeeName = e.Name,
                              EmployeeMail = e.Email,
                              EmployeePhone = e.Phone,
                              PositionId = p.PositionId,
                              CompanyId = e.CompanyId,
                              CompanyName = c.CompanyName,
                              EmployeeId = e.EmployeeId,
                              ArchiveUnitId = aust?.ArchiveUnitId.ToString() ?? string.Empty,
                              ArchiveUnitName = aust?.ArchiveUnitName ?? string.Empty,
                              CreatorId = crst?.CreatorId.ToString() ?? string.Empty,
                              CreatorName = crst?.CreatorName ?? string.Empty,
                          }
                ).ToList().FirstOrDefault();

            List<Claim> claims = null;
            if (result != null)
            {
                var arrArchiveUnit = userArchiveUnit.Where(x => x.UserId == result.UserId).Select(x => x.ArchiveUnit.ArchiveUnitCode).ToArray();

                claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Role, result.RoleCode),
                    new Claim(GlobalConst.Username ,result.Username),
                    new Claim(GlobalConst.UserId ,result.UserId.ToString()),
                    new Claim(GlobalConst.RoleId ,result.RoleId.ToString()),
                    new Claim(GlobalConst.RoleCode ,result.RoleCode.ToString()),
                    new Claim(GlobalConst.RoleName, result.RoleName),
                    new Claim(GlobalConst.EmployeeNIK, result.EmployeeNIK),
                    new Claim(GlobalConst.EmployeeName, result.EmployeeName),
                    new Claim(GlobalConst.EmployeeMail, string.IsNullOrEmpty(result.EmployeeMail) ? string.Empty : result.EmployeeMail),
                    new Claim(GlobalConst.EmployeePhone, string.IsNullOrEmpty(result.EmployeePhone) ? string.Empty : result.EmployeePhone),
                    new Claim(GlobalConst.PositionId, result.PositionId.ToString()),
                    new Claim(GlobalConst.CompanyId, result.CompanyId.ToString()!),
                    new Claim(GlobalConst.CompanyName, result.CompanyName),
                    new Claim(GlobalConst.EmployeeId, result.EmployeeId.ToString()),
                    new Claim(GlobalConst.ArchiveUnitId, result.ArchiveUnitId),
                    new Claim(GlobalConst.ArchiveUnitName, result.ArchiveUnitName),
                    new Claim(GlobalConst.CreatorId, result.CreatorId),
                    new Claim(GlobalConst.CreatorName, result.CreatorName),
                    new Claim(GlobalConst.ArchiveUnitCode, arrArchiveUnit.Length > 0  ? string.Join(",", arrArchiveUnit) : string.Empty)
                };
            }
       
            return claims;
        }

        public async Task<List<UserMenuListViewModel>> GetUserMenu(Guid id)
        {
            var menuResults = await _menuRepository.GetAll();
            var subMenuResults = await _subMenuRepository.GetAll();
            var pageResults = await _pageRepository.GetAll();
            var rolePageResults = await _rolePageRepository.GetAll();
            var roleResults = await _roleRepository.GetAll();
        

            var results = (from menu in menuResults
                          join subMenu in subMenuResults on menu.MenuId equals subMenu.MenuId
                          join page in pageResults on subMenu.SubmenuId equals page.SubmenuId
                          join rolePage in rolePageResults on page.PageId equals rolePage.PageId
                          join role in roleResults on rolePage.RoleId equals role.RoleId
                          where role.RoleId == id && menu.IsActive==true && subMenu.IsActive==true
                          && page.IsActive==true && role.IsActive==true
                          select new UserMenuListViewModel
                          {
                             MenuId = menu.MenuId,
                             MenuName = menu.Name,
                             MenuPath = menu.Path,
                             MenuIcon = menu.Icon,
                             MenuSort = menu.Sort,
                             SubMenuId = subMenu.SubmenuId,
                             SubMenuName = subMenu.Name,
                             SubMenuPath = subMenu.Path,
                             SubMenuSort = subMenu.Sort,
                             PageId = page.PageId,
                             PageName = page.Name,
                             PagePath = page.Path,
                             RoleId = role.RoleId,
                             RoleName = role.Name
                          }
                ).ToList();
            

            return results;
        }

        public async Task<int> Insert(MstUser model, string[] archiveUnitIds)
        {
            int result = -1;
            result = await _userRepository.Insert(model);

            if(result != -1 && archiveUnitIds.Length > 0)
            {
                List<IdxUserArchiveUnit> idxUserArchiveUnits = new();
                foreach (var item in archiveUnitIds)
                {
                    IdxUserArchiveUnit idxUserArchiveUnit = new();

                    Guid archiveUnitId = Guid.Empty;
                    if (Guid.TryParse(item, out archiveUnitId))
                    {
                        idxUserArchiveUnit.UserId = model.UserId;
                        idxUserArchiveUnit.ArchiveUnitId = archiveUnitId;
                        idxUserArchiveUnit.CreatedBy = model.CreatedBy;
                        idxUserArchiveUnit.CreatedDate = model.CreatedDate;

                        idxUserArchiveUnits.Add(idxUserArchiveUnit);
                    }
                }

                await _userArchiveUnitRepository.InsertBulk(idxUserArchiveUnits);
            }

            return result;
        }

        public async Task<bool> InsertBulk(List<MstUser> users)
        {
            return await _userRepository.InsertBulk(users);
        }

        public async Task<int> Update(MstUser model, string[] archiveUnitIds)
        {
            int result = -1;
            
            result = await _userRepository.Update(model);

            if(result != -1)
            {
                await _userArchiveUnitRepository.DeleteByUserId(model.UserId);

                if (archiveUnitIds.Length > 0)
                {
                    List<IdxUserArchiveUnit> idxUserArchiveUnits = new();
                    foreach (var item in archiveUnitIds)
                    {
                        IdxUserArchiveUnit idxUserArchiveUnit = new();

                        Guid archiveUnitId = Guid.Empty;
                        if (Guid.TryParse(item, out archiveUnitId))
                        {
                            idxUserArchiveUnit.UserId = model.UserId;
                            idxUserArchiveUnit.ArchiveUnitId = archiveUnitId;
                            idxUserArchiveUnit.CreatedBy = model.CreatedBy;
                            idxUserArchiveUnit.CreatedDate = model.CreatedDate;

                            idxUserArchiveUnits.Add(idxUserArchiveUnit);
                        }
                    }

                    await _userArchiveUnitRepository.InsertBulk(idxUserArchiveUnits);
                }
            }
            return result;
        }
        public async Task<int> ChangePassword(MstUser model)
        {
            var result = await _userRepository.ChangePassword(model);
            return result;
        }

        public async Task<bool> FindPasswordByUsername(Guid Id, string password)
        {
            return await _userRepository.FindPasswordByUsername(Id, password);
        }
    }
}
