<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    String sLoaiBaoCao = Request.QueryString["Loai"];    
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
        <%-- Quyết toán thường xuyên sLoaiBaoCao=0 --%>
            <%if (sLoaiBaoCao == "0")
              { %>
            <table class="mGrid">
                <tr>
                    <th style="width: 3%;">STT</th>
                    <th style="width: 15%;">Mã báo cáo - phụ lục</th>                        
                    <th style="width: 60%;">Tên báo cáo</th>
                    <th style="width: 22%;">Ghi chú</th>
                </tr>
                  <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">1</td>
                    <td align="center" style="padding: 3px 2px;">Mẫu số C08-D</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_ThongTri"), "Quyết toán thông tri")%></td>                        
                    <td align="center" style="padding: 3px 2px;">VungNV</td>
                </tr>
                <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">2</td>
                    <td align="center" style="padding: 3px 2px;">Biểu kiểm</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_BieuKiemAll"), "Biểu kiểm")%></td>                        
                    <td></td>
                </tr>
                <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">3</td>
                    <td align="center" style="padding: 3px 2px;">Phụ lục số 2a</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_LNS_DonVi"), "Quyết toán chọn LNS_DonVi")%></td>                        
                    <td align="center" style="padding: 3px 2px;">VungNV</td>
                </tr>
                <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">4</td>
                    <td align="center" style="padding: 3px 2px;">Phụ lục số 2a</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_DonVi_LNS"), "Quyết toán chọn DonVi_LNS")%></td>                        
                     <td align="center" style="padding: 3px 2px;">HungPX</td>
                    <td></td>
                </tr>
                 <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">5</td>
                    <td align="center" style="padding: 3px 2px;"></td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_TongHop_LNS"), "Quyết toán Tổng hợp chọn LNS")%></td>                        
                    <td align="center" style="padding: 3px 2px;">TuNB</td>
                </tr>
                 <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">6</td>
                    <td align="center" style="padding: 3px 2px;"></td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_TongHop_Nam_Quy"), "Quyết toán Tổng hợp Năm-Quý")%></td>                        
                    <td align="center" style="padding: 3px 2px;">TuNB</td>
                </tr>

                <%--quydq: 2015/09/21 --%>
                 <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">7</td>
                    <td align="center" style="padding: 3px 2px;"></td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_PhongBan"), "Quyết toán Phòng Ban")%></td>                        
                    <td align="center" style="padding: 3px 2px;">QuyDQ</td>
                </tr>
                <%--hungph: 2015/09/21 --%>
                 <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">7</td>
                    <td align="center" style="padding: 3px 2px;"></td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_TongQuyetToan_LNS_DonVi"), "Quyết toán Tổng hợp")%></td>                        
                    <td align="center" style="padding: 3px 2px;">hungph</td>
                </tr>
                <%--hungph: 2015/10/14 --%>
                 <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">7</td>
                    <td align="center" style="padding: 3px 2px;"></td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_TongHop_NhapSoLieu"), "Quyết toán Tổng hợp Nhập số liệu")%></td>                        
                    <td align="center" style="padding: 3px 2px;">hungph</td>
                </tr>
               <%-- <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">1</td>
                    <td align="center" style="padding: 3px 2px;">22_1</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_ThuongXuyen_22_1"), "Nhập dữ liệu")%></td>                        
                    <td>Ng</td>
                </tr>                   
                <tr>
                    <td align="center" style="padding: 3px 2px;">2</td>
                    <td align="center" style="padding: 3px 2px;">23_2</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_ThuongXuyen_23_2"), "Báo cáo quyết toán theo nhóm")%></td>                        
                    <td>Ng</td>
                </tr>  
                <tr>
                    <td align="center" style="padding: 3px 2px;">3</td>
                    <td align="center" style="padding: 3px 2px;">24_4</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_ThuongXuyen_24_4"), "So Sánh chỉ tiêu quyết toán")%></td>                        
                    <td>Ng</td>
                </tr>  
                <tr>
                    <td align="center" style="padding: 3px 2px;">4</td>
                    <td align="center" style="padding: 3px 2px;">25_5</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_25_5"), "Tổng hợp quyết toán tiết mục")%></td>                        
                    <td>Ng</td>
                </tr>                  
                <tr>
                    <td align="center" style="padding: 3px 2px;">5</td>
                    <td align="center" style="padding: 3px 2px;">26_6</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_ThongTri_6"), "Thông tri theo mục")%></td>                        
                    <td>Ti</td>
                </tr>
                <tr>
                    <td align="center" style="padding: 3px 2px;">6</td>
                    <td align="center" style="padding: 3px 2px;">27_7</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_ThongTri_7"), "Thông tri theo ngành")%></td>                        
                    <td>Ti</td>
                </tr>
                <tr>
                    <td align="center" style="padding: 3px 2px;">7</td>
                    <td align="center" style="padding: 3px 2px;">28_8</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_83"), "Tổng hợp chuẩn quyết toán KP lương, phụ cấp, tiền ăn")%></td>                        
                    <td>Ti</td>
                </tr>
                <tr>
                    <td align="center" style="padding: 3px 2px;">8</td>
                    <td align="center" style="padding: 3px 2px;">28_8b</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_8b"), "Tổng hợp chuẩn quyết toán KP lương, phụ cấp, tiền ăn(Nhóm)")%></td>                        
                    <td>Ti</td>
                </tr>  
                 <tr>
                    <td align="center" style="padding: 3px 2px;">9</td>
                    <td align="center" style="padding: 3px 2px;">29_A</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_29A"), "Tổng hợp quyết toán thường xuyên tháng quý")%></td>                        
                    <td>Ng</td>
                </tr>
                <tr>
                    <td align="center" style="padding: 3px 2px;">10</td>
                    <td align="center" style="padding: 3px 2px;">30_A1</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_30A1"), "Chi tiết quyết toán thường xuyên quý tháng")%></td>                        
                    <td>Ng</td>
                </tr>                     
                <tr>
                    <td align="center" style="padding: 3px 2px;">11</td>
                    <td align="center" style="padding: 3px 2px;">31_A2</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_31A2"), "Tổng hợp 3 yếu tố")%></td>                        
                    <td>Ng</td>
                </tr>  
                  <tr>
                    <td align="center" style="padding: 3px 2px;">12</td>
                    <td align="center" style="padding: 3px 2px;">32_A3</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_ThuongXuyen_32A3"), "Tổng hợp 3 yếu tố chọn đơn vị")%></td>                        
                    <td>Ng</td>
                </tr>
                <tr>
                    <td align="center" style="padding: 3px 2px;">13</td>
                    <td align="center" style="padding: 3px 2px;">33_B</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_33B"), "Tổng hợo QTTX Năm-Quý")%></td>                        
                    <td>Ng</td>
                </tr>
                <tr>
                    <td align="center" style="padding: 3px 2px;">14</td>
                    <td align="center" style="padding: 3px 2px;">34_C</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_ThuongXuyen_34C"), "Tổng hợp quyết toán thường xuyên theo đơn vị")%></td>                        
                    <td>Ng</td>
                </tr>
                     <tr>
                    <td align="center" style="padding: 3px 2px;">15</td>
                    <td align="center" style="padding: 3px 2px;">34_D</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_ThuongXuyen_34D"), "Tổng hợp quyết toán thường xuyên theo nhóm")%></td>                        
                    <td>Ng</td>
                </tr>  
                <tr>
                    <td align="center" style="padding: 3px 2px;">16</td>
                    <td align="center" style="padding: 3px 2px;">34_E</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_ThuongXuyen_34E"), "Tổng hợp quyết toán thường xuyên chọn đơn vị")%></td>                        
                    <td>Ng</td>
                </tr>  
                     <tr>
                    <td align="center" style="padding: 3px 2px;">17</td>
                    <td align="center" style="padding: 3px 2px;">35_F</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_35F"), "Tổng hợp theo tháng")%></td>                        
                    <td>Ng</td>
                </tr>  --%>                                         
            </table>
            <%}
              else if (sLoaiBaoCao == "1") 
              {%>
              <%-- Quyết toán nghiệp vụ sLoaiBaoCao=1 --%>
              <table class="mGrid">
                <tr>
                    <th style="width: 3%;">STT</th>
                    <th style="width: 15%;">Mã báo cáo - phụ lục</th>                        
                    <th style="width: 60%;">Tên báo cáo</th>
                    <th style="width: 22%;">Ghi chú</th>
                </tr>
                <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">1</td>
                    <td align="center" style="padding: 3px 2px;">36_1</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_nghiepvu_36_1"), "Nhập dữ liệu")%></td>                        
                    <td>Ng</td>
                </tr>   
                 <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">2</td>
                    <td align="center" style="padding: 3px 2px;">40_2</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_nghiepvu_40_2"), "Tổng hợp quyết toán Quý theo đơn vị")%></td>                        
                    <td>Ng</td>
                </tr>  
                  <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">3</td>
                    <td align="center" style="padding: 3px 2px;">40_3</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_nghiepvu_40_3"), "Tổng hợp quyết toán Quý theo nhóm")%></td>                        
                    <td>Ng</td>
                </tr> 
                  <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">4</td>
                    <td align="center" style="padding: 3px 2px;">40_4</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_nghiepvu_40_4"), "Tổng hợp quyết toán Quý chọn đơn vị")%></td>                        
                    <td>Ng</td>
                </tr>                
                <tr>
                    <td align="center" style="padding: 3px 2px;">5</td>
                    <td align="center" style="padding: 3px 2px;">41_5</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_ThongTri_5"), "Thông tri chuẩn quyết toán theo mục")%></td>                        
                    <td>Ti</td>
                </tr>  
                  <tr>
                    <td align="center" style="padding: 3px 2px;">6</td>
                    <td align="center" style="padding: 3px 2px;">41_5_Nganh</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_ThongTri_5_Nganh"), "Thông tri chuẩn quyết toán theo ngành")%></td>                        
                    <td>Ti</td>
                </tr>  
                   <tr>
                    <td align="center" style="padding: 3px 2px;">7</td>
                    <td align="center" style="padding: 3px 2px;">42_6</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_42_6"), "Tổng hợp chuẩn quyết toán")%></td>                        
                    <td>Ng</td>
                   </tr>
                   <tr>
                    <td align="center" style="padding: 3px 2px;">8</td>
                    <td align="center" style="padding: 3px 2px;">42_6b</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_42_6b"), "Tổng hợp chuẩn quyết toán-Nhóm")%></td>                        
                    <td>Ng</td>
                </tr>
                <tr>
                    <td align="center" style="padding: 3px 2px;">9</td>
                    <td align="center" style="padding: 3px 2px;">43_7</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_43_7"), "Tổng hợp quyết toán Năm-Quý")%></td>                        
                    <td>Ng</td>
                </tr>
                    <tr>
                    <td align="center" style="padding: 3px 2px;">10</td>
                    <td align="center" style="padding: 3px 2px;">44_8</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_44_8"), "Tổng hợp quyết toán Quý-Tháng")%></td>                        
                    <td>Ng</td>
                </tr>  
                   
                <tr>
                    <td align="center" style="padding: 3px 2px;">11</td>
                    <td align="center" style="padding: 3px 2px;">45_9</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_nghiepvu_45_9"), "So sánh chỉ tiêu quyết toán")%></td>                        
                    <td>Ng</td>
                </tr>  
                  <tr>
                    <td align="center" style="padding: 3px 2px;">12</td>
                    <td align="center" style="padding: 3px 2px;">46_A</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_NghiepVu_46A"), "Tổng hợp quyết toán tháng theo đơn vị")%></td>                        
                    <td>Ng</td>
                </tr>
                 <tr>
                    <td align="center" style="padding: 3px 2px;">13</td>
                    <td align="center" style="padding: 3px 2px;">46_B</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_NghiepVu_46B"), "Tổng hợp quyết toán tháng theo nhóm đơn vị")%></td>                        
                    <td>Ng</td>
                </tr>
                    <tr>
                    <td align="center" style="padding: 3px 2px;">14</td>
                    <td align="center" style="padding: 3px 2px;">46_C</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_NghiepVu_46C"), "Tổng hợp quyết toán tháng chọn đơn vị")%></td>                        
                    <td>Ng</td>
                 </tr>    
                   <tr>
                    <td align="center" style="padding: 3px 2px;">15</td>
                    <td align="center" style="padding: 3px 2px;">5b_1</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToanNghiepVu_5b_1"), "Thông tri chuẩn quyết toán-NSNN-Chi tiết")%></td>                        
                    <td>Ng</td>
                 </tr>
                   <tr>
                    <td align="center" style="padding: 3px 2px;">16</td>
                    <td align="center" style="padding: 3px 2px;">5b_2</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToanNghiepVu_5b_TongHop"), "Thông tri chuẩn quyết toán-NSNN-Tổng hợp")%></td>                        
                    <td>Ng</td>
                 </tr>              
                    <tr>
                    <td align="center" style="padding: 3px 2px;">17</td>
                    <td align="center" style="padding: 3px 2px;">38_1a</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToan_NghiepVu_38_1a"), "Báo cáo quyết toán kinh phí")%></td>                        
                    <td>Th</td>
                 </tr>                                                  
            </table>
            <%}
            else if (sLoaiBaoCao == "5") 
              {%>
              <%-- Quyết toán Nam sLoaiBaoCao=5 --%>
              <table class="mGrid">
                <tr>
                    <th style="width: 3%;">STT</th>
                    <th style="width: 15%;">Mã báo cáo - phụ lục</th>                        
                    <th style="width: 60%;">Tên báo cáo</th>
                    <th style="width: 22%;">Ghi chú</th>
                </tr>
                <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">1</td>
                    <td align="center" style="padding: 3px 2px;">Biểu 2A </td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToanNam_2a"), "Báo cáo số liệu kết luận quyết toán ngân sách")%></td>                        
                    <td></td>
                </tr>   
                  <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">2</td>
                    <td align="center" style="padding: 3px 2px;">PhuLuc</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToanNam_SoPheDuyet"), "Số phê duyệt quyết toán ngân sách")%></td>                        
                    <td></td>
                </tr> 
                 <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">3</td>
                    <td align="center" style="padding: 3px 2px;"> </td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQuyetToanNam_ThongBaoSoChuyenNamSau"), "Báo cáo thông báo số chuyển năm sau")%></td>                        
                    <td></td>
                </tr> 
                                                               
            </table>
            <%}%>
        </div>
    </div>
</div>
</asp:Content>

