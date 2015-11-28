<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Models.DuToanBS" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.DuToanBS" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript" src="../../../Scripts/jquery-latest.js"></script>
    <style type="text/css">
        .style1
        {
            width: 10%;
            height: 10px;
        }
        .style2
        {
            height: 10px;
        }
    </style>
</head>
<body>
    <%
        String ParentID = "DuToanBS";
        String MaND = User.Identity.Name;
        String iNamLamViec = ReportModels.LayNamLamViec(MaND);
        
        //hungpx dt dot hardcode
        DataTable dtDot = new DataTable();
        dtDot.Columns.Add("MaDot", typeof(string));
        dtDot.Columns.Add("TenDot", typeof(string));
        DataRow R1 = dtDot.NewRow();
        R1["MaDot"] = "-1";
        R1["TenDot"] = "--Chọn đợt--";
        dtDot.Rows.Add(R1);
        SelectOptionList slDot = new SelectOptionList(dtDot, "MaDot", "TenDot");
        String idDot = Convert.ToString(ViewData["iID_MaDot"]);
        if (String.IsNullOrEmpty(idDot))
        {
            if(dtDot.Rows.Count >0 )
                idDot = Convert.ToString(dtDot.Rows[0]["MaDot"]);
            else
                idDot = Guid.Empty.ToString();
        }
        dtDot.Dispose();
                
        //dt Danh sách phòng ban
        //String iID_MaPhongBan = NganSach_HamChungModels.MaPhongBanCuaMaND(MaND);
        
        DataTable dtPhongBan = DuToanBSModels.getDSPhongBan(iNamLamViec, MaND);
        SelectOptionList slPhongBan = new SelectOptionList(dtPhongBan, "iID_MaPhongBan", "sTenPhongBan");

        String MaPhongBan = Convert.ToString(ViewData["iID_MaPhongBan"]);
        if (MaPhongBan == null)
            MaPhongBan = "-1";
        dtPhongBan.Dispose();
        
        //hungpx: dt Loại ngân sách truyen di sau khi bam submit
        String sLNS = Convert.ToString(ViewData["sLNS"]);
        //DataTable dtLNS = DanhMucModels.NS_LoaiNganSach_PhongBan(iID_MaPhongBan);
        DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(MaND);
        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "TenHT");
        dtDonVi.Dispose();
        
        String BackURL = Url.Action("Index", "QuyetToan_Report", new { Loai = 0 });
        
        // hungpx: tach xau ma don vi 
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        if (String.IsNullOrEmpty(iID_MaDonVi)) iID_MaDonVi = "-100";
        String[] arrDonVi = iID_MaDonVi.Split(',');
        String[] arrView = new String[arrDonVi.Length];
        String Chuoi = "";
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        if (String.IsNullOrEmpty(PageLoad))
            PageLoad = "0";
        //if (String.IsNullOrEmpty(sLNS)) PageLoad = "0";
        if (PageLoad == "1")
        {

            for (int i = 0; i < arrDonVi.Length; i++)
            {
                arrView[i] =
                    String.Format(
                        @"/rptDuToanBS_ChiTieuNganSAch/viewpdf?iID_MaDonVi={0}&sLNS={1}&iID_MaDot={2}&MaND={3}&MaPhongBan={4}",
                        arrDonVi[i], sLNS, idDot, MaND, MaPhongBan);
                Chuoi += arrView[i];
                if (i < arrDonVi.Length - 1)
                    Chuoi += "+";
            }
           
        }

        int SoCot = 1;
        String[] arrMaNS = sLNS.Split(',');
        String urlExport = Url.Action("ExportToExcel", "rptDuToanBS_ChiTieuNganSach", new { });
        using (Html.BeginForm("EditSubmit", "rptDuToanBS_ChiTieuNganSach", new { ParentID = ParentID, }))
        {
    %>
   
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo so sánh chỉ tiêu quyết toán năm
                            <%=iNamLamViec%></span>
                    </td>
                    <td width="52%" style="text-align: left;">
                        <div class="login1" style="width: 50px; height: 20px; text-align: left;">
                            <a style="cursor: pointer;"></a>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="rptMain" style="background-color: #F0F9FE;">
            <div id="Div2" style="margin-left: 10px;" class="table_form2">
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="td_form2_td1" style="width: 10%; height: 20px">
                            <div>
                                <b>Chọn đợt</b></div>
                        </td>
                        <td class="td_form2_td5" style="width: 14%; height: 20px">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slDot, idDot, "iID_MaDot", "", "class=\"input1_2\" style=\"width:100%;\"onchange=Chon()")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 8%">
                            <div>
                                <b>Chọn đơn vị</b>
                            </div>
                        </td>
                         <td rowspan="25" style="width: 20%;" class="td_form2_td5">
                             <div id="<%= ParentID %>_tdDonVi" style="overflow: scroll; height: 400px">
                            </div>
                        </td>
                        
                        <td class="td_form2_td1" style="width: 8%">
                            <b>Loại ngân sách</b>
                        </td>
                       <td rowspan="25" style="width: 20%;" class="td_form2_td5">
                            <div id="<%= ParentID %>_tdLNS" style="overflow: scroll; height: 400px">
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 2%">
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="width: 10%; height: 20px">
                         <div>
                                <b>Chọn phòng ban </b>
                         </div>
                        </td>
                        <td class="td_form2_td5" style="width: 14%; height: 20px">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slPhongBan, MaPhongBan, "iID_MaPhongBan", "", "class=\"input1_2\" style=\"width:100%;\"onchange=Chon() ")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="center">
                            <table cellpadding="0" cellspacing="0" border="0" align="center">
                                <tr>
                                    <td>
                                        <input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="5px">
                                    </td>
                                    <td>
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <script type="text/javascript">
                     function CheckAll(value) {
                         $("input:checkbox[check-group='DonVi']").each(function (i) {
                             this.checked = value;
                         });
                         ChonDonVi();
                     }                                            
         </script>
          <script type="text/javascript">
              function CheckAllLNS(value) {
                  $("input:checkbox[check-group='LNS']").each(function (i) {
                      this.checked = value;
                  });
              }                                            
        </script>

        <script type="text/javascript">
            $(document).ready(function () {
                   $('.title_tong a').click(function () {
                    $('div#rptMain').slideToggle('normal');
                    $(this).toggleClass('active');
                    return false;
                });
                var count = <%=arrView.Length%>;
                var Chuoi = '<%=Chuoi%>';
                var Mang = Chuoi.split("+");
                var pageLoad = <%=PageLoad %>;
                
                if(pageLoad=="1") 
                {
                  var siteArray = new Array(count);
                  for (var i = 0; i < count; i++) {
                       siteArray[i] = Mang[i];
                    }
                  for (var i = 0; i < count; i++) {
                        window.open(siteArray[i], '_blank');
                    }
                } 
            });

            Chon();

            // hungpx update view don vi
            function Chon() {
                 var iID_MaDonVi = "";
                     $("input:checkbox[check-group='DonVi']").each(function (i) {
                         if (this.checked) {
                             if (iID_MaDonVi != "") iID_MaDonVi += ",";
                             iID_MaDonVi += this.value;
                         }
                     });
                 //var iID_MaDonVi = document.getElementById("<%=ParentID %>_iID_MaDonVi").value;
                 var iID_MaDot = document.getElementById("<%=ParentID %>_iID_MaDot").value;
                 var MaPhongBan = document.getElementById("<%=ParentID %>_iID_MaPhongBan").value;
                 
                jQuery.ajaxSetup({ cache: false });

                var url = unescape('<%= Url.Action("Ds_DonVi?ParentID=#0&iID_MaDot=#1&iID_MaPhongBan=#2&iID_MaDonVi=#3", "rptDuToanBS_ChiTieuNganSach") %>');
                url = unescape(url.replace("#0", "<%= ParentID %>"));
                url = unescape(url.replace("#1", iID_MaDot));
                url = unescape(url.replace("#2", MaPhongBan));
                var pageLoad = <%=PageLoad %>;
                
                if(pageLoad=="1") 
                    url = unescape(url.replace("#3", "<%= iID_MaDonVi %>"));
                else
                    url = unescape(url.replace("#3", iID_MaDonVi));
                $.getJSON(url, function (data) {
                    document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data;
                    ChonDonVi();
                });
            }
            function ChonDonVi() {
                var sLNS = "";
                $("input:checkbox[check-group='LNS']").each(function (i) {
                    if (this.checked) {
                        if (sLNS != "") sLNS += ",";
                        sLNS += this.value;
                    }
                });
                var iID_MaDonVi = "";
                $("input:checkbox[check-group='DonVi']").each(function (i) {
                    if (this.checked) {
                        if (iID_MaDonVi != "") iID_MaDonVi += ",";
                        iID_MaDonVi += this.value;
                    }
                });
                jQuery.ajaxSetup({ cache: false });

                var iID_MaDot = document.getElementById("<%=ParentID %>_iID_MaDot").value;
                var MaPhongBan = document.getElementById("<%=ParentID %>_iID_MaPhongBan").value;
                var url = unescape('<%= Url.Action("Ds_LNS?ParentID=#0&iID_MaDot=#1&iID_MaPhongBan=#2&iID_MaDonVi=#3&sLNS=#4", "rptDuToanBS_ChiTieuNganSach") %>');
                url = unescape(url.replace("#0", "<%= ParentID %>"));
                url = unescape(url.replace("#1", iID_MaDot));
                url = unescape(url.replace("#2", MaPhongBan));   
                var pageLoad = <%=PageLoad %>;           
                if(pageLoad=="1") {
                    url = unescape(url.replace("#3", "<%=iID_MaDonVi %>"));
                    url = unescape(url.replace("#4", "<%=sLNS %>"));
                }
                else{
                    url = unescape(url.replace("#4", sLNS));
                    url = unescape(url.replace("#3", iID_MaDonVi));
                }
                $.getJSON(url, function (data) {
                    document.getElementById("<%= ParentID %>_tdLNS").innerHTML = data;
                });
            }                                       
        </script>
        <script type="text/javascript">
            function Huy() {
                window.location.href = '<%=BackURL%>';
            }
        </script>
    </div>
    <%} %>
</body>
</html>
