<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        String ParentID = "Edit";
        String UserID = User.Identity.Name;
        String iLoai = Convert.ToString(ViewData["iLoai"]);
        String MaChungTu = Convert.ToString(ViewData["MaChungTu"]);
        String MaPhongBanNguoiDung = NganSach_HamChungModels.MaPhongBanCuaMaND(UserID);

        DataTable dtChungTu = ThuNop_ChungTuModels.GetChungTu(MaChungTu);
        DataRow R;
        int iSoChungTu = 0;
        String sTienToChungTu = "", dNgayChungTu = "", sNoiDung = "";
        if (dtChungTu.Rows.Count > 0)
        {
            R = dtChungTu.Rows[0];
            dNgayChungTu = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayChungTu"]));
            sNoiDung = Convert.ToString(R["sNoiDung"]);
            iSoChungTu = Convert.ToInt32(R["iSoChungTu"]);

            sTienToChungTu = Convert.ToString(R["sTienToChungTu"]);
        }
        else
        {
            dNgayChungTu = CommonFunction.LayXauNgay(DateTime.Now);
        }
        if (ViewData["DuLieuMoi"] == "1")
        {
            sTienToChungTu = PhanHeModels.LayTienToChungTu(ThuNopModels.iID_MaPhanHe);
            iSoChungTu = ThuNop_ChungTuModels.GetMaxChungTu() + 1;
        }
        dtChungTu.Dispose();



        DataTable dtLoaiNhapLieu = new DataTable();
        dtLoaiNhapLieu.Columns.Add("MaLoai", typeof(String));
        dtLoaiNhapLieu.Columns.Add("TenLoai", typeof(String));
        DataRow Row;
        for (int i = 1; i < 3; i++)
        {
            Row = dtLoaiNhapLieu.NewRow();
            dtLoaiNhapLieu.Rows.Add(Row);
            Row[0] = Convert.ToString(i);
            Row[1] = Convert.ToString(i);
        }
        SelectOptionList slLoaiNhapLieu = new SelectOptionList(dtLoaiNhapLieu, "MaLoai", "TenLoai");
        dtLoaiNhapLieu.Dispose();

        using (Html.BeginForm("EditSubmit", "ThuNop_ChungTu", new { ParentID = ParentID, MaChungTu = MaChungTu }))
        {
    %>
    <%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
    <%= Html.Hidden(ParentID + "_iLoai", iLoai)%>
    <%= Html.Hidden(ParentID + "_iID_MaPhongBan", MaPhongBanNguoiDung)%>
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
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "ThuNop_ChungTu", new { iLoai = 1 }), "Kế hoạch thu")%>
                    |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "ThuNop_ChungTu", new { iLoai = 2 }), "Tổng hợp thu năm")%>
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
                        <span>
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
                            %>&nbsp; &nbsp; </span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <div style="width: 60%; float: left;">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td class="td_form2_td1" style="width: 30%;">
                                <div>
                                    <b>Số chứng từ</b></div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <b>
                                        <%=sTienToChungTu%><%=iSoChungTu%></b>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1">
                                <div>
                                    <b>Ngày chứng từ</b></div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <%=MyHtmlHelper.DatePicker(ParentID, dNgayChungTu, "dNgayChungTu", "", "class=\"input1_2\"")%><br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayChungTu")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1">
                                <div>
                                    <b>Nội dung</b></div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <%=MyHtmlHelper.TextArea(ParentID, sNoiDung, "sNoiDung", "", "class=\"input1_2\" style=\"height: 100px;\"")%></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1">
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td width="65%" class="td_form2_td5">
                                                &nbsp;
                                            </td>
                                            <td width="30%" align="right" class="td_form2_td5">
                                                <input type="submit" class="button" id="Submit1" value="Lưu" />
                                            </td>
                                            <td width="5px">
                                                &nbsp;
                                            </td>
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
            </div>
        </div>
    </div>
    <%
}       
    %>
</asp:Content>
