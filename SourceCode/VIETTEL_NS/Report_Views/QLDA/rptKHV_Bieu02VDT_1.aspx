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
        String ParentID = "KHV_Bieu02";
        String MaND = User.Identity.Name;
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String MaTien = Convert.ToString(ViewData["MaTien"]);
        if (String.IsNullOrEmpty(MaTien))
        {
            MaTien = "0";
        }
        String iCapTongHop = Convert.ToString(ViewData["iCapTongHop"]);
        if (String.IsNullOrEmpty(iCapTongHop))
        {
            iCapTongHop = "5";
        }
        DataTable dtDeAn = QLDA_DuToan_NamModels.LayDanhSachDeAn_Report(MaND);
        String sDeAn = Convert.ToString(ViewData["sDeAn"]);
        if (String.IsNullOrEmpty(sDeAn)) sDeAn = "-1";
        String[] arrDeAn = sDeAn.Split(',');
        DataTable dtNgoaiTe = QLDA_ReportModel.getdtTien();
        SelectOptionList slNgoaiTe = new SelectOptionList(dtNgoaiTe, "iID_MaNgoaiTe", "sTen");
        dtNgoaiTe.Dispose();

        String dDenNgay = Convert.ToString(ViewData["dDenNgay"]);
        if (dDenNgay == "")
            dDenNgay = string.Format("{0:dd/MM/yyyy}", DateTime.Now);
        String URLView = "";
        if (PageLoad == "1")
            URLView = Url.Action("ViewPDF", "rptKHV_Bieu02VDT_1", new { NgoaiTe = MaTien, vidDenNgay = dDenNgay, sDeAn = sDeAn,iCapTongHop=iCapTongHop });
        String URL = Url.Action("Index", "QLDA_Report");
        String urlExport = Url.Action("ExportToExcel", "rptKHV_Bieu02VDT_1", new { NgoaiTe = MaTien, vidDenNgay = dDenNgay, sDeAn = sDeAn, iCapTongHop = iCapTongHop });
        using (Html.BeginForm("EditSubmit", "rptKHV_Bieu02VDT_1", new { ParentID = ParentID, vidDenNgay = dDenNgay }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo tổng hợp tình hình thực hiện tình hình ngân sách năm <%=ReportModels.iNamLamViec(MaND) %> </span>
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
                         <td class="td_form2_td1" style="width: 10%;"><div>
                           
                                    <%=NgonNgu.LayXau("Đề án")%></div></td>
                        <td style="width: 20%;">
                               <div style="width: 100%; height: 200px; overflow: scroll; border: 1px solid black;">
                                <table class="mGrid">
                                     <tr>
                               <td><input type="checkbox" id="checkAll" onclick="Chonall(this.checked)"></td>
                                <td> Chọn tất cả Đề án </td>
                                </tr>
                                    <%
                                    String TenDeAn = ""; String DeAn = "";
                                    String _Checked = "checked=\"checked\"";  
                                    for (int i = 0; i < dtDeAn.Rows.Count; i++)
                                    {
                                        _Checked = "";
                                        TenDeAn = Convert.ToString(dtDeAn.Rows[i]["TenHT"]);
                                        DeAn = Convert.ToString(dtDeAn.Rows[i]["sDeAn"]);
                                        for (int j = 0; j < arrDeAn.Length; j++)
                                        {
                                            if (DeAn == arrDeAn[j])
                                            {
                                                _Checked = "checked=\"checked\"";
                                                break;
                                            }
                                        }    
                                    %>
                                    <tr>
                                        <td style="width: 15%;">
                                            <input type="checkbox" value="<%=DeAn %>" <%=_Checked %> check-group="sDeAn" id="sDeAn" 
                                                name="sDeAn" />
                                        </td>
                                        <td>
                                            <%=TenDeAn%>
                                        </td>
                                    </tr>
                                  <%}%>
                                </table>
                            </div>
                         </td>
                        <td class="td_form2_td1" style="width: 10%;">
                            <div>
                                <%=NgonNgu.LayXau("Đến ngày:")%></div>
                        </td>
                        <td width="10%" style="vertical-align: middle;">
                            <div>
                                <%=MyHtmlHelper.DatePicker(ParentID, dDenNgay, "dDenNgay", "", "class=\"input1_2\"")%>
                            </div>
                        </td>
                         <td style="width: 10%;" class="td_form2_td1">
                            <div><%=NgonNgu.LayXau("Chọn loại tiền:")%></div></td>
                        <td width="10%" id="<%= ParentID %>_LoaiTien">                            
                              <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slNgoaiTe, MaTien, "MaTien", "", "class=\"input1_2\" style=\"width: 80%\"")%> </div>                       
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
                        <td colspan="8">
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
    <%=MyHtmlHelper.ActionLink(urlExport, "Export To Excel") %>
    <%} %>
    <iframe src="<%=URLView %>" height="600px" width="100%"></iframe>
</body>
</html>
<script type="text/javascript">
    function Huy() {
        window.location.href = '<%=URL %>';
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
         function Chonall(value) {
             $("input:checkbox[check-group='sDeAn']").each(function (i) {
                 this.checked = value;
             });
         }
       </script>