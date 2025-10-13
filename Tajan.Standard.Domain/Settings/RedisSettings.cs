namespace Tajan.Standard.Domain.Settings;

public class RedisSettings
{
    public string ServiceName { get; set; }

    public ResiliencySettings Resiliency { get; set; }

    public HostInfo MasterNode { get; set; }

    public List<HostInfo> ReplicaNodes { get; set; }

    public string GetConnectionStrings()
        => string.Join(",", MasterNode.ToString());


    public List<HostInfo> GetAllDistinctNodes()
    {
        List<HostInfo> replicaEndpointList = ReplicaNodes;
        replicaEndpointList.Add(MasterNode);
        return replicaEndpointList.DistinctBy(x => x.ToString()).ToList();
    }
}
