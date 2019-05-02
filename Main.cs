using System;
using System.Collections.Generic;
using SiteServer.Plugin;
using SS.Shopping.Core;
using SS.Shopping.Core.Model;
using SS.Shopping.Core.Pages;
using SS.Shopping.Core.Parse;
using SS.Shopping.Core.Provider;

namespace SS.Shopping
{
    public class Main : PluginBase
    {
        public const string PluginId = "SS.Shopping";

        public static AddressRepository AddressRepository { get; private set; }
        public static AreaRepository AreaRepository { get; private set; }
        public static CartRepository CartRepository { get; private set; }
        public static DeliveryRepository DeliveryRepository { get; private set; }
        public static OrderRepository OrderRepository { get; private set; }

        public override void Startup(IService service)
        {
            AddressRepository = new AddressRepository();
            AreaRepository = new AreaRepository();
            CartRepository = new CartRepository();
            DeliveryRepository = new DeliveryRepository();
            OrderRepository = new OrderRepository();

            service.AddSiteMenu(siteId => new Menu
                {
                    Text = "购物",
                    IconClass = "ion-ios-cart",
                    Menus = new List<Menu>
                    {
                        new Menu
                        {
                            Text =
                                $"{Utils.GetStateText(false, string.Empty)}订单（{OrderRepository.GetOrderCount(siteId, false, string.Empty)}）",
                            Href = $"pages/{nameof(PageOrder)}.aspx?isPayed={false}&state={string.Empty}"
                        },
                        new Menu
                        {
                            Text =
                                $"{Utils.GetStateText(true, string.Empty)}订单（{OrderRepository.GetOrderCount(siteId, true, string.Empty)}）",
                            Href = $"pages/{nameof(PageOrder)}.aspx?isPayed={true}&state={string.Empty}"
                        },
                        new Menu
                        {
                            Text =
                                $"{Utils.GetStateText(true, nameof(OrderState.Done))}订单（{OrderRepository.GetOrderCount(siteId, true, nameof(OrderState.Done))}）",
                            Href = $"pages/{nameof(PageOrder)}.aspx?isPayed={true}&state={nameof(OrderState.Done)}"
                        },
                        new Menu
                        {
                            Text = "购物设置",
                            Href = $"pages/{nameof(PageSettings)}.aspx"
                        },
                        new Menu
                        {
                            Text = "运费管理",
                            Href = $"pages/{nameof(PageDelivery)}.aspx"
                        }
                    }
                })
                .AddDatabaseTable(AddressRepository.TableName, AddressRepository.TableColumns)
                .AddDatabaseTable(AreaRepository.TableName, AreaRepository.TableColumns)
                .AddDatabaseTable(CartRepository.TableName, CartRepository.TableColumns)
                .AddDatabaseTable(DeliveryRepository.TableName, DeliveryRepository.TableColumns)
                .AddDatabaseTable(OrderRepository.TableName, OrderRepository.TableColumns)
                .AddStlElementParser(StlShoppingAdd.ElementName, StlShoppingAdd.Parse)
                .AddStlElementParser(StlShoppingAddSuccess.ElementName, StlShoppingAddSuccess.Parse)
                .AddStlElementParser(StlShoppingCart.ElementName, StlShoppingCart.Parse)
                .AddStlElementParser(StlShoppingPay.ElementName, StlShoppingPay.Parse)
                .AddStlElementParser(StlShoppingPaySuccess.ElementName, StlShoppingPaySuccess.Parse)
                .AddStlElementParser(StlShoppingOrders.ElementName, StlShoppingOrders.Parse)
                .AddStlElementParser(StlShoppingOrder.ElementName, StlShoppingOrder.Parse)
                ;

            //service.RestApiPost += (sender, e) =>
            //{
            //    var request = e.Request;

            //    if (!string.IsNullOrEmpty(e.RouteResource) && !string.IsNullOrEmpty(e.RouteId))
            //    {
            //        if (Utils.EqualsIgnoreCase(e.RouteResource, nameof(StlShoppingPay.ApiPayWeixinNotify)))
            //        {
            //            return StlShoppingPay.ApiPayWeixinNotify(request, e.RouteId);
            //        }
            //    }
            //    else if (!string.IsNullOrEmpty(e.RouteResource))
            //    {
            //        if (Utils.EqualsIgnoreCase(e.RouteResource, nameof(StlShoppingAdd.ApiAdd)))
            //        {
            //            return StlShoppingAdd.ApiAdd(request);
            //        }
            //        if (Utils.EqualsIgnoreCase(e.RouteResource, nameof(StlShoppingAddSuccess.ApiAddSuccessGet)))
            //        {
            //            return StlShoppingAddSuccess.ApiAddSuccessGet(request);
            //        }
            //        if (Utils.EqualsIgnoreCase(e.RouteResource, nameof(StlShoppingCart.ApiCartGet)))
            //        {
            //            return StlShoppingCart.ApiCartGet(request);
            //        }
            //        if (Utils.EqualsIgnoreCase(e.RouteResource, nameof(StlShoppingCart.ApiCartSave)))
            //        {
            //            return StlShoppingCart.ApiCartSave(request);
            //        }
            //        if (Utils.EqualsIgnoreCase(e.RouteResource, nameof(StlShoppingOrder.ApiOrderGet)))
            //        {
            //            return StlShoppingOrder.ApiOrderGet(request);
            //        }
            //        if (Utils.EqualsIgnoreCase(e.RouteResource, nameof(StlShoppingOrders.ApiOrdersGet)))
            //        {
            //            return StlShoppingOrders.ApiOrdersGet(request);
            //        }
            //        if (Utils.EqualsIgnoreCase(e.RouteResource, nameof(StlShoppingOrders.ApiOrdersRemove)))
            //        {
            //            return StlShoppingOrders.ApiOrdersRemove(request);
            //        }
            //        if (Utils.EqualsIgnoreCase(e.RouteResource, nameof(StlShoppingOrders.ApiOrdersPay)))
            //        {
            //            return StlShoppingOrders.ApiOrdersPay(request);
            //        }
            //        if (Utils.EqualsIgnoreCase(e.RouteResource, nameof(StlShoppingPay.ApiPayGet)))
            //        {
            //            return StlShoppingPay.ApiPayGet(request);
            //        }
            //        if (Utils.EqualsIgnoreCase(e.RouteResource, nameof(StlShoppingPay.ApiPaySaveAddress)))
            //        {
            //            return StlShoppingPay.ApiPaySaveAddress(request);
            //        }
            //        if (Utils.EqualsIgnoreCase(e.RouteResource, nameof(StlShoppingPay.ApiPayRemoveAddress)))
            //        {
            //            return StlShoppingPay.ApiPayRemoveAddress(request);
            //        }
            //        if (Utils.EqualsIgnoreCase(e.RouteResource, nameof(StlShoppingPay.ApiPaySetAddressAndDelivery)))
            //        {
            //            return StlShoppingPay.ApiPaySetAddressAndDelivery(request);
            //        }
            //        if (Utils.EqualsIgnoreCase(e.RouteResource, nameof(StlShoppingPay.ApiPay)))
            //        {
            //            return StlShoppingPay.ApiPay(request);
            //        }
            //        if (Utils.EqualsIgnoreCase(e.RouteResource, nameof(StlShoppingPay.ApiPayWeixinInterval)))
            //        {
            //            return StlShoppingPay.ApiPayWeixinInterval(request);
            //        }
            //        if (Utils.EqualsIgnoreCase(e.RouteResource, nameof(StlShoppingPaySuccess.ApiPaySuccessGet)))
            //        {
            //            return StlShoppingPaySuccess.ApiPaySuccessGet(request);
            //        }
            //    }

            //    throw new Exception("请求的资源不在服务器上");
            //};

            //service.RestApiGet += (sender, e) =>
            //{
            //    if (!string.IsNullOrEmpty(e.RouteResource))
            //    {
            //        if (Utils.EqualsIgnoreCase(e.RouteResource, nameof(StlShoppingPay.ApiPayQrCode)))
            //        {
            //            return StlShoppingPay.ApiPayQrCode(e.Request);
            //        }
            //    }

            //    throw new Exception("请求的资源不在服务器上");
            //};
        }
    }
}