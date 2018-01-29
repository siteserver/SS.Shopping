using Newtonsoft.Json;

namespace SS.Shopping.Model
{
    [JsonObject(MemberSerialization.OptOut)]
    public class ConfigInfo
    {
        public bool IsForceLogin { get; set; } = true;
    }
}