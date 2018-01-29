<%@ Page Language="C#" Inherits="SS.Shopping.Pages.PageDelivery" %>
  <!DOCTYPE html>
  <html>

  <head>
    <meta charset="utf-8">
    <link href="assets/plugin-utils/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="assets/plugin-utils/css/plugin-utils.css" rel="stylesheet" type="text/css" />
    <link href="assets/plugin-utils/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <script src="assets/plugin-utils/js/jquery.min.js"></script>
    <script src="assets/plugin-utils/js/bootstrap.min.js"></script>
    <script src="assets/js/sweetalert.min.js"></script>
  </head>

  <body>
    <form runat="server">

      <!-- container start -->
      <div class="container">
        <div class="m-b-25"></div>

        <div class="row">
          <div class="col-sm-12">
            <div class="card-box">
              <h4 class="text-dark  header-title m-t-0">
                运费管理
              </h4>
              <p class="text-muted m-b-25 font-13">请在此设置运费类型及费用明细。</p>
              <asp:Literal id="LtlMessage" runat="server" />

              <table class="table table-bordered table-hover m-0">
                <thead>
                  <tr class="info thead">
                    <th>名称</th>
                    <th>配送方式</th>
                    <th>费用明细</th>
                    <th class="text-center"></th>
                  </tr>
                </thead>
                <tbody>
                  <asp:Repeater ID="RptContents" runat="server">
                    <itemtemplate>
                      <tr>
                        <td>
                          <asp:Literal ID="ltlDeliveryName" runat="server"></asp:Literal>
                        </td>
                        <td>
                          <asp:Literal ID="ltlDeliveryType" runat="server"></asp:Literal>
                        </td>
                        <td>
                          <table class="table table-bordered table-hover m-0">
                            <thead>
                              <tr class="info thead">
                                <th class="text-center">配送范围</th>
                                <th class="text-center">首N件</th>
                                <th class="text-center">首费</th>
                                <th class="text-center">续M件</th>
                                <th class="text-center">续费</th>
                              </tr>
                            </thead>
                            <tbody>
                              <tr>
                                <td class="text-center">
                                  全国默认地区
                                </td>
                                <td class="text-center">
                                  <asp:Literal ID="ltlStartStandards" runat="server"></asp:Literal>
                                </td>
                                <td class="text-center">
                                  <asp:Literal ID="ltlStartFees" runat="server"></asp:Literal>
                                </td>
                                <td class="text-center">
                                  <asp:Literal ID="ltlAddStandards" runat="server"></asp:Literal>
                                </td>
                                <td class="text-center">
                                  <asp:Literal ID="ltlAddFees" runat="server"></asp:Literal>
                                </td>
                              </tr>
                              <asp:Repeater ID="RptAreas" runat="server">
                                <itemtemplate>
                                  <tr>
                                    <td class="text-center">
                                      <asp:Literal ID="ltlCities" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-center">
                                      <asp:Literal ID="ltlStartStandards" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-center">
                                      <asp:Literal ID="ltlStartFees" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-center">
                                      <asp:Literal ID="ltlAddStandards" runat="server"></asp:Literal>
                                    </td>
                                    <td class="text-center">
                                      <asp:Literal ID="ltlAddFees" runat="server"></asp:Literal>
                                    </td>
                                  </tr>
                                </itemtemplate>
                              </asp:Repeater>
                            </tbody>
                          </table>
                        </td>
                        <td class="text-center">
                          <asp:Literal ID="ltlActions" runat="server"></asp:Literal>
                        </td>
                      </tr>
                    </itemtemplate>
                  </asp:Repeater>
                </tbody>
              </table>

              <div class="m-b-25"></div>

              <asp:Button class="btn" onclick="BtnAdd_OnClick" Text="新 增" runat="server" />

            </div>
          </div>
        </div>
      </div>
      <!-- container end -->

    </form>
  </body>

  </html>