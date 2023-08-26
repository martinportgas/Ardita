using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Interfaces
{
    public interface ISessionService
    {
        void Set(string key, string value);
        string Get(string key);
        void Remove(string key);
    }
}
