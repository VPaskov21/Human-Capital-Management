using HCMApp.Data.Models;
using System.Collections.Generic;

namespace HCMApp.Data.Repositories
{
    public interface IAbsenceRequestsRepository
    {
        List<Absence_Request> GetAbsenceRequests();
        List<Absence_Request> GetAbsenceRequestsForUser(string email);
    }
}