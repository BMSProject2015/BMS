<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QuyetToan" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.TongHop" %>
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
        }
        ul.inlineBlock li p{
            padding:1px;    
        }
        ul.inlineBlock li p span
        {
            display:inline-block;
            width:150px;
            }
    </style>
    <script type="text/javascript" src="../../../Scripts/jquery-latest.js"></script>
</head>
<body>
    <%
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "KeToan";
        String MaND = User.Identity.Name;
        Object objNam = NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND, "iNamLamViec");
        Object objThang = NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND, "iThangLamViec");
        int iNam = DateTime.Now.Year;
        if (objNam != null) iNam = Convert.ToInt16(objNam);
        String iNamLamViec = Convert.ToString(iNam);
        // String iNamLamViec = Request.QueryString["iNamLamViec"];

        if (String.IsNullOrEmpty(iNamLamViec))
        {
            iNamLamViec = DateTime.Now.Year.ToString();
        }
        DateTime dNgayHienTai = DateTime.Now;
        String NamHienTai = Convert.ToString(dNgayHienTai.Year);
        int NamMin = Convert.ToInt32(dNgayHienTai.Year) - 10;
        int NamMax = Convert.ToInt32(dNgayHienTai.Year) + 10;
        DataTable dtNam = new DataTable();
        dtNam.Columns.Add("MaNam", typeof(String));
        dtNam.Columns.Add("TenNam", typeof(String));
        DataRow R;
        for (int i = NamMin; i < NamMax; i++)
        {
            R = dtNam.NewRow();
            dtNam.Rows.Add(R);
            R[0] = Convert.ToString(i);
            R[1] = Convert.ToString(i);
        }
        dtNam.Rows.InsertAt(dtNam.NewRow(), 0);
        dtNam.Rows[0]["TenNam"] = "-- Bạn chọn năm ngân sách --";
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        
        String iThang = Convert.ToString(ViewData["iThang"]);
        if (String.IsNullOrEmpty(iThang))
            iThang = Convert.ToString(objThang);
        //String iThang = Convert.ToString(objThang);
       // if (String.IsNullOrEmpty(iThang)) iThang = DateTime.Now.Month.ToString();

        String iNgay = Convert.ToString(ViewData["iNgay"]);
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        int ThangHienTai = DateTime.Now.Month;
        //Chọn từ tháng
        DataTable dtThang = DanhMucModels.DT_Thang(false);

        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        //Chọn từ ngày

        DataTable dtNgay = DanhMucModels.DT_Ngay(Convert.ToInt16(iThang), iNam, false);
        SelectOptionList slNgay = new SelectOptionList(dtNgay, "MaNgay", "TenNgay");
        if (String.IsNullOrEmpty(iNgay))
        {
            if (dtNgay.Rows.Count > 1 && dtNgay != null)
            {
                iNgay = Convert.ToString(dtNgay.Rows[dtNgay.Rows.Count - 1]["TenNgay"]);
            }

        }
        if (dtNgay != null)
        {
            dtNgay.Dispose();

        }

        String DVT = Convert.ToString(ViewData["DVT"]);
        if (String.IsNullOrEmpty(DVT))
        {
            DVT = "0";
        }
        DataTable dtDVT = rptKTTH_TongHopCapVonController.DanhSach_LoaiBaoCao();
        SelectOptionList slLoaiBaoCao = new SelectOptionList(dtDVT, "MaLoai", "TenLoai");
        dtDVT.Dispose();
        DataTable dtTrangThai = rptKTTH_TongHopCapVonController.tbTrangThai();
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "TenTrangThai");
        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
        {
            if (dtTrangThai.Rows.Count > 0)
            {
                iID_MaTrangThaiDuyet = Convert.ToString(dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"]);
            }
            else
            {
                iID_MaTrangThaiDuyet = Guid.Empty.ToString();
            }
        }
        dtTrangThai.Dispose();
        String BackURL = Url.Action("SoDoLuong", "KeToanTongHop");
        // VIETTEL.Report_Controllers.KeToan.TienMat.rptChiTietTheoDonVi_2Controller ctlCTDV = new VIETTEL.Report_Controllers.KeToan.TienMat.rptChiTietTheoDonVi_2Controller();
        String KhoGiay = Convert.ToString(ViewData["KhoGiay"]);
        DataTable dtKhoGiay = ReportModels.LoaiKhoGiay();
        SelectOptionList slKhoGiay = new SelectOptionList(dtKhoGiay, "MaKhoGiay", "TenKhoGiay");
        if (String.IsNullOrEmpty(KhoGiay))
        {
            KhoGiay = "1";
        }
        dtKhoGiay.Dispose();
        String urlReport = "";
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        if (PageLoad.Equals("1"))
        {
            urlReport = Url.Action("ViewPDF", "rptKTTH_TongHopCapVon", new { iNamLamViec = iNamLamViec, iThang = iThang, iNgay = iNgay, DVT = DVT, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, KhoGiay = KhoGiay });
        }
        using (Html.BeginForm("EditSubmit", "rptKTTH_TongHopCapVon", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td style="width: 45%">
                        <span>Tổng hợp cấp vốn</span>
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
            <div id="rptMain" style="width: 750px; max-width: 800px; margin: 0px auto; padding: 0px 0px;
                overflow: visible; text-align: center">
                <table style="width: 600px; text-align: center">
                    <tr>
                        <td style="width: 50px;">
                        </td>
                        <td style="width: 100px;">
                            <fieldset style="padding: 2px; border-radius: 5px; width: 300px; margin-right: 3px;
                                height: 40px">
                                <legend style="font-weight: bold;">
                                    <%=NgonNgu.LayXau("&nbsp;Đến ngày &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbspTháng &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbspNăm")%></legend>
                                <p>
                                    <label id="td_iNgay1" style="width:80px;">
                                        <%=MyHtmlHelper.DropDownList(ParentID, slNgay, iNgay, "iNgay", "", "style=\"width:80px\"")%></label>
                                    <label>
                                        <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang, "iThang", "", "style=\"width:80px\" onchange=\"ChonThang()\"")%></label>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slNam, iNamLamViec, "iNamLamViec", "", "class=\"input1_2\" style=\"width: 70px; \" disabled=\"disabled\"")%>
                                </p>
                            </fieldset>
                        </td>
                        <td style="width: 20px;">
                        </td>
                        <td style="width: 150px;">
                            <!--End .inlineBlock-->
                            <fieldset style="padding: 2px; border-radius: 5px; width: 450px; margin-right: 3px;
                                height: 40px">
                                <legend style="font-weight: bold;">
                                    <%=NgonNgu.LayXau("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Đơn vị tính &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Trạng thái duyệt&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Khổ giấy")%></legend>
                                <p>
                                    <label>
                                        <%=MyHtmlHelper.DropDownList(ParentID,slLoaiBaoCao,DVT,"DVT","style=\"width:30%\"") %></label>
                                    <label>
                                        <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "style=\"width:35%\"")%></label>
                                    <label>
                                        <%=MyHtmlHelper.DropDownList(ParentID, slKhoGiay, KhoGiay, "KhoGiay", "", "class=\"input1_2\" style=\"width: 120px;\"")%></label>
                                </p>
                            </fieldset>
                        </td>
                    </tr>
                </table>
                <p style="text-align: center; padding: 4px;">
                    <input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>"
                        style="display: inline-block; margin-right: 5px;" /><input class="button" type="button"
                            value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" style="display: inline-block;
                            margin-left: 5px;" /></p>
            </div>
            <!--End #rptMain-->
        </div>
        <!--End #table_form2-->
    </div>
    <%} %>
    <div>
    </div>
    <script type="text/javascript">

        function ChonThang() {
            var iThang = document.getElementById("<%=ParentID %>_iThang").value;
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("Get_objNgayThang?ParentID=#0&MaND=#1&iThang=#2&iNgay=#3","rptKTTH_TongHopCapVon") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", "<%= MaND %>"));
            url = unescape(url.replace("#2", iThang));
            url = unescape(url.replace("#3", "<%= iNgay %>"));
            $.getJSON(url, function (data) {
                document.getElementById("td_iNgay1").innerHTML = data;
            });
        }



        function Huy() {
            window.location.href = '<%=BackURL %>';
        }
        $(document).ready(function () {

            $('.title_tong a').click(function () {
                $('div#rptMain').slideToggle('normal');
                $(this).toggleClass('active');
                return false;
            });
        });           
    </script>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptKTTH_TongHopCapVon", new { iNamLamViec = iNamLamViec, iThang = iThang, iNgay = iNgay, DVT = DVT, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, KhoGiay = KhoGiay }), "Xuất ra file Excel")%>
    <div>
        <iframe src="<%=urlReport%>" height="600px" width="100%"></iframe>
    </div>
</body>
</html>
