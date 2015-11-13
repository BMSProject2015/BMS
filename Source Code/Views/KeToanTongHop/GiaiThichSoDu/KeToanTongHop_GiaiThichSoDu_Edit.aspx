<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QuyetToan" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        String MaND = User.Identity.Name;
        DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
        string iNam = DateTime.Now.Year.ToString();
        string iThang = DateTime.Now.Month.ToString();
        if (dtCauHinh.Rows.Count > 0 && dtCauHinh != null)
        {
            iNam = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
            iThang = Convert.ToString(dtCauHinh.Rows[0]["iThangLamViec"]);
        }
        if (dtCauHinh != null) dtCauHinh.Dispose();

        String ParentID = "Edit";
        String iID_MaGiaiThich = Convert.ToString(ViewData["iID_MaGiaiThich"]);
        String iLoai = Convert.ToString(ViewData["iLoai"]);
        if (String.IsNullOrEmpty(iLoai)) iLoai = "1";
        DataTable dtThang = DanhMucModels.DT_Thang(false);
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        dtThang.Dispose();
        String optThu = "", optTamUng = "", optTra = "";
        optThu = Request.QueryString["optThu"];
        optTamUng = Request.QueryString["optTamUng"];
        optTra = Request.QueryString["optTra"];
        if (String.IsNullOrEmpty(optThu))
            optThu = "0";
        if (String.IsNullOrEmpty(optTamUng))
            optTamUng = "0";
        if (String.IsNullOrEmpty(optTra))
            optTra = "0";
        String KhoGiay = Request.QueryString["KhoGiay"];
        if (string.IsNullOrEmpty(KhoGiay)) KhoGiay = "1";
        DataTable dtKhoGiay = ReportModels.LoaiKhoGiay();
        SelectOptionList slKhoGiay = new SelectOptionList(dtKhoGiay, "MaKhoGiay", "TenKhoGiay");

        if (dtKhoGiay!=null)
        {
            dtKhoGiay.Dispose();
        }
        //Trang thai duyet
        DataTable dtTrangThai = rptQuyetToan_25_5Controller.tbTrangThai();
        String iID_MaTrangThaiDuyet = Request.QueryString["iTrangThai"];
        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
            iID_MaTrangThaiDuyet = dtTrangThai.Rows.Count > 0 ? Convert.ToString(dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"]) : Guid.Empty.ToString();
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "TenTrangThai");
        if (dtTrangThai != null)
        {
            dtTrangThai.Dispose();
        }
        using (Html.BeginForm("EditSubmit", "KeToanTongHop_GiaiThichSoDu", new { ParentID = ParentID }))
        {
    %>
    <%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
    <%= Html.Hidden(ParentID + "_Xem", 0)%>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td style="width: 10%">
                <div style="padding-left: 20px; padding-top: 5px; padding-bottom: 5px; text-transform: uppercase;
                    color: #ec3237;">
                    <b>
                        <%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
                </div>
            </td>
            <td align="left">
                <div style="padding-top: 5px; padding-bottom: 5px; color: #ec3237;">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>
                    |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "LoaiTaiKhoan"), "Tài khoản kế toán")%>
                </div>
            </td>
            <td align="right" style="padding-bottom: 5px; color: #ec3237; font-weight: bold;
                padding-right: 20px;">
                <% Html.RenderPartial("LogOnUserControl_KeToan"); %>
            </td>
        </tr>
    </table>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <% if (ViewData["DuLieuMoi"] == "1")
                           {
                        %>
                        <span>Giải thích Bảng cân đối tài khoản</span>
                        <% 
                           }
                        %>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <div style="padding: 10px; font-size: 13px;">
                    <fieldset style="padding: 2px; border: 1px dashed #cecece; border-radius: 5px; width: 50%;
                        margin-right: 3px;">
                        <legend class="p"><span style="font-size: 11pt; line-height: 16px; font-weight: bold;">
                            &nbsp;
                            <%=NgonNgu.LayXau("&nbsp;&nbsp;Nội dung các phần giải thích")%></span></legend>
                        <table cellpadding="5" cellspacing="5" border="0" width="850px">
                            <tr>
                                <td>
                                </td>
                                <td colspan="4" style="padding: 10 0 10 0; text-align: center;">
                                    <div style="margin-top: 10px; margin-bottom: 10px;">
                                        <span>(Lưu ý: Chọn yếu tố cần phân tích theo TK giải thích)</span></div>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1" width="5%">
                                    <div>
                                        <%=MyHtmlHelper.Option(ParentID, "1", iLoai, "iLoai", "")%>
                                    </div>
                                </td>
                                <td class="td_form2_td5" width="10%">
                                    <div>
                                        <b>1 - Các khoản phải thu</b></div>
                                </td>
                                <td class="td_form2_td1" width="10%">
                                    <span style="color: Red;">Nội dung</span>
                                    <%=MyHtmlHelper.Option(ParentID, "0",optThu, "optThu","")%>
                                </td>
                                <td class="td_form2_td1" width="10%">
                                    <span style="color: Red;">Đơn vị</span>
                                    <%=MyHtmlHelper.Option(ParentID, "1", optThu,"optThu","")%>
                                </td>
                                <td class="td_form2_td1" width="10%">
                                    <span style="color: Red;">Kết hợp</span>
                                    <%=MyHtmlHelper.Option(ParentID, "2",optThu, "optThu","")%>
                                </td>
                                <td width="55%">
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1">
                                    <div>
                                        <%=MyHtmlHelper.Option(ParentID, "2", iLoai, "iLoai", "")%><br />
                                    </div>
                                </td>
                                <td class="td_form2_td5">
                                    <div>
                                        <b>2 - Các khoản tạm ứng </b>
                                    </div>
                                </td>
                                <td class="td_form2_td1" width="10%">
                                    <span style="color: Red;">Nội dung</span>
                                    <%=MyHtmlHelper.Option(ParentID, "0", optTamUng, "optTamUng", "")%>
                                </td>
                                <td class="td_form2_td1" width="10%">
                                    <span style="color: Red;">Đơn vị</span>
                                    <%=MyHtmlHelper.Option(ParentID, "1", optTamUng, "optTamUng", "")%>
                                </td>
                                <td class="td_form2_td1" width="10%">
                                    <span style="color: Red;">Kết hợp</span>
                                    <%=MyHtmlHelper.Option(ParentID, "2", optTamUng, "optTamUng", "")%>
                                </td>
                                <td width="55%">
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1">
                                    <div>
                                        <%=MyHtmlHelper.Option(ParentID, "3", iLoai, "iLoai", "")%><br />
                                    </div>
                                </td>
                                <td class="td_form2_td5">
                                    <div>
                                        <b>3 - Các khoản phải trả </b>
                                    </div>
                                </td>
                                <td class="td_form2_td1" width="10%">
                                    <span style="color: Red;">Nội dung</span>
                                    <%=MyHtmlHelper.Option(ParentID, "0", optTra, "optTra", "")%>
                                </td>
                                <td class="td_form2_td1" width="10%">
                                    <span style="color: Red;">Đơn vị</span>
                                    <%=MyHtmlHelper.Option(ParentID, "1", optTra, "optTra", "")%>
                                </td>
                                <td class="td_form2_td1" width="10%">
                                    <span style="color: Red;">Kết hợp</span>
                                    <%=MyHtmlHelper.Option(ParentID, "2", optTra, "optTra", "")%>
                                </td>
                                <td width="55%">
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1">
                                    <div>
                                        <%=MyHtmlHelper.Option(ParentID, "4", iLoai, "iLoai", "","")%><br />
                                    </div>
                                </td>
                                <td class="td_form2_td5">
                                    <div>
                                        <b>4 - Giải thích đề nghị của đơn vị</b></div>
                                </td>
                                <td>
                                    <div class="td_form2_td1">
                                        <%=MyHtmlHelper.CheckBox(ParentID, "TuDong", "TuDong","", "")%></div>
                                </td>
                                <td colspan="6" class="td_form2_td5">
                                    <div>
                                        <b>Tự động nhận số liệu</b>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="8" style="text-align: right;">
                                    <div style="margin-top: 10px; margin-bottom: 10px;">
                                        <span>(Chọn ô tự động nếu nhận trực tiếp từ dữ liệu gốc, nếu không chọn nhấn nút "Nhập"
                                            để gõ vào)</span></div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td style="text-align: right;">
                                    <b><span style="padding-left: 30px;">Tháng</span></b>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang, "iThang", "", "style=\"width:100px;\"")%>
                                    <b><span style="padding-left: 5px;">Năm</span></b>
                                    <select id="ddliNam" style="width: 70px;" disabled="disabled">
                                        <option value="">
                                            <%= iNam %>
                                        </option>
                                    </select>
                                     <b><span style="padding-left: 5px;">Trạng thái</span></b>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "style=\"width: 130px\"")%>
                                    &nbsp;
                                </td>
                                <td>
                                    <b>Khổ giấy:</b>
                                </td>
                                <td colspan="2">
                                    <%=MyHtmlHelper.DropDownList(ParentID, slKhoGiay, KhoGiay, "KhoGiay", "", "class=\"input1_2\" style=\"width: 100%;\"")%>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <div style="width: 850px; padding: 10px;">
                    <table cellpadding="0" cellspacing="0" border="0" align="right">
                        <tr>
                            <td>
                                <input type="submit" class="button" id="btnSubmit" value="Nhập" />
                            </td>
                            <td width="5px">
                            </td>
                            <td>
                                <input type="button" class="button" value="Xem" onclick="javascript:Xem()" />
                            </td>
                            <td width="5px">
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function Xem() {
            document.getElementById('<%=ParentID%>_Xem').value = 1;
            document.getElementById('btnSubmit').click();
        }
    </script>
    <%
        }    
    %>
</asp:Content>
