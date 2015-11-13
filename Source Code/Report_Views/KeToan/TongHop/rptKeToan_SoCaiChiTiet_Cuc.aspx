<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        ul.inlineBlock{
	        list-style: none inside;			
        }
        ul.inlineBlock li{			
	        /*-webkit-box-shadow: 2px 2px 0 #cecece;
	        box-shadow: 2px 2px 0 #cecece;	*/	
	        -webkit-box-shadow: rgba(200, 200, 200, 0.7) 0 4px 10px -1px;
            box-shadow: rgba(200, 200, 200, 0.7) 0 4px 10px -1px;
	        padding: 2px 5px;
	        display: inline-block;
	        vertical-align: middle; /*Mở comment để xem thuộc tính vertical-align*/
	        margin-right: 3px;
	        margin-left: 0px;
	        font-size: 13px;			
	        border-radius: 3px;
	        position: relative;
	    /*fix for IE 7*/
	        zoom:1;
	        *display: inline;		        
        }
        ul.inlineBlock li span
        {
            padding:2px 1px;    
            height:50px;
        }
        div.login1 {
            text-align : center;    
            background: transparent url(/Content/Report_Image/login.gif) no-repeat top center;
        }    
        div.login1 a {
            color: white;
            text-decoration: none;
            font: bold 16px "Museo 700";
            display: block;
            width: 50px; height: 20px;
            line-height: 20px;
            margin: 0px auto;
            background: transparent url(/Content/Report_Image/arrow.png) no-repeat 20px -29px;
            -webkit-border-radius:2px;
            border-radius:2px;
        }    
        div.login1 a.active {
            background-position: 20px 1px;
        }
        div.login1 a:active, a:focus {
            outline: none;
        }
        .errorafter
        {
           background-color:Yellow;
        }        
        ul.inlineBlock li fieldset .div
        {
            width:90px; height:23px; display:inline-block; text-align:center; padding:1px; font-size:14px;-moz-border-radius:3px;-webkit-border-radius:3px; border-radius:3px;
            font-family:Tahoma Arial; color:Black; line-height:23px; cursor:pointer; background-color:#dedede;  
        }
    </style>
    <script type="text/javascript" language="javascript" src="../../../Scripts/jquery-latest.js"></script>
</head>
<body>
    <% 
        String ParentID = "KeToan";
        DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
        String NamLamViec = Convert.ToString(ViewData["NamLamViec"]);
        if (String.IsNullOrEmpty(NamLamViec))
        {
            NamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
        }
        DateTime dNgayHienTai = DateTime.Now;
        String NamHienTai = Convert.ToString(dNgayHienTai.Year);
        DataTable dtNam = DanhMucModels.DT_Nam();
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        dtNam.Dispose();

        String ThangLamViec = Convert.ToString(ViewData["ThangLamViec"]);
        if (String.IsNullOrEmpty(ThangLamViec))
        {
            ThangLamViec = dtCauHinh.Rows[0]["iThangLamViec"].ToString();
        }
        DataTable dtThang = DanhMucModels.DT_Thang_CoThangKhong();
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        dtThang.Dispose();
        String iID_MaPhuongAn = Convert.ToString(ViewData["iID_MaPhuongAn"]);

        DataTable dtPhuongAn = KeToan_DanhMucThamSoModels.Get_dtDanhSachThamSoCuaBaoCao("rptKeToan_SoCaiChiTietController", NamLamViec);
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

        String iID_MaTaiKhoan =Convert.ToString(ViewData["iID_MaTaiKhoan"]);
        DataTable dtTaiKhoan = rptKeToan_SoCaiChiTiet_CucController.KeToan_ToIn(iID_MaPhuongAn);
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
        String URL = Url.Action("SoDoLuong", "KeToanTongHop");
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String UrlReport = "";
        if (PageLoad == "1")
            UrlReport = Url.Action("ViewPDF", "rptKeToan_SoCaiChiTiet_Cuc", new { NamLamViec = NamLamViec, ThangLamViec = ThangLamViec, iID_MaPhuongAn = iID_MaPhuongAn, iID_MaTaiKhoan = iID_MaTaiKhoan, KhoGiay = KhoGiay });
        using (Html.BeginForm("EditSubmit", "rptKeToan_SoCaiChiTiet_Cuc", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td width="47.9%">
                        <span>Báo cáo sổ cái chi tiết (Phương án thiết lập <a href="../../../KeToanTongHop_ThamSo">tại đây</a>)</span>
                    </td>
                    <td width="52%" style="text-align: left;">
                       <%-- <div class="login1" style="width: 50px; height: 20px; text-align: left;">
                            <a style="cursor: pointer;"></a>
                        </div>--%>
                    </td>
                </tr>
            </table>
        </div>
        <div id="table_form2" class="table_form2"  style="margin-top: 5px;">
            <div id="rptMain" style="width: 100%;  margin: 0px auto; padding: 0px 0px;
                overflow: visible;">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td width="5%">
                            &nbsp;
                        </td>
                        <td class="td_form2_td1" style="width: 10%;">
                            Năm làm việc:&nbsp;
                        </td>
                        <td width="10%">
                            <%=MyHtmlHelper.DropDownList(ParentID, slNam, NamLamViec, "NamLamViec", "", "class=\"input1_2\" style=\"width: 100%\" disabled=\"disabled\"")%>
                        </td>
                        <td class="td_form2_td1" width="10%">
                            Tháng làm việc:
                        </td>
                        <td width="10%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, ThangLamViec, "ThangLamViec", "", "class=\"input1_2\" style=\"width: 100%\"")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" width="10%">
                            Khổ giấy:
                        </td>
                        <td width="10%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slKhoGiay, KhoGiay, "KhoGiay", "", "class=\"input1_2\" style=\"width: 100%\"")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" width="10%">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td class="td_form2_td1">
                            Phương án:&nbsp;
                        </td>
                        <td colspan="3">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slPhuongAn, iID_MaPhuongAn, "iID_MaPhuongAn", "", "class=\"input1_2\" style=\"width: 100%\" onChange=ChonKieuIn()")%>
                            </div>
                        </td>
                        <td class="td_form2_td1">
                            Chọn tờ:&nbsp;
                        </td>
                        <td id="<%=ParentID%>_tdNoiDung" width="10%">
                            <% rptKeToan_SoCaiChiTiet_CucController rpt1 = new rptKeToan_SoCaiChiTiet_CucController();%>
                            <%=rpt1.obj_data(ParentID,iID_MaPhuongAn,iID_MaTaiKhoan) %>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan="6">
                            <table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;"
                                width="100%">
                                <tr>
                                    <td style="width: 49%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="2%">
                                    </td>
                                    <td style="width: 49%;" align="left">
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
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=URL %>';
        }
//        $(function () {
//            $("ul.inlineBlock li span #<%=ParentID %>_LoaiBaoCao").css({ 'border-color': '#cecece' });
//            $("div#rptMain").hide();
//            $('div.login1 a').click(function () {
//                $('div#rptMain').slideToggle('normal');
//                $(this).toggleClass('active');
//                return false;
//            });
//            $("div.div").bind('click', function () {
//                $(this).children().removeAttr("checked", "checked").attr("checked", "checked");
//                ChonReport();
//                $('.div').css('background', '#dedede');
//                $(this).css("background", "#F7F570");
//            });
//        });
       
    </script>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptKeToan_SoCaiChiTiet_Cuc", new { NamLamViec = NamLamViec, ThangLamViec = ThangLamViec, iID_MaPhuongAn = iID_MaPhuongAn, iID_MaTaiKhoan = iID_MaTaiKhoan, KhoGiay = KhoGiay }), "Xuất ra Excel")%>
    <script type="text/javascript">
        function ChonKieuIn() {
            var iID_MaPhuongAn = document.getElementById("<%=ParentID %>_iID_MaPhuongAn").value;
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("ds_NhomDonVi?ParentID=#0&iID_MaPhuongAn=#1&iID_MaTaiKhoan=#2", "rptKeToan_SoCaiChiTiet_Cuc") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", iID_MaPhuongAn));
            url = unescape(url.replace("#2", "<%= iID_MaTaiKhoan %>"));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_tdNoiDung").innerHTML = data;
            });
        }                                            
    </script>
    <%} %>
    <iframe src="<%=UrlReport%>"
        height="600px" width="100%"></iframe>
</body>
</html>
