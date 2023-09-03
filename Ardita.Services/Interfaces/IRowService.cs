﻿using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Services.Interfaces
{
    public interface IRowService
    {
        Task<TrxRow> GetById(Guid id);
        Task<IEnumerable<TrxRow>> GetAll(string par = " 1=1 ");

        /// <summary>
        /// get rows that have never been used by media storage in active
        /// </summary>
        /// <returns>list row</returns>
        Task<IEnumerable<TrxRow>> GetAvailableRow();
        Task<DataTableResponseModel<object>> GetList(DataTablePostModel model);
        Task<int> Insert(TrxRow model);
        Task<bool> InsertBulk(List<TrxRow> rows);
        Task<int> Delete(TrxRow model);
        Task<int> Update(TrxRow model);
    }
}
