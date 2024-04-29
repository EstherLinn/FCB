using Feature.Wealth.Component.Models.FundDetail;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Models.TabCards
{
    public class TabCardsModel
    {
        public Item Datasource { get; private set; }
        public string Title1 => ItemUtils.GetFieldValue(Datasource,_TabCard.Fields.Title1);
        public string ImageUrl1 { get; set; }
        public string BannerLink1 { get; set; }
        public bool IsBlackFont1 => ItemUtils.IsChecked(Datasource, _TabCard.Fields.IsBlackFont1);
        public string Description1 => ItemUtils.GetFieldValue(Datasource, _TabCard.Fields.Description1);


        public string Title2 => ItemUtils.GetFieldValue(Datasource, _TabCard.Fields.Title2);
        public string ImageUrl2 { get; set; }
        public string BannerLink2 { get; set; }
        public bool IsBlackFont2 => ItemUtils.IsChecked(Datasource, _TabCard.Fields.IsBlackFont2);
        public string Description2 => ItemUtils.GetFieldValue(Datasource, _TabCard.Fields.Description2);

        public string Title3 => ItemUtils.GetFieldValue(Datasource, _TabCard.Fields.Title3);
        public string ImageUrl3 { get; set; }
        public string BannerLink3 { get; set; }
        public bool IsBlackFont3 => ItemUtils.IsChecked(Datasource, _TabCard.Fields.IsBlackFont3);
        public string Description3 => ItemUtils.GetFieldValue(Datasource, _TabCard.Fields.Description3);

        public IList<string> FundIDList { get; set; }
        public IList<FundCardBasicDTO> FundCardsInfos { get; set; }
        public IList<NavDTO> FundCardsNavs { get; set; }

        public string FundDetailLink => FundRelatedSettingModel.GetFundDetailsUrl();

        public TabCardsModel(Item item)
        {
            if (item == null)
            {
                return;
            }

            this.Datasource = item;

            this.ImageUrl1 = ItemUtils.ImageUrl(item, _TabCard.Fields.Banner1);
            this.ImageUrl2 = ItemUtils.ImageUrl(item, _TabCard.Fields.Banner2);
            this.ImageUrl3 = ItemUtils.ImageUrl(item, _TabCard.Fields.Banner3);

            this.BannerLink1 = ItemUtils.GeneralLink(item, _TabCard.Fields.BannerLink1)?.Url;
            this.BannerLink2 = ItemUtils.GeneralLink(item, _TabCard.Fields.BannerLink2)?.Url;
            this.BannerLink3 = ItemUtils.GeneralLink(item, _TabCard.Fields.BannerLink3)?.Url;

            this.FundIDList = ItemUtils.GetMultiLineText(item, _TabCard.Fields.FundIDList)?.Take(3).ToList() ?? new List<string>();
        }
    }

    public class FundCardBasicDTO
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public bool IsFall { get; set; }
        public string Rate { get; set; }
    }

    public class NavDTO
    {
        public string ProductCode { get; set; }
        public string NetAssetValueDate { get; set; }
        public decimal NetAssetValue { get; set; }
    }
}