<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    MucLucNganSach_CauHinh
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        #scroller
        {
            position: relative;
        }
    </style>
    <%
        String ParentID = "MucLucNganSach";
        DataTable dt = MucLucNganSachModels.NS_LoaiNganSach();
        String sDanhSachTruongCam = "";
        String strDSDuocNhapTruongTien = MucLucNganSachModels.strDSDuocNhapTruongTien + ",sNhapTheoTruong";
        String strDSTruongTienTieuDe = MucLucNganSachModels.strDSTruongTienTieuDe + ",Nhập theo sLNS,Nhập theo sM,Nhập theo sTM";
        String[] arr1 = strDSDuocNhapTruongTien.Split(',');
        String[] arr2 = strDSTruongTienTieuDe.Split(',');
        using (Html.BeginForm("CauHinh_Submit", "MucLucNganSach", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Danh sách loại ngân sách</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td width="65%" class="td_form2_td5">
                            &nbsp;
                        </td>
                        <td width="30%" align="right" class="td_form2_td5">
                            <input type="submit" class="button" id="Submit2" value="Lưu" />
                        </td>
                        <td width="5px">
                            &nbsp;
                        </td>
                        <td class="td_form2_td5">
                            <input class="button" type="button" value="Hủy" onclick="history.go(-1)" />
                        </td>
                    </tr>
                </table>
                <div id="scroller">
                    <br />
                    <table class="mGrid">
                        <tr>
                            <th width="2%">
                                STT
                            </th>
                            <th width="5%">
                                LNS
                            </th>
                            <th width="19%">
                                Mục lục ngân sách
                            </th>
                            <%for (int j = 0; j < arr2.Length; j++)
                              { %>
                            <th width="5%">
                                <%=arr2[j] %>
                            </th>
                            <%} %>
                        </tr>
                    </table>
                </div>
                <div>
                    <table class="mGrid">
                        <tr>
                            <td colspan="3" align="center">
                                &nbsp;<b>Chọn tất cả </b>
                            </td>
                            <%for (int j = 0; j < arr1.Length; j++)
                              {%>
                            <% if (arr1[j] == "sNhapTheoTruong")
                               { %>
                            <td align="center" width="5%">
                                <%=String.Format("<input type=\"radio\" id=\"{0}_{1}\" name=\"{0}_{1}\" value=\"sLNS\" onclick=\"RCheckAll(this.checked,'{2}')\"/>", arr1[j], "sLNS", "sLNS")%>
                            </td>
                            <td align="center" width="5%">
                                <%=String.Format("<input type=\"radio\" id=\"{0}_{1}\" name=\"{0}_{1}\" value=\"sM\" onclick=\"RCheckAll(this.checked,'{2}')\"/>", arr1[j], "sLNS","sM")%>
                            </td>
                            <td align="center" width="5%">
                                <%=String.Format("<input type=\"radio\" id=\"{0}_{1}\" name=\"{0}_{1}\" value=\"sTM\" onclick=\"RCheckAll(this.checked,'{2}')\"/>", arr1[j], "sLNS", "sTM")%>
                            </td>
                            <%}
                               else
                               {%>
                            <td align="center">
                                <input type="checkbox" onclick="CheckAll(this.checked,'<%=arr1[j]%>')" />
                            </td>
                            <%} %>
                            <%} %>
                        </tr>
                        <%for (int i = 0; i < dt.Rows.Count; i++)
                          {
                              DataRow Row = dt.Rows[i];
                              String classtr = "";
                              if (i % 2 == 0)
                              {
                                  classtr = "class=\"alt\"";
                              }
                        %>
                        <tr <%=classtr %>>
                            <td width="2%">
                                <%=i+1 %>
                            </td>
                            <td width="5%">
                                <%=MyHtmlHelper.Label(Row["sLNS"], "sLNS")%>
                                <%=MyHtmlHelper.Hidden(ParentID, Row["sLNS"], "sLNS", sDanhSachTruongCam)%>
                            </td>
                            <td width="19%">
                                <%=MyHtmlHelper.Label(Row["sMoTa"], "sMoTa")%>
                            </td>
                            <%for (int j = 0; j < arr1.Length; j++)
                              {
                                  if (arr1[j] == "sNhapTheoTruong")
                                  { %>
                            <td align="center" width="5%">
                                <%=String.Format("<input type=\"radio\" radio-group=\"sLNS\" id=\"{0}_{1}\" name=\"{0}_{1}\" value=\"sLNS\" {2}/>", arr1[j], Convert.ToString(Row["sLNS"]), Convert.ToString(Row[arr1[j]]) == "sLNS" ? "checked='checked'" : "")%>
                            </td>
                            <td align="center" width="5%">
                                <%=String.Format("<input type=\"radio\" radio-group=\"sM\" id=\"{0}_{1}\" name=\"{0}_{1}\" value=\"sM\" {2}/>", arr1[j], Convert.ToString(Row["sLNS"]), Convert.ToString(Row[arr1[j]]) == "sM" ? "checked='checked'" : "")%>
                            </td>
                            <td align="center" width="5%">
                                <%=String.Format("<input type=\"radio\" radio-group=\"sTM\" id=\"{0}_{1}\" name=\"{0}_{1}\" value=\"sTM\" {2}/>", arr1[j], Convert.ToString(Row["sLNS"]), Convert.ToString(Row[arr1[j]]) == "sTM" ? "checked='checked'" : "")%>
                            </td>
                            <%}
                                  else
                                  { %>
                            <td align="center" width="5%">
                                <%=String.Format("<input type=\"checkbox\" check-group=\"{0}\"  id=\"{0}_{1}\" name=\"{0}_{1}\" {2}/>", arr1[j], Convert.ToString(Row["sLNS"]), Convert.ToBoolean(Row[arr1[j]]) ? "checked='checked'" : "")%>
                            </td>
                            <%} %>
                            <%} %>
                        </tr>
                        <%} %>
                    </table>
                </div>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td width="65%" class="td_form2_td5">
                        &nbsp;
                    </td>
                    <td width="30%" align="right" class="td_form2_td5">
                        <input type="submit" class="button" id="Submit1" value="Lưu" />
                    </td>
                    <td width="5px">
                        &nbsp;
                    </td>
                    <td class="td_form2_td5">
                        <input class="button" type="button" value="Hủy" onclick="history.go(-1)" />
                    </td>
                </tr>
            </table>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function ToMau(ctl) {
            ctl.bgColor = '#FFFF66';
        }
        function clear_mau(ctl) {

            ctl.bgColor = '#FFFFFF';
        }
        function CheckAll(value, groupname) {
            $("input:checkbox[check-group='" + groupname + "']").each(function (i) {
                this.checked = value;
            });
        }
        function RCheckAll(value, groupname) {
            $("input:radio[radio-group='" + groupname + "']").each(function (i) {
                this.checked = value;
            });
        }
        $(window).scroll(function () {
            if ($(window).scrollTop() > 200) {
                $('#scroller').css('top', $(window).scrollTop() - 200);
            }
            if ($(window).scrollTop() <= 200) {
                $('#scroller').css('top', 0);
            }
        }
);  
    </script>
    <%} %>
</asp:Content>
