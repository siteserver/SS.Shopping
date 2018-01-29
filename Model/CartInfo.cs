using System;

namespace SS.Shopping.Model
{
    public class CartInfo
    {
        public int Id { get; set; }

        public int PublishmentSystemId { get; set; }

        public int OrderId { get; set; }

        public string UserName { get; set; }

        public string SessionId { get; set; }

        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public string ImageUrl { get; set; }

        public string LinkUrl { get; set; }

        public decimal Fee { get; set; }

        public bool IsDelivery { get; set; }

        public int Count { get; set; }

        public DateTime AddDate { get; set; }
    }
}