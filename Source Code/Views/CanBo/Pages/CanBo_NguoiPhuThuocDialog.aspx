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
        if (Request.QueryString["iID_MaNguoiPhuThuoc"] != null) iID_MaQuaTrinhCongTac = Request.QueryString["iID_MaNguoiPhuThuoc"];
        String OnSuccess = "", DuLieuMoi = "";
        OnSuccess = Request.QueryString["OnSuccess"];
        String ParentID = "Create";
        if (String.IsNullOrEmpty(iID_MaQuaTrinhCongTac) == false && iID_MaQuaTrinhCongTac != "") DuLieuMoi = "0";
        else DuLieuMoi = "1";

        //Chức vụ
        var dtQuanHe = DanhMucModels.DT_DanhMuc("CanBo_QuanHeGD", true, "--- Lựa chọn ---");
        SelectOptionList slMaQuanHeGD = new SelectOptionList(dtQuanHe, "iID_MaDanhMuc", "sTen");
        if (dtQuanHe != null) dtQuanHe.Dispose();

        String sHoTen = "", iID_MaQuanHe = "", sLyDoGiamTru = "", dTuNgay = "", dDenNgay = "";


        //lấy chi tiết
        if (String.IsNullOrEmpty(iID_MaQuaTrinhCongTac) == false && iID_MaQuaTrinhCongTac != "")
        {
            var dt = CanBo_DanhMucNhanSuModels.GetChiTiet_NguoiPhuThuoc(iID_MaQuaTrinhCongTac);
            if (dt.Rows.Count > 0)
            {
                DataRow DR = dt.Rows[0];
                sHoTen = HamChung.ConvertToString(DR["sHoTen"]);
                iID_MaQuanHe = HamChung.ConvertToString(DR["iID_MaQuanHe"]);
                sLyDoGiamTru = HamChung.ConvertToString(DR["sLyDoGiamTru"]);
                String TuNgay = CommonFunction.LayXauNgay(HamChung.ConvertDateTime(DR["dTuNgay"]));
                if (TuNgay != "01/01/0001") dTuNgay = TuNgay;
                String DenNgay = CommonFunction.LayXauNgay(HamChung.ConvertDateTime(DR["dDenNgay"]));
                if (DenNgay != "01/01/0001") dDenNgay = DenNgay;
            }
        }


        using (Ajax.BeginForm("Edit_Fast_NguoiPhuThuoc_Submit", "CanBo_NguoiPhuThuoc", new { ParentID = ParentID, OnSuccess = OnSuccess, iID_MaCanBo = iID_MaCanBo, iID_MaQuaTrinhCongTac = iID_MaQuaTrinhCongTac }, new AjaxOptions { }))
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
                                    Họ tên&nbsp;<span style="color: Red;">*</span></div>
                            </td>
                            <td colspan="3">
                                <div>
                                    <%=MyHtmlHelper.TextBox(ParentID, sHoTen, "sHoTen", "", "class=\"input1_2\"  style=\"width:99%;\" maxlength='150'")%><br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_sHoTen")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td10" style="width: 15%">
                                <div>
                                    Mối quan hệ&nbsp;<span style="color: Red;">*</span></div>
                            </td>
                            <td colspan="3">
                                <div>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaQuanHeGD, iID_MaQuanHe, "iID_MaQuanHe", "", "class=\"input1_2\" style=\"with:100px;\"")%>
                                </div>
                            </td>
                        </tr>
                         <tr>
                            <td class="td_form2_td10" style="width: 15%">
                                <div>
                                    Lý do giảm trừ</div>
                            </td>
                            <td colspan="3">
                                <div>
                                    <%=MyHtmlHelper.TextBox(ParentID, sLyDoGiamTru, "sLyDoGiamTru", "", "class=\"input1_2\"  style=\"width:99%;\" maxlength='150'")%><br />
                                  
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
                                    Đến ngày</div>
                            </td>
                            <td style="width: 20%">
                                <div>
                                    <%=MyHtmlHelper.DatePicker(ParentID, dDenNgay, "dDenNgay", "", "class=\"input1_2\"")%>
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
