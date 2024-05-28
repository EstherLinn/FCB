using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Project.Wealth.Platform
{
  public  struct Templates
  {
        public struct Login
        {
            public static readonly ID Root = new ID("{8B08C8DA-1155-4BF1-80F2-13E812B21D9B}");

            public struct Fields
            {
                public static readonly ID Image = new ID("{D6DDD874-8D25-4364-A46C-FAF2E78D6568}");
                public static readonly ID Content = new ID("{6446F151-3557-47A4-97BA-19193BDBE894}");
            }
        }
    }
}