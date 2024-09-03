using Feature.Wealth.ScheduleAgent.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.QuartzSchedule;

namespace Feature.Wealth.ScheduleAgent.Schedules.Media
{
    internal class MediaItemRandomSort : SitecronAgentBase
    {
        private readonly MediaItemRandomRepository _repository;

        public MediaItemRandomSort()
        {
            this._repository = new MediaItemRandomRepository(this.Logger);
        }

        protected override Task Execute()
        {
            _repository.Initialize(this.JobItems.FirstOrDefault());

            return Task.Run(() =>
            {
                _repository.ChangeMediaItemSorting();
            });
        }
    }
}