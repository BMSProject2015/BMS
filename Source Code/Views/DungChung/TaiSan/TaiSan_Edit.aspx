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
        String iID_Ma = Convert.ToString(ViewData["iID_MaTaiSan"]);
        String ParentID = "Edit";
        String sKyHieu = "", sTen = "", sMoTa = "", iID_MaLoaiTaiSan = "", bPublic = "", iID_MaDanhMuc = "";

        //chi tiết tài khoản nếu trong trường hợp sửa
        DataTable dt = TaiSanModel.getChiTiet(iID_Ma);
        if (dt.Rows.Count > 0 && iID_Ma != null && iID_Ma != "")
        {
            DataRow R = dt.Rows[0];
            sKyHieu = HamChung.ConvertToString(R["sKyHieu"]);
            sTen = HamChung.ConvertToString(R["sTen"]);
            sMoTa = HamChung.ConvertToString(R["sMoTa"]);
            iID_MaLoaiTaiSan = Convert.ToString(R["iID_MaLoaiTaiSan"]);
            iID_MaDanhMuc = Convert.ToString(R["iID_MaDanhMuc"]);
            bPublic = Convert.ToString(R["bPublic"]);
        }

        //danh sách loại tài sản
        var tbl = LoaiTaiSanModels.DT_LoaiTS(false, "--- Tất cả ---");
        SelectOptionList slLoaiTS = new SelectOptionList(tbl, "iID_MaLoaiTaiSan", "TenHT");
        if (tbl != null) dt.Dispose();

        //danh mục đơn vị tính
        DataTable dtLoaiCT = DanhMucModels.DT_DanhMuc("DonViTinh", false, "--- Tất cả ---");
        SelectOptionList slDonViTinh = new SelectOptionList(dtLoaiCT, "iID_MaDanhMuc", "sTen");
        if (dtLoaiCT != null) dtLoaiCT.Dispose();
        //tìm kiếm tài sản
        using (Html.BeginForm("EditSubmit", "TaiSan", new { ParentID = ParentID, iID_MaTaiSan = iID_Ma }))
        {
    %>
    <%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
    <%= Html.Hidden(ParentID + "_iID_MaTaiSan", iID_Ma)%>
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
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "LoaiTaiSan"), "Danh sách loại tài sản")%>
                    |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "TaiSan"), "Danh sách tài sản")%>
                </div>
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
                        <span>Nhập thông tin tài sản</span>
                        <% 
                            }
                           else
                           { %>
                        <span>Sửa thông tin tài sản</span>
                        <% } %>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <table cellpadding="5" cellspacing="5" width="50%">
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Ký hiệu</b>&nbsp;<span style="color: Red;">*</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sKyHieu, "sKyHieu", "", "class=\"input1_2\" tab-index='-1'", 2)%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_sKyHieu")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Tên tài sản</b>&nbsp;<span style="color: Red;">*</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sTen, "sTen", "", "class=\"input1_2\"", 2)%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_sTen")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Loại tài sản</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slLoaiTS, iID_MaLoaiTaiSan, "iID_MaLoaiTaiSan", "", "class=\"input1_2\" ")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaLoaiTaiSan")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Đơn vị tính </b>
                            </div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slDonViTinh, iID_MaDanhMuc, "iID_MaDanhMuc", "", "class=\"input1_2\" ")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaDanhMuc")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Mô tả</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextArea(ParentID, sMoTa, "sMoTa", "", "class=\"input1_2\"")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                            <b>    Hoạt động</b></div>
                        </td>
                        <td class="td_form2_td5" align="left">
                            <div>
                                <%=MyHtmlHelper.CheckBox(ParentID, bPublic, "bPublic", "")%>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <br />
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td width="70%">
                &nbsp;
            </td>
            <td width="30%" align="right">
                <table cellpadding="0" cellspacing="0" border="0" align="right">
                    <tr>
                        <td>
                            <input type="submit" class="button" value="Lưu" />
                        </td>
                        <td width="5px">
                        </td>
                        <td>
                            <input type="button" class="button" value="Hủy" onclick="javascript:history.go(-1)" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <%
        } if (dt != null) { dt.Dispose(); };    
    %>
</asp:Content>
