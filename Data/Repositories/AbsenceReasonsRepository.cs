using HCMApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Repositories
{
    public class AbsenceReasonsRepository : IAbsenceReasonsRepository
    {
        private readonly AppDbContext context;

        public AbsenceReasonsRepository(AppDbContext context)
        {
            this.context = context;
        }

        public List<Absence_Reason> GetAbsence_Reasons() => context.Absence_Reasons.ToList();
    }
}
