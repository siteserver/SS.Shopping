using System;
using System.Collections.Generic;
using System.Data;
using SiteServer.Plugin;
using SS.Shopping.Model;

namespace SS.Shopping.Provider
{
    public static class CartDao
    {
        public const string TableName = "ss_shopping_cart";

        public static List<TableColumn> Columns => new List<TableColumn>
        {
            new TableColumn
            {
                AttributeName = nameof(CartInfo.Id),
                DataType = DataType.Integer,
                IsPrimaryKey = true,
                IsIdentity = true
            },
            new TableColumn
            {
                AttributeName = nameof(CartInfo.SiteId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(CartInfo.OrderId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(CartInfo.UserName),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(CartInfo.SessionId),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(CartInfo.ProductId),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(CartInfo.ProductName),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(CartInfo.ImageUrl),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(CartInfo.LinkUrl),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(CartInfo.Fee),
                DataType = DataType.Decimal
            },
            new TableColumn
            {
                AttributeName = nameof(CartInfo.IsDelivery),
                DataType = DataType.Boolean
            },
            new TableColumn
            {
                AttributeName = nameof(CartInfo.Count),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(CartInfo.AddDate),
                DataType = DataType.DateTime
            }
        };

        public static int Insert(CartInfo cartInfo)
        {
            string sqlString = $@"INSERT INTO {TableName}
           ({nameof(CartInfo.SiteId)}, 
            {nameof(CartInfo.OrderId)}, 
            {nameof(CartInfo.UserName)}, 
            {nameof(CartInfo.SessionId)}, 
            {nameof(CartInfo.ProductId)}, 
            {nameof(CartInfo.ProductName)}, 
            {nameof(CartInfo.ImageUrl)}, 
            {nameof(CartInfo.LinkUrl)}, 
            {nameof(CartInfo.Fee)}, 
            {nameof(CartInfo.IsDelivery)}, 
            {nameof(CartInfo.Count)}, 
            {nameof(CartInfo.AddDate)})
     VALUES
           (@{nameof(CartInfo.SiteId)}, 
            @{nameof(CartInfo.OrderId)}, 
            @{nameof(CartInfo.UserName)}, 
            @{nameof(CartInfo.SessionId)}, 
            @{nameof(CartInfo.ProductId)}, 
            @{nameof(CartInfo.ProductName)}, 
            @{nameof(CartInfo.ImageUrl)}, 
            @{nameof(CartInfo.LinkUrl)}, 
            @{nameof(CartInfo.Fee)}, 
            @{nameof(CartInfo.IsDelivery)}, 
            @{nameof(CartInfo.Count)}, 
            @{nameof(CartInfo.AddDate)})";

            var parameters = new List<IDataParameter>
            {
                Context.DatabaseApi.GetParameter(nameof(cartInfo.SiteId), cartInfo.SiteId),
                Context.DatabaseApi.GetParameter(nameof(cartInfo.OrderId), 0),
                Context.DatabaseApi.GetParameter(nameof(cartInfo.UserName), cartInfo.UserName),
                Context.DatabaseApi.GetParameter(nameof(cartInfo.SessionId), cartInfo.SessionId),
                Context.DatabaseApi.GetParameter(nameof(cartInfo.ProductId), cartInfo.ProductId),
                Context.DatabaseApi.GetParameter(nameof(cartInfo.ProductName), cartInfo.ProductName),
                Context.DatabaseApi.GetParameter(nameof(cartInfo.ImageUrl), cartInfo.ImageUrl),
                Context.DatabaseApi.GetParameter(nameof(cartInfo.LinkUrl), cartInfo.LinkUrl),
                Context.DatabaseApi.GetParameter(nameof(cartInfo.Fee), cartInfo.Fee),
                Context.DatabaseApi.GetParameter(nameof(cartInfo.IsDelivery), cartInfo.IsDelivery),
                Context.DatabaseApi.GetParameter(nameof(cartInfo.Count), cartInfo.Count),
                Context.DatabaseApi.GetParameter(nameof(cartInfo.AddDate), cartInfo.AddDate)
            };

            return Context.DatabaseApi.ExecuteNonQueryAndReturnId(TableName, nameof(CartInfo.Id), Context.ConnectionString, sqlString, parameters.ToArray());
        }

        public static void Update(CartInfo cartInfo)
        {
            string sqlString = $@"UPDATE {TableName} SET
                {nameof(CartInfo.SiteId)} = @{nameof(CartInfo.SiteId)}, 
                {nameof(CartInfo.UserName)} = @{nameof(CartInfo.UserName)}, 
                {nameof(CartInfo.SessionId)} = @{nameof(CartInfo.SessionId)}, 
                {nameof(CartInfo.ProductId)} = @{nameof(CartInfo.ProductId)}, 
                {nameof(CartInfo.ProductName)} = @{nameof(CartInfo.ProductName)}, 
                {nameof(CartInfo.ImageUrl)} = @{nameof(CartInfo.ImageUrl)}, 
                {nameof(CartInfo.LinkUrl)} = @{nameof(CartInfo.LinkUrl)}, 
                {nameof(CartInfo.Fee)} = @{nameof(CartInfo.Fee)}, 
                {nameof(CartInfo.IsDelivery)} = @{nameof(CartInfo.IsDelivery)}, 
                {nameof(CartInfo.Count)} = @{nameof(CartInfo.Count)}, 
                {nameof(CartInfo.AddDate)} = @{nameof(CartInfo.AddDate)}
            WHERE {nameof(CartInfo.Id)} = @{nameof(CartInfo.Id)}";

            var parameters = new List<IDataParameter>
            {
                Context.DatabaseApi.GetParameter(nameof(cartInfo.SiteId), cartInfo.SiteId),
                Context.DatabaseApi.GetParameter(nameof(cartInfo.UserName), cartInfo.UserName),
                Context.DatabaseApi.GetParameter(nameof(cartInfo.SessionId), cartInfo.SessionId),
                Context.DatabaseApi.GetParameter(nameof(cartInfo.ProductId), cartInfo.ProductId),
                Context.DatabaseApi.GetParameter(nameof(cartInfo.ProductName), cartInfo.ProductName),
                Context.DatabaseApi.GetParameter(nameof(cartInfo.ImageUrl), cartInfo.ImageUrl),
                Context.DatabaseApi.GetParameter(nameof(cartInfo.LinkUrl), cartInfo.LinkUrl),
                Context.DatabaseApi.GetParameter(nameof(cartInfo.Fee), cartInfo.Fee),
                Context.DatabaseApi.GetParameter(nameof(cartInfo.IsDelivery), cartInfo.IsDelivery),
                Context.DatabaseApi.GetParameter(nameof(cartInfo.Count), cartInfo.Count),
                Context.DatabaseApi.GetParameter(nameof(cartInfo.AddDate), cartInfo.AddDate),
                Context.DatabaseApi.GetParameter(nameof(cartInfo.Id), cartInfo.Id)
            };

            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters.ToArray());
        }

        public static void UpdateUserName(int siteId, string sessionId, string userName)
        {
            string sqlString = $@"UPDATE {TableName} SET
                {nameof(CartInfo.UserName)} = @{nameof(CartInfo.UserName)} WHERE
                {nameof(CartInfo.SiteId)} = @{nameof(CartInfo.SiteId)} AND 
                {nameof(CartInfo.OrderId)} = 0 AND 
                {nameof(CartInfo.SessionId)} = @{nameof(CartInfo.SessionId)}";

            var parameters = new List<IDataParameter>
            {
                
                Context.DatabaseApi.GetParameter(nameof(CartInfo.UserName), userName),
                Context.DatabaseApi.GetParameter(nameof(CartInfo.SiteId), siteId),
                Context.DatabaseApi.GetParameter(nameof(CartInfo.SessionId), sessionId)
            };

            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters.ToArray());
        }

        public static void UpdateOrderId(List<int> cartIdList, int orderId)
        {
            if (cartIdList == null || cartIdList.Count == 0) return;

            string sqlString = $@"UPDATE {TableName} SET
                {nameof(CartInfo.OrderId)} = @{nameof(CartInfo.OrderId)} WHERE
                {nameof(CartInfo.Id)} IN ({string.Join(",", cartIdList)})";

            var parameters = new List<IDataParameter>
            {
                Context.DatabaseApi.GetParameter(nameof(CartInfo.OrderId), orderId)
            };

            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters.ToArray());
        }

        public static void Delete(int siteId, string userName, string sessionId)
        {
            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(sessionId)) return;

            string sqlString = $"DELETE FROM {TableName} WHERE {nameof(CartInfo.SiteId)} = @{nameof(CartInfo.SiteId)} AND {nameof(CartInfo.OrderId)} = 0";
            var parameters = new List<IDataParameter>
            {
                Context.DatabaseApi.GetParameter(nameof(CartInfo.SiteId), siteId)
            };

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(sessionId))
            {
                sqlString += $" AND ({nameof(CartInfo.UserName)} = @{nameof(CartInfo.UserName)} OR {nameof(CartInfo.SessionId)} = @{nameof(CartInfo.SessionId)})";
                parameters.Add(Context.DatabaseApi.GetParameter(nameof(CartInfo.UserName), userName));
                parameters.Add(Context.DatabaseApi.GetParameter(nameof(CartInfo.SessionId), sessionId));
            }
            else if (!string.IsNullOrEmpty(userName))
            {
                sqlString += $" AND {nameof(CartInfo.UserName)} = @{nameof(CartInfo.UserName)}";
                parameters.Add(Context.DatabaseApi.GetParameter(nameof(CartInfo.UserName), userName));
            }
            else if (!string.IsNullOrEmpty(sessionId))
            {
                sqlString += $" AND {nameof(CartInfo.SessionId)} = @{nameof(CartInfo.SessionId)}";
                parameters.Add(Context.DatabaseApi.GetParameter(nameof(CartInfo.SessionId), sessionId));
            }
            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters.ToArray());
        }

        public static int GetCartId(int siteId, string sessionId, string productId)
        {
            string sqlString = $@"SELECT {nameof(CartInfo.Id)} FROM {TableName} WHERE 
                {nameof(CartInfo.SiteId)} = @{nameof(CartInfo.SiteId)} AND
                {nameof(CartInfo.OrderId)} = 0 AND
                {nameof(CartInfo.SessionId)} = @{nameof(CartInfo.SessionId)} AND
                {nameof(CartInfo.ProductId)} = @{nameof(CartInfo.ProductId)}";

            var parameters = new []
            {
                Context.DatabaseApi.GetParameter(nameof(CartInfo.SiteId), siteId),
                Context.DatabaseApi.GetParameter(nameof(CartInfo.SessionId), sessionId),
                Context.DatabaseApi.GetParameter(nameof(CartInfo.ProductId), productId)
            };

            return Dao.GetIntResult(sqlString, parameters);
        }

        public static List<CartInfo> GetCartInfoList(int siteId, string userName, string sessionId)
        {
            var list = new List<CartInfo>();
            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(sessionId)) return list;

            string sqlString = $@"SELECT {nameof(CartInfo.Id)}, 
                {nameof(CartInfo.SiteId)}, 
                {nameof(CartInfo.OrderId)}, 
                {nameof(CartInfo.UserName)}, 
                {nameof(CartInfo.SessionId)}, 
                {nameof(CartInfo.ProductId)}, 
                {nameof(CartInfo.ProductName)}, 
                {nameof(CartInfo.ImageUrl)}, 
                {nameof(CartInfo.LinkUrl)}, 
                {nameof(CartInfo.Fee)}, 
                {nameof(CartInfo.IsDelivery)}, 
                {nameof(CartInfo.Count)}, 
                {nameof(CartInfo.AddDate)}
                FROM {TableName} WHERE 
                {nameof(CartInfo.SiteId)} = @{nameof(CartInfo.SiteId)} AND
                {nameof(CartInfo.OrderId)} = 0";

            var parameters = new List<IDataParameter>
            {
                Context.DatabaseApi.GetParameter(nameof(CartInfo.SiteId), siteId)
            };

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(sessionId))
            {
                sqlString += $" AND ({nameof(CartInfo.UserName)} = @{nameof(CartInfo.UserName)} OR {nameof(CartInfo.SessionId)} = @{nameof(CartInfo.SessionId)})";
                parameters.Add(Context.DatabaseApi.GetParameter(nameof(CartInfo.UserName), userName));
                parameters.Add(Context.DatabaseApi.GetParameter(nameof(CartInfo.SessionId), sessionId));
            }
            else if (!string.IsNullOrEmpty(userName))
            {
                sqlString += $" AND {nameof(CartInfo.UserName)} = @{nameof(CartInfo.UserName)}";
                parameters.Add(Context.DatabaseApi.GetParameter(nameof(CartInfo.UserName), userName));
            }
            else if (!string.IsNullOrEmpty(sessionId))
            {
                sqlString += $" AND {nameof(CartInfo.SessionId)} = @{nameof(CartInfo.SessionId)}";
                parameters.Add(Context.DatabaseApi.GetParameter(nameof(CartInfo.SessionId), sessionId));
            }

            sqlString += $" ORDER BY {nameof(CartInfo.Id)} DESC";

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters.ToArray()))
            {
                while (rdr.Read())
                {
                    list.Add(GetCartInfo(rdr));
                }
                rdr.Close();
            }

            return list;
        }

        public static List<CartInfo> GetCartInfoList(int orderId)
        {
            var list = new List<CartInfo>();

            string sqlString = $@"SELECT {nameof(CartInfo.Id)}, 
                {nameof(CartInfo.SiteId)}, 
                {nameof(CartInfo.OrderId)}, 
                {nameof(CartInfo.UserName)}, 
                {nameof(CartInfo.SessionId)}, 
                {nameof(CartInfo.ProductId)}, 
                {nameof(CartInfo.ProductName)}, 
                {nameof(CartInfo.ImageUrl)}, 
                {nameof(CartInfo.LinkUrl)}, 
                {nameof(CartInfo.Fee)}, 
                {nameof(CartInfo.IsDelivery)}, 
                {nameof(CartInfo.Count)}, 
                {nameof(CartInfo.AddDate)}
                FROM {TableName} WHERE 
                {nameof(CartInfo.OrderId)} = @{nameof(CartInfo.OrderId)}";

            var parameters = new List<IDataParameter>
            {
                Context.DatabaseApi.GetParameter(nameof(CartInfo.OrderId), orderId)
            };

            sqlString += $" ORDER BY {nameof(CartInfo.Id)} DESC";

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters.ToArray()))
            {
                while (rdr.Read())
                {
                    list.Add(GetCartInfo(rdr));
                }
                rdr.Close();
            }

            return list;
        }

        public static CartInfo GetCartInfo(int cartId)
        {
            CartInfo cartInfo = null;

            string sqlString = $@"SELECT {nameof(CartInfo.Id)}, 
            {nameof(CartInfo.SiteId)}, 
            {nameof(CartInfo.OrderId)}, 
            {nameof(CartInfo.UserName)}, 
            {nameof(CartInfo.SessionId)}, 
            {nameof(CartInfo.ProductId)}, 
            {nameof(CartInfo.ProductName)}, 
            {nameof(CartInfo.ImageUrl)}, 
            {nameof(CartInfo.LinkUrl)}, 
            {nameof(CartInfo.Fee)}, 
            {nameof(CartInfo.IsDelivery)}, 
            {nameof(CartInfo.Count)}, 
            {nameof(CartInfo.AddDate)}
            FROM {TableName} WHERE {nameof(CartInfo.Id)} = {cartId}";

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString))
            {
                if (rdr.Read())
                {
                    cartInfo = GetCartInfo(rdr);
                }
                rdr.Close();
            }

            return cartInfo;
        }

        private static CartInfo GetCartInfo(IDataRecord rdr)
        {
            if (rdr == null) return null;
            
            var cartInfo = new CartInfo();

            var i = 0;
            cartInfo.Id = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
            i++;
            cartInfo.SiteId = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
            i++;
            cartInfo.OrderId = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
            i++;
            cartInfo.UserName = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            cartInfo.SessionId = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            cartInfo.ProductId = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            cartInfo.ProductName = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            cartInfo.ImageUrl = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            cartInfo.LinkUrl = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            cartInfo.Fee = rdr.IsDBNull(i) ? 0 : rdr.GetDecimal(i);
            i++;
            cartInfo.IsDelivery = !rdr.IsDBNull(i) && rdr.GetBoolean(i);
            i++;
            cartInfo.Count = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
            i++;
            cartInfo.AddDate = rdr.IsDBNull(i) ? DateTime.Now : rdr.GetDateTime(i);

            return cartInfo;
        }
    }
}
