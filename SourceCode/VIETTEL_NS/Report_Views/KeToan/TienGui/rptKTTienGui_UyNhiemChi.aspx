<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QuyetToan" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.TienGui" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
</head>

<body>


<%  String MaDiv = Request.QueryString["idDiv"];
    String MaDivDate = Request.QueryString["idDivDate"];
    String srcFile = Convert.ToString(ViewData["srcFile"]);
    String ParentID = "QuyetToanThongTri";
    String UserID = User.Identity.Name;
    DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(UserID);
    String iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
    String iThang = Convert.ToString(dtCauHinh.Rows[0]["iThangLamViec"]);
    dtCauHinh.Dispose();
    String iID_MaChungTu =Convert.ToString(ViewData["iID_MaChungTu"]);
    String siID_MaChungTuChiTiet = Convert.ToString(ViewData["sSoChungTuChiTiet"]);
    String inmuc = Convert.ToString(ViewData["inmuc"]);
    String[] arriID_MaChungTuChiTiet = siID_MaChungTuChiTiet.Split(',');
    int ChiSo = Convert.ToInt16(ViewData["ChiSo"]);
    int iSoLien = Convert.ToInt16(ViewData["iSoLien"]);
    String iID_MaChungTuChiTiet = arriID_MaChungTuChiTiet[ChiSo];
   // inmuc = "3";
    if (inmuc == "3") iID_MaChungTuChiTiet = siID_MaChungTuChiTiet;
    if (ChiSo < arriID_MaChungTuChiTiet.Length - 1) ChiSo = ChiSo + 1;
    String LoaiBaoCao = Convert.ToString(ViewData["LoaiBaoCao"]);
    
    String iID_MaTaiKhoan = Request.QueryString["iID_MaTaiKhoan"];
    String iNgay = Request.QueryString["iNgay"];
    String iThangCT = Request.QueryString["iThangCT"];
    String OnSuccess = "";
    OnSuccess = Request.QueryString["OnSuccess"];
    String BackURL = Url.Action("Index", "KeToanChiTietTienGui");
    String UrlReport = Url.Action("ViewPDF", "rptKTTienGui_UyNhiemChi", new { MaND = UserID, iID_MaChungTu = iID_MaChungTu, sSoChungTuChiTiet = iID_MaChungTuChiTiet, LoaiBaoCao = LoaiBaoCao, inmuc = inmuc});
    String UrlReport_Hiden = Url.Action("ViewPDF_Hiden", "rptKTTienGui_UyNhiemChi", new { MaND = UserID, iID_MaChungTu = iID_MaChungTu, sSoChungTuChiTiet = iID_MaChungTuChiTiet, LoaiBaoCao = LoaiBaoCao, inmuc = inmuc,iSoLien=iSoLien });
    using (Html.BeginForm("EditSubmit", "rptKTTienGui_UyNhiemChi", new { ParentID = ParentID, iNamLamViec = iNamLamViec, iThang = iThang, ChiSo = ChiSo }))
    {
    %>
    <%=MyHtmlHelper.Hidden("", siID_MaChungTuChiTiet, "sSoChungTuChiTiet", "")%>
    <%=MyHtmlHelper.Hidden(ParentID, iID_MaChungTu, "iID_MaChungTu", "")%>
    <%=MyHtmlHelper.Hidden(ParentID, inmuc, "inmuc", "")%>
    <%=MyHtmlHelper.Hidden(ParentID, LoaiBaoCao, "LoaiBaoCao", "")%>
    <%=MyHtmlHelper.Hidden(ParentID, iSoLien, "iSoLien_show", "")%>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo ủy nhiệm chi</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                   
                    <tr>
                        <td align="right">
                            <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                        </td>
                        <td width="5px">
                        </td>
                        <td>
                            <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>"  onclick="Huy()" />
                        </td>
                        <td>
                            <input class="button" type="button" value="<%=NgonNgu.LayXau("In")%>"  onclick="javascript:printDoc('ID_DIV')" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <%} %>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=BackURL %>';
        }

        function printDoc(id) {


            var prtContent = document.getElementById(id);

            
            var WinPrint = window.open('', '', 'width=800,height=650,resizable=1,scrollbars=1,left=100');
            WinPrint.document.write(prtContent.innerHTML);
            WinPrint.document.close();

            WinPrint.focus();
            //WinPrint.print();
            //WinPrint.close();
            //CallPrint(id);
            


        }


        
 </script>
 <div>
 
 <iframe src="<%=UrlReport%>" height="600px" width="100%">
     </iframe>
</div>
<div style="display:none"  id="ID_DIV" >
<iframe id="ID_UNC" name="ID_UNC" src="<%=UrlReport_Hiden%>" height="600px" width="100%">
</iframe>
</div>
</body>
</html>

