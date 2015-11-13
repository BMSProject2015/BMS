<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% 
        String MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        String ParentID = "Edit_PhanHe";
        String MaTrangThaiDuyetTuChoi = "";
        String MaTrangThaiDuyetTrinhDuyet = "";
        String MaPhanHe = "";
        String MaNhomNguoiDung = "";
        String sTen = "";
        String LoaiTrangThaiDuyet = "";
        String sMauSac = "";

        DataTable dt = PhanHe_TrangThaiDuyetModel.GetRow_PhanHe_TrangThaiDuyet(MaTrangThaiDuyet);
        DataTable dtPhanHe = PhanHe_TrangThaiDuyetModel.DT_PhanHe(true, "---Chọn phân hệ---");
        DataTable dtNhomNguoiDung = PhanHe_TrangThaiDuyetModel.DT_NguoiDung(true, "--- Chọn nhóm người dùng ---");
        DataTable dtLoaiTrangThaiDuyet = PhanHe_TrangThaiDuyetModel.DT_LoaiTrangThaiDuyet(true, "---Chọn loại trạng thái duyệt---");
        SelectOptionList optPhanHe = new SelectOptionList(dtPhanHe, "iID_MaPhanHe", "sTen");
        SelectOptionList optNhomNguoiDung = new SelectOptionList(dtNhomNguoiDung, "iID_MaNhomNguoiDung", "sTen");
        SelectOptionList optLoaiTrangThaiDuyet = new SelectOptionList(dtLoaiTrangThaiDuyet, "iLoaiTrangThaiDuyet", "sTen");

        MaPhanHe = dt.Rows[0]["iID_MaPhanHe"].ToString();
        DataTable dtTuChoi = PhanHe_TrangThaiDuyetModel.DT_TrangThaiDuyet(true, "---Chọn mã trạng thái duyệt từ chối---", MaPhanHe);
        DataTable dtTrinhDuyet = PhanHe_TrangThaiDuyetModel.DT_TrangThaiDuyet(true, "---Chọn mã trạng thái duyệt trình duyệt---", MaPhanHe);
        SelectOptionList optTuChoi = new SelectOptionList(dtTuChoi, "iID_MaTrangThaiDuyet", "sTen");
        SelectOptionList optTrinhDuyet = new SelectOptionList(dtTrinhDuyet, "iID_MaTrangThaiDuyet", "sTen");
        if (dt.Rows.Count > 0)
        {
            MaTrangThaiDuyetTrinhDuyet = dt.Rows[0]["iID_MaTrangThaiDuyet_TrinhDuyet"].ToString();
            MaTrangThaiDuyetTuChoi = dt.Rows[0]["iID_MaTrangThaiDuyet_TuChoi"].ToString();
            MaNhomNguoiDung = dt.Rows[0]["iID_MaNhomNguoiDung"].ToString();
            sTen = dt.Rows[0]["sTen"].ToString();
            LoaiTrangThaiDuyet = dt.Rows[0]["iLoaiTrangThaiDuyet"].ToString();
            sMauSac = dt.Rows[0]["sMauSac"].ToString();
        }
        String strReadOnlyMa = "";
        String strIcon = ""; 
        if (ViewData["DuLieuMoi"] == "0")
        {
            strReadOnlyMa = "readonly=\"readonly\" style=\"background:#ebebeb;\"";
            strIcon = "<img src='../Content/Themes/images/tick.png' alt='' />";
        }
        using (Html.BeginForm("Edit_PhanHeSubmit", "PhanHe_TrangThaiDuyet", new { ParentID = ParentID, MaTrangThaiDuyet = MaTrangThaiDuyet }))
        {
           
    %>
    <%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
    <%= Html.Hidden(ParentID + "iID_MaTrangThaiDuyet", MaTrangThaiDuyet)%>
    <div class="box_tong">
        <div class="title_tong">
            <table border="0" cellspacing="0" cellpadding="0" width="100%">
                <tr>
                    <td>
                        <span>Chỉnh sửa mã trạng thái duyệt từ chối, mã trạng thái duyệt trình duyệt</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <table border="0" cellspacing="0" cellpadding="0" width="70%">
                <tr>
                    <td class="td_form2_td1">
                        <div>
                            Trạng thái duyệt từ chối</div>
                    </td>
                    <td class="td_form2_td5">
                        <div>
                            <%=MyHtmlHelper.DropDownList(ParentID, optTuChoi, MaTrangThaiDuyetTuChoi, "iID_MaTrangThaiDuyet_TuChoi", null, "style=\"width: 49%;\"")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1">
                        <div>
                            Trạng thái duyệt trình duyệt</div>
                    </td>
                    <td class="td_form2_td5">
                        <div>
                            <%=MyHtmlHelper.DropDownList(ParentID, optTrinhDuyet, MaTrangThaiDuyetTrinhDuyet, "iID_MaTrangThaiDuyet_TrinhDuyet", null, "style=\"width: 49%;\"")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1">
                        <div>
                            Nhóm người dùng sửa</div>
                    </td>
                    <td class="td_form2_td5">
                        <div>
                            <%=MyHtmlHelper.DropDownList(ParentID, optNhomNguoiDung, MaNhomNguoiDung, "iID_MaNhomNguoiDung", null, "style=\"width:49%;\" " + strReadOnlyMa + "")%><%=strIcon %>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1">
                        <div>
                            Tên trạng thái duyệt</div>
                    </td>
                    <td class="td_form2_td5">
                        <div>
                            <%=MyHtmlHelper.TextBox(ParentID, sTen, "sTen", "", "style=\"width:20%;\" " + strReadOnlyMa + "")%><%=strIcon %><br />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1">
                        <div>
                            Phân hệ</div>
                    </td>
                    <td class="td_form2_td5">
                        <div>
                            <%=MyHtmlHelper.DropDownList(ParentID, optPhanHe, MaPhanHe, "iID_MaPhanHe", null, "style=\"width:20%;\" " + strReadOnlyMa + "")%><%=strIcon %>
                            <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaPhanHe")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1">
                        <div>
                            Loại trạng thái duyệt</div>
                    </td>
                    <td class="td_form2_td5">
                        <div>
                            <%=MyHtmlHelper.DropDownList(ParentID, optLoaiTrangThaiDuyet, LoaiTrangThaiDuyet, "iLoaiTrangThaiDuyet", null, "style=\"width:20%;\" " + strReadOnlyMa + "")%><%=strIcon %>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1">
                        <div>
                            Màu sắc</div>
                    </td>
                    <td class="td_form2_td5">
                        <div>
                            <%=MyHtmlHelper.TextBox(ParentID, sMauSac, "sMauSac", "", "style=\"width:20%;\" " + strReadOnlyMa + "")%><%=strIcon %>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td width="70%">
                    &nbsp;
                </td>
                <td width="30%" align="right">
                    <table cellpadding="0" cellspacing="0" border="0" align="right">
                        <tr>
                            <td>
                                <input type="submit" class="button4" value="Lưu" />
                            </td>
                            <td width="5px">
                            </td>
                            <td>
                                <input type="button" class="button4" value="Hủy" onclick="javascript:history.go(-1)" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <%} %>
</asp:Content>
