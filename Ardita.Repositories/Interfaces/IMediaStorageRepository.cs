﻿using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Repositories.Interfaces;

public interface IMediaStorageRepository
{
    Task<TrxMediaStorage> GetById(Guid id);
    Task<IEnumerable<TrxMediaStorage>> GetAll();
    Task<IEnumerable<TrxMediaStorage>> GetByFilterModel(DataTableModel model);
    Task<int> GetCount();
    Task<int> Insert(TrxMediaStorage model, List<TrxMediaStorageDetail> detail);
    Task<int> Delete(TrxMediaStorage model);
    Task<int> Update(TrxMediaStorage model);
}