using System.Collections.Generic;
using Datory;
using SiteServer.Plugin;
using SqlKata;
using SS.Shopping.Core.Model;

namespace SS.Shopping.Core.Provider
{
    public class OrderRepository
    {
        private readonly Repository<OrderInfo> _repository;
        public OrderRepository()
        {
            _repository = new Repository<OrderInfo>(Context.Environment.DatabaseType, Context.Environment.ConnectionString);
        }

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        private static class Attr
        {
            public const string Id = nameof(OrderInfo.Id);
            public const string Guid = nameof(OrderInfo.Guid);
            public const string SiteId = nameof(OrderInfo.SiteId);
            public const string UserName = nameof(OrderInfo.UserName);
            public const string SessionId = nameof(OrderInfo.SessionId);
            public const string RealName = nameof(OrderInfo.RealName);
            public const string Mobile = nameof(OrderInfo.Mobile);
            public const string Tel = nameof(OrderInfo.Tel);
            public const string Location = nameof(OrderInfo.Location);
            public const string Address = nameof(OrderInfo.Address);
            public const string ZipCode = nameof(OrderInfo.ZipCode);
            public const string Message = nameof(OrderInfo.Message);
            public const string Channel = nameof(OrderInfo.Channel);
            public const string TotalFee = nameof(OrderInfo.TotalFee);
            public const string ExpressCost = nameof(OrderInfo.ExpressCost);
            public const string TotalCount = nameof(OrderInfo.TotalCount);
            public const string IsPayed = nameof(OrderInfo.IsPayed);
            public const string State = nameof(OrderInfo.State);
            public const string AddDate = nameof(OrderInfo.AddDate);
        }

        public int Insert(OrderInfo orderInfo)
        {
            return _repository.Insert(orderInfo);
        }

        public void UpdateIsPayed(string guid)
        {
            _repository.Update(Q
                .Set(Attr.IsPayed, true)
                .Where(Attr.Guid, guid)
            );
        }

        public bool IsPayed(string guid)
        {
            return _repository.Get<bool>(Q
                .Select(Attr.IsPayed)
                .Where(Attr.Guid, guid)
            );
        }

        public void UpdateIsPayedAndState(int orderId, bool isPayed, string state)
        {
            _repository.Update(Q
                .Set(Attr.IsPayed, isPayed)
                .Set(Attr.State, state)
                .Where(Attr.Id, orderId)
            );
        }

        public void Delete(int orderId)
        {
            _repository.Delete(orderId);
        }

        public void Delete(List<int> deleteIdList)
        {
            if (deleteIdList == null || deleteIdList.Count == 0) return;

            _repository.Delete(Q.WhereIn(Attr.Id, deleteIdList));
        }

        public int GetOrderCount(int siteId)
        {
            return _repository.Count(Q.Where(Attr.SiteId, siteId));
        }

        public int GetOrderCount(int siteId, bool isPayed, string state)
        {
            Query query;

            if (isPayed)
            {
                query = Q
                    .Where(Attr.SiteId, siteId)
                    .Where(Attr.IsPayed, true);
            }
            else
            {
                query = Q
                    .Where(Attr.SiteId, siteId)
                    .Where(q => q.Where(Attr.IsPayed, false).OrWhereNull(Attr.IsPayed));
            }

            if (!string.IsNullOrEmpty(state))
            {
                query.Where(Attr.State, state);
            }

            return _repository.Count(query);
        }

        //public string GetSelectStringBySearch(int siteId, bool isPayed, string state, string keyword)
        //{
        //    var sqlString = $@"SELECT {nameof(OrderInfo.Id)}, 
        //    {nameof(OrderInfo.SiteId)}, 
        //    {nameof(OrderInfo.Guid)}, 
        //    {nameof(OrderInfo.UserName)}, 
        //    {nameof(OrderInfo.SessionId)}, 
        //    {nameof(OrderInfo.RealName)}, 
        //    {nameof(OrderInfo.Mobile)}, 
        //    {nameof(OrderInfo.Tel)}, 
        //    {nameof(OrderInfo.Location)}, 
        //    {nameof(OrderInfo.Address)}, 
        //    {nameof(OrderInfo.ZipCode)}, 
        //    {nameof(OrderInfo.Message)}, 
        //    {nameof(OrderInfo.Channel)}, 
        //    {nameof(OrderInfo.TotalFee)}, 
        //    {nameof(OrderInfo.ExpressCost)}, 
        //    {nameof(OrderInfo.TotalCount)}, 
        //    {nameof(OrderInfo.IsPayed)}, 
        //    {nameof(OrderInfo.State)}, 
        //    {nameof(OrderInfo.AddDate)}
        //    FROM {TableName} WHERE {nameof(OrderInfo.SiteId)} = {siteId}";

        //    if (isPayed)
        //    {
        //        sqlString += $@" AND {nameof(OrderInfo.IsPayed)} = 1";
        //    }
        //    else
        //    {
        //        sqlString += $@" AND ({nameof(OrderInfo.IsPayed)} = 0 OR {nameof(OrderInfo.IsPayed)} IS NULL)";
        //    }

        //    if (!string.IsNullOrEmpty(state))
        //    {
        //        state = Context.UtilsApi.FilterSql(state);
        //        sqlString += $" AND {nameof(OrderInfo.State)} = '{state}'";
        //    }
        //    if (!string.IsNullOrEmpty(keyword))
        //    {
        //        keyword = Context.UtilsApi.FilterSql(keyword);
        //        sqlString += $" AND ({nameof(OrderInfo.Guid)} LIKE '%{keyword}%' OR {nameof(OrderInfo.RealName)} LIKE '%{keyword}%' OR {nameof(OrderInfo.Mobile)} LIKE '%{keyword}%' OR {nameof(OrderInfo.Location)} LIKE '%{keyword}%' OR {nameof(OrderInfo.Address)} LIKE '%{keyword}%')";
        //    }
        //    sqlString += $" ORDER BY {nameof(OrderInfo.Id)} DESC";

        //    return sqlString;
        //}

        public OrderInfo GetOrderInfo(int orderId)
        {
            return _repository.Get(orderId);
        }

        public OrderInfo GetOrderInfo(string guid)
        {
            return _repository.Get(guid);
        }

        public IList<OrderInfo> GetOrderInfoList(string userName, bool isPayed)
        {
            if (isPayed)
            {
                return _repository.GetAll(Q
                    .Where(Attr.UserName, userName)
                    .Where(Attr.IsPayed, true)
                    .OrderByDesc(Attr.Id)
                );
            }

            return _repository.GetAll(Q
                .Where(Attr.UserName, userName)
                .Where(q => q.Where(Attr.IsPayed, false).OrWhereNull(Attr.IsPayed))
                .OrderByDesc(Attr.Id)
            );
        }

        public IList<OrderInfo> GetOrderInfoList(string userName, string state)
        {
            var query = Q.Where(Attr.UserName, userName);

            if (!string.IsNullOrEmpty(state))
            {
                query.Where(Attr.State, state);
            }

            query.OrderByDesc(Attr.Id);

            return _repository.GetAll(query);
        }
    }
}
