<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Models.CapPhat" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript" src="../../../Scripts/jquery-latest.js"></script>
</head>
<body>
    <%
    String ParentID = "CapPhatNganSach";
    String MaND = User.Identity.Name;
    String iNamLamViec = ReportModels.LayNamLamViec(MaND);
        
    //VungNV: 2015/09/28 Danh sách Đợt cấp phát
    DataTable dtDot = CapPhat_ReportModels.LayDotCapPhat(MaND);
    SelectOptionList slDot = new SelectOptionList(dtDot,"iNamCapPhat", "iNamCapPhat");
    dtDot.Dispose();

    String iNamCapPhat = Convert.ToString(ViewData["iNamCapPhat"]); 
              
    //Danh sách Loại ngân sách
    String sLNS = Convert.ToString(ViewData["sLNS"]);

    //VungNV: 2015/09/28 Danh sách phòng ban
    DataTable dtPhongBan = CapPhat_ReportModels.LayDSPhongBan(MaND);
    SelectOptionList slPhongBan = new SelectOptionList(dtPhongBan, "iID_MaPhongBan", "sTenPhongBan");
    dtPhongBan.Dispose();
    
    //Lấy mã phòng ban từ ViewData
    String MaPhongBan = Convert.ToString(ViewData["MaPhongBan"]);

    //VungNV: 2015/09/25 Lấy giá trị của loại cấp phát từ ViewData
    String LoaiCapPhat = Convert.ToString(ViewData["LoaiCapPhat"]);
    
    // VungNV: 2015/09/23 Lấy giá trị của loại thông trị từ ViewData
    String LoaiThongTri = Convert.ToString(ViewData["LoaiThongTri"]);
    DataTable dtLoaiThongTri  = CommonFunction.Lay_dtDanhMuc("LoaiCapPhat");
 
    SelectOptionList slLoaiThongTri = new SelectOptionList(dtLoaiThongTri, "iID_MaDanhMuc", "sTen");
    
    if (String.IsNullOrEmpty(LoaiThongTri))
    {
        LoaiThongTri = dtLoaiThongTri.Rows.Count > 0 ? Convert.ToString(dtLoaiThongTri.Rows[0]["iID_MaDanhMuc"]) : Guid.Empty.ToString();
    }
        
    dtLoaiThongTri.Dispose();

    String BackURL = Url.Action("Index", "CapPhat_Report", new { Loai = 0 });

    //VungNV: 2015/09/23 Lấy loại báo cáo từ ViewData    
    String LoaiTongHop = Convert.ToString(ViewData["LoaiTongHop"]);
    if (String.IsNullOrEmpty(LoaiTongHop))
    {
        LoaiTongHop = "ChiTiet";
    }
        
    //VungNV: 2015/09/23 Lấy loại báo cáo chi tiết từ ViewData
    String DenMuc = Convert.ToString(ViewData["DenMuc"]);
    if (String.IsNullOrEmpty(DenMuc))
    {
        DenMuc = "Nganh";
    }
    
    //VungNV: Lấy mã đơn vị từ ViewData    
    String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
    String[] arrMaDonVi = iID_MaDonVi.Split(',');
    String[] arrView = new String[arrMaDonVi.Length];
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
        //VungNV: 2015/09/30 Chọn hiện thị chi tiết
        if (LoaiTongHop == "ChiTiet")
        {
            for (int i = 0; i < arrMaDonVi.Length; i++ )
            {
                arrView[i] = String.Format(
                    @"/rptCapPhat_ThongTri/viewpdf?iID_MaDonVi={0}&sLNS={1}&iNamCapPhat={2}&MaND={3}&LoaiTongHop={4}&DenMuc={5}&LoaiCapPhat={6}&LoaiThongTri={7}",
                    arrMaDonVi[i], sLNS, iNamCapPhat, MaND, LoaiTongHop, DenMuc, LoaiCapPhat, LoaiThongTri);
                Chuoi +=arrView[i];
               if (i < arrMaDonVi.Length - 1)
               {
                    Chuoi += "+";
               }            
            }
        }
        //VungNV: 2015/09/30 Chọn hiện thị tổng hợp
        else {
            arrView = new string[1];
            arrView[0] = String.Format(
                    @"/rptCapPhat_ThongTri/viewpdf?iID_MaDonVi={0}&sLNS={1}&iNamCapPhat={2}&MaND={3}&LoaiTongHop={4}&DenMuc={5}&LoaiCapPhat={6}&LoaiThongTri={7}",
                    iID_MaDonVi, sLNS, iNamCapPhat, MaND, LoaiTongHop, DenMuc, LoaiCapPhat, LoaiThongTri);
            Chuoi += arrView[0];
        }  
    }

    String sGhiChu = Convert.ToString(ViewData["sGhiChu"]);

    using (Html.BeginForm("EditSubmit", "rptCapPhat_ThongTri", new { ParentID = ParentID }))
    {
    %>
    <%=MyHtmlHelper.Hidden(ParentID, MaND, "MaND", "")%>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo tổng hợp tình hình cấp phát ngân sách năm
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
                                <b>Chọn đợt cấp phát</b></div>
                        </td>
                        <td style="width: 25%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slDot, iNamCapPhat, "iNamCapPhat", "", "class=\"input1_2\" style=\"width:100%;\"onchange=Chon()")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%; height: 10px">
                            <b>Chọn đơn vị</b>
                        </td>
                        <td  style="width: 20%;" rowspan="5">
                            <div class="td_form2_td5" id="<%= ParentID %>_tdDonVi" style="overflow: scroll; height: 260px; width: 100%">
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 7%; height: 10px">
                            <b>Ghi chú  </b>
                        </td>
                        <td style="height: 40px; width: 20%" rowspan="5">
                            <div style="height: 250px; " id="<%= ParentID %>_tdGhiChu">
                            </div>
                            <div style="color: red; font-size: medium; padding-left: 5px; padding-bottom: 10px">
                                <%=MyHtmlHelper.Option(ParentID, "ChiTiet", LoaiTongHop, "LoaiTongHop", "", "")%>
                                Chi tiết ĐV
                                <%=MyHtmlHelper.Option(ParentID, "TongHop", LoaiTongHop, "LoaiTongHop", "", "")%>
                                Tổng hợp ĐV
                            </div>
                            <div style="color: red; font-size: medium; padding-left: 5px">
                              
                                <%=MyHtmlHelper.Option(ParentID, "Nganh", DenMuc, "DenMuc", "", "")%>
                               Đến Ngành
                                 <%=MyHtmlHelper.Option(ParentID, "Muc", DenMuc, "DenMuc", "", "")%>
                              Đến Mục
                            </div>
                        </td>
                        <td>
                        </td>
                    </tr>

                    <tr>
                        <td class="td_form2_td1" style="width: 12%; height: 10px">
                            <div>
                                <b>Loại cấp phát</b></div>
                        </td>
                        <td style="width: 10%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slLoaiThongTri, LoaiThongTri, "LoaiThongTri", "", "class=\"input1_2\" style=\"width:100%;\"onchange=Chon()")%>
                            </div>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="width: 12%; height: 10px">
                            <div>
                                <b>Chọn phòng ban</b></div>
                        </td>
                        <td style="width: 10%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slPhongBan, MaPhongBan, "iID_MaPhongBan", "", "class=\"input1_2\" style=\"width:100%;\"onchange=Chon()")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="width: 10%; height: 10px">
                            <b>Chọn loại ngân sách</b>
                        </td>
                        <td class="td_form2_td5" style="width: 15%;">
                            <div id="<%= ParentID %>_tdLNS" style="overflow: scroll; height: 200px">
                            </div>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="width: 12%; height: 10px">
                            <div>
                                <b>Ghi chú cấp phát</b></div>
                        </td>
                        <td style="width: 12%; height: 10px">
                            <div >
                             <%=MyHtmlHelper.TextBox(ParentID, LoaiCapPhat, "LoaiCapPhat","","style=\"width:100%; resize: none\" onchange=update_LoaiCapPhat()")%>
                            </div>
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
            function CheckAllLNS(value) {
                $("input:checkbox[check-group='LNS']").each(function (i) {
                    this.checked = value;
                });
                ChonLNS();
            }

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
                //VungNV: Lấy giá trị loại cấp phát
                    jQuery.ajaxSetup({cache: false});
                    var url = unescape('<%= Url.Action("LayLoaiCapPhat?ParentID=#0&MaND=#1","rptCapPhat_ThongTri") %>');
                    url = unescape(url.replace("#0", "<%= ParentID %>"));
                    url = unescape(url.replace("#1", "<%= MaND %>"));
                    
                    $.getJSON(url, function(data){
	                    document.getElementById("<%= ParentID %>_LoaiCapPhat").value = data;
                    });
                });

            function changeTest(value) {

                var sGhiChu=document.getElementById("<%= ParentID %>_sGhiChu").value;
                var arrGhiChu = sGhiChu.split('\n');
                sGhiChu = "";
                for (var i = 0; i < arrGhiChu.length; i++) {
                    sGhiChu += arrGhiChu[i] + '^';
                }
                var url = unescape('<%= Url.Action("CapNhapGhiChu?sGhiChu=#0&MaND=#1&iID_MaDonVi=#2", "rptCapPhat_ThongTri") %>');
                url = unescape(url.replace("#0",sGhiChu));
                url = unescape(url.replace("#1","<%=MaND %>"));
                url = unescape(url.replace("#2", iID_MaDonVi));

                $.getJSON(url, function (data) {
                });
            }

            //VungNV: 2015/09/30 Thêm mới hoặc cập nhật giá trị vào bảng QTA.GhiChu
            function update_LoaiCapPhat() {
                
                var LoaiCapPhat = document.getElementById("<%= ParentID %>_LoaiCapPhat").value.trim();

                jQuery.ajaxSetup({ cache: false });
                var url = unescape('<%= Url.Action("CapNhatLoaiCapPhat?LoaiCapPhat=#0&MaND=#1", "rptCapPhat_ThongTri") %>');
                url = unescape(url.replace("#0",LoaiCapPhat));
                url = unescape(url.replace("#1","<%=MaND %>"));

                $.getJSON(url, function (data) {});
            }
            

            Chon();
            //VungNV: 2015/09/26 Hiển thị danh sách LNS
            function Chon() {
                  
                var NamCapPhat = document.getElementById("<%= ParentID %>_iNamCapPhat").value;
                var LoaiThongTri = document.getElementById("<%=ParentID %>_LoaiThongTri").value;
                var MaPhongBan = document.getElementById("<%=ParentID %>_iID_MaPhongBan").value;

                jQuery.ajaxSetup({ cache: false });
               
                var url = unescape('<%= Url.Action("LayDanhSachLNS?ParentID=#0&iNamCapPhat=#1&LoaiThongTri=#2&sLNS=#3&iID_MaPhongBan=#4", "rptCapPhat_ThongTri") %>');
                url = unescape(url.replace("#0", "<%= ParentID %>"));
                url = unescape(url.replace("#1", NamCapPhat));
                url = unescape(url.replace("#2",LoaiThongTri));
                url = unescape(url.replace("#3","<%=sLNS %>"));
                url = unescape(url.replace("#4", MaPhongBan));
                
                $.getJSON(url, function (data) {
                    document.getElementById("<%= ParentID %>_tdLNS").innerHTML = data;
                    ChonLNS();
                });
              
            }
            //VungNV: 2015/09/30 Khi chọn LNS thì sẽ hiển thị danh sách đơn vị
            function ChonLNS() {

                 var sLNS = "";
                 $("input:checkbox[check-group='LNS']").each(function (i) {
                     if (this.checked) {
                         if (sLNS != "") sLNS += ",";
                         sLNS += this.value;
                     }
                 });

                var NamCapPhat = document.getElementById("<%= ParentID %>_iNamCapPhat").value;
                var LoaiThongTri = document.getElementById("<%=ParentID %>_LoaiThongTri").value;
                var MaPhongBan = document.getElementById("<%=ParentID %>_iID_MaPhongBan").value;

                jQuery.ajaxSetup({ cache: false });
                var url = unescape('<%= Url.Action("LayDanhSachDonVi?ParentID=#0&iNamCapPhat=#1&LoaiThongTri=#2&sLNS=#3&iID_MaDonVi=#4&iID_MaPhongBan=#5", "rptCapPhat_ThongTri") %>');
                url = unescape(url.replace("#0", "<%= ParentID %>"));
                url = unescape(url.replace("#1", NamCapPhat));
                url = unescape(url.replace("#2",LoaiThongTri));
                url = unescape(url.replace("#3",sLNS));
                url = unescape(url.replace("#4", "<%= iID_MaDonVi%>"));
                url = unescape(url.replace("#5",MaPhongBan));

                $.getJSON(url, function (data) {
                    document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data;
                });
            }
            
             var iID_MaDonVi = "";
              function GhiChu(value) {
                 
                  var check = value.checked;
                  if(check)
                      iID_MaDonVi=value.value;
                  else {
                      iID_MaDonVi = "-1";
                  }
                 //VungNV: Lấy giá trị ghi chú
                jQuery.ajaxSetup({ cache: false });
                var url = unescape('<%= Url.Action("LayGhiChu?ParentID=#0&MaND=#1&iID_MaDonVi=#2", "rptCapPhat_ThongTri") %>');
                url = unescape(url.replace("#0", "<%= ParentID %>"));
                url = unescape(url.replace("#1", "<%= MaND %>"));
                url = unescape(url.replace("#2",iID_MaDonVi));

                $.getJSON(url, function (data) {
                    document.getElementById("<%= ParentID %>_tdGhiChu").innerHTML = data;
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
