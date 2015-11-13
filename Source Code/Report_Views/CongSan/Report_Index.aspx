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
                <tr>
                    <td align="center" style="padding: 3px 2px;">1</td>
                    <td align="center" style="padding: 3px 2px;">Mẫu số 01/SKT/TSCĐ</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptCongSan_SoTaiSanCoDinh"), "Sổ tài sản cố định")%></td>                        
                    <td></td>
                </tr>
                    <tr>
                    <td align="center" style="padding: 3px 2px;">2</td>
                    <td align="center" style="padding: 3px 2px;">Mẫu số 02/SKT/TSCĐ</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKTCS_SoTheoDoiTSCD"), "Sổ theo dõi tài sản cố định")%></td>                        
                    <td></td>
                </tr>  
                <tr>
                    <td align="center" style="padding: 3px 2px;">3</td>
                    <td align="center" style="padding: 3px 2px;">Mẫu 52</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKTCS_MauC52"), "Đánh giá lại tài sản")%></td>                        
                    <td></td>
                </tr>  

                 <tr>
                    <td align="center" style="padding: 3px 2px;">4</td>
                    <td align="center" style="padding: 3px 2px;">Mẫu 53</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKTCS_MauC53"), "Biên Bản Kiểm Kê Tài Sản")%></td>                        
                    <td></td>
                </tr> 
                 <tr>
                    <td align="center" style="padding: 3px 2px;">5</td>
                    <td align="center" style="padding: 3px 2px;">Mục - 4.4.2.6</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKTCS_HoachToanTSCD"), "Hạch Toán Tài Sản Cố Định")%></td>                        
                    <td></td>
                </tr>  
                
                 <tr>
                    <td align="center" style="padding: 3px 2px;">6</td>
                    <td align="center" style="padding: 3px 2px;">Mẫu số 06-BC/TSCĐ</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKTCS_KeKhaiNhaDat_6"), "Báo cáo kê khai nhà, đất")%></td>                        
                    <td></td>
                </tr>  
                 <tr>
                    <td align="center" style="padding: 3px 2px;">7</td>
                    <td align="center" style="padding: 3px 2px;">Mẫu số 07-BC/TSCĐ</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKTCS_KeKhaiOtO_Mau7"), "Báo cáo kê khai xe Ô TÔ")%></td>                        
                    <td></td>
                </tr>  
                
                 <tr>
                    <td align="center" style="padding: 3px 2px;">8</td>
                    <td align="center" style="padding: 3px 2px;">Mẫu số 08-BC/TSCĐ</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKTCS_KeKhaiTaiSan"), "Báo cáo kê khai tài sản có nguyên giá từ 500 triệu trở lên")%></td>                        
                    <td></td>
                </tr>  
                <tr>
                    <td align="center" style="padding: 3px 2px;">8</td>
                    <td align="center" style="padding: 3px 2px;">Mẫu số 09-BC/TSCĐ</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKTCS_BaoCaoTongHop9"), "Báo cáo tổng hợp hiện trạng sử dụng nhà đất")%></td>                        
                    <td></td>
                </tr>
               <tr>
                    <td align="center" style="padding: 3px 2px;">8</td>
                    <td align="center" style="padding: 3px 2px;">Mẫu số 10-BC/TSCĐ</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptKTCS_TongHopMau10"), "Báo cáo tổng hợp tình hình tăng giảm TSCĐ")%></td>                        
                    <td></td>
                </tr>
              <tr>
                    <td align="center" style="padding: 3px 2px;"></td>
                    <td align="center" style="padding: 3px 2px;">Phụ lục 1</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptCongSan_PhuLuc1"), "Báo cáo tổng hợp tài sản cố định")%></td>                        
                    <td></td>
                </tr>
                <tr>
                    <td align="center" style="padding: 3px 2px;"></td>
                    <td align="center" style="padding: 3px 2px;">Phụ lục 2</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptCongSan_PhuLuc2"), "Báo cáo phân loại nhà,vật kiến trúc")%></td>                        
                    <td></td>
                </tr>
                <tr>
                    <td align="center" style="padding: 3px 2px;"></td>
                    <td align="center" style="padding: 3px 2px;">Phụ lục 3</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptCongSan_PhuLuc3"), "Báo cáo phân loại phương tiện vận tải")%></td>                        
                    <td></td>
                </tr>
            </table>
        </div>
    </div>
</div>
</asp:Content>

