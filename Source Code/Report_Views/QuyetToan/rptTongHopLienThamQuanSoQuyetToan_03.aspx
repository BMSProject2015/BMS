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
    <title>BÁO CÁO QUÂN SỐ QUYẾT TOÁN</title>
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
    String ParentID = "QuyetToanQuanSo";
    String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
    String iThang = Convert.ToString(ViewData["iThang"]);    
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
    // Tháng    
    DataTable dtThang = DanhMucModels.DT_Thang(false);
    SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
    if (String.IsNullOrEmpty(iThang))
    {
        iThang = DanhMucModels.ThangLamViec(UserID).ToString();
    }
    //dt Trạng thái duyệt
    DataTable dtTrangThai = rptTongHopLienThamQuanSoQuyetToan_03Controller.tbTrangThai();
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
    String pageload = Convert.ToString(ViewData["pageload"]);    
    String urlReport = "";
    if (pageload == "1")
        urlReport = Url.Action("ViewPDF", "rptTongHopLienThamQuanSoQuyetToan_03", new { iThang = iThang, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
    dtThang.Dispose();
    String URL = Url.Action("Index", "QuyetToan_QuanSo_Report");
    using (Html.BeginForm("EditSubmit", "rptTongHopLienThamQuanSoQuyetToan_03", new { ParentID = ParentID }))
    {
    %>   
     <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo tổng hợp liên thẩm quân số quyết toán</span>
                    </td>
                </tr>
            </table>
        </div>        
        <div id="table_form2" class="table_form2">
            <div id="" style="width:450px; max-width:450px; margin:0px auto; padding:0px 0px; overflow:visible; ">
                <div id="main" style="width:450px; float:left; border:1px solid #cecece; border-radius:3px; height:40px; text-align:center;padding:4px 0px 2px 0px;">              
                    <div id="Left" style="float:left; width:400px;">
                        <div style="float:left; width:200px; ">                                         
                            <div class="div-label" style="width:100px; text-align:center;float:left;">
                                <p class="p"><%=NgonNgu.LayXau("Chọn tháng")%></p>                      
                            </div>
                            <div class="div-label" style="width:100px; float:right; text-align:left; ">
                                <p class="p"><%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang, "iThang_Quy", "", "class=\"input1_2\" style=\"width:100%; padding:2px;\"")%></p>                                    
                            </div>                       
                        </div><!-----------End doanh nghiệp-----------> 
                        <div style="float:left; width:200px;">              
                            <div class="div-label" style="width:100px; text-align:center;float:left;">                                
                                <p class="p"><%=NgonNgu.LayXau("Trạng Thái ")%></p>                                
                            </div>
                            <div class="div-label" style="width:100px; float:right; text-align:left; ">                               
                                <p class="p"><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%;padding:2px;\"")%></p>                                
                            </div>                       
                        </div><!-----------End năm làm việc----------->
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
        </script>
    </div>  
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptTongHopLienThamQuanSoQuyetToan_03", new { iThang = iThang, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet }), "ExportToExcel")%>
     <iframe src="<%=urlReport%>" height="600px" width="100%">
     </iframe>  
</body>
</html>
