using HCMApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Repositories
{
    public class AbsenceRequestsRepository : IAbsenceRequestsRepository
    {
        private readonly AppDbContext context;

        public AbsenceRequestsRepository(AppDbContext context)
        {
            this.context = context;
        }

        public List<Absence_Request> GetAbsenceRequests() => context.Absence_Requests.Include(n => n.Absence_Reason)
                                                                                      .Include(n => n.User)
                                                                                        .ThenInclude(n => n.Leave_Days)
                                                                                       .ToList();

        public List<Absence_Request> GetAbsenceRequestsForUser(string email) => GetAbsenceRequests().Where(n => n.User.Email.ToLower().Equals(email.ToLower())).ToList();
    }
}
