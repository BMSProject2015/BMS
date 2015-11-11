<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

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
        int i;
        String ParentID = "Index";
        String page = Request.QueryString["page"];
        String MaND = User.Identity.Name;
        int CurrentPage = 1;
        SqlCommand cmd;

        if (String.IsNullOrEmpty(page) == false)
        {
            CurrentPage = Convert.ToInt32(page);
        }

        String MaPhanHe = Convert.ToString(ViewData["iID_MaPhanHe"]);
        DataTable dtPhanHe = PhanHe_TrangThaiDuyetModel.DT_PhanHe(false, "");
        if (String.IsNullOrEmpty(MaPhanHe))
            MaPhanHe = System.Web.Configuration.WebConfigurationManager.AppSettings["MaPhanHe"];
        //  MaPhanHe = Convert.ToString(dtPhanHe.Rows[0]["iID_MaPhanHe"]);
        SelectOptionList optPhanHe = new SelectOptionList(dtPhanHe, "iID_MaPhanHe", "sTen");
        dtPhanHe.Dispose();

        DataRow R;
        DataTable dtChuKy = DanhMucChuKyModels.Get_dtDanhMucChuKy();
        R = dtChuKy.NewRow();
        R["iID_MaChuKy"] = 0;
        R["TenHienThi"] = "----";
        dtChuKy.Rows.InsertAt(R, 0);

        SelectOptionList slChuKy = new SelectOptionList(dtChuKy, "iID_MaChuKy", "TenHienThi");
        dtChuKy.Dispose();


        DataTable dt = DanhMuc_BaoCao_ChuKyModels.Get_dtDanhMucBaoCaoChuKyTheoPhanHe(MaPhanHe, MaND);
        DataTable dtTK = DanhMuc_BaoCao_ChuKyModels.Get_TaiKhoan(MaPhanHe);
        SelectOptionList slTK = new SelectOptionList(dtTK, "sID_MaNguoiDungTao", "sID_MaNguoiDungTao");
        String strThemMoi = Url.Action("Edit", "DanhMuc_BaoCao_ChuKy", new { iID_MaPhanHe = MaPhanHe });

        using (Html.BeginForm("Loc", "DanhMuc_BaoCao_ChuKy", new { ParentID = ParentID }))
        {
    %>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td align="left" style="width: 12%;">
                <div style="padding-left: 22px; padding-bottom: 5px; text-transform: uppercase; color: #ec3237;">
                    <b>
                        <%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
                </div>
            </td>
            <td align="left">
                <div style="padding-bottom: 5px; color: #ec3237;">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>
                    |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "KeToanTongHop"), "Danh sách chứng từ ghi sổ")%>
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
            <table border="0" cellspacing="0" cellpadding="0" width="100%">
                <tr>
                    <td>
                        <span>Danh sách</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <table border="0" cellspacing="0" cellpadding="0" width="70%">
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                Phân hệ</div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, optPhanHe, MaPhanHe, "iID_MaPhanHe", null, "style=\"width: 100%;\"")%></div>
                        </td>
                        <td>
                            <div style="padding: 3px 10px; float:left;">
                                <input type="submit" value="Lọc" class="button" />
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <%} %>
    <br />
    <%if (dt.Rows.Count == 0)
      { %>
    <%using (Html.BeginForm("CopyTuTaiKhoanKhac", "DanhMuc_BaoCao_ChuKy", new { ParentID = ParentID }))
      { %>
    <table cellpadding="0" cellspacing="0" border="0" align="left">
        <tr>
            <td>
                Copy từ tài khoản
            </td>
            <td>
                &nbsp;&nbsp;
            </td>
            <td>
                <%=MyHtmlHelper.DropDownList(ParentID, slTK, "", "sID_MaNguoiDungTao", "", "style=\"width:150px;\"")%>
                <%=MyHtmlHelper.Hidden(ParentID,MaPhanHe, "iID_MaPhanHe", "")%>
            </td>
            <td>
                &nbsp;&nbsp;
            </td>
            <td>
                <input type="submit" class="button4" value="Lưu" />
            </td>
        </tr>
    </table>
    <%} %>
    <%} %>
    <div style="float: left; width: 100%; padding-bottom: 5px;">
        <input id="Button1" style="float: right;" type="button" class="button_title" value="Thêm mới"
            onclick="javascript:location.href='<%=strThemMoi %>'" />
    </div>
    <br />
    <% using (Html.BeginForm("EditSubmit", "DanhMuc_BaoCao_ChuKy", new { ParentID = ParentID, iID_MaPhanHe = MaPhanHe }))
       { %>
    <div class="box_tong1">
        <div class="title_tong1">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>&nbsp;</span>
                    </td>
                </tr>
            </table>
        </div>
        <table class="mGrid">
            <tr>
                <th style="width: 3%;" align="center">
                    STT
                </th>
                <th style="width: 10%;" align="center">
                    Báo cáo
                </th>
                <th style="width: 5%;" align="center">
                    Tên vị trí
                </th>
                <th style="width: 17%;" align="center">
                    Vị trí 1
                </th>
                <th style="width: 17%;" align="center">
                    Vị trí 2
                </th>
                <th style="width: 17%;" align="center">
                    Vị trí 3
                </th>
                <th style="width: 17%;" align="center">
                    Vị trí 4
                </th>
                <th style="width: 17%;" align="center">
                    Vị trí 5
                </th>
            </tr>
            <%
           for (i = 0; i < dt.Rows.Count; i++)
           {
               R = dt.Rows[i];
               String MaBaoCao_ChuKy = Convert.ToString(R["iID_MaBaoCao_ChuKy"]);
               String iID_MaPhanHe = Convert.ToString(R["iID_MaPhanHe"]);
               String urlEdit = Url.Action("Edit", "DanhMuc_BaoCao_ChuKy", new { iID_MaBaoCao_ChuKy = MaBaoCao_ChuKy, iID_MaPhanHe = iID_MaPhanHe });
               String classtr = "";
               int STT = i + 1;
               if (i % 2 == 0)
               {
                   classtr = "class=\"alt\"";
               }
            %>
            <tr <%=classtr %>>
                <td align="center" rowspan="3">
                    <%=STT%>
                </td>
                <td align="left" rowspan="3">
                    <%=MyHtmlHelper.ActionLink(urlEdit,HttpUtility.HtmlEncode(R["sTenBaoCao"])) %>
                    <%=MyHtmlHelper.Hidden(ParentID,MaBaoCao_ChuKy,"iID_MaBaoCao_ChuKy","") %>
                </td>
                <td>
                    Thừa lệnh
                </td>
                <td>
                    <%=MyHtmlHelper.DropDownList(ParentID + "_" + MaBaoCao_ChuKy, slChuKy, Convert.ToString(R["iID_MaThuaLenh1"]), "iID_MaThuaLenh1", "", "style=\"width:98%\"")%>
                </td>
                <td>
                    <%=MyHtmlHelper.DropDownList(ParentID + "_" + MaBaoCao_ChuKy, slChuKy, Convert.ToString(R["iID_MaThuaLenh2"]), "iID_MaThuaLenh2", "", "style=\"width:98%\"")%>
                </td>
                <td>
                    <%=MyHtmlHelper.DropDownList(ParentID + "_" + MaBaoCao_ChuKy, slChuKy, Convert.ToString(R["iID_MaThuaLenh3"]), "iID_MaThuaLenh3", "", "style=\"width:98%\"")%>
                </td>
                <td>
                    <%=MyHtmlHelper.DropDownList(ParentID + "_" + MaBaoCao_ChuKy, slChuKy, Convert.ToString(R["iID_MaThuaLenh4"]), "iID_MaThuaLenh4", "", "style=\"width:98%\"")%>
                </td>
                <td>
                    <%=MyHtmlHelper.DropDownList(ParentID + "_" + MaBaoCao_ChuKy, slChuKy, Convert.ToString(R["iID_MaThuaLenh5"]), "iID_MaThuaLenh5", "", "style=\"width:98%\"")%>
                </td>
            </tr>
            <tr <%=classtr %>>
                <td>
                    Chức danh
                </td>
                <td>
                    <%=MyHtmlHelper.DropDownList(ParentID + "_" + MaBaoCao_ChuKy, slChuKy, Convert.ToString(R["iID_MaChucDanh1"]), "iID_MaChucDanh1", "","style=\"width:98%\"") %>
                </td>
                <td>
                    <%=MyHtmlHelper.DropDownList(ParentID + "_" + MaBaoCao_ChuKy, slChuKy, Convert.ToString(R["iID_MaChucDanh2"]), "iID_MaChucDanh2", "", "style=\"width:98%\"")%>
                </td>
                <td>
                    <%=MyHtmlHelper.DropDownList(ParentID + "_" + MaBaoCao_ChuKy, slChuKy, Convert.ToString(R["iID_MaChucDanh3"]), "iID_MaChucDanh3", "", "style=\"width:98%\"")%>
                </td>
                <td>
                    <%=MyHtmlHelper.DropDownList(ParentID + "_" + MaBaoCao_ChuKy, slChuKy, Convert.ToString(R["iID_MaChucDanh4"]), "iID_MaChucDanh4", "", "style=\"width:98%\"")%>
                </td>
                <td>
                    <%=MyHtmlHelper.DropDownList(ParentID + "_" + MaBaoCao_ChuKy, slChuKy, Convert.ToString(R["iID_MaChucDanh5"]), "iID_MaChucDanh5", "", "style=\"width:98%\"")%>
                </td>
            </tr>
            <tr <%=classtr %>>
                <td>
                    Tên
                </td>
                <td>
                    <%=MyHtmlHelper.DropDownList(ParentID + "_" + MaBaoCao_ChuKy, slChuKy, Convert.ToString(R["iID_MaTen1"]), "iID_MaTen1", "", "style=\"width:98%\"")%>
                </td>
                <td>
                    <%=MyHtmlHelper.DropDownList(ParentID + "_" + MaBaoCao_ChuKy, slChuKy, Convert.ToString(R["iID_MaTen2"]), "iID_MaTen2", "", "style=\"width:98%\"")%>
                </td>
                <td>
                    <%=MyHtmlHelper.DropDownList(ParentID + "_" + MaBaoCao_ChuKy, slChuKy, Convert.ToString(R["iID_MaTen3"]), "iID_MaTen3", "", "style=\"width:98%\"")%>
                </td>
                <td>
                    <%=MyHtmlHelper.DropDownList(ParentID + "_" + MaBaoCao_ChuKy, slChuKy, Convert.ToString(R["iID_MaTen4"]), "iID_MaTen4", "", "style=\"width:98%\"")%>
                </td>
                <td>
                    <%=MyHtmlHelper.DropDownList(ParentID + "_" + MaBaoCao_ChuKy, slChuKy, Convert.ToString(R["iID_MaTen5"]), "iID_MaTen5", "", "style=\"width:98%\"")%>
                </td>
            </tr>
            <%} %>
        </table>
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
    <%} %>
    <%dt.Dispose();    
    %>
</asp:Content>
