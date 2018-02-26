using System;
using System.Collections.Generic;

namespace SS.Shopping.Model
{
    public class OrderInfo
    {
        public int Id { get; set; }

        public int SiteId { get; set; }

        public string Guid { get; set; }

        public string UserName { get; set; }

        public string SessionId { get; set; }

        public string RealName { get; set; }

        public string Mobile { get; set; }

        public string Tel { get; set; }

        public string Location { get; set; }

        public string Address { get; set; }

        public string Message { get; set; }

        public string Channel { get; set; }

        public decimal TotalFee { get; set; }

        public decimal ExpressCost { get; set; }

        public int TotalCount { get; set; }

        public bool IsPaied { get; set; }

        public string State { get; set; }

        public DateTime AddDate { get; set; }

        public List<CartInfo> CartInfoList { get; set; }
    }
}