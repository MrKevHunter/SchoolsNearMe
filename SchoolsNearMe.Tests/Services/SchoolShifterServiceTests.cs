using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SchoolsNearMe.Models;
using SchoolsNearMe.Services;

namespace SchoolMap.Net.Tests.Services
{
    [TestFixture]
    public class SchoolShifterServiceTests
    {
        [Test]
        public void GivenASingleSchool_WhenCalled_ReturnsTheSchoolInTheSamePlace()
        {
            var coordinate = new Coordinate(50, 50);
            var schools = new List<School> {new School {Location = coordinate}};
            var schoolShifter = new SchoolShifterService();
            List<School> shiftedSchools = schoolShifter.Shift(schools);
            Assert.That(shiftedSchools.Count(),Is.EqualTo(1));
            Assert.That(shiftedSchools[0].Location, Is.EqualTo(coordinate));
        }

        [Test]
        public void GivenTwoSchoolsWithTheSameLocation_WhenCalled_ReturnsTwoSchoolsWithTheLatitudeOfTheSecondChanged()
        {
            var coordinate = new Coordinate(50, 50);
            var schools = new List<School>
                {
                    new School {SchoolName = "SchoolA", Location = coordinate},
                    new School() {SchoolName = "SchoolB", Location = coordinate}
                };
            var schoolShifter = new SchoolShifterService();
            List<School> shiftedSchools = schoolShifter.Shift(schools);
            Assert.That(shiftedSchools.Count(), Is.EqualTo(2));
            Assert.That(shiftedSchools[0].Location,Is.EqualTo(coordinate));
            Assert.That(shiftedSchools[1].Location,Is.Not.EqualTo(coordinate));
            Assert.That(shiftedSchools[1].Location.Latitude, Is.EqualTo(50.005));

        }
        [Test]
        public void GivenThreeSchoolsWithTheSameLocation_WhenCalled_ReturnsThreeSchoolsWithTheLatitudeOfTheLastTwoChanged()
        {
            var coordinate = new Coordinate(50, 50);
            var schools = new List<School>
                {
                    new School {SchoolName = "SchoolA", Location = coordinate},
                    new School() {SchoolName = "SchoolB", Location = coordinate},
                    new School() {SchoolName = "SchoolC", Location = coordinate}
                };
            var schoolShifter = new SchoolShifterService();
            List<School> shiftedSchools = schoolShifter.Shift(schools);
            Assert.That(shiftedSchools.Count(), Is.EqualTo(3));
            Assert.That(shiftedSchools[0].Location, Is.EqualTo(coordinate));
            Assert.That(shiftedSchools[1].Location, Is.Not.EqualTo(coordinate));
            Assert.That(shiftedSchools[1].Location.Latitude, Is.EqualTo(50.005));
            
            Assert.That(shiftedSchools[2].Location, Is.Not.EqualTo(coordinate));
            Assert.That(shiftedSchools[2].Location.Latitude, Is.EqualTo(50.010));
        }
    }
}