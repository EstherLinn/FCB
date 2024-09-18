using Feature.Wealth.Component.Models.RelatedArticlesLink;
using Sitecore.Mvc.Presentation;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Repositories
{
    public class RelatedArticlesLinkRepository
    {
        public RelatedArticlesLinkModel RelatedArticleLink()
        {
            var renderingContext = RenderingContext.CurrentOrNull;
            var dataSource = renderingContext?.ContextItem;

            if (dataSource == null || dataSource.TemplateID != Templates.RelatedArticles.Id)
            {
                return null;
            }

            RelatedArticlesLinkModel model = new RelatedArticlesLinkModel();

            model.Datasource = dataSource;
            model.MainTitle = ItemUtils.GetFieldValue(dataSource, Templates.RelatedArticles.Fields.MainTitle);
            model.ImagePc1 = ItemUtils.ImageUrl(dataSource, Templates.RelatedArticles.Fields.ImagePc1);
            model.ImageMb1 = ItemUtils.ImageUrl(dataSource, Templates.RelatedArticles.Fields.ImageMb1);
            model.Title1 = ItemUtils.GetFieldValue(dataSource, Templates.RelatedArticles.Fields.Title1);
            model.Link1 = ItemUtils.GeneralLink(dataSource, Templates.RelatedArticles.Fields.Link1).Url;
            model.LinkTarget1 = ItemUtils.GeneralLink(dataSource, Templates.RelatedArticles.Fields.Link1).Target;
            model.LinkTitle1 = ItemUtils.GeneralLink(dataSource, Templates.RelatedArticles.Fields.Link1).Title;
            model.ImagePc2 = ItemUtils.ImageUrl(dataSource, Templates.RelatedArticles.Fields.ImagePc2);
            model.ImageMb2 = ItemUtils.ImageUrl(dataSource, Templates.RelatedArticles.Fields.ImageMb2);
            model.Title2 = ItemUtils.GetFieldValue(dataSource, Templates.RelatedArticles.Fields.Title2);
            model.Link2 = ItemUtils.GeneralLink(dataSource, Templates.RelatedArticles.Fields.Link2).Url;
            model.LinkTarget2 = ItemUtils.GeneralLink(dataSource, Templates.RelatedArticles.Fields.Link2).Target;
            model.LinkTitle2 = ItemUtils.GeneralLink(dataSource, Templates.RelatedArticles.Fields.Link2).Title;
            model.ImagePc3 = ItemUtils.ImageUrl(dataSource, Templates.RelatedArticles.Fields.ImagePc3);
            model.ImageMb3 = ItemUtils.ImageUrl(dataSource, Templates.RelatedArticles.Fields.ImageMb3);
            model.Title3 = ItemUtils.GetFieldValue(dataSource, Templates.RelatedArticles.Fields.Title3);
            model.Link3 = ItemUtils.GeneralLink(dataSource, Templates.RelatedArticles.Fields.Link3).Url;
            model.LinkTarget3 = ItemUtils.GeneralLink(dataSource, Templates.RelatedArticles.Fields.Link3).Target;
            model.LinkTitle3 = ItemUtils.GeneralLink(dataSource, Templates.RelatedArticles.Fields.Link3).Title;
            model.ImagePc4 = ItemUtils.ImageUrl(dataSource, Templates.RelatedArticles.Fields.ImagePc4);
            model.ImageMb4 = ItemUtils.ImageUrl(dataSource, Templates.RelatedArticles.Fields.ImageMb4);
            model.Title4 = ItemUtils.GetFieldValue(dataSource, Templates.RelatedArticles.Fields.Title4);
            model.Link4 = ItemUtils.GeneralLink(dataSource, Templates.RelatedArticles.Fields.Link4).Url;
            model.LinkTarget4 = ItemUtils.GeneralLink(dataSource, Templates.RelatedArticles.Fields.Link4).Target;
            model.LinkTitle4 = ItemUtils.GeneralLink(dataSource, Templates.RelatedArticles.Fields.Link4).Title;
            model.PageId = Sitecore.Context.Item.ID.ToString();
            model.RenderingId = renderingContext.Rendering.RenderingItem.ID.ToString();
            model.DatasourceId = dataSource.ID.ToString();

            return model;
        }
    }
}
