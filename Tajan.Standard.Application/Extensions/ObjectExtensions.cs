using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Tajan.Standard.Application.Extensions;

public static class ObjectExtensions
{
    public static string Serialize(this object obj)
        => JsonConvert.SerializeObject(obj);

    public static string SerializeWithCamelCaseProperties(this object obj)
        => JsonConvert.SerializeObject(obj,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
}
