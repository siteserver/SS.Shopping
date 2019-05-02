<%@ Page Language="C#" Inherits="SS.Shopping.Core.Pages.PageDeliveryAdd" %>
  <!DOCTYPE html>
  <html>

  <head>
    <meta charset="utf-8">
    <link href="../assets/plugin-utils/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../assets/plugin-utils/css/plugin-utils.css" rel="stylesheet" type="text/css" />
    <link href="../assets/plugin-utils/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../assets/plugin-utils/css/ionicons.min.css" rel="stylesheet" type="text/css" />
    <script src="../assets/plugin-utils/js/jquery.min.js"></script>
    <script src="../assets/plugin-utils/js/bootstrap.min.js"></script>
    <script src="../assets/js/sweetalert.min.js"></script>
  </head>

  <body>
    <div style="padding: 20px 0;">

      <div class="container">
        <form id="form" runat="server" class="form-horizontal">

          <div class="row">
            <div class="card-box">
              <div class="row">
                <div class="col-lg-10">
                  <h4 class="m-t-0 header-title">
                    <b>
                      编辑运费
                    </b>
                  </h4>
                  <p class="text-muted font-13 m-b-30"></p>
                </div>
              </div>

              <asp:Literal id="LtlMessage" runat="server" />

              <div class="form-horizontal">

                <div class="form-group">
                  <label class="col-sm-2 control-label">运费名称</label>
                  <div class="col-sm-3">
                    <asp:TextBox id="TbDeliveryName" class="form-control" runat="server"></asp:TextBox>
                  </div>
                  <div class="col-sm-7">
                    <asp:RequiredFieldValidator ControlToValidate="TbDeliveryName" errorMessage=" *" foreColor="red" display="Dynamic" runat="server"
                    />
                  </div>
                </div>

                <div class="form-group">
                  <label class="col-sm-2 control-label">配送方式</label>
                  <div class="col-sm-3">
                    <asp:DropDownList ID="DdlDeliveryType" class="form-control" runat="server">
                      <asp:ListItem Text="平邮" Value="平邮" />
                      <asp:ListItem Text="快递" Value="快递" />
                      <asp:ListItem Text="EMS" Value="EMS" />
                    </asp:DropDownList>
                  </div>
                  <div class="col-sm-7">

                  </div>
                </div>

                <div class="form-group">
                  <label class="col-sm-2 control-label">运费设置</label>
                  <div class="col-sm-10">
                    <table class="table table-bordered table-hover m-0">
                      <thead>
                        <tr class="info thead">
                          <th>配送区域</th>
                          <th>首N件</th>
                          <th>首费(￥)</th>
                          <th>续M件</th>
                          <th>续费(￥)</th>
                        </tr>
                      </thead>
                      <tbody>
                        <tr>
                          <td class="text-center">
                            全国默认地区
                          </td>
                          <td class="text-center">
                            <asp:TextBox id="TbStartStandards" class="form-control" runat="server"></asp:TextBox>
                          </td>
                          <td class="text-center">
                            <asp:TextBox ID="TbStartFees" class="form-control" runat="server"></asp:TextBox>
                          </td>
                          <td class="text-center">
                            <asp:TextBox ID="TbAddStandards" class="form-control" runat="server"></asp:TextBox>
                          </td>
                          <td class="text-center">
                            <asp:TextBox ID="TbAddFees" class="form-control" runat="server"></asp:TextBox>
                          </td>
                        </tr>
                        <asp:Repeater ID="RptAreas" runat="server">
                          <itemtemplate>
                            <tr>
                              <td class="text-center">
                                <asp:HiddenField ID="hfAreaId" runat="server"></asp:HiddenField>
                                <asp:Literal ID="ltlCities" runat="server"></asp:Literal>
                              </td>
                              <td class="text-center">
                                <asp:TextBox id="tbStartStandards" class="form-control" runat="server"></asp:TextBox>
                              </td>
                              <td class="text-center">
                                <asp:TextBox ID="tbStartFees" class="form-control" runat="server"></asp:TextBox>
                              </td>
                              <td class="text-center">
                                <asp:TextBox ID="tbAddStandards" class="form-control" runat="server"></asp:TextBox>
                              </td>
                              <td class="text-center">
                                <asp:TextBox ID="tbAddFees" class="form-control" runat="server"></asp:TextBox>
                              </td>
                            </tr>
                          </itemtemplate>
                        </asp:Repeater>
                        <tr>
                          <td colspan="5">
                            <asp:Button id="BtnAreaAdd" class="btn btn-success" Text="为指定地区设置运费" runat="server" />
                          </td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                </div>

                <hr />

                <div class="form-group m-b-0">
                  <div class="col-sm-offset-2 col-sm-10">
                    <asp:Button class="btn btn-primary m-r-5" onclick="BtnSubmit_OnClick" Text="保 存" runat="server" />
                    <asp:Button class="btn" onclick="BtnCancel_OnClick" Text="取 消" runat="server" />
                  </div>
                </div>

              </div>
            </div>
          </div>

          <!-- modalAreas start -->
          <asp:PlaceHolder id="PhModalAreas" visible="false" runat="server">
            <div id="modalAreas" class="modal fade">
              <div class="modal-dialog" style="width:80%;">
                <div class="modal-content">
                  <div class="modal-header">
                    <button type="button" class="close" onClick="$('#modalAreas').modal('hide');return false;" aria-hidden="true">×</button>
                    <h4 class="modal-title" id="modalLabel">
                      选择地区
                    </h4>
                  </div>
                  <div class="modal-body">
                    <div class="form-horizontal">

                      <div class="form-group">
                        <div id="divAreas" class="col-sm-6" style="overflow: scroll">

                          <div class="panel-group">
                            <div class="panel panel-default">
                              <div class="panel-heading">
                                <h4 class="panel-title">
                                  <a data-toggle="collapse" href="#collapse-1" class="collapsed">
                                    安徽省
                                  </a>
                                </h4>
                              </div>
                              <div id="collapse-1" class="panel-collapse collapse" style="height: 0px;">
                                <div class="panel-body">
                                  <div class="list-group">
                                    <a id="source-合肥市" href="javascript:;" onclick="addCity('合肥市', this)" class="list-group-item" data-value="合肥市">合肥市</a>
                                    <a id="source-芜湖市" href="javascript:;" onclick="addCity('芜湖市', this)" class="list-group-item" data-value="芜湖市">芜湖市</a>
                                    <a id="source-蚌埠市" href="javascript:;" onclick="addCity('蚌埠市', this)" class="list-group-item" data-value="蚌埠市">蚌埠市</a>
                                    <a id="source-淮南市" href="javascript:;" onclick="addCity('淮南市', this)" class="list-group-item" data-value="淮南市">淮南市</a>
                                    <a id="source-马鞍山市" href="javascript:;" onclick="addCity('马鞍山市', this)" class="list-group-item" data-value="马鞍山市">马鞍山市</a>
                                    <a id="source-淮北市" href="javascript:;" onclick="addCity('淮北市', this)" class="list-group-item" data-value="淮北市">淮北市</a>
                                    <a id="source-铜陵市" href="javascript:;" onclick="addCity('铜陵市', this)" class="list-group-item" data-value="铜陵市">铜陵市</a>
                                    <a id="source-安庆市" href="javascript:;" onclick="addCity('安庆市', this)" class="list-group-item" data-value="安庆市">安庆市</a>
                                    <a id="source-黄山市" href="javascript:;" onclick="addCity('黄山市', this)" class="list-group-item" data-value="黄山市">黄山市</a>
                                    <a id="source-滁州市" href="javascript:;" onclick="addCity('滁州市', this)" class="list-group-item" data-value="滁州市">滁州市</a>
                                    <a id="source-阜阳市" href="javascript:;" onclick="addCity('阜阳市', this)" class="list-group-item" data-value="阜阳市">阜阳市</a>
                                    <a id="source-宿州市" href="javascript:;" onclick="addCity('宿州市', this)" class="list-group-item" data-value="宿州市">宿州市</a>
                                    <a id="source-六安市" href="javascript:;" onclick="addCity('六安市', this)" class="list-group-item" data-value="六安市">六安市</a>
                                    <a id="source-亳州市" href="javascript:;" onclick="addCity('亳州市', this)" class="list-group-item" data-value="亳州市">亳州市</a>
                                    <a id="source-池州市" href="javascript:;" onclick="addCity('池州市', this)" class="list-group-item" data-value="池州市">池州市</a>
                                    <a id="source-宣城市" href="javascript:;" onclick="addCity('宣城市', this)" class="list-group-item" data-value="宣城市">宣城市</a>
                                  </div>
                                </div>
                              </div>
                            </div>

                            <div class="panel panel-default">
                              <div class="panel-heading">
                                <h4 class="panel-title">
                                  <a data-toggle="collapse" href="#collapse-2" class="collapsed">
                                    澳门
                                  </a>
                                </h4>
                              </div>
                              <div id="collapse-2" class="panel-collapse collapse" style="height: 0px;">
                                <div class="panel-body">
                                  <div class="list-group">
                                    <a id="source-澳门" href="javascript:;" onclick="addCity('澳门', this)" class="list-group-item" data-value="澳门">澳门</a>
                                  </div>
                                </div>
                              </div>
                            </div>

                            <div class="panel panel-default">
                              <div class="panel-heading">
                                <h4 class="panel-title">
                                  <a data-toggle="collapse" href="#collapse-3" class="collapsed">
                                    北京市
                                  </a>
                                </h4>
                              </div>
                              <div id="collapse-3" class="panel-collapse collapse" style="height: 0px;">
                                <div class="panel-body">
                                  <div class="list-group">
                                    <a id="source-北京市" href="javascript:;" onclick="addCity('北京市', this)" class="list-group-item" data-value="北京市">北京市</a>
                                  </div>
                                </div>
                              </div>
                            </div>

                            <div class="panel panel-default">
                              <div class="panel-heading">
                                <h4 class="panel-title">
                                  <a data-toggle="collapse" href="#collapse-4" class="collapsed">
                                    重庆市
                                  </a>
                                </h4>
                              </div>
                              <div id="collapse-4" class="panel-collapse collapse" style="height: 0px;">
                                <div class="panel-body">
                                  <div class="list-group">
                                    <a id="source-重庆市" href="javascript:;" onclick="addCity('重庆市', this)" class="list-group-item" data-value="重庆市">重庆市</a>
                                  </div>
                                </div>
                              </div>
                            </div>

                            <div class="panel panel-default">
                              <div class="panel-heading">
                                <h4 class="panel-title">
                                  <a data-toggle="collapse" href="#collapse-5" class="collapsed">
                                    福建省
                                  </a>
                                </h4>
                              </div>
                              <div id="collapse-5" class="panel-collapse collapse" style="height: 0px;">
                                <div class="panel-body">
                                  <div class="list-group">
                                    <a id="source-福州市" href="javascript:;" onclick="addCity('福州市', this)" class="list-group-item" data-value="福州市">福州市</a>
                                    <a id="source-厦门市" href="javascript:;" onclick="addCity('厦门市', this)" class="list-group-item" data-value="厦门市">厦门市</a>
                                    <a id="source-莆田市" href="javascript:;" onclick="addCity('莆田市', this)" class="list-group-item" data-value="莆田市">莆田市</a>
                                    <a id="source-三明市" href="javascript:;" onclick="addCity('三明市', this)" class="list-group-item" data-value="三明市">三明市</a>
                                    <a id="source-泉州市" href="javascript:;" onclick="addCity('泉州市', this)" class="list-group-item" data-value="泉州市">泉州市</a>
                                    <a id="source-漳州市" href="javascript:;" onclick="addCity('漳州市', this)" class="list-group-item" data-value="漳州市">漳州市</a>
                                    <a id="source-南平市" href="javascript:;" onclick="addCity('南平市', this)" class="list-group-item" data-value="南平市">南平市</a>
                                    <a id="source-龙岩市" href="javascript:;" onclick="addCity('龙岩市', this)" class="list-group-item" data-value="龙岩市">龙岩市</a>
                                    <a id="source-宁德市" href="javascript:;" onclick="addCity('宁德市', this)" class="list-group-item" data-value="宁德市">宁德市</a>
                                  </div>
                                </div>
                              </div>
                            </div>

                            <div class="panel panel-default">
                              <div class="panel-heading">
                                <h4 class="panel-title">
                                  <a data-toggle="collapse" href="#collapse-6" class="collapsed">
                                    广东省
                                  </a>
                                </h4>
                              </div>
                              <div id="collapse-6" class="panel-collapse collapse" style="height: 0px;">
                                <div class="panel-body">
                                  <div class="list-group">
                                    <a id="source-广州市" href="javascript:;" onclick="addCity('广州市', this)" class="list-group-item" data-value="广州市">广州市</a>
                                    <a id="source-韶关市" href="javascript:;" onclick="addCity('韶关市', this)" class="list-group-item" data-value="韶关市">韶关市</a>
                                    <a id="source-深圳市" href="javascript:;" onclick="addCity('深圳市', this)" class="list-group-item" data-value="深圳市">深圳市</a>
                                    <a id="source-珠海市" href="javascript:;" onclick="addCity('珠海市', this)" class="list-group-item" data-value="珠海市">珠海市</a>
                                    <a id="source-汕头市" href="javascript:;" onclick="addCity('汕头市', this)" class="list-group-item" data-value="汕头市">汕头市</a>
                                    <a id="source-佛山市" href="javascript:;" onclick="addCity('佛山市', this)" class="list-group-item" data-value="佛山市">佛山市</a>
                                    <a id="source-江门市" href="javascript:;" onclick="addCity('江门市', this)" class="list-group-item" data-value="江门市">江门市</a>
                                    <a id="source-湛江市" href="javascript:;" onclick="addCity('湛江市', this)" class="list-group-item" data-value="湛江市">湛江市</a>
                                    <a id="source-茂名市" href="javascript:;" onclick="addCity('茂名市', this)" class="list-group-item" data-value="茂名市">茂名市</a>
                                    <a id="source-肇庆市" href="javascript:;" onclick="addCity('肇庆市', this)" class="list-group-item" data-value="肇庆市">肇庆市</a>
                                    <a id="source-惠州市" href="javascript:;" onclick="addCity('惠州市', this)" class="list-group-item" data-value="惠州市">惠州市</a>
                                    <a id="source-梅州市" href="javascript:;" onclick="addCity('梅州市', this)" class="list-group-item" data-value="梅州市">梅州市</a>
                                    <a id="source-汕尾市" href="javascript:;" onclick="addCity('汕尾市', this)" class="list-group-item" data-value="汕尾市">汕尾市</a>
                                    <a id="source-河源市" href="javascript:;" onclick="addCity('河源市', this)" class="list-group-item" data-value="河源市">河源市</a>
                                    <a id="source-阳江市" href="javascript:;" onclick="addCity('阳江市', this)" class="list-group-item" data-value="阳江市">阳江市</a>
                                    <a id="source-清远市" href="javascript:;" onclick="addCity('清远市', this)" class="list-group-item" data-value="清远市">清远市</a>
                                    <a id="source-东莞市" href="javascript:;" onclick="addCity('东莞市', this)" class="list-group-item" data-value="东莞市">东莞市</a>
                                    <a id="source-中山市" href="javascript:;" onclick="addCity('中山市', this)" class="list-group-item" data-value="中山市">中山市</a>
                                    <a id="source-潮州市" href="javascript:;" onclick="addCity('潮州市', this)" class="list-group-item" data-value="潮州市">潮州市</a>
                                    <a id="source-揭阳市" href="javascript:;" onclick="addCity('揭阳市', this)" class="list-group-item" data-value="揭阳市">揭阳市</a>
                                    <a id="source-云浮市" href="javascript:;" onclick="addCity('云浮市', this)" class="list-group-item" data-value="云浮市">云浮市</a>
                                  </div>
                                </div>
                              </div>
                            </div>

                            <div class="panel panel-default">
                              <div class="panel-heading">
                                <h4 class="panel-title">
                                  <a data-toggle="collapse" href="#collapse-7" class="collapsed">
                                    甘肃省
                                  </a>
                                </h4>
                              </div>
                              <div id="collapse-7" class="panel-collapse collapse" style="height: 0px;">
                                <div class="panel-body">
                                  <div class="list-group">
                                    <a id="source-兰州市" href="javascript:;" onclick="addCity('兰州市', this)" class="list-group-item" data-value="兰州市">兰州市</a>
                                    <a id="source-嘉峪关市" href="javascript:;" onclick="addCity('嘉峪关市', this)" class="list-group-item" data-value="嘉峪关市">嘉峪关市</a>
                                    <a id="source-白银市" href="javascript:;" onclick="addCity('白银市', this)" class="list-group-item" data-value="白银市">白银市</a>
                                    <a id="source-武威市" href="javascript:;" onclick="addCity('武威市', this)" class="list-group-item" data-value="武威市">武威市</a>
                                    <a id="source-张掖市" href="javascript:;" onclick="addCity('张掖市', this)" class="list-group-item" data-value="张掖市">张掖市</a>
                                    <a id="source-平凉市" href="javascript:;" onclick="addCity('平凉市', this)" class="list-group-item" data-value="平凉市">平凉市</a>
                                    <a id="source-酒泉市" href="javascript:;" onclick="addCity('酒泉市', this)" class="list-group-item" data-value="酒泉市">酒泉市</a>
                                    <a id="source-庆阳市" href="javascript:;" onclick="addCity('庆阳市', this)" class="list-group-item" data-value="庆阳市">庆阳市</a>
                                    <a id="source-定西市" href="javascript:;" onclick="addCity('定西市', this)" class="list-group-item" data-value="定西市">定西市</a>
                                    <a id="source-陇南市" href="javascript:;" onclick="addCity('陇南市', this)" class="list-group-item" data-value="陇南市">陇南市</a>
                                    <a id="source-临夏回族自治州" href="javascript:;" onclick="addCity('临夏回族自治州', this)" class="list-group-item" data-value="临夏回族自治州">临夏回族自治州</a>
                                    <a id="source-甘南藏族自治州" href="javascript:;" onclick="addCity('甘南藏族自治州', this)" class="list-group-item" data-value="甘南藏族自治州">甘南藏族自治州</a>
                                  </div>
                                </div>
                              </div>
                            </div>

                            <div class="panel panel-default">
                              <div class="panel-heading">
                                <h4 class="panel-title">
                                  <a data-toggle="collapse" href="#collapse-8" class="collapsed">
                                    广西壮族自治区
                                  </a>
                                </h4>
                              </div>
                              <div id="collapse-8" class="panel-collapse collapse" style="height: 0px;">
                                <div class="panel-body">
                                  <div class="list-group">
                                    <a id="source-南宁市" href="javascript:;" onclick="addCity('南宁市', this)" class="list-group-item" data-value="南宁市">南宁市</a>
                                    <a id="source-柳州市" href="javascript:;" onclick="addCity('柳州市', this)" class="list-group-item" data-value="柳州市">柳州市</a>
                                    <a id="source-桂林市" href="javascript:;" onclick="addCity('桂林市', this)" class="list-group-item" data-value="桂林市">桂林市</a>
                                    <a id="source-梧州市" href="javascript:;" onclick="addCity('梧州市', this)" class="list-group-item" data-value="梧州市">梧州市</a>
                                    <a id="source-北海市" href="javascript:;" onclick="addCity('北海市', this)" class="list-group-item" data-value="北海市">北海市</a>
                                    <a id="source-防城港市" href="javascript:;" onclick="addCity('防城港市', this)" class="list-group-item" data-value="防城港市">防城港市</a>
                                    <a id="source-钦州市" href="javascript:;" onclick="addCity('钦州市', this)" class="list-group-item" data-value="钦州市">钦州市</a>
                                    <a id="source-贵港市" href="javascript:;" onclick="addCity('贵港市', this)" class="list-group-item" data-value="贵港市">贵港市</a>
                                    <a id="source-玉林市" href="javascript:;" onclick="addCity('玉林市', this)" class="list-group-item" data-value="玉林市">玉林市</a>
                                    <a id="source-百色市" href="javascript:;" onclick="addCity('百色市', this)" class="list-group-item" data-value="百色市">百色市</a>
                                    <a id="source-贺州市" href="javascript:;" onclick="addCity('贺州市', this)" class="list-group-item" data-value="贺州市">贺州市</a>
                                    <a id="source-河池市" href="javascript:;" onclick="addCity('河池市', this)" class="list-group-item" data-value="河池市">河池市</a>
                                    <a id="source-来宾市" href="javascript:;" onclick="addCity('来宾市', this)" class="list-group-item" data-value="来宾市">来宾市</a>
                                    <a id="source-崇左市" href="javascript:;" onclick="addCity('崇左市', this)" class="list-group-item" data-value="崇左市">崇左市</a>
                                  </div>
                                </div>
                              </div>
                            </div>

                            <div class="panel panel-default">
                              <div class="panel-heading">
                                <h4 class="panel-title">
                                  <a data-toggle="collapse" href="#collapse-9" class="collapsed">
                                    贵州省
                                  </a>
                                </h4>
                              </div>
                              <div id="collapse-9" class="panel-collapse collapse" style="height: 0px;">
                                <div class="panel-body">
                                  <div class="list-group">
                                    <a id="source-贵阳市" href="javascript:;" onclick="addCity('贵阳市', this)" class="list-group-item" data-value="贵阳市">贵阳市</a>
                                    <a id="source-六盘水市" href="javascript:;" onclick="addCity('六盘水市', this)" class="list-group-item" data-value="六盘水市">六盘水市</a>
                                    <a id="source-遵义市" href="javascript:;" onclick="addCity('遵义市', this)" class="list-group-item" data-value="遵义市">遵义市</a>
                                    <a id="source-安顺市" href="javascript:;" onclick="addCity('安顺市', this)" class="list-group-item" data-value="安顺市">安顺市</a>
                                    <a id="source-铜仁市" href="javascript:;" onclick="addCity('铜仁市', this)" class="list-group-item" data-value="铜仁市">铜仁市</a>
                                    <a id="source-黔西南布依族苗族自治州" href="javascript:;" onclick="addCity('黔西南布依族苗族自治州', this)" class="list-group-item" data-value="黔西南布依族苗族自治州">黔西南布依族苗族自治州</a>
                                    <a id="source-毕节" href="javascript:;" onclick="addCity('毕节', this)" class="list-group-item" data-value="毕节">毕节</a>
                                    <a id="source-黔东南苗族侗族自治州" href="javascript:;" onclick="addCity('黔东南苗族侗族自治州', this)" class="list-group-item" data-value="黔东南苗族侗族自治州">黔东南苗族侗族自治州</a>
                                    <a id="source-黔南布依族苗族自治州" href="javascript:;" onclick="addCity('黔南布依族苗族自治州', this)" class="list-group-item" data-value="黔南布依族苗族自治州">黔南布依族苗族自治州</a>
                                  </div>
                                </div>
                              </div>
                            </div>

                            <div class="panel panel-default">
                              <div class="panel-heading">
                                <h4 class="panel-title">
                                  <a data-toggle="collapse" href="#collapse-10" class="collapsed">
                                    河北省
                                  </a>
                                </h4>
                              </div>
                              <div id="collapse-10" class="panel-collapse collapse" style="height: 0px;">
                                <div class="panel-body">
                                  <div class="list-group">
                                    <a id="source-石家庄市" href="javascript:;" onclick="addCity('石家庄市', this)" class="list-group-item" data-value="石家庄市">石家庄市</a>
                                    <a id="source-唐山市" href="javascript:;" onclick="addCity('唐山市', this)" class="list-group-item" data-value="唐山市">唐山市</a>
                                    <a id="source-秦皇岛市" href="javascript:;" onclick="addCity('秦皇岛市', this)" class="list-group-item" data-value="秦皇岛市">秦皇岛市</a>
                                    <a id="source-邯郸市" href="javascript:;" onclick="addCity('邯郸市', this)" class="list-group-item" data-value="邯郸市">邯郸市</a>
                                    <a id="source-邢台市" href="javascript:;" onclick="addCity('邢台市', this)" class="list-group-item" data-value="邢台市">邢台市</a>
                                    <a id="source-保定市" href="javascript:;" onclick="addCity('保定市', this)" class="list-group-item" data-value="保定市">保定市</a>
                                    <a id="source-张家口市" href="javascript:;" onclick="addCity('张家口市', this)" class="list-group-item" data-value="张家口市">张家口市</a>
                                    <a id="source-承德市" href="javascript:;" onclick="addCity('承德市', this)" class="list-group-item" data-value="承德市">承德市</a>
                                    <a id="source-沧州市" href="javascript:;" onclick="addCity('沧州市', this)" class="list-group-item" data-value="沧州市">沧州市</a>
                                    <a id="source-廊坊市" href="javascript:;" onclick="addCity('廊坊市', this)" class="list-group-item" data-value="廊坊市">廊坊市</a>
                                    <a id="source-衡水市" href="javascript:;" onclick="addCity('衡水市', this)" class="list-group-item" data-value="衡水市">衡水市</a>
                                  </div>
                                </div>
                              </div>
                            </div>

                            <div class="panel panel-default">
                              <div class="panel-heading">
                                <h4 class="panel-title">
                                  <a data-toggle="collapse" href="#collapse-11" class="collapsed">
                                    湖北省
                                  </a>
                                </h4>
                              </div>
                              <div id="collapse-11" class="panel-collapse collapse" style="height: 0px;">
                                <div class="panel-body">
                                  <div class="list-group">
                                    <a id="source-武汉市" href="javascript:;" onclick="addCity('武汉市', this)" class="list-group-item" data-value="武汉市">武汉市</a>
                                    <a id="source-黄石市" href="javascript:;" onclick="addCity('黄石市', this)" class="list-group-item" data-value="黄石市">黄石市</a>
                                    <a id="source-十堰市" href="javascript:;" onclick="addCity('十堰市', this)" class="list-group-item" data-value="十堰市">十堰市</a>
                                    <a id="source-宜昌市" href="javascript:;" onclick="addCity('宜昌市', this)" class="list-group-item" data-value="宜昌市">宜昌市</a>
                                    <a id="source-鄂州市" href="javascript:;" onclick="addCity('鄂州市', this)" class="list-group-item" data-value="鄂州市">鄂州市</a>
                                    <a id="source-荆门市" href="javascript:;" onclick="addCity('荆门市', this)" class="list-group-item" data-value="荆门市">荆门市</a>
                                    <a id="source-孝感市" href="javascript:;" onclick="addCity('孝感市', this)" class="list-group-item" data-value="孝感市">孝感市</a>
                                    <a id="source-荆州市" href="javascript:;" onclick="addCity('荆州市', this)" class="list-group-item" data-value="荆州市">荆州市</a>
                                    <a id="source-黄冈市" href="javascript:;" onclick="addCity('黄冈市', this)" class="list-group-item" data-value="黄冈市">黄冈市</a>
                                    <a id="source-咸宁市" href="javascript:;" onclick="addCity('咸宁市', this)" class="list-group-item" data-value="咸宁市">咸宁市</a>
                                    <a id="source-随州市" href="javascript:;" onclick="addCity('随州市', this)" class="list-group-item" data-value="随州市">随州市</a>
                                    <a id="source-恩施土家族苗族自治州" href="javascript:;" onclick="addCity('恩施土家族苗族自治州', this)" class="list-group-item" data-value="恩施土家族苗族自治州">恩施土家族苗族自治州</a>
                                    <a id="source-襄阳市" href="javascript:;" onclick="addCity('襄阳市', this)" class="list-group-item" data-value="襄阳市">襄阳市</a>
                                  </div>
                                </div>
                              </div>
                            </div>

                            <div class="panel panel-default">
                              <div class="panel-heading">
                                <h4 class="panel-title">
                                  <a data-toggle="collapse" href="#collapse-12" class="collapsed">
                                    黑龙江省
                                  </a>
                                </h4>
                              </div>
                              <div id="collapse-12" class="panel-collapse collapse" style="height: 0px;">
                                <div class="panel-body">
                                  <div class="list-group">
                                    <a id="source-哈尔滨市" href="javascript:;" onclick="addCity('哈尔滨市', this)" class="list-group-item" data-value="哈尔滨市">哈尔滨市</a>
                                    <a id="source-齐齐哈尔市" href="javascript:;" onclick="addCity('齐齐哈尔市', this)" class="list-group-item" data-value="齐齐哈尔市">齐齐哈尔市</a>
                                    <a id="source-鸡西市" href="javascript:;" onclick="addCity('鸡西市', this)" class="list-group-item" data-value="鸡西市">鸡西市</a>
                                    <a id="source-鹤岗市" href="javascript:;" onclick="addCity('鹤岗市', this)" class="list-group-item" data-value="鹤岗市">鹤岗市</a>
                                    <a id="source-双鸭山市" href="javascript:;" onclick="addCity('双鸭山市', this)" class="list-group-item" data-value="双鸭山市">双鸭山市</a>
                                    <a id="source-大庆市" href="javascript:;" onclick="addCity('大庆市', this)" class="list-group-item" data-value="大庆市">大庆市</a>
                                    <a id="source-伊春市" href="javascript:;" onclick="addCity('伊春市', this)" class="list-group-item" data-value="伊春市">伊春市</a>
                                    <a id="source-佳木斯市" href="javascript:;" onclick="addCity('佳木斯市', this)" class="list-group-item" data-value="佳木斯市">佳木斯市</a>
                                    <a id="source-七台河市" href="javascript:;" onclick="addCity('七台河市', this)" class="list-group-item" data-value="七台河市">七台河市</a>
                                    <a id="source-牡丹江市" href="javascript:;" onclick="addCity('牡丹江市', this)" class="list-group-item" data-value="牡丹江市">牡丹江市</a>
                                    <a id="source-黑河市" href="javascript:;" onclick="addCity('黑河市', this)" class="list-group-item" data-value="黑河市">黑河市</a>
                                    <a id="source-绥化市" href="javascript:;" onclick="addCity('绥化市', this)" class="list-group-item" data-value="绥化市">绥化市</a>
                                    <a id="source-大兴安岭地区" href="javascript:;" onclick="addCity('大兴安岭地区', this)" class="list-group-item" data-value="大兴安岭地区">大兴安岭地区</a>
                                  </div>
                                </div>
                              </div>
                            </div>

                            <div class="panel panel-default">
                              <div class="panel-heading">
                                <h4 class="panel-title">
                                  <a data-toggle="collapse" href="#collapse-13" class="collapsed">
                                    河南省
                                  </a>
                                </h4>
                              </div>
                              <div id="collapse-13" class="panel-collapse collapse" style="height: 0px;">
                                <div class="panel-body">
                                  <div class="list-group">
                                    <a id="source-郑州市" href="javascript:;" onclick="addCity('郑州市', this)" class="list-group-item" data-value="郑州市">郑州市</a>
                                    <a id="source-开封市" href="javascript:;" onclick="addCity('开封市', this)" class="list-group-item" data-value="开封市">开封市</a>
                                    <a id="source-洛阳市" href="javascript:;" onclick="addCity('洛阳市', this)" class="list-group-item" data-value="洛阳市">洛阳市</a>
                                    <a id="source-平顶山市" href="javascript:;" onclick="addCity('平顶山市', this)" class="list-group-item" data-value="平顶山市">平顶山市</a>
                                    <a id="source-安阳市" href="javascript:;" onclick="addCity('安阳市', this)" class="list-group-item" data-value="安阳市">安阳市</a>
                                    <a id="source-鹤壁市" href="javascript:;" onclick="addCity('鹤壁市', this)" class="list-group-item" data-value="鹤壁市">鹤壁市</a>
                                    <a id="source-新乡市" href="javascript:;" onclick="addCity('新乡市', this)" class="list-group-item" data-value="新乡市">新乡市</a>
                                    <a id="source-焦作市" href="javascript:;" onclick="addCity('焦作市', this)" class="list-group-item" data-value="焦作市">焦作市</a>
                                    <a id="source-濮阳市" href="javascript:;" onclick="addCity('濮阳市', this)" class="list-group-item" data-value="濮阳市">濮阳市</a>
                                    <a id="source-许昌市" href="javascript:;" onclick="addCity('许昌市', this)" class="list-group-item" data-value="许昌市">许昌市</a>
                                    <a id="source-漯河市" href="javascript:;" onclick="addCity('漯河市', this)" class="list-group-item" data-value="漯河市">漯河市</a>
                                    <a id="source-三门峡市" href="javascript:;" onclick="addCity('三门峡市', this)" class="list-group-item" data-value="三门峡市">三门峡市</a>
                                    <a id="source-南阳市" href="javascript:;" onclick="addCity('南阳市', this)" class="list-group-item" data-value="南阳市">南阳市</a>
                                    <a id="source-商丘市" href="javascript:;" onclick="addCity('商丘市', this)" class="list-group-item" data-value="商丘市">商丘市</a>
                                    <a id="source-信阳市" href="javascript:;" onclick="addCity('信阳市', this)" class="list-group-item" data-value="信阳市">信阳市</a>
                                    <a id="source-周口市" href="javascript:;" onclick="addCity('周口市', this)" class="list-group-item" data-value="周口市">周口市</a>
                                    <a id="source-驻马店市" href="javascript:;" onclick="addCity('驻马店市', this)" class="list-group-item" data-value="驻马店市">驻马店市</a>
                                  </div>
                                </div>
                              </div>
                            </div>

                            <div class="panel panel-default">
                              <div class="panel-heading">
                                <h4 class="panel-title">
                                  <a data-toggle="collapse" href="#collapse-14" class="collapsed">
                                    湖南省
                                  </a>
                                </h4>
                              </div>
                              <div id="collapse-14" class="panel-collapse collapse" style="height: 0px;">
                                <div class="panel-body">
                                  <div class="list-group">
                                    <a id="source-长沙市" href="javascript:;" onclick="addCity('长沙市', this)" class="list-group-item" data-value="长沙市">长沙市</a>
                                    <a id="source-株洲市" href="javascript:;" onclick="addCity('株洲市', this)" class="list-group-item" data-value="株洲市">株洲市</a>
                                    <a id="source-湘潭市" href="javascript:;" onclick="addCity('湘潭市', this)" class="list-group-item" data-value="湘潭市">湘潭市</a>
                                    <a id="source-衡阳市" href="javascript:;" onclick="addCity('衡阳市', this)" class="list-group-item" data-value="衡阳市">衡阳市</a>
                                    <a id="source-邵阳市" href="javascript:;" onclick="addCity('邵阳市', this)" class="list-group-item" data-value="邵阳市">邵阳市</a>
                                    <a id="source-岳阳市" href="javascript:;" onclick="addCity('岳阳市', this)" class="list-group-item" data-value="岳阳市">岳阳市</a>
                                    <a id="source-常德市" href="javascript:;" onclick="addCity('常德市', this)" class="list-group-item" data-value="常德市">常德市</a>
                                    <a id="source-张家界市" href="javascript:;" onclick="addCity('张家界市', this)" class="list-group-item" data-value="张家界市">张家界市</a>
                                    <a id="source-益阳市" href="javascript:;" onclick="addCity('益阳市', this)" class="list-group-item" data-value="益阳市">益阳市</a>
                                    <a id="source-郴州市" href="javascript:;" onclick="addCity('郴州市', this)" class="list-group-item" data-value="郴州市">郴州市</a>
                                    <a id="source-永州市" href="javascript:;" onclick="addCity('永州市', this)" class="list-group-item" data-value="永州市">永州市</a>
                                    <a id="source-怀化市" href="javascript:;" onclick="addCity('怀化市', this)" class="list-group-item" data-value="怀化市">怀化市</a>
                                    <a id="source-娄底市" href="javascript:;" onclick="addCity('娄底市', this)" class="list-group-item" data-value="娄底市">娄底市</a>
                                    <a id="source-湘西土家族苗族自治州" href="javascript:;" onclick="addCity('湘西土家族苗族自治州', this)" class="list-group-item" data-value="湘西土家族苗族自治州">湘西土家族苗族自治州</a>
                                  </div>
                                </div>
                              </div>
                            </div>

                            <div class="panel panel-default">
                              <div class="panel-heading">
                                <h4 class="panel-title">
                                  <a data-toggle="collapse" href="#collapse-15" class="collapsed">
                                    海南省
                                  </a>
                                </h4>
                              </div>
                              <div id="collapse-15" class="panel-collapse collapse" style="height: 0px;">
                                <div class="panel-body">
                                  <div class="list-group">
                                    <a id="source-海口市" href="javascript:;" onclick="addCity('海口市', this)" class="list-group-item" data-value="海口市">海口市</a>
                                    <a id="source-三亚市" href="javascript:;" onclick="addCity('三亚市', this)" class="list-group-item" data-value="三亚市">三亚市</a>
                                    <a id="source-三沙市" href="javascript:;" onclick="addCity('三沙市', this)" class="list-group-item" data-value="三沙市">三沙市</a>
                                    <a id="source-儋州市" href="javascript:;" onclick="addCity('儋州市', this)" class="list-group-item" data-value="儋州市">儋州市</a>
                                  </div>
                                </div>
                              </div>
                            </div>

                            <div class="panel panel-default">
                              <div class="panel-heading">
                                <h4 class="panel-title">
                                  <a data-toggle="collapse" href="#collapse-16" class="collapsed">
                                    吉林省
                                  </a>
                                </h4>
                              </div>
                              <div id="collapse-16" class="panel-collapse collapse" style="height: 0px;">
                                <div class="panel-body">
                                  <div class="list-group">
                                    <a id="source-长春市" href="javascript:;" onclick="addCity('长春市', this)" class="list-group-item" data-value="长春市">长春市</a>
                                    <a id="source-吉林市" href="javascript:;" onclick="addCity('吉林市', this)" class="list-group-item" data-value="吉林市">吉林市</a>
                                    <a id="source-四平市" href="javascript:;" onclick="addCity('四平市', this)" class="list-group-item" data-value="四平市">四平市</a>
                                    <a id="source-辽源市" href="javascript:;" onclick="addCity('辽源市', this)" class="list-group-item" data-value="辽源市">辽源市</a>
                                    <a id="source-通化市" href="javascript:;" onclick="addCity('通化市', this)" class="list-group-item" data-value="通化市">通化市</a>
                                    <a id="source-白山市" href="javascript:;" onclick="addCity('白山市', this)" class="list-group-item" data-value="白山市">白山市</a>
                                    <a id="source-松原市" href="javascript:;" onclick="addCity('松原市', this)" class="list-group-item" data-value="松原市">松原市</a>
                                    <a id="source-白城市" href="javascript:;" onclick="addCity('白城市', this)" class="list-group-item" data-value="白城市">白城市</a>
                                    <a id="source-延边朝鲜族自治州" href="javascript:;" onclick="addCity('延边朝鲜族自治州', this)" class="list-group-item" data-value="延边朝鲜族自治州">延边朝鲜族自治州</a>
                                  </div>
                                </div>
                              </div>
                            </div>

                            <div class="panel panel-default">
                              <div class="panel-heading">
                                <h4 class="panel-title">
                                  <a data-toggle="collapse" href="#collapse-17" class="collapsed">
                                    江苏省
                                  </a>
                                </h4>
                              </div>
                              <div id="collapse-17" class="panel-collapse collapse" style="height: 0px;">
                                <div class="panel-body">
                                  <div class="list-group">
                                    <a id="source-南京市" href="javascript:;" onclick="addCity('南京市', this)" class="list-group-item" data-value="南京市">南京市</a>
                                    <a id="source-无锡市" href="javascript:;" onclick="addCity('无锡市', this)" class="list-group-item" data-value="无锡市">无锡市</a>
                                    <a id="source-徐州市" href="javascript:;" onclick="addCity('徐州市', this)" class="list-group-item" data-value="徐州市">徐州市</a>
                                    <a id="source-常州市" href="javascript:;" onclick="addCity('常州市', this)" class="list-group-item" data-value="常州市">常州市</a>
                                    <a id="source-苏州市" href="javascript:;" onclick="addCity('苏州市', this)" class="list-group-item" data-value="苏州市">苏州市</a>
                                    <a id="source-南通市" href="javascript:;" onclick="addCity('南通市', this)" class="list-group-item" data-value="南通市">南通市</a>
                                    <a id="source-连云港市" href="javascript:;" onclick="addCity('连云港市', this)" class="list-group-item" data-value="连云港市">连云港市</a>
                                    <a id="source-淮安市" href="javascript:;" onclick="addCity('淮安市', this)" class="list-group-item" data-value="淮安市">淮安市</a>
                                    <a id="source-盐城市" href="javascript:;" onclick="addCity('盐城市', this)" class="list-group-item" data-value="盐城市">盐城市</a>
                                    <a id="source-扬州市" href="javascript:;" onclick="addCity('扬州市', this)" class="list-group-item" data-value="扬州市">扬州市</a>
                                    <a id="source-镇江市" href="javascript:;" onclick="addCity('镇江市', this)" class="list-group-item" data-value="镇江市">镇江市</a>
                                    <a id="source-泰州市" href="javascript:;" onclick="addCity('泰州市', this)" class="list-group-item" data-value="泰州市">泰州市</a>
                                    <a id="source-宿迁市" href="javascript:;" onclick="addCity('宿迁市', this)" class="list-group-item" data-value="宿迁市">宿迁市</a>
                                  </div>
                                </div>
                              </div>
                            </div>

                            <div class="panel panel-default">
                              <div class="panel-heading">
                                <h4 class="panel-title">
                                  <a data-toggle="collapse" href="#collapse-18" class="collapsed">
                                    江西省
                                  </a>
                                </h4>
                              </div>
                              <div id="collapse-18" class="panel-collapse collapse" style="height: 0px;">
                                <div class="panel-body">
                                  <div class="list-group">
                                    <a id="source-南昌市" href="javascript:;" onclick="addCity('南昌市', this)" class="list-group-item" data-value="南昌市">南昌市</a>
                                    <a id="source-景德镇市" href="javascript:;" onclick="addCity('景德镇市', this)" class="list-group-item" data-value="景德镇市">景德镇市</a>
                                    <a id="source-萍乡市" href="javascript:;" onclick="addCity('萍乡市', this)" class="list-group-item" data-value="萍乡市">萍乡市</a>
                                    <a id="source-九江市" href="javascript:;" onclick="addCity('九江市', this)" class="list-group-item" data-value="九江市">九江市</a>
                                    <a id="source-新余市" href="javascript:;" onclick="addCity('新余市', this)" class="list-group-item" data-value="新余市">新余市</a>
                                    <a id="source-鹰潭市" href="javascript:;" onclick="addCity('鹰潭市', this)" class="list-group-item" data-value="鹰潭市">鹰潭市</a>
                                    <a id="source-赣州市" href="javascript:;" onclick="addCity('赣州市', this)" class="list-group-item" data-value="赣州市">赣州市</a>
                                    <a id="source-吉安市" href="javascript:;" onclick="addCity('吉安市', this)" class="list-group-item" data-value="吉安市">吉安市</a>
                                    <a id="source-宜春市" href="javascript:;" onclick="addCity('宜春市', this)" class="list-group-item" data-value="宜春市">宜春市</a>
                                    <a id="source-抚州市" href="javascript:;" onclick="addCity('抚州市', this)" class="list-group-item" data-value="抚州市">抚州市</a>
                                    <a id="source-上饶市" href="javascript:;" onclick="addCity('上饶市', this)" class="list-group-item" data-value="上饶市">上饶市</a>
                                  </div>
                                </div>
                              </div>
                            </div>

                            <div class="panel panel-default">
                              <div class="panel-heading">
                                <h4 class="panel-title">
                                  <a data-toggle="collapse" href="#collapse-19" class="collapsed">
                                    辽宁省
                                  </a>
                                </h4>
                              </div>
                              <div id="collapse-19" class="panel-collapse collapse" style="height: 0px;">
                                <div class="panel-body">
                                  <div class="list-group">
                                    <a id="source-沈阳市" href="javascript:;" onclick="addCity('沈阳市', this)" class="list-group-item" data-value="沈阳市">沈阳市</a>
                                    <a id="source-大连市" href="javascript:;" onclick="addCity('大连市', this)" class="list-group-item" data-value="大连市">大连市</a>
                                    <a id="source-鞍山市" href="javascript:;" onclick="addCity('鞍山市', this)" class="list-group-item" data-value="鞍山市">鞍山市</a>
                                    <a id="source-抚顺市" href="javascript:;" onclick="addCity('抚顺市', this)" class="list-group-item" data-value="抚顺市">抚顺市</a>
                                    <a id="source-本溪市" href="javascript:;" onclick="addCity('本溪市', this)" class="list-group-item" data-value="本溪市">本溪市</a>
                                    <a id="source-丹东市" href="javascript:;" onclick="addCity('丹东市', this)" class="list-group-item" data-value="丹东市">丹东市</a>
                                    <a id="source-锦州市" href="javascript:;" onclick="addCity('锦州市', this)" class="list-group-item" data-value="锦州市">锦州市</a>
                                    <a id="source-营口市" href="javascript:;" onclick="addCity('营口市', this)" class="list-group-item" data-value="营口市">营口市</a>
                                    <a id="source-阜新市" href="javascript:;" onclick="addCity('阜新市', this)" class="list-group-item" data-value="阜新市">阜新市</a>
                                    <a id="source-辽阳市" href="javascript:;" onclick="addCity('辽阳市', this)" class="list-group-item" data-value="辽阳市">辽阳市</a>
                                    <a id="source-盘锦市" href="javascript:;" onclick="addCity('盘锦市', this)" class="list-group-item" data-value="盘锦市">盘锦市</a>
                                    <a id="source-铁岭市" href="javascript:;" onclick="addCity('铁岭市', this)" class="list-group-item" data-value="铁岭市">铁岭市</a>
                                    <a id="source-朝阳市" href="javascript:;" onclick="addCity('朝阳市', this)" class="list-group-item" data-value="朝阳市">朝阳市</a>
                                    <a id="source-葫芦岛市" href="javascript:;" onclick="addCity('葫芦岛市', this)" class="list-group-item" data-value="葫芦岛市">葫芦岛市</a>
                                  </div>
                                </div>
                              </div>
                            </div>

                            <div class="panel panel-default">
                              <div class="panel-heading">
                                <h4 class="panel-title">
                                  <a data-toggle="collapse" href="#collapse-20" class="collapsed">
                                    内蒙古自治区
                                  </a>
                                </h4>
                              </div>
                              <div id="collapse-20" class="panel-collapse collapse" style="height: 0px;">
                                <div class="panel-body">
                                  <div class="list-group">
                                    <a id="source-呼和浩特市" href="javascript:;" onclick="addCity('呼和浩特市', this)" class="list-group-item" data-value="呼和浩特市">呼和浩特市</a>
                                    <a id="source-包头市" href="javascript:;" onclick="addCity('包头市', this)" class="list-group-item" data-value="包头市">包头市</a>
                                    <a id="source-乌海市" href="javascript:;" onclick="addCity('乌海市', this)" class="list-group-item" data-value="乌海市">乌海市</a>
                                    <a id="source-赤峰市" href="javascript:;" onclick="addCity('赤峰市', this)" class="list-group-item" data-value="赤峰市">赤峰市</a>
                                    <a id="source-通辽市" href="javascript:;" onclick="addCity('通辽市', this)" class="list-group-item" data-value="通辽市">通辽市</a>
                                    <a id="source-鄂尔多斯市" href="javascript:;" onclick="addCity('鄂尔多斯市', this)" class="list-group-item" data-value="鄂尔多斯市">鄂尔多斯市</a>
                                    <a id="source-呼伦贝尔市" href="javascript:;" onclick="addCity('呼伦贝尔市', this)" class="list-group-item" data-value="呼伦贝尔市">呼伦贝尔市</a>
                                    <a id="source-巴彦淖尔市" href="javascript:;" onclick="addCity('巴彦淖尔市', this)" class="list-group-item" data-value="巴彦淖尔市">巴彦淖尔市</a>
                                    <a id="source-乌兰察布市" href="javascript:;" onclick="addCity('乌兰察布市', this)" class="list-group-item" data-value="乌兰察布市">乌兰察布市</a>
                                    <a id="source-兴安盟" href="javascript:;" onclick="addCity('兴安盟', this)" class="list-group-item" data-value="兴安盟">兴安盟</a>
                                    <a id="source-锡林郭勒盟" href="javascript:;" onclick="addCity('锡林郭勒盟', this)" class="list-group-item" data-value="锡林郭勒盟">锡林郭勒盟</a>
                                    <a id="source-阿拉善盟" href="javascript:;" onclick="addCity('阿拉善盟', this)" class="list-group-item" data-value="阿拉善盟">阿拉善盟</a>
                                  </div>
                                </div>
                              </div>
                            </div>

                            <div class="panel panel-default">
                              <div class="panel-heading">
                                <h4 class="panel-title">
                                  <a data-toggle="collapse" href="#collapse-21" class="collapsed">
                                    宁夏回族自治区
                                  </a>
                                </h4>
                              </div>
                              <div id="collapse-21" class="panel-collapse collapse" style="height: 0px;">
                                <div class="panel-body">
                                  <div class="list-group">
                                    <a id="source-银川市" href="javascript:;" onclick="addCity('银川市', this)" class="list-group-item" data-value="银川市">银川市</a>
                                    <a id="source-石嘴山市" href="javascript:;" onclick="addCity('石嘴山市', this)" class="list-group-item" data-value="石嘴山市">石嘴山市</a>
                                    <a id="source-吴忠市" href="javascript:;" onclick="addCity('吴忠市', this)" class="list-group-item" data-value="吴忠市">吴忠市</a>
                                    <a id="source-固原市" href="javascript:;" onclick="addCity('固原市', this)" class="list-group-item" data-value="固原市">固原市</a>
                                    <a id="source-中卫市" href="javascript:;" onclick="addCity('中卫市', this)" class="list-group-item" data-value="中卫市">中卫市</a>
                                  </div>
                                </div>
                              </div>
                            </div>

                            <div class="panel panel-default">
                              <div class="panel-heading">
                                <h4 class="panel-title">
                                  <a data-toggle="collapse" href="#collapse-22" class="collapsed">
                                    青海省
                                  </a>
                                </h4>
                              </div>
                              <div id="collapse-22" class="panel-collapse collapse" style="height: 0px;">
                                <div class="panel-body">
                                  <div class="list-group">
                                    <a id="source-西宁市" href="javascript:;" onclick="addCity('西宁市', this)" class="list-group-item" data-value="西宁市">西宁市</a>
                                    <a id="source-海东地区" href="javascript:;" onclick="addCity('海东地区', this)" class="list-group-item" data-value="海东地区">海东地区</a>
                                    <a id="source-海北藏族自治州" href="javascript:;" onclick="addCity('海北藏族自治州', this)" class="list-group-item" data-value="海北藏族自治州">海北藏族自治州</a>
                                    <a id="source-海南藏族自治州" href="javascript:;" onclick="addCity('海南藏族自治州', this)" class="list-group-item" data-value="海南藏族自治州">海南藏族自治州</a>
                                    <a id="source-黄南藏族自治州" href="javascript:;" onclick="addCity('黄南藏族自治州', this)" class="list-group-item" data-value="黄南藏族自治州">黄南藏族自治州</a>
                                    <a id="source-果洛藏族自治州" href="javascript:;" onclick="addCity('果洛藏族自治州', this)" class="list-group-item" data-value="果洛藏族自治州">果洛藏族自治州</a>
                                    <a id="source-玉树藏族自治州" href="javascript:;" onclick="addCity('玉树藏族自治州', this)" class="list-group-item" data-value="玉树藏族自治州">玉树藏族自治州</a>
                                    <a id="source-海西蒙古族藏族自治州" href="javascript:;" onclick="addCity('海西蒙古族藏族自治州', this)" class="list-group-item" data-value="海西蒙古族藏族自治州">海西蒙古族藏族自治州</a>
                                  </div>
                                </div>
                              </div>
                            </div>

                            <div class="panel panel-default">
                              <div class="panel-heading">
                                <h4 class="panel-title">
                                  <a data-toggle="collapse" href="#collapse-23" class="collapsed">
                                    四川省
                                  </a>
                                </h4>
                              </div>
                              <div id="collapse-23" class="panel-collapse collapse" style="height: 0px;">
                                <div class="panel-body">
                                  <div class="list-group">
                                    <a id="source-成都市" href="javascript:;" onclick="addCity('成都市', this)" class="list-group-item" data-value="成都市">成都市</a>
                                    <a id="source-自贡市" href="javascript:;" onclick="addCity('自贡市', this)" class="list-group-item" data-value="自贡市">自贡市</a>
                                    <a id="source-攀枝花市" href="javascript:;" onclick="addCity('攀枝花市', this)" class="list-group-item" data-value="攀枝花市">攀枝花市</a>
                                    <a id="source-泸州市" href="javascript:;" onclick="addCity('泸州市', this)" class="list-group-item" data-value="泸州市">泸州市</a>
                                    <a id="source-德阳市" href="javascript:;" onclick="addCity('德阳市', this)" class="list-group-item" data-value="德阳市">德阳市</a>
                                    <a id="source-绵阳市" href="javascript:;" onclick="addCity('绵阳市', this)" class="list-group-item" data-value="绵阳市">绵阳市</a>
                                    <a id="source-广元市" href="javascript:;" onclick="addCity('广元市', this)" class="list-group-item" data-value="广元市">广元市</a>
                                    <a id="source-遂宁市" href="javascript:;" onclick="addCity('遂宁市', this)" class="list-group-item" data-value="遂宁市">遂宁市</a>
                                    <a id="source-内江市" href="javascript:;" onclick="addCity('内江市', this)" class="list-group-item" data-value="内江市">内江市</a>
                                    <a id="source-乐山市" href="javascript:;" onclick="addCity('乐山市', this)" class="list-group-item" data-value="乐山市">乐山市</a>
                                    <a id="source-南充市" href="javascript:;" onclick="addCity('南充市', this)" class="list-group-item" data-value="南充市">南充市</a>
                                    <a id="source-眉山市" href="javascript:;" onclick="addCity('眉山市', this)" class="list-group-item" data-value="眉山市">眉山市</a>
                                    <a id="source-宜宾市" href="javascript:;" onclick="addCity('宜宾市', this)" class="list-group-item" data-value="宜宾市">宜宾市</a>
                                    <a id="source-广安市" href="javascript:;" onclick="addCity('广安市', this)" class="list-group-item" data-value="广安市">广安市</a>
                                    <a id="source-达州市" href="javascript:;" onclick="addCity('达州市', this)" class="list-group-item" data-value="达州市">达州市</a>
                                    <a id="source-雅安市" href="javascript:;" onclick="addCity('雅安市', this)" class="list-group-item" data-value="雅安市">雅安市</a>
                                    <a id="source-巴中市" href="javascript:;" onclick="addCity('巴中市', this)" class="list-group-item" data-value="巴中市">巴中市</a>
                                    <a id="source-资阳市" href="javascript:;" onclick="addCity('资阳市', this)" class="list-group-item" data-value="资阳市">资阳市</a>
                                    <a id="source-阿坝藏族羌族自治州" href="javascript:;" onclick="addCity('阿坝藏族羌族自治州', this)" class="list-group-item" data-value="阿坝藏族羌族自治州">阿坝藏族羌族自治州</a>
                                    <a id="source-甘孜藏族自治州" href="javascript:;" onclick="addCity('甘孜藏族自治州', this)" class="list-group-item" data-value="甘孜藏族自治州">甘孜藏族自治州</a>
                                    <a id="source-凉山彝族自治州" href="javascript:;" onclick="addCity('凉山彝族自治州', this)" class="list-group-item" data-value="凉山彝族自治州">凉山彝族自治州</a>
                                  </div>
                                </div>
                              </div>
                            </div>

                            <div class="panel panel-default">
                              <div class="panel-heading">
                                <h4 class="panel-title">
                                  <a data-toggle="collapse" href="#collapse-24" class="collapsed">
                                    山东省
                                  </a>
                                </h4>
                              </div>
                              <div id="collapse-24" class="panel-collapse collapse" style="height: 0px;">
                                <div class="panel-body">
                                  <div class="list-group">
                                    <a id="source-济南市" href="javascript:;" onclick="addCity('济南市', this)" class="list-group-item" data-value="济南市">济南市</a>
                                    <a id="source-青岛市" href="javascript:;" onclick="addCity('青岛市', this)" class="list-group-item" data-value="青岛市">青岛市</a>
                                    <a id="source-淄博市" href="javascript:;" onclick="addCity('淄博市', this)" class="list-group-item" data-value="淄博市">淄博市</a>
                                    <a id="source-枣庄市" href="javascript:;" onclick="addCity('枣庄市', this)" class="list-group-item" data-value="枣庄市">枣庄市</a>
                                    <a id="source-东营市" href="javascript:;" onclick="addCity('东营市', this)" class="list-group-item" data-value="东营市">东营市</a>
                                    <a id="source-烟台市" href="javascript:;" onclick="addCity('烟台市', this)" class="list-group-item" data-value="烟台市">烟台市</a>
                                    <a id="source-潍坊市" href="javascript:;" onclick="addCity('潍坊市', this)" class="list-group-item" data-value="潍坊市">潍坊市</a>
                                    <a id="source-济宁市" href="javascript:;" onclick="addCity('济宁市', this)" class="list-group-item" data-value="济宁市">济宁市</a>
                                    <a id="source-泰安市" href="javascript:;" onclick="addCity('泰安市', this)" class="list-group-item" data-value="泰安市">泰安市</a>
                                    <a id="source-威海市" href="javascript:;" onclick="addCity('威海市', this)" class="list-group-item" data-value="威海市">威海市</a>
                                    <a id="source-日照市" href="javascript:;" onclick="addCity('日照市', this)" class="list-group-item" data-value="日照市">日照市</a>
                                    <a id="source-莱芜市" href="javascript:;" onclick="addCity('莱芜市', this)" class="list-group-item" data-value="莱芜市">莱芜市</a>
                                    <a id="source-临沂市" href="javascript:;" onclick="addCity('临沂市', this)" class="list-group-item" data-value="临沂市">临沂市</a>
                                    <a id="source-德州市" href="javascript:;" onclick="addCity('德州市', this)" class="list-group-item" data-value="德州市">德州市</a>
                                    <a id="source-聊城市" href="javascript:;" onclick="addCity('聊城市', this)" class="list-group-item" data-value="聊城市">聊城市</a>
                                    <a id="source-滨州市" href="javascript:;" onclick="addCity('滨州市', this)" class="list-group-item" data-value="滨州市">滨州市</a>
                                    <a id="source-菏泽市" href="javascript:;" onclick="addCity('菏泽市', this)" class="list-group-item" data-value="菏泽市">菏泽市</a>
                                  </div>
                                </div>
                              </div>
                            </div>

                            <div class="panel panel-default">
                              <div class="panel-heading">
                                <h4 class="panel-title">
                                  <a data-toggle="collapse" href="#collapse-25" class="collapsed">
                                    上海市
                                  </a>
                                </h4>
                              </div>
                              <div id="collapse-25" class="panel-collapse collapse" style="height: 0px;">
                                <div class="panel-body">
                                  <div class="list-group">
                                    <a id="source-上海市" href="javascript:;" onclick="addCity('上海市', this)" class="list-group-item" data-value="上海市">上海市</a>
                                  </div>
                                </div>
                              </div>
                            </div>

                            <div class="panel panel-default">
                              <div class="panel-heading">
                                <h4 class="panel-title">
                                  <a data-toggle="collapse" href="#collapse-26" class="collapsed">
                                    山西省
                                  </a>
                                </h4>
                              </div>
                              <div id="collapse-26" class="panel-collapse collapse" style="height: 0px;">
                                <div class="panel-body">
                                  <div class="list-group">
                                    <a id="source-太原市" href="javascript:;" onclick="addCity('太原市', this)" class="list-group-item" data-value="太原市">太原市</a>
                                    <a id="source-大同市" href="javascript:;" onclick="addCity('大同市', this)" class="list-group-item" data-value="大同市">大同市</a>
                                    <a id="source-阳泉市" href="javascript:;" onclick="addCity('阳泉市', this)" class="list-group-item" data-value="阳泉市">阳泉市</a>
                                    <a id="source-长治市" href="javascript:;" onclick="addCity('长治市', this)" class="list-group-item" data-value="长治市">长治市</a>
                                    <a id="source-晋城市" href="javascript:;" onclick="addCity('晋城市', this)" class="list-group-item" data-value="晋城市">晋城市</a>
                                    <a id="source-朔州市" href="javascript:;" onclick="addCity('朔州市', this)" class="list-group-item" data-value="朔州市">朔州市</a>
                                    <a id="source-晋中市" href="javascript:;" onclick="addCity('晋中市', this)" class="list-group-item" data-value="晋中市">晋中市</a>
                                    <a id="source-运城市" href="javascript:;" onclick="addCity('运城市', this)" class="list-group-item" data-value="运城市">运城市</a>
                                    <a id="source-忻州市" href="javascript:;" onclick="addCity('忻州市', this)" class="list-group-item" data-value="忻州市">忻州市</a>
                                    <a id="source-临汾市" href="javascript:;" onclick="addCity('临汾市', this)" class="list-group-item" data-value="临汾市">临汾市</a>
                                    <a id="source-吕梁市" href="javascript:;" onclick="addCity('吕梁市', this)" class="list-group-item" data-value="吕梁市">吕梁市</a>
                                  </div>
                                </div>
                              </div>
                            </div>

                            <div class="panel panel-default">
                              <div class="panel-heading">
                                <h4 class="panel-title">
                                  <a data-toggle="collapse" href="#collapse-27" class="collapsed">
                                    陕西省
                                  </a>
                                </h4>
                              </div>
                              <div id="collapse-27" class="panel-collapse collapse" style="height: 0px;">
                                <div class="panel-body">
                                  <div class="list-group">
                                    <a id="source-西安市" href="javascript:;" onclick="addCity('西安市', this)" class="list-group-item" data-value="西安市">西安市</a>
                                    <a id="source-铜川市" href="javascript:;" onclick="addCity('铜川市', this)" class="list-group-item" data-value="铜川市">铜川市</a>
                                    <a id="source-宝鸡市" href="javascript:;" onclick="addCity('宝鸡市', this)" class="list-group-item" data-value="宝鸡市">宝鸡市</a>
                                    <a id="source-咸阳市" href="javascript:;" onclick="addCity('咸阳市', this)" class="list-group-item" data-value="咸阳市">咸阳市</a>
                                    <a id="source-渭南市" href="javascript:;" onclick="addCity('渭南市', this)" class="list-group-item" data-value="渭南市">渭南市</a>
                                    <a id="source-延安市" href="javascript:;" onclick="addCity('延安市', this)" class="list-group-item" data-value="延安市">延安市</a>
                                    <a id="source-汉中市" href="javascript:;" onclick="addCity('汉中市', this)" class="list-group-item" data-value="汉中市">汉中市</a>
                                    <a id="source-榆林市" href="javascript:;" onclick="addCity('榆林市', this)" class="list-group-item" data-value="榆林市">榆林市</a>
                                    <a id="source-安康市" href="javascript:;" onclick="addCity('安康市', this)" class="list-group-item" data-value="安康市">安康市</a>
                                    <a id="source-商洛市" href="javascript:;" onclick="addCity('商洛市', this)" class="list-group-item" data-value="商洛市">商洛市</a>
                                  </div>
                                </div>
                              </div>
                            </div>

                            <div class="panel panel-default">
                              <div class="panel-heading">
                                <h4 class="panel-title">
                                  <a data-toggle="collapse" href="#collapse-28" class="collapsed">
                                    天津市
                                  </a>
                                </h4>
                              </div>
                              <div id="collapse-28" class="panel-collapse collapse" style="height: 0px;">
                                <div class="panel-body">
                                  <div class="list-group">
                                    <a id="source-天津市" href="javascript:;" onclick="addCity('天津市', this)" class="list-group-item" data-value="天津市">天津市</a>
                                  </div>
                                </div>
                              </div>
                            </div>

                            <div class="panel panel-default">
                              <div class="panel-heading">
                                <h4 class="panel-title">
                                  <a data-toggle="collapse" href="#collapse-29" class="collapsed">
                                    西藏自治区
                                  </a>
                                </h4>
                              </div>
                              <div id="collapse-29" class="panel-collapse collapse" style="height: 0px;">
                                <div class="panel-body">
                                  <div class="list-group">
                                    <a id="source-拉萨市" href="javascript:;" onclick="addCity('拉萨市', this)" class="list-group-item" data-value="拉萨市">拉萨市</a>
                                    <a id="source-昌都地区" href="javascript:;" onclick="addCity('昌都地区', this)" class="list-group-item" data-value="昌都地区">昌都地区</a>
                                    <a id="source-山南地区" href="javascript:;" onclick="addCity('山南地区', this)" class="list-group-item" data-value="山南地区">山南地区</a>
                                    <a id="source-日喀则地区" href="javascript:;" onclick="addCity('日喀则地区', this)" class="list-group-item" data-value="日喀则地区">日喀则地区</a>
                                    <a id="source-林芝地区" href="javascript:;" onclick="addCity('林芝地区', this)" class="list-group-item" data-value="林芝地区">林芝地区</a>
                                    <a id="source-那曲地区" href="javascript:;" onclick="addCity('那曲地区', this)" class="list-group-item" data-value="那曲地区">那曲地区</a>
                                    <a id="source-阿里地区" href="javascript:;" onclick="addCity('阿里地区', this)" class="list-group-item" data-value="阿里地区">阿里地区</a>
                                  </div>
                                </div>
                              </div>
                            </div>

                            <div class="panel panel-default">
                              <div class="panel-heading">
                                <h4 class="panel-title">
                                  <a data-toggle="collapse" href="#collapse-30" class="collapsed">
                                    新疆维吾尔自治区
                                  </a>
                                </h4>
                              </div>
                              <div id="collapse-30" class="panel-collapse collapse" style="height: 0px;">
                                <div class="panel-body">
                                  <div class="list-group">
                                    <a id="source-乌鲁木齐市" href="javascript:;" onclick="addCity('乌鲁木齐市', this)" class="list-group-item" data-value="乌鲁木齐市">乌鲁木齐市</a>
                                    <a id="source-克拉玛依市" href="javascript:;" onclick="addCity('克拉玛依市', this)" class="list-group-item" data-value="克拉玛依市">克拉玛依市</a>
                                    <a id="source-吐鲁番地区" href="javascript:;" onclick="addCity('吐鲁番地区', this)" class="list-group-item" data-value="吐鲁番地区">吐鲁番地区</a>
                                    <a id="source-哈密地区" href="javascript:;" onclick="addCity('哈密地区', this)" class="list-group-item" data-value="哈密地区">哈密地区</a>
                                    <a id="source-昌吉回族自治州" href="javascript:;" onclick="addCity('昌吉回族自治州', this)" class="list-group-item" data-value="昌吉回族自治州">昌吉回族自治州</a>
                                    <a id="source-博尔塔拉蒙古自治州" href="javascript:;" onclick="addCity('博尔塔拉蒙古自治州', this)" class="list-group-item" data-value="博尔塔拉蒙古自治州">博尔塔拉蒙古自治州</a>
                                    <a id="source-巴音郭楞蒙古自治州" href="javascript:;" onclick="addCity('巴音郭楞蒙古自治州', this)" class="list-group-item" data-value="巴音郭楞蒙古自治州">巴音郭楞蒙古自治州</a>
                                    <a id="source-阿克苏地区" href="javascript:;" onclick="addCity('阿克苏地区', this)" class="list-group-item" data-value="阿克苏地区">阿克苏地区</a>
                                    <a id="source-喀什地区" href="javascript:;" onclick="addCity('喀什地区', this)" class="list-group-item" data-value="喀什地区">喀什地区</a>
                                    <a id="source-和田地区" href="javascript:;" onclick="addCity('和田地区', this)" class="list-group-item" data-value="和田地区">和田地区</a>
                                    <a id="source-伊犁哈萨克自治州" href="javascript:;" onclick="addCity('伊犁哈萨克自治州', this)" class="list-group-item" data-value="伊犁哈萨克自治州">伊犁哈萨克自治州</a>
                                    <a id="source-塔城地区" href="javascript:;" onclick="addCity('塔城地区', this)" class="list-group-item" data-value="塔城地区">塔城地区</a>
                                    <a id="source-阿勒泰地区" href="javascript:;" onclick="addCity('阿勒泰地区', this)" class="list-group-item" data-value="阿勒泰地区">阿勒泰地区</a>
                                    <a id="source-克孜勒苏柯尔克孜自治州" href="javascript:;" onclick="addCity('克孜勒苏柯尔克孜自治州', this)" class="list-group-item" data-value="克孜勒苏柯尔克孜自治州">克孜勒苏柯尔克孜自治州</a>
                                  </div>
                                </div>
                              </div>
                            </div>

                            <div class="panel panel-default">
                              <div class="panel-heading">
                                <h4 class="panel-title">
                                  <a data-toggle="collapse" href="#collapse-31" class="collapsed">
                                    云南省
                                  </a>
                                </h4>
                              </div>
                              <div id="collapse-31" class="panel-collapse collapse" style="height: 0px;">
                                <div class="panel-body">
                                  <div class="list-group">
                                    <a id="source-昆明市" href="javascript:;" onclick="addCity('昆明市', this)" class="list-group-item" data-value="昆明市">昆明市</a>
                                    <a id="source-曲靖市" href="javascript:;" onclick="addCity('曲靖市', this)" class="list-group-item" data-value="曲靖市">曲靖市</a>
                                    <a id="source-玉溪市" href="javascript:;" onclick="addCity('玉溪市', this)" class="list-group-item" data-value="玉溪市">玉溪市</a>
                                    <a id="source-保山市" href="javascript:;" onclick="addCity('保山市', this)" class="list-group-item" data-value="保山市">保山市</a>
                                    <a id="source-昭通市" href="javascript:;" onclick="addCity('昭通市', this)" class="list-group-item" data-value="昭通市">昭通市</a>
                                    <a id="source-丽江市" href="javascript:;" onclick="addCity('丽江市', this)" class="list-group-item" data-value="丽江市">丽江市</a>
                                    <a id="source-临沧市" href="javascript:;" onclick="addCity('临沧市', this)" class="list-group-item" data-value="临沧市">临沧市</a>
                                    <a id="source-楚雄彝族自治州" href="javascript:;" onclick="addCity('楚雄彝族自治州', this)" class="list-group-item" data-value="楚雄彝族自治州">楚雄彝族自治州</a>
                                    <a id="source-红河哈尼族彝族自治州" href="javascript:;" onclick="addCity('红河哈尼族彝族自治州', this)" class="list-group-item" data-value="红河哈尼族彝族自治州">红河哈尼族彝族自治州</a>
                                    <a id="source-文山壮族苗族自治州" href="javascript:;" onclick="addCity('文山壮族苗族自治州', this)" class="list-group-item" data-value="文山壮族苗族自治州">文山壮族苗族自治州</a>
                                    <a id="source-西双版纳傣族自治州" href="javascript:;" onclick="addCity('西双版纳傣族自治州', this)" class="list-group-item" data-value="西双版纳傣族自治州">西双版纳傣族自治州</a>
                                    <a id="source-大理白族自治州" href="javascript:;" onclick="addCity('大理白族自治州', this)" class="list-group-item" data-value="大理白族自治州">大理白族自治州</a>
                                    <a id="source-德宏傣族景颇族自治州" href="javascript:;" onclick="addCity('德宏傣族景颇族自治州', this)" class="list-group-item" data-value="德宏傣族景颇族自治州">德宏傣族景颇族自治州</a>
                                    <a id="source-怒江傈僳族自治州" href="javascript:;" onclick="addCity('怒江傈僳族自治州', this)" class="list-group-item" data-value="怒江傈僳族自治州">怒江傈僳族自治州</a>
                                    <a id="source-迪庆藏族自治州" href="javascript:;" onclick="addCity('迪庆藏族自治州', this)" class="list-group-item" data-value="迪庆藏族自治州">迪庆藏族自治州</a>
                                    <a id="source-普洱市" href="javascript:;" onclick="addCity('普洱市', this)" class="list-group-item" data-value="普洱市">普洱市</a>
                                  </div>
                                </div>
                              </div>
                            </div>

                            <div class="panel panel-default">
                              <div class="panel-heading">
                                <h4 class="panel-title">
                                  <a data-toggle="collapse" href="#collapse-32" class="collapsed">
                                    浙江省
                                  </a>
                                </h4>
                              </div>
                              <div id="collapse-32" class="panel-collapse collapse" style="height: 0px;">
                                <div class="panel-body">
                                  <div class="list-group">
                                    <a id="source-杭州市" href="javascript:;" onclick="addCity('杭州市', this)" class="list-group-item" data-value="杭州市">杭州市</a>
                                    <a id="source-宁波市" href="javascript:;" onclick="addCity('宁波市', this)" class="list-group-item" data-value="宁波市">宁波市</a>
                                    <a id="source-温州市" href="javascript:;" onclick="addCity('温州市', this)" class="list-group-item" data-value="温州市">温州市</a>
                                    <a id="source-嘉兴市" href="javascript:;" onclick="addCity('嘉兴市', this)" class="list-group-item" data-value="嘉兴市">嘉兴市</a>
                                    <a id="source-湖州市" href="javascript:;" onclick="addCity('湖州市', this)" class="list-group-item" data-value="湖州市">湖州市</a>
                                    <a id="source-绍兴市" href="javascript:;" onclick="addCity('绍兴市', this)" class="list-group-item" data-value="绍兴市">绍兴市</a>
                                    <a id="source-金华市" href="javascript:;" onclick="addCity('金华市', this)" class="list-group-item" data-value="金华市">金华市</a>
                                    <a id="source-衢州市" href="javascript:;" onclick="addCity('衢州市', this)" class="list-group-item" data-value="衢州市">衢州市</a>
                                    <a id="source-舟山市" href="javascript:;" onclick="addCity('舟山市', this)" class="list-group-item" data-value="舟山市">舟山市</a>
                                    <a id="source-台州市" href="javascript:;" onclick="addCity('台州市', this)" class="list-group-item" data-value="台州市">台州市</a>
                                    <a id="source-丽水市" href="javascript:;" onclick="addCity('丽水市', this)" class="list-group-item" data-value="丽水市">丽水市</a>
                                  </div>
                                </div>
                              </div>
                            </div>
                          </div>

                        </div>

                        <div class="col-sm-6 text-left">

                          <div class="panel panel-default">
                            <div class="panel-heading">
                              <h4 class="panel-title">
                                <a id="selectedTips" data-toggle="collapse" href="javascript:;">
                                  已选择0个
                                </a>
                              </h4>
                            </div>
                            <div class="panel-collapse collapse in">
                              <div class="panel-body">
                                <div id="selectedCities" class="list-group"></div>
                              </div>
                            </div>
                          </div>

                        </div>

                      </div>
                    </div>
                    <div class="modal-footer">
                      <asp:Button class="btn btn-primary" id="BtnAreas" runat="server" Text="提 交"></asp:Button>
                      <button type="button" class="btn btn-default" onClick="$('#modalAreas').modal('hide');return false;">取 消</button>
                    </div>
                  </div>
                </div>
              </div>
              <script>
                var selectedCities = <%=GetSelectedCities()%>;

                function addCity(city) {
                  if (selectedCities.indexOf(city) !== -1) return;
                  selectedCities.push(city);
                  $('#selectedCities').append('<a href="javascript:;" onclick="removeCity(\'' + city +
                    '\', this)" class="list-group-item">' + city + '</a>');
                  $('#source-' + city).addClass('disabled');
                  $('#selectedTips').text('已选择' + selectedCities.length + '个');
                }

                function removeCity(city, e) {
                  selectedCities.splice(selectedCities.indexOf(city), 1);
                  $(e).remove();
                  $('#source-' + city).removeClass('disabled');
                  $('#selectedTips').text('已选择' + selectedCities.length + '个');
                }

                $(document).ready(function () {
                  $('#divAreas').height($(document).height() - 260);
                  for (var i = 0; i < selectedCities.length; i++) {
                    $('#selectedCities').append('<a href="javascript:;" onclick="removeCity(\'' + selectedCities[i] +
                      '\', this)" class="list-group-item">' + selectedCities[i] + '</a>');
                    $('#source-' + selectedCities[i]).addClass('disabled');
                  }
                  $('#selectedTips').text('已选择' + selectedCities.length + '个');
                  $('#modalAreas').modal();
                });
              </script>
          </asp:PlaceHolder>
          <!-- modalAreas end -->

        </form>
        </div>
      </div>
  </body>

  </html>