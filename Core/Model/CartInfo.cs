using System;
using Datory;

namespace SS.Shopping.Core.Model
{
    [Table("ss_shopping_cart")]
    public class CartInfo : Entity
    {
        [TableColumn]
        public int SiteId { get; set; }

        [TableColumn]
        public int OrderId { get; set; }

        [TableColumn]
        public string UserName { get; set; }

        [TableColumn]
        public string SessionId { get; set; }

        [TableColumn]
        public string ProductId { get; set; }

        [TableColumn]
        public string ProductName { get; set; }

        [TableColumn]
        public string ImageUrl { get; set; }

        [TableColumn]
        public string LinkUrl { get; set; }

        [TableColumn]
        public decimal Fee { get; set; }

        [TableColumn]
        public bool IsDelivery { get; set; }

        [TableColumn]
        public int Count { get; set; }

        [TableColumn]
        public DateTime? AddDate { get; set; }
    }
}