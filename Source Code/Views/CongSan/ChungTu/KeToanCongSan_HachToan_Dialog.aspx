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
    String ParentID = "Edit";
    String iID_MaChungTuChiTiet = Request.QueryString["iID_MaChungTuChiTiet"];
    String iID_MaKyHieuHachToan = KTCS_CauHinhHachToanModels.Get_MaCauHinhHachToan(iID_MaChungTuChiTiet);
    NameValueCollection data = KTCS_ChungTuChiTietModels.LayThongTin(iID_MaChungTuChiTiet);
%>
<form action="<%=Url.Action("EditSubmit", "KTCS_TinhHachToan", new {ParentID = ParentID})%>" method="post">    
<%=MyHtmlHelper.Hidden(ParentID,iID_MaChungTuChiTiet,"iID_MaChungTuChiTiet","") %>    
<%=MyHtmlHelper.Hidden(ParentID,iID_MaKyHieuHachToan,"iID_MaKyHieuHachToan","") %>    
<div style="background-color: #f0f9fe; background-repeat: repeat;">
    <div style="padding: 3px;">
        <div id="nhapform">
            <div id="form2">
                <table cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td valign="top" style="width: 60%;">
                            <div class="box_tong">
                                <div class="title_tong">
                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                        <tr>
                                            <td>
                                                <span>Thông tin chứng từ tài sản</span>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div style="padding: 3px;">
                                <table cellpadding="0" cellspacing="0" border="0" class="table_form2">
                                    <tr>
                                        <td class="td_form2_td1" style="width: 30%">
                                            <div><b>Số chứng từ ghi sổ</b></div>
                                        </td>
                                        <td class="td_form2_td5" style="width: 70%">
                                            <div><b><%=data["sSoChungTuGhiSo"]%></b></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_form2_td1">
                                            <div><b>Số chứng từ</b></div>
                                        </td>
                                        <td class="td_form2_td5">
                                            <div><b><%=data["sSoChungTuChiTiet"]%></b></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_form2_td1">
                                            <div><b>Mã tài sản</b></div>
                                        </td>
                                        <td class="td_form2_td5">
                                            <div><b><%=data["iID_MaTaiSan"]%></b></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_form2_td1">
                                            <div><b>Tên tài sản</b></div>
                                        </td>
                                        <td class="td_form2_td5">
                                            <div><b><%=data["sTenTaiSan"]%></b></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_form2_td1">
                                            <div><b>Nội dung chứng từ</b></div>
                                        </td>
                                        <td class="td_form2_td5">
                                            <div><b><%=data["sNoiDung"]%></b></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_form2_td1">
                                            <div><b>Số tiền chứng từ</b></div>
                                        </td>
                                        <td class="td_form2_td5">
                                            <div><b><%=CommonFunction.DinhDangSo(data["rSoTien"])%></b></div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                        <td valign="top" style="width: 40%">
                            <div class="box_tong">
                                <div class="title_tong">
                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                        <tr>
                                            <td>
                                                <span>Thông tin hạch toán tài khoản</span>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div style="padding: 3px;">
                                <table cellpadding="0" cellspacing="0" border="0" class="table_form2">
                                <% 
                                    DataTable dtHachToanChiTiet = KTCS_CauHinhHachToanModels.Get_dtCauHinhHachToanChiTiet(iID_MaKyHieuHachToan);
                                    String iID_MaTaiKhoan_Co;
                                    String iID_MaTaiKhoan_No;
                                    String KyHieuTruongTien;
                                    for (int i = 0; i < dtHachToanChiTiet.Rows.Count; i++)
                                    {
                                        iID_MaTaiKhoan_Co = Convert.ToString(dtHachToanChiTiet.Rows[i]["iID_MaTaiKhoan_Co"]);
                                        iID_MaTaiKhoan_No = Convert.ToString(dtHachToanChiTiet.Rows[i]["iID_MaTaiKhoan_No"]);
                                %>
                                    <tr>
                                        <td class="td_form2_td1" style="width: 30%">
                                            <div><b>
                                                Có: <%=iID_MaTaiKhoan_Co%><br />
                                                Nợ: <%=iID_MaTaiKhoan_No%>
                                            </b></div>
                                        </td>
                                        <td class="td_form2_td5" style="width: 70%">
                                            <div><b><%=dtHachToanChiTiet.Rows[i]["sGiaTri"]%></b></div>
                                        </td>
                                    </tr>
                                <%} %>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>         
                <br />       
                <table cellpadding="4" cellspacing="4" border="0" class="table_form2">                                        
                    <tr>
                        <td align="center" colspan="4">
                            <div>
                                 <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td width="48%" align="right" class="td_form2_td1">
                                            <button class="button" onclick="parent.BangDuLieu_Dialog_ChiTiet_btnOK_Click('<%=ParentID %>');">OK</button>
                                        </td>
                                        <td width="5px">&nbsp;</td>
                                        <td width="48%" class="td_form2_td1" align="left">
                                            <input type="button" class="button" onclick="parent.jsHachToan_Dialog_close();" value="Hủy"/>                                            
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <br />    
            </div>
        </div>
    </div>
</div>
</form>
</asp:Content>
