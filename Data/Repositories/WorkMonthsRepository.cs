using HCMApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Repositories
{
    public class WorkMonthsRepository : IWorkMonthsRepository
    {
        private readonly AppDbContext context;

        public WorkMonthsRepository(AppDbContext context)
        {
            this.context = context;
        }

        public List<WorkMonth> GetWorkMonths() => context.WorkMonths.ToList();
    }
}
