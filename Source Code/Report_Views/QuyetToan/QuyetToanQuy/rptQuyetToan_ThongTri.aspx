<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
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
        
        //Danh sách quý
        DataTable dtQuy = DanhMucModels.DT_Quy_QuyetToan();
        DataRow R1 = dtQuy.NewRow();
        R1["MaQuy"] = "5";
        R1["TenQuy"] = "Bổ sung";
        dtQuy.Rows.Add(R1);
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
        dtQuy.Dispose();

        //Danh sách Năm ngân sách
        String iID_MaNamNganSach = Convert.ToString(ViewData["iID_MaNamNganSach"]);
        DataTable dtNamNganSach = QuyetToanModels.getDSNamNganSach();
        SelectOptionList slNamNganSach = new SelectOptionList(dtNamNganSach, "MaLoai", "sTen");
        dtNamNganSach.Dispose();

        String iThang_Quy = Convert.ToString(ViewData["iThang_Quy"]);
        if (String.IsNullOrEmpty(iThang_Quy))
        {
                iThang_Quy = ReportModels.LayQuyHienTai();
        }

        //Danh sách Loại ngân sách
        String sLNS = Convert.ToString(ViewData["sLNS"]);
        if (String.IsNullOrEmpty(sLNS))
        {
            sLNS = Guid.Empty.ToString();
        }
        
        String iID_MaPhongBan = NganSach_HamChungModels.MaPhongBanCuaMaND(MaND);
        DataTable dtLNS = DanhMucModels.NS_LoaiNganSach_PhongBan(iID_MaPhongBan);
        dtLNS.Dispose();

        //VungNV: 2015/09/21 Danh sách phòng ban
        DataTable dtPhongBan = QuyetToanModels.getDSPhongBan(iNamLamViec, MaND);
        SelectOptionList slPhongBan = new SelectOptionList(dtPhongBan, "iID_MaPhongBan", "sTenPhongBan");
        dtPhongBan.Dispose();
        
        String MaPhongBan = Convert.ToString(ViewData["MaPhongBan"]);
        String LoaiCapPhat = Convert.ToString(ViewData["LoaiCapPhat"]);
        String LoaiThongTri = Convert.ToString(ViewData["LoaiThongTri"]);
        
        //Danh sách loại thông tri
        DataTable dtLoaiThongTri = QuyetToanModels.GetDanhSachLoaiNSQuyetToan_ThongTri();
        SelectOptionList slLoaiThongTri = new SelectOptionList(dtLoaiThongTri, "MaLoai", "sTen");
        if (String.IsNullOrEmpty(LoaiThongTri))
        {
            if (dtLoaiThongTri.Rows.Count > 0)
            {
                LoaiThongTri = Convert.ToString(dtLoaiThongTri.Rows[0]["MaLoai"]);
            }
            else
            {
                LoaiThongTri = Guid.Empty.ToString();
            }
        }
        
        dtLoaiThongTri.Dispose();

        String BackURL = Url.Action("Index", "QuyetToan_Report", new { Loai = 0 });

        String[] arrLNS = sLNS.Split(',');
        
        String LoaiTongHop = Convert.ToString(ViewData["LoaiTongHop"]);
        if (String.IsNullOrEmpty(LoaiTongHop))
        {
            LoaiTongHop = "ChiTiet";
        }
        
        String DenMuc = Convert.ToString(ViewData["DenMuc"]);
        if (String.IsNullOrEmpty(DenMuc))
        {
            DenMuc = "Nganh";
        }
        
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
            if (LoaiTongHop == "ChiTiet")
            {
                for (int i = 0; i < arrMaDonVi.Length; i++ )
                {
                    arrView[i] = String.Format(
                        @"/rptQuyetToanThongTri/viewpdf?iID_MaDonVi={0}&sLNS={1}&iThang_Quy={2}&iID_MaNamNganSach={3}&MaND={4}&LoaiTongHop={5}&DenMuc={6}&LoaiCapPhat={7}&LoaiThongTri={8}",
                        arrMaDonVi[i], sLNS, iThang_Quy, iID_MaNamNganSach, MaND, LoaiTongHop, DenMuc, LoaiCapPhat, LoaiThongTri);
                    Chuoi +=arrView[i];
                   if (i < arrMaDonVi.Length - 1)
                   {
                        Chuoi += "+";
                   }
                       
                }
            }
            //VungNV: 2015/09/23 LoaiTongHop == TongHop
            else {
                arrView = new string[1];
                arrView[0] = String.Format(
                        @"/rptQuyetToanThongTri/viewpdf?iID_MaDonVi={0}&sLNS={1}&iThang_Quy={2}&iID_MaNamNganSach={3}&MaND={4}&LoaiTongHop={5}&DenMuc={6}&LoaiCapPhat={7}&LoaiThongTri={8}",
                        iID_MaDonVi, sLNS, iThang_Quy, iID_MaNamNganSach, MaND, LoaiTongHop, DenMuc, LoaiCapPhat, LoaiThongTri);
                Chuoi += arrView[0];
            }  
        }

        String sGhiChu = Convert.ToString(ViewData["sGhiChu"]);
        int SoCot = 1;

        using (Html.BeginForm("FormSubmit", "rptQuyetToanThongTri", new { ParentID = ParentID }))
    {
    %>
    <%=MyHtmlHelper.Hidden(ParentID, MaND, "MaND", "")%>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo quyết toán thông tri
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
                                <b>Chọn quý </b></div>
                        </td>
                        <td style="width: 25%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, iThang_Quy, "iThang_Quy", "", "class=\"input1_2\" style=\"width:100%;\"onchange=Chon()")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 7%; height: 10px">
                            <b>Chọn đơn vị &nbsp;&nbsp;</b>                         
                        </td>
                        <td style="width: 20%;" rowspan="5">
                            <div class="td_form2_td5" id="<%= ParentID %>_tdDonVi" style="overflow: scroll; height: 260px; width:100%">
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 7%; height: 10px">
                            <div><b>Ghi chú </b></div>
                        </td>
                        <td style="height: 40px; width: 20%" rowspan="5">
                            <div style="height: 250px;" id="<%= ParentID %>_tdGhiChu">
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
                        <td class="td_form2_td1" style="width: 10%; height: 20px">
                            <div>
                                <b>Chọn năm </b></div>
                        </td>
                        <td style="width: 15%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slNamNganSach, iID_MaNamNganSach, "iID_MaNamNganSach", "", "class=\"input1_2\" style=\"width:100%;\"onchange=Chon() ")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%; height: 20px">
                            <div>
                                <b></b>
                            </div>
                        </td>
                        <td>
                        </td>
                        <td style="height: 40px; width: 20%">
                        </td>
                        <td>
                        </td>
                        <td class="td_form2_td1" style="width: 10%; height: 20px">
                            <div>
                                <b></b>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="width: 15%; height: 10px">
                            <div>
                                <b>Loại ngân sách </b></div>
                        </td>
                        <td style="width: 10%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slLoaiThongTri, LoaiThongTri, "LoaiThongTri", "", "class=\"input1_2\" style=\"width:100%;\"onchange=Chon()")%>
                            </div>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <!--Begin: VungNV: 2015/09/21: thêm mới dropdownlist chọn phòng ban -->
                    <tr>
                        <td class="td_form2_td1" style="width: 12%; height: 10px">
                            <div>
                                <b>Chọn phòng ban </b></div>
                        </td>
                        <td style="width: 10%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slPhongBan, MaPhongBan, "iID_MaPhongBan", "", "class=\"input1_2\" style=\"width:100%;\"onchange=Chon()")%>
                            </div>
                        </td>
                    </tr>
                    <!-- End: VungNV: 2015/09/21: thêm mới dropdownlist chọn phòng ban -->
                    <tr>
                        <td class="td_form2_td1" style="width: 10%; height: 10px">
                            <div>
                            <b>Chọn LNS </b>
                            </div>
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
                    <!-- Begin VungNV: 2015/09/25 thêm mới textbox Loại cấp phát-->
                    <tr>
                        <td class="td_form2_td1" style="width: 12%; height: 10px">
                            <div>
                                <b>Ghi chú quyết toán </b></div>
                        </td>
                        <td style="width: 20%; height: 10px">
                            <div >
                             <%=MyHtmlHelper.TextBox(ParentID, LoaiCapPhat, "LoaiCapPhat","", "class=\"input1_2\" style=\"width:100%;\"onchange=CapNhatLoaiQuyetToan()")%>
                            </div>
                        </td>
                    </tr>
                    <!-- End VungNV: 2015/09/25 thêm mới textbox Loại cấp phát-->
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
               
                //VungNV: 2015/09/30 Lấy giá trị loại quyết toán
                    jQuery.ajaxSetup({cache: false});
                    var url2 = unescape('<%= Url.Action("LayLoaiQuyetToan?ParentID=#0&MaND=#1","rptQuyetToanThongTri") %>');
                    url2 = unescape(url2.replace("#0", "<%= ParentID %>"));
                    url2 = unescape(url2.replace("#1", "<%= MaND %>"));

                    $.getJSON(url2, function(data){
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
                
                var url = unescape('<%= Url.Action("CapNhatGhiChuQuyetToan?sGhiChu=#0&MaND=#1&iID_MaDonVi=#2", "rptQuyetToanThongTri") %>');
                url = unescape(url.replace("#0",sGhiChu));
                url = unescape(url.replace("#1","<%=MaND %>"));
                url = unescape(url.replace("#2", iID_MaDonVi));

                $.getJSON(url, function () {
                   
                });
            }

            //VungNV: 2015/09/26 thêm mới hoặc udpate ghi chú vào bảng QTA.GhiChu 
            function CapNhatLoaiQuyetToan() {
                
                var sLoaiQuyetToan = document.getElementById("<%= ParentID %>_LoaiCapPhat").value.trim();

                jQuery.ajaxSetup({ cache: false });
                var url = unescape('<%= Url.Action("CapNhatLoaiQuyetToan?sLoaiQuyetToan=#0&MaND=#1", "rptQuyetToanThongTri") %>');
                url = unescape(url.replace("#0",sLoaiQuyetToan));
                url = unescape(url.replace("#1","<%=MaND %>"));

                $.getJSON(url, function () {});
            }
            

            Chon();
            //CHon loai thong tri
            function Chon() {
                  
                var Thang = document.getElementById("<%= ParentID %>_iThang_Quy").value;
                var iID_MaNamNganSach = document.getElementById("<%=ParentID %>_iID_MaNamNganSach").value;
                var LoaiThongTri = document.getElementById("<%=ParentID %>_LoaiThongTri").value;
                var MaPhongBan = document.getElementById("<%=ParentID %>_iID_MaPhongBan").value;

                jQuery.ajaxSetup({ cache: false });
               
                var url = unescape('<%= Url.Action("LayDanhSachLNS?ParentID=#0&Thang_Quy=#1&LoaiThongTri=#2&sLNS=#3&iID_MaNamNganSach=#4&iID_MaPhongBan=#5", "rptQuyetToanThongTri") %>');
                url = unescape(url.replace("#0", "<%= ParentID %>"));
                url = unescape(url.replace("#1", Thang));
                url = unescape(url.replace("#2",LoaiThongTri));
                url = unescape(url.replace("#3","<%=sLNS %>"));
                url = unescape(url.replace("#4", iID_MaNamNganSach));
                url = unescape(url.replace("#5", MaPhongBan));
                
                $.getJSON(url, function (data) {
                    document.getElementById("<%= ParentID %>_tdLNS").innerHTML = data;
                    ChonLNS();
                });
              
            }
            //chon LNS
            function ChonLNS() {
                  var sLNS = "";
             $("input:checkbox[check-group='LNS']").each(function (i) {
                 if (this.checked) {
                     if (sLNS != "") sLNS += ",";
                     sLNS += this.value;
                 }
             });
                var Thang = document.getElementById("<%= ParentID %>_iThang_Quy").value;
                var iID_MaNamNganSach = document.getElementById("<%=ParentID %>_iID_MaNamNganSach").value;
                var LoaiThongTri = document.getElementById("<%=ParentID %>_LoaiThongTri").value;
                var MaPhongBan = document.getElementById("<%=ParentID %>_iID_MaPhongBan").value;

                jQuery.ajaxSetup({ cache: false });
                var url = unescape('<%= Url.Action("LayDanhSachDonVi?ParentID=#0&Thang_Quy=#1&LoaiThongTri=#2&sLNS=#3&iID_MaNamNganSach=#4&iID_MaDonVi=#5&iID_MaPhongBan=#6", "rptQuyetToanThongTri") %>');
                url = unescape(url.replace("#0", "<%= ParentID %>"));
                url = unescape(url.replace("#1", Thang));
                url = unescape(url.replace("#2",LoaiThongTri));
                url = unescape(url.replace("#3",sLNS));
                url = unescape(url.replace("#4", iID_MaNamNganSach));
                url = unescape(url.replace("#5", "<%= iID_MaDonVi%>"));
                url = unescape(url.replace("#6",MaPhongBan));

                $.getJSON(url, function (data) {
                    document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data;
                });
            }
            //ghi chu
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
                var url = unescape('<%= Url.Action("LayGhiChuQuyetToan?ParentID=#0&MaND=#1&iID_MaDonVi=#2", "rptQuyetToanThongTri") %>');
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
