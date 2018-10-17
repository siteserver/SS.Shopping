using System.Collections.Generic;
using System.Data;
using SiteServer.Plugin;
using SS.Shopping.Model;

namespace SS.Shopping.Provider
{
    public static class AreaDao
    {
        public const string TableName = "ss_shopping_area";

        public static List<TableColumn> Columns => new List<TableColumn>
        {
            new TableColumn
            {
                AttributeName = nameof(AreaInfo.Id),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(AreaInfo.DeliveryId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(AreaInfo.Cities),
                DataType = DataType.VarChar,
                DataLength = 2000
            },
            new TableColumn
            {
                AttributeName = nameof(AreaInfo.StartStandards),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(AreaInfo.StartFees),
                DataType = DataType.Decimal
            },
            new TableColumn
            {
                AttributeName = nameof(AreaInfo.AddStandards),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(AreaInfo.AddFees),
                DataType = DataType.Decimal
            }
        };

        public static int Insert(AreaInfo areaInfo)
        {
            string sqlString = $@"INSERT INTO {TableName}
           ({nameof(AreaInfo.DeliveryId)}, 
            {nameof(AreaInfo.Cities)}, 
            {nameof(AreaInfo.StartStandards)}, 
            {nameof(AreaInfo.StartFees)}, 
            {nameof(AreaInfo.AddStandards)}, 
            {nameof(AreaInfo.AddFees)})
     VALUES
           (@{nameof(AreaInfo.DeliveryId)}, 
            @{nameof(AreaInfo.Cities)}, 
            @{nameof(AreaInfo.StartStandards)}, 
            @{nameof(AreaInfo.StartFees)}, 
            @{nameof(AreaInfo.AddStandards)}, 
            @{nameof(AreaInfo.AddFees)})";

            var parameters = new List<IDataParameter>
            {
                Context.DatabaseApi.GetParameter(nameof(areaInfo.DeliveryId), areaInfo.DeliveryId),
                Context.DatabaseApi.GetParameter(nameof(areaInfo.Cities), areaInfo.Cities),
                Context.DatabaseApi.GetParameter(nameof(areaInfo.StartStandards), areaInfo.StartStandards),
                Context.DatabaseApi.GetParameter(nameof(areaInfo.StartFees), areaInfo.StartFees),
                Context.DatabaseApi.GetParameter(nameof(areaInfo.AddStandards), areaInfo.AddStandards),
                Context.DatabaseApi.GetParameter(nameof(areaInfo.AddFees), areaInfo.AddFees)
            };

            return Context.DatabaseApi.ExecuteNonQueryAndReturnId(TableName, nameof(AreaInfo.Id), Context.ConnectionString, sqlString, parameters.ToArray());
        }

        public static void Update(AreaInfo areaInfo)
        {
            string sqlString = $@"UPDATE {TableName} SET
                {nameof(AreaInfo.DeliveryId)} = @{nameof(AreaInfo.DeliveryId)}, 
                {nameof(AreaInfo.Cities)} = @{nameof(AreaInfo.Cities)}, 
                {nameof(AreaInfo.StartStandards)} = @{nameof(AreaInfo.StartStandards)}, 
                {nameof(AreaInfo.StartFees)} = @{nameof(AreaInfo.StartFees)}, 
                {nameof(AreaInfo.AddStandards)} = @{nameof(AreaInfo.AddStandards)}, 
                {nameof(AreaInfo.AddFees)} = @{nameof(AreaInfo.AddFees)}
            WHERE {nameof(AreaInfo.Id)} = @{nameof(AreaInfo.Id)}";

            var parameters = new List<IDataParameter>
            {
                Context.DatabaseApi.GetParameter(nameof(areaInfo.DeliveryId), areaInfo.DeliveryId),
                Context.DatabaseApi.GetParameter(nameof(areaInfo.Cities), areaInfo.Cities),
                Context.DatabaseApi.GetParameter(nameof(areaInfo.StartStandards), areaInfo.StartStandards),
                Context.DatabaseApi.GetParameter(nameof(areaInfo.StartFees), areaInfo.StartFees),
                Context.DatabaseApi.GetParameter(nameof(areaInfo.AddStandards), areaInfo.AddStandards),
                Context.DatabaseApi.GetParameter(nameof(areaInfo.AddFees), areaInfo.AddFees),
                Context.DatabaseApi.GetParameter(nameof(areaInfo.Id), areaInfo.Id)
            };

            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters.ToArray());
        }

        public static void Delete(int areaId)
        {
            string sqlString = $"DELETE FROM {TableName} WHERE {nameof(AreaInfo.Id)} = @{nameof(AreaInfo.Id)}";
            var parameters = new List<IDataParameter>
            {
                Context.DatabaseApi.GetParameter(nameof(AreaInfo.Id), areaId)
            };

            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters.ToArray());
        }

        public static List<AreaInfo> GetAreaInfoList(int deliveryId)
        {
            var list = new List<AreaInfo>();

            string sqlString = $@"SELECT {nameof(AreaInfo.Id)}, 
                {nameof(AreaInfo.DeliveryId)}, 
                {nameof(AreaInfo.Cities)}, 
                {nameof(AreaInfo.StartStandards)}, 
                {nameof(AreaInfo.StartFees)}, 
                {nameof(AreaInfo.AddStandards)}, 
                {nameof(AreaInfo.AddFees)}
                FROM {TableName} WHERE {nameof(AreaInfo.DeliveryId)} = {deliveryId} ORDER BY {nameof(AreaInfo.Id)}";

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString))
            {
                while (rdr.Read())
                {
                    list.Add(GetAreaInfo(rdr));
                }
                rdr.Close();
            }

            return list;
        }

        public static AreaInfo GetAreaInfo(int areaId)
        {
            AreaInfo areaInfo = null;

            string sqlString = $@"SELECT {nameof(AreaInfo.Id)}, 
            {nameof(AreaInfo.DeliveryId)}, 
            {nameof(AreaInfo.Cities)}, 
            {nameof(AreaInfo.StartStandards)}, 
            {nameof(AreaInfo.StartFees)}, 
            {nameof(AreaInfo.AddStandards)}, 
            {nameof(AreaInfo.AddFees)}
            FROM {TableName} WHERE {nameof(AreaInfo.Id)} = {areaId}";

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString))
            {
                if (rdr.Read())
                {
                    areaInfo = GetAreaInfo(rdr);
                }
                rdr.Close();
            }

            return areaInfo;
        }

        private static AreaInfo GetAreaInfo(IDataRecord rdr)
        {
            if (rdr == null) return null;
            
            var areaInfo = new AreaInfo();

            var i = 0;
            areaInfo.Id = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
            i++;
            areaInfo.DeliveryId = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
            i++;
            areaInfo.Cities = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            areaInfo.StartStandards = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
            i++;
            areaInfo.StartFees = rdr.IsDBNull(i) ? 0 : rdr.GetDecimal(i);
            i++;
            areaInfo.AddStandards = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
            i++;
            areaInfo.AddFees = rdr.IsDBNull(i) ? 0 : rdr.GetDecimal(i);

            return areaInfo;
        }
    }
}
