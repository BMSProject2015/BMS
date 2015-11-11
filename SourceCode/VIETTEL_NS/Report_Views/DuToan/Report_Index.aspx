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
                        <th>
                            Tên báo cáo
                        </th>
                        <th style="width: 12%;">
                            Phụ lục số
                        </th>
                    </tr>
                    <%
                        switch (sLoaiBaoCao)
                        {
                            case "0":
                    %>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            1
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToan_TongHop_TongHop"), "Tổng hợp Bìa")%>
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Phụ lục số 4c-C
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            2
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToan_ThuNop_TongHop_4c_C"), "Tổng hợp dự toán ngân sách  Phần thu")%>
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Phụ lục số 4c-C
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            3
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToan_ThuNop_TongHop"), "Tổng hợp phần thu ngân sách (Biểu tổng họp theo đơn vị)")%>
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Phụ lục số 4c2-C
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            4
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToan_TongHop_1"), "Số phân bổ dự toán ngân sách các đơn vị (Phần ngân sách quốc phòng)")%>
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Phụ lục số 4d1-C
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            5
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToan_TongHop_2"), "Số phân bổ dự toán ngân sách các đơn vị (Phần ngân sách nhà nước)")%>
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Phụ lục số 4d2-C
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            6
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToan_TongHop_THB2"), "Tổng hợp phần chi ngân sách")%>
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Biểu THB-2
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            7
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptPhanBoDuToanNganSachNam"), "Công khai phân bổ dự toán ngân sách")%>
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            PL số 2
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            8
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToan_1040100_TongHop"), "Dự toán chi ngân sách quốc phòng (Phần ngân sách bảo đảm)")%>
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Phụ lục số 1a-C
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            9
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToan_1010000_TongHop"), "Dự toán chi ngân sách sử dụng (phần thường xuyên)")%>
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Phụ lục số 2a-C
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            12
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToan_1020200_TongHop"), "Dự toán chi ngân sách quốc phòng (phần ngân sách tùy viên quốc phòng)")%>
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Phụ lục số 2d-C
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            13
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToan_1030100_TongHop_XDCB"), "Dự toán chi ngân sách quốc phòng (phần ngân sách XDCB)")%>
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Phụ lục số 2d-C
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            14
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToan_1030100_TongHop_3bC"), "Dự toán chi ngân sách quốc phòng (phần ngân sách XDCB biểu 2)")%>
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            3b-C
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            15
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToan_1050000_TongHop_1"), "Dự toán chi ngân sách quốc phòng (phần chi cho Doanh Nghiệp)")%>
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            4a-C
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            16
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToan_1050000_TongHop"), "Dự toán chi ngân sách quốc phòng (phần chi cho Doanh Nghiệp chọn tờ) ")%>
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            4a2-C
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            17
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToan_1050100_TongHop"), "Dự toán chi ngân sách quốc phòng (phần chi hỗ trợ doanh nghiệp làm nhiệm vụ C) ")%>
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                           2gC
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            18
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToan_109_TongHop"), "Tổng hợp dự toán chi việc nhà nước giao tính vào NSQP")%>
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Phụ lục số 4b-C
                        </td>
                    </tr>
                     <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            19
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToan_2_TongHop"), "Tổng hợp dự toán ngân sách nhà nước ")%>
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Phụ lục số 5C
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            20
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToan_TongHop_2060101"), "Tổng hợp ngân sách người có công")%>
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Phụ lục số 5d-C
                        </td>
                    </tr>
                  
                    <%
                        break;
                            case "1":
                    %>
                  <tr>
                        <td align="center" style="padding: 3px 2px;">
                            1
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToan_TongHop_TungDonVi"), "Bìa")%>
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Biểu số 01
                        </td>
                    </tr>
                  
                     <tr>
                        <td align="center" style="padding: 3px 2px;">
                            2
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToan_ThuNop_TungDonVi"), "Dự toán ngân sách năm - Phần thu")%>
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Phụ lục số 1T
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="padding: 3px 2px;">
                            3
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToan_1040100_TungDonVi"), "DỰ TOÁN CHI NGÂN SÁCH QUỐC PHÒNG - Phần ngân sách bảo đảm toàn quân")%>
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Phụ lục số 1a
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="padding: 3px 2px;">
                            4
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToan_1040100_TungNganh"), "DỰ TOÁN CHI NGÂN SÁCH QUỐC PHÒNG - Phần phân cấp ngân sách bảo đảm toàn quân")%>
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Phụ lục số 1b
                        </td>
                    </tr>
                      <tr>
                        <td align="center" style="padding: 3px 2px;">
                            5
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToan_1010000_TungDonVi"), "Dự toán chi ngân sách sử dụng (Phần lương, phụ cấp, trợ cấp, tiền ăn)")%>
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Phụ lục số 2a
                        </td>
                    </tr>
                    
                    <tr>
                        <td align="center" style="padding: 3px 2px;">
                            6
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToan_1020100_TungDonVi"), "Dự toán chi ngân sách sử dụng (Phần ngân sách nghiệp vụ)")%>
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Phụ lục số 2b
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="padding: 3px 2px;">
                            7
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToan_1020200_TungDonVi"), "Dự toán chi ngân sách quốc phòng (Phần ngân sách Tùy viên Quốc phòng)")%>
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Phụ lục số 5d
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="padding: 3px 2px;">
                            7-1
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToan_1020100_KT_TungDonVi"), "Dự toán chi ngân sách sử dụng (Phần ngân sách nghiệp vụ ngành kỹ thuật)")%>
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Phụ lục số 2d
                        </td>
                    </tr>
                     <tr>
                        <td align="center" style="padding: 3px 2px;">
                            8
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToan_1020500_TungDonVi"), "DỰ TOÁN CHI NGÂN SÁCH QUỐC PHÒNG (Hỗ trợ các đoàn KTQP & Đơn vị sự nghiệp công lập)")%>
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Phụ lục số 2e
                        </td>
                    </tr>
                     <tr>
                        <td align="center" style="padding: 3px 2px;">
                            9
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToan_1050100_TungDonVi"), "DỰ TOÁN CHI NGÂN SÁCH QUỐC PHÒNG (Phần chi hỗ trợ doanh nghiệp làm nhiệm vụ C)")%>
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Phụ lục số 2g
                        </td>
                    </tr>
                     <tr>
                        <td align="center" style="padding: 3px 2px;">
                            10
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToan_1030100_TungDonVi"), "Dự toán chi ngân sách xây dựng cơ bản (Nguồn ngân sách Quốc phòng)")%>
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Phụ lục số 3
                        </td>
                    </tr>
                     <tr>
                        <td align="center" style="padding: 3px 2px;">
                            11
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToan_1050000_TungDonVi"), "Dự toán chi ngân sách Quốc phòng (Ngân sách hỗ trợ Doanh nghiệp)")%>
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Phụ lục số 4a
                        </td>
                    </tr>
                     
                    <tr>
                        <td align="center" style="padding: 3px 2px;">
                            12
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToan_109_TungDonVi"), "Dự toán chi việc nhà nước giao tính vào NSQP")%>
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Phụ lục số 4b
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="padding: 3px 2px;">
                            13
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToan_2_TungDonVi"), "Dự toán chi ngân sách nhà nước giao")%>
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Phụ lục số 5
                        </td>
                    </tr>
                     <tr>
                        <td align="center" style="padding: 3px 2px;">
                            14
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToan_206_TungDonVi"), "Dự toán chi ngân sách nhà nước giao(Phần chi người có công)")%>
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            Phụ lục số 5d
                        </td>
                    </tr>

                  
                   
                   
                   
                    
                   
                   
                   
                   
                    <tr>
                        <td align="center" style="padding: 3px 2px;">
                            XX
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToan_DonVi"), "In tổng hợp báo cáo đơn vị")%>
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                            IN DON VI
                        </td>
                    </tr>
                    <%
                        break;
                        }    
                    %>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
