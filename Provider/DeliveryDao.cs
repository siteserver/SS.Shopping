using System.Collections.Generic;
using System.Data;
using SiteServer.Plugin;
using SS.Shopping.Core;
using SS.Shopping.Model;

namespace SS.Shopping.Provider
{
    public static class DeliveryDao
    {
        public const string TableName = "ss_shopping_delivery";

        public static List<TableColumn> Columns => new List<TableColumn>
        {
            new TableColumn
            {
                AttributeName = nameof(DeliveryInfo.Id),
                DataType = DataType.Integer,
                IsPrimaryKey = true,
                IsIdentity = true
            },
            new TableColumn
            {
                AttributeName = nameof(DeliveryInfo.SiteId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(DeliveryInfo.DeliveryName),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(DeliveryInfo.DeliveryType),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(DeliveryInfo.StartStandards),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(DeliveryInfo.StartFees),
                DataType = DataType.Decimal
            },
            new TableColumn
            {
                AttributeName = nameof(DeliveryInfo.AddStandards),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(DeliveryInfo.AddFees),
                DataType = DataType.Decimal
            },
            new TableColumn
            {
                AttributeName = nameof(DeliveryInfo.Taxis),
                DataType = DataType.Integer
            }
        };

        public static int Insert(DeliveryInfo deliveryInfo)
        {
            string sqlString = $@"INSERT INTO {TableName}
           ({nameof(DeliveryInfo.SiteId)},
            {nameof(DeliveryInfo.DeliveryName)}, 
            {nameof(DeliveryInfo.DeliveryType)}, 
            {nameof(DeliveryInfo.StartStandards)}, 
            {nameof(DeliveryInfo.StartFees)}, 
            {nameof(DeliveryInfo.AddStandards)}, 
            {nameof(DeliveryInfo.AddFees)},
            {nameof(DeliveryInfo.Taxis)})
     VALUES
           (@{nameof(DeliveryInfo.SiteId)}, 
            @{nameof(DeliveryInfo.DeliveryName)}, 
            @{nameof(DeliveryInfo.DeliveryType)}, 
            @{nameof(DeliveryInfo.StartStandards)}, 
            @{nameof(DeliveryInfo.StartFees)}, 
            @{nameof(DeliveryInfo.AddStandards)}, 
            @{nameof(DeliveryInfo.AddFees)},
            @{nameof(DeliveryInfo.Taxis)})";

            var taxis = GetMaxTaxis(deliveryInfo.SiteId) + 1;

            var parameters = new List<IDataParameter>
            {
                Context.DatabaseApi.GetParameter(nameof(deliveryInfo.SiteId), deliveryInfo.SiteId),
                Context.DatabaseApi.GetParameter(nameof(deliveryInfo.DeliveryName), deliveryInfo.DeliveryName),
                Context.DatabaseApi.GetParameter(nameof(deliveryInfo.DeliveryType), deliveryInfo.DeliveryType),
                Context.DatabaseApi.GetParameter(nameof(deliveryInfo.StartStandards), deliveryInfo.StartStandards),
                Context.DatabaseApi.GetParameter(nameof(deliveryInfo.StartFees), deliveryInfo.StartFees),
                Context.DatabaseApi.GetParameter(nameof(deliveryInfo.AddStandards), deliveryInfo.AddStandards),
                Context.DatabaseApi.GetParameter(nameof(deliveryInfo.AddFees), deliveryInfo.AddFees),
                Context.DatabaseApi.GetParameter(nameof(deliveryInfo.Taxis), taxis)
            };

            return Context.DatabaseApi.ExecuteNonQueryAndReturnId(TableName, nameof(DeliveryInfo.Id), Context.ConnectionString, sqlString, parameters.ToArray());
        }

        public static void Update(DeliveryInfo deliveryInfo)
        {
            string sqlString = $@"UPDATE {TableName} SET
                {nameof(DeliveryInfo.SiteId)} = @{nameof(DeliveryInfo.SiteId)}, 
                {nameof(DeliveryInfo.DeliveryName)} = @{nameof(DeliveryInfo.DeliveryName)}, 
                {nameof(DeliveryInfo.DeliveryType)} = @{nameof(DeliveryInfo.DeliveryType)}, 
                {nameof(DeliveryInfo.StartStandards)} = @{nameof(DeliveryInfo.StartStandards)}, 
                {nameof(DeliveryInfo.StartFees)} = @{nameof(DeliveryInfo.StartFees)}, 
                {nameof(DeliveryInfo.AddStandards)} = @{nameof(DeliveryInfo.AddStandards)}, 
                {nameof(DeliveryInfo.AddFees)} = @{nameof(DeliveryInfo.AddFees)}
            WHERE {nameof(DeliveryInfo.Id)} = @{nameof(DeliveryInfo.Id)}";

            var parameters = new List<IDataParameter>
            {
                Context.DatabaseApi.GetParameter(nameof(deliveryInfo.SiteId), deliveryInfo.SiteId),
                Context.DatabaseApi.GetParameter(nameof(deliveryInfo.DeliveryName), deliveryInfo.DeliveryName),
                Context.DatabaseApi.GetParameter(nameof(deliveryInfo.DeliveryType), deliveryInfo.DeliveryType),
                Context.DatabaseApi.GetParameter(nameof(deliveryInfo.StartStandards), deliveryInfo.StartStandards),
                Context.DatabaseApi.GetParameter(nameof(deliveryInfo.StartFees), deliveryInfo.StartFees),
                Context.DatabaseApi.GetParameter(nameof(deliveryInfo.AddStandards), deliveryInfo.AddStandards),
                Context.DatabaseApi.GetParameter(nameof(deliveryInfo.AddFees), deliveryInfo.AddFees),
                Context.DatabaseApi.GetParameter(nameof(deliveryInfo.Id), deliveryInfo.Id)
            };

            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters.ToArray());
        }

        public static void Delete(int deliveryId)
        {
            string sqlString = $"DELETE FROM {TableName} WHERE {nameof(DeliveryInfo.Id)} = @{nameof(DeliveryInfo.Id)}";
            var parameters = new List<IDataParameter>
            {
                Context.DatabaseApi.GetParameter(nameof(DeliveryInfo.Id), deliveryId)
            };

            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters.ToArray());
        }

        public static void DeleteDeliveryNameIsEmpty()
        {
            string sqlString = $"DELETE FROM {TableName} WHERE {nameof(DeliveryInfo.DeliveryName)} IS NULL";

            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString);
        }

        public static List<DeliveryInfo> GetDeliveryInfoList(int siteId)
        {
            var list = new List<DeliveryInfo>();

            string sqlString = $@"SELECT {nameof(DeliveryInfo.Id)}, 
                {nameof(DeliveryInfo.SiteId)}, 
                {nameof(DeliveryInfo.DeliveryName)}, 
                {nameof(DeliveryInfo.DeliveryType)}, 
                {nameof(DeliveryInfo.StartStandards)}, 
                {nameof(DeliveryInfo.StartFees)}, 
                {nameof(DeliveryInfo.AddStandards)}, 
                {nameof(DeliveryInfo.AddFees)}, 
                {nameof(DeliveryInfo.Taxis)}
                FROM {TableName} WHERE {nameof(DeliveryInfo.SiteId)} = {siteId} ORDER BY {nameof(DeliveryInfo.Taxis)} DESC";

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString))
            {
                while (rdr.Read())
                {
                    list.Add(GetDeliveryInfo(rdr));
                }
                rdr.Close();
            }

            return list;
        }

        public static DeliveryInfo GetDeliveryInfo(int deliveryId)
        {
            DeliveryInfo deliveryInfo = null;

            string sqlString = $@"SELECT {nameof(DeliveryInfo.Id)}, 
            {nameof(DeliveryInfo.SiteId)}, 
            {nameof(DeliveryInfo.DeliveryName)}, 
            {nameof(DeliveryInfo.DeliveryType)}, 
            {nameof(DeliveryInfo.StartStandards)}, 
            {nameof(DeliveryInfo.StartFees)}, 
            {nameof(DeliveryInfo.AddStandards)}, 
            {nameof(DeliveryInfo.AddFees)}, 
            {nameof(DeliveryInfo.Taxis)}
            FROM {TableName} WHERE {nameof(DeliveryInfo.Id)} = {deliveryId}";

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString))
            {
                if (rdr.Read())
                {
                    deliveryInfo = GetDeliveryInfo(rdr);
                }
                rdr.Close();
            }

            return deliveryInfo;
        }

        public static bool UpdateTaxisToUp(int siteId, int deliveryId)
        {
            var sqlString = Utils.GetTopSqlString(Context.DatabaseType, TableName, "Id, Taxis", $"WHERE ((Taxis > (SELECT Taxis FROM {TableName} WHERE Id = {deliveryId})) AND SiteId ={siteId}) ORDER BY Taxis", 1);

            var higherId = 0;
            var higherTaxis = 0;

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString))
            {
                if (rdr.Read())
                {
                    higherId = rdr.GetInt32(0);
                    higherTaxis = rdr.GetInt32(1);
                }
                rdr.Close();
            }

            var selectedTaxis = GetTaxis(deliveryId);

            if (higherId != 0)
            {
                SetTaxis(deliveryId, higherTaxis);
                SetTaxis(higherId, selectedTaxis);
                return true;
            }
            return false;
        }

        public static bool UpdateTaxisToDown(int siteId, int deliveryId)
        {
            var sqlString = Utils.GetTopSqlString(Context.DatabaseType, TableName, "Id, Taxis", $"WHERE ((Taxis < (SELECT Taxis FROM {TableName} WHERE (Id = {deliveryId}))) AND SiteId = {siteId}) ORDER BY Taxis DESC", 1);

            var lowerId = 0;
            var lowerTaxis = 0;

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString))
            {
                if (rdr.Read())
                {
                    lowerId = rdr.GetInt32(0);
                    lowerTaxis = rdr.GetInt32(1);
                }
                rdr.Close();
            }

            var selectedTaxis = GetTaxis(deliveryId);

            if (lowerId != 0)
            {
                SetTaxis(deliveryId, lowerTaxis);
                SetTaxis(lowerId, selectedTaxis);
                return true;
            }
            return false;
        }

        private static int GetMaxTaxis(int siteId)
        {
            string sqlString =
                $"SELECT MAX(Taxis) FROM {TableName} WHERE {nameof(DeliveryInfo.SiteId)} = {siteId}";
            return Dao.GetIntResult(sqlString);
        }

        private static int GetTaxis(int deliveryId)
        {
            string sqlString = $"SELECT Taxis FROM {TableName} WHERE ({nameof(DeliveryInfo.Id)} = {deliveryId})";
            return Dao.GetIntResult(sqlString);
        }

        private static void SetTaxis(int deliveryId, int taxis)
        {
            string sqlString = $"UPDATE {TableName} SET Taxis = {taxis} WHERE Id = {deliveryId}";
            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString);
        }

        private static DeliveryInfo GetDeliveryInfo(IDataRecord rdr)
        {
            if (rdr == null) return null;
            
            var deliveryInfo = new DeliveryInfo();

            var i = 0;
            deliveryInfo.Id = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
            i++;
            deliveryInfo.SiteId = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
            i++;
            deliveryInfo.DeliveryName = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            deliveryInfo.DeliveryType = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            deliveryInfo.StartStandards = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
            i++;
            deliveryInfo.StartFees = rdr.IsDBNull(i) ? 0 : rdr.GetDecimal(i);
            i++;
            deliveryInfo.AddStandards = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
            i++;
            deliveryInfo.AddFees = rdr.IsDBNull(i) ? 0 : rdr.GetDecimal(i);
            i++;
            deliveryInfo.Taxis = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);

            return deliveryInfo;
        }
    }
}
