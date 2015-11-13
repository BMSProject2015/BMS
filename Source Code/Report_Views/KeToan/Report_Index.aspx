<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td align="left" style="width: 12%;">
                <div style="padding-left: 22px; padding-bottom: 5px; text-transform: uppercase; color: #ec3237;">
                    <b>
                        <%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
                </div>
            </td>
            <td align="left">
                <div style="padding-bottom: 5px; color: #ec3237;">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>
                </div>
            </td>
            <td align="right" style="padding-bottom: 5px; color: #ec3237; font-weight: bold;
                padding-right: 20px;">
                <% Html.RenderPartial("LogOnUserControl_KeToan"); %>
            </td>
        </tr>
    </table>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Danh sách báo cáo</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <table class="mGrid">
                    <tr>
                        <th style="width: 3%;">
                            STT
                        </th>
                        <th style="width: 15%;">
                            Mã báo cáo - phụ lục
                        </th>
                        <th style="width: 60%;">
                            Tên báo cáo
                        </th>
                        <th style="width: 22%;">
                            Ghi chú
                        </th>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            1
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKeToanTongHop_CanDoiTaiKhoan"), "Bảng cân đối tài khoản")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                       <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            2
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKeToanTongHop_SoCai"), "Nhật ký - Sổ cái")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                     <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            3
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKeToanTongHop_PhanHo"), "Phân hộ")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                        <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            4
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKeToan_TongHopPhanHo"), "Tổng hợp phân hộ")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                      <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            5
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKeToan_SoCaiChiTiet"), "Sổ cái chi tiết")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                         <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            6
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptChiTietTheoDonVi_2"), "Chi tiết tài khoản/đơn vị")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                    
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            7
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKTTongHop_ChiTiet_TongHopTaiKhoan"), "Chi tiết + tổng hợp tài khoản")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                      <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            8
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKeToanTongHop_GiaiThichSoDuTaiKhoan"), "Giải thích số dư tài khoản")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                    
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            9
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptPhanHoGiaiDoan"), "Phân hộ giai đoạn - Chi tiết tài khoản theo đơn vị mẫu 2")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            10
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKeToan_InBia"), "In bìa")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                  
                
                 
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            11
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptTongHopTaiKhoan"), "Tổng hợp tài khoản")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                  
                   
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            12
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKT_ChiTiet_TaiKhoanTheoDonVi1"), "Biểu kỹ thuật 3")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            13
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKeToan_BangCanDoiSoDu"), "Cân đối số dư")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                   
               
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            14
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKTTongHop_QuanHeDoiUng_TaiKhoan"), "Quan hệ đối ứng tài khoản")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            15
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKTTongHop_CanDoiNguonVaVon"), "Bảng cân đối nguồn và vốn")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            16
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKT_SoQuy"), "Sổ qũy")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            17
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKTTH_TongHopCapVon"), "Tổng hợp cấp vốn")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                        <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            18
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKeToan_QuyetToanNam_DonVi"), "Quyết toán năm")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                  <%--   <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            18
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKTTongHop_ChiTietCacKhoanTamUng"), "Chi tiết các khoản tạm ứng")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            19
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKTTH_ChiTietPhaiThu"), "Chi tiết phải thu")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            20
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKeToanTongHop_CanDoiThuChiTaiChinh"), "Cân đối thu chi tài chính")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            21
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKTTK_ChiTietTamThu"), "Chi tiết tạm thu")%>
                        </td>
                        <td>
                        </td>
                    </tr>--%>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
