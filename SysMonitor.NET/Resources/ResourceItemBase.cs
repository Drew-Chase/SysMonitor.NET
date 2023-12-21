using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SysMonitor.NET.Resources;

public abstract class ResourceItemBase<T> where T : struct, IComparable
{
    [JsonProperty("name")]
    public string Name { get; private set; }
    [JsonProperty("max")]
    public T MaxValue { get; private set; }
    [JsonProperty("min")]
    public T MinValue { get; private set; }
    protected ResourceItemBase(string name, T maxValue, T minValue)
    {
        Name = name;
        MaxValue = maxValue;
        MinValue = minValue;
    }
    public async Task<T> GetSystemUsageAsync() => await Task.Run(GetSystemUsage);
    public async Task<T> GetApplicationUsageAsync() => await Task.Run(GetApplicationUsage);
    public abstract T GetSystemUsage();
    public abstract T GetApplicationUsage();

    public JObject ToJson()
    {
        return JObject.FromObject(this);
    }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(ToJson());
    }
}
