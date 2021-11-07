using HCMApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly AppDbContext context;

        public RegionRepository(AppDbContext context)
        {
            this.context = context;
        }

        public List<Region> GetRegions() => context.Regions.Include(n => n.Country).ToList();

        public Region GetRegionByName(string regionName) => GetRegions().Where(n => n.RegionName.ToLower().Equals(regionName.ToLower())).SingleOrDefault();
    }
}
