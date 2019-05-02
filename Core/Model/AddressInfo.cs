using Datory;

namespace SS.Shopping.Core.Model
{
    [Table("ss_shopping_address")]
    public class AddressInfo : Entity
    {
        [TableColumn]
        public string UserName { get; set; }

        [TableColumn]
        public string SessionId { get; set; }

        [TableColumn]
        public string RealName { get; set; }

        [TableColumn]
        public string Mobile { get; set; }

        [TableColumn]
        public string Tel { get; set; }

        [TableColumn]
        public string Location { get; set; }

        [TableColumn]
        public string Address { get; set; }

        [TableColumn]
        public string ZipCode { get; set; }

        [TableColumn]
        public bool IsDefault { get; set; }
    }
}