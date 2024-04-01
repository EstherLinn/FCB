using Feature.Wealth.Component.Models.MultipleFileDownloadList;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Repositories
{
    public class MultipleFileDownloadListRepository
    {
        public MultipleFileDownloadListModel GetMultipleFileDownloadList()
        {
            var dataSource = RenderingContext.CurrentOrNull?.Rendering?.Item;
            if (dataSource == null)
            {
                return null;
            }

            var listItems = new List<DownloadListItemModel>();

            foreach (var listItem in dataSource.Children.Where(child => child.TemplateID == Templates.MultipleFileDownloadListItem.Id))
            {
                var listSubItems = new List<DownloadListSubItemModel>();

                foreach (var listSubItem in listItem.Children.Where(child => child.TemplateID == Templates.MultipleFileDownloadListSubItem.Id))
                {
                    DateTime date = (DateTime)ItemUtils.GetDateTimeFieldValue(listSubItem, Templates.MultipleFileDownloadListSubItem.Fields.Date);

                    listSubItems.Add(new DownloadListSubItemModel
                    {
                        item = listSubItem,
                        Date = date != DateTime.MinValue ? date.ToString("yyyy/MM/dd") : string.Empty,
                        Title = ItemUtils.GetFieldValue(listSubItem, Templates.MultipleFileDownloadListSubItem.Fields.Title),
                        Link = ItemUtils.GeneralLink(listSubItem, Templates.MultipleFileDownloadListSubItem.Fields.Link).Url
                    });
                }

                listItems.Add(new DownloadListItemModel
                {
                    item = listItem,
                    ItemTitle = ItemUtils.GetFieldValue(listItem, Templates.MultipleFileDownloadListItem.Fields.ItemTitle),
                    DownloadListSubitems = listSubItems
                });
            }

            var downloadList = new MultipleFileDownloadListModel
            {
                DataSource = dataSource,
                DownloadListItems = listItems
            };

            return downloadList;
        }
    }
}