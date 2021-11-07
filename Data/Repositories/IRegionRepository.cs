using HCMApp.Data.Models;
using System.Collections.Generic;

namespace HCMApp.Data.Repositories
{
    public interface IRegionRepository
    {
        Region GetRegionByName(string regionName);
        List<Region> GetRegions();
    }
}