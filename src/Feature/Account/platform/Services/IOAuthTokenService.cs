using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Account.Services
{
    interface IOAuthTokenService<T>
    {
        string AppId { get; }
        string AppSecret { get; }
        string TokenUrl { get; }
        string ProfileUrl { get; }
        string RedirectUrl { get; }

        public Task<T> GetTokensByCode(string code);
    }
}
