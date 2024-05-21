using Sitecore.Data;

namespace Feature.Wealth.Component.Models.Navigation
{
    public class NavigationModel { }

    public class Templates
    {
        public struct ImageNavigationLink
        {
            public static readonly ID Id = new("{94F00CE4-A1DD-4347-B745-B53D4CF4C644}");

            public struct Fields
            {
                public static readonly ID Image = new("{6302F9CF-6E86-4053-B810-73A892619AB9}");
                public static readonly ID Image3x = new("{265D0CAA-97C7-41A9-9F00-1D33BF9C88DC}");
                public static readonly ID Title = new("{7D3ECF5E-4590-4E20-939C-D413367DDF02}");
                public static readonly ID Slogan = new("{0A3E3E67-ECA5-4C9D-9DB7-B45CE3D453A4}");
                public static readonly ID ButtonText = new("{024C6EE2-E2F3-4905-9E73-A8139428CB17}");
                public static readonly ID ButtonLink = new("{B9431912-1C75-4AD6-B7DF-B451F89B2127}");
            }
        }

        public struct IconNavigationLink
        {
            public static readonly ID Id = new("{8FAD2BF3-6CDD-436B-8996-5EEAEB7A63FF}");

            public struct Fields
            {
                public static readonly ID Icon = new("{47706230-D8E7-4603-BEDD-A7D2EE36B519}");
            }
        }

        public struct Navigable
        {
            public static readonly ID Id = new("{5EF236A8-1037-456D-894B-4B1D3CB4BDBC}");

            public struct Fields
            {
                public static readonly ID ShowChildren = new("{CC01D8FD-C850-40FB-883B-9ACC2D0B3628}");
                public static readonly ID ShowNavigation = new("{6C065467-19E3-403C-B882-A584557E9AA2}");
                public static readonly ID Highlight = new("{870DAF58-0CBE-447C-B4CB-E5E13BBC4D41}");
                public static readonly ID Background = new("{9B20CCFD-FB93-4502-B2AA-B117D49E4429}");
            }
        }
    }
}