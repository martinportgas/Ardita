using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.Pages;
using Ardita.Models.ViewModels.Positions;
using Ardita.Repositories.Classess;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Classess
{
    public class PageService : IPageService
    {
        private readonly IPageRepository _pageRepository;
        private readonly ISubMenuRepository _subMenuRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IPageDetailRepository _pageDetailRepository;

        public PageService(
            IPageRepository pageRepository,
            ISubMenuRepository subMenuRepository,
            IMenuRepository menuRepository,
            IPageDetailRepository pageDetailRepository
            )
        {
            _pageRepository = pageRepository;
            _subMenuRepository = subMenuRepository;
            _menuRepository = menuRepository;
            _pageDetailRepository = pageDetailRepository;
        }

        public async Task<int> Delete(MstPage model)
        {
            return await _pageRepository.Delete(model);
        }
        public async Task<int> DeleteDetail(Guid id)
        {
            return await _pageDetailRepository.DeleteByMainId(id);
        }

        public async Task<IEnumerable<MstPage>> GetAll(string par = " 1=1 ")
        {
            return await _pageRepository.GetAll(par);
        }

        public async Task<MstPage> GetById(Guid id)
        {
            return await _pageRepository.GetById(id);
        }
        public async Task<IEnumerable<MstPageDetail>> GetDetailByMainId(Guid id)
        {
            return await _pageDetailRepository.GetByMainId(id);
        }

        public async Task<DataTableResponseModel<MstPage>> GetListPage(DataTablePostModel model)
        {
            try
            {
                int dataCount = 0;
                if (model.SubMenuId != null)
                {
                    dataCount = await _pageRepository.GetCountBySubMenuId(model.SubMenuId);
                }
                else 
                {
                    dataCount = await _pageRepository.GetCount();
                }

                var filterData = new DataTableModel();

                filterData.sortColumn = model.columns[model.order[0].column].data;
                filterData.sortColumnDirection = model.order[0].dir;
                filterData.searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value;
                filterData.pageSize = model.length;
                filterData.skip = model.start;
                filterData.SubMenuId = model.SubMenuId;

                var results = await _pageRepository.GetByFilterModel(filterData);

                var responseModel = new DataTableResponseModel<MstPage>();

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

        public async Task<int> Insert(MstPage model)
        {
            return await _pageRepository.Insert(model);
        }
        public async Task<int> InsertDetail(MstPageDetail model)
        {
            return await _pageDetailRepository.Insert(model);
        }

        public async Task<int> Update(MstPage model)
        {
            return await _pageRepository.Update(model);
        }
    }
}
