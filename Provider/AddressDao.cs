using System.Collections.Generic;
using System.Data;
using SiteServer.Plugin;
using SS.Shopping.Model;

namespace SS.Shopping.Provider
{
    public class AddressDao
    {
        public const string TableName = "ss_shopping_address";

        private readonly string _connectionString;
        private readonly IDataApi _helper;

        public AddressDao(string connectionString, IDataApi dataApi)
        {
            _connectionString = connectionString;
            _helper = dataApi;
        }

        public static List<TableColumn> Columns => new List<TableColumn>
        {
            new TableColumn
            {
                AttributeName = nameof(AddressInfo.Id),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(AddressInfo.UserName),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(AddressInfo.SessionId),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(AddressInfo.RealName),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(AddressInfo.Mobile),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(AddressInfo.Tel),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(AddressInfo.Location),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(AddressInfo.Address),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(AddressInfo.IsDefault),
                DataType = DataType.Boolean
            }
        };

        public int Insert(AddressInfo addressInfo)
        {
            string sqlString = $@"INSERT INTO {TableName}
           ({nameof(AddressInfo.UserName)}, 
            {nameof(AddressInfo.SessionId)}, 
            {nameof(AddressInfo.RealName)}, 
            {nameof(AddressInfo.Mobile)}, 
            {nameof(AddressInfo.Tel)}, 
            {nameof(AddressInfo.Location)}, 
            {nameof(AddressInfo.Address)}, 
            {nameof(AddressInfo.IsDefault)})
     VALUES
           (@{nameof(AddressInfo.UserName)}, 
            @{nameof(AddressInfo.SessionId)}, 
            @{nameof(AddressInfo.RealName)}, 
            @{nameof(AddressInfo.Mobile)}, 
            @{nameof(AddressInfo.Tel)}, 
            @{nameof(AddressInfo.Location)}, 
            @{nameof(AddressInfo.Address)}, 
            @{nameof(AddressInfo.IsDefault)})";

            var parameters = new List<IDataParameter>
            {
                _helper.GetParameter(nameof(addressInfo.UserName), addressInfo.UserName),
                _helper.GetParameter(nameof(addressInfo.SessionId), addressInfo.SessionId),
                _helper.GetParameter(nameof(addressInfo.RealName), addressInfo.RealName),
                _helper.GetParameter(nameof(addressInfo.Mobile), addressInfo.Mobile),
                _helper.GetParameter(nameof(addressInfo.Tel), addressInfo.Tel),
                _helper.GetParameter(nameof(addressInfo.Location), addressInfo.Location),
                _helper.GetParameter(nameof(addressInfo.Address), addressInfo.Address),
                _helper.GetParameter(nameof(addressInfo.IsDefault), addressInfo.IsDefault)
            };

            return _helper.ExecuteNonQueryAndReturnId(TableName, nameof(AddressInfo.Id), _connectionString, sqlString, parameters.ToArray());
        }

        public void Update(AddressInfo addressInfo)
        {
            string sqlString = $@"UPDATE {TableName} SET
                {nameof(AddressInfo.UserName)} = @{nameof(AddressInfo.UserName)}, 
                {nameof(AddressInfo.SessionId)} = @{nameof(AddressInfo.SessionId)}, 
                {nameof(AddressInfo.RealName)} = @{nameof(AddressInfo.RealName)}, 
                {nameof(AddressInfo.Mobile)} = @{nameof(AddressInfo.Mobile)}, 
                {nameof(AddressInfo.Tel)} = @{nameof(AddressInfo.Tel)}, 
                {nameof(AddressInfo.Location)} = @{nameof(AddressInfo.Location)}, 
                {nameof(AddressInfo.Address)} = @{nameof(AddressInfo.Address)}, 
                {nameof(AddressInfo.IsDefault)} = @{nameof(AddressInfo.IsDefault)}
            WHERE {nameof(AddressInfo.Id)} = @{nameof(AddressInfo.Id)}";

            var parameters = new List<IDataParameter>
            {
                _helper.GetParameter(nameof(addressInfo.UserName), addressInfo.UserName),
                _helper.GetParameter(nameof(addressInfo.SessionId), addressInfo.SessionId),
                _helper.GetParameter(nameof(addressInfo.RealName), addressInfo.RealName),
                _helper.GetParameter(nameof(addressInfo.Mobile), addressInfo.Mobile),
                _helper.GetParameter(nameof(addressInfo.Tel), addressInfo.Tel),
                _helper.GetParameter(nameof(addressInfo.Location), addressInfo.Location),
                _helper.GetParameter(nameof(addressInfo.Address), addressInfo.Address),
                _helper.GetParameter(nameof(addressInfo.IsDefault), addressInfo.IsDefault),
                _helper.GetParameter(nameof(addressInfo.Id), addressInfo.Id)
            };

            _helper.ExecuteNonQuery(_connectionString, sqlString, parameters.ToArray());
        }

        public void SetDefaultToFalse(string userName, string sessionId)
        {
            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(sessionId)) return;

            string sqlString = $"UPDATE {TableName} SET {nameof(AddressInfo.IsDefault)} = @{nameof(AddressInfo.IsDefault)} WHERE";
            var parameters = new List<IDataParameter>{
                _helper.GetParameter(nameof(AddressInfo.IsDefault), false)
            };

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(sessionId))
            {
                sqlString += $" {nameof(AddressInfo.UserName)} = @{nameof(AddressInfo.UserName)} OR {nameof(AddressInfo.SessionId)} = @{nameof(AddressInfo.SessionId)}";
                parameters.Add(_helper.GetParameter(nameof(AddressInfo.UserName), userName));
                parameters.Add(_helper.GetParameter(nameof(AddressInfo.SessionId), sessionId));
            }
            else if (!string.IsNullOrEmpty(userName))
            {
                sqlString += $" {nameof(AddressInfo.UserName)} = @{nameof(AddressInfo.UserName)}";
                parameters.Add(_helper.GetParameter(nameof(AddressInfo.UserName), userName));
            }
            else if (!string.IsNullOrEmpty(sessionId))
            {
                sqlString += $" {nameof(AddressInfo.SessionId)} = @{nameof(AddressInfo.SessionId)}";
                parameters.Add(_helper.GetParameter(nameof(AddressInfo.SessionId), sessionId));
            }
            _helper.ExecuteNonQuery(_connectionString, sqlString, parameters.ToArray());
        }

        public void SetDefault(string userName, string sessionId, int addressId)
        {
            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(sessionId)) return;

            string sqlString = $"UPDATE {TableName} SET {nameof(AddressInfo.IsDefault)} = @{nameof(AddressInfo.IsDefault)} WHERE";
            var parameters = new List<IDataParameter>
            {
                _helper.GetParameter(nameof(AddressInfo.IsDefault), false)
            };
            string sqlString2 = $"UPDATE {TableName} SET {nameof(AddressInfo.IsDefault)} = @{nameof(AddressInfo.IsDefault)} WHERE {nameof(AddressInfo.Id)} = @{nameof(AddressInfo.Id)} AND";
            var parameters2 = new List<IDataParameter>
            {
                _helper.GetParameter(nameof(AddressInfo.IsDefault), true),
                _helper.GetParameter(nameof(AddressInfo.Id), addressId)
            };

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(sessionId))
            {
                sqlString += $" ({nameof(AddressInfo.UserName)} = @{nameof(AddressInfo.UserName)} OR {nameof(AddressInfo.SessionId)} = @{nameof(AddressInfo.SessionId)})";
                parameters.Add(_helper.GetParameter(nameof(AddressInfo.UserName), userName));
                parameters.Add(_helper.GetParameter(nameof(AddressInfo.SessionId), sessionId));

                sqlString2 += $" ({nameof(AddressInfo.UserName)} = @{nameof(AddressInfo.UserName)} OR {nameof(AddressInfo.SessionId)} = @{nameof(AddressInfo.SessionId)})";
                parameters2.Add(_helper.GetParameter(nameof(AddressInfo.UserName), userName));
                parameters2.Add(_helper.GetParameter(nameof(AddressInfo.SessionId), sessionId));
            }
            else if (!string.IsNullOrEmpty(userName))
            {
                sqlString += $" {nameof(AddressInfo.UserName)} = @{nameof(AddressInfo.UserName)}";
                parameters.Add(_helper.GetParameter(nameof(AddressInfo.UserName), userName));
                sqlString2 += $" {nameof(AddressInfo.UserName)} = @{nameof(AddressInfo.UserName)}";
                parameters2.Add(_helper.GetParameter(nameof(AddressInfo.UserName), userName));
            }
            else if (!string.IsNullOrEmpty(sessionId))
            {
                sqlString += $" {nameof(AddressInfo.SessionId)} = @{nameof(AddressInfo.SessionId)}";
                parameters.Add(_helper.GetParameter(nameof(AddressInfo.SessionId), sessionId));
                sqlString2 += $" {nameof(AddressInfo.SessionId)} = @{nameof(AddressInfo.SessionId)}";
                parameters2.Add(_helper.GetParameter(nameof(AddressInfo.SessionId), sessionId));
            }

            _helper.ExecuteNonQuery(_connectionString, sqlString, parameters.ToArray());
            _helper.ExecuteNonQuery(_connectionString, sqlString2, parameters2.ToArray());
        }

        public void Delete(int addressId)
        {
            string sqlString = $"DELETE FROM {TableName} WHERE {nameof(AddressInfo.Id)} = @{nameof(AddressInfo.Id)}";
            var parameters = new List<IDataParameter>
            {
                _helper.GetParameter(nameof(AddressInfo.Id), addressId)
            };

            _helper.ExecuteNonQuery(_connectionString, sqlString, parameters.ToArray());
        }

        public void Delete(string userName, string sessionId)
        {
            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(sessionId)) return;

            string sqlString = $"DELETE FROM {TableName} WHERE";
            var parameters = new List<IDataParameter>();

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(sessionId))
            {
                sqlString += $" {nameof(AddressInfo.UserName)} = @{nameof(AddressInfo.UserName)} OR {nameof(AddressInfo.SessionId)} = @{nameof(AddressInfo.SessionId)}";
                parameters.Add(_helper.GetParameter(nameof(AddressInfo.UserName), userName));
                parameters.Add(_helper.GetParameter(nameof(AddressInfo.SessionId), sessionId));
            }
            else if (!string.IsNullOrEmpty(userName))
            {
                sqlString += $" {nameof(AddressInfo.UserName)} = @{nameof(AddressInfo.UserName)}";
                parameters.Add(_helper.GetParameter(nameof(AddressInfo.UserName), userName));
            }
            else if (!string.IsNullOrEmpty(sessionId))
            {
                sqlString += $" {nameof(AddressInfo.SessionId)} = @{nameof(AddressInfo.SessionId)}";
                parameters.Add(_helper.GetParameter(nameof(AddressInfo.SessionId), sessionId));
            }
            _helper.ExecuteNonQuery(_connectionString, sqlString, parameters.ToArray());
        }

        public List<AddressInfo> GetAddressInfoList(string userName, string sessionId)
        {
            var list = new List<AddressInfo>();
            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(sessionId)) return list;

            string sqlString = $@"SELECT {nameof(AddressInfo.Id)}, 
                {nameof(AddressInfo.UserName)}, 
                {nameof(AddressInfo.SessionId)}, 
                {nameof(AddressInfo.RealName)}, 
                {nameof(AddressInfo.Mobile)}, 
                {nameof(AddressInfo.Tel)}, 
                {nameof(AddressInfo.Location)}, 
                {nameof(AddressInfo.Address)}, 
                {nameof(AddressInfo.IsDefault)}
                FROM {TableName} WHERE";

            var parameters = new List<IDataParameter>();

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(sessionId))
            {
                sqlString += $" {nameof(AddressInfo.UserName)} = @{nameof(AddressInfo.UserName)} OR {nameof(AddressInfo.SessionId)} = @{nameof(AddressInfo.SessionId)}";
                parameters.Add(_helper.GetParameter(nameof(AddressInfo.UserName), userName));
                parameters.Add(_helper.GetParameter(nameof(AddressInfo.SessionId), sessionId));
            }
            else if (!string.IsNullOrEmpty(userName))
            {
                sqlString += $" {nameof(AddressInfo.UserName)} = @{nameof(AddressInfo.UserName)}";
                parameters.Add(_helper.GetParameter(nameof(AddressInfo.UserName), userName));
            }
            else if (!string.IsNullOrEmpty(sessionId))
            {
                sqlString += $" {nameof(AddressInfo.SessionId)} = @{nameof(AddressInfo.SessionId)}";
                parameters.Add(_helper.GetParameter(nameof(AddressInfo.SessionId), sessionId));
            }

            sqlString += $" ORDER BY {nameof(AddressInfo.Id)} DESC";

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters.ToArray()))
            {
                while (rdr.Read())
                {
                    list.Add(GetAddressInfo(rdr));
                }
                rdr.Close();
            }

            return list;
        }

        public AddressInfo GetAddressInfo(int addressId)
        {
            AddressInfo addressInfo = null;

            string sqlString = $@"SELECT {nameof(AddressInfo.Id)}, 
            {nameof(AddressInfo.UserName)}, 
            {nameof(AddressInfo.SessionId)}, 
            {nameof(AddressInfo.RealName)}, 
            {nameof(AddressInfo.Mobile)}, 
            {nameof(AddressInfo.Tel)}, 
            {nameof(AddressInfo.Location)}, 
            {nameof(AddressInfo.Address)}, 
            {nameof(AddressInfo.IsDefault)}
            FROM {TableName} WHERE {nameof(AddressInfo.Id)} = {addressId}";

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString))
            {
                if (rdr.Read())
                {
                    addressInfo = GetAddressInfo(rdr);
                }
                rdr.Close();
            }

            return addressInfo;
        }

        private static AddressInfo GetAddressInfo(IDataRecord rdr)
        {
            if (rdr == null) return null;
            
            var addressInfo = new AddressInfo();

            var i = 0;
            addressInfo.Id = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
            i++;
            addressInfo.UserName = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            addressInfo.SessionId = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            addressInfo.RealName = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            addressInfo.Mobile = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            addressInfo.Tel = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            addressInfo.Location = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            addressInfo.Address = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            addressInfo.IsDefault = !rdr.IsDBNull(i) && rdr.GetBoolean(i);

            return addressInfo;
        }

    }
}
