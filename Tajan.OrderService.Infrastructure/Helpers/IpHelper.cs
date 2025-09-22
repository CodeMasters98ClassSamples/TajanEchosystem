using System.Net.Sockets;
using System.Net;

namespace Tajan.OrderService.Infrastructure.Helpers;

public class IpHelper
{
    public static string GetIpAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        return string.Empty;
    }
}
