using System.Collections.Generic;
using System.Data;
using SiteServer.Plugin;
using SS.Shopping.Model;

namespace SS.Shopping.Provider
{
    public class AreaDao
    {
        public const string TableName = "ss_shopping_area";

        private readonly string _connectionString;
        private readonly IDatabaseApi _helper;

        public AreaDao(string connectionString, IDatabaseApi dataApi)
        {
            _connectionString = connectionString;
            _helper = dataApi;
        }

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

        public int Insert(AreaInfo areaInfo)
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
                _helper.GetParameter(nameof(areaInfo.DeliveryId), areaInfo.DeliveryId),
                _helper.GetParameter(nameof(areaInfo.Cities), areaInfo.Cities),
                _helper.GetParameter(nameof(areaInfo.StartStandards), areaInfo.StartStandards),
                _helper.GetParameter(nameof(areaInfo.StartFees), areaInfo.StartFees),
                _helper.GetParameter(nameof(areaInfo.AddStandards), areaInfo.AddStandards),
                _helper.GetParameter(nameof(areaInfo.AddFees), areaInfo.AddFees)
            };

            return _helper.ExecuteNonQueryAndReturnId(TableName, nameof(AreaInfo.Id), _connectionString, sqlString, parameters.ToArray());
        }

        public void Update(AreaInfo areaInfo)
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
                _helper.GetParameter(nameof(areaInfo.DeliveryId), areaInfo.DeliveryId),
                _helper.GetParameter(nameof(areaInfo.Cities), areaInfo.Cities),
                _helper.GetParameter(nameof(areaInfo.StartStandards), areaInfo.StartStandards),
                _helper.GetParameter(nameof(areaInfo.StartFees), areaInfo.StartFees),
                _helper.GetParameter(nameof(areaInfo.AddStandards), areaInfo.AddStandards),
                _helper.GetParameter(nameof(areaInfo.AddFees), areaInfo.AddFees),
                _helper.GetParameter(nameof(areaInfo.Id), areaInfo.Id)
            };

            _helper.ExecuteNonQuery(_connectionString, sqlString, parameters.ToArray());
        }

        public void Delete(int areaId)
        {
            string sqlString = $"DELETE FROM {TableName} WHERE {nameof(AreaInfo.Id)} = @{nameof(AreaInfo.Id)}";
            var parameters = new List<IDataParameter>
            {
                _helper.GetParameter(nameof(AreaInfo.Id), areaId)
            };

            _helper.ExecuteNonQuery(_connectionString, sqlString, parameters.ToArray());
        }

        public List<AreaInfo> GetAreaInfoList(int deliveryId)
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

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString))
            {
                while (rdr.Read())
                {
                    list.Add(GetAreaInfo(rdr));
                }
                rdr.Close();
            }

            return list;
        }

        public AreaInfo GetAreaInfo(int areaId)
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

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString))
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
