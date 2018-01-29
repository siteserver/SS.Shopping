using Newtonsoft.Json;

namespace SS.Shopping.Model
{
    [JsonObject(MemberSerialization.OptOut)]
    public class DeliveryInfo
    {
        public int Id { get; set; }

        public int PublishmentSystemId { get; set; }

        public string DeliveryName { get; set; }

        public string DeliveryType { get; set; }

        public int StartStandards { get; set; }

        public decimal StartFees { get; set; }

        public int AddStandards { get; set; }

        public decimal AddFees { get; set; }

        public int Taxis { get; set; }
    }
}