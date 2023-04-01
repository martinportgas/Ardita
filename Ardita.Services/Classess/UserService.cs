using Ardita.Models.DbModels;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Classess
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<int> Delete(MstUser model)
        {
            return await _userRepository.Delete(model);
        }

        public async Task<IEnumerable<MstUser>> GetAll()
        {
            return await _userRepository.GetAll();
        }

        public async Task<IEnumerable<MstUser>> GetById(Guid id)
        {
            return await _userRepository.GetById(id);
        }

        public async Task<int> Insert(MstUser model)
        {
            return await _userRepository.Insert(model);
        }

        public async Task<int> Update(MstUser model)
        {
            return await _userRepository.Update(model);
        }

        public void Upload(MstUser model)
        {
            _userRepository.Upload(model);
        }
    }
}
