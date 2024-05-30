using Sitecore.Data;

namespace Feature.Wealth.Component.Models.StructuredProduct
{
    /// <summary>
    /// 內頁資料源
    /// </summary>
    internal struct StructProductDetailDatasource
    {
        internal static readonly ID Id = new ID("{DEB3D97E-6F6E-4B02-96DE-34BF0B065DD1}");

        internal struct Fields
        {
            // 繼承 _StructProductTagDatasource
        }
    }

    /// <summary>
    /// 列表資料源
    /// </summary>
    internal struct StructProductListDatasource
    {
        internal static readonly ID Id = new ID("{68D35A72-BBF2-42F7-AA3A-476155910216}");

        internal struct Fields
        {
            internal static readonly ID DetailPageItem = new ID("{14AD72A5-A926-47A7-89DB-7CCB8C45848B}");

            // 繼承 _StructProductTagDatasource
        }
    }

    /// <summary>
    /// Tag資料源
    /// </summary>
    internal struct _StructProductTagDatasource
    {
        internal static readonly ID Id = new ID("{F527CF20-0071-4050-BEC7-926D2434E77C}");

        internal struct Fields
        {
            internal static readonly ID KeywordDatasource = new ID("{46AD65AB-28CF-4639-BB8D-3D98B4B4D19A}");
            internal static readonly ID TopicDatasource = new ID("{15F66C63-EEC4-4009-8A1A-358D84159CA2}");
        }
    }

    /// <summary>
    /// Tag設定項
    /// </summary>
    internal struct StructProductTag
    {
        internal static readonly ID Id = new ID("{D6254AF9-94B2-4578-86A6-FEDB902FA77F}");

        internal struct Fields
        {
            internal static readonly ID TagName = new ID("{0BEABC41-00D1-464E-A94D-2C4DC04D171D}");
            internal static readonly ID ProductCodeList = new ID("{B764C38B-DA68-47BB-82DC-3443E4BF6B6D}");
        }
    }
}
