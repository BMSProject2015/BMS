<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.TongHop" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">        
        div.login1{
            text-align : center;    
            background: transparent url(/Content/Report_Image/login.gif) no-repeat top center;
        }    
        div.login1 a{
            color: white;
            text-decoration: none;
            font: bold 16px "Museo 700";
            display: block;
            height: 20px;
            padding:2px;
            width:100px;
            line-height: 20px;
            margin: 0px auto;
            -moz-border-radius:2px;
            -webkit-border-radius:2px;
            border-radius:2px;
        }    
        div.login1 a.active {background-position: 20px 1px;}
        div.login1 a:active, a:focus {outline: none;}
        .label{font-weight:bold;}
        #confirmOverlay{
	        width:100%;
	        height:100%;
	        position:fixed;
	        top:0;
	        left:0;	        
	        background-color:Black;
	        opacity:0.6;	        
	        z-index:100000;
            display: none;   
            border:none;
            cursor:default;
        }
        #confirmBox{
	        background-color:#f1f1f1;
	        width:500px;	        
	        min-height:300px;	        
	        /*position:fixed;*/
	        left:50%;
	        top:50%;	       
	        /*margin:-130px 0 0 -230px;   */   
	        border: 1px solid white;
	        -moz-box-shadow: 0 0 4px 2px #888;
	        -webkit-box-shadow: 0 0 4px 2px #888;	          
	        overflow:hidden;
	        cursor:default;
	        z-index:100001;
	        display:none;	        
	        border-radius:2px;
	        -webkit-border-radius:2px;
	        -moz-border-radius:2px;
	        box-shadow: 0 0 4px 2px #888;
        }       
        #confirmBox h4{
	        letter-spacing:0.3px;
	        padding:3px 3px 3px 5px; background-color:#dedede;
	        font-size:12px;
	        font-family:Arial;
	        color:Black; line-height:20px;border-bottom:1px solid #cacaca;	
        }    
        #confirmBox >div{
            padding:4px;                        
            overflow:hidden;  
        }    
        #confirmBox .p
        {
            background-color:#dedede;
            opacity:1;
            padding:3px;
            text-align:center;
            color:White;
            font-family:Arial;
            font-size:12px;
            clear:both;
            line-height:20px;
        }
        #confirmBox #right  
        {
          padding:4px;
          font-size:13px;
          font-family: Tahoma Arial;
          color:Black;       
        }
        #confirmBox #right >fieldset
        {
           border:1px dashed;
           padding:3px 5px;
           border-radius:3px;
           -moz-border-radius:3px;
           -webkit-border-radius:3px; 
           font-size:14px;
        } 
        #confirmBox #right >fieldset legend
        {
            padding:3px 3px 1px 5px;
            }
        #confirmBox #right >fieldset p 
        {
            padding:1px 1px 1px 3px;
        }            
        #confirmBox #right >fieldset p span
        {
           padding:1px 1px 1px 5px;
            }   
        #right p label
        {
            font-size:14px;
            padding:4px;
            }    
    </style>
    <script type="text/javascript" language="javascript" src="../../../Scripts/jquery-latest.js"></script>    
    <script type="text/javascript" src="../../../Scripts/Report/jquery-1.8.0.min.js"></script>
    <script type="text/javascript" src="../../../Scripts/Report/jquery-ui-1.8.23.custom.min.js"></script>
    <script type="text/javascript" src="../../../Scripts/Report/ShowDialog.js"></script>
</head>
<body>
    <%
    String srcFile = Convert.ToString(ViewData["srcFile"]);
    String ParentID = "KeToan";
    String MaND = User.Identity.Name;    
    String iNamLamViec = Convert.ToString(ViewData["iNamLamViec"]);    
    if (String.IsNullOrEmpty(iNamLamViec))
    {
        iNamLamViec = DanhMucModels.NamLamViec(MaND).ToString();
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
    SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
    //Mã tài khoản            
    String iID_MaTaiKhoan = Convert.ToString(ViewData["iID_MaTaiKhoan"]);     
    String cap = Convert.ToString(ViewData["CapTK"]);
    if (String.IsNullOrEmpty(cap))
        cap = "3";
    var dtiID_MaTaiKhoan = rptKTTongHop_QuanHeDoiUng_TaiKhoanController.GetTaiKhoan(iNamLamViec, cap);   
    if (String.IsNullOrEmpty(iID_MaTaiKhoan))
    {
        iID_MaTaiKhoan = dtiID_MaTaiKhoan.Rows.Count > 0 ? Convert.ToString(dtiID_MaTaiKhoan.Rows[0]["iID_MaTaiKhoan"]) : Guid.Empty.ToString();
    }        
    
    String iThang1 = Convert.ToString(ViewData["iThang1"]);
    String iThang2 = Convert.ToString(ViewData["iThang2"]);
    String iNgay1 = Convert.ToString(ViewData["iNgay1"]);
    String iNgay2 = Convert.ToString(ViewData["iNgay2"]);         
    int ThangHienTai = DateTime.Now.Month;
    if (String.IsNullOrEmpty(iThang1))
        iThang1 = DanhMucModels.ThangLamViec(MaND).ToString();
    if (String.IsNullOrEmpty(iThang2))
        iThang2 = DanhMucModels.ThangLamViec(MaND).ToString();
    //Chọn từ tháng
    DataTable dtThang = DanhMucModels.DT_Thang(false);
    SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
    //Chọn từ ngày    
    if (String.IsNullOrEmpty(iNgay1))
    {
        iNgay1 = "1";
    }
    //dtNgay.Dispose();
    if (String.IsNullOrEmpty(iNgay2))
        iNgay2 = "28";
    String pageload = Convert.ToString(ViewData["pageload"]);
    String urlReport = pageload.Equals("1")?Url.Action("ViewPDF","rptKTTongHop_QuanHeDoiUng_TaiKhoan",new{ iNamLamViec = iNamLamViec, iID_MaTaiKhoan = iID_MaTaiKhoan, iThang1 = iThang1,iThang2 = iThang2, iNgay1 = iNgay1, iNgay2 = iNgay2 }):"";
                
    String BackURL = Url.Action("Index", "KeToan_Report", new { sLoai = "1" });
    using (Html.BeginForm("EditSubmit", "rptKTTongHop_QuanHeDoiUng_TaiKhoan", new { ParentID = ParentID }))
    {
    %>   
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr><td><span>Báo cáo quan hệ đối ứng tài khoản</span></td></tr>
            </table>
        </div>        
        <div id="table_form2" class="table_form2">            
            <div style="margin:0 auto; height:25px; text-align:center; cursor:pointer;" class="login1"><a>Xem báo cáo</a></div>
        </div><!---End #table_form2--->
    </div>    
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=BackURL %>';
        }
        function ChonCap() {
            var TenPage = document.getElementsByName("<%=ParentID %>_TKCap");
            var pages;
            var i = 0;
            for (i = 0; i < TenPage.length; i++) {
                if (TenPage[i].checked) {
                    pages = TenPage[i].value;
                }
            }
            document.getElementById("<%= ParentID %>_divCap").value = pages;
            ChonTaiKhoan();
        }
        function ChonTaiKhoan() {
            var Nam = document.getElementById("<%=ParentID %>_iNamLamViec").value
            var cap = document.getElementById("<%=ParentID %>_divCap").value
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("Get_dsTaiKhoan?ParentID=#0&iNam=#1&LenTK=#2&iID_MaTaiKhoan=#3","rptKTTongHop_QuanHeDoiUng_TaiKhoan") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", Nam));
            url = unescape(url.replace("#2", cap));
            url = unescape(url.replace("#3", "<%=iID_MaTaiKhoan %>>"));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_divTaiKhoan").innerHTML = data;
            });
        }
        function ChonNgay(idNgay, idThang, divNgay, FromOrTo) {
            var Nam = document.getElementById("<%=ParentID %>_iNamLamViec").value
            var Thang = document.getElementById("<%=ParentID %>_" + idThang).value
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("Get_dsNgay?ParentID=#0&iThang=#1&iNam=#2&iNgay=#3&FromOrTo=#4", "rptKTTongHop_QuanHeDoiUng_TaiKhoan") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", Thang));
            url = unescape(url.replace("#2", Nam));
            url = unescape(url.replace("#3", idNgay));
            url = unescape(url.replace("#4", FromOrTo));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_" + divNgay).innerHTML = data;
            });
        }
        function TuNgay() {
            ChonNgay("<%=iNgay1 %>", "iThang1", "divTuNgay", "iNgay1");
        }
        function DenNgay() {
            ChonNgay("<%=iNgay2 %>", "iThang2", "divDenNgay", "iNgay2");
        }
        function chonNam() {
            TuNgay();
            DenNgay();
            ChonTaiKhoan();
        }
        $(function () {
            $("#<%=ParentID %>_iThang1").change(function () {
                var iTuThang = parseInt($(this).val());
                var iDenThang = parseInt($("#<%=ParentID %>_iThang2").val());
                if (iTuThang > iDenThang) {
                    $("#<%=ParentID %>_iThang2 option[value=" + (iTuThang) + "]").attr('selected', true);
                    DenNgay();
                }
            });
            $("#<%=ParentID %>_iThang2").change(function () {
                var iDenThang = parseInt($(this).val());
                var iTuThang = parseInt($("#<%=ParentID %>_iThang1").val());
                if (iDenThang < iTuThang) {
                    $("#<%=ParentID %>_iThang1 option[value=" + iDenThang + "]").attr('selected', true);
                    TuNgay();
                }
            });
            $("input:radio").bind('click', function () {
                $('.label').removeClass('label');
                $(this).next().addClass('label');
            });
            $("input:radio").each(function () {
                if ($(this).val() == '<%=cap %>') {
                    $(this).next().addClass('label');
                }
            });
            var max = 0;
            $('#right fieldset p span').each(function () {
                if ($(this).width() > max)
                    max = $(this).width();
            });
            $('#right fieldset p span').width(max);
            $('label.labelWidth').css({ 'display': 'inline-block' }).width(107);
            $(".login1 a").click(function () {
                ShowDialog(500);
            });
            $('*').keyup(function (e) {
                if (e.keyCode == '27') {
                    Hide();
                }
            });
            if ("<%=pageload %>" == "0") {
                ShowDialog(500);
            }
        });      
    </script>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptKTTongHop_QuanHeDoiUng_TaiKhoan", new { iNamLamViec = iNamLamViec, iID_MaTaiKhoan = iID_MaTaiKhoan, iThang1 = iThang1, iThang2 = iThang2, iNgay1 = iNgay1, iNgay2 = iNgay2 }), "Xuất ra file Excel")%>
    
    <div id="confirmBox" title="Chọn tài khoản cần phân tích">          
        <div>
            <div id="left" style="float:left; width:250px; text-align:left;">
                <span id="<%= ParentID %>_divTaiKhoan">  
                    <% rptKTTongHop_QuanHeDoiUng_TaiKhoanController rpt = new rptKTTongHop_QuanHeDoiUng_TaiKhoanController(); %>                          
                    <%=rpt.obj_DSTaiKhoan(ParentID,iNamLamViec,cap,iID_MaTaiKhoan) %>
                </span>
            </div>
            <div id="right" style="float:right; width:230px; text-align:left;">                        
                <fieldset>
                    <legend><%=NgonNgu.LayXau("Chọn TK cấp") %></legend>
                    <p><%=MyHtmlHelper.Option(ParentID, "3", cap ,"TKCap", "", "onchange=\"ChonCap()\"")%><span>Cấp 1</span></p>
                    <p><%=MyHtmlHelper.Option(ParentID, "4", cap, "TKCap", "", "onchange=\"ChonCap()\"")%><span>Cấp 2</span></p>                           
                    <p><%=MyHtmlHelper.Option(ParentID, "5", cap, "TKCap", "", "onchange=\"ChonCap()\"")%><span>Cấp 3</span></p>
                    <p><%=MyHtmlHelper.Option(ParentID, "6", cap, "TKCap", "", "onchange=\"ChonCap()\"")%><span>Tất cả</span></p>                            
                </fieldset>
                <div style="display:none;"><%=MyHtmlHelper.TextBox(ParentID,cap,"divCap","","") %></div>       
                <p><label style="line-height:20px; height:20px; padding:3px; display:inline-block; width:107px;"><%=NgonNgu.LayXau("Chọn năm") %></label><%=MyHtmlHelper.DropDownList(ParentID, slNam, iNamLamViec, "iNamLamViec", "", "class=\"input1_2\" style=\"width: 70px; padding:2px;\" onchange=\"chonNam()\"")%></p>                        
                <p style="display:none;"><label style="line-height:20px; height:20px; padding:3px; display:inline-block; width:107px;"><%=NgonNgu.LayXau("Số dư còn lại tăng") %></label><%=MyHtmlHelper.DropDownList(ParentID, slNam, iNamLamViec, "", "", "class=\"input1_2\" style=\"width: 70px; padding:2px;\"")%></p>
                <fieldset>
                    <legend><%=NgonNgu.LayXau("Từ ngày&nbsp;&nbsp;Tháng&nbsp;&nbsp;&nbsp;&nbsp;Đến ngày&nbsp;&nbsp;Tháng")%></legend>
                    <p>                   
                        <label id="<%=ParentID %>_divTuNgay"><%=rpt.obj_DSNgay(ParentID,iThang1,iNamLamViec,iNgay1,"iNgay1") %></label>
                        <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang1, "iThang1", "", "class=\"input1_2\" style=\"width:45px; padding:2px;\" onchange=\"TuNgay()\"")%>                                             
                        <label id="<%=ParentID %>_divDenNgay"><%=rpt.obj_DSNgay(ParentID,iThang2,iNamLamViec,iNgay2,"iNgay2") %></label>
                        <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang2, "iThang2", "", "class=\"input1_2\" style=\"width:45px; padding:2px;\" onchange=\"DenNgay()\"")%>
                    </p>
                </fieldset>                
            </div>
        </div>
        <p class="p"><input style="display:inline-block; margin-right:5px;" type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" /><input style="display:inline-block; margin-left:5px;" class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" /></p>    
    </div>        
    <%} %>
    <iframe src="<%=urlReport%>" height="600px" width="100%"></iframe>    
</body>
</html>