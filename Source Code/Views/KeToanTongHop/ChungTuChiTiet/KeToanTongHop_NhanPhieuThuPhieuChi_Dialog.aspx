<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

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
        String MaND = User.Identity.Name;
        String MaDiv = Request.QueryString["idDiv"];
        String MaDivDate = Request.QueryString["idDivDate"];
        String iNamLamViec = Convert.ToString(NguoiDungCauHinhModels.iNamLamViec);
        int sSoGhiSo = KeToanTongHop_ChungTuModels.GetMaxChungTu_CuoiCung(iNamLamViec);
        String iID_MaChungTu = KeToanTongHop_ChungTuModels.LayMaChungTu(Convert.ToString(sSoGhiSo));
        if (String.IsNullOrEmpty(iID_MaChungTu) || iID_MaChungTu==Guid.Empty.ToString())
        {
            iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
        }
        String iThang = Request.QueryString["iThang"];
        String OnSuccess = "";
        OnSuccess = Request.QueryString["OnSuccess"];
        String ParentID = "Create";

        NameValueCollection data = KeToanTongHop_ChungTuModels.LayThongTin(iID_MaChungTu);
        String iNgay = data["iNgay"];
        String iNam = data["iNamLamViec"];
        DataTable dtNgay = DanhMucModels.DT_Ngay();
        SelectOptionList slNgay = new SelectOptionList(dtNgay, "MaNgay", "TenNgay");
        dtNgay.Dispose();

        DataTable dtThang = DanhMucModels.DT_Thang();
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        dtThang.Dispose();

        DataTable dtDV = NganSach_HamChungModels.DSDonViCuaNguoiDung(MaND);
        SelectOptionList slDV = new SelectOptionList(dtDV, "iID_MaDonVi", "sTen");
        dtDV.Dispose();

        DataTable dt = KeToanTongHop_ChungTuChiTietModels.Lay_DanhSachChungTuChiTietTienMat_TheoThang(iNam, iThang);

        using (Html.BeginForm("Edit_Fast_Submit_Nhan", "KeToanTongHop_ChungTu", new { ParentID = ParentID, iLoai = 3 }))
        {
    %>
    <div style="background-color: #ffffff; background-repeat: repeat">
        <div style="padding: 5px 1px 10px 1px;">
            <div class="box_tong">
                <div class="title_tong">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td>
                                <span>Danh sách Phiếu thu/Phiếu chi trong tháng:
                                    <%=iThang %></span>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="nhapform">
                    <div id="form2">
                        <table cellpadding="0" cellspacing="0" border="0" class="table_form2">
                            <tr>
                                <td align="left" colspan="2" valign="top">
                                    <table class="mGrid">
                                        <tr>
                                            <th style="width: 25px; text-align: center;">
                                                &nbsp;
                                            </th>
                                            <th style="width: 150px; text-align: center;">
                                                Số ghi sổ KTTH
                                            </th>
                                            <th style="text-align: center;">
                                                Số ghi sổ KTCT - Diễn giải
                                            </th>
                                            <th style="width: 12px;">
                                                &nbsp;
                                            </th>
                                        </tr>
                                    </table>
                                    <div id="divListDanhSach">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="height: 10px; font-size: 5px;">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div style="padding-left: 20px; cursor: pointer;">
                                        <input type="checkbox" id="Checkbox2" onclick="ChonCB(this.checked)" style="cursor: pointer;" />
                                        &nbsp;<b>Chứng từ chưa nhận vào KTTH</b> (Đánh dấu để chỉ xem các bút toán chưa
                                        nhận vào dữ liệu KT)</div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%; padding-left: 20px;">
                                    <fieldset style="height: 45px; border-radius: 4px; -moz-border-radius: 4px; -webkit-border-radius: 4px;
                                        cursor: pointer;">
                                        <legend>&nbsp;Thông tin chứng từ</legend>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td style="width: 20%">
                                                    <div style="margin-left: 5px;">
                                                        <b>Số C.T.G.S cuối cùng: &nbsp<%=sSoGhiSo.ToString()%></b>
                                                    </div>
                                                </td>
                                                <td style="width: 7%">
                                                    Ngày
                                                </td>
                                                <td style="width: 10%">
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slNgay, iNgay, "iNgay", "", "class=\"input1_2\" style=\"width:90%;\"")%>
                                                </td>
                                                <td style="width: 7%">
                                                    Tháng
                                                </td>
                                                <td style="width: 10%">
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang, "iThang", "", "class=\"input1_2\" disabled=\"disabled\" style=\"width:90%;\"")%>
                                                </td>
                                                <td style="width: 7%">
                                                    Đơn vị
                                                </td>
                                                <td style="width: 20%">
                                                    <%=MyHtmlHelper.TextBox(ParentID, "", "sDonVi", "", "class=\"input1_2\" style=\"width:90%;\"")%>
                                                </td>
                                                <td style="width: 10%">
                                                    <div style="margin-left: 5px;">
                                                        <b>Số C.T.G.S</b>
                                                    </div>
                                                </td>
                                                <td style="width: 10%">
                                                    <%=MyHtmlHelper.TextBox(ParentID, sSoGhiSo+1, "sSoChungTu", "", "class=\"input1_2\" style=\"width:90%;\"")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table border="0" cellpadding="0" cellspacing="0" style="padding-top: 10px;" align="center"
                            width="100%">
                            <tr>
                                <td align="right" style="width: 45%">
                                    <input id="btnThem" type="submit" class="button4" value="Thêm" />
                                </td>
                                <td style="width: 1%;">
                                    &nbsp;
                                </td>
                                <td align="left" style="width: 45%">
                                    <input type="button" class="button4" value="Hủy" onclick="Dialog_close('<%=ParentID %>');" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%
        } if (dt != null) { dt.Dispose(); };
    %>
    <script type="text/javascript">
        ChonCB("false");
        function setCheckboxes() {
            $('input:checkbox[group-index="1"]').each(function (i) {
                this.checked = document.getElementById('chkCheckAll').checked;
            });
        }
        function ChonCB(CB) {
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("ds_ChungTu?ParentID=#0&iNamLamViec=#1&iThang=#2&iLoai=#3&LayChungTuChuaNhan=#4", "KeToanTongHop_ChungTu")%>');
            url = unescape(url.replace("#0", "<%=ParentID %>"));
            url = unescape(url.replace("#1", "<%=iNam %>"));
            url = unescape(url.replace("#2", "<%=iThang %>"));
            url = unescape(url.replace("#3", 3));
            url = unescape(url.replace("#4", CB));

            $.getJSON(url, function (data) {
                document.getElementById("divListDanhSach").innerHTML = data;
            });

        }    
    </script>
</body>
</html>
