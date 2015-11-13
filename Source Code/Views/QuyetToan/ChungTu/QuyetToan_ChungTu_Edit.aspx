<%@ Page Title="" Language="C#" validateRequest="false" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Controllers.QuyetToan" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    String ParentID = "Edit";
    String UserID = User.Identity.Name;
    String Loai = Convert.ToString(Request.QueryString["Loai"]);
    if (String.IsNullOrEmpty(Loai)) Loai = Convert.ToString(ViewData["Loai"]);
    String MaChungTu = Convert.ToString(ViewData["MaChungTu"]);
    String MaPhongBanNguoiDung = NganSach_HamChungModels.MaPhongBanCuaMaND(UserID);

    DataTable dtChungTu = QuyetToan_ChungTuModels.GetChungTu(MaChungTu);
    DataRow R;
    int iSoChungTu = 0;
    String sLNS = "", dNgayChungTu = "", sNoiDung = "", sLyDo = "", iID_MaTrangThaiDuyet = "", sThang = "", iID_MaDonVi = "", sTienToChungTu="";
    String ReadOnly = "";
    if (dtChungTu.Rows.Count > 0)
    {
        R = dtChungTu.Rows[0];
        dNgayChungTu = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayChungTu"]));
        sNoiDung = Convert.ToString(R["sNoiDung"]);
        iSoChungTu = Convert.ToInt32(R["iSoChungTu"]);
        sLyDo = Convert.ToString(R["sLyDo"]);
        iID_MaTrangThaiDuyet = Convert.ToString(R["iID_MaTrangThaiDuyet"]);
        sThang = Convert.ToString(R["iThang_Quy"]);
        iID_MaDonVi = Convert.ToString(R["iID_MaDonVi"]);
        sTienToChungTu = Convert.ToString(R["sTienToChungTu"]);
        sLNS = Convert.ToString(R["sDSLNS"]);
        ReadOnly = "disabled=\"disabled\"";
    }
    else
    {
        if (String.IsNullOrEmpty(Convert.ToString(ViewData["iThang"])))
        {
            sThang = Convert.ToString(DateTime.Now.Month);
            if (sThang == "1" || sThang == "2" || sThang == "3")
                sThang = "1";
            else if (sThang == "4" || sThang == "4" || sThang == "6")
                sThang = "2";
            else if (sThang == "7" || sThang == "8" || sThang == "9")
                sThang = "3";
            else
                sThang = "4";
        }
        else
        {
            sThang = Convert.ToString(ViewData["iThang"]);
        }

        if(String.IsNullOrEmpty(Convert.ToString(ViewData["NgayChungTu"])))
            dNgayChungTu = CommonFunction.LayXauNgay(DateTime.Now);
        else
            dNgayChungTu = Convert.ToString(ViewData["NgayChungTu"]);
        sLNS = Convert.ToString(ViewData["sLNS"]);
        iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        sNoiDung = Convert.ToString(ViewData["sNoiDung"]);
    }
    if (ViewData["DuLieuMoi"] == "1")
    {
        sTienToChungTu = PhanHeModels.LayTienToChungTu(QuyetToanModels.iID_MaPhanHeQuyetToan);
        iSoChungTu = QuyetToan_ChungTuModels.GetMaxChungTu() + 1;
    }
   
    String iID_MaPhongBan = NganSach_HamChungModels.MaPhongBanCuaMaND(UserID);
    DataTable dtLNS = DanhMucModels.NS_LoaiNganSachQuocPhong(iID_MaPhongBan);
    SelectOptionList slLNS = new SelectOptionList(dtLNS, "sLNS", "TenHT");
    dtLNS.Dispose();
    dtChungTu.Dispose();

    DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(UserID);
    SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "TenHT");
    DataRow R1 = dtDonVi.NewRow();
    R1["iID_MaDonVi"] = "";
    R1["TenHT"] = "--- Chọn đơn vị ---";
    dtDonVi.Rows.InsertAt(R1, 0);
    dtDonVi.Dispose();

    DataTable dtThang = DanhMucModels.DT_Quy();
    R1 = dtThang.NewRow();
    R1["MaQuy"] = "5";
    R1["TenQuy"] = "Bổ sung";
    dtThang.Rows.Add(R1);
    SelectOptionList slThang = new SelectOptionList(dtThang, "MaQuy", "TenQuy");
    dtThang.Dispose();
   
    using (Html.BeginForm("EditSubmit", "QuyetToan_ChungTu", new { ParentID = ParentID, MaChungTu = MaChungTu, Loai = Loai }))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td align="left" style="width: 9%;">
            <div style="padding-left: 22px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                <b><%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
            </div>         
        </td>
        <td align="left">
            <div style="padding-bottom: 5px; color:#ec3237;">
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%> |
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "QuyetToan_ChungTu", new { Loai = Loai}), "Chứng từ quyết toán")%>
            </div>
        </td>
    </tr>
</table>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td><span>
                    <%
                    if (ViewData["DuLieuMoi"] == "1")
                    {
                        %>
                        <%=NgonNgu.LayXau("Thêm mới chứng từ")%>
                        <%
                    }
                    else
                    {
                        %>
                        <%=NgonNgu.LayXau("Sửa thông tin chứng từ")%>
                        <%
                    }
                    %>&nbsp; &nbsp;
                </span></td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <div style="width: 100%; float: left;">
                <div style="width: 60%; float: left;">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td class="td_form2_td1" style="width: 30%;">
                                <div><b>Số chứng từ</b></div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <b><%=sTienToChungTu%><%=iSoChungTu%></b>
                                </div>
                            </td>
                        </tr>
                     
                        <tr>
                            <td class="td_form2_td1">
                                <div><b>Đơn vị</b></div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "onchange=\"ChonDonViNhapLieu(this.value)\" class=\"input1_2\"")%><br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaDonVi")%>
                               <%--     <script language="javascript" type="text/javascript">
                                        function ChonDonViNhapLieu(jGiaTriDonVi) {
                                            var objThangQuy = document.getElementById("<%= ParentID %>_iThang_Quy");
                                            var jThangQuy = '';
                                            if (objThangQuy != null) {
                                                jThangQuy = objThangQuy.options[objThangQuy.selectedIndex].value;
                                            }
                                            ThongKeGiaTriQuyetToan('1010000', jGiaTriDonVi, jThangQuy, '0')
                                        }    
                                    </script>--%>
                                </div>
                            </td>
                        </tr>
                        <%if (Loai == "1")
                          { %>
                           <tr>
                            <td class="td_form2_td1">
                                <div><b>Loại ngân sách</b></div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <%= MyHtmlHelper.DropDownList(ParentID, slLNS, sLNS, "sLNS", "",
                                                                            String.Format(
                                                                                "{0} onchange=\"ChonDonViNhapLieu(this.value)\" class=\"input1_2\"",
                                                                                ReadOnly)) %><br />
                                </div>
                            </td>
                        </tr>
                        <% } %>
                        <tr>
                            <td class="td_form2_td1">
                                <div><b>Quý</b></div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slThang, sThang, "iThang_Quy", "", "onchange=\"ChonThangNhapLieu(this.value)\" class=\"input1_2\" style=\"width:17%;\"")%><br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_iThang_Quy")%>
                                  <%--  <script language="javascript" type="text/javascript">
                                        function ChonThangNhapLieu(jGiaTriThang) {
                                            var objDonVi = document.getElementById("<%= ParentID %>_iID_MaDonVi");
                                            var jGiaTriDonVi = '';
                                            if (objDonVi != null) {
                                                jGiaTriDonVi = objDonVi.options[objDonVi.selectedIndex].value;
                                            }
                                            ThongKeGiaTriQuyetToan('1010000', jGiaTriDonVi, jGiaTriThang, '0')
                                        }    
                                    </script>--%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1">
                                <div><b>Ngày chứng từ</b></div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <%=MyHtmlHelper.DatePicker(ParentID, dNgayChungTu, "dNgayChungTu", "", "class=\"input1_2\" onblur=isDate(this);")%><br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayChungTu")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1">
                                <div><b>Nội dung</b></div>
                            </td>
                            <td class="td_form2_td5">
                                <div><%=MyHtmlHelper.TextArea(ParentID,sNoiDung, "sNoiDung", "", "class=\"input1_2\" style=\"height: 80px;\"")%></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1"></td>
                            <td class="td_form2_td5">
                                <div>
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td width="65%" class="td_form2_td5">&nbsp;</td>   
                                            <td width="30%" align="right" class="td_form2_td5">
                                                <input type="submit" class="button" id="Submit1" value="Lưu" />
                                            </td>          
                                                <td width="5px">&nbsp;</td>          
                                            <td class="td_form2_td5">
                                                <input class="button" type="button" value="Hủy" onclick="history.go(-1)" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
              <%--  <div style="width: 36%; float: right;">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td style="height: 50px;">&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="td_form2_td5">
                                <div>
                                    <fieldset style="width: 100%;">
                                        <legend><b>Tổng số quyết toán tự chi</b></legend>
                                        <table class="tblhost-filter">
                                            <tr>
                                                <td style="width: 30%;">
                                                    Số quyết toán trong kỳ:
                                                </td>
                                                <td style="width: 2%;">:</td>
                                                <td style="text-align: left; font-size: 13px; color: Red;" id="rSoQuyetToanTrongKy_TuChi">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Số quyết toán đến kỳ:
                                                </td>
                                                <td>:</td>
                                                <td style="text-align: left; font-size: 13px; color: Red;" id="rSoQuyetToanDenKy_TuChi">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Tổng số quyết toán:
                                                </td>
                                                <td>:</td>
                                                <td style="text-align: left; font-size: 13px; color: Red;" id="rTongSoQuyetToan_TuChi">
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td5">
                                <div>
                                    <fieldset style="width: 100%;">
                                        <legend><b>Tổng số quyết toán hiện vật</b></legend>
                                        <table class="tblhost-filter">
                                            <tr>
                                                <td style="width: 30%;">
                                                    Số quyết toán trong kỳ:
                                                </td>
                                                <td style="width: 2%;">:</td>
                                                <td style="text-align: left; font-size: 13px; color: Red;" id="rSoQuyetToanTrongKy_HienVat">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Số quyết toán đến kỳ:
                                                </td>
                                                <td>:</td>
                                                <td style="text-align: left; font-size: 13px; color: Red;" id="rSoQuyetToanDenKy_HienVat">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Tổng số quyết toán:
                                                </td>
                                                <td>:</td>
                                                <td style="text-align: left; font-size: 13px; color: Red;" id="rTongSoQuyetToan_HienVat">
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div> --%>               
            </div>
        </div>
    </div>
</div>
<%
    }       
%>
<script type="text/javascript">
    ThongKeGiaTriQuyetToan('1010000','<%=iID_MaDonVi %>', '<%=sThang %>', '0');
    function ThongKeGiaTriQuyetToan(jLNS, jMaDonVi, jThangQuy, jLoaiThangQuy) {        
        jQuery.ajaxSetup({ cache: false });
        var url = unescape('<%= Url.Action("get_TongGiaTriQuyetToan?sLNS=#0&iID_MaDonVi=#1&Thang_Quy=#2&LoaiThang_Quy=#3", "QuyetToan_ChungTu") %>');
        url = unescape(url.replace("#0", jLNS));
        url = unescape(url.replace("#1", jMaDonVi));
        url = unescape(url.replace("#2", jThangQuy));
        url = unescape(url.replace("#3", jLoaiThangQuy));
        $.getJSON(url, function (item) {
            document.getElementById("rSoQuyetToanTrongKy_TuChi").innerHTML = item.rSoQuyetToanTrongKy_TuChi;
            document.getElementById("rSoQuyetToanDenKy_TuChi").innerHTML = item.rSoQuyetToanDenKy_TuChi;
            document.getElementById("rTongSoQuyetToan_TuChi").innerHTML = item.rTongSoQuyetToan_TuChi;
            document.getElementById("rSoQuyetToanTrongKy_HienVat").innerHTML = item.rSoQuyetToanTrongKy_HienVat;
            document.getElementById("rSoQuyetToanDenKy_HienVat").innerHTML = item.rSoQuyetToanDenKy_HienVat;
            document.getElementById("rTongSoQuyetToan_HienVat").innerHTML = item.rTongSoQuyetToan_HienVat;
        });
    }                                            
</script>
</asp:Content>


