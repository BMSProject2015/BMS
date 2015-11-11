<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

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
        String MaChungTu = Convert.ToString(ViewData["iID_Vay"]);
        String MaPhongBanNguoiDung = NganSach_HamChungModels.MaPhongBanCuaMaND(UserID);
        DataTable dtChungTu = VayNoModels.GetChungTu(MaChungTu);
        DataRow R;
        String sSoChungTu = "", dNgayChungTu = "", sNoiDung = "", sLyDo = "", iID_MaTrangThaiDuyet = "";
        if (dtChungTu.Rows.Count > 0)
        {
            R = dtChungTu.Rows[0];
            dNgayChungTu = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayChungTu"]));
            sNoiDung = Convert.ToString(R["sNoiDung"]);
            sSoChungTu = Convert.ToString(R["sSoChungTu"]);
            sLyDo = Convert.ToString(R["sLyDoDuyet"]);
            iID_MaTrangThaiDuyet = Convert.ToString(R["iID_MaTrangThaiDuyet"]);
        }
        else
        {
            dNgayChungTu = CommonFunction.LayXauNgay(DateTime.Now);
        }
        //nam lam viec        
        string ThangHienTai = DanhMucModels.ThangLamViec(UserID).ToString();
        string NamHienTai = DanhMucModels.NamLamViec(UserID).ToString();
        DataTable dtNam = HamChung.getYear(DateTime.Now, false, "");
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        if (dtNam != null) dtNam.Dispose();

        //tháng
        DataTable dtThang = HamChung.getMonth(DateTime.Now, false, "", "Tháng");
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        if (dtThang != null) dtThang.Dispose();
        using (Html.BeginForm("EditSubmit", "ChungTuVayNo", new { ParentID = ParentID, iID_MaChungTu = MaChungTu }))
        {
    %>
    <%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
    <%= Html.Hidden(ParentID + "_iID_Vay", MaChungTu)%>
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
                <div style="width: 50%; float: left;">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td class="td_form2_td1">
                                <div>
                                    Tháng chứng từ</div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slThang, ThangHienTai, "iThang", "", "class=\"input1_2\" ")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1">
                                <div>
                                    Năm chứng từ</div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slNam, NamHienTai, "iNam", "", "class=\"input1_2\" ")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1" style="width: 15%;">
                                <div>
                                    Số chứng từ</div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <%
                                        if (ViewData["DuLieuMoi"] == "1")
                                        {
                                    %>
                                    <%=MyHtmlHelper.TextBox(ParentID, sSoChungTu, "sSoChungTu", "", "class=\"input1_2\" tab-index='-1'")%>
                                      <%= Html.ValidationMessage(ParentID + "_" + "err_sSoChungTu")%>
                                    <%
                                        }
                                else
                                {
                                    %>
                                    <%=sSoChungTu%>
                                    <%
                                        }%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1">
                                <div>
                                    Ngày chứngtừ</div>
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
                                    Nội dung</div>
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
