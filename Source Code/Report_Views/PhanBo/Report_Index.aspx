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
    sLoaiBaoCao = "0";
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
                    <th style="width: 3%;">STT</th>
                    <th>Tên báo cáo</th>                        
                    <th style="width: 25%;">Phụ lục số</th> 
                </tr>
                <%
                    switch (sLoaiBaoCao) { 
                        case "0":
                            %>
                            <tr class="alt">
                                <td align="center" style="padding: 3px 2px;">1</td>
                                <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptThongBaoCapChiTieuNganSachNam"), "Thông báo ngân sách- Thông báo")%></td>                        
                                <td align="center" style="padding: 3px 2px;">4_1_Nghiep</td>
                            </tr>  
                             <tr class="alt">
                                <td align="center" style="padding: 3px 2px;">2</td>
                                <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptPhanBo_4_2"), "Thông báo ngân sách- Thông báo lũy kế")%></td>                        
                                <td align="center" style="padding: 3px 2px;">4_2_Nghiep</td>
                            </tr>
                            <tr class="alt">
                                <td align="center" style="padding: 3px 2px;">3</td>
                                <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptThongBaoBoSungNganSach"), "Thông báo ngân sách- Thông báo tổng hợp")%></td>                        
                                <td align="center" style="padding: 3px 2px;">4_3_Nghiep</td>
                            </tr>
                            <tr class="alt">
                                <td align="center" style="padding: 3px 2px;">4</td>
                                <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptThongBaoCapChiTieuNganSach"), "Thông báo ngân sách- Thông báo LK")%></td>                        
                                <td align="center" style="padding: 3px 2px;">4_4_Tien</td>
                            </tr>    
                             <tr class="alt">
                                <td align="center" style="padding: 3px 2px;">5</td>
                                <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptTongHopChiTieu"), "Tổng hợp chỉ tiêu")%></td>                        
                                <td align="center" style="padding: 3px 2px;">5_Nghiep</td>
                            </tr>       
                             <tr class="alt">
                                <td align="center" style="padding: 3px 2px;">6</td>
                                <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptTongHopChiTieuDonVi"), "Tổng hợp chỉ tiêu đơn vị ")%></td>                        
                                <td align="center" style="padding: 3px 2px;">6_Tuan</td>
                            </tr>                                
                             <tr class="alt">
                                <td align="center" style="padding: 3px 2px;">7</td>
                                <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptCT_TongHopCT7"), "Tổng hợp chỉ tiêu ngân sách quốc phòng ")%></td>                        
                                <td align="center" style="padding: 3px 2px;">7_Tien</td>
                            </tr>                                   
                             <tr class="alt">
                                <td align="center" style="padding: 3px 2px;">8</td>
                                <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptPhanBo_8_8"), "Tổng hợp chỉ tiêu chọn đơn vị ")%></td>                        
                                <td align="center" style="padding: 3px 2px;">8_Nghiep</td>
                            </tr>
                              <tr class="alt">
                                <td align="center" style="padding: 3px 2px;">9</td>
                                <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptPB_TongHopChiTieuNganSachQuocPhong_9"), "Tổng hợp chỉ tiêu bổ sung đơn vị")%></td>                        
                                <td align="center" style="padding: 3px 2px;">9_Tien</td>
                            </tr>
                            <tr class="alt">
                                <td align="center" style="padding: 3px 2px;">10</td>
                                <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptPhanBo_10"), "Tổng Hợp Chỉ Tiêu phân bổ đến đợt- đợt  ")%></td>                        
                                <td align="center" style="padding: 3px 2px;">10_Tien</td>
                            </tr>
                            <tr class="alt">
                                <td align="center" style="padding: 3px 2px;">11</td>
                                <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptPhanBo_11_11"), "Tổng Hợp Chỉ Tiêu phân bổ đến đợt- đơn vị ")%></td>                        
                                <td align="center" style="padding: 3px 2px;">11_Nghiep</td>
                            </tr>     
                             <tr class="alt">
                                <td align="center" style="padding: 3px 2px;">12</td>
                                <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptPhanBo_12"), "Tổng Hợp Chỉ Tiêu phân bổ đến đơn vị- đợt ")%></td>                        
                                <td align="center" style="padding: 3px 2px;">12_Tien</td>
                            </tr>  
                             <tr class="alt">
                                <td align="center" style="padding: 3px 2px;">13</td>
                                <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptTongHopNganSach"), "Tổng Hợp Chỉ Tiêu đến đợt ")%></td>                        
                                <td align="center" style="padding: 3px 2px;">A_Tien</td>
                            </tr>       
                            
                             <tr class="alt">
                                <td align="center" style="padding: 3px 2px;">14</td>
                                <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptThongBaoChiTieuNganSachNamTheoLNSvaDonVi"), "Thông báo chỉ tiêu ngân sách theo Loại ngân sách và đơn vị ")%></td>                        
                                <td align="center" style="padding: 3px 2px;">B_Tien</td>
                            </tr>
                            
                              <tr class="alt">
                                <td align="center" style="padding: 3px 2px;">15</td>
                                <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptPhanBo_15"), "Tổng hợp cấp ngân sách ")%></td>                        
                                <td align="center" style="padding: 3px 2px;">C_Tien</td>
                            </tr>                
                            <tr class="alt">
                                <td align="center" style="padding: 3px 2px;">16</td>
                                <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptPhanBo_16D_TheoNganh"), "Tổng Hợp chỉ tiêu theo MLNS+Đơn vị ")%></td>                        
                                <td align="center" style="padding: 3px 2px;">16-D_Nghiep</td>
                            </tr>
                             <tr class="alt">
                                <td align="center" style="padding: 3px 2px;">17</td>
                                <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptPhanBo_18E_TheoNganh"), " Tổng hợp chỉ tiêu theo ngành ")%></td>                        
                                <td align="center" style="padding: 3px 2px;">18-E_Nghiep</td>
                            </tr>

                             <tr class="alt">
                                <td align="center" style="padding: 3px 2px;">18</td>
                                <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptPhanBo_21R"), "Chỉ tiêu duyệt tổng quyết toán")%></td>                        
                                <td align="center" style="padding: 3px 2px;">21_R_Nghiep</td>
                            </tr>
                           <tr class="alt">
                                <td align="center" style="padding: 3px 2px;">19</td>
                                <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptPhanBo_19P"), "Tổng hợp năm - LNS")%></td>                        
                                <td align="center" style="padding: 3px 2px;">19P_Nghiep</td>
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
