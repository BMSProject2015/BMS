<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


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
                    <th style="width: 3%;">STT</th>
                    <th style="width: 15%;">Mã báo cáo - phụ lục</th>                        
                    <th style="width: 60%;">Tên báo cáo</th>
                    <th style="width: 22%;">Ghi chú</th>
                </tr>  
                                       
                            
                            
                             <tr class="alt">
                                <td align="center" style="padding: 3px 2px;">1</td>
                                 <td align="center" style="padding: 3px 2px;"></td>
                                <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKTKhoBac_DoiChieuDuToan"), "Bảng đối chiếu với KBNN")%></td>                        
                                 <td>Le</td>
                            </tr>
                         
                        
                          <tr class="alt">
                                <td align="center" style="padding: 3px 2px;">2</td>
                                 <td align="center" style="padding: 3px 2px;"></td>
                                <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDoiChieuSuDungKinhPhi"), "Đối chiếu tình hình sử dụng kinh phí")%></td> 
                                <td>Le</td>
                            </tr> 
                            <tr class="alt">
                                <td align="center" style="padding: 3px 2px;">3</td>
                                 <td align="center" style="padding: 3px 2px;"></td>
                                <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKiemTraSoLieuRutDT"), "Kiểm tra số liệu rút dự toán")%></td> 
                                <td>Le</td>
                            </tr>  
                             <tr class="alt">
                                <td align="center" style="padding: 3px 2px;">4</td>
                                 <td align="center" style="padding: 3px 2px;"></td>
                                <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKTKB_BangKeRutDuToan"), "Bảng kê rút dự toán")%></td> 
                                <td>Nghiep</td>
                            </tr>     
                         <tr class="alt">
                            <td align="center" style="padding: 3px 2px;">5</td>
                            <td align="center" style="padding: 3px 2px;"></td>
                            <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKT_TongHop_RutDuToan"), "Tổng hợp rút dự toán")%></td> 
                            <td>Th</td>
                        </tr>   
                        <tr class="alt">
                            <td align="center" style="padding: 3px 2px;">6</td>
                            <td align="center" style="padding: 3px 2px;"></td>
                            <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptPhanTichDuToanNS_52"), "Phân tích DTNS theo LNS")%></td> 
                            <td>Quang</td>
                        </tr>  
                        <tr class="alt">
                            <td align="center" style="padding: 3px 2px;">7</td>
                            <td align="center" style="padding: 3px 2px;">Mẫu số: S75-H</td>
                            <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKTKB_TheoDoiTamUng_S75H"), "Theo dõi tạm ứng tại kho bạc")%></td> 
                            <td>Th</td>
                        </tr>   
                        <tr class="alt">
                            <td align="center" style="padding: 3px 2px;">8</td>
                            <td align="center" style="padding: 3px 2px;"></td>
                            <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDoiChieuCTMT"), "Đối chiếu CT-MT")%></td> 
                            <td>le</td>
                        </tr>   
                         <tr class="alt">
                            <td align="center" style="padding: 3px 2px;">9</td>
                            <td align="center" style="padding: 3px 2px;"></td>
                            <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKTKB_SoSanhDuToanNganSachDaSuDung"), "So sánh chỉ tiêu – Rút DT")%></td> 
                            <td>Tuan</td>
                        </tr>   
                         <tr class="alt">
                            <td align="center" style="padding: 3px 2px;">10</td>
                            <td align="center" style="padding: 3px 2px;"></td>
                            <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKTKB_TheoDoiDuToan"), "Theo dõi Dự toán NS")%></td> 
                            <td>Nghiep</td>
                        </tr>  

                        <tr class="alt">
                            <td align="center" style="padding: 3px 2px;">11</td>
                            <td align="center" style="padding: 3px 2px;"></td>
                            <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKTTienGui_UNC"), "Bảng kê chi tiết tài khoản tiền gửi")%></td> 
                            <td>Quang</td>
                        </tr>  
                         <tr class="alt">
                            <td align="center" style="padding: 3px 2px;">11</td>
                            <td align="center" style="padding: 3px 2px;"></td>
                            <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKTTienGui_SoTienGui"), "Số Tiền Gửi")%></td> 
                            <td>Duylv</td>
                        </tr>  
                         <tr class="alt">
                            <td align="center" style="padding: 3px 2px;">12</td>
                            <td align="center" style="padding: 3px 2px;"></td>
                            <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptThongTri_CapThuThanhKhoan"), "Thông tri cấp - thu thanh khoản")%></td> 
                            <td>le</td>
                        </tr> 
                         <tr class="alt">
                            <td align="center" style="padding: 3px 2px;">13</td>
                            <td align="center" style="padding: 3px 2px;"></td>
                            <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKeToanTongHopNgay"), "KTTG-Tổng hợp ngày")%></td> 
                            <td>Ng</td>
                        </tr>     
                          <tr class="alt">
                            <td align="center" style="padding: 3px 2px;">14</td>
                            <td align="center" style="padding: 3px 2px;"></td>
                            <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKTTG_SoChiTietTienGui"), "KTTG-Sổ chi tiết tiền gủi")%></td> 
                            <td>Ng</td>
                        </tr>     
                          <tr class="alt">
                            <td align="center" style="padding: 3px 2px;">15</td>
                            <td align="center" style="padding: 3px 2px;"></td>
                            <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKTTG_TongHopSoDuVonBangTien"), "KTTG-Tổng hợp số dư vốn bằng tiền")%></td> 
                            <td>Ng</td>
                        </tr>  
                         <tr class="alt">
                            <td align="center" style="padding: 3px 2px;">16</td>
                            <td align="center" style="padding: 3px 2px;"></td>
                            <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKTTG_TongHopThucThu_ThucChiVonBangTien"), "KTTG-Tổng hợp thực thu thực chi vốn bằng tiền")%></td> 
                            <td>Ng</td>
                        </tr> 
                         <tr class="alt">
                            <td align="center" style="padding: 3px 2px;">17</td>
                            <td align="center" style="padding: 3px 2px;"></td>
                            <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKTTG_TongHopThuVonBangTien"), "KTTG-Tổng hợp thu vốn bằng tiền")%></td> 
                            <td>Ng</td>
                        </tr> 
                         <tr class="alt">
                            <td align="center" style="padding: 3px 2px;">18</td>
                            <td align="center" style="padding: 3px 2px;"></td>
                            <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKTTG_XacNhanSoDuTaiKhoan"), "KTTG-Xác nhận số dư tài khoản")%></td> 
                            <td>Ng</td>
                        </tr>                         
            </table>
        </div>
    </div>
</div>
</asp:Content>
