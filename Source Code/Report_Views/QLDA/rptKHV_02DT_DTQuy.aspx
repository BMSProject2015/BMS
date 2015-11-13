<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QLDA" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <% 
        String ParentID = "KHV_02DT_DTQUY";
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String MaND = User.Identity.Name;
        DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
        String iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
        String sDeAn = Convert.ToString(ViewData["sDeAn"]);
        if (String.IsNullOrEmpty(sDeAn))
        {
            PageLoad = "0";
        }
        DataTable dtNgoaiTe = QLDA_ReportModel.getdtTien();
        String NgoaiTe = Convert.ToString(ViewData["NgoaiTe"]);
        if (String.IsNullOrEmpty(NgoaiTe))
        {
            NgoaiTe = "0";
        }
        SelectOptionList slNgoaiTe = new SelectOptionList(dtNgoaiTe, "iID_MaNgoaiTe", "sTen");
        dtNgoaiTe.Dispose();


        DataTable dtQuy = DanhMucModels.DT_Quy(false);
        String Quy = Convert.ToString(ViewData["Quy"]);
        if (String.IsNullOrEmpty(Quy))
            Quy = "1";
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
        dtQuy.Dispose();

        String iCapTongHop = Convert.ToString(ViewData["iCapTongHop"]);
        if (String.IsNullOrEmpty(iCapTongHop))
        {
            iCapTongHop = "5";
        }
        String URLView = "";
        if (PageLoad == "1")
            URLView = Url.Action("ViewPDF", "rptKHV_02DT_DTQuy", new { sDeAn = sDeAn, NgoaiTe = NgoaiTe, Quy = Quy, iNamLamViec = iNamLamViec, iCapTongHop = iCapTongHop });
        String URL = Url.Action("Index", "QLDA_Report");
        String urlExport = Url.Action("ExportToExcel", "rptKHV_02DT_DTQuy", new { sDeAn = sDeAn, NgoaiTe = NgoaiTe, Quy = Quy, iNamLamViec = iNamLamViec, iCapTongHop=iCapTongHop });
        using (Html.BeginForm("EditSubmit", "rptKHV_02DT_DTQuy", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td width="50%">
                	    <span>Báo cáo dự toán qúy</span>
                    </td>
                    <td width="50%" align="left">
                        <div class="login12"><a href="#">Ẩn/Hiện form lọc báo cáo</a></div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td width="15%">
                        </td>
                        <td width="20%">
                         <% rptKHV_02DT_DTQuyController rpt = new rptKHV_02DT_DTQuyController();
                            rptKHV_02DT_DTQuyController.QLDA_DTQuy _Data = new rptKHV_02DT_DTQuyController.QLDA_DTQuy();
                               _Data = rpt.obj_QLDA(ParentID, sDeAn, NgoaiTe);
                            %>
                              <%=_Data.DeAn%>
                        </td>
                        <td width="30%">
                         <table border="0" cellspacing="0" cellpadding="0" width="100%">
                        	<tr>
                             <td class="td_form2_td1" style="width: 35%;">
                            <div>
                                <%=NgonNgu.LayXau("Chọn quý:")%></div>
                            </td>
                             <td width="40%" style="vertical-align: middle;">
                             <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, Quy, "Quy", "", "class=\"input1_2\" style=\"width:80%;\" ")%>
                            </div>
                              </td>
                            </tr>
                            <tr>
                             <td class="td_form2_td1">
                            <div>
                                <%=NgonNgu.LayXau("Chọn ngoại tệ:")%></div>
                            </td>
                            <td>
                            <%=_Data.NgoaiTe%>
                            </td>
                            </tr>
                        </table>
                        </td>
                          <td width="20%"><div>
                                <fieldset>
                                    <legend><b>Tổng hợp tới cấp</b></legend>
                                    <table>
                                        <tr>
                                            <td class="td_form2_td1" style="padding-right: 10px;">
                                                <div>
                                                    Đề án:</div>
                                            </td>
                                            <td class="td_form2_td1">
                                                <div>
                                                    <%=MyHtmlHelper.Option(ParentID, "0", iCapTongHop, "iCapTongHop", "", " style=\"width:30px\"")%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_form2_td1" style="padding-right: 10px;">
                                                <div>
                                                    Dự án:</div>
                                            </td>
                                            <td class="td_form2_td1">
                                                <div>
                                                    <%=MyHtmlHelper.Option(ParentID, "1", iCapTongHop, "iCapTongHop", "", " style=\"width:30px\"")%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_form2_td1" style="padding-right: 10px;">
                                                <div>
                                                    Dự án thành phần:</div>
                                            </td>
                                            <td class="td_form2_td1">
                                                <div>
                                                    <%=MyHtmlHelper.Option(ParentID, "2", iCapTongHop, "iCapTongHop", "", " style=\"width:30px\" ")%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_form2_td1" style="padding-right: 10px;">
                                                <div>
                                                    Công trình:</div>
                                            </td>
                                            <td class="td_form2_td1">
                                                <div>
                                                    <%=MyHtmlHelper.Option(ParentID, "3", iCapTongHop, "iCapTongHop", "", " style=\"width:30px\" ")%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_form2_td1" style="padding-right: 10px;">
                                                <div>
                                                    HM Công trình:</div>
                                            </td>
                                            <td class="td_form2_td1">
                                                <div>
                                                    <%=MyHtmlHelper.Option(ParentID, "4", iCapTongHop, "iCapTongHop", "", " style=\"width:30px\" ")%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_form2_td1" style="padding-right: 10px;">
                                                <div>
                                                    HM Chi Tiết:</div>
                                            </td>
                                            <td class="td_form2_td1">
                                                <div>
                                                    <%=MyHtmlHelper.Option(ParentID, "5", iCapTongHop, "iCapTongHop", "", " style=\"width:30px\" ")%></div>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>
                            </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan="4">
                            <table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;"
                                width="100%">
                                <tr>
                                    <td style="width: 47%;" align="right">
                                        <input type="submit" class="button" name="submitButton" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="1%">
                                    </td>
                                    <td style="width: 48%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <%} %>
    <%=MyHtmlHelper.ActionLink(urlExport, "Export To Excel") %>
    <iframe src="<%=URLView %>" height="600px" width="100%"></iframe>
      <script type="text/javascript">
          function Huy() {
              window.location.href = '<%=URL %>';
          }
    </script>
     <script type="text/javascript">
         function Chonall(sDeAn) {
             $("input:checkbox[check-group='MaDeAn']").each(function (i) {
                 if (sDeAn) {
                     this.checked = true;
                 }
                 else {
                     this.checked = false;
                 }
             }
             );
         }                                            
    </script>
    <script type="text/javascript">
        $(function () {
            $('div.login12 a').click(function () {
                $('div#Div1').slideToggle('normal');
                $(this).toggleClass('active');
                return false;
            });

        });
    </script>
</body>
</html>
