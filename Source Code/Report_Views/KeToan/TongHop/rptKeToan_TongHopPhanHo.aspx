<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QuyetToan" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="Stylesheet" type="text/css" href="../../../Content/Report_Style/ReportCSS.css" />
    <style type="text/css">   
        ul.inlineBlock li fieldset legend{text-align:left; padding:3px 6px;font-size:13px;}  
        ul.inlineBlock li fieldset p{padding:2px 4px; text-align:right;}        
        ul.inlineBlock li fieldset p span:first-child{float:left;}
    </style>
    <script type="text/javascript" language="javascript" src="../../../Scripts/jquery-latest.js"></script>
</head>
<body>
    <% 
        String ParentID = "KeToan";
        String KieuIn = Convert.ToString(ViewData["KieuIn"]);
        String UserID = User.Identity.Name;
        DataTable dtKieuIn = rptKeToan_TongHopPhanHoController.KieuIn();
        SelectOptionList slKieuIn = new SelectOptionList(dtKieuIn, "MaKieuIn", "TenKieuIn");
        if (String.IsNullOrEmpty(KieuIn))
        {
            KieuIn = "1";
        }
        dtKieuIn.Dispose();
        String NamLamViec = Convert.ToString(ViewData["NamLamViec"]);
        if (String.IsNullOrEmpty(NamLamViec))
        {
            NamLamViec = DanhMucModels.NamLamViec(UserID).ToString();
        }
        DateTime dNgayHienTai = DateTime.Now;
        String NamHienTai = Convert.ToString(dNgayHienTai.Year);
        DataTable dtNam = DanhMucModels.DT_Nam();
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        dtNam.Dispose();

        String ThangLamViec = Convert.ToString(ViewData["ThangLamViec"]);
        if (String.IsNullOrEmpty(ThangLamViec))
        {
            ThangLamViec = DanhMucModels.ThangLamViec(UserID).ToString();
        }
        DataTable dtTrangThai = rptQuyetToan_25_5Controller.tbTrangThai();
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "TenTrangThai");
        String iTrangThai = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        if (String.IsNullOrEmpty(iTrangThai))
            iTrangThai = dtTrangThai.Rows.Count > 0 ? Convert.ToString(dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"]) : Guid.Empty.ToString();  
        DataTable dtThang = DanhMucModels.DT_Thang_CoThangKhong();
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        dtThang.Dispose();
        String iID_MaPhuongAn = Convert.ToString(ViewData["iID_MaPhuongAn"]);

        DataTable dtPhuongAn = KeToan_DanhMucThamSoModels.Get_dtDanhSachThamSoCuaBaoCao("rptKeToan_TongHopPhanHoController", NamLamViec);
        SelectOptionList slPhuongAn = new SelectOptionList(dtPhuongAn, "sThamSo", "sThamSo");
        if (String.IsNullOrEmpty(iID_MaPhuongAn))
        {
            if (dtPhuongAn.Rows.Count > 0)
            {
                iID_MaPhuongAn = dtPhuongAn.Rows[0]["sThamSo"].ToString();
            }
            else
            {
                iID_MaPhuongAn = Guid.Empty.ToString();
            }
        }
        dtPhuongAn.Dispose();

        String iID_MaTaiKhoan = Convert.ToString(ViewData["iID_MaTaiKhoan"]);
        DataTable dtTaiKhoan = rptKeToan_TongHopPhanHoController.KeToan_ToIn(iID_MaPhuongAn, KieuIn, iTrangThai);
        SelectOptionList slTaiKhoan = new SelectOptionList(dtTaiKhoan, "MaTo", "TenTo");
        if (String.IsNullOrEmpty(iID_MaTaiKhoan))
        {
            if (dtTaiKhoan.Rows.Count > 0)
            {
                iID_MaTaiKhoan = dtTaiKhoan.Rows[0]["MaTo"].ToString();
            }
            else
            {
                iID_MaTaiKhoan = Guid.Empty.ToString();
            }
        }
        dtTaiKhoan.Dispose();

        String KhoGiay = Convert.ToString(ViewData["KhoGiay"]);
        DataTable dtKhoGiay = ReportModels.LoaiKhoGiay();
        SelectOptionList slKhoGiay = new SelectOptionList(dtKhoGiay, "MaKhoGiay", "TenKhoGiay");
        if (String.IsNullOrEmpty(KhoGiay))
        {
            KhoGiay = "1";
        }
        dtKhoGiay.Dispose();


       String BackURL = Url.Action("SoDoLuong", "KeToanTongHop");
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String UrlReport = "";
        if (PageLoad == "1")
            UrlReport = Url.Action("ViewPDF", "rptKeToan_TongHopPhanHo", new { NamLamViec = NamLamViec, ThangLamViec = ThangLamViec, KieuIn = KieuIn, iID_MaPhuongAn = iID_MaPhuongAn, iID_MaTaiKhoan = iID_MaTaiKhoan, KhoGiay = KhoGiay, iID_MaTrangThaiDuyet = iTrangThai });
        using (Html.BeginForm("EditSubmit", "rptKeToan_TongHopPhanHo", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td width="47.9%"><span>Báo cáo tổng hợp phân hộ</span></td>
                    <td width="52%" style="text-align: left;">
                        <div class="login1" style="width: 50px; height: 20px; text-align: left;"><a style="cursor: pointer;"></a></div>
                    </td>
                </tr>
            </table>
        </div><!---End .title_tong--->
        <div id="table_form2" class="table_form2">
            <div id="rptMain" style="width: 100%;margin: 0px auto; padding: 0px 0px; overflow: visible; text-align:center;">
                <%--<table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td width="10%">
                            &nbsp;
                        </td>
                        <td class="td_form2_td1" style="width: 10%;">
                            Kiểu in ra:&nbsp;
                        </td>
                        <td width="14%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slKieuIn, KieuIn, "KieuIn", "", "class=\"input1_2\" style=\"width: 100%\" onChange=ChonKieuIn()")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" width="10%">
                            Năm làm việc:&nbsp;
                        </td>
                        <td width="8%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slNam, NamLamViec, "NamLamViec", "", "class=\"input1_2\" style=\"width: 100%\"")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" width="10%">
                            Tháng làm việc:&nbsp;
                        </td>
                        <td width="8%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, ThangLamViec, "ThangLamViec", "", "class=\"input1_2\" style=\"width: 100%\"")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" width="10%">
                            Khổ giấy: &nbsp;
                        </td>
                        <td width="10%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slKhoGiay, KhoGiay, "KhoGiay", "", "class=\"input1_2\" style=\"width: 100%\"")%>
                            </div>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td class="td_form2_td1">
                            Phương án:&nbsp;
                        </td>
                        <td colspan="5">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slPhuongAn, iID_MaPhuongAn, "iID_MaPhuongAn", "", "class=\"input1_2\" style=\"width: 100%\" onChange=ChonKieuIn()")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" id="<%= ParentID%>_tdText">
                            <% rptKeToan_TongHopPhanHoController rpt = new rptKeToan_TongHopPhanHoController();
                               rptKeToan_TongHopPhanHoController.KeToan_TongHopPhanHoData _data = new rptKeToan_TongHopPhanHoController.KeToan_TongHopPhanHoData();
                               _data = rpt.obj_data(ParentID, iID_MaPhuongAn, KieuIn, iID_MaTaiKhoan);
                            %>
                            <%= _data.Text %>
                        </td>
                        <td id="<%=ParentID%>_tdNoiDung" width="10%">
                            <%=_data.NoiDung %>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td colspan="4">
                            <table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;"
                                width="100%">
                                <tr>
                                    <td style="width: 49%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="2%">
                                    </td>
                                    <td style="width: 49%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="history.go(-1)" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>--%> 
                <ul class="inlineBlock">
                    <li>
                        <fieldset>
                            <p><span><%=NgonNgu.LayXau("Tháng:") %></span><%=MyHtmlHelper.DropDownList(ParentID, slThang, ThangLamViec, "ThangLamViec", "", "class=\"input1_2\" style=\"width: 70px\"")%></p>
                            <p><span><%=NgonNgu.LayXau("Năm:") %></span><%=MyHtmlHelper.DropDownList(ParentID, slNam, NamLamViec, "NamLamViec", "", "class=\"input1_2\" style=\"width: 70px\"")%></p>
                        </fieldset>                        
                    </li>
                    <li>
                        <fieldset>
                            <p><span><%=NgonNgu.LayXau("Khổ giấy:") %></span><%=MyHtmlHelper.DropDownList(ParentID, slKhoGiay, KhoGiay, "KhoGiay", "", "class=\"input1_2\" style=\"width: 150px\"")%></p>
                        <p><span><%=NgonNgu.LayXau("Kiểu in ra:") %></span><%=MyHtmlHelper.DropDownList(ParentID, slKieuIn, KieuIn, "KieuIn", "", "class=\"input1_2\" style=\"width: 150px\" onChange=ChonKieuIn()")%></p>    
                        </fieldset>                
                    </li>
                    <li>
                        <fieldset>
                            <legend><%=NgonNgu.LayXau("Phương án:") %></legend>
                            <p style="margin-bottom:10px;">
                                <%=MyHtmlHelper.DropDownList(ParentID, slPhuongAn, iID_MaPhuongAn, "iID_MaPhuongAn", "", "class=\"input1_2\" style=\"width: 280px\" onChange=ChonKieuIn()")%>
                            </p>
                        </fieldset>
                    </li>
                    <li>                        
                        <% rptKeToan_TongHopPhanHoController rpt = new rptKeToan_TongHopPhanHoController();
                            rptKeToan_TongHopPhanHoController.KeToan_TongHopPhanHoData _data = new rptKeToan_TongHopPhanHoController.KeToan_TongHopPhanHoData();
                            _data = rpt.obj_data(ParentID, iID_MaPhuongAn, KieuIn, iID_MaTaiKhoan, iTrangThai);
                        %>   
                        <fieldset>
                            <p><span id="<%= ParentID%>_tdText"><%= _data.Text %></span>&nbsp;<span id="<%=ParentID%>_tdNoiDung"><%=_data.NoiDung %></span></p>
                            <p>
                                <span><%=NgonNgu.LayXau("Trạng thái:") %></span>
                                <span><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iTrangThai, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 180px;\"")%></span> 
                            </p>    
                        </fieldset>                 
                    </li>                    
                </ul><!--End .inlineBlock--> 
                <p style="text-align:center; padding:4px;"><input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" style="display:inline-block; margin-right:5px;" /><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>"  onclick="Huy()" style="display:inline-block; margin-left:5px;" /></p>                                
            </div><!--End #rptMain-->            
        </div><!--End #table_form2--> 
    </div><!--End .box_tong--> 
    <script type="text/javascript">
        $(function () {
            $('div.login1 a').click(function () {
                $('div#rptMain').slideToggle('normal');
                $(this).toggleClass('active');
                return false;
            });
        });
        function Huy() {
            window.location.href = '<%=BackURL %>';
        }
    </script>
    <script type="text/javascript">
        function ChonKieuIn() {
            var KieuIn = document.getElementById("<%=ParentID %>_KieuIn").value;
            var iID_MaPhuongAn = document.getElementById("<%=ParentID %>_iID_MaPhuongAn").value;
            var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("ds_NhomDonVi?ParentID=#0&KieuIn=#1&iID_MaPhuongAn=#2&iID_MaTaiKhoan=#3&iID_MaTrangThaiDuyet=#4", "rptKeToan_TongHopPhanHo") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", KieuIn));
            url = unescape(url.replace("#2", iID_MaPhuongAn));
            url = unescape(url.replace("#3", "<%= iID_MaTaiKhoan %>"));
            url = unescape(url.replace("#4", iID_MaTrangThaiDuyet));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_tdText").innerHTML = data.Text;
                document.getElementById("<%= ParentID %>_tdNoiDung").innerHTML = data.NoiDung;
            });
        }                                            
    </script>
    <%} %>
    <iframe src="<%=UrlReport%>"
        height="600px" width="100%"></iframe>
</body>
</html>
