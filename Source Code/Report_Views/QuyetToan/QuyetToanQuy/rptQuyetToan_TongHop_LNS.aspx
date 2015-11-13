<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QuyetToan" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript" src="../../../Scripts/jquery-latest.js"></script>
</head>
<body>
    <%
        String ParentID = "QuyetToanNganSach";
        String MaND = User.Identity.Name;
        String iNamLamViec = ReportModels.LayNamLamViec(MaND);
        DataTable dtQuy = DanhMucModels.DT_Quy_QuyetToan();
        DataRow R1 = dtQuy.NewRow();
        R1["MaQuy"] = "5";
        R1["TenQuy"] = "Bổ sung";
        dtQuy.Rows.Add(R1);
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
        dtQuy.Dispose();

        String iID_MaNamNganSach = Convert.ToString(ViewData["iID_MaNamNganSach"]);
        DataTable dtNamNganSach = QuyetToanModels.getDSNamNganSach();
        SelectOptionList slNamNganSach = new SelectOptionList(dtNamNganSach, "MaLoai", "sTen");
        dtNamNganSach.Dispose();
        
        // dtPhongBan.
        string iID_MaPhongBan = Convert.ToString(ViewData["iID_MaPhongBan"]);
        DataTable dtPhongBan = QuyetToanModels.getDSPhongBan(iNamLamViec, MaND);
        SelectOptionList slPhongBan = new SelectOptionList(dtPhongBan, "iID_MaPhongBan", "sTenPhongBan");
        dtPhongBan.Dispose();
        
        String iThang_Quy = Convert.ToString(ViewData["iThang_Quy"]);
        
        // dung quy hien tai
        if (String.IsNullOrEmpty(iThang_Quy))
        {
            String mon = DateTime.Now.Month.ToString();
            if (mon == "1" || mon == "2" || mon == "3")
                iThang_Quy = "1";
            else if (mon == "4" || mon == "5" || mon == "6")
                iThang_Quy = "2";
            else if (mon == "7" || mon == "8" || mon == "9")
                iThang_Quy = "3";
            else
                iThang_Quy = "4";
        }
        
        //dt Loại ngân sách
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
        
        String BackURL = Url.Action("Index", "QuyetToan_Report", new { Loai = 0 });

        String[] arrLNS = sLNS.Split(',');
        String MaTo = Convert.ToString(ViewData["MaTo"]);
        String[] arrMaTo = MaTo.Split(',');
        String[] arrView = new String[arrMaTo.Length];
        
        String Chuoi = "";
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        if (String.IsNullOrEmpty(PageLoad))
            PageLoad = "0";
        if (String.IsNullOrEmpty(MaTo)) PageLoad = "0";
        
        if (PageLoad == "1")
        {

            //generate pdf link 
            for (int i = 0; i < arrMaTo.Length; i++)
            {
                arrView[i] =
                    String.Format(
                        @"/rptQuyetToan_TongHop_LNS/viewpdf?MaTo={0}&sLNS={1}&iThang_Quy={2}&iID_MaNamNganSach={3}&MaND={4}&iID_MaPhongBan={5}",
                        arrMaTo[i], sLNS, iThang_Quy, iID_MaNamNganSach, MaND, iID_MaPhongBan);
                Chuoi += arrView[i];
                if (i < arrMaTo.Length - 1)
                    Chuoi += "+";
            }
        }

        int SoCot = 1;
        String urlExport = Url.Action("ExportToExcel", "rptQuyetToan_TongHop_LNS", new { });
        using (Html.BeginForm("EditSubmit", "rptQuyetToan_TongHop_LNS",new {ParentID = ParentID}))
        {
    %>
    <%=MyHtmlHelper.Hidden(ParentID, MaND, "MaND","")%>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo tổng hợp tình hình quyết toán ngân sách năm
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
                        <td class="td_form2_td1" style="width: 10%; height: 10px">
                            <div>
                                <b>Chọn Quý :</b></div>
                        </td>
                        <td style="width: 10%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, iThang_Quy, "iThang_Quy", "", "class=\"input1_2\" style=\"width:100%;\"onchange=Chon()")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%">
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
                
                                    String strsTen = "", strsGiaTri = "", strChecked = "";
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
                                              strsGiaTri = Convert.ToString(dtLNS.Rows[i + c]["sLNS"]);
                                              if(arrLNS.Contains(strsGiaTri))
                                              {
                                                      strChecked = "checked=\"checked\"";
                                              }
                                    %>
                                    <td align="center" style="width: 40px;">
                                        <input type="checkbox" value="<%=strsGiaTri %>" <%=strChecked %> check-group="LNS"
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
                        <td class="td_form2_td1" style="width: 10%; height: 10px">
                            <b>Chọn tờ :</b>
                        </td>
                        <td rowspan="25" style="width: 30%;">
                            <div id="<%= ParentID %>_tdDonVi" style="overflow: scroll; height: 400px">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="width: 10%; height: 20px">
                            <div>
                                <b>Chọn năm :</b></div>
                        </td>
                        <td>
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slNamNganSach, iID_MaNamNganSach, "iID_MaNamNganSach", "", "class=\"input1_2\" style=\"width:100%;\"onchange=Chon() ")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%; height: 20px">
                            <div>
                                <b></b>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%; height: 20px">
                            <div>
                                <b></b>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="width: 10%; height: 20px">
                            <div>
                                <b>Chọn phòng ban :</b></div>
                        </td>
                        <td>
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slPhongBan, iID_MaPhongBan, "iID_MaPhongBan", "", "class=\"input1_2\" style=\"width:100%;\"onchange=Chon() ")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%; height: 20px">
                            <div>
                                <b></b>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%; height: 20px">
                            <div>
                                <b></b>
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
            function CheckAllDV(value) {
                $("input:checkbox[check-group='LNS']").each(function (i) {
                    this.checked = value;
                });
                Chon();
            }                                            
        </script>
        
         <script type="text/javascript">
             function CheckAllTO(value) {
                 $("input:checkbox[check-group='To']").each(function (i) {
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
                var Mang=Chuoi.split("+");
                var pageLoad = <%=PageLoad %>;
                if(pageLoad=="1") 
                {
                    var siteArray = new Array(count);
                    for (var i = 0; i < count; i++) 
                    {
                        siteArray[i] = Mang[i];
                    }
                    for (var i = 0; i < count; i++) {
                        window.open(siteArray[i], '_blank');
                    }
                } 
            });
            
            Chon();
            function Chon() 
            {
                var sLNS = "";
                $("input:checkbox[check-group='LNS']").each(function (i) 
                {
                    if (this.checked) 
                    {
                        if (sLNS != "") sLNS += ",";
                        sLNS += this.value;
                    }
                });
                Thang = document.getElementById("<%= ParentID %>_iThang_Quy").value;
                var iID_MaNamNganSach = document.getElementById("<%=ParentID %>_iID_MaNamNganSach").value;
                var iID_MaPhongBan = document.getElementById("<%=ParentID %>_iID_MaPhongBan").value;
                jQuery.ajaxSetup({ cache: false });
                var url = unescape('<%= Url.Action("Ds_DonVi?ParentID=#0&Thang_Quy=#1&MaTo=#2&sLNS=#3&iID_MaNamNganSach=#4&iID_MaPhongBan=#5", "rptQuyetToan_TongHop_LNS") %>');
                url = unescape(url.replace("#0", "<%= ParentID %>"));
                url = unescape(url.replace("#1", Thang));
                url = unescape(url.replace("#2","<%= MaTo%>"));
                url = unescape(url.replace("#3",sLNS));
                url = unescape(url.replace("#4", iID_MaNamNganSach));
                url = unescape(url.replace("#5", iID_MaPhongBan));
                $.getJSON(url, function (data) 
                {
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
            <%=MyHtmlHelper.ActionLink(urlExport, "Export To Excel") %>
        </div>
    </div>
    <%} %>
</body>
</html>
