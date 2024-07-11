using Feature.Wealth.Component.Template;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using System.Linq;
using Xcms.Sitecore.Feature.Navigation.Models;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.FieldTypes;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Repositories
{
    /// <summary>
    /// Given a Datasource that represents a Navigation Menu, will generate
    /// a tree of ViewModels for use in constructing HTML page navigation.
    /// Useful for static navigation such as primary site menus or footers.
    /// </summary>
    public class DeclaredNavigationRepository
    {
        /// <summary>
        /// Creates a new instance of DeclaredNavigationRepository
        /// </summary>
        public DeclaredNavigationRepository() { }

        /// <summary>
        /// Returns an instance of NavigationMenu with available link groups and navigation links
        /// pre-populated and ready with display-compatible values.
        /// </summary>
        /// <param name="datasource">The Navigation Menu Item to process.</param>
        /// <param name="contextItem">Optional. The Item represented by the current HttpRequest.</param>
        /// <returns></returns>
        public NavigationMenu GetNavigation(Item datasource, Item contextItem = null)
        {
            var output = new NavigationMenu
            {
                Parent = new DeclaredNode()
            };
            ProcessChildren(datasource, output, contextItem);

            return output;
        }

        private void ProcessChildren(Item parent, DeclaredNode parentNode, Item contextItem)
        {
            if (!parent.HasChildren)
            {
                return;
            }

            var children = parent.GetChildren();
            foreach (var child in children.TakeWhile(child => child.HasContextLanguage()))
            {
                if (child.IsDerived(Templates.ImageNavigationLink.Id))
                {
                    var nav = new NavigationPageLink
                    {
                        Parent = parentNode,
                        Item = child,
                        NavigationTitle = child.GetFieldValue(Templates.PageNavigationTitle.Fields.NavigationTitle),
                        Link = string.IsNullOrEmpty(child["RedirectUrl"]) ? child.GeneralLink(Templates.NavigationLink.Fields.Link) : child.GeneralLink("RedirectUrl") ?? new Link()
                    };

                    if (nav.Link.Type == LinkType.NotSet || nav.Link.Url.IsNullOrEmpty())
                    {
                        nav.Link.Type = LinkType.Internal;
                        nav.Link.Url = child.Url();
                    }

                    nav.IsActive = LinkTargetIsAncestorOfContext(nav.Link, contextItem);
                    parentNode.Children.Add(nav);
                    ProcessChildren(child, nav, contextItem);
                    continue;
                }

                if (child.IsDerived(Templates.Page.Id))
                {
                    var nav = new NavigationPageLink
                    {
                        Parent = parentNode,
                        Item = child,
                        NavigationTitle = child.GetFieldValue(Templates.PageNavigationTitle.Fields.NavigationTitle),
                        Link = string.IsNullOrEmpty(child["RedirectUrl"]) ? child.GeneralLink(Templates.NavigationLink.Fields.Link) : child.GeneralLink("RedirectUrl") ?? new Link()
                    };

                    if (nav.Link.Type == LinkType.NotSet || nav.Link.Url.IsNullOrEmpty())
                    {
                        nav.Link.Type = LinkType.Internal;
                        nav.Link.Url = child.Url();
                    }

                    nav.IsActive = LinkTargetIsAncestorOfContext(nav.Link, contextItem);
                    parentNode.Children.Add(nav);
                    ProcessChildren(child, nav, contextItem);
                    continue;
                }

                // If we get here and the Item hasn't been processed, it's an unknown Item type.
                Log.Warn($"Declared Navigation Repository could not process an unsupported Item type: {child.TemplateName} for Item: {child.Paths.FullPath}.", this);
            }
        }

        private static bool LinkTargetIsAncestorOfContext(Link link, Item contextItem)
        {
            if (contextItem == null)
            {
                return false;
            }

            if (link.Type != LinkType.Internal)
            {
                return false;
            }

            if (link.TargetItem == null)
            {
                return false;
            }

            return link.TargetItem.Axes.IsAncestorOf(contextItem);
        }
    }
}

namespace Feature.Wealth.Component.Template
{
    internal struct Templates
    {
        internal struct Page
        {
            internal static readonly ID Id = new("{F61EE8BA-3566-42D0-B4E2-68D847E1ED2F}");
        }

        internal struct ListPage
        {
            internal static readonly ID Id = new("{A676B5B3-8A32-4620-8626-7C38B806EB79}");
        }

        internal struct LandingPage
        {
            internal static readonly ID Id = new("{1E8EF1B7-3785-4EAC-BCBB-51E0D3A881DA}");
        }

        internal struct PageNavigationTitle
        {
            internal static readonly ID Id = new("{B83F10F1-6F84-49E2-A807-856B6FF6C10A}");

            internal struct Fields
            {
                internal static readonly ID NavigationTitle = new("{A8FAD4B5-F582-4B63-9031-8FA8EA8B6D06}");
            }
        }

        internal struct LinkGroup
        {
            internal static readonly ID Id = new("{4A78B518-F009-45F3-89B9-BA2E8D0B7775}");
        }

        internal struct NavigationLink
        {
            internal static readonly ID Id = new("{38212B66-F3C1-46FC-9A32-E04F4337BF8F}");

            internal struct Fields
            {
                internal static readonly ID Link = new("{343B5D00-6E10-4863-840B-CEE38447B2F1}");
                internal static readonly ID UseThisDisplayName = new("{894E0985-DC34-4A16-80D6-5739E02A7FB3}");
            }
        }

        internal struct ImageNavigationLink
        {
            internal static readonly ID Id = new("{34827329-CEC6-4A46-90F1-1B93FB46DFDE}");

            internal struct Fields
            {
                internal static readonly ID Image = new("{C6E7B3E8-0BCA-4F7E-A7B9-22C0D1B43B63}");
            }
        }
    }
}