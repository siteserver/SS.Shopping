using System.Collections.Generic;
using Datory;
using SiteServer.Plugin;
using SqlKata;
using SS.Shopping.Core.Model;

namespace SS.Shopping.Core.Provider
{
    public class CartRepository
    {
        private readonly Repository<CartInfo> _repository;
        public CartRepository()
        {
            _repository = new Repository<CartInfo>(Context.Environment.DatabaseType, Context.Environment.ConnectionString);
        }

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        private static class Attr
        {
            public const string Id = nameof(CartInfo.Id);
            public const string SiteId = nameof(CartInfo.SiteId);
            public const string OrderId = nameof(CartInfo.OrderId);
            public const string UserName = nameof(CartInfo.UserName);
            public const string SessionId = nameof(CartInfo.SessionId);
            public const string ProductId = nameof(CartInfo.ProductId);
            public const string ProductName = nameof(CartInfo.ProductName);
            public const string ImageUrl = nameof(CartInfo.ImageUrl);
            public const string LinkUrl = nameof(CartInfo.LinkUrl);
            public const string Fee = nameof(CartInfo.Fee);
            public const string IsDelivery = nameof(CartInfo.IsDelivery);
            public const string Count = nameof(CartInfo.Count);
            public const string AddDate = nameof(CartInfo.AddDate);
        }

        public int Insert(CartInfo cartInfo)
        {
            return _repository.Insert(cartInfo);
        }

        public void Update(CartInfo cartInfo)
        {
            _repository.Update(cartInfo);
        }

        public void UpdateUserName(int siteId, string sessionId, string userName)
        {
            _repository.Update(Q
                .Set(Attr.UserName, userName)
                .Where(Attr.SiteId, siteId)
                .Where(Attr.OrderId, 0)
                .Where(Attr.SessionId, sessionId)
            );
        }

        public void UpdateOrderId(List<int> cartIdList, int orderId)
        {
            if (cartIdList == null || cartIdList.Count == 0) return;

            _repository.Update(Q
                .Set(Attr.OrderId, orderId)
                .WhereIn(Attr.Id, cartIdList)
            );
        }

        private Query GetQuery(string userName, string sessionId)
        {
            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(sessionId)) return null;

            Query query;

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(sessionId))
            {
                query = Q.Where(q => q
                    .Where(Attr.UserName, userName)
                    .OrWhere(Attr.SessionId, sessionId)
                );
            }
            else if (!string.IsNullOrEmpty(userName))
            {
                query = Q.Where(Attr.UserName, userName);
            }
            else
            {
                query = Q.Where(Attr.SessionId, sessionId);
            }

            return query;
        }

        public void Delete(int siteId, string userName, string sessionId)
        {
            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(sessionId)) return;

            var query = GetQuery(userName, sessionId);
            query.Where(Attr.SiteId, siteId);
            query.Where(Attr.OrderId, 0);

            _repository.Delete(query);
        }

        public int GetCartId(int siteId, string sessionId, string productId)
        {
            return _repository.Get<int>(Q
                .Select(Attr.Id)
                .Where(Attr.SiteId, siteId)
                .Where(Attr.OrderId, 0)
                .Where(Attr.SessionId, sessionId)
                .Where(Attr.ProductId, productId)
                );
        }

        public IList<CartInfo> GetCartInfoList(int siteId, string userName, string sessionId)
        {
            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(sessionId)) return new List<CartInfo>();

            var query = GetQuery(userName, sessionId);
            query.Where(Attr.SiteId, siteId);
            query.Where(Attr.OrderId, 0);

            return _repository.GetAll(query);
        }

        public IList<CartInfo> GetCartInfoList(int orderId)
        {
            return _repository.GetAll(Q
                .Where(Attr.OrderId, orderId)
                .OrderByDesc(Attr.Id)
            );
        }

        public CartInfo GetCartInfo(int cartId)
        {
            return _repository.Get(cartId);
        }
    }
}
