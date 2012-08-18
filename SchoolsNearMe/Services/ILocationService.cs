using System.Net;
using SchoolsNearMe.Models;

namespace SchoolsNearMe.Services
{
    public interface ILocationService
    {
        Coordinate GetLocationByIpAddress(IPAddress ipAddress);
    }
}
