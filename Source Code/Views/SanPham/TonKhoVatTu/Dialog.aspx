<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%=NgonNgu.LayXau("Cổng thông tin điện tử BQP")%></title>
</head>
<body>
<%
    String MaDiv = Request.QueryString["idDiv"];
    String MaDivDate = Request.QueryString["idDivDate"];
    String sMaVatTu = Request.QueryString["sMaVatTu"];
    String OnSuccess = "";
    OnSuccess = Request.QueryString["OnSuccess"];
    String ParentID = "Edit";
    Double SoLuongTonKho = 0;
    String rSoTonMoi = "";
    String iID_MaVatTu = "";
    String sTen = "";
    String DonViTinh = "";
    SqlCommand cmd;
    DataTable dt = null ;

    String sID_MaNguoiDung = User.Identity.Name;
    String IPSua = Request.UserHostAddress;
    cmd = new SqlCommand("SELECT iID_MaDonVi FROM QT_NhomNguoiDung " +
                           "WHERE iID_MaNhomNguoiDung = (SELECT iID_MaNhomNguoiDung " +
                                                        "FROM QT_NguoiDung " +
                                                        "WHERE sID_MaNguoiDung = @sID_MaNguoiDung)");
    cmd.Parameters.AddWithValue("@sID_MaNguoiDung", sID_MaNguoiDung);
    String iID_MaDonViDangNhap = Connection.GetValueString(cmd, "");
    cmd.Dispose();
    
    if (sMaVatTu != "")
    {
        cmd = new SqlCommand("SELECT iID_MaVatTu,sTen,rSoLuongTonKho,iDM_MaDonViTinh FROM DM_VatTu WHERE sMaVatTu = @sMaVatTu");
        cmd.Parameters.AddWithValue("@sMaVatTu", sMaVatTu);
        dt = Connection.GetDataTable(cmd);
        cmd.Dispose();

        if (dt.Rows.Count > 0)
        {
            sTen = Convert.ToString(dt.Rows[0]["sTen"]);
            iID_MaVatTu = Convert.ToString(dt.Rows[0]["iID_MaVatTu"]);
            String iDM_MaDonViTinh = Convert.ToString(dt.Rows[0]["iDM_MaDonViTinh"]);

            cmd = new SqlCommand("SELECT sTen FROM DC_DanhMuc WHERE iID_MaDanhMuc = @iDM_MaDonViTinh");
            cmd.Parameters.AddWithValue("@iDM_MaDonViTinh", iDM_MaDonViTinh);
            DonViTinh = Convert.ToString(Connection.GetValueString(cmd, ""));
            cmd.Dispose();

            cmd = new SqlCommand("SELECT rSoLuongTonKho FROM DM_DonVi_TonKho " +
                "WHERE iID_MaDonVi = @iID_MaDonVi AND iID_MaVatTu = @iID_MaVatTu");
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonViDangNhap);
            cmd.Parameters.AddWithValue("@iID_MaVatTu", iID_MaVatTu);
            SoLuongTonKho = Convert.ToDouble(Connection.GetValueString(cmd, "0"));
            cmd.Dispose();
        }
    }
    using (Ajax.BeginForm("Edit_Fast_Submit", "TonKhoVatTu", new { ParentID = ParentID, OnSuccess = OnSuccess, sMaVatTu = sMaVatTu, iID_MaDonVi = iID_MaDonViDangNhap, sID_MaNguoiDungSua = sID_MaNguoiDung, IPSua = IPSua, MaDiv = MaDiv, MaDivDate = MaDivDate }, new AjaxOptions { }))
    {
%>
<div style="background-color: #f0f9fe; background-repeat: repeat; border: solid 1px #ec3237">
    <div style="padding: 10px; text-align: center;">
        <h3>Nhập số lượng tồn mới cho mã vật tư</h3>
    </div>
    <div style="padding: 10px;">
        <div id="nhapform">
            <div id="form2">
                <table cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td style="width: 25%; padding-right: 15px" align="right">
                            <b>Mã vật tư</b>
                        </td>
                        <td style="width: 75%" align="right">
                            <%=MyHtmlHelper.TextBox(ParentID, sMaVatTu, "sMaVatTu", "", "style=\"width:99%;\" readonly=\"readonly\"")%>
                        </td>
                    </tr>
                    <tr><td style="height: 10px; font-size: 5px;" colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td align="right" style="padding-right: 15px">
                            <b>Tên vật tư</b>
                        </td>
                        <td align="right">
                            <%=MyHtmlHelper.TextBox(ParentID, sTen, "sTen", "", "style=\"width:99%;\" readonly=\"readonly\"")%>
                        </td>
                    </tr>
                    <tr><td style="height: 10px; font-size: 5px;" colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td align="right" style="padding-right: 15px">
                            <b>Số lượng tồn cũ</b>
                        </td>
                        <td align="right">
                            <%=MyHtmlHelper.TextBox(ParentID, SoLuongTonKho, "rSoLuongTonKho", "", "style=\"width:99%;\" readonly=\"readonly\"")%>
                        </td>
                    </tr>
                    <tr><td style="height: 10px; font-size: 5px;" colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td align="right" style="padding-right: 15px">
                            <b>Đơn vị tính</b>
                        </td>
                        <td align="right">
                            <%=MyHtmlHelper.TextBox(ParentID, DonViTinh, "iDM_MaDonViTinh", "", "style=\"width:99%;\" readonly=\"readonly\"")%>
                        </td>
                    </tr>
                    <tr><td style="height: 10px; font-size: 5px;" colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td align="right" style="padding-right: 15px">
                            <b>Số lượng tồn mới</b>
                        </td>
                        <td align="right">
                            <%=MyHtmlHelper.TextBox(ParentID, rSoTonMoi, "rSoLuongTonKhoMoi", "", "style=\"width:99%;\"")%>
                        </td>
                    </tr>
                    <tr><td style="height: 10px; font-size: 5px;" colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td align="right" colspan="2">
                            <input type="submit" class="button4" value="Lưu" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</div>
<%
    } if (dt != null) { dt.Dispose(); };
%>
</body>
</html>
