using Ardita.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Classess
{
    public class SessionService : ISessionService
    {
        private readonly Dictionary<string, string> _sessionData = new Dictionary<string, string>();
        public string Get(string key)
        {
            _sessionData.TryGetValue(key, out string value);
            return value;
        }

        public void Remove(string key)
        {
            _sessionData.Remove(key);
        }

        public void Set(string key, string value)
        {
            if(string.IsNullOrEmpty(Get(key)))
                _sessionData[key] = value;
            else
            {
                Remove(key);
                _sessionData[key] = value;
            }
        }
    }
}
