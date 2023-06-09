﻿using Ardita.Models.DbModels;
using Ardita.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Interfaces
{
    public interface ISubMenuService
    {
        Task<IEnumerable<MstSubmenu>> GetById(Guid id);
        Task<IEnumerable<MstSubmenu>> GetAll();
        Task<int> Insert(MstSubmenu model);
        Task<int> Delete(MstSubmenu model);
        Task<int> Update(MstSubmenu model);
        Task<List<SubMenuTypes>> GetSubMenuTypeToLookUp();
        Task<List<SubMenuTypes>> GetSubMenuTypeToLookUp(Guid id);
    }
}
