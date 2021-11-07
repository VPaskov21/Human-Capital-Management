using HCMApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Repositories
{
    public class SalaryHistoriesRepository : ISalaryHistoriesRepository
    {
        private readonly AppDbContext context;

        public SalaryHistoriesRepository(AppDbContext context)
        {
            this.context = context;
        }

        public List<SalaryHistory> GetSalaryHistories() => context.SalaryHistories.Include(n => n.User)
                                                                                    .ThenInclude(n => n.Salary)
                                                                                    .Include(n => n.WorkMonth).ToList();
    }
}
