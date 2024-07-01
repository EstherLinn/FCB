using Feature.Wealth.ScheduleAgent.Repositories;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.QuartzSchedule;

namespace Feature.Wealth.ScheduleAgent.Schedules.News
{
    internal class NewsContent : SitecronAgentBase
    {
        private readonly NewsRepository _repository;

        public NewsContent()
        {
            this._repository = new NewsRepository(this.Logger);
        }

        protected override async Task Execute()
        {
            await _repository.SaveNewsLContentData();
        }
    }
}