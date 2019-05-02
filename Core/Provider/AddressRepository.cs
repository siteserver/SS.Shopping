using System.Collections.Generic;
using Datory;
using SiteServer.Plugin;
using SqlKata;
using SS.Shopping.Core.Model;

namespace SS.Shopping.Core.Provider
{
    public class AddressRepository
    {
        private readonly Repository<AddressInfo> _repository;
        public AddressRepository()
        {
            _repository = new Repository<AddressInfo>(Context.Environment.DatabaseType, Context.Environment.ConnectionString);
        }

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        private static class Attr
        {
            public const string Id = nameof(AddressInfo.Id);
            public const string UserName = nameof(AddressInfo.UserName);
            public const string SessionId = nameof(AddressInfo.SessionId);
            public const string RealName = nameof(AddressInfo.RealName);
            public const string Mobile = nameof(AddressInfo.Mobile);
            public const string Tel = nameof(AddressInfo.Tel);
            public const string Location = nameof(AddressInfo.Location);
            public const string Address = nameof(AddressInfo.Address);
            public const string ZipCode = nameof(AddressInfo.ZipCode);
            public const string IsDefault = nameof(AddressInfo.IsDefault);
        }

        public int Insert(AddressInfo addressInfo)
        {
            return _repository.Insert(addressInfo);
        }

        public void Update(AddressInfo addressInfo)
        {
            _repository.Update(addressInfo);
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

        public void SetDefaultToFalse(string userName, string sessionId)
        {
            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(sessionId)) return;

            var query = GetQuery(userName, sessionId);
            query.Set(Attr.IsDefault, false);

            _repository.Update(query);
        }

        public void SetDefault(string userName, string sessionId, int addressId)
        {
            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(sessionId)) return;

            var query1 = GetQuery(userName, sessionId);
            query1.Set(Attr.IsDefault, false);

            var query2 = GetQuery(userName, sessionId);
            query2.Where(Attr.Id, addressId).Set(Attr.IsDefault, true);

            _repository.Update(query1);
            _repository.Update(query2);
        }

        public void Delete(int addressId)
        {
            _repository.Delete(addressId);
        }

        public void Delete(string userName, string sessionId)
        {
            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(sessionId)) return;

            var query = GetQuery(userName, sessionId);

            _repository.Delete(query);
        }

        public IList<AddressInfo> GetAddressInfoList(string userName, string sessionId)
        {
            var list = new List<AddressInfo>();
            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(sessionId)) return list;

            var query = GetQuery(userName, sessionId);
            query.OrderByDesc(Attr.Id);

            return _repository.GetAll(query);
        }

        public AddressInfo GetAddressInfo(int addressId)
        {
            return _repository.Get(addressId);
        }
    }
}
