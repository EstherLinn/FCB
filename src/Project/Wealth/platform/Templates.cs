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

        public struct LoginErrorMesaage
        {
            public static readonly ID Root = new ID("{CFF12E21-8AA8-41A2-B9E6-FA3EE1B4341A}");
            public struct Fields
            {
                public static readonly ID ErrorMessage = new ID("{C6896022-AA88-4341-B1BA-EE54E771B274}");
                public static readonly ID ServerErrorMessage = new ID("{498DB32D-A282-44B9-8FB8-2ABE9F40456E}");
            }
        }
    }
}