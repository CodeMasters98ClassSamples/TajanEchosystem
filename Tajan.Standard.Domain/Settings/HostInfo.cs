namespace Tajan.Standard.Domain.Settings;

public class HostInfo
{
    public string Host { get; set; }
    public ushort Port { get; set; }

    public override string ToString()
        => Host + ":" + Port;
}
