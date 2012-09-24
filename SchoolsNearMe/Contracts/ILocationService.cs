using System.Net;
using SchoolsNearMe.Models;

namespace SchoolsNearMe.Contracts
{
    public interface ILocationService
    {
        Coordinate GetLocationByIpAddress(IPAddress ipAddress);
    }
}
