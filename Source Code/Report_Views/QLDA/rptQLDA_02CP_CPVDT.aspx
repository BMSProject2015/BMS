<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QLDA" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QuyetToan" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.TienGui" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        div.login1
        {
            text-align: center;
            background: transparent url(/Content/Report_Image/login.gif) no-repeat top center;
        }
        div.login1 a
        {
            color: white;
            text-decoration: none;
            font: bold 16px "Museo 700";
            display: block;
            width: 50px;
            height: 20px;
            line-height: 20px;
            margin: 0px auto;
            background: transparent url(/Content/Report_Image/arrow.png) no-repeat 20px -29px;
            -webkit-border-radius: 2px;
            border-radius: 2px;
        }
        div.login1 a.active
        {
            background-position: 20px 1px;
        }
        div.login1 a:active, a:focus
        {
            outline: none;
        }
    </style>
</head>
<body>
    <%
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String MaND = User.Identity.Name;
        Object NamLamViec = NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND, "iNamLamViec");
        String ParentID = "QLDA";
        String dNgayBaoCao = Convert.ToString(ViewData["dNgayBaoCao"]);
        String pageLoad = Convert.ToString(ViewData["LoadPage"]);
        String iID_MaNgoaiTe = Convert.ToString(ViewData["iID_MaNgoaiTe"]);
        String dsDeAn = Convert.ToString(ViewData["dsDeAn"]);
        String iCapTongHop = Convert.ToString(ViewData["iCapTongHop"]);
        String BackURL = Url.Action("Index", "QLDA_Report", new { sLoai = "0" });
        String viewPdfUrl = String.Empty;
        if (String.IsNullOrEmpty(iCapTongHop))
        {
            iCapTongHop = "4";
        }
        if (String.IsNullOrEmpty(iID_MaNgoaiTe))
        {
            iID_MaNgoaiTe = "0";
        }
        if (String.IsNullOrEmpty(dsDeAn))
        {
            dsDeAn = "-1";
        }
        if (String.IsNullOrEmpty(dsDeAn))
        {
            dsDeAn = "-1";
        }
        if (String.IsNullOrEmpty(dNgayBaoCao))
        {
            dNgayBaoCao = DateTime.Now.ToString("dd/MM/yyyy");
        }
        if (pageLoad.Equals("1"))
        {
            viewPdfUrl = Url.Action("ViewPDF", "rptQLDA_02CP_CPVDT", new { NamLamViec = NamLamViec, strNgayNhap = dNgayBaoCao, iID_MaNgoaiTe = iID_MaNgoaiTe, dsDeAn = dsDeAn, iCapTongHop = iCapTongHop });
        }
        using (Html.BeginForm("EditSubmit", "rptQLDA_02CP_CPVDT", new { ParentID = ParentID }))
        { 
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td width="47.9%">
                        <span>Báo cáo chi tiết cấp phát vốn đầu tư</span>
                    </td>
                    <td width="52%" style="text-align: left;">
                        <div class="login1" style="width: 50px; height: 20px; text-align: left;">
                            <a style="cursor: pointer;"></a>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="table_form2" class="table_form2">
            <div id="rptMain" style="width: 100%; margin: 0px auto; padding: 0px 0px; overflow: visible;">
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="padding-left: 20px; width: 40%" valign="top">
                            <div id="td_TaiKhoan" style="width: 98%; height: 200px; overflow: scroll; border: 1px solid black;">
                                <fieldset>
                                    <legend><b>Danh sách đề án</b></legend>
                                    <% rptQLDA_02CP_CPVDTController rpt = new rptQLDA_02CP_CPVDTController();           
                                    %>
                                    <%=rpt.sDanhSachDeAn(dsDeAn, NamLamViec.ToString())%>
                                </fieldset>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="padding-left: 20px; width: 30%" valign="top">
                            <div id="td_LoaiNgoaiTe" style="width: 98%; height: 200px; overflow: scroll; border: 1px solid black;">
                                <fieldset>
                                    <legend><b>Chọn loại ngoại tệ</b></legend>
                                    <%=rpt.sDanhSachNgoaiTe(iID_MaNgoaiTe)%>
                                </fieldset>
                            </div>
                            <div>
                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td align="right">
                                            <div>
                                                <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                            </div>
                                        </td>
                                        <td width="5px">
                                            &nbsp;
                                        </td>
                                        <td align="left">
                                            <div>
                                                <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" /></div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="padding-left: 20px; width: 30%" valign="top">
                            <div >
                                <fieldset>
                                    <legend><b>Tổng hợp tới cấp</b></legend>
                                    <table>
                                        <tr>
                                            <td class="td_form2_td1" style="padding-right: 10px;">
                                                <div>
                                                    <b>Dự án</b></div>
                                            </td>
                                            <td class="td_form2_td1">
                                                <div>
                                                    <%=MyHtmlHelper.Option(ParentID, "0", iCapTongHop, "iCapTongHop", "", " style=\"width:30px\"")%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_form2_td1" style="padding-right: 10px;">
                                                <div>
                                                    <b>Dự án thành phần</b></div>
                                            </td>
                                            <td class="td_form2_td1">
                                                <div>
                                                    <%=MyHtmlHelper.Option(ParentID, "1", iCapTongHop, "iCapTongHop", "", " style=\"width:30px\" ")%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_form2_td1" style="padding-right: 10px;">
                                                <div>
                                                    <b>Công trình</b></div>
                                            </td>
                                            <td class="td_form2_td1">
                                                <div>
                                                    <%=MyHtmlHelper.Option(ParentID, "2", iCapTongHop, "iCapTongHop", "", " style=\"width:30px\" ")%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_form2_td1" style="padding-right: 10px;">
                                                <div>
                                                    <b>Hạng mục công trình</b></div>
                                            </td>
                                            <td class="td_form2_td1">
                                                <div>
                                                    <%=MyHtmlHelper.Option(ParentID, "3", iCapTongHop, "iCapTongHop", "", " style=\"width:30px\" ")%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_form2_td1" style="padding-right: 10px;">
                                                <div>
                                                    <b>Hạng mục chi tiết</b></div>
                                            </td>
                                            <td class="td_form2_td1">
                                                <div>
                                                    <%=MyHtmlHelper.Option(ParentID, "4", iCapTongHop, "iCapTongHop", "", " style=\"width:30px\" ")%></div>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>
                            <div style="width: 100%; height: 50px;" valign="bottom">
                                <table>
                                    <tr>
                                        <td>
                                            <div>
                                                <%=NgonNgu.LayXau("Chọn ngày báo cáo")%></div>
                                        </td>
                                        <td>
                                            <div>
                                                <%=MyHtmlHelper.DatePicker(ParentID, dNgayBaoCao, "dNgayBaoCao", "", "class=\"input1_2\"")%>
                                                <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayBaoCao")%>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function CheckAll(value) {
            $("input:checkbox[check-group='iID_MaDanhMucDuAn']").each(function (i) {
                this.checked = value;
            });
        }

        function CheckAllNgoaiTe(value) {
            $("input:checkbox[check-group='iID_MaNgoaiTe']").each(function (i) {
                this.checked = value;
            });
        }   
    </script>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=BackURL %>';
        }
    </script>
    <%} %><script type="text/javascript">
              $(function () {
                  $("div#rptMain").hide();
                  $('div.login1 a').click(function () {
                      $('div#rptMain').slideToggle('normal');
                      $(this).toggleClass('active');
                      return false;
                  });
              });
       
    </script>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptQLDA_02CP_CPVDT", new { NamLamViec = NamLamViec, strNgayNhap = dNgayBaoCao, iID_MaNgoaiTe = iID_MaNgoaiTe, dsDeAn = dsDeAn, iCapTongHop = iCapTongHop }), "Export To Excel")%>
    <iframe src="<%=viewPdfUrl%>" height="600px" width="100%"></iframe>
</body>
</html>
