using System.Collections.Generic;
using SchoolsNearMe.Models;

namespace SchoolsNearMe.Contracts
{
    public interface ISchoolShifterService
    {
        List<School> Shift(IEnumerable<School> schools);
    }
}