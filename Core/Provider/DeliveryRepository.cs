using System.Collections.Generic;
using Datory;
using SiteServer.Plugin;
using SS.Shopping.Core.Model;

namespace SS.Shopping.Core.Provider
{
    public class DeliveryRepository
    {
        private readonly Repository<DeliveryInfo> _repository;
        public DeliveryRepository()
        {
            _repository = new Repository<DeliveryInfo>(Context.Environment.DatabaseType, Context.Environment.ConnectionString);
        }

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        private static class Attr
        {
            public const string Id = nameof(DeliveryInfo.Id);
            public const string SiteId = nameof(DeliveryInfo.SiteId);
            public const string DeliveryName = nameof(DeliveryInfo.DeliveryName);
            public const string DeliveryType = nameof(DeliveryInfo.DeliveryType);
            public const string StartStandards = nameof(DeliveryInfo.StartStandards);
            public const string StartFees = nameof(DeliveryInfo.StartFees);
            public const string AddStandards = nameof(DeliveryInfo.AddStandards);
            public const string AddFees = nameof(DeliveryInfo.AddFees);
            public const string Taxis = nameof(DeliveryInfo.Taxis);
        }

        public int Insert(DeliveryInfo deliveryInfo)
        {
            deliveryInfo.Taxis = GetMaxTaxis(deliveryInfo.SiteId) + 1;

            return _repository.Insert(deliveryInfo);
        }

        public void Update(DeliveryInfo deliveryInfo)
        {
            _repository.Update(deliveryInfo);
        }

        public void Delete(int deliveryId)
        {
            _repository.Delete(deliveryId);
        }

        public void DeleteDeliveryNameIsEmpty()
        {
            _repository.Delete(Q.WhereNull(Attr.DeliveryName));
        }

        public IList<DeliveryInfo> GetDeliveryInfoList(int siteId)
        {
            return _repository.GetAll(Q
                .Where(Attr.SiteId, siteId)
                .OrderByDesc(Attr.Taxis)
            );
        }

        public DeliveryInfo GetDeliveryInfo(int deliveryId)
        {
            return _repository.Get(deliveryId);
        }

        private int GetMaxTaxis(int siteId)
        {
            return _repository.Max(Attr.Taxis, Q
                       .Where(Attr.SiteId, siteId)
                   ) ?? 0;
        }

        public void TaxisDown(int siteId, int id)
        {
            var itemInfo = GetDeliveryInfo(id);
            if (itemInfo == null) return;

            var higherFieldInfo = _repository.Get(Q
                .Where(Attr.SiteId, siteId)
                .Where(Attr.Taxis, ">", itemInfo.Taxis)
                .OrderBy(Attr.Taxis)
            );

            if (higherFieldInfo != null)
            {
                var higherId = higherFieldInfo.Id;
                var higherTaxis = higherFieldInfo.Taxis;

                SetTaxis(siteId, id, higherTaxis);
                SetTaxis(siteId, higherId, itemInfo.Taxis);
            }
        }

        public void TaxisUp(int siteId, int id)
        {
            var itemInfo = GetDeliveryInfo(id);
            if (itemInfo == null) return;

            var lowerInfo = _repository.Get(Q
                .Where(Attr.SiteId, siteId)
                .Where(Attr.Taxis, "<", itemInfo.Taxis)
                .OrderByDesc(Attr.Taxis)
            );

            if (lowerInfo != null)
            {
                var lowerId = lowerInfo.Id;
                var lowerTaxis = lowerInfo.Taxis;

                SetTaxis(siteId, id, lowerTaxis);
                SetTaxis(siteId, lowerId, itemInfo.Taxis);
            }
        }

        private void SetTaxis(int siteId, int itemId, int taxis)
        {
            _repository.Update(Q
                .Set(Attr.Taxis, taxis)
                .Where(Attr.Id, itemId)
            );
        }
    }
}
