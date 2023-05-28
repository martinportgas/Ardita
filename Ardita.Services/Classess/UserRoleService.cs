using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.UserRoles;
using Ardita.Repositories.Classess;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;

namespace Ardita.Services.Classess
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPositionRepository _positionRepository;

        public UserRoleService(
            IUserRoleRepository userRoleRepository,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IEmployeeRepository employeeRepository,
            IPositionRepository positionRepository
            )
        {
            _userRoleRepository = userRoleRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _employeeRepository = employeeRepository;
            _positionRepository = positionRepository;
        }

        public Task<int> Delete(IdxUserRole model)
        {
            return _userRoleRepository.Delete(model);
        }

        public async Task<IEnumerable<IdxUserRole>> GetAll()
        {
            return await _userRoleRepository.GetAll();
        }

        public async Task<IEnumerable<IdxUserRole>> GetById(Guid id)
        {
            return await _userRoleRepository.GetById(id);
        }

        public async Task<UserRoleListViewModel> GetListUserRoles(Guid Id)
        {
            var userRoleListViewModel = new UserRoleListViewModel();

            var userRoleResult = await _userRoleRepository.GetAll();
            var userResult = await _userRepository.GetAll();
            var roleResult = await _roleRepository.GetAll();


            var userHeader = userResult.Where(x => x.UserId == Id).FirstOrDefault();

            var userRoleDetail = (from userRole in userRoleResult
                                  join user in userResult on userRole.UserId equals user.UserId
                                  join role in roleResult on userRole.RoleId equals role.RoleId
                                  where userRole.UserId == Id
                                  select new UserRoleListViewDetailModel
                                  {
                                      UserRoleId = userRole.UserRoleId,
                                      RoleId = role.RoleId,
                                      RoleCode = role.Code,
                                      RoleName = role.Name
                                  });


            userRoleListViewModel.UserRoles = userRoleDetail.ToList();
            userRoleListViewModel.Users = userHeader;
            userRoleListViewModel.Roles = roleResult.ToList();

            return userRoleListViewModel;
        }
        public async Task<DataTableResponseModel<object>> GetList(DataTablePostModel model)
        {
            try
            {
                var filterData = new DataTableModel
                {
                    sortColumn = model.columns[model.order[0].column].name,
                    sortColumnDirection = model.order[0].dir,
                    searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value,
                    pageSize = model.length,
                    skip = model.start,
                    PositionId = model.PositionId,
                    SessionUser = model.SessionUser,
                };

                int dataCount = await _userRoleRepository.GetCountByFilterModel(filterData);
                var results = await _userRoleRepository.GetByFilterModel(filterData);

                var responseModel = new DataTableResponseModel<object>
                {
                    draw = model.draw,
                    recordsTotal = dataCount,
                    recordsFiltered = dataCount,
                    data = results.ToList()
                };

                return responseModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Task<int> Insert(IdxUserRole model)
        {
            return _userRoleRepository.Insert(model);
        }

        public Task<int> Update(IdxUserRole model)
        {
            return _userRoleRepository.Update(model);
        }
    }
}
