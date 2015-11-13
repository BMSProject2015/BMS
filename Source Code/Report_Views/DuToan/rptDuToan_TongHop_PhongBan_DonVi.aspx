<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.DuToan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 92px;
        }
        .style2
        {
            height: 10px;
            width: 92px;
        }
    </style>
</head>
<body>
    <%
        String ParentID = "DuToan";
        String MaND = User.Identity.Name;
        String iNamLamViec = ReportModels.LayNamLamViec(MaND);
        
        //dtDot
        String iID_MaDot = Convert.ToString(ViewData["iID_MaDot"]);
        DataTable dtDot = DuToanBSModels.getDSDot(iNamLamViec, MaND);
        SelectOptionList slDot = new SelectOptionList(dtDot, "iDotCap", "iDotCap");
        dtDot.Dispose();
        
        //dtPhongBan
        string iID_MaPhongBan = Convert.ToString(ViewData["iID_MaPhongBan"]);
        DataTable dtPhongBan = DuToan_ReportModels.getDSPhongBanAll(iNamLamViec, MaND);
        SelectOptionList slPhongBan = new SelectOptionList(dtPhongBan, "iID_MaPhongBan", "sTenPhongBan");
        dtPhongBan.Dispose();
        
        //dtLoaiNganSach
        String sLNS = Convert.ToString(ViewData["sLNS"]);
        String iID_MaPhongBanND = NganSach_HamChungModels.MaPhongBanCuaMaND(MaND);
        DataTable dtLNS = DanhMucModels.NS_LoaiNganSach_PhongBan(iID_MaPhongBanND);
        if (String.IsNullOrEmpty(sLNS))
        {
            if (dtLNS.Rows.Count > 0)
            {
                sLNS = Convert.ToString(dtLNS.Rows[0]["sLNS"]);
            }
            else
            {
                sLNS = Guid.Empty.ToString();
            }
        }
        dtLNS.Dispose();
        String[] arrLNS = sLNS.Split(',');

        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        String[] arrMaDonVi = iID_MaDonVi.Split(',');
        String[] arrView = new String[arrMaDonVi.Length];
        String chuoi = "";
        String LoaiTongHop = Convert.ToString(ViewData["LoaiTongHop"]);
        if (String.IsNullOrEmpty(LoaiTongHop))
        {
            LoaiTongHop = "ChiTiet";
        }

        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        if (String.IsNullOrEmpty(PageLoad) || String.IsNullOrEmpty(iID_MaDonVi))
        {
            PageLoad = "0";
        }

        if (PageLoad == "1")
        {
            if (LoaiTongHop == "ChiTiet")
            {
                for (int i = 0; i < arrMaDonVi.Length; i++)
                {
                    arrView[i] = String.Format(@"/rptDuToan_TongHop_PhongBan_DonVi/viewpdf?iID_MaDonVi={0}&sLNS={1}&iID_MaDot={2}&iID_MaPhongBan={3}&MaND={4}&LoaiTongHop={5}",
                        arrMaDonVi[i], sLNS, iID_MaDot, iID_MaPhongBan, MaND, LoaiTongHop);
                    chuoi += arrView[i];
                    if (i < arrMaDonVi.Length - 1)
                    {
                        chuoi += "+";
                    }
                }
            }
            else
            {
                arrView = new string[1];
                arrView[0] = String.Format(@"/rptDuToan_TongHop_PhongBan_DonVi/viewpdf?iID_MaDonVi={0}&sLNS={1}&iID_MaDot={2}&iID_MaPhongBan={3}&MaND={4}&LoaiTongHop={5}",
                        iID_MaDonVi, sLNS, iID_MaDot, iID_MaPhongBan, MaND, LoaiTongHop);
                chuoi += arrView[0];
            }
        }
        int SoCot = 1;
        String urlExport = Url.Action("ExportToExcel", "rptDuToan_TongHop_PhongBan_DonVi", new { });
        String BackURL = Url.Action("Index", "DuToan_Report", new { sLoai = 1 });
        using (Html.BeginForm("EditSubmit", "rptDuToan_TongHop_PhongBan_DonVi", new { ParentID = ParentID }))
        {
    %>
    <%=MyHtmlHelper.Hidden(ParentID, MaND, "MaND", "")%>
         <div class="box_tong">
            <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo tình hình dự toán bổ sung năm <%=iNamLamViec%></span>
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
            <div>
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="td_form2_td1" style="width: 10%; height: 10px">
                            <div><b>Chọn Đợt</b></div>
                        </td>
                        <td style="width: 10%">
                            <div>
                            <%=MyHtmlHelper.DropDownList(ParentID, slDot, iID_MaDot, "iID_MaDot", "", "class=\"input1_2\" style=\"width:100%;\"onchange=Chon()")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 15%">
                            <b>Loại ngân sách: &nbsp;&nbsp; </b>
                        </td>
                        <td style="width: 25%" rowspan="25">
                            <div  style="overflow: scroll; height: 400px">
                            
                            <table class="mGrid" style="width: 100%">
                                <tr>
                                    <th align="center" style="width: 40px;">
                                        <input type="checkbox" id="abc" onclick="CheckAllDV(this.checked)" />
                                    </th>
                                    <%for (int c = 0; c < SoCot * 2 - 1; c++)
                                      {%>
                                    <th>
                                    </th>
                                    <%} %>
                                </tr>
                                <%
                
                                String strsTen = "", MaLNS = "", strChecked = "";
                                for (int i = 0; i < dtLNS.Rows.Count; i = i + SoCot)
                                {
                    
                    
                                %>
                                <tr>
                                    <%for (int c = 0; c < SoCot; c++)
                                      {
                                          if (i + c < dtLNS.Rows.Count)
                                          {
                                              strChecked = "";
                                              strsTen = Convert.ToString(dtLNS.Rows[i + c]["TenHT"]);
                                              MaLNS = Convert.ToString(dtLNS.Rows[i + c]["sLNS"]);
                                              for (int j = 0; j < arrLNS.Length; j++)
                                              {
                                                  if (MaLNS.Equals(arrLNS[j]))
                                                  {
                                                      strChecked = "checked=\"checked\"";
                                                      break;
                                                  }
                                              }
                                    %>
                                    <td align="center" style="width: 40px;">
                                        <input type="checkbox" value="<%=MaLNS %>" <%=strChecked %> check-group="LNS"
                                            onclick="Chon()" id="sLNS" name="sLNS" />
                                    </td>
                                    <td align="left">
                                        <%=strsTen%>
                                    </td>
                                    <%} %>
                                    <%} %>
                                </tr>
                                <%}%>
                            </table>
                            </div>
                        </td>
                        <td>
                        </td>
                        <td class="style1"> 
                            <div style="width: 130px"><b>Chọn Đơn Vị</b></div>
                        </td>
                        <td rowspan="25" style="width: 30%;">
                            <div id="<%= ParentID %>_tdDonVi" style="overflow: scroll; height: 400px">
                            </div>
                        </td>
                       
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="width: 15%">
                            <div><b>Chọn Phòng Ban</b></div>
                        </td>
                        <td style="width: 10%">
                            <div>
                            <%=MyHtmlHelper.DropDownList(ParentID, slPhongBan, iID_MaPhongBan, "iID_MaPhongBan", "", "class=\"input1_2\" style=\"width:100%;\"onchange=Chon()")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td class="style2">
                            <div style="color: red; font-size: medium; padding-left: 5px; padding-bottom: 10px; width: 126px;">
                                <%=MyHtmlHelper.Option(ParentID, "ChiTiet", LoaiTongHop, "LoaiTongHop", "", "")%>
                                Chi tiết ĐV
                                
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%; height: 10px">
                        
                        </td>
                        <td>
                        
                        </td>
                        <td>
                        
                        </td>
                        <td>
                        </td>
                        
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td class="style2">
                            <div style="color: red; font-size: medium; padding-left: 5px; padding-bottom: 10px; width: 125px;">
                            <%=MyHtmlHelper.Option(ParentID, "TongHop", LoaiTongHop, "LoaiTongHop", "", "")%>
                                Tổng hợp ĐV
                            </div>
                        </td>
                        
                       
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
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
                        <td colspan="7" align="center">
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
             function CheckAllDV(value) {
                 $("input:checkbox[check-group='LNS']").each(function (i) {
                     this.checked = value;
                 });
                 Chon();
             }                                            
         </script>
         <script type="text/javascript">
             function CheckAll(value) {
                 $("input:checkbox[check-group='DonVi']").each(function (i) {
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
                var chuoi = '<%=chuoi%>';
                var Mang=chuoi.split("+");
                   var pageLoad = <%=PageLoad %>;
                   if(pageLoad=="1") {
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
            function Chon(){

                var sLNS = "";
                 $("input:checkbox[check-group='LNS']").each(function (i) {
                     if (this.checked) {
                         if (sLNS != "") sLNS += ",";
                         sLNS += this.value;
                     }
                 });
                var iID_MaPhongBan = document.getElementById("<%=ParentID %>_iID_MaPhongBan").value;
                var iID_MaDot = document.getElementById("<%=ParentID %>_iID_MaDot").value;

                jQuery.ajaxSetup({cache: false});
                var url = unescape('<%= Url.Action("Ds_DonVi?ParentID=#0&iID_MaDonVi=#1&iID_MaDot=#2&iID_MaPhongBan=#3&sLNS=#4", "rptDuToan_TongHop_PhongBan_DonVi") %>')
                url = unescape(url.replace("#0", "<%= ParentID %>"));
                url = unescape(url.replace("#1","<%= iID_MaDonVi %>"));
                url = unescape(url.replace("#2", iID_MaDot));
                url = unescape(url.replace("#3", iID_MaPhongBan));
                url = unescape(url.replace("#4", sLNS));

                $.getJSON(url, function (data) {
                    document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data;
                });
            }
         </script>
         <script type="text/javascript">
             function Huy() {
                 window.location.href = '<%=BackURL%>';
             }
        </script>
        <div>
            <%=MyHtmlHelper.ActionLink(urlExport, "Export To Excel")%>
        </div>
        </div>

    <%} %>
</body>
</html>
