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
    public class Main : PluginBase
    {
        public static AddressDao AddressDao { get; private set; }

        public static AreaDao AreaDao { get; private set; }

        public static CartDao CartDao { get; private set; }

        public static Dao Dao { get; private set; }

        public static DeliveryDao DeliveryDao { get; private set; }

        public static OrderDao OrderDao { get; private set; }

        private readonly Dictionary<int, ConfigInfo> _configInfoDict = new Dictionary<int, ConfigInfo>();

        public ConfigInfo GetConfigInfo(int siteId)
        {
            if (!_configInfoDict.ContainsKey(siteId))
            {
                _configInfoDict[siteId] = ConfigApi.GetConfig<ConfigInfo>(siteId) ?? new ConfigInfo();
            }
            return _configInfoDict[siteId];
        }

        internal static Main Instance { get; private set; }

        public override void Startup(IService service)
        {
            Instance = this;

            Dao = new Dao(ConnectionString, DataApi);
            AddressDao = new AddressDao(ConnectionString, DataApi);
            AreaDao = new AreaDao(ConnectionString, DataApi);
            CartDao = new CartDao(ConnectionString, DataApi);
            DeliveryDao = new DeliveryDao(DatabaseType, ConnectionString, DataApi);
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
                .AddStlElementParser(StlShoppingOrder.ElementName, StlShoppingOrder.Parse)
                ;

            service.ApiPost += (sender, args) =>
            {
                var request = args.Request;
                var action = args.Action;
                var id = args.Id;

                if (!string.IsNullOrEmpty(action) && !string.IsNullOrEmpty(id))
                {
                    if (Utils.EqualsIgnoreCase(action, nameof(StlShoppingPay.ApiPayWeixinNotify)))
                    {
                        return StlShoppingPay.ApiPayWeixinNotify(request, id);
                    }
                }
                else if (!string.IsNullOrEmpty(action))
                {
                    if (Utils.EqualsIgnoreCase(action, nameof(StlShoppingAdd.ApiAdd)))
                    {
                        return StlShoppingAdd.ApiAdd(request);
                    }
                    if (Utils.EqualsIgnoreCase(action, nameof(StlShoppingAddSuccess.ApiAddSuccessGet)))
                    {
                        return StlShoppingAddSuccess.ApiAddSuccessGet(request);
                    }
                    if (Utils.EqualsIgnoreCase(action, nameof(StlShoppingCart.ApiCartGet)))
                    {
                        return StlShoppingCart.ApiCartGet(request);
                    }
                    if (Utils.EqualsIgnoreCase(action, nameof(StlShoppingCart.ApiCartSave)))
                    {
                        return StlShoppingCart.ApiCartSave(request);
                    }
                    if (Utils.EqualsIgnoreCase(action, nameof(StlShoppingOrder.ApiOrderGet)))
                    {
                        return StlShoppingOrder.ApiOrderGet(request);
                    }
                    if (Utils.EqualsIgnoreCase(action, nameof(StlShoppingOrders.ApiOrdersGet)))
                    {
                        return StlShoppingOrders.ApiOrdersGet(request);
                    }
                    if (Utils.EqualsIgnoreCase(action, nameof(StlShoppingOrders.ApiOrdersRemove)))
                    {
                        return StlShoppingOrders.ApiOrdersRemove(request);
                    }
                    if (Utils.EqualsIgnoreCase(action, nameof(StlShoppingOrders.ApiOrdersPay)))
                    {
                        return StlShoppingOrders.ApiOrdersPay(request);
                    }
                    if (Utils.EqualsIgnoreCase(action, nameof(StlShoppingPay.ApiPayGet)))
                    {
                        return StlShoppingPay.ApiPayGet(request);
                    }
                    if (Utils.EqualsIgnoreCase(action, nameof(StlShoppingPay.ApiPaySaveAddress)))
                    {
                        return StlShoppingPay.ApiPaySaveAddress(request);
                    }
                    if (Utils.EqualsIgnoreCase(action, nameof(StlShoppingPay.ApiPayRemoveAddress)))
                    {
                        return StlShoppingPay.ApiPayRemoveAddress(request);
                    }
                    if (Utils.EqualsIgnoreCase(action, nameof(StlShoppingPay.ApiPaySetAddressAndDelivery)))
                    {
                        return StlShoppingPay.ApiPaySetAddressAndDelivery(request);
                    }
                    if (Utils.EqualsIgnoreCase(action, nameof(StlShoppingPay.ApiPay)))
                    {
                        return StlShoppingPay.ApiPay(request);
                    }
                    if (Utils.EqualsIgnoreCase(action, nameof(StlShoppingPay.ApiPayWeixinInterval)))
                    {
                        return StlShoppingPay.ApiPayWeixinInterval(request);
                    }
                    if (Utils.EqualsIgnoreCase(action, nameof(StlShoppingPaySuccess.ApiPaySuccessGet)))
                    {
                        return StlShoppingPaySuccess.ApiPaySuccessGet(request);
                    }
                }

                throw new Exception("请求的资源不在服务器上");
            };

            service.ApiGet += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Action))
                {
                    if (Utils.EqualsIgnoreCase(args.Action, nameof(StlShoppingPay.ApiPayQrCode)))
                    {
                        return StlShoppingPay.ApiPayQrCode(args.Request);
                    }
                }

                throw new Exception("请求的资源不在服务器上");
            };
        }
    }
}