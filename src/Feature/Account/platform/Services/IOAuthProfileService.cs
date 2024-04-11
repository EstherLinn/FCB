using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Account.Services
{
    interface IOAuthProfileService<T>
    {
        public abstract Task<T> GetProfileByToken(string access_token);
    }
}
