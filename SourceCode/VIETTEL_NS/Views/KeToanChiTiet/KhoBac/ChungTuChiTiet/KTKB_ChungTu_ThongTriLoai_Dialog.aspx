<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient"%>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.KhoBac" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>CapPhat_ThongTri_Dialog</title>
</head>
<body>
    <div>
    <%
       
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "QuyetToanThongTri";
        String UserID = User.Identity.Name;
        DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(UserID);
        String iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
        String iThang = Convert.ToString(dtCauHinh.Rows[0]["iThangLamViec"]);
        dtCauHinh.Dispose();
        String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
        //String sTenDonVi_Nhan = Request.QueryString["sTenDonVi_Nhan"];
        //DataTable dtSo = rptKTKhoBac_GiayRutDuToanController.Lay_SoChungTu(iNamLamViec,iThang,sSoChungTuChiTiet);
        //SelectOptionList slSo = new SelectOptionList(dtSo, "sTenDonVi_Nhan", "sTenDonVi_Nhan");
        //if (String.IsNullOrEmpty(sTenDonVi_Nhan)==false)
        //{
        //    sTenDonVi_Nhan = Convert.ToString(dtSo.Rows[0]["sTenDonVi_Nhan"]);
        //}
        //dtSo.Dispose();
        using (Html.BeginForm("EditSubmit", "rptKTKB_ThongTriRutDT", new { ParentID = ParentID, iNamLamViec = iNamLamViec, iThang = iThang, iID_MaChungTu = iID_MaChungTu }))
        {
    %>
    <div style="background-color: #ffffff; background-repeat: repeat; ">
        <div style="padding: 5px 1px 10px 1px;">
            <div class="box_tong" >
                <%--<div style="width:180px;padding:2px; margin:0 auto;">
                <p><input type="submit" class="button4" value="Tiếp tục" />
                <input type="button" class="button4" value="Hủy" onclick="Dialog_close('<%=ParentID %>');"/></p></div>--%>
                <div id="nhapform">
                  
                    <div id="form2">
                      <div id="Div1">
                        <div id="Div2">
                             <table cellpadding="0" cellspacing="0" border="0" style="width:150px;">                       
                            <tr>
                                <td style="width:100%;text-align:center;">
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr >
                                       
                                            <td >&nbsp;</td>
                                            <td align="center" style="width: 50px"><div><input type="submit" class="button4" value="Tiếp tục" /></div></td>
                                            <td style="width: 1%">&nbsp;</td>
                                            <td align="center" style="width: 50px"><input type="button" class="button4" value="Hủy" onclick="Dialog_close('<%=ParentID %>');"/></td>
                                            <td style="width: 1%">&nbsp;</td>
                                        </tr>
                                    </table>
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
            function setCheckboxes() {
                $('input:checkbox[group-index="1"]').each(function (i) {
                    this.checked = document.getElementById('chkCheckAll').checked;
                });
            }    
    </script>
</body>
</html>