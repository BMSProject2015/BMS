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
    <title>BÁO CÁO TÌNH HÌNH THU NỘP QUÝ NĂM</title>
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
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
        String iQuy = Convert.ToString(ViewData["iQuy"]);
        if (String.IsNullOrEmpty(iQuy))
        {
            iQuy = "0";
        }
        //Loại hình doanh nghiệp
        DataTable dtLoaiDN = DanhMucModels.DT_DanhMuc("TCDN_LoaiHinhDN", true, "--- Loại hình doanh nghiệp ---");
        SelectOptionList slLoaiHinh = new SelectOptionList(dtLoaiDN, "iID_MaDanhMuc", "sTen");
        String iLoaiDN = Convert.ToString(ViewData["iLoaiDN"]);
        if (String.IsNullOrEmpty(iLoaiDN))
        {
            iLoaiDN = Guid.Empty.ToString();
        }
        if (dtLoaiDN != null) dtLoaiDN.Dispose();
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
        DataTable dtDoanhNghiep = rptTCDN_TinhHinhThuNop_QuyNam_03Controller.GetDoanhNghiep(iQuy, Nam, OnOrOff,iLoaiDN);
        if (String.IsNullOrEmpty(iID_MaDoanhNghiep))
        {            
            iID_MaDoanhNghiep = Guid.Empty.ToString();
        }
        dtDoanhNghiep.Dispose(); 
        if (PageLoad == "1")
            urlReport = Url.Action("ViewPDF", "rptTCDN_TinhHinhThuNop_QuyNam_03", new { iQuy = iQuy, iNam = Nam, iAll = OnOrOff, iLoaiDN=iLoaiDN, iMaDN = iID_MaDoanhNghiep });
        using (Html.BeginForm("EditSubmit", "rptTCDN_TinhHinhThuNop_QuyNam_03", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo tình hình thu nộp quý năm</span>
                    </td>
                </tr>
            </table>
        </div>        
        <div id="table_form2" class="table_form2">
            <div id="" style="width:1024px; max-width:1024px; margin:0px auto; padding:0px 0px; overflow:visible; ">
                <div id="main" style="width:1024px; float:left; border:1px solid #cecece; border-radius:3px; height:40px; text-align:center;padding:4px 0px 2px 0px;">              
                    <div id="Left" style="float:left; width:600px; vertical-align:middle;">
                        <div style="float:left; width:170px; ">                                         
                            <div class="div-label" style="width:70px; text-align:center;float:left;">
                                <p class="p"><%=NgonNgu.LayXau("Chọn quý")%></p>                      
                            </div>
                            <div class="div-label" style="width:100px; float:right; text-align:left; ">
                                <p class="p"><%=MyHtmlHelper.DropDownList(ParentID, slQuy, iQuy, "iQuy", "", "class=\"input1_2\" style=\"width:100%; padding:4px;\" onchange=\"ChonDoanhNghiep()\"")%></p>                                    
                            </div>                       
                        </div><!-----------End doanh nghiệp-----------> 
                        <div style="float:left; width:170px;">              
                            <div class="div-label" style="width:70px; text-align:center;float:left;">                                
                                <p class="p"><%=NgonNgu.LayXau("Chọn năm")%></p>                                
                            </div>
                            <div class="div-label" style="width:100px; float:right; text-align:left; ">                               
                                <p class="p"><%=MyHtmlHelper.DropDownList(ParentID, slNam, Nam, "iNamLamViec", "", "class=\"input1_2\" style=\"width: 100%;padding:4px;\" onchange=\"ChonDoanhNghiep()\"")%></p>                                
                            </div>                       
                        </div><!-----------End năm làm việc----------->
                        <div style="float:left; width:250px;">              
                            <div class="div-label" style="width:100px; text-align:center;float:left;">                                
                                <p class="p"><%=NgonNgu.LayXau("Loại hình DN")%></p>                                
                            </div>
                            <div class="div-label" style="width:150px; float:right; text-align:left; ">                               
                                <p class="p"><%=MyHtmlHelper.DropDownList(ParentID, slLoaiHinh, iLoaiDN, "iLoaiDN", "", "class=\"input1_2\" style=\"width: 100%;padding:4px;\" onchange=\"ChonDoanhNghiep()\"")%></p>                                
                            </div>                       
                        </div><!-----------End năm làm việc----------->
                    </div>
                    <div id="Right" style="float:right;width:424px; vertical-align:middle;display:inline-block">
                        <div style="float:left; width:250px;">
                            <div class="div-label" style="width:100px; text-align:center;float:left;">                                
                                <p class="p"><%=NgonNgu.LayXau("Đơn vị báo cáo")%></p>                                
                            </div>
                            <div class="div-label" style="width:150px; text-align:center;float:left;">                    
                                <p class="p" id="<%=ParentID %>_divDoanhNghiep">                                   
                                    <%rptTCDN_TinhHinhThuNop_QuyNam_03Controller rpt = new rptTCDN_TinhHinhThuNop_QuyNam_03Controller();%>
                                    <%=rpt.obj_DSDoanhNghiep(ParentID, iQuy, Nam, OnOrOff,iLoaiDN, iID_MaDoanhNghiep)%>                                          
                                </p>                                                         
                            </div>
                        </div>
                        <div style="float:right; width:170px; padding-top:10px;vertical-align:middle;">
                            <p class="p"><input type="checkbox" value="rAll" <%=_Checked%>  id="rAll" style="cursor:pointer;" onclick="ChonTongHop(this.checked)" /><span style="font-size:9pt; line-height:22px; padding-left:4px;">Tổng hợp theo năm</span></p>
                            <div style="display:none;">
                                <%=MyHtmlHelper.TextBox(ParentID, OnOrOff, "iALL", "", "")%>
                            </div> 
                        </div>                                
                    </div>
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
            var Nam = document.getElementById("<%=ParentID %>_iNamLamViec").value
            var Quy = document.getElementById("<%=ParentID %>_iQuy").value
            var All = document.getElementById("<%=ParentID %>_iALL").value
            var LoaiDN = document.getElementById("<%=ParentID %>_iLoaiDN").value;
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("Get_dsDoanhNghiep?ParentID=#0&Quy=#1&Nam=#2&All=#3&iLoaiDN=#4&iMaDN=#5", "rptTCDN_TinhHinhThuNop_QuyNam_03") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", Quy));
            url = unescape(url.replace("#2", Nam));
            url = unescape(url.replace("#3", All));
            url = unescape(url.replace("#4", LoaiDN));
            url = unescape(url.replace("#5", "<%= iID_MaDoanhNghiep %>"));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_divDoanhNghiep").innerHTML = data;
            });
        }          
    </script>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptTCDN_TinhHinhThuNop_QuyNam_03", new { iQuy = iQuy, iNam = Nam, iAll = OnOrOff,iLoaiDN=iLoaiDN, iMaDN = iID_MaDoanhNghiep }), "Xuất ra excel")%>
    <iframe src="<%=urlReport%>" height="600px" width="100%">
    </iframe>
</body>
</html>
