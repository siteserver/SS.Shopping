using System;
using System.Collections.Generic;
using Datory;

namespace SS.Shopping.Core.Model
{
    [Table("ss_shopping_order")]
    public class OrderInfo : Entity
    {
        [TableColumn]
        public int SiteId { get; set; }

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
        public string Message { get; set; }

        [TableColumn]
        public string Channel { get; set; }

        [TableColumn]
        public decimal TotalFee { get; set; }

        [TableColumn]
        public decimal ExpressCost { get; set; }

        [TableColumn]
        public int TotalCount { get; set; }

        [TableColumn]
        public bool IsPayed { get; set; }

        [TableColumn]
        public string State { get; set; }

        [TableColumn]
        public DateTime? AddDate { get; set; }

        public List<CartInfo> CartInfoList { get; set; }
    }
}