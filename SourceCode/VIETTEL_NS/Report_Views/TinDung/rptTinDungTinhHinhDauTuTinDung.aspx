<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.PhanBo" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <div>
     <%
        String ParentID = "TinDung";
        DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
        String iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
        String iThangLamViec = dtCauHinh.Rows[0]["iThangLamViec"].ToString();
        String pageload =Convert.ToString(ViewData["pageload"]);
        String iNam = Convert.ToString(ViewData["iNam"]);
        if (String.IsNullOrEmpty(iNam))
        {
            iNam = iNamLamViec;
        }
        DataTable dtNam = DanhMucModels.DT_Nam(true);
        SelectOptionList slNam = new SelectOptionList(dtNam,"MaNam","TenNam");
        dtNam.Dispose();
        String iThang = Convert.ToString(ViewData["iThang"]);
        if (String.IsNullOrEmpty(iThang))
        {
            iThang = iThangLamViec;
        }
        DataTable dtThang = DanhMucModels.DT_Thang(true);
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        dtThang.Dispose();

        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
        {
            iID_MaTrangThaiDuyet = "0";
        }
        DataTable dtTrangThai = ReportModels.Get_dtDSTrangThaiDuyet(PhanHeModels.iID_MaPhanHeTinDung);
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
        String BackURL = Url.Action("Index", "TinDung_Report");
        String URL = "";
         if(pageload=="1") URL=Url.Action("ViewPDF","rptTinDungTinhHinhDauTuTinDung",new{iID_MaTrangThaiDuyet=iID_MaTrangThaiDuyet,iNam=iNam,iThang=iThang});
        using (Html.BeginForm("EditSubmit", "rptTinDungTinhHinhDauTuTinDung", new { ParentID = ParentID }))
        {%>
         <div class="box_tong">
          <div class="title_tong">
                 <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td>
                            <span>Báo cáo tình hình đầu tư tín dụng</span>
                         </td>
                    </tr>
                </table>
            </div>
                <div id="Div1">
                <div id="Div2">
                    <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td style="width: 15%;"></td>
                         <td class="td_form2_td1" style="width: 10%;">
                            <div>
                                <%=NgonNgu.LayXau("Năm làm việc:")%></div>
                        </td>
                       <td width="10%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slNam, iNam, "iNam", "", "class=\"input1_2\" style=\"width: 100%\" ")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%;">
                            <div>
                                <%=NgonNgu.LayXau("Tháng làm việc:")%></div>
                        </td>
                       <td width="10%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang, "iThang", "", "class=\"input1_2\" style=\"width: 100%\"  ")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%;">
                            <div>
                                <%=NgonNgu.LayXau("Trạng thái:")%></div>
                        </td>
                       <td width="10%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%\"")%>
                            </div>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan="6"><table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
                            <tr>
                                    <td style="width: 49%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />                                    </td>
                                    <td width="2%">                                    </td>
                                    <td style="width: 49%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />                                    </td>
                                </tr>
                           </table></td> 
<td>
                        </td>
                    </tr>
    </table>
    </div>
    </div>
    </div>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptTinDungTinhHinhDauTuTinDung",new{iID_MaTrangThaiDuyet=iID_MaTrangThaiDuyet,iNam=iNam,iThang=iThang}), "Xuất ra Excels")%>
      <script type="text/javascript">
          function Huy() {
              window.location.href = '<%=BackURL%>';
          }
    </script>
    <%}%>
    <iframe src="<%=URL%>" height="600px" width="100%"></iframe>
    </div>
</body>
</html>
