<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Models.DuToanBS" %>

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
        
        //lấy đợt từ ngày : ... đến ngày :...
        String iID_MaDotTu = Convert.ToString(ViewData["iID_MaDotTu"]);
        String iID_MaDotDen = Convert.ToString(ViewData["iID_MaDotDen"]);        
        DataTable dtDot = DuToanBS_ReportModels.LayDSDot(iNamLamViec, MaND);
        SelectOptionList slDot = new SelectOptionList(dtDot, "iDotCap", "iDotCap");
        
        dtDot.Dispose();
                
        //dt Danh sách phòng ban
        String iID_MaPhongBan = Convert.ToString(ViewData["iID_MaPhongBan"]);
        DataTable dtPhongBan = DuToanBS_ReportModels.LayDSPhongBan(iNamLamViec, MaND);
        SelectOptionList slPhongBan = new SelectOptionList(dtPhongBan, "iID_MaPhongBan", "sTenPhongBan");
        dtPhongBan.Dispose();
        
        //dt Loại ngân sách
        String sLNS = Convert.ToString(ViewData["sLNS"]);
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        
        //loai bao cao
        String LoaiBaoCao = Convert.ToString(ViewData["LoaiTongHop"]);
        if (String.IsNullOrEmpty(LoaiBaoCao)) LoaiBaoCao = "ChiTiet";
        
        String BackURL = Url.Action("Index", "DuToanBS_Report", new { Loai = 0 });

        String[] arrDonVi = iID_MaDonVi.Split(',');
        String[] arrView = new String[arrDonVi.Length];
        String Chuoi = "";
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        
        if (String.IsNullOrEmpty(PageLoad))
            PageLoad = "0";

        //Nếu không chọn loại ngân sách thì không cho xuất báo cáo
        if (String.IsNullOrEmpty(sLNS))
        {
            PageLoad = "0";
            sLNS = Guid.Empty.ToString();
        }

        //Nếu không chọn đơn vị không cho xuất báo cáo
        if (String.IsNullOrEmpty(iID_MaDonVi))
        {
            PageLoad = "0";
        } 
        
        if (PageLoad == "1")
        {
            if (LoaiBaoCao == "ChiTiet")
            {
                for (int i = 0; i < arrDonVi.Length; i++)
                {
                    arrView[i] =
                        String.Format(
                            @"/rptDuToanBS_TongHop_ChiNganSach/viewpdf?iID_MaDonVi={0}&sLNS={1}&iID_MaDotTu={2}&iID_MaDotDen={3}&iID_MaPhongBan={4}&MaND={5}&LoaiBaoCao={6}",
                            arrDonVi[i], sLNS, iID_MaDotTu, iID_MaDotDen, iID_MaPhongBan, MaND,LoaiBaoCao);
                    Chuoi += arrView[i];
                    if (i < arrDonVi.Length - 1) {
                        Chuoi += "+";
                    }
                }
            }
            else
            {
                arrView = new string[1];
                arrView[0] =
                    String.Format(
                        @"/rptDuToanBS_TongHop_ChiNganSach/viewpdf?iID_MaDonVi={0}&sLNS={1}&iID_MaDotTu={2}&iID_MaDotDen={3}&iID_MaPhongBan={4}&MaND={5}&LoaiBaoCao={6}",
                        iID_MaDonVi, sLNS, iID_MaDotTu, iID_MaDotDen, iID_MaPhongBan, MaND,LoaiBaoCao);
                Chuoi += arrView[0];
            }
        }

        int SoCot = 1;

        using (Html.BeginForm("FormSubmit", "rptDuToanBS_TongHop_ChiNganSach", new { ParentID = ParentID }))
        {
    %>
   
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>BÁO CÁO TỔNG HỢP DỰ TOÁN CHI NGÂN SÁCH NĂM </span>

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
                                <b>Chọn Đợt Từ :</b></div>
                        </td>
                        <td class="td_form2_td5" style="width: 14%; height: 20px">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slDot, iID_MaDotTu, "iID_MaDotTu", "", "class=\"input1_2\" style=\"width:100%;\"onchange=Chon()")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width :10% ; height : 20px">
                            <div><b>Chọn Đơn vị : </b></div>
                        </td>
                         <td class="td_form2_td5" rowspan="25" style="width: 20%;">
                             <div id="<%= ParentID %>_tdDonVi" style="overflow: scroll; height: 400px">
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width : 10%; height:20px">
                            <div><b>Loại Ngân Sách:</b></div>
                        </td>
                       <td class="td_form2_td5" rowspan="25" style="width: 20%;">
                            <div id="<%= ParentID %>_tdLNS" style="overflow: scroll; height: 400px">
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 5%; height: 20px">
                            <div>
                                <b></b>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="width: 10%; height: 20px">
                            <div>
                                <b>Đến Ngày :</b></div>
                        </td>
                        <td class="td_form2_td5" style="width: 14%; height: 20px">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slDot, iID_MaDotDen, "iID_MaDotDen", "", "class=\"input1_2\" style=\"width:100%;\"onchange=Chon()")%>
                            </div>
                        </td>                    
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="width: 10%; height: 20px">
                         <div>
                                <b>Chọn phòng ban :</b>
                         </div>
                        </td>
                        <td class="td_form2_td5" style="width: 14%; height: 20px">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slPhongBan, iID_MaPhongBan, "iID_MaPhongBan", "", "class=\"input1_2\" style=\"width:100%;\"onchange=Chon() ")%>
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
                        <td style="height: 40px" colspan="2">
                            <div style="color: red; font-size: medium">
                                <%=MyHtmlHelper.Option(ParentID, "ChiTiet", LoaiBaoCao, "LoaiTongHop", "", "onchange=\" Chon()\"")%>
                                Chi tiết đơn vị
                            </div>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td style="height: 20px" colspan="2">
                            <div style="color: red; font-size: medium">
                                <%=MyHtmlHelper.Option(ParentID, "TongHop", LoaiBaoCao, "LoaiTongHop", "", "")%>
                                Tổng hợp đơn vị
                            </div>
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
            function CheckAllDonVi(value) {
                $("input:checkbox[check-group='DV']").each(function (i) {
                    this.checked = value;
                });
                ChonDonVi();
            }                                            
        </script>
        
        <script type="text/javascript">
            function CheckAll(value) {
                $("input:checkbox[check-group='LNS']").each(function (i) {
                    this.checked = value;
                });
            }                                            
        </script>
        
        <script type="text/javascript">
            function ChonDonVi() {
                
                var iID_MaDotTu = document.getElementById("<%=ParentID %>_iID_MaDotTu").value;
                var iID_MaDotDen = document.getElementById("<%=ParentID %>_iID_MaDotDen").value;
                var MaPhongBan = document.getElementById("<%=ParentID %>_iID_MaPhongBan").value;
                var iID_MaDonVi = "";
                $("input:checkbox[check-group='DV']").each(function (i) {
                    if (this.checked) {
                        if (iID_MaDonVi != "") iID_MaDonVi += ",";
                        iID_MaDonVi += this.value;
                    }
                });
                
                jQuery.ajaxSetup({ cache: false });
                var url = unescape('<%= Url.Action("LayDanhSachLNS?ParentID=#0&iID_MaDotTu=#1&iID_MaDotDen=#2&iID_MaDonVi=#3&sLNS=#4&iID_MaPhongBan=#5", "rptDuToanBS_TongHop_ChiNganSach") %>');
                url = unescape(url.replace("#0", "<%= ParentID %>"));
                url = unescape(url.replace("#1", iID_MaDotTu));
                url = unescape(url.replace("#2", iID_MaDotDen));
                url = unescape(url.replace("#3", iID_MaDonVi));
                url = unescape(url.replace("#4", "<%= sLNS %>"));
                url = unescape(url.replace("#5", MaPhongBan));
                $.getJSON(url, function (data) {
                    document.getElementById("<%= ParentID %>_tdLNS").innerHTML = data;
                });
            }

            Chon();

            function Chon() {
                var iID_MaDonVi = '<%=iID_MaDonVi %>';
                $("input:checkbox[check-group='DV']").each(function (i) {
                    if (this.checked) {
                        if (iID_MaDonVi != "") iID_MaDonVi += ",";
                        iID_MaDonVi += this.value;
                    }
                });
                var iID_MaDotTu = document.getElementById("<%=ParentID %>_iID_MaDotTu").value;
                var iID_MaDotDen = document.getElementById("<%=ParentID %>_iID_MaDotDen").value;
                var MaPhongBan = document.getElementById("<%=ParentID %>_iID_MaPhongBan").value;

                jQuery.ajaxSetup({ cache: false });

                var url = unescape('<%= Url.Action("LayDanhSachDonVi?ParentID=#0&iID_MaDotTu=#1&iID_MaDotDen=#2&iID_MaDonVi=#3&iID_MaPhongBan=#4", "rptDuToanBS_TongHop_ChiNganSach") %>');
                url = unescape(url.replace("#0", "<%= ParentID %>"));
                url = unescape(url.replace("#1", iID_MaDotTu));
                url = unescape(url.replace("#2", iID_MaDotDen));
                url = unescape(url.replace("#3", iID_MaDonVi));
                url = unescape(url.replace("#4", MaPhongBan));
                $.getJSON(url, function (data) {
                    document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data;
                    ChonDonVi();
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
