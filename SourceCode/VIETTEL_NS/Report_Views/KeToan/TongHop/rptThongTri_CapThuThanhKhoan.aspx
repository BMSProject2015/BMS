<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
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
        String ParentID = "KeToanTongHop";
        String iNamLamViec = Convert.ToString(ViewData["iNamLamViec"]);

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
        String UserID = User.Identity.Name;
        String iID_MaDonVi =Convert.ToString(ViewData["iID_MaDonVi"]);
        
        String iThang = Convert.ToString(ViewData["iThang"]);
        String Loai = Convert.ToString(ViewData["Loai"]);
        //Chọn tháng
        DataTable dtThang = DanhMucModels.DT_Thang();
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        if (String.IsNullOrEmpty(iThang))
        {
            iThang = Convert.ToString(dtThang.Rows[1]["TenThang"]);
        }
        dtThang.Dispose();
        
        // Đơn Vị
        DataTable dtDonVi = rptThongTri_CapThuThanhKhoanController.dtDonVi(iThang,iNamLamViec);

        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "Ten");
        if (String.IsNullOrEmpty(iID_MaDonVi))
        {
            iID_MaDonVi = Convert.ToString(dtDonVi.Rows[0]["iID_MaDonVi"]);
        }
        dtDonVi.Dispose();
        //Loai
        DataTable dtLoai = rptThongTri_CapThuThanhKhoanController.DanhSach_Loai();
        SelectOptionList slLoai = new SelectOptionList(dtLoai, "MaLoai", "TenLoai");
        if (String.IsNullOrEmpty(Loai))
        {
            Loai = "0";
        }
        dtLoai.Dispose();
//TaiKhoan
        String iID_MaTaiKhoan = Convert.ToString(ViewData["iID_MaTaiKhoan"]);
        DataTable dtTaiKhoan = rptThongTri_CapThuThanhKhoanController.TenTaiKhoan(iNamLamViec);
        SelectOptionList slTK = new SelectOptionList(dtTaiKhoan, "iID_MaTaiKhoan", "TenTK");
        if (String.IsNullOrEmpty(iID_MaTaiKhoan))
        {
            if (dtTaiKhoan.Rows.Count > 0)
                iID_MaTaiKhoan = Convert.ToString(dtTaiKhoan.Rows[0]["iID_MaTaiKhoan"]);
            else
                iID_MaTaiKhoan = "-111";
            String[] arrTaiKhoan = iID_MaTaiKhoan.Split(',');
        }
        dtTaiKhoan.Dispose();
        
      
        //cap thanh khoan
        DataTable dtcap = rptThongTri_CapThuThanhKhoanController.Cap( iThang, iID_MaDonVi,iNamLamViec,iID_MaTaiKhoan);
        //String rSoTien = Convert.ToString(dtcap.Rows[0]["rSoTien"]);
        //rSoTien = CommonFunction.DinhDangSo(rSoTien);
        SelectOptionList slcap = new SelectOptionList(dtcap, "iID_MaDonVi", "rSoTien");

        if (String.IsNullOrEmpty(iID_MaDonVi))
        {
            iID_MaDonVi = Convert.ToString(dtcap.Rows[0]["iID_MaDonVi"]);
            String[] arrTaiKhoan = iID_MaTaiKhoan.Split(',');
        }
        
        dtcap.Dispose();
       
        //thu thanh khoan
        DataTable dtthu = rptThongTri_CapThuThanhKhoanController.thu(iThang, iID_MaDonVi, iNamLamViec, iID_MaTaiKhoan);
        //DataTable dtTT = dtthu;
        //for (int i = 0; i < 2; i++)
        //{
        //    CommonFunction.DinhDangSo(rSoTien);
        //}
        //dtTT.Dispose();
        //Double Thu = Convert.ToDouble(dtthu.Rows[0]["rSoTien"]);
        //Double Cap = Convert.ToDouble(dtcap.Rows[0]["rSoTien"]);
        SelectOptionList slthu = new SelectOptionList(dtthu, "iID_MaDonVi", "rSoTien");
        if (String.IsNullOrEmpty(iID_MaDonVi))
        {
            iID_MaDonVi = Convert.ToString(dtthu.Rows[0]["iID_MaDonVi"]);
            String[] arrTaiKhoan = iID_MaTaiKhoan.Split(',');
        }
       
        dtthu.Dispose();
        String BackURL = Url.Action("Index", "KeToan_ChiTiet_Report", new { sLoai = "1" });
        DataTable dtTrangThai = HamChung.GetTrangThai(PhanHeModels.iID_MaPhanHeKeToanTongHop, LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop), true, "--Tất cả--");
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
        String iTrangThai = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        if (String.IsNullOrEmpty(iTrangThai))
            iTrangThai = dtTrangThai.Rows.Count > 0 ? Convert.ToString(dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"]) : Guid.Empty.ToString();
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String UrlReport = "";
        if (PageLoad == "1")
            UrlReport = Url.Action("ViewPDF", "rptThongTri_CapThuThanhKhoan", new { iID_MaDonVi = iID_MaDonVi, iNamLamViec = iNamLamViec, iID_MaTaiKhoan = iID_MaTaiKhoan, iThang = iThang, iID_MaTrangThaiDuyet = iTrangThai, Loai = Loai });
        using (Html.BeginForm("EditSubmit", "rptThongTri_CapThuThanhKhoan", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td width="47.9%">
                        <span>Báo cáo thông tri cấp thu thanh khoản</span>
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
            <div id="rptMain" style="width:1024px; max-width:1024px; margin:0px auto; padding:0px 0px; overflow:visible; text-align:center;">
                <ul class="inlineBlock">                       
                    <li>
                        <fieldset style="padding:3px 5px;font-size:13px; height:70px;">
                            <legend style="padding:3px 3px 1px 5px;"><%=NgonNgu.LayXau("Chọn báo cáo:")%></legend>
                            <p id="divCap">
                               <%=MyHtmlHelper.Option(ParentID, "0", Loai, "Loai", "")%><span><%=NgonNgu.LayXau("1. Cấp thanh khoản")%></span>
                             <%=MyHtmlHelper.DropDownList(ParentID, slcap, iID_MaDonVi, "rSoTien", "", "class=\"input1_2\" style=\"width: 150px; padding:2px;\"")%>
                               <%--<%=MyHtmlHelper.Label(Cap,"rSoTien")%>--%>
                            </P>

                             <p id="divThu">  <%=MyHtmlHelper.Option(ParentID, "1", Loai, "Loai", "")%><span><%=NgonNgu.LayXau("2. Thu thanh khoản")%></span>
                              <%=MyHtmlHelper.DropDownList(ParentID, slthu, iID_MaDonVi, "rSoTien", "", "class=\"input1_2\" style=\"width: 150px; padding:2px;\"")%> 
                              <%--<%=MyHtmlHelper.Label(Thu,"rSoTien")%>--%>
                            </p>
                        </fieldset>
                    </li>
                    <li>
                        <fieldset style="border:1px solid #cecece;padding:3px 5px; font-size:13px;height:70px;">
                            <legend style="padding:3px 3px 1px 5px;"></legend>
                            <p style="text-align:right;"><span><%=NgonNgu.LayXau("Tháng:") %></span><%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang, "iThang", "", "class=\"input1_2\" style=\"width: 150px; padding:2px;\" onchange=\"chonDV(this.value)\"")%></p>
                        <p style="text-align:right;"><span><%=NgonNgu.LayXau("Trạng thái duyệt:") %></span><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iTrangThai, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 150px; padding:2px;\"")%></p>
                        </fieldset>
                    </li>
                    <li>
                        <fieldset style="padding:3px 5px;font-size:13px; height:70px;">
                            <legend style="padding:3px 3px 1px 5px;"><%=NgonNgu.LayXau("Đơn vị")%></legend>
                            <p id="divDonVi" style="text-align:right;"><%=MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 200px;height:50px; font-size:13px;\" size='3' tab-index='-1'\" onchange=\"chonCap()\"")%></p>
                        
                        </fieldset>
                    </li>
                </ul><!--End .inlineBlock-->
                <%=MyHtmlHelper.Hidden(ParentID,iNamLamViec,"iNamLamViec","")%>
                <%=MyHtmlHelper.Hidden(ParentID, iID_MaTaiKhoan, "iID_MaTaiKhoan", "")%>
                <p id="divCap"style="text-align:center; padding:4px;"><input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" style="display:inline-block; margin-right:5px;" /><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>"  onclick="Huy()" style="display:inline-block; margin-left:5px;" /></p>                    
            </div><!--End #rptMain-->
        </div>
    </div>


   
    <%} %>
    <div>
    </div>
    <%
        //dtDonVi.Dispose();        
        dtThang.Dispose();        
    %>
    <script type="text/javascript">
        $(function () {
            $('div.login1 a').click(function () {
                $('div#rptMain').slideToggle('normal');
                $(this).toggleClass('active');
                return false;
            });
        });       
    </script>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=BackURL %>';
        }
        function chonDV(iThang) {
          
           // var iThang = document.getElementById("<%=ParentID %>_iThang").value;
            var iNamLamViec = document.getElementById("<%=ParentID %>_iNamLamViec").value;
            var iID_MaDonVi = document.getElementById("<%=ParentID %>_iID_MaDonVi").value;
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("ObjDanhSachDonVi?ParentID=#0&iThang=#1&iNamLamViec=#2&iID_MaDonVi=#3", "rptThongTri_CapThuThanhKhoan") %>');
            url = unescape(url.replace("#0", "<%=ParentID %>"));
            url = unescape(url.replace("#1", iThang));
            url = unescape(url.replace("#2", iNamLamViec));
            url = unescape(url.replace("#3", "<%=iID_MaDonVi %>"));
            $.getJSON(url, function (data) {
                document.getElementById('divDonVi').innerHTML = data;
                chonCap();
            });
        }
        function chonCap() {
            var iID_MaDonVi = document.getElementById("<%=ParentID %>_iID_MaDonVi").value;
            var iThang = document.getElementById("<%=ParentID %>_iThang").value;
            var iNamLamViec = document.getElementById("<%=ParentID %>_iNamLamViec").value;
            var iID_MaTaiKhoan = document.getElementById("<%=ParentID %>_iID_MaTaiKhoan").value;
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("ObjCap?ParentID=#0&iThang=#1&iNamLamViec=#2&iID_MaDonVi=#3&iID_MaTaiKhoan=#4", "rptThongTri_CapThuThanhKhoan") %>');
            url = unescape(url.replace("#0", "<%=ParentID %>"));
            url = unescape(url.replace("#1", iThang));
            url = unescape(url.replace("#2", iNamLamViec));
            url = unescape(url.replace("#3", iID_MaDonVi ));
            url = unescape(url.replace("#4", "<%=iID_MaTaiKhoan %>"));
            $.getJSON(url, function (data) {
                document.getElementById('divCap').innerHTML = data.sCap;
                document.getElementById('divThu').innerHTML = data.sThu;
            });
        }       
    </script>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptThongTri_CapThuThanhKhoan", new { iID_MaDonVi = iID_MaDonVi, iNamLamViec = iNamLamViec, iID_MaTaiKhoan = iID_MaTaiKhoan, iThang = iThang, iID_MaTrangThaiDuyet = iTrangThai ,Loai =Loai}), "Xuất ra file Excel")%>
    <iframe src="<%=UrlReport%>"
        height="600px" width="100%"></iframe>
</body>
</html>
