<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<%
    if (Request.QueryString["Saved"] == "1")
    {
        %>
        <script type="text/javascript">
            $(document).ready(function () {
                parent.KTCS_TaiSan_Dialog_Close(true);
            });                                 
        </script>
        <%
    }
    else
    {%> 
    <%   
    String ParentID = "TaiSan_Chung";
    String iID_MaTaiSan = Request.QueryString["iID_MaTaiSan"];
    DataTable dt = KTCS_TaiSanModels.Get_dtTaiSan(iID_MaTaiSan);
    String iID_MaLoaiTaiSan = "";
    String sTenTaiSan = "";

    if (dt.Rows.Count > 0)
    {
        iID_MaLoaiTaiSan = Convert.ToString(dt.Rows[0]["iID_MaLoaiTaiSan"]);
        sTenTaiSan = Convert.ToString(dt.Rows[0]["sTenTaiSan"]);
    }

    NameValueCollection data = new NameValueCollection();
    data = KTCS_TaiSanModels.LayThongTinTaiSanChiTiet(iID_MaTaiSan);
    String DuLieuMoi = "1";
    if (KTCS_TaiSanModels.KiemTraCoChiTietTaiSan(iID_MaTaiSan)) DuLieuMoi = "0";   
%>
<form action="<%=Url.Action("ChiTiet_Submit", "KTCS_TaiSan", new {ParentID = ParentID})%>" method="post">
 <%=MyHtmlHelper.Hidden(ParentID, iID_MaTaiSan, "iID_MaTaiSan", "")%>
 <%=MyHtmlHelper.Hidden(ParentID, iID_MaLoaiTaiSan, "iID_MaLoaiTaiSan", "")%>
 <%=MyHtmlHelper.Hidden(ParentID, DuLieuMoi, "DuLieuMoi", "")%>
 <%=MyHtmlHelper.Hidden(ParentID, data["iID_MaTaiSanChiTiet"], "iID_MaTaiSanChiTiet", "")%>
 <%=MyHtmlHelper.Hidden(ParentID,0,"iLoaiTS","") %>
<div style="background-color: #f0f9fe; background-repeat: repeat; border: solid 1px #ec3237">
    <div style="padding: 10px;">
        <div id="nhapform">
            <div id="form2">
                <table cellpadding="1" cellspacing="1" border="0" class="table_form2" width="100%">
                    <tr>
                        <td style="width: 250px;">
                            <b>CỤC TÀI CHÍNH</b>
                        </td>
                        <td style="float: right;">
                            <b>Mẫu số 04-T/TSCĐ</b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>51-01</b>
                        </td>
                        <td rowspan="2" style="text-align: center; color: Red; float: right;">
                            <div style="width: 50px; height: 20px; font-size: 16px;">
                                <b>tài sản  chung</b>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>Đơn vị dự toán</b>
                        </td>
                    </tr>
                </table>
                <table cellpadding="4" cellspacing="4" border="0" class="table_form2">
                    <tr>
                        <td colspan="4" style="text-align: center; font-size: 15px;">
                            <b>THẺ TÀI SẢN CỐ ĐỊNH</b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="text-align: center;">
                            <b>Số:&nbsp;<span style="color: Red; font-size: 13px;">
                                <%=MyHtmlHelper.Label(iID_MaTaiSan, "sKyHieuTaiSan", "", "") %>
                                - T/TSCĐ</span></b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="height: 20px;">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%; height: 20px;">
                            <div>
                                1. Tên tài sản</div>
                        </td>
                        <td colspan="3">
                            <div>
                                <b> <%=MyHtmlHelper.Label(ParentID, sTenTaiSan, "sTenTaiSan", "", "") %></b>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div>
                                Hãng sản xuất</div>
                        </td>
                        <td colspan="3">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, data, "sHangSX_500", "", "class=\"input1_2\" tab-index='1'", 1)%></div>
                        </td>
                        
                    </tr>  
                   <tr>
                        <td>
                            <div> 3. Năm sản xuất</div>
                        </td>
                        <td>
                            <div><%=MyHtmlHelper.TextBox(ParentID, data, "iNamSX", "", "class=\"input1_2\" tab-index='10'")%></div>
                        </td>
                        <td>
                            <div style="margin-left: 5px;"> Nước sản xuất</div>
                        </td>
                        <td>
                            <div><%=MyHtmlHelper.TextBox(ParentID, data, "sNuocSX", "", "class=\"input1_2\" tab-index='11'")%></div>
                        </td>
                    </tr>
                      <tr>
                        <td>
                            <div> 4. Thời gian mua sắm</div>
                        </td>
                        <td>
                            <div><%=MyHtmlHelper.DatePicker(ParentID, data, "dNgayMua", "", "class=\"input1_2\" tab-index='12'")%></div>
                        </td>
                    </tr>
                     <tr>
                        <td>
                            <div> 5. Thời gian đưa vào sử dụng</div>
                        </td>
                        <td>
                            <div><%=MyHtmlHelper.DatePicker(ParentID, data, "dNgayDuaVaoKhauHao", "", "class=\"input1_2\" tab-index='13'")%></div>
                        </td>
                    </tr>
                      <tr>
                        <td>
                            <div> 6. Nguyên giá</div>
                        </td>
                        <td>
                            <div><%=MyHtmlHelper.TextBox(ParentID, data, "rNguyenGiaBanDau", "", "class=\"input1_2\" tab-index='14'", 1)%></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div> 7. Tên người hoặc bộ phận sử dụng: <b><%=MyHtmlHelper.Label(ParentID, "", "sTenPhongBan", "", "")%></b></div>
                        </td>
                         <td colspan="2">
                              <div><%=MyHtmlHelper.TextBox(ParentID, data, "sBoPhanSD", "", "class=\"input1_2\" tab-index='15'")%></div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 10px; font-size: 5px;" colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                            <div>
                                 <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td width="65%" class="td_form2_td5">
                                            &nbsp;
                                        </td>
                                        <td width="30%" align="right" class="td_form2_td5">
                                            <button class="button" onclick="parent.BangDuLieu_Dialog_ChiTiet_btnOK_Click('<%=ParentID %>');">OK</button>
                                        </td
                                        <td width="5px">
                                            &nbsp;
                                        </td>
                                        <td class="td_form2_td5">
                                              <button class="button" onclick="parent.BangDuLieu_Dialog_ChiTiet_btnIn_Click('<%=ParentID %>','<%=iID_MaTaiSan %>');">In</button>
                                        </td>
                                        <td width="5px">
                                            &nbsp;
                                        </td>
                                        <td class="td_form2_td5">
                                            <input type="button" class="button" onclick="parent.KTCS_TaiSan_Dialog_Close();" value="Hủy"/>                                            
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</div>
</form>
<%}%>
</asp:Content>
