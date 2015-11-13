<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
        String sSoHopDong = Request.QueryString["sSoHopDong"];
        String dTuNgay = Request.QueryString["dTuNgay"];
        String dDenNgay = Request.QueryString["dDenNgay"];
        String page = Request.QueryString["page"];

        String ParentID = "QLDA_HopDong";
        String NamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);

        int iDotMoi = 0;
        if (QLDA_CapPhatModels.Get_Max_Dot(NamLamViec) != "")
        {
            iDotMoi = Convert.ToInt32(QLDA_CapPhatModels.Get_Max_Dot(NamLamViec)) + 1;
        };
        Boolean bThemMoi = false;
        String iThemMoi = "";
        if (ViewData["bThemMoi"] != null)
        {
            bThemMoi = Convert.ToBoolean(ViewData["bThemMoi"]);
            if (bThemMoi)
                iThemMoi = "on";
        }

        int CurrentPage = 1;
        if (String.IsNullOrEmpty(page) == false)
        {
            CurrentPage = Convert.ToInt32(page);
        }
        String sNguoiDung = "";
        //Xác định là nhóm trợ lý phòng ban là người tạo mới trong NS_PhanHe_TrangThaiDuyet
        bool isModify = LuongCongViecModel.NguoiDungTaoMoi(PhanHeModels.iID_MaPhanHeVonDauTu, User.Identity.Name);
        if (isModify) sNguoiDung = User.Identity.Name;
        DataTable dt = QLDA_HopDongModels.Get_DanhSachHopDong(sSoHopDong, dTuNgay, dDenNgay, sNguoiDung, CurrentPage, Globals.PageSize);

        double nums = QLDA_HopDongModels.Get_DanhSachHopDong_Count(sSoHopDong, dTuNgay, dDenNgay);
        int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);

        String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("List", "QLDA_HopDong", new { sSoHopDong = sSoHopDong, TuNgay = dTuNgay, DenNgay = dDenNgay, page = x }));
        String iID_MaHopDong = Convert.ToString(ViewData["iID_MaHopDong"]);
        String SQL = "SELECT Convert(varchar,dNgayHopDong,103) as NgayHopDong,Convert(varchar,dNgayLap,103) as NgayLap,* FROM QLDA_HopDong WHERE iID_MaHopDong='" + iID_MaHopDong + "'";
        DataTable dtHD = Connection.GetDataTable(SQL);
        sSoHopDong = "";
        String dNgayHopDong="", dNgayLap = "", sNoiDung = "";
        if (dtHD.Rows.Count > 0)
        {
            sSoHopDong = Convert.ToString(dtHD.Rows[0]["sSoHopDong"]);
            dNgayHopDong = Convert.ToString(dtHD.Rows[0]["NgayHopDong"]);
            dNgayLap = Convert.ToString(dtHD.Rows[0]["NgayLap"]);
            sNoiDung = Convert.ToString(dtHD.Rows[0]["sNoiDung"]);
        }
        using (Html.BeginForm("EditSubmit", "QLDA_HopDong", new { ParentID = ParentID, iID_MaHopDong = iID_MaHopDong }))
        {
    %>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td align="left" style="width: 9%;">
                <div style="padding-left: 22px; padding-bottom: 5px; text-transform: uppercase; color: #ec3237;">
                    <b>
                        <%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
                </div>
            </td>
            <td align="left">
                <div style="padding-bottom: 5px; color: #ec3237;">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>
                    |
                    <%=MyHtmlHelper.ActionLink(Url.Action("List", "QLDA_HopDong"), "Danh sách hợp đồng")%>
                </div>
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $('#pHeader').click(function () {
                $('#dvContent').slideToggle('slow');
            });
        });
        $(document).ready(function () {
            $("DIV.ContainerPanel > DIV.collapsePanelHeader > DIV.ArrowExpand").toggle(
            function () {
                $(this).parent().next("div.Content").show("slow");
                $(this).attr("class", "ArrowClose");
            },
            function () {
                $(this).parent().next("div.Content").hide("slow");
                $(this).attr("class", "ArrowExpand");
            });
        });            
    </script>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Sửa hợp đồng</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <table cellpadding="0" cellspacing="0" width="50%" class="table_form2">
                    <tr>
                        <td style="width: 50%">
                            <table cellpadding="0" cellspacing="0" border="0" width="50%" class="table_form2"
                                id="tb_DotNganSach">
                                <tr>
                                    <td class="td_form2_td1" style="width: 15%;">
                                        <div>
                                            <%=NgonNgu.LayXau("Số hợp đồng")%></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, sSoHopDong, "sSoHopDong", "", "class=\"input1_2\"")%><br />
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_sSoHopDong")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1" style="width: 15%;">
                                        <div>
                                            <%=NgonNgu.LayXau("Ngày hợp đồng")%></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DatePicker(ParentID, dNgayHopDong, "dNgayHopDong", "", "class=\"input1_2\"")%><br />
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayHopDong")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1" style="width: 15%;">
                                        <div>
                                            <%=NgonNgu.LayXau("Ngày lập")%></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DatePicker(ParentID, dNgayLap, "dNgayLap", "", "class=\"input1_2\"")%><br />
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayLap")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1" style="width: 15%;">
                                        <div>
                                            <%=NgonNgu.LayXau("Nội dung")%></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextArea(ParentID, sNoiDung, "sNoiDung", "", "class=\"input1_2\"")%><br />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1" style="width: 15%;">
                                        <div>
                                        </div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <table cellpadding="0" cellspacing="0" border="0">
                                                <tr>
                                                    <td width="65%" class="td_form2_td5">
                                                        &nbsp;
                                                    </td>
                                                    <td width="30%" align="right" class="td_form2_td5">
                                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Sửa")%>" />
                                                    </td>
                                                    <td width="5px">
                                                        &nbsp;
                                                    </td>
                                                    <td class="td_form2_td5">
                                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="history.go(-1)" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 50%">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <%} %>
</asp:Content>
