using HCMApp.Data.Models;
using System.Collections.Generic;

namespace HCMApp.Data.Repositories
{
    public interface ISalaryHistoriesRepository
    {
        List<SalaryHistory> GetSalaryHistories();
    }
}