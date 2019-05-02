using Datory;

namespace SS.Shopping.Core.Model
{
    [Table("ss_shopping_area")]
    public class AreaInfo : Entity
    {
        [TableColumn]
        public int DeliveryId { get; set; }

        [TableColumn]
        public string Cities { get; set; }

        [TableColumn]
        public int StartStandards { get; set; }

        [TableColumn]
        public decimal StartFees { get; set; }

        [TableColumn]
        public int AddStandards { get; set; }

        [TableColumn]
        public decimal AddFees { get; set; }
    }
}