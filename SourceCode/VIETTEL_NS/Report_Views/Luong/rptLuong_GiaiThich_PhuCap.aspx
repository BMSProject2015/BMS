<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.Luong" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">     
		ul#inlineBlock{
			list-style: none inside;			
		}
		ul#inlineBlock li{			
			-webkit-box-shadow: 2px 2px 0 #cecece;
			box-shadow: 2px 2px 0 #cecece;			
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
			height:23px;
			line-height:23px;
		}
		ul#inlineBlock li a{
			color: white;
			text-decoration: none;
			font-weight: 600;
		}
	.erroremail{color:#FF3600;}
    </style>
</head>
<body>
     <%
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "Luong";
        String iThang = Convert.ToString(ViewData["iThang"]);
        String UserID = User.Identity.Name;
        if (String.IsNullOrEmpty(iThang))
        {
            iThang = CauHinhLuongModels.LayThangLamViec(UserID).ToString();
        }
        //tháng     
        var dtThang = HamChung.getMonth(DateTime.Now, true, " Tháng: ", "Tháng");
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        if (dtThang != null) dtThang.Dispose();

        String iNam = Convert.ToString(ViewData["iNam"]);
        if (String.IsNullOrEmpty(iNam))
        {
            iNam = CauHinhLuongModels.LayNamLamViec(UserID).ToString();
        }
        DateTime dNgayHienTai = DateTime.Now;
        String NamHienTai = Convert.ToString(dNgayHienTai.Year);
        int NamMin = Convert.ToInt32(dNgayHienTai.Year) - 10;
        int NamMax = Convert.ToInt32(dNgayHienTai.Year) + 10;
        DataTable dtNam = HamChung.getYear(dNgayHienTai, true, " Năm: ");        
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        String URL = Url.Action("Index", "Luong_Report");
        DataTable dtTrangThai = HamChung.GetTrangThai(PhanHeModels.iID_MaPhanHeLuong.ToString(), LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeLuong).ToString());
        DataRow dR = dtTrangThai.NewRow();
        dR["iID_MaTrangThaiDuyet"] = "-1";
        dR["sTen"] = "Chọn trạng thái duyệt--";
        dtTrangThai.Rows.InsertAt(dR, 0);
        dR = dtTrangThai.NewRow();
        dR["iID_MaTrangThaiDuyet"] = "0";
        dR["sTen"] = "--Tất cả--";
        dtTrangThai.Rows.InsertAt(dR, 1);
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
        String iTrangThai = Convert.ToString(ViewData["iTrangThai"]);
        if (String.IsNullOrEmpty(iTrangThai))
        {
            if (dtTrangThai.Rows.Count > 0)
                iTrangThai = dtTrangThai.Rows[1]["iID_MaTrangThaiDuyet"].ToString();
            else        
                iTrangThai = Guid.Empty.ToString();
        }
        String iReport = Convert.ToString(ViewData["iReport"]);
        DataTable dtReport = new DataTable();
        dtReport.Columns.Add("IDReport", typeof(string));
        dtReport.Columns.Add("NameReport", typeof(string));
        DataRow dReport=dtReport.NewRow();
        dReport["IDReport"] = "rVKHD";
        dReport["NameReport"] = "Chi tiết Phụ cấp vượt khung-hạn định";
        dtReport.Rows.InsertAt(dReport, 0);
        dReport = dtReport.NewRow();
        dReport["IDReport"] = "rTrN";
        dReport["NameReport"] = "Chi tiết Phụ cấp trách nhiệm";
        dtReport.Rows.InsertAt(dReport, 1);
        dReport = dtReport.NewRow();
        dReport["IDReport"] = "rKV";
        dReport["NameReport"] = "Chi tiết Phụ cấp khu vực";
        dtReport.Rows.InsertAt(dReport, 2);
        dReport = dtReport.NewRow();
        dReport["IDReport"] = "rDB";
        dReport["NameReport"] = "Chi tiết Phụ cấp đặc biệt";
        dtReport.Rows.InsertAt(dReport, 3);
        dReport = dtReport.NewRow();
        dReport["IDReport"] = "rCV";
        dReport["NameReport"] = "Chi tiết Phụ cấp công vụ";
        dtReport.Rows.InsertAt(dReport, 4);
        dReport = dtReport.NewRow();
        dReport["IDReport"] = "rKHAC";
        dReport["NameReport"] = "Chi tiết Phụ cấp khác";
        dtReport.Rows.InsertAt(dReport, 5);
        SelectOptionList slReport = new SelectOptionList(dtReport, "IDReport", "NameReport");
        if (String.IsNullOrEmpty(iReport))
        {
            iReport = "rVKHD";
        }
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String urlReport = "";        
        
        if (PageLoad == "1")
            urlReport = Url.Action("ViewPDF", "rptLuong_GiaiThich_PhuCap", new { iThang = iThang, iNam = iNam, iTrangThai = iTrangThai, iReport = iReport });
        using (Html.BeginForm("EditSubmit", "rptLuong_GiaiThich_PhuCap", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo giải thích phụ cấp</span>
                    </td>
                </tr>
            </table>
        </div>        
        <div id="table_form2" class="table_form2">
            <div id="" style="width:1024px; max-width:1024px; margin:0px auto; padding:0px 0px; overflow:visible;">
                <ul id="inlineBlock">
                    <li>
                        <div>
                            <span style="float:left; text-align:center; width:80px;"><%=NgonNgu.LayXau("Chọn tháng")%></span>
                            <span style="float:right; text-align:center; width:80px;"><%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang, "iThang", "", "class=\"input1_2\" style=\"width:100%; padding:2px;\"")%></span>
                        </div>
                    </li>
                    <li>
                        <div>
                            <span style="float:left; text-align:center; width:80px;"><%=NgonNgu.LayXau("Chọn năm")%></span>
                            <span style="float:right; text-align:center; width:80px;"><%=MyHtmlHelper.DropDownList(ParentID, slNam, iNam, "iNam", "", "class=\"input1_2\" style=\"width:100%; padding:2px;\"")%></span>
                        </div>
                    </li>
                    <li>
                        <div>
                            <span style="float:left; text-align:center; width:100px;"><%=NgonNgu.LayXau("Chọn trạng thái")%></span>
                            <span style="float:right; text-align:center; width:150px;"><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iTrangThai, "iTrangThai", "", "class=\"input1_2\" style=\"width:100%; padding:2px;\"")%></span>
                        </div>
                    </li>
                    <li>
                        <div>
                            <span style="float:left; text-align:center; width:120px;"><%=NgonNgu.LayXau("Chọn loại báo cáo")%></span>
                            <span style="float:right; text-align:center; width:250px;"><%=MyHtmlHelper.DropDownList(ParentID, slReport, iReport, "iReport", "", "class=\"input1_2\" style=\"width:100%; padding:2px;\"")%></span>
                        </div>
                    </li>
                </ul>                
                <div id="both" style="clear:both; min-height:30px; line-height:30px; margin-bottom:-5px; ">
                    <table cellpadding="0" cellspacing="0" border="0" align="center" style="margin: 5px auto; width:200px;">
                        <tr>
                            <td><input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" /></td>
                            <td width="5px"></td>
                            <td><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" /></td>
                        </tr>
                    </table>   
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=URL %>';
        }
        function chon_Click(id1, id2, id3, values1, values2, values3) {
            $("#<%=ParentID %>_" + id1).change(function () {
                if ($(this).val() == values1) {
                    $("#Submit1").attr("disabled", true);
                    $(this).css("background", "");
                    $(this).css("background", "#ffee66");
                }
                else {
                    if ($("#<%=ParentID %>_" + id2).val() == values2) {
                        $("#Submit1").attr("disabled", true);
                    }
                    else {
                        if ($("#<%=ParentID %>_" + id3).val() == values3) {
                            $("#Submit1").attr("disabled", true);
                        }
                        else {
                            $("#Submit1").attr("disabled", false);
                        }
                    }
                    $(this).css("background", "");
                }
            });
        }
        $(document).ready(function () {
            chon_Click("iThang", "iNam", "iTrangThai", "0", "0", "-1");
            chon_Click("iNam", "iThang", "iTrangThai", "0", "0", "-1");
            chon_Click("iTrangThai", "iNam", "iThang", "-1", "0", "0");
        });
    </script>
    <%} %>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptLuong_GiaiThich_PhuCap", new { iThang = iThang, iNam = iNam, iTrangThai = iTrangThai, iReport = iReport }), "Xuất ra file Excel")%>
    <iframe src="<%=urlReport%>" height="600px" width="100%">
    </iframe>
</body>
</html>