using HCMApp.Data.Models;
using System.Collections.Generic;

namespace HCMApp.Data.Repositories
{
    public interface IWorkMonthsRepository
    {
        List<WorkMonth> GetWorkMonths();
    }
}