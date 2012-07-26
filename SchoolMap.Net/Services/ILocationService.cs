using System.Net;
using SchoolMap.Net.Models;

namespace SchoolMap.Net.Services
{
    public interface ILocationService
    {
        Coordinate GetLocationByIpAddress(IPAddress ipAddress);
    }
}
