<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    String iID_MaTaiSan =Convert.ToString(ViewData["iID_MaTaiSan"]);
    DataTable dt= KTCS_TaiSan_DonViModels.DanhSachDieuChuyen(iID_MaTaiSan);
%>
<div style="width: 100%; float: left;">
    <div style="width: 100%; float:left;">
        <div class="box_tong">
            <div class="title_tong">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td>
                            <span>Danh sách tài sản</span>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="nhapform">
                <div id="form2">                    
                    <table cellpadding="0" cellspacing="0" border="0" class="mGrid" width="100%">
                        <tr>
                            <th>STT</th>
                            <th>Loại</th>
                            <th>Nhóm</th>
                            <th>Mã tài sản</th>
                            <th>Tên tài sản</th>
                            <th>Số năm hao mòn</th>
                            <th>Nguyên giá</th>
                            <th>Đơn vị</th>
                            <th>Thao tác</th>
                        </tr>
                        <%for (int i = 0; i < dt.Rows.Count; i++)
                          {
                              String MaTaiSan = Convert.ToString(dt.Rows[i]["iID_MaTaiSan"]);
                              String rNguyenGia = Convert.ToString(dt.Rows[i]["rNguyenGia"]);
                              String urlThanhLy = Url.Action("ThanhLy", "KTCS_TaiSan", new { iID_MaTaiSan = MaTaiSan });
                              String DaThanhLy = "Chưa thanh lý";
                              if (Convert.ToBoolean(dt.Rows[i]["bDaKhauHao"]) == false)
                              {
                                  DaThanhLy = "Đã thanh lý";
                              }
                          %>
                              
                        <tr>
                            <td><%=i+1 %></td>
                            <td><%=dt.Rows[i]["iLoaiTS"]%></td>
                            <td><%=dt.Rows[i]["sTenNhomTaiSan"]%></td>
                            <td><%=dt.Rows[i]["iID_MaTaiSan"]%></td>
                            <td><%=dt.Rows[i]["sTenTaiSan"]%></td>
                            <td><%=dt.Rows[i]["rSoNamKhauHao"]%></td>
                            <td align="right"><%=CommonFunction.DinhDangSo(rNguyenGia)%></td>                            
                            <td></td>
                        </tr>
                        <%} %>
                    </table>
                </div>
            </div>
        </div>
    </div>
</div>

</asp:Content>
