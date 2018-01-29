using Newtonsoft.Json;

namespace SS.Shopping.Model
{
    [JsonObject(MemberSerialization.OptOut)]
    public class AreaInfo
    {
        public int Id { get; set; }

        public int DeliveryId { get; set; }

        public string Cities { get; set; }

        public int StartStandards { get; set; }

        public decimal StartFees { get; set; }

        public int AddStandards { get; set; }

        public decimal AddFees { get; set; }
    }
}