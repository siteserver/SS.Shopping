using System;
using System.Collections.Generic;
using System.Data;
using SiteServer.Plugin;
using SS.Shopping.Model;

namespace SS.Shopping.Provider
{
    public class OrderDao
    {
        public const string TableName = "ss_shopping_order";

        private readonly string _connectionString;
        private readonly IDatabaseApi _helper;

        public OrderDao(string connectionString, IDatabaseApi dataApi)
        {
            _connectionString = connectionString;
            _helper = dataApi;
        }

        public static List<TableColumn> Columns => new List<TableColumn>
        {
            new TableColumn
            {
                AttributeName = nameof(OrderInfo.Id),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(OrderInfo.SiteId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(OrderInfo.Guid),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(OrderInfo.UserName),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(OrderInfo.SessionId),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(OrderInfo.RealName),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(OrderInfo.Mobile),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(OrderInfo.Tel),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(OrderInfo.Location),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(OrderInfo.Address),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(OrderInfo.ZipCode),
                DataType = DataType.VarChar,
                DataLength = 50
            },
            new TableColumn
            {
                AttributeName = nameof(OrderInfo.Message),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(OrderInfo.Channel),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(OrderInfo.TotalFee),
                DataType = DataType.Decimal
            },
            new TableColumn
            {
                AttributeName = nameof(OrderInfo.ExpressCost),
                DataType = DataType.Decimal
            },
            new TableColumn
            {
                AttributeName = nameof(OrderInfo.TotalCount),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(OrderInfo.IsPaied),
                DataType = DataType.Boolean
            },
            new TableColumn
            {
                AttributeName = nameof(OrderInfo.State),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(OrderInfo.AddDate),
                DataType = DataType.DateTime
            }
        };

        public int Insert(OrderInfo orderInfo)
        {
            string sqlString = $@"INSERT INTO {TableName}
           ({nameof(OrderInfo.SiteId)}, 
            {nameof(OrderInfo.Guid)}, 
            {nameof(OrderInfo.UserName)}, 
            {nameof(OrderInfo.SessionId)}, 
            {nameof(OrderInfo.RealName)}, 
            {nameof(OrderInfo.Mobile)}, 
            {nameof(OrderInfo.Tel)}, 
            {nameof(OrderInfo.Location)}, 
            {nameof(OrderInfo.Address)}, 
            {nameof(OrderInfo.ZipCode)}, 
            {nameof(OrderInfo.Message)}, 
            {nameof(OrderInfo.Channel)}, 
            {nameof(OrderInfo.TotalFee)}, 
            {nameof(OrderInfo.ExpressCost)}, 
            {nameof(OrderInfo.TotalCount)}, 
            {nameof(OrderInfo.IsPaied)}, 
            {nameof(OrderInfo.State)}, 
            {nameof(OrderInfo.AddDate)})
     VALUES
           (@{nameof(OrderInfo.SiteId)}, 
            @{nameof(OrderInfo.Guid)}, 
            @{nameof(OrderInfo.UserName)}, 
            @{nameof(OrderInfo.SessionId)}, 
            @{nameof(OrderInfo.RealName)}, 
            @{nameof(OrderInfo.Mobile)}, 
            @{nameof(OrderInfo.Tel)}, 
            @{nameof(OrderInfo.Location)}, 
            @{nameof(OrderInfo.Address)}, 
            @{nameof(OrderInfo.ZipCode)}, 
            @{nameof(OrderInfo.Message)}, 
            @{nameof(OrderInfo.Channel)}, 
            @{nameof(OrderInfo.TotalFee)}, 
            @{nameof(OrderInfo.ExpressCost)}, 
            @{nameof(OrderInfo.TotalCount)}, 
            @{nameof(OrderInfo.IsPaied)}, 
            @{nameof(OrderInfo.State)}, 
            @{nameof(OrderInfo.AddDate)})";

            var parameters = new List<IDataParameter>
            {
                _helper.GetParameter(nameof(orderInfo.SiteId), orderInfo.SiteId),
                _helper.GetParameter(nameof(orderInfo.Guid), orderInfo.Guid),
                _helper.GetParameter(nameof(orderInfo.UserName), orderInfo.UserName),
                _helper.GetParameter(nameof(orderInfo.SessionId), orderInfo.SessionId),
                _helper.GetParameter(nameof(orderInfo.RealName), orderInfo.RealName),
                _helper.GetParameter(nameof(orderInfo.Mobile), orderInfo.Mobile),
                _helper.GetParameter(nameof(orderInfo.Tel), orderInfo.Tel),
                _helper.GetParameter(nameof(orderInfo.Location), orderInfo.Location),
                _helper.GetParameter(nameof(orderInfo.Address), orderInfo.Address),
                _helper.GetParameter(nameof(orderInfo.ZipCode), orderInfo.ZipCode),
                _helper.GetParameter(nameof(orderInfo.Message), orderInfo.Message),
                _helper.GetParameter(nameof(orderInfo.Channel), orderInfo.Channel),
                _helper.GetParameter(nameof(orderInfo.TotalFee), orderInfo.TotalFee),
                _helper.GetParameter(nameof(orderInfo.ExpressCost), orderInfo.ExpressCost),
                _helper.GetParameter(nameof(orderInfo.TotalCount), orderInfo.TotalCount),
                _helper.GetParameter(nameof(orderInfo.IsPaied), orderInfo.IsPaied),
                _helper.GetParameter(nameof(orderInfo.State), orderInfo.State),
                _helper.GetParameter(nameof(orderInfo.AddDate), orderInfo.AddDate)
            };

            return _helper.ExecuteNonQueryAndReturnId(TableName, nameof(OrderInfo.Id), _connectionString, sqlString, parameters.ToArray());
        }

        //public void Update(OrderInfo orderInfo)
        //{
        //    string sqlString = $@"UPDATE {TableName} SET
        //        {nameof(OrderInfo.SiteId)} = @{nameof(OrderInfo.SiteId)}, 
        //        {nameof(OrderInfo.UserName)} = @{nameof(OrderInfo.UserName)}, 
        //        {nameof(OrderInfo.SessionId)} = @{nameof(OrderInfo.SessionId)}, 
        //        {nameof(OrderInfo.RealName)} = @{nameof(OrderInfo.RealName)}, 
        //        {nameof(OrderInfo.Mobile)} = @{nameof(OrderInfo.Mobile)}, 
        //        {nameof(OrderInfo.Tel)} = @{nameof(OrderInfo.Tel)}, 
        //        {nameof(OrderInfo.Location)} = @{nameof(OrderInfo.Location)}, 
        //        {nameof(OrderInfo.Address)} = @{nameof(OrderInfo.Address)}, 
        //        {nameof(OrderInfo.Message)} = @{nameof(OrderInfo.Message)}, 
        //        {nameof(OrderInfo.Channel)} = @{nameof(OrderInfo.Channel)}, 
        //        {nameof(OrderInfo.TotalFee)} = @{nameof(OrderInfo.TotalFee)}, 
        //        {nameof(OrderInfo.ExpressCost)} = @{nameof(OrderInfo.ExpressCost)}, 
        //        {nameof(OrderInfo.TotalCount)} = @{nameof(OrderInfo.TotalCount)}, 
        //        {nameof(OrderInfo.IsPaied)} = @{nameof(OrderInfo.IsPaied)}, 
        //        {nameof(OrderInfo.State)} = @{nameof(OrderInfo.State)}, 
        //        {nameof(OrderInfo.AddDate)} = @{nameof(OrderInfo.AddDate)}
        //    WHERE {nameof(OrderInfo.Id)} = @{nameof(OrderInfo.Id)}";

        //    var parameters = new List<IDataParameter>
        //    {
        //        _helper.GetParameter(nameof(orderInfo.SiteId), orderInfo.SiteId),
        //        _helper.GetParameter(nameof(orderInfo.UserName), orderInfo.UserName),
        //        _helper.GetParameter(nameof(orderInfo.SessionId), orderInfo.SessionId),
        //        _helper.GetParameter(nameof(orderInfo.RealName), orderInfo.RealName),
        //        _helper.GetParameter(nameof(orderInfo.Mobile), orderInfo.Mobile),
        //        _helper.GetParameter(nameof(orderInfo.Tel), orderInfo.Tel),
        //        _helper.GetParameter(nameof(orderInfo.Location), orderInfo.Location),
        //        _helper.GetParameter(nameof(orderInfo.Address), orderInfo.Address),
        //        _helper.GetParameter(nameof(orderInfo.Message), orderInfo.Message),
        //        _helper.GetParameter(nameof(orderInfo.Channel), orderInfo.Channel),
        //        _helper.GetParameter(nameof(orderInfo.TotalFee), orderInfo.TotalFee),
        //        _helper.GetParameter(nameof(orderInfo.ExpressCost), orderInfo.ExpressCost),
        //        _helper.GetParameter(nameof(orderInfo.TotalCount), orderInfo.TotalCount),
        //        _helper.GetParameter(nameof(orderInfo.IsPaied), orderInfo.IsPaied),
        //        _helper.GetParameter(nameof(orderInfo.State), orderInfo.State),
        //        _helper.GetParameter(nameof(orderInfo.AddDate), orderInfo.AddDate),
        //        _helper.GetParameter(nameof(orderInfo.Id), orderInfo.Id)
        //    };

        //    _helper.ExecuteNonQuery(_connectionString, sqlString, parameters.ToArray());
        //}

        //public void UpdateIsPaied(int orderId)
        //{
        //    string sqlString = $@"UPDATE {TableName} SET
        //        {nameof(OrderInfo.IsPaied)} = @{nameof(OrderInfo.IsPaied)} WHERE
        //        {nameof(OrderInfo.Id)} = @{nameof(OrderInfo.Id)}";

        //    var parameters = new List<IDataParameter>
        //    {
        //        _helper.GetParameter(nameof(OrderInfo.IsPaied), true),
        //        _helper.GetParameter(nameof(OrderInfo.Id), orderId)
        //    };

        //    _helper.ExecuteNonQuery(_connectionString, sqlString, parameters.ToArray());
        //}

        public void UpdateIsPaied(string guid)
        {
            string sqlString = $@"UPDATE {TableName} SET
                {nameof(OrderInfo.IsPaied)} = @{nameof(OrderInfo.IsPaied)} WHERE
                {nameof(OrderInfo.Guid)} = @{nameof(OrderInfo.Guid)}";

            var parameters = new List<IDataParameter>
            {
                _helper.GetParameter(nameof(OrderInfo.IsPaied), true),
                _helper.GetParameter(nameof(OrderInfo.Guid), guid)
            };

            _helper.ExecuteNonQuery(_connectionString, sqlString, parameters.ToArray());
        }

        public bool IsPaied(string guid)
        {
            var isPaied = false;

            string sqlString = $@"SELECT {nameof(OrderInfo.IsPaied)} FROM {TableName} WHERE {nameof(OrderInfo.Guid)} = @{nameof(OrderInfo.Guid)}";

            var parameters = new List<IDataParameter>
            {
                _helper.GetParameter(nameof(OrderInfo.Guid), guid)
            };

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters.ToArray()))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    isPaied = rdr.GetBoolean(0);
                }
                rdr.Close();
            }

            return isPaied;
        }

        public void UpdateIsPaiedAndState(int orderId, bool isPaied, string state)
        {
            string sqlString = $@"UPDATE {TableName} SET
                    {nameof(OrderInfo.IsPaied)} = @{nameof(OrderInfo.IsPaied)},
                    {nameof(OrderInfo.State)} = @{nameof(OrderInfo.State)} 
                WHERE {nameof(OrderInfo.Id)} = @{nameof(OrderInfo.Id)}";

            var parameters = new List<IDataParameter>
            {
                _helper.GetParameter(nameof(OrderInfo.IsPaied), isPaied),
                _helper.GetParameter(nameof(OrderInfo.State), state),
                _helper.GetParameter(nameof(OrderInfo.Id), orderId)
            };

            _helper.ExecuteNonQuery(_connectionString, sqlString, parameters.ToArray());
        }

        public void Delete(int orderId)
        {
            string sqlString = $"DELETE FROM {TableName} WHERE {nameof(OrderInfo.Id)} = @{nameof(OrderInfo.Id)}";
            var parameters = new List<IDataParameter>
            {
                _helper.GetParameter(nameof(OrderInfo.Id), orderId)
            };

            _helper.ExecuteNonQuery(_connectionString, sqlString, parameters.ToArray());
        }

        public void Delete(List<int> deleteIdList)
        {
            if (deleteIdList == null || deleteIdList.Count == 0) return;

            string sqlString =
                $"DELETE FROM {TableName} WHERE {nameof(OrderInfo.Id)} IN ({string.Join(",", deleteIdList)})";
            _helper.ExecuteNonQuery(_connectionString, sqlString);
        }

        //public string GetSelectStringByState(int siteId, string state)
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
        //    {nameof(OrderInfo.Message)}, 
        //    {nameof(OrderInfo.Channel)}, 
        //    {nameof(OrderInfo.TotalFee)}, 
        //    {nameof(OrderInfo.ExpressCost)}, 
        //    {nameof(OrderInfo.TotalCount)}, 
        //    {nameof(OrderInfo.IsPaied)}, 
        //    {nameof(OrderInfo.State)}, 
        //    {nameof(OrderInfo.AddDate)}
        //    FROM {TableName} WHERE {nameof(OrderInfo.SiteId)} = {siteId}";
        //    if (!string.IsNullOrEmpty(state))
        //    {
        //        state = Main.DataApi.FilterSql(state);
        //        sqlString += $" AND {nameof(OrderInfo.State)} = '{state}'";
        //    }
        //    sqlString += $" ORDER BY {nameof(OrderInfo.Id)} DESC";

        //    return sqlString;
        //}

        public int GetOrderCount(int siteId)
        {
            string sqlString = $@"SELECT COUNT(*) FROM {TableName} WHERE {nameof(OrderInfo.SiteId)} = @{nameof(OrderInfo.SiteId)}";

            var parameters = new[]
            {
                _helper.GetParameter(nameof(OrderInfo.SiteId), siteId)
            };

            return Main.Dao.GetIntResult(sqlString, parameters);
        }

        public int GetOrderCount(int siteId, bool isPaied, string state)
        {
            string sqlString;

            if (isPaied)
            {
                sqlString = $@"SELECT COUNT(*) FROM {TableName} WHERE 
                                {nameof(OrderInfo.SiteId)} = @{nameof(OrderInfo.SiteId)} AND
                                {nameof(OrderInfo.IsPaied)} = 1";
            }
            else
            {
                sqlString = $@"SELECT COUNT(*) FROM {TableName} WHERE 
                                {nameof(OrderInfo.SiteId)} = @{nameof(OrderInfo.SiteId)} AND
                                ({nameof(OrderInfo.IsPaied)} = 0 OR {nameof(OrderInfo.IsPaied)} IS NULL)";
            }

            var parameters = new List<IDataParameter>
            {
                _helper.GetParameter(nameof(OrderInfo.SiteId), siteId)
            };

            if (!string.IsNullOrEmpty(state))
            {
                sqlString += $" AND {nameof(OrderInfo.State)} = @{nameof(OrderInfo.State)}";
                parameters.Add(_helper.GetParameter(nameof(OrderInfo.State), state));
            }

            return Main.Dao.GetIntResult(sqlString, parameters.ToArray());
        }

        public string GetSelectStringBySearch(int siteId, bool isPaied, string state, string keyword)
        {
            var sqlString = $@"SELECT {nameof(OrderInfo.Id)}, 
            {nameof(OrderInfo.SiteId)}, 
            {nameof(OrderInfo.Guid)}, 
            {nameof(OrderInfo.UserName)}, 
            {nameof(OrderInfo.SessionId)}, 
            {nameof(OrderInfo.RealName)}, 
            {nameof(OrderInfo.Mobile)}, 
            {nameof(OrderInfo.Tel)}, 
            {nameof(OrderInfo.Location)}, 
            {nameof(OrderInfo.Address)}, 
            {nameof(OrderInfo.ZipCode)}, 
            {nameof(OrderInfo.Message)}, 
            {nameof(OrderInfo.Channel)}, 
            {nameof(OrderInfo.TotalFee)}, 
            {nameof(OrderInfo.ExpressCost)}, 
            {nameof(OrderInfo.TotalCount)}, 
            {nameof(OrderInfo.IsPaied)}, 
            {nameof(OrderInfo.State)}, 
            {nameof(OrderInfo.AddDate)}
            FROM {TableName} WHERE {nameof(OrderInfo.SiteId)} = {siteId}";

            if (isPaied)
            {
                sqlString += $@" AND {nameof(OrderInfo.IsPaied)} = 1";
            }
            else
            {
                sqlString += $@" AND ({nameof(OrderInfo.IsPaied)} = 0 OR {nameof(OrderInfo.IsPaied)} IS NULL)";
            }

            if (!string.IsNullOrEmpty(state))
            {
                state = Main.Instance.UtilsApi.FilterSql(state);
                sqlString += $" AND {nameof(OrderInfo.State)} = '{state}'";
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = Main.Instance.UtilsApi.FilterSql(keyword);
                sqlString += $" AND ({nameof(OrderInfo.Guid)} LIKE '%{keyword}%' OR {nameof(OrderInfo.RealName)} LIKE '%{keyword}%' OR {nameof(OrderInfo.Mobile)} LIKE '%{keyword}%' OR {nameof(OrderInfo.Location)} LIKE '%{keyword}%' OR {nameof(OrderInfo.Address)} LIKE '%{keyword}%')";
            }
            sqlString += $" ORDER BY {nameof(OrderInfo.Id)} DESC";

            return sqlString;
        }

        public OrderInfo GetOrderInfo(int orderId)
        {
            OrderInfo orderInfo = null;

            string sqlString = $@"SELECT {nameof(OrderInfo.Id)}, 
            {nameof(OrderInfo.SiteId)}, 
            {nameof(OrderInfo.Guid)}, 
            {nameof(OrderInfo.UserName)}, 
            {nameof(OrderInfo.SessionId)}, 
            {nameof(OrderInfo.RealName)}, 
            {nameof(OrderInfo.Mobile)}, 
            {nameof(OrderInfo.Tel)}, 
            {nameof(OrderInfo.Location)}, 
            {nameof(OrderInfo.Address)}, 
            {nameof(OrderInfo.ZipCode)}, 
            {nameof(OrderInfo.Message)}, 
            {nameof(OrderInfo.Channel)}, 
            {nameof(OrderInfo.TotalFee)}, 
            {nameof(OrderInfo.ExpressCost)}, 
            {nameof(OrderInfo.TotalCount)}, 
            {nameof(OrderInfo.IsPaied)}, 
            {nameof(OrderInfo.State)}, 
            {nameof(OrderInfo.AddDate)}
            FROM {TableName} WHERE {nameof(OrderInfo.Id)} = {orderId}";

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString))
            {
                if (rdr.Read())
                {
                    orderInfo = GetOrderInfo(rdr);
                }
                rdr.Close();
            }

            return orderInfo;
        }

        public OrderInfo GetOrderInfo(string guid)
        {
            OrderInfo orderInfo = null;

            string sqlString = $@"SELECT {nameof(OrderInfo.Id)}, 
            {nameof(OrderInfo.SiteId)}, 
            {nameof(OrderInfo.Guid)}, 
            {nameof(OrderInfo.UserName)}, 
            {nameof(OrderInfo.SessionId)}, 
            {nameof(OrderInfo.RealName)}, 
            {nameof(OrderInfo.Mobile)}, 
            {nameof(OrderInfo.Tel)}, 
            {nameof(OrderInfo.Location)}, 
            {nameof(OrderInfo.Address)}, 
            {nameof(OrderInfo.ZipCode)}, 
            {nameof(OrderInfo.Message)}, 
            {nameof(OrderInfo.Channel)}, 
            {nameof(OrderInfo.TotalFee)}, 
            {nameof(OrderInfo.ExpressCost)}, 
            {nameof(OrderInfo.TotalCount)}, 
            {nameof(OrderInfo.IsPaied)}, 
            {nameof(OrderInfo.State)}, 
            {nameof(OrderInfo.AddDate)}
            FROM {TableName} WHERE {nameof(OrderInfo.Guid)} = @{nameof(OrderInfo.Guid)}";

            var parameters = new List<IDataParameter>
            {
                _helper.GetParameter(nameof(OrderInfo.Guid), guid)
            };

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters.ToArray()))
            {
                if (rdr.Read())
                {
                    orderInfo = GetOrderInfo(rdr);
                }
                rdr.Close();
            }

            return orderInfo;
        }

        public List<OrderInfo> GetOrderInfoList(string userName, bool isPaied)
        {
            var list = new List<OrderInfo>();

            string sqlString;
            if (isPaied)
            {
                sqlString = $@"SELECT
                    {nameof(OrderInfo.Id)}, 
                    {nameof(OrderInfo.SiteId)}, 
                    {nameof(OrderInfo.Guid)}, 
                    {nameof(OrderInfo.UserName)}, 
                    {nameof(OrderInfo.SessionId)}, 
                    {nameof(OrderInfo.RealName)}, 
                    {nameof(OrderInfo.Mobile)}, 
                    {nameof(OrderInfo.Tel)}, 
                    {nameof(OrderInfo.Location)}, 
                    {nameof(OrderInfo.Address)}, 
                    {nameof(OrderInfo.ZipCode)}, 
                    {nameof(OrderInfo.Message)}, 
                    {nameof(OrderInfo.Channel)}, 
                    {nameof(OrderInfo.TotalFee)}, 
                    {nameof(OrderInfo.ExpressCost)}, 
                    {nameof(OrderInfo.TotalCount)}, 
                    {nameof(OrderInfo.IsPaied)}, 
                    {nameof(OrderInfo.State)}, 
                    {nameof(OrderInfo.AddDate)}
                FROM {TableName} WHERE 
                    {nameof(OrderInfo.UserName)} = @{nameof(OrderInfo.UserName)} AND 
                    {nameof(OrderInfo.IsPaied)} = @{nameof(OrderInfo.IsPaied)}
                ORDER BY {nameof(OrderInfo.Id)} DESC";
            }
            else
            {
                sqlString = $@"SELECT
                    {nameof(OrderInfo.Id)}, 
                    {nameof(OrderInfo.SiteId)}, 
                    {nameof(OrderInfo.Guid)}, 
                    {nameof(OrderInfo.UserName)}, 
                    {nameof(OrderInfo.SessionId)}, 
                    {nameof(OrderInfo.RealName)}, 
                    {nameof(OrderInfo.Mobile)}, 
                    {nameof(OrderInfo.Tel)}, 
                    {nameof(OrderInfo.Location)}, 
                    {nameof(OrderInfo.Address)}, 
                    {nameof(OrderInfo.ZipCode)}, 
                    {nameof(OrderInfo.Message)}, 
                    {nameof(OrderInfo.Channel)}, 
                    {nameof(OrderInfo.TotalFee)}, 
                    {nameof(OrderInfo.ExpressCost)}, 
                    {nameof(OrderInfo.TotalCount)}, 
                    {nameof(OrderInfo.IsPaied)}, 
                    {nameof(OrderInfo.State)}, 
                    {nameof(OrderInfo.AddDate)}
                FROM {TableName} WHERE 
                    {nameof(OrderInfo.UserName)} = @{nameof(OrderInfo.UserName)} AND 
                    ({nameof(OrderInfo.IsPaied)} = @{nameof(OrderInfo.IsPaied)} OR {nameof(OrderInfo.IsPaied)} IS NULL)
                ORDER BY {nameof(OrderInfo.Id)} DESC";
            }

            var parameters = new List<IDataParameter>
            {
                _helper.GetParameter(nameof(OrderInfo.UserName), userName),
                _helper.GetParameter(nameof(OrderInfo.IsPaied), isPaied)
            };

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters.ToArray()))
            {
                while (rdr.Read())
                {
                    list.Add(GetOrderInfo(rdr));
                }
                rdr.Close();
            }

            return list;
        }

        public List<OrderInfo> GetOrderInfoList(string userName, string state)
        {
            var list = new List<OrderInfo>();

            string sqlString = $@"SELECT {nameof(OrderInfo.Id)}, 
            {nameof(OrderInfo.SiteId)}, 
            {nameof(OrderInfo.Guid)}, 
            {nameof(OrderInfo.UserName)}, 
            {nameof(OrderInfo.SessionId)}, 
            {nameof(OrderInfo.RealName)}, 
            {nameof(OrderInfo.Mobile)}, 
            {nameof(OrderInfo.Tel)}, 
            {nameof(OrderInfo.Location)}, 
            {nameof(OrderInfo.Address)}, 
            {nameof(OrderInfo.ZipCode)}, 
            {nameof(OrderInfo.Message)}, 
            {nameof(OrderInfo.Channel)}, 
            {nameof(OrderInfo.TotalFee)}, 
            {nameof(OrderInfo.ExpressCost)}, 
            {nameof(OrderInfo.TotalCount)}, 
            {nameof(OrderInfo.IsPaied)}, 
            {nameof(OrderInfo.State)}, 
            {nameof(OrderInfo.AddDate)}
            FROM {TableName} WHERE {nameof(OrderInfo.UserName)} = @{nameof(OrderInfo.UserName)}";

            var parameters = new List<IDataParameter>
            {
                _helper.GetParameter(nameof(OrderInfo.UserName), userName)
            };

            if (!string.IsNullOrEmpty(state))
            {
                sqlString += $" AND {nameof(OrderInfo.State)} = @{nameof(OrderInfo.State)}";
                parameters.Add(_helper.GetParameter(nameof(OrderInfo.State), state));
            }

            sqlString += $" ORDER BY {nameof(OrderInfo.Id)} DESC";

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters.ToArray()))
            {
                while (rdr.Read())
                {
                    list.Add(GetOrderInfo(rdr));
                }
                rdr.Close();
            }

            return list;
        }

        private static OrderInfo GetOrderInfo(IDataRecord rdr)
        {
            if (rdr == null) return null;

            var orderInfo = new OrderInfo();

            var i = 0;
            orderInfo.Id = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
            i++;
            orderInfo.SiteId = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
            i++;
            orderInfo.Guid = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            orderInfo.UserName = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            orderInfo.SessionId = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            orderInfo.RealName = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            orderInfo.Mobile = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            orderInfo.Tel = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            orderInfo.Location = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            orderInfo.Address = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            orderInfo.ZipCode = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            orderInfo.Message = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            orderInfo.Channel = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            orderInfo.TotalFee = rdr.IsDBNull(i) ? 0 : rdr.GetDecimal(i);
            i++;
            orderInfo.ExpressCost = rdr.IsDBNull(i) ? 0 : rdr.GetDecimal(i);
            i++;
            orderInfo.TotalCount = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
            i++;
            orderInfo.IsPaied = !rdr.IsDBNull(i) && rdr.GetBoolean(i);
            i++;
            orderInfo.State = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            orderInfo.AddDate = rdr.IsDBNull(i) ? DateTime.Now : rdr.GetDateTime(i);

            return orderInfo;
        }

    }
}
