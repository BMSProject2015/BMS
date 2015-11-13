<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        String sLoaiBaoCao = Request.QueryString["sLoai"];    
    %>
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
                            Biểu 01/CT
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQLDA_01CT"), "Báo cáo danh mục công trình")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <!--Thuong-->
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            2
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Biểu 02/CT
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQLDA_ChiTiet_DanhMucCongTrinh"), "Báo cáo chi tiết danh mục công trình")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            2_1
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Biểu 02/CT
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQLDA_ChiTiet_DanhMucCongTrinh_1"), "Báo cáo chi tiết danh mục công trình(chonQD)")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                      <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            3
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Biểu 01/DT
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQLDA_01DT_KHVDT"), "Kế hoạch VĐT năm")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            4
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Biểu 02/DT
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKHV_02DT_DTQuy"), "Dự toán quý")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            5
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Biểu 01/CP
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQLDA_01_CP"), "Đề nghị cấp phát ")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            6
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Biểu 02/CP
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQLDA_02_CPVDT"), "BC chi tiết cấp phát VĐT năm")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            7
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Biểu 03/CP
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQLDA_03_CP_1"), "BC chi tiêt cấp phát VĐT năm")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            8
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Biểu 04/CP
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQLDA_04_CP"), "BC chi tiêt CP VĐT theo ĐV TH")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                     <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            9
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Biểu 05/CP
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQLDA_05_CP"), "BC chi tiêt CP VĐT theo ĐV TH")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            10
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Biểu 06/CP
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQLDA_03_CP"), "BC chi tiêt cấp phát VĐT năm")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            10
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Biểu 01/VĐT-N
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKHV_Bieu01VDT"), "Báo cáo tình hình TH VĐT năm")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            11
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Biểu 02/VĐT-N
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKHV_Bieu01VDTUngTruoc"), "Báo cáo tình hình TH VĐT ứng trước năm")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            12
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Biểu 01/TH-VĐT
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKHV_01VDT_THDACT"), "Báo cáo tổng hợp TH VĐT CT-DA")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                     <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            13
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Biểu 02/TH-VĐT
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKHV_02VDT_THNSNN"), "Báo cáo tổng hợp TH NSNN")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            14
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Biểu 02/QTHT
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQLDA_02_QTHT"), "Báo cáo Tổng hợp quyết toán công trình, dự án hoàn thành")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                       <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            15
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Biểu 02/TH_VDT
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKHV_Bieu02VDT"), "Báo cáo Tổng hợp tình hình thực hiện nhà nước giao")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            16
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Biểu 01/QTN
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQLDA_01QTN"), "Báo cáo Tổng hợp quyết toán vốn đầu tư")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                     <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            17
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Biểu 02/QTN_Tu
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQLDA_02QTN"), "Báo cáo Tổng hợp quyết toán năm")%>
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
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQLDA_ThongBaoKHV"), "QTDA - Thông báo kế hoạch vốn năm")%>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <%--  <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            14
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                        rptQLDA_01DT
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQLDA_01DT"), "rptQLDA_01DT")%>
                        </td>
                        <td>
                            
                        </td>
                    </tr>--%>
                    <%--   <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            15
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                        rptQLDA_01QTN
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQLDA_01QTN"), "rptQLDA_01QTN")%>
                        </td>
                        <td>
                            
                        </td>
                    </tr>--%>
                    <%--  <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            17
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                        rptQLDA_DuToan_Quy
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQLDA_DuToan_Quy"), "rptQLDA_DuToan_Quy")%>
                        </td>
                        <td>
                            
                        </td>
                    </tr>--%>
                    <%-- <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            18
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                        rptQLDA_QTHT_02QTHT
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQLDA_QTHT_02QTHT"), "rptQLDA_QTHT_02QTHT")%>
                        </td>
                        <td>
                            
                        </td>
                    </tr>--%>
                    <%--   <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            19
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                        rptQT_08A_B16
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQT_08A_B16"), "rptQT_08A_B16")%>
                        </td>
                        <td>
                            
                        </td>
                    </tr>--%>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
