﻿using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Interfaces
{
    public interface IPageService
    {
        Task<MstPage> GetById(Guid id);
        Task<IEnumerable<MstPageDetail>> GetDetailByMainId(Guid id);
        Task<IEnumerable<MstPage>> GetAll(string par = " 1=1 ");
        Task<DataTableResponseModel<MstPage>> GetListPage(DataTablePostModel model);
        Task<int> Insert(MstPage model);
        Task<int> InsertDetail(MstPageDetail model);
        Task<int> Delete(MstPage model);
        Task<int> DeleteDetail(Guid id);
        Task<int> Update(MstPage model);
    }
}
