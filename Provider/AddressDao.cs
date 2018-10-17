using System.Collections.Generic;
using System.Data;
using SiteServer.Plugin;
using SS.Shopping.Model;

namespace SS.Shopping.Provider
{
    public static class AddressDao
    {
        public const string TableName = "ss_shopping_address";

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
                AttributeName = nameof(AddressInfo.ZipCode),
                DataType = DataType.VarChar,
                DataLength = 50
            },
            new TableColumn
            {
                AttributeName = nameof(AddressInfo.IsDefault),
                DataType = DataType.Boolean
            }
        };

        public static int Insert(AddressInfo addressInfo)
        {
            string sqlString = $@"INSERT INTO {TableName}
           ({nameof(AddressInfo.UserName)}, 
            {nameof(AddressInfo.SessionId)}, 
            {nameof(AddressInfo.RealName)}, 
            {nameof(AddressInfo.Mobile)}, 
            {nameof(AddressInfo.Tel)}, 
            {nameof(AddressInfo.Location)}, 
            {nameof(AddressInfo.Address)}, 
            {nameof(AddressInfo.ZipCode)}, 
            {nameof(AddressInfo.IsDefault)})
     VALUES
           (@{nameof(AddressInfo.UserName)}, 
            @{nameof(AddressInfo.SessionId)}, 
            @{nameof(AddressInfo.RealName)}, 
            @{nameof(AddressInfo.Mobile)}, 
            @{nameof(AddressInfo.Tel)}, 
            @{nameof(AddressInfo.Location)}, 
            @{nameof(AddressInfo.Address)}, 
            @{nameof(AddressInfo.ZipCode)}, 
            @{nameof(AddressInfo.IsDefault)})";

            var parameters = new List<IDataParameter>
            {
                Context.DatabaseApi.GetParameter(nameof(addressInfo.UserName), addressInfo.UserName),
                Context.DatabaseApi.GetParameter(nameof(addressInfo.SessionId), addressInfo.SessionId),
                Context.DatabaseApi.GetParameter(nameof(addressInfo.RealName), addressInfo.RealName),
                Context.DatabaseApi.GetParameter(nameof(addressInfo.Mobile), addressInfo.Mobile),
                Context.DatabaseApi.GetParameter(nameof(addressInfo.Tel), addressInfo.Tel),
                Context.DatabaseApi.GetParameter(nameof(addressInfo.Location), addressInfo.Location),
                Context.DatabaseApi.GetParameter(nameof(addressInfo.Address), addressInfo.Address),
                Context.DatabaseApi.GetParameter(nameof(addressInfo.ZipCode), addressInfo.ZipCode),
                Context.DatabaseApi.GetParameter(nameof(addressInfo.IsDefault), addressInfo.IsDefault)
            };

            return Context.DatabaseApi.ExecuteNonQueryAndReturnId(TableName, nameof(AddressInfo.Id), Context.ConnectionString, sqlString, parameters.ToArray());
        }

        public static void Update(AddressInfo addressInfo)
        {
            string sqlString = $@"UPDATE {TableName} SET
                {nameof(AddressInfo.UserName)} = @{nameof(AddressInfo.UserName)}, 
                {nameof(AddressInfo.SessionId)} = @{nameof(AddressInfo.SessionId)}, 
                {nameof(AddressInfo.RealName)} = @{nameof(AddressInfo.RealName)}, 
                {nameof(AddressInfo.Mobile)} = @{nameof(AddressInfo.Mobile)}, 
                {nameof(AddressInfo.Tel)} = @{nameof(AddressInfo.Tel)}, 
                {nameof(AddressInfo.Location)} = @{nameof(AddressInfo.Location)}, 
                {nameof(AddressInfo.Address)} = @{nameof(AddressInfo.Address)}, 
                {nameof(AddressInfo.ZipCode)} = @{nameof(AddressInfo.ZipCode)}, 
                {nameof(AddressInfo.IsDefault)} = @{nameof(AddressInfo.IsDefault)}
            WHERE {nameof(AddressInfo.Id)} = @{nameof(AddressInfo.Id)}";

            var parameters = new List<IDataParameter>
            {
                Context.DatabaseApi.GetParameter(nameof(addressInfo.UserName), addressInfo.UserName),
                Context.DatabaseApi.GetParameter(nameof(addressInfo.SessionId), addressInfo.SessionId),
                Context.DatabaseApi.GetParameter(nameof(addressInfo.RealName), addressInfo.RealName),
                Context.DatabaseApi.GetParameter(nameof(addressInfo.Mobile), addressInfo.Mobile),
                Context.DatabaseApi.GetParameter(nameof(addressInfo.Tel), addressInfo.Tel),
                Context.DatabaseApi.GetParameter(nameof(addressInfo.Location), addressInfo.Location),
                Context.DatabaseApi.GetParameter(nameof(addressInfo.Address), addressInfo.Address),
                Context.DatabaseApi.GetParameter(nameof(addressInfo.ZipCode), addressInfo.ZipCode),
                Context.DatabaseApi.GetParameter(nameof(addressInfo.IsDefault), addressInfo.IsDefault),
                Context.DatabaseApi.GetParameter(nameof(addressInfo.Id), addressInfo.Id)
            };

            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters.ToArray());
        }

        public static void SetDefaultToFalse(string userName, string sessionId)
        {
            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(sessionId)) return;

            string sqlString = $"UPDATE {TableName} SET {nameof(AddressInfo.IsDefault)} = @{nameof(AddressInfo.IsDefault)} WHERE";
            var parameters = new List<IDataParameter>{
                Context.DatabaseApi.GetParameter(nameof(AddressInfo.IsDefault), false)
            };

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(sessionId))
            {
                sqlString += $" {nameof(AddressInfo.UserName)} = @{nameof(AddressInfo.UserName)} OR {nameof(AddressInfo.SessionId)} = @{nameof(AddressInfo.SessionId)}";
                parameters.Add(Context.DatabaseApi.GetParameter(nameof(AddressInfo.UserName), userName));
                parameters.Add(Context.DatabaseApi.GetParameter(nameof(AddressInfo.SessionId), sessionId));
            }
            else if (!string.IsNullOrEmpty(userName))
            {
                sqlString += $" {nameof(AddressInfo.UserName)} = @{nameof(AddressInfo.UserName)}";
                parameters.Add(Context.DatabaseApi.GetParameter(nameof(AddressInfo.UserName), userName));
            }
            else if (!string.IsNullOrEmpty(sessionId))
            {
                sqlString += $" {nameof(AddressInfo.SessionId)} = @{nameof(AddressInfo.SessionId)}";
                parameters.Add(Context.DatabaseApi.GetParameter(nameof(AddressInfo.SessionId), sessionId));
            }
            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters.ToArray());
        }

        public static void SetDefault(string userName, string sessionId, int addressId)
        {
            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(sessionId)) return;

            string sqlString = $"UPDATE {TableName} SET {nameof(AddressInfo.IsDefault)} = @{nameof(AddressInfo.IsDefault)} WHERE";
            var parameters = new List<IDataParameter>
            {
                Context.DatabaseApi.GetParameter(nameof(AddressInfo.IsDefault), false)
            };
            string sqlString2 = $"UPDATE {TableName} SET {nameof(AddressInfo.IsDefault)} = @{nameof(AddressInfo.IsDefault)} WHERE {nameof(AddressInfo.Id)} = @{nameof(AddressInfo.Id)} AND";
            var parameters2 = new List<IDataParameter>
            {
                Context.DatabaseApi.GetParameter(nameof(AddressInfo.IsDefault), true),
                Context.DatabaseApi.GetParameter(nameof(AddressInfo.Id), addressId)
            };

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(sessionId))
            {
                sqlString += $" ({nameof(AddressInfo.UserName)} = @{nameof(AddressInfo.UserName)} OR {nameof(AddressInfo.SessionId)} = @{nameof(AddressInfo.SessionId)})";
                parameters.Add(Context.DatabaseApi.GetParameter(nameof(AddressInfo.UserName), userName));
                parameters.Add(Context.DatabaseApi.GetParameter(nameof(AddressInfo.SessionId), sessionId));

                sqlString2 += $" ({nameof(AddressInfo.UserName)} = @{nameof(AddressInfo.UserName)} OR {nameof(AddressInfo.SessionId)} = @{nameof(AddressInfo.SessionId)})";
                parameters2.Add(Context.DatabaseApi.GetParameter(nameof(AddressInfo.UserName), userName));
                parameters2.Add(Context.DatabaseApi.GetParameter(nameof(AddressInfo.SessionId), sessionId));
            }
            else if (!string.IsNullOrEmpty(userName))
            {
                sqlString += $" {nameof(AddressInfo.UserName)} = @{nameof(AddressInfo.UserName)}";
                parameters.Add(Context.DatabaseApi.GetParameter(nameof(AddressInfo.UserName), userName));
                sqlString2 += $" {nameof(AddressInfo.UserName)} = @{nameof(AddressInfo.UserName)}";
                parameters2.Add(Context.DatabaseApi.GetParameter(nameof(AddressInfo.UserName), userName));
            }
            else if (!string.IsNullOrEmpty(sessionId))
            {
                sqlString += $" {nameof(AddressInfo.SessionId)} = @{nameof(AddressInfo.SessionId)}";
                parameters.Add(Context.DatabaseApi.GetParameter(nameof(AddressInfo.SessionId), sessionId));
                sqlString2 += $" {nameof(AddressInfo.SessionId)} = @{nameof(AddressInfo.SessionId)}";
                parameters2.Add(Context.DatabaseApi.GetParameter(nameof(AddressInfo.SessionId), sessionId));
            }

            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters.ToArray());
            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString2, parameters2.ToArray());
        }

        public static void Delete(int addressId)
        {
            string sqlString = $"DELETE FROM {TableName} WHERE {nameof(AddressInfo.Id)} = @{nameof(AddressInfo.Id)}";
            var parameters = new List<IDataParameter>
            {
                Context.DatabaseApi.GetParameter(nameof(AddressInfo.Id), addressId)
            };

            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters.ToArray());
        }

        public static void Delete(string userName, string sessionId)
        {
            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(sessionId)) return;

            string sqlString = $"DELETE FROM {TableName} WHERE";
            var parameters = new List<IDataParameter>();

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(sessionId))
            {
                sqlString += $" {nameof(AddressInfo.UserName)} = @{nameof(AddressInfo.UserName)} OR {nameof(AddressInfo.SessionId)} = @{nameof(AddressInfo.SessionId)}";
                parameters.Add(Context.DatabaseApi.GetParameter(nameof(AddressInfo.UserName), userName));
                parameters.Add(Context.DatabaseApi.GetParameter(nameof(AddressInfo.SessionId), sessionId));
            }
            else if (!string.IsNullOrEmpty(userName))
            {
                sqlString += $" {nameof(AddressInfo.UserName)} = @{nameof(AddressInfo.UserName)}";
                parameters.Add(Context.DatabaseApi.GetParameter(nameof(AddressInfo.UserName), userName));
            }
            else if (!string.IsNullOrEmpty(sessionId))
            {
                sqlString += $" {nameof(AddressInfo.SessionId)} = @{nameof(AddressInfo.SessionId)}";
                parameters.Add(Context.DatabaseApi.GetParameter(nameof(AddressInfo.SessionId), sessionId));
            }
            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters.ToArray());
        }

        public static List<AddressInfo> GetAddressInfoList(string userName, string sessionId)
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
                {nameof(AddressInfo.ZipCode)}, 
                {nameof(AddressInfo.IsDefault)}
                FROM {TableName} WHERE";

            var parameters = new List<IDataParameter>();

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(sessionId))
            {
                sqlString += $" {nameof(AddressInfo.UserName)} = @{nameof(AddressInfo.UserName)} OR {nameof(AddressInfo.SessionId)} = @{nameof(AddressInfo.SessionId)}";
                parameters.Add(Context.DatabaseApi.GetParameter(nameof(AddressInfo.UserName), userName));
                parameters.Add(Context.DatabaseApi.GetParameter(nameof(AddressInfo.SessionId), sessionId));
            }
            else if (!string.IsNullOrEmpty(userName))
            {
                sqlString += $" {nameof(AddressInfo.UserName)} = @{nameof(AddressInfo.UserName)}";
                parameters.Add(Context.DatabaseApi.GetParameter(nameof(AddressInfo.UserName), userName));
            }
            else if (!string.IsNullOrEmpty(sessionId))
            {
                sqlString += $" {nameof(AddressInfo.SessionId)} = @{nameof(AddressInfo.SessionId)}";
                parameters.Add(Context.DatabaseApi.GetParameter(nameof(AddressInfo.SessionId), sessionId));
            }

            sqlString += $" ORDER BY {nameof(AddressInfo.Id)} DESC";

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters.ToArray()))
            {
                while (rdr.Read())
                {
                    list.Add(GetAddressInfo(rdr));
                }
                rdr.Close();
            }

            return list;
        }

        public static AddressInfo GetAddressInfo(int addressId)
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
            {nameof(AddressInfo.ZipCode)}, 
            {nameof(AddressInfo.IsDefault)}
            FROM {TableName} WHERE {nameof(AddressInfo.Id)} = {addressId}";

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString))
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
            addressInfo.ZipCode = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            addressInfo.IsDefault = !rdr.IsDBNull(i) && rdr.GetBoolean(i);

            return addressInfo;
        }
    }
}
