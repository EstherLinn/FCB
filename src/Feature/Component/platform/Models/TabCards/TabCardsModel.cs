using Feature.Wealth.Component.Models.FundDetail;
using Newtonsoft.Json;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Models.TabCards
{
    public class TabCardsModel
    {
        public Item Datasource { get; private set; }
        public string Title1 => ItemUtils.GetFieldValue(Datasource, _TabCard.Fields.Title1);
        public string ImageUrl1 { get; set; }
        public string BannerLink1 { get; set; }
        public string BannerTarget1 { get; set; }
        public bool IsBlackFont1 => ItemUtils.IsChecked(Datasource, _TabCard.Fields.IsBlackFont1);
        public string Description1 => ItemUtils.GetFieldValue(Datasource, _TabCard.Fields.Description1);


        public string Title2 => ItemUtils.GetFieldValue(Datasource, _TabCard.Fields.Title2);
        public string ImageUrl2 { get; set; }
        public string BannerLink2 { get; set; }
        public string BannerTarget2 { get; set; }
        public bool IsBlackFont2 => ItemUtils.IsChecked(Datasource, _TabCard.Fields.IsBlackFont2);
        public string Description2 => ItemUtils.GetFieldValue(Datasource, _TabCard.Fields.Description2);

        public string Title3 => ItemUtils.GetFieldValue(Datasource, _TabCard.Fields.Title3);
        public string ImageUrl3 { get; set; }
        public string BannerLink3 { get; set; }
        public string BannerTarget3 { get; set; }
        public bool IsBlackFont3 => ItemUtils.IsChecked(Datasource, _TabCard.Fields.IsBlackFont3);
        public string Description3 => ItemUtils.GetFieldValue(Datasource, _TabCard.Fields.Description3);

        public string Title4 => ItemUtils.GetFieldValue(Datasource, _TabCard.Fields.Title4);
        public string ImageUrl4 { get; set; }
        public string BannerLink4 { get; set; }
        public string BannerTarget4 { get; set; }
        public bool IsBlackFont4 => ItemUtils.IsChecked(Datasource, _TabCard.Fields.IsBlackFont4);
        public string Description4 => ItemUtils.GetFieldValue(Datasource, _TabCard.Fields.Description4);

        public string Title5 => ItemUtils.GetFieldValue(Datasource, _TabCard.Fields.Title5);
        public string ImageUrl5 { get; set; }
        public string BannerLink5 { get; set; }
        public string BannerTarget5 { get; set; }
        public bool IsBlackFont5 => ItemUtils.IsChecked(Datasource, _TabCard.Fields.IsBlackFont5);
        public string Description5 => ItemUtils.GetFieldValue(Datasource, _TabCard.Fields.Description5);

        public string Title6 => ItemUtils.GetFieldValue(Datasource, _TabCard.Fields.Title6);
        public string ImageUrl6 { get; set; }
        public string BannerLink6 { get; set; }
        public string BannerTarget6 { get; set; }
        public bool IsBlackFont6 => ItemUtils.IsChecked(Datasource, _TabCard.Fields.IsBlackFont6);
        public string Description6 => ItemUtils.GetFieldValue(Datasource, _TabCard.Fields.Description6);

        public string Title7 => ItemUtils.GetFieldValue(Datasource, _TabCard.Fields.Title7);
        public string ImageUrl7 { get; set; }
        public string BannerLink7 { get; set; }
        public string BannerTarget7 { get; set; }
        public bool IsBlackFont7 => ItemUtils.IsChecked(Datasource, _TabCard.Fields.IsBlackFont7);
        public string Description7 => ItemUtils.GetFieldValue(Datasource, _TabCard.Fields.Description7);

        public string Title8 => ItemUtils.GetFieldValue(Datasource, _TabCard.Fields.Title8);
        public string ImageUrl8 { get; set; }
        public string BannerLink8 { get; set; }
        public string BannerTarget8 { get; set; }
        public bool IsBlackFont8 => ItemUtils.IsChecked(Datasource, _TabCard.Fields.IsBlackFont8);
        public string Description8 => ItemUtils.GetFieldValue(Datasource, _TabCard.Fields.Description8);

        public IList<string> FundIDList { get; set; }
        public IList<FundCardBasicDTO> FundCardsInfos { get; set; }
        public IList<NavDTO> FundCardsNavs { get; set; }

        public HtmlString FundIDListHtmlString { get; set; }
        public HtmlString FundCardsInfosHtmlString { get; set; }
        public HtmlString FundCardsNavsHtmlString { get; set; }

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
            this.ImageUrl4 = ItemUtils.ImageUrl(item, _TabCard.Fields.Banner4);
            this.ImageUrl5 = ItemUtils.ImageUrl(item, _TabCard.Fields.Banner5);
            this.ImageUrl6 = ItemUtils.ImageUrl(item, _TabCard.Fields.Banner6);
            this.ImageUrl7 = ItemUtils.ImageUrl(item, _TabCard.Fields.Banner7);
            this.ImageUrl8 = ItemUtils.ImageUrl(item, _TabCard.Fields.Banner8);

            var link1 = ItemUtils.GeneralLink(item, _TabCard.Fields.BannerLink1);
            var link2 = ItemUtils.GeneralLink(item, _TabCard.Fields.BannerLink2);
            var link3 = ItemUtils.GeneralLink(item, _TabCard.Fields.BannerLink3);
            var link4 = ItemUtils.GeneralLink(item, _TabCard.Fields.BannerLink4);
            var link5 = ItemUtils.GeneralLink(item, _TabCard.Fields.BannerLink5);
            var link6 = ItemUtils.GeneralLink(item, _TabCard.Fields.BannerLink6);
            var link7 = ItemUtils.GeneralLink(item, _TabCard.Fields.BannerLink7);
            var link8 = ItemUtils.GeneralLink(item, _TabCard.Fields.BannerLink8);

            this.BannerLink1 = link1?.Url;
            this.BannerLink2 = link2?.Url;
            this.BannerLink3 = link3?.Url;
            this.BannerLink4 = link4?.Url;
            this.BannerLink5 = link5?.Url;
            this.BannerLink6 = link6?.Url;
            this.BannerLink7 = link7?.Url;
            this.BannerLink8 = link8?.Url;
            this.BannerTarget1 = link1?.Target;
            this.BannerTarget2 = link2?.Target;
            this.BannerTarget3 = link3?.Target;
            this.BannerTarget4 = link4?.Target;
            this.BannerTarget5 = link5?.Target;
            this.BannerTarget6 = link6?.Target;
            this.BannerTarget7 = link7?.Target;
            this.BannerTarget8 = link8?.Target;

            this.FundIDList = ItemUtils.GetMultiLineText(item, _TabCard.Fields.FundIDList)?.Where(s => !string.IsNullOrWhiteSpace(s)).Take(3).ToList() ?? new List<string>();
            this.FundIDListHtmlString = new HtmlString(JsonConvert.SerializeObject(this.FundIDList));
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