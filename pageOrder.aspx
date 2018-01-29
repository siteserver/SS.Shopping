<%@ Page Language="C#" Inherits="SS.Shopping.Pages.PageOrder" %>
  <%@ Register TagPrefix="ctrl" Namespace="SS.Shopping.Controls" Assembly="SS.Shopping" %>
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
                  <asp:Literal id="LtlPageTitle" runat="server"></asp:Literal>
                </h4>
                <p class="text-muted m-b-25 font-13"></p>
                <asp:Literal id="LtlMessage" runat="server" />

                <div class="form-inline m-b-10">
                  <div class="form-group m-r-10">
                    <label>订单状态</label>
                    <asp:DropDownList id="DdlSearchState" class="form-control" runat="server"></asp:DropDownList>
                  </div>
                  <div class="form-group m-r-10">
                    <label>关键词</label>
                    <asp:TextBox id="TbSearchKeyword" class="form-control" runat="server"></asp:TextBox>
                  </div>
                  <asp:Button class="btn btn-primary btn-md" onclick="BtnSearch_OnClick" Text="查 询" runat="server" />
                </div>

                <table class="table table-bordered table-hover m-0">
                  <thead>
                    <tr class="info thead">
                      <th>订单号</th>
                      <th>姓名</th>
                      <th>手机</th>
                      <th>地区</th>
                      <th>支付方式</th>
                      <th>金额</th>
                      <th>商品数量</th>
                      <th class="text-center" style="width:160px;">添加时间</th>
                      <th class="text-center">状态</th>
                      <th width="20" class="text-center">
                        <input onclick="var checked = this.checked;$(':checkbox').each(function(){$(this)[0].checked = checked;checked ? $(this).parents('tr').addClass('success') : $(this).parents('tr').removeClass('success')});"
                          type="checkbox" />
                      </th>
                    </tr>
                  </thead>
                  <tbody>
                    <asp:Repeater ID="RptContents" runat="server">
                      <itemtemplate>
                        <tr onClick="$(this).toggleClass('success');$(this).find(':checkbox')[0].checked = $(this).hasClass('success');">
                          <td>
                            <asp:Literal ID="LtlGuid" runat="server"></asp:Literal>
                          </td>
                          <td>
                            <asp:Literal ID="ltlRealName" runat="server"></asp:Literal>
                          </td>
                          <td>
                            <asp:Literal ID="ltlMobile" runat="server"></asp:Literal>
                          </td>
                          <td>
                            <asp:Literal ID="ltlLocation" runat="server"></asp:Literal>
                          </td>
                          <td>
                            <asp:Literal ID="ltlChannel" runat="server"></asp:Literal>
                          </td>
                          <td>
                            <asp:Literal ID="ltlAmount" runat="server"></asp:Literal>
                          </td>
                          <td>
                            <asp:Literal ID="ltlTotalCount" runat="server"></asp:Literal>
                          </td>
                          <td class="text-center">
                            <asp:Literal ID="ltlDateTime" runat="server"></asp:Literal>
                          </td>
                          <td class="text-center">
                            <asp:Literal ID="ltlState" runat="server"></asp:Literal>
                          </td>
                          <td class="text-center">
                            <input type="checkbox" name="idCollection" value='<%#DataBinder.Eval(Container.DataItem, "Id")%>' />
                          </td>
                        </tr>
                      </itemtemplate>
                    </asp:Repeater>
                  </tbody>


                </table>

                <div class="m-b-25"></div>

                <ctrl:sqlPager id="SpContents" runat="server" class="table table-pager" />
                <asp:Button class="btn" id="BtnDelete" Text="删 除" runat="server" />
                <asp:Button class="btn" id="BtnStateAll" Text="更改状态" runat="server" />

              </div>
            </div>
          </div>
        </div>
        <!-- container end -->

        <!-- modalView start -->
        <asp:PlaceHolder id="PhModalView" visible="false" runat="server">
          <div id="modalView" class="modal fade" style="display: none;">
            <div class="modal-dialog" style="width:95%;">
              <div class="modal-content">
                <div class="modal-header">
                  <button type="button" class="close" onClick="$('#modalView').modal('hide');return false;" aria-hidden="true">×</button>
                  <h4 class="modal-title" id="modalLabel">
                    查看订单
                  </h4>
                </div>
                <div class="modal-body">

                  <asp:Literal ID="LtlModalViewMessage" runat="server"></asp:Literal>

                  <div class="form-horizontal" role="form">

                    <div class="form-group">
                      <label class="col-md-2 control-label">订单号</label>
                      <div class="col-md-4">
                        <p class="form-control-static">
                          <asp:Literal ID="LtlGuid" runat="server"></asp:Literal>
                        </p>
                      </div>
                      <label class="col-md-2 control-label">姓名</label>
                      <div class="col-md-4">
                        <p class="form-control-static">
                          <asp:Literal ID="LtlRealName" runat="server"></asp:Literal>
                        </p>
                      </div>
                    </div>
                    <div class="form-group">
                      <label class="col-md-2 control-label">手机</label>
                      <div class="col-md-4">
                        <p class="form-control-static">
                          <asp:Literal ID="LtlMobile" runat="server"></asp:Literal>
                        </p>
                      </div>
                      <label class="col-md-2 control-label">固定电话</label>
                      <div class="col-md-4">
                        <p class="form-control-static">
                          <asp:Literal ID="LtlTel" runat="server"></asp:Literal>
                        </p>
                      </div>
                    </div>
                    <div class="form-group">
                      <label class="col-md-2 control-label">快递区域</label>
                      <div class="col-md-10">
                        <p class="form-control-static">
                          <asp:Literal ID="LtlLocation" runat="server"></asp:Literal>
                        </p>
                      </div>
                    </div>
                    <div class="form-group">
                      <label class="col-md-2 control-label">快递详细地址</label>
                      <div class="col-md-10">
                        <p class="form-control-static">
                          <asp:Literal ID="LtlAddress" runat="server"></asp:Literal>
                        </p>
                      </div>
                    </div>
                    <div class="form-group">
                      <label class="col-md-2 control-label">支付渠道</label>
                      <div class="col-md-4">
                        <p class="form-control-static">
                          <asp:Literal ID="LtlChannel" runat="server"></asp:Literal>
                        </p>
                      </div>
                      <label class="col-md-2 control-label">订单金额</label>
                      <div class="col-md-4">
                        <p class="form-control-static">
                          <asp:Literal ID="LtlAmount" runat="server"></asp:Literal>
                        </p>
                      </div>
                    </div>
                    <div class="form-group">
                      <label class="col-md-2 control-label">下单时间</label>
                      <div class="col-md-4">
                        <p class="form-control-static">
                          <asp:Literal ID="LtlAddDate" runat="server"></asp:Literal>
                        </p>
                      </div>
                      <label class="col-md-2 control-label">订单状态</label>
                      <div class="col-md-4">
                        <p class="form-control-static">
                          <asp:Literal ID="LtlState" runat="server"></asp:Literal>
                        </p>
                      </div>
                    </div>

                  </div>

                  <table class="tablesaw m-t-20 table m-b-0 tablesaw-stack">
                    <thead>
                      <tr>
                        <th scope="col">商品名称</th>
                        <th scope="col">商品图片</th>
                        <th scope="col">购买数量</th>
                      </tr>
                    </thead>
                    <tbody>
                      <asp:Repeater ID="RptCarts" runat="server">
                        <itemtemplate>
                          <tr>
                            <td>
                              <span class="tablesaw-cell-content">
                                <asp:Literal ID="ltlTitle" runat="server"></asp:Literal>
                              </span>
                            </td>
                            <td>
                              <span class="tablesaw-cell-content">
                                <asp:Literal ID="ltlImage" runat="server"></asp:Literal>
                              </span>
                            </td>
                            <td>
                              <span class="tablesaw-cell-content">
                                <asp:Literal ID="ltlCount" runat="server"></asp:Literal>
                              </span>
                            </td>
                          </tr>
                        </itemtemplate>
                      </asp:Repeater>

                    </tbody>
                  </table>

                </div>
                <div class="modal-footer">
                  <asp:Button id="BtnState" class="btn btn-primary" runat="server" Text="更改状态"></asp:Button>
                  <button type="button" class="btn btn-default" onClick="$('#modalView').modal('hide');return false;">取 消</button>
                </div>
              </div>
            </div>
          </div>
          <script>
            $(document).ready(function () {
              $('#modalView').modal();
            });
          </script>
        </asp:PlaceHolder>
        <!-- modalView end -->

        <!-- modalState start -->
        <asp:PlaceHolder id="PhModalState" visible="false" runat="server">
          <div id="modalState" class="modal fade">
            <div class="modal-dialog" style="width:60%;">
              <div class="modal-content">
                <div class="modal-header">
                  <button type="button" class="close" onClick="$('#modalState').modal('hide');return false;" aria-hidden="true">×</button>
                  <h4 class="modal-title" id="modalLabel">
                    更改状态
                  </h4>
                </div>
                <div class="modal-body">
                  <div class="form-horizontal">

                    <div class="form-group">
                      <label class="col-sm-2 control-label">状态</label>
                      <div class="col-sm-4">
                        <asp:DropDownList ID="DdlState" class="form-control" runat="server">
                        </asp:DropDownList>
                      </div>
                      <div class="col-sm-6">
                        <span class="help-block"></span>
                      </div>
                    </div>

                  </div>
                </div>
                <div class="modal-footer">
                  <asp:Button class="btn btn-primary" onclick="BtnState_OnClick" runat="server" Text="提 交"></asp:Button>
                  <button type="button" class="btn btn-default" onClick="$('#modalState').modal('hide');return false;">取 消</button>
                </div>
              </div>
            </div>
          </div>
          <script>
            $(document).ready(function () {
              $('#modalState').modal();
            });
          </script>
        </asp:PlaceHolder>
        <!-- modalState end -->

      </form>
    </body>

    </html>