using System.Collections.Generic;
using System.Linq;
using SchoolsNearMe.Contracts;
using SchoolsNearMe.Models;

namespace SchoolsNearMe.Services
{
    public class SchoolShifterService : ISchoolShifterService
    {
        public List<School> Shift(IEnumerable<School> schools)
        {
            var schoolList = schools as List<School> ?? schools.ToList();
            var coordinates = schoolList.GroupBy(x => x.Location).Where(g => g.Count() > 1).Select(x => x.Key);
            foreach (var coordinate in coordinates)
            {
                decimal shiftSize = 0.00100M;
                decimal shift = shiftSize;
                Coordinate localCoordinate = coordinate;
                IEnumerable<School> schoolsToModify = schoolList.Where(x => x.Location.Equals(localCoordinate)).Skip(1);
                foreach (var school in schoolsToModify)
                {
                    school.Location = new Coordinate(school.Location.Latitude, school.Location.Longitude + shift);
                    shift += shiftSize;
                }
            }
            return schoolList.ToList();
        }
    }
}