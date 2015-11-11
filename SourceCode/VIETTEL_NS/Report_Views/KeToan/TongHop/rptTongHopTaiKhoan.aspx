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
        String ParentID = "THTK";
        String iNamLamViec = Convert.ToString(ViewData["iNamLamViec"]);
        String MaND = User.Identity.Name;
        if (String.IsNullOrEmpty(iNamLamViec))
        {
            iNamLamViec = Convert.ToString(NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND, "iNamLamViec"));
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
       
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        String iThang =  Convert.ToString(ViewData["iThang"]);
        // Đơn Vị
        DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(MaND);

        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
        if (String.IsNullOrEmpty(iID_MaDonVi))
        {
            iID_MaDonVi = "00";
        }
        dtDonVi.Dispose();

        String iID_MaTaiKhoan = Convert.ToString(ViewData["iID_MaTaiKhoan"]);
        var dtiID_MaTaiKhoan = rptTongHopTaiKhoanController.TenTaiKhoan(iNamLamViec);
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
        //Chọn tháng
        DataTable dtThang = DanhMucModels.DT_Thang();
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        if (String.IsNullOrEmpty(iThang))
        {
            iThang = Convert.ToString(NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND, "iThangLamViec"));
        }
        dtThang.Dispose();
        String BackURL = Url.Action("SoDoLuong", "KeToanTongHop");
        DataTable dtTrangThai = HamChung.GetTrangThai(PhanHeModels.iID_MaPhanHeKeToanTongHop, LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop), true, "--Tất cả--");
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
        String iTrangThai =  Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        if (String.IsNullOrEmpty(iTrangThai))
            iTrangThai = dtTrangThai.Rows.Count > 0 ? Convert.ToString(dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"]) : Guid.Empty.ToString();
        String pageload = Convert.ToString(ViewData["PageLoad"]);
        String urlReport = pageload.Equals("1") ? Url.Action("ViewPDF", "rptTongHopTaiKhoan", new { iID_MaDonVi = iID_MaDonVi, iNamLamViec = iNamLamViec, iID_MaTaiKhoan = iID_MaTaiKhoan, iThang = iThang, iID_MaTrangThaiDuyet = iTrangThai }) : "";
        using (Html.BeginForm("EditSubmit", "rptTongHopTaiKhoan", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td width="47.9%">
                        <span>Báo cáo Tổng hợp tài khoản (Ký hiệu 36)</span>
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
            <div id="rptMain" style="width: 100%; margin: 0px auto; padding: 0px 0px; overflow: visible; text-align:center;">
                 <ul class="inlineBlock">  
                    <li >
                        <p style="text-align:right;">
                            <span><%=NgonNgu.LayXau("Chọn đơn vị:") %></span><%=MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 250px; padding:2px;\"")%>
                        </p>
                        <p style="text-align:right;">
                            <span><%=NgonNgu.LayXau("Chọn tài khoản:") %></span><%=MyHtmlHelper.DropDownList(ParentID, slTK, iID_MaTaiKhoan, "iID_MaTaiKhoan", "", "class=\"input1_2\" style=\"width: 250px; padding:2px;\"")%>
                        </p>
                    </li>
                    <li >
                        <p style="text-align:right;"><span><%=NgonNgu.LayXau("Chọn tháng:") %></span><%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang, "iThang", "", "class=\"input1_2\" style=\"width: 150px; padding:2px;\"")%></p>
                        <p style="text-align:right;"><span><%=NgonNgu.LayXau("Chọn trạng thái duyệt:") %></span><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iTrangThai, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 150px; padding:2px;\"")%></p>
                    </li>
                    </ul><!--End .inlineBlock-->
                    <%=MyHtmlHelper.DropDownList(ParentID, slNam, iNamLamViec, "iNamLamViec", "", "class=\"input1_2\" style=\"width: 40%; display:none;\"")%>
                    <p style="text-align:center; padding:4px;"><input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" style="display:inline-block; margin-right:20px;" />
                    <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>"  onclick="Huy()" style="display:inline-block; margin-left:0px;" /></p>        
            </div>
        </div>
    </div>
    <%} %>
    <div>
    </div>
    <%
        dtDonVi.Dispose();        
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
    </script>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptTongHopTaiKhoan", new { iID_MaDonVi = iID_MaDonVi, iNamLamViec = iNamLamViec, iID_MaTaiKhoan = iID_MaTaiKhoan, iThang = iThang, iID_MaTrangThaiDuyet = iTrangThai }), "Xuất ra file Excel")%>
    <iframe src="<%=urlReport%>"
        height="600px" width="100%"></iframe>
</body>
</html>
