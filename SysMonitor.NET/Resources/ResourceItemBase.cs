using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SysMonitor.NET.Resources;

public abstract class ResourceItemBase<T> where T : struct, IComparable
{
    [JsonIgnore]
    public IResourceResult Result { get; protected set; }
    [JsonProperty("name")]
    public string Name { get; protected set; }
    [JsonProperty("max")]
    public T MaxValue { get; protected set; }
    [JsonProperty("min")]
    public T MinValue { get; protected set; }
    protected ResourceItemBase(string name, T maxValue, T minValue)
    {
        Name = name;
        MaxValue = maxValue;
        MinValue = minValue;
    }
    public void Update()
    {
        GetApplicationUsage();
        GetSystemUsage();
    }
    public async Task UpdateAsync()
    {
        await Task.WhenAll(GetApplicationUsageAsync(), GetSystemUsageAsync());
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
