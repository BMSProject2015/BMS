<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QuyetToan" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.TienMat" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../../Content/Report_Style/ReportCSS.css" rel="StyleSheet" type="text/css" />
    <script src="../../../Scripts/Report/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/Report/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/Report/ShowDialog.js" type="text/javascript"></script>
    <style type="text/css">
        fieldset
        {
            padding: 3px;
            border: 1px solid #dedede;
            border-radius: 3px;
            -webkit-border-radius: 3px;
            -moz-border-radius: 3px;
        }
        fieldset legend
        {
            padding: 3px;
            font-size: 14px;
            font-family: Tahoma Arial;
        }
        fieldset p
        {
            padding: 2px;
        }
        fieldset p span
        {
            padding: 2px;
            font-size: 13.5px;
        }
        div#td_TaiKhoan.mGrid tr:even
        {
            background-color: #dedede;
        }
    </style>
</head>
<body>
    <%
       
        String ParentID = "KeToan";
        String MaND = User.Identity.Name;
        Object objNam = NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND, "iNamLamViec");
        int iNam = DateTime.Now.Year;
        if (objNam != null) iNam = Convert.ToInt16(objNam);
        String iNamLamViec = Convert.ToString(iNam);
        // String iNamLamViec = Request.QueryString["iNamLamViec"];

        if (String.IsNullOrEmpty(iNamLamViec))
        {
            iNamLamViec = DateTime.Now.Year.ToString();
        }
        Object objiThang = NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND, "iThangLamViec");
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String UserID = User.Identity.Name;
        DataTable dtPhongBan = NganSach_HamChungModels.DSDonViCuaNguoiDung(UserID);
        // String iID_MaDonVi = Request.QueryString["iID_MaDonVi"];
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iTrangThai"]);

        //Trang thai duyet
        DataTable dtTrangThai = rptQuyetToan_25_5Controller.tbTrangThai();
        // String iID_MaTrangThaiDuyet = Request.QueryString["iTrangThai"];
        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
            iID_MaTrangThaiDuyet = "1";
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "TenTrangThai");
        if (dtTrangThai != null)
        {
            dtTrangThai.Dispose();
        }
        // String sMaDonVi = iID_MaDonVi;
        if (String.IsNullOrEmpty(iID_MaDonVi))
        {
            PageLoad = "0";
        }
        String[] arrPhongBan = iID_MaDonVi.Split(',');
        dtPhongBan.Dispose();
        String[] arrMaDonVi;
        String MaDonVi = "-111";//= arrMaDonVi[ChiSo];
        int ChiSo = 0;
        String LoaiBaoCao = Convert.ToString(ViewData["LoaiBaoCao"]);
        if (String.IsNullOrEmpty(LoaiBaoCao))
        {
            LoaiBaoCao = "0";
        }
        if (String.IsNullOrEmpty(iID_MaDonVi) == false)
        {
            arrMaDonVi = iID_MaDonVi.Split(',');

            ChiSo = Convert.ToInt16(Request.QueryString["ChiSo"]);
            if (ChiSo == arrMaDonVi.Length)
            {
                ChiSo = 0;
            }
            if (ChiSo <= arrMaDonVi.Length - 1)
            {
                MaDonVi = arrMaDonVi[ChiSo];
                ChiSo = ChiSo + 1;
            }
            else
            {
                ChiSo = 0;
            }
        }
        else
        {
            iID_MaDonVi = "-111";
        }
        if (LoaiBaoCao == "1")
        {
            ChiSo = 0;
            MaDonVi = iID_MaDonVi;
        }
        String iID_MaTaiKhoan = Convert.ToString(ViewData["iID_MaTaiKhoan"]);
        //String iID_MaTaiKhoan = Request.QueryString["iID_MaTaiKhoan"];
        var dtiID_MaTaiKhoan = rptChiTietTheoDonVi_2Controller.TenTaiKhoan(iNamLamViec);
        SelectOptionList slTK = new SelectOptionList(dtiID_MaTaiKhoan, "iID_MaTaiKhoan", "TenTK");
        if (String.IsNullOrEmpty(iID_MaTaiKhoan))
        {
            if (dtiID_MaTaiKhoan.Rows.Count > 0)
            {
                iID_MaTaiKhoan = Convert.ToString(dtiID_MaTaiKhoan.Rows[0]["iID_MaTaiKhoan"]);
            }
            else
            {
                iID_MaTaiKhoan = "";
            }
        }
        String iThang1 = Convert.ToString(ViewData["iThang1"]);
        if (String.IsNullOrEmpty(iThang1)) iThang1 = Convert.ToString(objiThang);
        String iThang2 = Convert.ToString(ViewData["iThang2"]);
        if (String.IsNullOrEmpty(iThang2)) iThang2 = Convert.ToString(objiThang);
        String iNgay1 = Convert.ToString(ViewData["iNgay1"]);
        String iNgay2 = Convert.ToString(ViewData["iNgay2"]);

        int ThangHienTai = DateTime.Now.Month;
        //Chọn từ tháng
        DataTable dtThang = DanhMucModels.DT_Thang(false);

        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        //Chọn từ ngày

        DataTable dtNgay1 = DanhMucModels.DT_Ngay(Convert.ToInt16(iThang1), iNam, false);
        SelectOptionList slNgay = new SelectOptionList(dtNgay1, "MaNgay", "TenNgay");
        if (String.IsNullOrEmpty(iNgay1))
        {
            iNgay1 = Convert.ToString(dtNgay1.Rows[0]["TenNgay"]);
        }
        dtNgay1.Dispose();
        DataTable dtNgay2 = DanhMucModels.DT_Ngay(Convert.ToInt16(iThang2), iNam, false);
        if (String.IsNullOrEmpty(iNgay2))
        {
            iNgay2 = Convert.ToString(dtNgay2.Rows[dtNgay2.Rows.Count - 1]["MaNgay"]);
           // iNgay2 = Convert.ToString(HamChung.GetDaysInMonth(Convert.ToInt32(iThang2), Convert.ToInt32(iNamLamViec)));
        }
        dtNgay2.Dispose();
        String TuNgay = iNamLamViec + "/" + iThang1 + "/" + iNgay1;
        String DenNgay = iNamLamViec + "/" + iThang2 + "/" + iNgay2;
        DataTable dtLoaiBaoCao = rptChiTietTheoDonVi_2Controller.DanhSach_LoaiBaoCao();
        SelectOptionList slLoaiBaoCao = new SelectOptionList(dtLoaiBaoCao, "MaLoai", "TenLoai");
        dtLoaiBaoCao.Dispose();
        String BackURL = Url.Action("SoDoLuong", "KeToanTongHop");
        String ControllerName = "rptChiTietTheoDonVi_2";
        String TK = Url.Action("Index", "rptKeToan_DanhMucTaiKhoan", new { sKyHieu = "62", ControllerName = ControllerName });
        VIETTEL.Report_Controllers.KeToan.TienMat.rptChiTietTheoDonVi_2Controller ctlCTDV = new VIETTEL.Report_Controllers.KeToan.TienMat.rptChiTietTheoDonVi_2Controller();

        String urlReport = "";
     
        if (PageLoad.Equals("1"))
        {
            urlReport = Url.Action("ViewPDF", "rptChiTietTheoDonVi_2", new { iID_MaDonVi = MaDonVi, iNamLamViec = iNamLamViec, iID_MaTaiKhoan = iID_MaTaiKhoan, iThang1 = iThang1, iThang2 = iThang2, iNgay1 = iNgay1, iNgay2 = iNgay2, LoaiBaoCao = LoaiBaoCao,iTrangThai = iID_MaTrangThaiDuyet });
        }
        using (Html.BeginForm("EditSubmit", "rptChiTietTheoDonVi_2", new { ParentID = ParentID, ChiSo=ChiSo }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <%--<td>
                        <span>Báo cáo chi tiết theo đơn vị</span>
                    </td>--%>
                    <td width="47.9%">
                        <span>Báo cáo chi tiết theo đơn vị</span>
                    </td>
                    <td width="52%" style="text-align: left;">
                        <div class="login1" style="width: 50px; height: 20px; text-align: left;">
                            <a style="cursor: pointer;"></a>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <!--End .title_tong-->

        <div id="confirmBox" title="Báo cáo chi tiết theo tài khoản/đơn vị" style="width: 660px; min-height: 200px;
            display: none;">
            <div>
                <div id="left" style="float: left; width: 305px; text-align: left; padding: 3px;">
                    <fieldset>
                        <legend>
                         <b>    <%=NgonNgu.LayXau("Tài khoản (Ký hiệu 62)")%></b> </legend>
                             <input  class="button" id="Submit1" onclick='TaiKhoan()' value="<%=NgonNgu.LayXau("Thêm TK")%>"
                    style="display: inline-block; margin-right: 5px;" />
                        <p>
                            <%=MyHtmlHelper.DropDownList(ParentID, slTK, iID_MaTaiKhoan, "iID_MaTaiKhoan", "", "class=\"input1_2\" style=\"width: 100%\" size='18' tab-index='-1'")%>
                        </p>
                    </fieldset>
                    <fieldset>
                        <legend>
                            <%=NgonNgu.LayXau("Từ ngày&nbsp;&nbsp;&nbsp;Tháng&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Đến ngày&nbsp;&nbsp;Tháng")%></legend>
                        <p>
                            <label id="td_iNgay1">
                                <%=MyHtmlHelper.DropDownList(ParentID, slNgay, iNgay1, "iNgay1", "","style=\"width:20%\"")%></label>
                            <label>
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang1, "iThang1", "", "style=\"width:20%\" onchange=\"ChonThang(this.value,'iNgay1')\"")%></label>
                            <label id="td_iNgay2">
                                <%=MyHtmlHelper.DropDownList(ParentID, slNgay, iNgay2, "iNgay2", "","style=\"width:20%\"")%></label>
                            <label>
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang2, "iThang2", "", "style=\"width:20%\" onchange=\"ChonThang(this.value,'iNgay2')\"")%></label>
                        </p>
                    </fieldset>
                       <fieldset>
                        <legend>
                            <b> <%=NgonNgu.LayXau("Trạng thái:")%></b> </legend>
                        <p style="margin-bottom: 5px;">
                            <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", null, "class=\"input1_2\"")%></p>
                    </fieldset>
                </div>
                <div id="right" style="float: right; width: 305px; text-align: left; padding: 3px;">
                    <fieldset>
                        <legend>
                         <b>  <%=NgonNgu.LayXau("Chọn đơn vị")%></b> </legend>
                        <table class="mGrid">
                            <tr>
                                <th align="center" style="width: 25px;">
                                    <input type="checkbox" id="abc" onclick="CheckAll(this.checked)" />
                                </th>
                                <th align="left" style="font-size: 12px;">
                                    Tên đơn vị
                                </th>
                            </tr>
                        </table>
                        <div id="divDonVi" style="width: 100%; height: 310px; overflow: scroll;">
                            <%=ctlCTDV.get_sDanhSachDonVi(TuNgay, DenNgay, MaND,iID_MaDonVi)%>
                        </div>
                    </fieldset>
                    <fieldset>
                        <legend>
                           <b>  <%=NgonNgu.LayXau("Kiểu in:") %></b> </legend>
                        <div style="padding-bottom: 5px;">
                            <%=MyHtmlHelper.Option(ParentID, "0", LoaiBaoCao, "LoaiBaoCao", "")%><span style="font-size: 12px;
                                font-weight: bold;">1. In riêng từng đơn vị đã chọn</span></div>
                        <div>
                            <%=MyHtmlHelper.Option(ParentID, "1", LoaiBaoCao, "LoaiBaoCao", "")%><span style="font-size: 12px;
                                font-weight: bold;">2. In liên tiếp các đơn vị đã chọn</span></div>

                                 <div>
                            <%=MyHtmlHelper.Option(ParentID, "2", LoaiBaoCao, "LoaiBaoCao", "")%><span style="font-size: 12px;
                                font-weight: bold;">3. In đơn vị theo BQL (chọn 1 đơn vị)</span></div>
                    </fieldset>
                </div>
                <!--End #right-->
            </div>
            <!--End div-->
            <p style="text-align: center; padding: 4px; clear: both;">
                <input type="submit" class="button" id="Submit2" onclick='clicks()' value="<%=NgonNgu.LayXau("Thực hiện")%>"
                    style="display: inline-block; margin-right: 5px;" /><input class="button" type="button"
                        value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" style="display: inline-block;
                        margin-left: 5px;" /></p>
        </div>
        <!--End #confirmBox-->
    </div>
    <%} %>
    <div>
    </div>
    <script type="text/javascript">
        function ChonDV(DV) {
            $("input:checkbox[check-group='iID_MaDonVi']").each(function (i) {
                this.checked = DV;
            });
        }    

        function ChonThang(Thang,TenTruong) {            
            var Ngay = document.getElementById("<%=ParentID %>_" + TenTruong).value;
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("Get_objNgayThang?ParentID=#0&TenTruong=#1&Ngay=#2&Thang=#3&iNam=#4", "rptChiTietTheoDonVi_2") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", TenTruong));
            url = unescape(url.replace("#2", Ngay));
            url = unescape(url.replace("#3", Thang));
            url = unescape(url.replace("#4", <%=iNamLamViec %>));
                $.getJSON(url, function (data) {
                document.getElementById("td_"+TenTruong).innerHTML = data;                 
                DonViHienThi();
            });            
        }       
        function DonViHienThi()
        {                         
            var ngay1=document.getElementById("<%=ParentID %>_iNgay1").value;
            var ngay2=document.getElementById("<%=ParentID %>_iNgay2").value;
            var Thang1=document.getElementById("<%=ParentID %>_iThang1").value;
            var Thang2=document.getElementById("<%=ParentID %>_iThang2").value;
            var TuNgay="<%=iNamLamViec %>"+"/"+Thang1+"/"+ngay1;
            var DenNgay="<%=iNamLamViec %>"+"/"+Thang2+"/"+ngay2;

            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("ObjDanhSachDonVi?TuNgay=#0&DenNgay=#1&MaND=#2&iID_MaDonVi=#3", "rptChiTietTheoDonVi_2") %>');
            url = unescape(url.replace("#0", TuNgay));
            url = unescape(url.replace("#1", DenNgay));
            url = unescape(url.replace("#2", "<%=MaND %>"));
            url = unescape(url.replace("#3", "<%=iID_MaDonVi %>"));
            $.getJSON(url, function (data) {
                document.getElementById('divDonVi').innerHTML = data;
            });
        }     
        $(function(){
            if("<%=PageLoad %>"=="0"){
                ShowDialog(640);
            }
            $('*').keyup(function (e) {
                if (e.keyCode == '27') {
                    Hide();
                }
            });
            $('div.login1 a').click(function () {
                ShowDialog(640);
            });    
        });                                
    </script>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=BackURL %>';
        }
        function TaiKhoan() {
            window.location.href = '<%=TK %>';
        }
    </script>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptChiTietTheoDonVi_2", new { iID_MaDonVi = iID_MaDonVi, iNamLamViec = iNamLamViec, iID_MaTaiKhoan = iID_MaTaiKhoan, iThang1 = iThang1, iThang2 = iThang2, iNgay1 = iNgay1, iNgay2 = iNgay2, LoaiBaoCao = LoaiBaoCao, iTrangThai = iID_MaTrangThaiDuyet }), "Xuất ra file Excel")%>
    <div>
        <iframe src="<%=urlReport%>" height="600px" width="100%"></iframe>
    </div>
</body>
</html>
