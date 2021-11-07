using HCMApp.Data.Models;
using System.Collections.Generic;

namespace HCMApp.Data.Repositories
{
    public interface IAbsenceReasonsRepository
    {
        List<Absence_Reason> GetAbsence_Reasons();
    }
}