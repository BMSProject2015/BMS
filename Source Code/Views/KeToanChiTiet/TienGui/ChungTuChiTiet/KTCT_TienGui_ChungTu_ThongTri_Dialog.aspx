<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient"%>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.TienGui" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>CapPhat_ThongTri_Dialog</title>
    <script type="text/javascript" src="../../../Scripts/jquery-latest.js"></script>
</head>

<body>
    <div>
    <%
       
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "RutDuToan";
        String UserID = User.Identity.Name;
        DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(UserID);
        String iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
        String iThang = Convert.ToString(dtCauHinh.Rows[0]["iThangLamViec"]);
        dtCauHinh.Dispose();
        String LoaiTK = Request.QueryString["LoaiTK"];
        // String iNamLamViec, String iThang, String iID_MaChungTu, String sSoChungTuChiTiet
        String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
        String iID_MaDonVi ="";
        
        DataTable dtPhongBan = DonViModels.DanhSach_DonVi_ChungTu_KeToan_TienMat(iID_MaChungTu);
        if (dtPhongBan.Rows.Count > 0)
            iID_MaDonVi = Convert.ToString(dtPhongBan.Rows[0]["iID_MaDonVi"].ToString());
        else
            iID_MaDonVi = Guid.Empty.ToString();

        SelectOptionList slPhongBan = new SelectOptionList(dtPhongBan, "iID_MaDonVi", "sTen");
        //DataTable dtSo = rptKTTienGui_UyNhiemChiController.Lay_SoChungTu(iNamLamViec, iThang, iID_MaChungTu);
 
        //String[] arriID_MaChungTu = iID_MaChungTu.Split(',');
        //dtSo.Dispose();
        //String LoaiBaoCao = Convert.ToString(ViewData["LoaiBaoCao"]);
        //if (String.IsNullOrEmpty(LoaiBaoCao))
        //{
        //    LoaiBaoCao = "KB";
        //}  
        //String inmuc = Convert.ToString(ViewData["inmuc"]);
        //if (String.IsNullOrEmpty(inmuc))
        //{  
        //    inmuc = "1";
        //}        
        using (Html.BeginForm("EditSubmit", "rptThongTriCap", new { ParentID = ParentID, iID_MaChungTu = iID_MaChungTu, iID_MaDonVi = iID_MaDonVi, LoaiTK = LoaiTK, iThang = iThang, iNamLamViec = iNamLamViec }))
        {
    %>
    <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
        <tr>
            <td style="padding-top: 10px;">
                <div>
                    <b>
                        <%=NgonNgu.LayXau("Chọn đơn vị cần in thông tri")%></b>
                </div>
            </td>
        </tr>
        <!------------------da sua--------------------->
        <tr>
            <td>
                <div>
                    <%=MyHtmlHelper.DropDownList(ParentID, slPhongBan, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 100%\" size='20'")%>
                </div>
            </td>
        </tr>
        <tr>
            <td style="height: 10px;">
            </td>
        </tr>
        <tr align="center" style="text-align: center;">
            <td style="text-align: center;">
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 10%">
                        </td>
                        <td align="right">
                            
                        </td>
                        <td style="width: 1%">
                            &nbsp;
                        </td>
                        <td align="left">
                            <input type="button" class="button4" value="Hủy" onclick="Dialog_close('<%=ParentID %>');" />
                        </td>
                        <td style="width: 10%">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <%} %>
</body>
</html>