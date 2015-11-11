<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.TongHop" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <%
        String ParentID = "KeToanTongHop";
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
        String iID_MaDonVi = Request.QueryString["iID_MaDonVi"];
        String LoaiTK = Request.QueryString["LoaiTK"];
        String iThang = Request.QueryString["iThang"];
        String iNam = Request.QueryString["iNamLamViec"];


        String iNgay1 = Request.QueryString["iNgay1"];
        String iNgay2 = Request.QueryString["iNgay2"];
        String iThang1 = Request.QueryString["iThang1"];
        String iThang2 = Request.QueryString["iThang2"];
        String LoaiBaoCao = Request.QueryString["LoaiBaoCao"];
        if (String.IsNullOrEmpty(LoaiBaoCao))
        {
            LoaiBaoCao = "PC2";
        }
        String iNamLamViec = Request.QueryString["iNamLamViec"];

        String OnSuccess = "";
        OnSuccess = Request.QueryString["OnSuccess"];
        String URL = Url.Action("Index", "KeToanChiTietTienMat");
    
    %>
    <!---------Test----------------------->
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Tổng hợp ngày</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                       
                        <td align="center">
                            <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=URL %>';
        }  
    </script>
    <iframe src="<%=Url.Action("ViewPDF","rptKTTM_TongHopNgay", new{iNgay1 = iNgay1, iNgay2 = iNgay2, iThang1 = iThang1, iThang2 = iThang2, LoaiBaoCao = LoaiBaoCao, iNamLamViec=iNam})%>"
        height="600px" width="100%"></iframe>
</body>
</html>
