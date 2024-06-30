using Feature.Wealth.ScheduleAgent.Repositories;
using System.Linq;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.QuartzSchedule;

namespace Feature.Wealth.ScheduleAgent.Schedules.News
{
    internal class NewsList : SitecronAgentBase
    {
        private readonly NewsRepository _repository;

        public NewsList()
        {
            this._repository = new NewsRepository(this.Logger);
        }

        protected override async Task Execute()
        {
            var jobItem = this.JobItems.FirstOrDefault();
            await _repository.SaveNewsListData(jobItem);
        }
    }
}