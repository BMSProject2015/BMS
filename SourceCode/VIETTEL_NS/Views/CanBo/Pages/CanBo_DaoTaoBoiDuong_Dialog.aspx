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
        string iDaTotNghiep = "0";//0: dang hoc; 1 Da tot nghiep
        if (Request.QueryString["iID_MaQuaTrinhCongTac"] != null) iID_MaQuaTrinhCongTac = Request.QueryString["iID_MaQuaTrinhCongTac"];
        String OnSuccess = "", DuLieuMoi = "";
        OnSuccess = Request.QueryString["OnSuccess"];
        String ParentID = "Create";
        if (String.IsNullOrEmpty(iID_MaQuaTrinhCongTac) == false && iID_MaQuaTrinhCongTac != "") DuLieuMoi = "0";
        else DuLieuMoi = "1";
        String sNoiDaoTao = "", iID_MaChuyenNganh = "", iID_MaHinhThucDaoTao = "", dTuNgay = "",
            dDenNgay = "", iID_MaVanBang = "", iID_MaKetQuaDaoTao = "", iID_MaNoiDungDaoTao = "",
            bTrongNuoc = "", sMoTa = "", sID_MaTrinhDo="";

        //Chuyên ngành
        var dtChuyenNganh = DanhMucModels.DT_DanhMuc("CanBo_ChuyenNganh", true, "--- Lựa chọn ---");
        SelectOptionList slMaChuyenNganh = new SelectOptionList(dtChuyenNganh, "iID_MaDanhMuc", "sTen");
        if (dtChuyenNganh != null) dtChuyenNganh.Dispose();

        //Hình thức đào tạo
        var dtHTDT = DanhMucModels.DT_DanhMuc("CanBo_HinhThucDaoTao", true, "--- Lựa chọn ---");
        SelectOptionList slMaHinhThucDaoTao = new SelectOptionList(dtHTDT, "iID_MaDanhMuc", "sTen");
        if (dtHTDT != null) dtHTDT.Dispose();
        //Nội dung đào tạo
        var dtNoiDungDT = DanhMucModels.DT_DanhMuc("CanBo_NoiDungDaoTao", true, "--- Lựa chọn ---");
        SelectOptionList slMaNoiDungDaoTao = new SelectOptionList(dtNoiDungDT, "iID_MaDanhMuc", "sTen");
        if (dtNoiDungDT != null) dtNoiDungDT.Dispose();
        //Văn bẳng
        var dtVanBang = DanhMucModels.DT_DanhMuc("CanBo_VanBang", true, "--- Lựa chọn ---");
        SelectOptionList slMaVanBang = new SelectOptionList(dtVanBang, "iID_MaDanhMuc", "sTen");
        if (dtVanBang != null) dtVanBang.Dispose();


        //kết quả đào tạo
        var dtKetQuaDT = DanhMucModels.DT_DanhMuc("CanBo_KetQuaDaoTao", true, "--- Lựa chọn ---");
        SelectOptionList slMaKetQuaDT = new SelectOptionList(dtKetQuaDT, "iID_MaDanhMuc", "sTen");
        if (dtKetQuaDT != null) dtKetQuaDT.Dispose();

        //Trình độ đào tạo
        var dtTrinhDo = CanBo_DanhMucNhanSuModels.getHocVan(false);
        SelectOptionList slTrinhDo = new SelectOptionList(dtTrinhDo, "sID_MaTrinhDo", "sTen");
        if (dtTrinhDo != null) dtTrinhDo.Dispose();


        //Loại
        var dtTrongNuoc = CanBo_DanhMucNhanSuModels.getNoiDaoTao(false, "--- Lựa chọn ---");
        SelectOptionList slTrongNuoc = new SelectOptionList(dtTrongNuoc, "iID_Ma", "sTen");
        if (dtTrongNuoc != null) dtTrongNuoc.Dispose();
        //lấy chi tiết
        if (String.IsNullOrEmpty(iID_MaQuaTrinhCongTac) == false && iID_MaQuaTrinhCongTac != "")
        {
            var dt = CanBo_QuaTrinhDaoTaoModels.GetChiTiet(iID_MaQuaTrinhCongTac);
            if (dt.Rows.Count > 0)
            {
                DataRow DR = dt.Rows[0];
                sNoiDaoTao = HamChung.ConvertToString(DR["sNoiDaoTao"]);
                iID_MaChuyenNganh = HamChung.ConvertToString(DR["iID_MaChuyenNganh"]);
                iID_MaHinhThucDaoTao = HamChung.ConvertToString(DR["iID_MaHinhThucDaoTao"]);
                String TuNgay = CommonFunction.LayXauNgay(HamChung.ConvertDateTime(DR["dTuNgay"]));
                if (TuNgay != "01/01/0001") dTuNgay = TuNgay;
                String DenNgay = CommonFunction.LayXauNgay(HamChung.ConvertDateTime(DR["dDenNgay"]));
                if (DenNgay != "01/01/0001") dDenNgay = DenNgay;

                iID_MaVanBang = HamChung.ConvertToString(DR["iID_MaVanBang"]);
                iID_MaKetQuaDaoTao = HamChung.ConvertToString(DR["iID_MaKetQuaDaoTao"]);
                iID_MaNoiDungDaoTao = HamChung.ConvertToString(DR["iID_MaNoiDungDaoTao"]);
                sID_MaTrinhDo = HamChung.ConvertToString(DR["sID_MaTrinhDo"]);
                bTrongNuoc = HamChung.ConvertToString(DR["bTrongNuoc"]);
                sMoTa = HamChung.ConvertToString(DR["sMoTa"]);
                iDaTotNghiep = HamChung.ConvertToString(DR["iDaTotNghiep"]);
            }
        }
        using (Ajax.BeginForm("Edit_Fast_DaoTao_Submit", "CanBo_DaoTaoBoiDuong", new { ParentID = ParentID, OnSuccess = OnSuccess, iID_MaCanBo = iID_MaCanBo, iID_MaQuaTrinhCongTac = iID_MaQuaTrinhCongTac }, new AjaxOptions { }))
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
                                    Nơi đào tạo&nbsp;<span style="color: Red;">*</span></div>
                            </td>
                            <td colspan="3">
                                <div>
                                    <%=MyHtmlHelper.TextBox(ParentID, sNoiDaoTao, "sNoiDaoTao", "", "class=\"input1_2\" tab-index='-1' style=\"width:99%;\" maxlength='250'")%><br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_sNoiDaoTao")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td10" style="width: 15%">
                                <div>
                                    Chuyên ngành&nbsp;<span style="color: Red;">*</span></div>
                            </td>
                            <td style="width: 20%">
                                <div>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaChuyenNganh, iID_MaChuyenNganh, "iID_MaChuyenNganh", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                    <br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaChuyenNganh")%>
                                </div>
                            </td>
                            <td class="td_form2_td10" style="width: 15%">
                                <div>
                                    Hình thức đào tạo&nbsp;<span style="color: Red;">*</span></div>
                            </td>
                            <td style="width: 20%">
                                <div>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaHinhThucDaoTao, iID_MaHinhThucDaoTao, "iID_MaHinhThucDaoTao", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                    <br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaHinhThucDaoTao")%>
                                </div>
                            </td>
                        </tr>
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
                                    Văn bằng&nbsp;<span style="color: Red;">*</span></div>
                            </td>
                            <td style="width: 20%">
                                <div>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaVanBang, iID_MaVanBang, "iID_MaVanBang", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                    <br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaVanBang")%>
                                </div>
                            </td>
                            <td class="td_form2_td10" style="width: 15%">
                                <div>
                                    Trình độ&nbsp;<span style="color: Red;">*</span></div>
                            </td>
                            <td style="width: 20%">
                                <div>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slTrinhDo, sID_MaTrinhDo, "sID_MaTrinhDo", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td10" style="width: 15%">
                                <div>
                                </div>
                            </td>
                            <td style="width: 20%">
                                <div>
                                    <%=MyHtmlHelper.Option(ParentID, "0", iDaTotNghiep, "iDaTotNghiep", "", "style=\"width:10%;\" ")%>Đang học
                                    <%=MyHtmlHelper.Option(ParentID, "1", iDaTotNghiep, "iDaTotNghiep", "", "style=\"width:10%;\" ")%>Đã tốt nghiệp
                                </div>
                            </td>
                            <td class="td_form2_td10" style="width: 15%">
                                <div>
                                    Kết quả đào tạo&nbsp;<span style="color: Red;">*</span></div>
                            </td>
                            <td style="width: 20%">
                                <div>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaKetQuaDT, iID_MaKetQuaDaoTao, "iID_MaKetQuaDaoTao", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                    <br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaKetQuaDaoTao")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td10" style="width: 15%">
                                <div>
                                    Nội dung đào tạo&nbsp;<span style="color: Red;">*</span></div>
                            </td>
                            <td style="width: 20%">
                                <div>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaNoiDungDaoTao, iID_MaNoiDungDaoTao, "iID_MaNoiDungDaoTao", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                    <br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaNoiDungDaoTao")%>
                                </div>
                            </td>
                            <td class="td_form2_td10" style="width: 15%">
                                <div>
                                    Đào tạo</div>
                            </td>
                            <td style="width: 20%">
                                <div>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slTrongNuoc, bTrongNuoc, "bTrongNuoc", "", "class=\"input1_2\" style=\"with:100px;\"")%>
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
                                    <%=MyHtmlHelper.TextArea(ParentID, sMoTa, "sMoTa", "", "class=\"input1_2\"  style=\"width:99%; resize:none;\"")%>
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
