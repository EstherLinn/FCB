using Feature.Wealth.Component.Models.FundDetail;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Linq;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Models.TabCards
{
    public class TabCardsModel
    {
        public Item Datasource { get; private set; }

        public string ImageUrl1 { get; set; }
        public string BannerLink1 { get; set; }
        public bool IsBlackFont1 => ((CheckboxField)Datasource?.Fields[_TabCard.Fields.IsBlackFont1])?.Checked ?? false;

        public string ImageUrl2 { get; set; }
        public string BannerLink2 { get; set; }
        public bool IsBlackFont2 => ((CheckboxField)Datasource?.Fields[_TabCard.Fields.IsBlackFont2])?.Checked ?? false;

        public string ImageUrl3 { get; set; }
        public string BannerLink3 { get; set; }
        public bool IsBlackFont3 => ((CheckboxField)Datasource?.Fields[_TabCard.Fields.IsBlackFont3])?.Checked ?? false;

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

            this.BannerLink1 = string.IsNullOrEmpty(ItemUtils.GeneralLink(item, _TabCard.Fields.BannerLink1)?.Url) ? "#" : ItemUtils.GeneralLink(item, _TabCard.Fields.BannerLink1)?.Url;
            this.BannerLink2 = string.IsNullOrEmpty(ItemUtils.GeneralLink(item, _TabCard.Fields.BannerLink2)?.Url) ? "#" : ItemUtils.GeneralLink(item, _TabCard.Fields.BannerLink2)?.Url;
            this.BannerLink3 = string.IsNullOrEmpty(ItemUtils.GeneralLink(item, _TabCard.Fields.BannerLink3)?.Url) ? "#" : ItemUtils.GeneralLink(item, _TabCard.Fields.BannerLink3)?.Url;

            this.FundIDList = this.Datasource.GetFieldValue(_TabCard.Fields.FundIDList)?.ToString().Split(';').Take(3).ToList() ?? new List<string>();
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