<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.KhoBac" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>CapPhat_ThongTri_Dialog</title>
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
            String iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
            String iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
            dtCauHinh.Dispose();
            String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
            //String sTenDonVi_Nhan = Request.QueryString["sTenDonVi_Nhan"];
            // DataTable dtSo = rptKTKhoBac_GiayRutDuToanController.Lay_SoChungTu(iNamLamViec,iThang,iID_MaChungTu);
            //DataTable dtSo = rptThongTriLoaiLNSController.dtDanhSachDonVi_Nhan(iNamLamViec, iThang, iID_MaChungTu);
            DataTable dtSo = rptThongTriLoaiNSController.dtDanhSachDonVi_Nhan(iNamLamViec, iThang, iID_MaChungTu);
            SelectOptionList slSo = new SelectOptionList(dtSo, "sTenDonVi_Nhan", "sTenDonVi_Nhan");
            dtSo.Dispose();
            using (Html.BeginForm("EditSubmit", "rptThongTriLoaiNS", new { ParentID = ParentID, iNamLamViec = iNamLamViec, iThang = iThang, ChiSo = 0 }))
            {
            
        %>
        <%=MyHtmlHelper.Hidden(ParentID,iID_MaChungTu,"iID_MaChungTu","") %>
        <div style="background-color: #ffffff; background-repeat: repeat">
            <div style="padding: 5px 1px 10px 1px;">
                <div class="box_tong">
                    <div id="nhapform">
                        <div id="form2">
                            <div id="Div1">
                                <div id="Div2">
                                    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                                        <tr>
                                            <td>
                                             <div style="width: 100%; height: 250px; overflow: scroll; position: relative;">
                                                <table class="mGrid">
                                                    <tr>
                                                        <td class="td_form2_td1" style="width: 20px; text-align: center">
                                                            <input type="checkbox" id="abc" onclick="CheckAll(this.checked)" />
                                                        </td>
                                                        <td class="td_form2_td1" style="width: 200px; text-align: center; font-weight: bold;">
                                                            <div>
                                                                <%=NgonNgu.LayXau("Chọn đơn vị")%></div>
                                                        </td>
                                                    </tr>
                                                    <%for (int i = 0; i < dtSo.Rows.Count; i++)
                                                      {
                                                          String sSoChungTuChiTiet = Convert.ToString(dtSo.Rows[i]["sSoChungTuChiTiet"]);
                                                          String sTenDonVi_Nhan = Convert.ToString(dtSo.Rows[i]["sTenDonVi_Nhan"]);
                                                    %>
                                                    <tr>
                                                        <td style="text-align: center">
                                                            <input type="checkbox" value="<%=sSoChungTuChiTiet%>" check-group="sSoChungTuChiTiet"
                                                                id="<%=ParentID %>_sSoChungTuChiTiet" name="<%=ParentID %>_sSoChungTuChiTiet" />
                                                        </td>
                                                        <td style="text-align: left">
                                                            <%=sTenDonVi_Nhan%>
                                                        </td>
                                                    </tr>
                                                    <%} %>
                                                </table></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 10px; font-size: 5px;">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;&nbsp;
                                                <%=MyHtmlHelper.CheckBox(ParentID,"0","InTenMLNS","","") %>&nbsp;&nbsp;&nbsp;<b style="color: Red">IN
                                                    Tên MLNS(Nội Dung)</b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 10px; font-size: 5px;">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100%">
                                                <p style="text-align: center;">
                                                    <input type="submit" class="button4" value="Tiếp tục" style="margin-right: 5px; display: inline-block;" /><input
                                                        type="button" class="button4" value="Hủy" onclick="Dialog_close('<%=ParentID %>');"
                                                        style="margin-left: 5px; display: inline-block;" />
                                                </p>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%
}
        %>
    </div>
    <script type="text/javascript">
        function CheckAll(value) {
            $("input:checkbox[check-group='sSoChungTuChiTiet']").each(function (i) {
                this.checked = value;
            });
        }                                            
    </script>
</body>
</html>
