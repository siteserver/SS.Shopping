using System;
using System.Collections.Generic;
using SiteServer.Plugin;
using SS.Shopping.Core;
using SS.Shopping.Model;
using SS.Shopping.Pages;
using SS.Shopping.Parse;
using SS.Shopping.Provider;

namespace SS.Shopping
{
    public class Main : IPlugin
    {
        public static DatabaseType DatabaseType { get; private set; }
        public static string ConnectionString { get; private set; }
        public static IDataApi DataApi { get; private set; }
        public static IAdminApi AdminApi { get; private set; }
        public static IFilesApi FilesApi { get; private set; }
        public static IParseApi ParseApi { get; private set; }
        public static IConfigApi ConfigApi { get; private set; }
        public static ISiteApi SiteApi { get; private set; }

        public static AddressDao AddressDao { get; private set; }

        public static AreaDao AreaDao { get; private set; }

        public static CartDao CartDao { get; private set; }

        public static Dao Dao { get; private set; }

        public static DeliveryDao DeliveryDao { get; private set; }

        public static OrderDao OrderDao { get; private set; }

        private static readonly Dictionary<int, ConfigInfo> ConfigInfoDict = new Dictionary<int, ConfigInfo>();

        public static ConfigInfo GetConfigInfo(int siteId)
        {
            if (!ConfigInfoDict.ContainsKey(siteId))
            {
                ConfigInfoDict[siteId] = ConfigApi.GetConfig<ConfigInfo>(siteId) ?? new ConfigInfo();
            }
            return ConfigInfoDict[siteId];
        }

        public void Startup(IContext context, IService service)
        {
            DatabaseType = context.Environment.DatabaseType;
            ConnectionString = context.Environment.ConnectionString;
            DataApi = context.DataApi;
            AdminApi = context.AdminApi;
            FilesApi = context.FilesApi;
            ParseApi = context.ParseApi;
            ConfigApi = context.ConfigApi;
            SiteApi = context.SiteApi;

            Dao = new Dao(ConnectionString, DataApi);
            AddressDao = new AddressDao(ConnectionString, DataApi);
            AreaDao = new AreaDao(ConnectionString, DataApi);
            CartDao = new CartDao(ConnectionString, DataApi);
            DeliveryDao = new DeliveryDao(context.Environment.DatabaseType, ConnectionString, DataApi);
            OrderDao = new OrderDao(ConnectionString, DataApi);

            service.AddSiteMenu(siteId => new Menu
                {
                    Text = "购物",
                    IconClass = "ion-ios-cart",
                    Menus = new List<Menu>
                    {
                        new Menu
                        {
                            Text =
                                $"{Utils.GetStateText(false, string.Empty)}订单（{OrderDao.GetOrderCount(siteId, false, string.Empty)}）",
                            Href = $"{nameof(PageOrder)}.aspx?isPaied={false}&state={string.Empty}"
                        },
                        new Menu
                        {
                            Text =
                                $"{Utils.GetStateText(true, string.Empty)}订单（{OrderDao.GetOrderCount(siteId, true, string.Empty)}）",
                            Href = $"{nameof(PageOrder)}.aspx?isPaied={true}&state={string.Empty}"
                        },
                        new Menu
                        {
                            Text =
                                $"{Utils.GetStateText(true, nameof(OrderState.Done))}订单（{OrderDao.GetOrderCount(siteId, true, nameof(OrderState.Done))}）",
                            Href = $"{nameof(PageOrder)}.aspx?isPaied={true}&state={nameof(OrderState.Done)}"
                        },
                        new Menu
                        {
                            Text = "购物设置",
                            Href = $"{nameof(PageSettings)}.aspx"
                        },
                        new Menu
                        {
                            Text = "运费管理",
                            Href = $"{nameof(PageDelivery)}.aspx"
                        }
                    }
                })
                .AddDatabaseTable(AddressDao.TableName, AddressDao.Columns)
                .AddDatabaseTable(AreaDao.TableName, AreaDao.Columns)
                .AddDatabaseTable(CartDao.TableName, CartDao.Columns)
                .AddDatabaseTable(DeliveryDao.TableName, DeliveryDao.Columns)
                .AddDatabaseTable(OrderDao.TableName, OrderDao.Columns)
                .AddStlElementParser(StlShoppingAdd.ElementName, StlShoppingAdd.Parse)
                .AddStlElementParser(StlShoppingAddSuccess.ElementName, StlShoppingAddSuccess.Parse)
                .AddStlElementParser(StlShoppingCart.ElementName, StlShoppingCart.Parse)
                .AddStlElementParser(StlShoppingPay.ElementName, StlShoppingPay.Parse)
                .AddStlElementParser(StlShoppingPaySuccess.ElementName, StlShoppingPaySuccess.Parse)
                .AddStlElementParser(StlShoppingOrders.ElementName, StlShoppingOrders.Parse)
                .AddStlElementParser(StlShoppingOrder.ElementName, StlShoppingOrder.Parse);

            service.ApiPost += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Name) && !string.IsNullOrEmpty(args.Id))
                {
                    if (Utils.EqualsIgnoreCase(args.Name, nameof(StlShoppingPay.ApiPayWeixinNotify)))
                    {
                        return StlShoppingPay.ApiPayWeixinNotify(args.Request, args.Id);
                    }
                }
                if (!string.IsNullOrEmpty(args.Name))
                {
                    if (Utils.EqualsIgnoreCase(args.Name, nameof(StlShoppingAdd.ApiAdd)))
                    {
                        return StlShoppingAdd.ApiAdd(args.Request);
                    }
                    if (Utils.EqualsIgnoreCase(args.Name, nameof(StlShoppingAddSuccess.ApiAddSuccessGet)))
                    {
                        return StlShoppingAddSuccess.ApiAddSuccessGet(args.Request);
                    }
                    if (Utils.EqualsIgnoreCase(args.Name, nameof(StlShoppingCart.ApiCartGet)))
                    {
                        return StlShoppingCart.ApiCartGet(args.Request);
                    }
                    if (Utils.EqualsIgnoreCase(args.Name, nameof(StlShoppingCart.ApiCartSave)))
                    {
                        return StlShoppingCart.ApiCartSave(args.Request);
                    }
                    if (Utils.EqualsIgnoreCase(args.Name, nameof(StlShoppingOrder.ApiOrderGet)))
                    {
                        return StlShoppingOrder.ApiOrderGet(args.Request);
                    }
                    if (Utils.EqualsIgnoreCase(args.Name, nameof(StlShoppingOrders.ApiOrdersGet)))
                    {
                        return StlShoppingOrders.ApiOrdersGet(args.Request);
                    }
                    if (Utils.EqualsIgnoreCase(args.Name, nameof(StlShoppingOrders.ApiOrdersRemove)))
                    {
                        return StlShoppingOrders.ApiOrdersRemove(args.Request);
                    }
                    if (Utils.EqualsIgnoreCase(args.Name, nameof(StlShoppingOrders.ApiOrdersPay)))
                    {
                        return StlShoppingOrders.ApiOrdersPay(args.Request);
                    }
                    if (Utils.EqualsIgnoreCase(args.Name, nameof(StlShoppingPay.ApiPayGet)))
                    {
                        return StlShoppingPay.ApiPayGet(args.Request);
                    }
                    if (Utils.EqualsIgnoreCase(args.Name, nameof(StlShoppingPay.ApiPaySaveAddress)))
                    {
                        return StlShoppingPay.ApiPaySaveAddress(args.Request);
                    }
                    if (Utils.EqualsIgnoreCase(args.Name, nameof(StlShoppingPay.ApiPayRemoveAddress)))
                    {
                        return StlShoppingPay.ApiPayRemoveAddress(args.Request);
                    }
                    if (Utils.EqualsIgnoreCase(args.Name, nameof(StlShoppingPay.ApiPaySetAddressAndDelivery)))
                    {
                        return StlShoppingPay.ApiPaySetAddressAndDelivery(args.Request);
                    }
                    if (Utils.EqualsIgnoreCase(args.Name, nameof(StlShoppingPay.ApiPay)))
                    {
                        return StlShoppingPay.ApiPay(args.Request);
                    }
                    if (Utils.EqualsIgnoreCase(args.Name, nameof(StlShoppingPay.ApiPayWeixinInterval)))
                    {
                        return StlShoppingPay.ApiPayWeixinInterval(args.Request);
                    }
                    if (Utils.EqualsIgnoreCase(args.Name, nameof(StlShoppingPaySuccess.ApiPaySuccessGet)))
                    {
                        return StlShoppingPaySuccess.ApiPaySuccessGet(args.Request);
                    }
                }

                throw new Exception("请求的资源不在服务器上");
            };

            service.ApiGet += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Name))
                {
                    if (Utils.EqualsIgnoreCase(args.Name, nameof(StlShoppingPay.ApiPayQrCode)))
                    {
                        return StlShoppingPay.ApiPayQrCode(args.Request);
                    }
                }

                throw new Exception("请求的资源不在服务器上");
            };
        }
    }
}