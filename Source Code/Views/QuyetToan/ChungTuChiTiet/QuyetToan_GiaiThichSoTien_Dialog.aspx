<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<%
    String ParentID="QTGT";
    String iID_MaChungTu=Request.QueryString["iID_MaChungTu"];
    NameValueCollection data = QuyetToan_ChungTuChiTietModels.LayThongTin_GiaiThichSoTien(iID_MaChungTu);
     %>
<form action="<%=Url.Action("GiaiThichSoTien_Submit", "QuyetToan_ChungTuChiTiet", new {ParentID = ParentID,iID_MaChungTu=iID_MaChungTu})%>" method="post">
 <%=MyHtmlHelper.Hidden(ParentID,data,"DuLieuMoi","") %>
 <%=MyHtmlHelper.Hidden(ParentID,"310","sKyHieuDoiTuong_310","") %>
 <%=MyHtmlHelper.Hidden(ParentID,"320","sKyHieuDoiTuong_320","") %>
 <%=MyHtmlHelper.Hidden(ParentID,"330","sKyHieuDoiTuong_330","") %>
 <%=MyHtmlHelper.Hidden(ParentID,data, "iID_MaGiaiThichSoTien_310", "")%>
 <%=MyHtmlHelper.Hidden(ParentID,data, "iID_MaGiaiThichSoTien_320", "")%>
 <%=MyHtmlHelper.Hidden(ParentID,data, "iID_MaGiaiThichSoTien_330", "")%>
<div style="background-color: #f0f9fe; background-repeat: repeat; border: solid 1px #ec3237">
    <div style="padding: 10px;">
        <div id="nhapform">
            <div id="form2">
                <table cellpadding="1" cellspacing="1" border="0" class="table_form2" width="100%">
                    <tr>
                        <td style="width: 250px;">
                            <b>I Tiền lương xin quyết toán</b>
                        </td>
                        <td>
                            <b>Sĩ quan</b>
                        </td>
                        <td>
                            <b>QNCN</b>
                        </td>
                        <td>
                            <b>CNVCQP</b>
                        </td>
                        <td>
                            <b>Hợp đồng</b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <b>&nbsp;&nbsp;1. Tiền lương tháng</b>
                        </td>                        
                    </tr>
                    <tr>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;Trong đó:- Lương ngạch bậc
                        </td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rSiQuan1", "", "readonly")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rQNCN1", "", "readonly")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rCNVCQ1", "", "readonly")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rHopDong1", "", "readonly")%></td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;Trong đó:- Phụ cấp lương
                        </td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rSiQuan2", "", "readonly")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rQNCN2", "", "readonly")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rCNVCQ2", "", "readonly")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rHopDong2", "", "readonly")%></td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <b>&nbsp;&nbsp;2. Trừ tiền lương của những ngày nghỉ không hưởng lương</b>
                        </td>                        
                    </tr>
                    <tr>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;Trong đó:- Lương ngạch bậc
                        </td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rSiQuan3", "", "readonly")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rQNCN3", "", "readonly")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rCNVCQ3", "", "readonly")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rHopDong3", "", "readonly")%></td>
                    </tr>
                    
                    <tr>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;Trong đó:- Phụ cấp lương
                        </td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rSiQuan4", "", "readonly")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rQNCN4", "", "readonly")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rCNVCQ4", "", "readonly")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rHopDong4", "", "readonly")%></td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <b>&nbsp;&nbsp;3. Tiền lương xin quyết toán tháng này</b>
                        </td>                        
                    </tr>
                    <tr>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;Trong đó:- Lương ngạch bậc
                        </td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rSiQuan5", "", "readonly")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rQNCN5", "", "readonly")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rCNVCQ5", "", "readonly")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rHopDong5", "", "readonly")%></td>
                    </tr>
                    
                    <tr>
                        <td>
                           &nbsp;&nbsp;&nbsp;&nbsp;Trong đó:- Phụ cấp lương
                        </td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rSiQuan6", "", "readonly")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rQNCN6", "", "readonly")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rCNVCQ6", "", "readonly")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rHopDong6", "", "readonly")%></td>
                    </tr>                    
                </table>
                <table cellpadding="1" cellspacing="1" border="0" class="table_form2" width="100%">
                    <tr><td colspan="2"><b>II. Quân số phải cung cấp tiền ăn</b></td></tr>
                    <tr>
                        <td style="width:35%;"><b>&nbsp;&nbsp;1. Số ngày ăn phải cung cấp tính theo quân số lĩnh phụ cấp</b></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "iSoNgayAn_1", "", "tab-index=0")%></td>
                    </tr>
                    <tr>
                        <td style="width:35%;"><b>&nbsp;&nbsp;2. Cộng số ngày ăn phải cung cấp</b></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "iSoNgayAn_2", "", "tab-index=1")%></td>
                    </tr>
                    <tr>
                        <td style="width:35%;"><b>&nbsp;&nbsp;3. Trừ số ngày ăn không phải cung cấp</b></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "iSoNgayAn_3", "", "tab-index=2")%></td>
                    </tr>                    
                    <tr>
                        <td style="width:35%;"><b>&nbsp;&nbsp;4. Số ngày ăn xin quyết toán</b></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "iSoNgayAn_4", "", "tab-index=3")%></td>
                    </tr>
                </table>
                
                <table cellpadding="1" cellspacing="1" border="0" class="table_form2" width="100%">
                    <tr>
                        <td style="width:20%;"><b>III. Ra quân trong tháng</b></td>
                        <td colspan="2" align="center"><b>Sĩ quan</b></td>
                        <td colspan="2" align="center"><b>QNCN</b></td>
                        <td colspan="2" align="center"><b>CNVCQP</b></td>
                        <td colspan="2" align="center"><b>HSQ-CS</b></td>
                    </tr>
                    <tr>
                        <td>
                            1.Xuất ngũ                          
                        </td>
                        
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "iSiQuan_320", "", "style=\"width:98%\" tab-index=4")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rSiQuan_320", "", "tab-index=5")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "iQNCN_320", "", "style=\"width:98%\" tab-index=6")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rQNCN_320", "", "tab-index=7")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "iCNVCQP_320", "", "style=\"width:98%\" tab-index=8")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rCNVCQP_320", "", "tab-index=9")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "iHSQCS_320", "", "style=\"width:98%\" tab-index=10")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rHSQCS_320", "", "tab-index=11")%></td>
                    </tr>
                    <tr>
                        <td>2.Hưu&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "iSiQuan_310", "", "style=\"width:98%\" tab-index=12")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rSiQuan_310", "", "tab-index=13")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "iQNCN_310", "", "style=\"width:98%\" tab-index=14")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rQNCN_310", "", "tab-index=15")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "iCNVCQP_310", "", "style=\"width:98%\" tab-index=16")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rCNVCQP_310", "", "tab-index=17")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "iHSQCS_310", "", "style=\"width:98%\" tab-index=18")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rHSQCS_310", "", "tab-index=19")%></td>
                    </tr>
                    <tr>
                        <td>3.Thôi việc&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "iSiQuan_330", "", "style=\"width:98%\" tab-index=20")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rSiQuan_330", "", "tab-index=21")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "iQNCN_330", "", "style=\"width:98%\" tab-index=22")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rQNCN_330", "", "tab-index=23")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "iCNVCQP_330", "", "style=\"width:98%\" tab-index=24")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rCNVCQP_330", "", "tab-index=25")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "iHSQCS_330", "", "style=\"width:98%\" tab-index=26")%></td>
                        <td><%=MyHtmlHelper.TextBox(ParentID, data, "rHSQCS_330", "", "tab-index=27")%></td>
                    </tr>
                </table>
            
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td width="65%" class="td_form2_td5">
                            &nbsp;
                        </td>
                        <td width="30%" align="right" class="td_form2_td5">
                            <button class="button" onclick="parent.BangDuLieu_Dialog_ChiTiet_btnOK_Click('<%=ParentID %>');">Lưu</button>
                        </td>
                        <td width="5px">
                            &nbsp;
                        </td>
                        <td class="td_form2_td5">
                            <input type="button" class="button" onclick="parent.jsGTST_Dialog_close_Reload();" value="Quay lại"/>                                            
                        </td>
                    </tr>
                </table>                           
            </div>
        </div>
    </div>
</div>
</form>
</asp:Content>
