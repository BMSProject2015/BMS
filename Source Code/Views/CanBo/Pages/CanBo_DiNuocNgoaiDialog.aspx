<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<%@ Import Namespace="VIETTEL.Models" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=ConfigurationManager.AppSettings["TitleView"]%></title>
</head>
<body>
    <%
        String MaDiv = Request.QueryString["idDiv"];
        String MaDivDate = Request.QueryString["idDivDate"];
        String iID_MaCanBo = Request.QueryString["iID_MaCanBo"];
        String iID_MaQuaTrinhCongTac = "";
        if (Request.QueryString["iID_MaDiNuocNgoai"] != null) iID_MaQuaTrinhCongTac = Request.QueryString["iID_MaDiNuocNgoai"];
        String OnSuccess = "", DuLieuMoi = "";
        OnSuccess = Request.QueryString["OnSuccess"];
        String ParentID = "Create";
        if (String.IsNullOrEmpty(iID_MaQuaTrinhCongTac) == false && iID_MaQuaTrinhCongTac != "") DuLieuMoi = "0";
        else DuLieuMoi = "1";


        //Quốc gia
        var dtQuocGia = DanhMucModels.DT_DanhMuc("CanBo_QuocGia", true, "--- Lựa chọn ---");
        SelectOptionList slMaQuocGia = new SelectOptionList(dtQuocGia, "iID_MaDanhMuc", "sTen");
        if (dtQuocGia != null) dtQuocGia.Dispose();
        //Quốc gia
        var dtNguon = DanhMucModels.DT_DanhMuc("CanBo_NguonKinhPhi", true, "--- Lựa chọn ---");
        SelectOptionList slMaNguonKinhPhi = new SelectOptionList(dtNguon, "iID_MaDanhMuc", "sTen");
        if (dtNguon != null) dtNguon.Dispose();
        String sLyDoDi = "", iID_MaNuoc = "", rKinhPhi = "", dTuNgay = "", dDenNgay = "", dNgayKy = "", sNguoiKy = "", dNgayHieuLuc = "", iID_MaNguonKinhPhi = "", sSoQD = "",
            sMoTa = "";


        //lấy chi tiết
        if (String.IsNullOrEmpty(iID_MaQuaTrinhCongTac) == false && iID_MaQuaTrinhCongTac != "")
        {
            var dt = CanBo_DanhMucNhanSuModels.GetChiTiet_DiNuocNgoai(iID_MaQuaTrinhCongTac);
            if (dt.Rows.Count > 0)
            {
                DataRow DR = dt.Rows[0];
                sLyDoDi = HamChung.ConvertToString(DR["sLyDoDi"]);
                iID_MaNuoc = HamChung.ConvertToString(DR["iID_MaNuoc"]);

                rKinhPhi = HamChung.ConvertToString(DR["rKinhPhi"]);
                iID_MaNguonKinhPhi = HamChung.ConvertToString(DR["iID_MaNguonKinhPhi"]);
                sSoQD = HamChung.ConvertToString(DR["sSoQD"]);
                String TuNgay = CommonFunction.LayXauNgay(HamChung.ConvertDateTime(DR["dTuNgay"]));
                if (TuNgay != "01/01/0001") dTuNgay = TuNgay;
                String DenNgay = CommonFunction.LayXauNgay(HamChung.ConvertDateTime(DR["dDenNgay"]));
                if (DenNgay != "01/01/0001") dDenNgay = DenNgay;

                sNguoiKy = HamChung.ConvertToString(DR["sNguoiKy"]);
                sMoTa = HamChung.ConvertToString(DR["sMoTa"]);
                String NgayHieuLuc = CommonFunction.LayXauNgay(HamChung.ConvertDateTime(DR["dNgayHieuLuc"]));
                if (NgayHieuLuc != "01/01/0001") dNgayHieuLuc = NgayHieuLuc;
                String NgayKy = CommonFunction.LayXauNgay(HamChung.ConvertDateTime(DR["dNgayKy"]));
                if (NgayKy != "01/01/0001") dNgayKy = NgayKy;

            }
        }


        using (Ajax.BeginForm("Edit_Fast_DiNuocNgoai_Submit", "CanBo_DiNuocNgoai", new { ParentID = ParentID, OnSuccess = OnSuccess, iID_MaCanBo = iID_MaCanBo, iID_MaDiNuocNgoai = iID_MaQuaTrinhCongTac }, new AjaxOptions { }))
        {
    %>
    <%= Html.Hidden(ParentID + "_DuLieuMoi", DuLieuMoi)%>
    <div style="background-color: #ffffff; background-repeat: repeat">
        <div style="padding: 5px 1px 10px 1px;">
            <div class="box_tong">
                <div id="form2" style="padding: 10px 10px">
                    <table border="0" cellpadding="10" cellspacing="10" width="100%">
                        <tr>
                            <td class="td_form2_td10" style="width: 15%">
                                <div>
                                    Từ ngày&nbsp;<span style="color: Red;">*</span></div>
                            </td>
                            <td style="width: 20%">
                                <div>
                                    <%=MyHtmlHelper.DatePicker(ParentID, dTuNgay, "dTuNgay", "", "class=\"input1_2\"")%>
                                    <br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_dTuNgay")%>
                                </div>
                            </td>
                            <td class="td_form2_td10" style="width: 15%">
                                <div>
                                    Đến ngày&nbsp;<span style="color: Red;">*</span></div>
                            </td>
                            <td style="width: 20%">
                                <div>
                                    <%=MyHtmlHelper.DatePicker(ParentID, dDenNgay, "dDenNgay", "", "class=\"input1_2\"")%>
                                    <br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_dDenNgay")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td10" style="width: 15%">
                                <div>
                                    Nước đi&nbsp;<span style="color: Red;">*</span></div>
                            </td>
                            <td colspan="3">
                                <div>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaQuocGia, iID_MaNuoc, "iID_MaNuoc", "", "class=\"input1_2\" style=\"with:100px;\"")%>
                                    <br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaNuoc")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td10" style="width: 15%">
                                <div>
                                    Lý do đi</div>
                            </td>
                            <td colspan="3">
                                <div>
                                    <%=MyHtmlHelper.TextBox(ParentID, sLyDoDi, "sLyDoDi", "", "class=\"input1_2\"  style=\"width:99%;\" maxlength='400'")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td10" style="width: 15%">
                                <div>
                                    Kinh phí</div>
                            </td>
                            <td style="width: 20%">
                                <div>
                                    <%=MyHtmlHelper.TextBox(ParentID, rKinhPhi, "rKinhPhi", "", "class=\"input1_2\" style=\"width:99%;\" maxlength='50'", 1)%>
                                </div>
                            </td>
                            <td class="td_form2_td10" style="width: 15%">
                                <div>
                                    Nguồn kinh phí</div>
                            </td>
                            <td style="width: 20%">
                                <div>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaNguonKinhPhi, iID_MaNguonKinhPhi, "iID_MaNguonKinhPhi", "", "class=\"input1_2\" style=\"with:100px;\"")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td10" style="width: 15%">
                                <div>
                                    Quyết định số</div>
                            </td>
                            <td style="width: 20%">
                                <div>
                                    <%=MyHtmlHelper.TextBox(ParentID, sSoQD, "sSoQD", "", "class=\"input1_2\" style=\"width:99%;\" maxlength='50'")%>
                                </div>
                            </td>
                            <td class="td_form2_td10" style="width: 15%">
                                <div>
                                    Ngày ký</div>
                            </td>
                            <td style="width: 20%">
                                <div>
                                    <%=MyHtmlHelper.DatePicker(ParentID, dNgayKy, "dNgayKy", "", "class=\"input1_2\"")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td10" style="width: 15%">
                                <div>
                                    Người ký</div>
                            </td>
                            <td style="width: 20%">
                                <div>
                                    <%=MyHtmlHelper.TextBox(ParentID, sNguoiKy, "sNguoiKy", "", "class=\"input1_2\" style=\"width:99%;\" maxlength='150'")%>
                                </div>
                            </td>
                            <td class="td_form2_td10" style="width: 15%">
                                <div>
                                    Ngày hiệu lực</div>
                            </td>
                            <td style="width: 20%">
                                <div>
                                    <%=MyHtmlHelper.DatePicker(ParentID, dNgayHieuLuc, "dNgayHieuLuc", "", "class=\"input1_2\"")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td10" style="width: 15%">
                                <div>
                                    Ghi chú</div>
                            </td>
                            <td colspan="3">
                                <div>
                                    <%=MyHtmlHelper.TextArea(ParentID, sMoTa, "sMoTa", "", "class=\"input1_2\"  style=\"width:99%;; resize:none;\"")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="height: 10px;">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td align="right" style="width: 10%">
                                            <input type="submit" class="button4" value="Lưu" />
                                        </td>
                                        <td style="width: 1%">
                                            &nbsp;
                                        </td>
                                        <td align="right" style="width: 10%">
                                            <input type="button" class="button4" value="Hủy" onclick="Dialog_close('<%=ParentID %>');" />
                                        </td>
                                        <td style="width: 1%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
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
</body>
</html>
