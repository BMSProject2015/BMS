<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  
<%
    String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
    if (String.IsNullOrEmpty(iID_MaChungTu)) iID_MaChungTu = Convert.ToString(ViewData["iID_MaChungTu"]);
    
NameValueCollection data = QuyetToan_ChungTuModels.LayThongTin(iID_MaChungTu);
String MaLoai = Convert.ToString(data["MaLoai"]);
String sTenDOnVi = Convert.ToString(DonViModels.Get_TenDonVi(data["iID_MaDonVi"],User.Identity.Name));
//Update lại các trường chỉ tiêu lấy từ cấp phát sang
//DataTable dtChungTuChiTiet = QuyetToan_ChungTuChiTietModels.Get_dtChungTuChiTiet(iID_MaChungTu);
//QuyetToan_ChungTuChiTietModels.Update_TruongDuToan(dtChungTuChiTiet);
//QuyetToan_ChungTuChiTietModels.Update_TruongChiTieu(dtChungTuChiTiet);
//QuyetToan_ChungTuChiTietModels.Update_TruongDaQuyetToan(dtChungTuChiTiet);
%>
  <script src="<%= Url.Content("~/Scripts/jsQuyetToan.js?id=") %><%=DateTime.Now.ToString("yyMMddHHmmss") %>"></script>
<div style="width: 100%; float: left;">
    <div style="width: 100%; float:left;">
        <div class="box_tong">
            <div class="title_tong">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td>
                            <span>Thông tin chứng từ</span>
                        </td>
                          <td align="right">
                                <span>F2:Thêm dòng</span>
                            </td>
                            <td align="right" style="width: 100px;">
                                <span>Delete: Xóa</span>
                            </td>
                            <td align="right" style="width: 140px;">
                                <span>Backspace: Sửa </span>
                            </td>
                            <td align="left">
                                <span>F10: Lưu</span>
                            </td>
                    </tr>
                </table>
            </div>
            <div id="nhapform">
                <div id="form2">
                   <%-- <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form3">
                        <tr>
                            <td class="td_form2_td1" style="width: 15%"><div><b>Số chứng từ</b></div></td>
                            <td class="td_form2_td5" style="width: 35%"><div><b><%=data["sTienToChungTu"]%><%=data["iSoChungTu"]%></b></div></td>
                            <%
                            int LoaiThoiGianQuyetToan = Convert.ToInt32(data["bLoaiThang_Quy"]);
                            switch (LoaiThoiGianQuyetToan) { 
                                case 0:
                                %>
                                    <td class="td_form2_td1" style="width: 15%"><div><b>Tháng</b></div></td>
                                    <td class="td_form2_td5" style="width: 35%"><div><b><%=data["iThang_Quy"]%></b></div></td>
                                <%
                                    break;
                                case 1:
                                    String strThoiGianQuyetToan = "";
                                    if (Convert.ToInt32(data["iThang_Quy"]) == 3)
                                    {
                                        strThoiGianQuyetToan = "I/" + Convert.ToString(data["iNamLamViec"]);
                                    }
                                    else if (Convert.ToInt32(data["iThang_Quy"]) == 6)
                                    {
                                        strThoiGianQuyetToan = "II/" + Convert.ToString(data["iNamLamViec"]);
                                    }
                                    else if (Convert.ToInt32(data["iThang_Quy"]) == 9)
                                    {
                                        strThoiGianQuyetToan = "III/" + Convert.ToString(data["iNamLamViec"]);
                                    }
                                    else if (Convert.ToInt32(data["iThang_Quy"]) == 12)
                                    {
                                        strThoiGianQuyetToan = "IV/" + Convert.ToString(data["iNamLamViec"]);
                                    }
                                %>
                                    <td class="td_form2_td1" style="width: 15%"><div><b>Quý</b></div></td>
                                    <td class="td_form2_td5" style="width: 35%"><div><b><%=strThoiGianQuyetToan%></b></div></td>
                                <%
                                    break;
                                case 2:
                                %>
                                    <td class="td_form2_td1" style="width: 15%"><div><b>Năm</b></div></td>
                                    <td class="td_form2_td5" style="width: 35%"><div><b><%=data["iThang_Quy"]%></b></div></td>
                                <%
                                    break;
                            }  
                            %>
                        </tr>
                        <tr>
                            <td class="td_form2_td1" style="width: 15%"><div><b>Loại ngân sách</b></div></td>
                            <td class="td_form2_td5" style="width: 35%"><div><b><%=data["sDSLNS"]%></b></div></td>
                            <td class="td_form2_td1" style="width: 15%" style="width: 20%">
                                <div><b>Ngày chứng từ</b></div>
                            </td>
                            <td class="td_form2_td5" style="width: 35%"><div><b>
                                <%=String.Format("{0:dd/MM/yyyy}",Convert.ToDateTime(data["dNgayChungTu"]))%></b></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1" style="width: 15%"><div><b>Nội dung chứng từ</b></div></td>
                            <td class="td_form2_td5" colspan="3"><div><b><%=HttpUtility.HtmlEncode(data["sNoiDung"])%></b></div></td>
                        </tr>
                    </table>--%>
                      <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form3">
                                        <tr>
                                            <%--<td class="td_form2_td1" style="width: 80px;">
                                                <div>
                                                    <b>Đợt ngày:</b></div>
                                            </td>
                                            <td class="td_form2_td5" style="width: 60px;">
                                                <div>
                                                    <%=String.Format("{0:dd/MM/yyyy}",Convert.ToDateTime(data["dNgayChungTu"]))%></div>
                                            </td>
                                            <td class="td_form2_td1" style="width: 70px;">
                                                <div>
                                                    <b>Loại NS:</b></div>
                                            </td>
                                            <td class="td_form2_td5" style="width: 200px;">
                                                <div>
                                                    <%=data["sMoTa"]%></div>
                                            </td>--%>
                                            <td class="td_form2_td1" style="width: 5%">
                                                <div>
                                                    <b>BQL:</b></div>
                                            </td>
                                            <td class="td_form2_td5" style="width: 5%">
                                                <div>
                                                    <%=data["sTenPhongBan"]%></div>
                                            </td>
                                            <td class="td_form2_td1" style="width: 8%">
                                                <div>
                                                    <b>Đơn vị:</b></div>
                                            </td>
                                            <td class="td_form2_td5" style="width: 15%">
                                                <div>
                                                    <%=data["iID_MaDonVi"] + "-" + sTenDOnVi%></div>
                                            </td>
                                            <td class="td_form2_td1" style="width: 5%">
                                                <div>
                                                    <b>Quý:</b></div>
                                            </td>
                                            <td class="td_form2_td5" style="width: 5%">
                                                <div>
                                                    <%=data["iThang_Quy"]%></div>
                                            </td>
                                             <td class="td_form2_td1" style="width: 10%">
                                                <div>
                                                    <b>Ngày chứng từ:</b></div>
                                            </td>
                                            <td class="td_form2_td5" style="width: 10%">
                                                <div>
                                                   <%=String.Format("{0:dd/MM/yyyy}",Convert.ToDateTime(data["dNgayChungTu"]))%></div>
                                            </td>
                                            <td class="td_form2_td1" style="width: 10%">
                                                <div>
                                                    <b>Nội dung đợt:</b></div>
                                            </td>
                                            <td class="td_form2_td5" style="width: 45%">
                                                <div>
                                                    <%=data["sNoiDung"]%></div>
                                            </td>
                                        </tr>
                                    </table>
                    <%Html.RenderPartial("~/Views/QuyetToan/ChungTuChiTiet/QuyetToan_ChungTuChiTiet_Index_DanhSach.ascx", new { ControlID = "ChiTieuChiTiet", MaND = User.Identity.Name }); %>    
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
           
            jsQuyetToan_Url_Frame = '<%=Url.Action("ChungTuChiTiet_Frame", "QuyetToan_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu,MaLoai=MaLoai})%>';
        });
    </script>
</div>    
</asp:Content>
