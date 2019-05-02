using Datory;

namespace SS.Shopping.Core.Model
{
    [Table("ss_shopping_delivery")]
    public class DeliveryInfo : Entity
    {
        [TableColumn]
        public int SiteId { get; set; }

        [TableColumn]
        public string DeliveryName { get; set; }

        [TableColumn]
        public string DeliveryType { get; set; }

        [TableColumn]
        public int StartStandards { get; set; }

        [TableColumn]
        public decimal StartFees { get; set; }

        [TableColumn]
        public int AddStandards { get; set; }

        [TableColumn]
        public decimal AddFees { get; set; }

        [TableColumn]
        public int Taxis { get; set; }
    }
}