using System.Collections.Generic;
using Datory;
using SiteServer.Plugin;
using SS.Shopping.Core.Model;

namespace SS.Shopping.Core.Provider
{
    public class AreaRepository
    {
        private readonly Repository<AreaInfo> _repository;
        public AreaRepository()
        {
            _repository = new Repository<AreaInfo>(Context.Environment.DatabaseType, Context.Environment.ConnectionString);
        }

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        private static class Attr
        {
            public const string Id = nameof(AreaInfo.Id);
            public const string DeliveryId = nameof(AreaInfo.DeliveryId);
            public const string Cities = nameof(AreaInfo.Cities);
            public const string StartStandards = nameof(AreaInfo.StartStandards);
            public const string StartFees = nameof(AreaInfo.StartFees);
            public const string AddStandards = nameof(AreaInfo.AddStandards);
            public const string AddFees = nameof(AreaInfo.AddFees);
        }

        public int Insert(AreaInfo areaInfo)
        {
            return _repository.Insert(areaInfo);
        }

        public void Update(AreaInfo areaInfo)
        {
            _repository.Update(areaInfo);
        }

        public void Delete(int areaId)
        {
            _repository.Delete(areaId);
        }

        public IList<AreaInfo> GetAreaInfoList(int deliveryId)
        {
            return _repository.GetAll(Q
                .Where(Attr.DeliveryId, deliveryId)
                .OrderBy(Attr.Id)
            );
        }

        public AreaInfo GetAreaInfo(int areaId)
        {
            return _repository.Get(areaId);
        }
    }
}
