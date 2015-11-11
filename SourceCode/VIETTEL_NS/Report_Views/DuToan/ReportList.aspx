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
               <table  width="100%" cellpadding="0"  cellspacing="0" border="0" class="mGrid">
                   <tr>
                        <th>STT</th>
                        <th>Tên báo cáo</th>                        
                        <th>Phụ lục số</th> 
                   </tr>
                   <tr><td colspan="3">Phân hệ dự toán</td></tr>
                   <tr>
                        <td>1</td>
                        <td><%=MyHtmlHelper.ActionLink(Url.Action("Index","rptPhanBoDuToanNganSachNam", new{sLNS="1"}),"Số phân bổ dự toán ngân sách năm(Phần ngấn sách quốc phòng)") %></td>                        
                        <td>4d1-C</td>
                   </tr>                   
                   <tr>
                        <td>2</td>
                        <td><%=MyHtmlHelper.ActionLink(Url.Action("Index","rptPhanBoDuToanNganSachNam", new{sLNS="2"}),"Số phân bổ dự toán ngân sách năm(Phần ngấn sách nhà nước)") %></td>                        
                        <td>4d2-C</td>
                   </tr>
                   
                   <tr>
                        <td>3</td>
                       <td><%=MyHtmlHelper.ActionLink(Url.Action("Index","rptDuToanChiNganSachQuocPhongDamBao"),"Dự toán chi ngân sách quốc phòng (Phần ngấn sách đảm bảo)") %></td>                                            
                       <td>1a-C</td>
                   </tr>
                   <tr>
                       <td>4</td>
                       <td><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDTCNSQP_XayDungCoBan"), "Dự toán chi ngân sách quốc phòng (Phần xây dựng cơ bản)")%></td>                                            
                       <td>3-C</td>
                   </tr>
                    <tr>
                       <td>5</td>
                       <td><%=MyHtmlHelper.ActionLink(Url.Action("Index","rptDuToanChiNganSachSuDung"),"Dự toán chi ngân sách sử dụng (Phần ngấn sách thường xuyên)") %></td>                                            
                       <td>2a-C</td>
                   </tr>
                   <tr>
                       <td>6</td>
                       <td><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToanNganSachNhaNuoc"), "Dự toán ngân sách nhà nước giao")%></td>                                            
                       <td>5-C</td>
                   </tr>
                   <tr>
                       <td>7</td>
                       <td><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToanChiNganSachQuocPhongNamTuyVien"), "Dự toán chi ngân sách quốc phòng năm tùy viên")%></td>                                            
                       <td>2d-c</td>
                   </tr>
                    <tr>
                       <td>8</td>
                       <td><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rtprptDuToanChiNganSachQuocPhongXDCBCongTrinhPhoThong"), "Dự toán chi ngân sách quốc phòng (Phần ngân sách XDCB - Công trình phổ thông)")%></td>                                            
                       <td>3c-c</td>
                   </tr>
                    <tr>
                       <td>9</td>
                       <td><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rtpDuToanChiNganSachSuDungPhanCacNganhCap"), "Dự toán chi ngân sách sử dụng(Phân ngành các cấp)")%></td>                                            
                       <td>2c-c</td>
                   </tr>
                   <tr>
                       <td>10</td>
                       <td><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToanNganSachNhaNuoc5a"), "Dự toán ngân sách nhà nước giao 5a")%></td>                                            
                       <td>5a</td>
                   </tr>

                    <tr>
                       <td>11</td>
                       <td><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToanNganSachNhaNuoc5"), "Dự toán ngân sách nhà nước giao 5")%></td>                                            
                       <td>5</td>
                    </tr>
                     <tr>
                       <td>12</td>
                       <td><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToanChiNSQP_NganSachKhac"), "Dự toán chi ngân sách quốc phòng (ngân sách khác)")%></td>                                            
                       <td>4b2-C</td>
                    </tr>

                    <tr>
                       <td>13</td>
                       <td><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToanChiNganSachQuocPhongNganSachBaoDamToanQuan"), "Dự toán chi ngân sách quốc phòng (Ngân sách đảm bảo toàn quân)")%></td>                                            
                       <td>1a</td>
                    </tr>

                   <tr><td colspan="3">Phân hệ dự toán cho đơn vị</td></tr>
                    <tr>
                       <td>1</td>
                       <td><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToanChiNganSachSuDung_PhanNganSachNghiepVu"), "Dự toán chi ngân sách sử dụng(Phần danh sách nghiệp vụ)")%></td>                                            
                       <td>2b</td>
                   </tr>
                   <tr>
                       <td>2</td>
                       <td><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDTCNSSuDungTheoDonVi"), "Dự toán chi ngân sách sử dụng(Phần lương , Phụ cấp , trợ cấp , tiền ăn)")%></td>                                            
                       <td>2a</td>
                   </tr>
                    <tr>
                       <td>3</td>
                       <td><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToanChiNganSachQuocPhongNamTuyVien_DonVi"), "Dự toán chi ngân sách  quoc phong nam tuy vien  theo don vi")%></td>                                            
                       <td>2d</td>
                   </tr>
                   <tr>
                       <td>4</td>
                       <td><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDuToanChiNSQP_NganSachKhac_DonVi"), "Dự toán chi ngân sách quốc phòng (ngân sách khác) theo đơn vị")%></td>                                            
                       <td>4b</td>
                   </tr>
                    <tr>
                       <td>5</td>
                       <td><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDTCNSXayDungCoBan_PhanNganSachQP"), "Dự toán chi ngân sách xây dựng cơ bản ( Phần ngân sách quốc phòng )")%></td>                                            
                       <td>3</td>
                   </tr>
                   <tr>
                       <td>6</td>
                       <td><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDTCNSNhaNuoc_PhanChiNguoiCoCong"), "Dự toán chi ngân sách nhà nước giao ( Phần chi người có công )")%></td>                                            
                       <td>5d</td>
                   </tr>
                     <tr>
                       <td>7</td>
                       <td><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptDTCNSQP_NganSachHoTroDoanhNghiepTheoDonVi"), "Dự toán chi ngân sách Quốc Phòng ( Ngân sách hỗ trợ Doanh Nghiệp )")%></td>                                            
                       <td>4a</td>
                   </tr>
                   <tr><td colspan="3">Phân hệ phân bổ chỉ tiêu</td></tr>
                   
                   
               </table>
            </div>
        </div>
  </div>
</asp:Content>
