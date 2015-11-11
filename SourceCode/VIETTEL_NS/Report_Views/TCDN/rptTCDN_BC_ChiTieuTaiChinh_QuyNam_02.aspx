<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.TCDN" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>BÁO CÁO MỘT SỐ CHỈ TIÊU TÀI CHÍNH QUÝ NĂM</title>
    <style type="text/css">
        .div-floatleft
        {                
            max-height:80px;            
        }
        .div-label
        {           
            font-size:13px;  
            padding:5px 0px;                 
        }
        .div-txt
        {
            padding-top:5px;                  
        }    
        .p
        {
            height:23px;
            line-height:23px;
            padding:1px 2px;    
        }
    </style>
    <link type="text/css" rel="Stylesheet" href="../../Content/style.css" />
</head>
<body>
    <%
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "TCDN";
        String Nam = Convert.ToString(ViewData["iNam"]);
        String UserID = User.Identity.Name;
        if (String.IsNullOrEmpty(Nam))
        {            
            Nam = DanhMucModels.NamLamViec(UserID).ToString();
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
        String iID_MaDoanhNghiep = Convert.ToString(ViewData["iMaDN"]);       
        String PageLoad = Convert.ToString(ViewData["pageload"]);
        String urlReport = "";
        String URL = Url.Action("Index", "TCDN_Report");
        DataTable dtQuy = DanhMucModels.DT_Quy();
        dtQuy.Rows[0]["MaQuy"] = "0";
        dtQuy.AcceptChanges();
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
        String iQuy = Convert.ToString(ViewData["iQuy"]);
        if (String.IsNullOrEmpty(iQuy))
        {
            iQuy = "0";
        }
        String OnOrOff = Convert.ToString(ViewData["iAll"]);
        String _Checked = "";
        if (String.IsNullOrEmpty(OnOrOff))
        {
            OnOrOff = "off";
        }
        if (OnOrOff == "on")
            _Checked = "checked=\"checked\"";
        else if (OnOrOff == "off")
            _Checked = "";
        DataTable dtDoanhNghiep = rptTCDN_BC_ChiTieuTaiChinh_QuyNam_02Controller.GetDoanhNghiep(iQuy, Nam, OnOrOff);
        if (String.IsNullOrEmpty(iID_MaDoanhNghiep))
        {            
            iID_MaDoanhNghiep = Guid.Empty.ToString();
        }
        dtDoanhNghiep.Dispose(); 
        if (PageLoad == "1")
            urlReport = Url.Action("ViewPDF", "rptTCDN_BC_ChiTieuTaiChinh_QuyNam_02", new { iQuy = iQuy, iNam = Nam, iAll = OnOrOff, iMaDN = iID_MaDoanhNghiep });
        using (Html.BeginForm("EditSubmit", "rptTCDN_BC_ChiTieuTaiChinh_QuyNam_02", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo một số chỉ tiêu tài chính quý năm</span>
                    </td>
                </tr>
            </table>
        </div>        
        <div id="table_form2" class="table_form2">
            <div id="" style="width:900px; max-width:900px; margin:0px auto; padding:0px 0px; overflow:visible; ">
                <ul class="inlineBlock">
                    <li>
                        <div>
                            <span style="float:left; text-align:center; width:80px;"><%=NgonNgu.LayXau("Chọn quý")%></span>
                            <span style="float:right; text-align:center; width:80px;"><%=MyHtmlHelper.DropDownList(ParentID, slQuy, iQuy, "iQuy", "", "class=\"input1_2\" style=\"width:100%; padding:2px;\" onchange=\"ChonDoanhNghiep()\"")%></span>
                        </div>
                    </li>
                    <li>
                        <div>
                            <span style="float:left; text-align:center; width:80px;"><%=NgonNgu.LayXau("Chọn năm")%></span>
                            <span style="float:right; text-align:center; width:80px;"><%=MyHtmlHelper.DropDownList(ParentID, slNam, Nam, "iNam", "", "class=\"input1_2\" style=\"width:100%; padding:2px;\" onchange=\"ChonDoanhNghiep()\"")%></span>
                        </div>
                    </li>
                    <li>
                        <div>
                            <span style="float:left; text-align:center; width:120px;"><%=NgonNgu.LayXau("Chọn đơn vị báo cáo")%></span>
                            <span style="float:right; text-align:center; width:220px;" id="<%=ParentID %>_divDoanhNghiep">
                                <%rptTCDN_BC_ChiTieuTaiChinh_QuyNam_02Controller rpt = new rptTCDN_BC_ChiTieuTaiChinh_QuyNam_02Controller();%>
                                <%=rpt.obj_DSDoanhNghiep(ParentID, iQuy, Nam, OnOrOff, iID_MaDoanhNghiep)%>
                            </span>
                        </div>
                    </li>
                    <li>
                        <div>
                            <input type="checkbox" value="rAll" <%=_Checked%>  id="rAll" style="cursor:pointer;" onclick="ChonTongHop(this.checked)" /><span style="font-size:9pt; line-height:22px; padding-left:2px;">Tổng hợp theo năm</span>
                            <span style="float:right; text-align:center; width:80px; display:none;"><%=MyHtmlHelper.TextBox(ParentID, OnOrOff, "iALL", "", "")%></span>
                        </div>
                    </li>
                </ul>                
                </div>
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
    <%} %>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=URL %>';
        }
        function ChonTongHop(DonVi) {
            $("input:checkbox").each(function (i) {
                if (DonVi) {
                    document.getElementById("<%= ParentID %>_iALL").value = "on";                    
                }
                else {
                    document.getElementById("<%= ParentID %>_iALL").value = "off";                    
                }
            });
            ChonDoanhNghiep()
        }
        function ChonDoanhNghiep() {
            var Nam = document.getElementById("<%=ParentID %>_iNam").value
            var Quy = document.getElementById("<%=ParentID %>_iQuy").value
            var All = document.getElementById("<%=ParentID %>_iALL").value
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("Get_dsDoanhNghiep?ParentID=#0&Quy=#1&Nam=#2&All=#3&iMaDN=#4", "rptTCDN_BC_ChiTieuTaiChinh_QuyNam_02") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", Quy));
            url = unescape(url.replace("#2", Nam));
            url = unescape(url.replace("#3", All));
            url = unescape(url.replace("#4", "<%= iID_MaDoanhNghiep %>"));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_divDoanhNghiep").innerHTML = data;
            });
        }
        function chon_Click(id1, id2, values1, values2) {
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
                            $("#Submit1").attr("disabled", false);
                    }                    
                    $(this).css("background", "");
                }
            });
        }
        function load_id(id, values) {
            var temp = $("#<%=ParentID %>_" + id).val();
            if (temp == values) {
                $("#<%=ParentID %>_" + id).css("background", "#ffee66");                
            }
            else {
                $("#<%=ParentID %>_" + id).css("background", "");                
            }
        }
        $(document).ready(function () {            
            if ($("#<%=ParentID %>_iQuy").val() == "0") {                
                $("#Submit1").attr("disabled", true);                
            }
            if ($("#<%=ParentID %>_iNam").val() == "0") {
                $("#Submit1").attr("disabled", true);
            }
            chon_Click("iQuy", "iNam", "0", "");
            chon_Click("iNam", "iQuy", "", "");
        });
    </script>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptTCDN_BC_ChiTieuTaiChinh_QuyNam_02", new { Quy = iQuy, Nam = Nam, All = OnOrOff, MaDN = iID_MaDoanhNghiep }), "Xuất ra excel")%>
    <iframe src="<%=urlReport%>" height="600px" width="100%">
    </iframe>
</body>
</html>
