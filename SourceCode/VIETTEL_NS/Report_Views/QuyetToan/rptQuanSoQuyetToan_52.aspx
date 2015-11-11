<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QuyetToan" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
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
    String MaND = User.Identity.Name;
    String ParentID = "QuyetToanQuanSo";
    String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
    DataTable dtTrangThai = rptQuanSoQuyetToan_52Controller.tbTrangThai();
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
    String iQuy = Convert.ToString(ViewData["iQuy"]);
    String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
    String iAll = Convert.ToString(ViewData["iAll"]);
    String UserID = User.Identity.Name;  
   
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
    dtNam.Rows[0]["TenNam"] = "-- Bạn chọn năm quyết toán --";
    SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");        
    // Đơn Vị
    DataTable dtDonVi = rptQuanSoQuyetToan_52Controller.GetDonvi(iQuy, iID_MaTrangThaiDuyet,MaND);    
    if (String.IsNullOrEmpty(iID_MaDonVi))
    {        
        iID_MaDonVi = Guid.Empty.ToString();
    }
    dtDonVi.Dispose();
    //Loai ngan sach
    DataTable dtQuy = DanhMucModels.DT_Quy();
    dtQuy.Rows.RemoveAt(0);
    SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
    if (String.IsNullOrEmpty(iQuy))
    {
        iQuy = "1";
    }
    String pageload = Convert.ToString(ViewData["pageload"]);
    String _Checked = "";
    if (String.IsNullOrEmpty(iAll))
    {
        iAll = "off";
    }
    if (iAll == "on")
        _Checked = "checked=\"checked\"";
    else if (iAll == "off")
        _Checked = "";
    String urlReport = "";
    if (pageload == "1")
        urlReport = Url.Action("ViewPDF", "rptQuanSoQuyetToan_52", new { iQuy = iQuy, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iAll = iAll, iMaDV = iID_MaDonVi });
    dtQuy.Dispose();
    String URL = Url.Action("Index", "QuyetToan_QuanSo_Report");
    using (Html.BeginForm("EditSubmit", "rptQuanSoQuyetToan_52", new { ParentID = ParentID }))
    {
    %>   
     <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo quân số quyết toán quý</span>
                    </td>
                </tr>
            </table>
        </div>        
        <div id="table_form2" class="table_form2">
            <div id="" style="width:850px; max-width:850px; margin:0px auto; padding:0px 0px; overflow:visible; ">
                <div id="main" style="width:850px; float:left; border:1px solid #cecece; border-radius:3px; height:40px; text-align:center;padding:4px 0px 2px 0px;">              
                    <div id="Left" style="float:left; width:400px;">
                        <div style="float:left; width:200px; ">                                         
                            <div class="div-label" style="width:100px; text-align:center;float:left;">
                                <p class="p"><%=NgonNgu.LayXau("Chọn quý")%></p>                      
                            </div>
                            <div class="div-label" style="width:100px; float:right; text-align:left; ">
                                <p class="p"><%=MyHtmlHelper.DropDownList(ParentID, slQuy, iQuy, "iQuy", "", "class=\"input1_2\" style=\"width:100%; padding:2px;\" onchange=\"ChonDonvi()\"")%></p>                                    
                            </div>                       
                        </div><!-----------End doanh nghiệp-----------> 
                        <div style="float:left; width:200px;">              
                            <div class="div-label" style="width:100px; text-align:center;float:left;">                                
                                <p class="p"><%=NgonNgu.LayXau("Trạng Thái :")%></p>                                
                            </div>
                            <div class="div-label" style="width:100px; float:right; text-align:left; ">                               
                                <p class="p"><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%;padding:2px;\" onchange=\"ChonDonvi()\"")%></p>                                
                            </div>                       
                        </div><!-----------End năm làm việc----------->
                    </div>
                    <div id="Right" style="float:right;width:450px;">
                        <div style="float:left; width:300px;">
                            <div class="div-label" style="width:150px; text-align:center;float:left;">                                
                                <p class="p"><%=NgonNgu.LayXau("Chọn đơn vị")%></p>                                
                            </div>
                            <div class="div-label" style="width:150px; text-align:center;float:left;">                    
                                <p class="p" id="<%=ParentID %>_divDonvi">                                   
                                    <%rptQuanSoQuyetToan_52Controller rpt = new rptQuanSoQuyetToan_52Controller();%>
                                    <%=rpt.obj_DSDonvi(ParentID, iQuy, iID_MaTrangThaiDuyet, iID_MaDonVi,MaND)%>                                          
                                </p>                                                         
                            </div>
                        </div>
                        <div style="float:right; width:150px; padding-top:10px;">
                            <p class="p"><input type="checkbox" value="rAll" <%=_Checked%>  id="rAll" style="cursor:pointer;" onclick="ChonTongHop(this.checked)" /><span style="font-size:9pt; line-height:22px; padding-left:4px;">Tất cả các đơn vị</span></p>
                            <div style="display:none;">
                                <%=MyHtmlHelper.TextBox(ParentID, iAll, "iALL", "", "")%>
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
    <div>
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
        }
        function ChonDonvi() {
            var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value
            var Quy = document.getElementById("<%=ParentID %>_iQuy").value
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("Get_dsDonvi?ParentID=#0&iQuy=#1&iID_MaTrangThaiDuyet=#2&iMaDV=#3", "rptQuanSoQuyetToan_52") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", Quy));
            url = unescape(url.replace("#2", iID_MaTrangThaiDuyet));
            url = unescape(url.replace("#3", "<%= iID_MaDonVi %>"));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_divDonvi").innerHTML = data;
            });            
        }     
        </script>
    </div>  
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptQuanSoQuyetToan_52", new { iQuy = iQuy, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iAll = iAll, iMaDV = iID_MaDonVi }), "ExportToExcel")%>
     <iframe src="<%=urlReport%>" height="600px" width="100%">
     </iframe>  
</body>
</html>
