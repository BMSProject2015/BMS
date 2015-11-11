<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

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
        if (Request.QueryString["iID_MaKyLuat"] != null) iID_MaQuaTrinhCongTac = Request.QueryString["iID_MaKyLuat"];
        String OnSuccess = "", DuLieuMoi = "";
        OnSuccess = Request.QueryString["OnSuccess"];
        String ParentID = "Create";
        if (String.IsNullOrEmpty(iID_MaQuaTrinhCongTac) == false && iID_MaQuaTrinhCongTac != "") DuLieuMoi = "0";
        else DuLieuMoi = "1";

        //Cấp khen thưởng kỷ luật
        var dtCapKhenThuong = DanhMucModels.DT_DanhMuc("CanBo_CapKhenThuongKyLuat", true, "--- Lựa chọn ---");
        SelectOptionList slMaCapKhen = new SelectOptionList(dtCapKhenThuong, "iID_MaDanhMuc", "sTen");
        if (dtCapKhenThuong != null) dtCapKhenThuong.Dispose();

        //hình thức kỷ luật
        var dtHinhThuc = DanhMucModels.DT_DanhMuc("CanBo_HinhThucKhenThuongKyLuat", true, "--- Lựa chọn ---");
        SelectOptionList slMaHinhThuc = new SelectOptionList(dtHinhThuc, "iID_MaDanhMuc", "sTen");
        if (dtHinhThuc != null) dtHinhThuc.Dispose();

        String iID_MaCapKyLuat = "", iID_MaHinhThuc = "", iThang = "", iNam = "",
            sLyDo = "", sSoQD = "", dNgayKy = "", sNguoiKy = "", dNgayHieuLuc = "", sMoTa = "";


        //lấy chi tiết
        if (String.IsNullOrEmpty(iID_MaQuaTrinhCongTac) == false && iID_MaQuaTrinhCongTac != "")
        {
            var dt = CanBo_DanhMucNhanSuModels.GetChiTiet_KyLuat(iID_MaQuaTrinhCongTac);
            if (dt.Rows.Count > 0)
            {
                DataRow DR = dt.Rows[0];           
                iID_MaCapKyLuat = HamChung.ConvertToString(DR["iID_MaCapKyLuat"]);
                iID_MaHinhThuc = HamChung.ConvertToString(DR["iID_MaHinhThuc"]);

                iThang = HamChung.ConvertToString(DR["iThang"]);
                iNam = HamChung.ConvertToString(DR["iNam"]);

                sLyDo = HamChung.ConvertToString(DR["sLyDo"]);
                sSoQD = HamChung.ConvertToString(DR["sSoQD"]);


             
                
                
                sNguoiKy = HamChung.ConvertToString(DR["sNguoiKy"]);
                sMoTa = HamChung.ConvertToString(DR["sMoTa"]);


                String TuNgay = CommonFunction.LayXauNgay(HamChung.ConvertDateTime(DR["dNgayKy"]));
                if (TuNgay != "01/01/0001") dNgayKy = TuNgay;
                String DenNgay = CommonFunction.LayXauNgay(HamChung.ConvertDateTime(DR["dNgayHieuLuc"]));
                if (DenNgay != "01/01/0001") dNgayHieuLuc = DenNgay;
            }
        }


        using (Ajax.BeginForm("Edit_Fast_KyLuat_Submit", "CanBo_KyLuat", new { ParentID = ParentID, OnSuccess = OnSuccess, iID_MaCanBo = iID_MaCanBo, iID_MaKyLuat = iID_MaQuaTrinhCongTac }, new AjaxOptions { }))
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
                                    Cấp kỷ luật&nbsp;<span style="color: Red;">*</span></div>
                            </td>
                            <td style="width: 20%">
                                <div>
                                     <%=MyHtmlHelper.DropDownList(ParentID, slMaCapKhen, iID_MaCapKyLuat, "iID_MaCapKyLuat", "", "class=\"input1_2\" style=\"with:100px;\"")%>
                                    <br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaCapKyLuat")%>
                                </div>
                            </td>
                            <td class="td_form2_td10" style="width: 15%">
                                <div>
                                    Hình thức kỷ luật</div>
                            </td>
                            <td style="width: 20%">
                                <div>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaHinhThuc, iID_MaHinhThuc, "iID_MaHinhThuc", "", "class=\"input1_2\" style=\"with:100px;\"")%>
                                    <br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaHinhThuc")%>   </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td10" style="width: 15%">
                                <div>
                                    Năm&nbsp;<span style="color: Red;">*</span></div>
                            </td>
                            <td style="width: 20%">
                                <div>
                                    <%=MyHtmlHelper.TextBox(ParentID, iNam, "iNam", "", "class=\"input1_2\"  style=\"width:99%;\" maxlength='150'")%>
                                     <br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_iNam")%>
                                </div>
                            </td>
                            <td class="td_form2_td10" style="width: 15%">
                                <div>
                                  Tháng</div>
                            </td>
                            <td style="width: 20%">
                                  <div>
                                    <%=MyHtmlHelper.TextBox(ParentID, iThang, "iThang", "", "class=\"input1_2\"  style=\"width:99%;\" maxlength='150'")%>
                                     <br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_iThang")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td10" style="width: 15%">
                                <div>
                                    Lý do kỷ luật</div>
                            </td>
                            <td colspan="3">
                                <div>
                                    <%=MyHtmlHelper.TextBox(ParentID, sLyDo, "sLyDo", "", "class=\"input1_2\"  style=\"width:99%;\" maxlength='150'")%><br />
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
