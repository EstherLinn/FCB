using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Account.Pipelines.DI
{
    public class MemberLoginService : IMemberLoginService
    {
        public bool IsAppLogin { get; set; }

        public bool AppLoginSuccess { get; set; }

    }
}
