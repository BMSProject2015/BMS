<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" culture="vi-VN" uiCulture="vi-VN" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%
    String MaND = User.Identity.Name;
    String ParentID = "KeToan";
    DataTable dtThang = DanhMucModels.DT_Thang();
    DataRow R2;
    R2 = dtThang.NewRow();
    R2["MaThang"] = 13;
    R2["TenThang"] = "Quý I";
    dtThang.Rows.Add(R2);
    R2 = dtThang.NewRow();
    R2["MaThang"] = 14;
    R2["TenThang"] = "Quý II";
    dtThang.Rows.Add(R2);
    R2 = dtThang.NewRow();
    R2["MaThang"] = 15;
    R2["TenThang"] = "Quý III";
    dtThang.Rows.Add(R2);
    R2 = dtThang.NewRow();
    R2["MaThang"] = 16;
    R2["TenThang"] = "Quý IV";
    dtThang.Rows.Add(R2);
    R2 = dtThang.NewRow();
    R2["MaThang"] = 17;
    R2["TenThang"] = "6 Tháng đầu năm";
    dtThang.Rows.Add(R2);
    R2 = dtThang.NewRow();
    R2["MaThang"] = 18;
    R2["TenThang"] = "6 Tháng cuối năm";
    dtThang.Rows.Add(R2);
    R2 = dtThang.NewRow();
    R2["MaThang"] = 19;
    R2["TenThang"] = "Cả năm";
    dtThang.Rows.Add(R2);
    SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");


    String iNam = Convert.ToString(ViewData["iNam"]);

    if (String.IsNullOrEmpty(iNam))
    {
        iNam = DanhMucModels.NamLamViec(MaND).ToString();
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
    
    String iThang = Convert.ToString(ViewData["iThang"]);
    if (String.IsNullOrEmpty(iThang)) iThang =DanhMucModels.ThangLamViec(MaND).ToString();
    dtThang.Dispose();
    String Pageload = Convert.ToString(ViewData["Pageload"]);
    String sThongBao = Convert.ToString(ViewData["sThongBao"]);   
    String OK = Convert.ToString(ViewData["OK"]);

    DataTable dtKhoGiay = ReportModels.LoaiKhoGiay();
    String KhoGiay = Convert.ToString(ViewData["KhoGiay"]);
    SelectOptionList slKhoGiay = new SelectOptionList(dtKhoGiay, "MaKhoGiay", "TenKhoGiay");
    if (String.IsNullOrEmpty(KhoGiay)) KhoGiay = "2";
    String urlReport = "";    
    if (Pageload == "1" && (OK=="OK"||OK=="CANCEL"))
    {
       urlReport=Url.Action("ViewPDF", "rptKeToanTongHop_CanDoiTaiKhoan", new { iNam = iNam, iThang = iThang,KhoGiay=KhoGiay });
    }
    String urlsubmit = Url.Action("EditSubmit", "rptKeToanTongHop_CanDoiTaiKhoan", new { ParentID = ParentID,iNam = iNam, iThang = iThang,KhoGiay=KhoGiay});
    //String BackURL = Url.Action("Index", "KeToan_Report", new { sLoai = "1" });
    String BackURL = Url.Action("SoDoLuong", "KeToanTongHop", new { sLoai = "1" });
    DataTable dtTrangThai = HamChung.GetTrangThai(PhanHeModels.iID_MaPhanHeKeToanTongHop, LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop), true, "--Tất cả--");
    SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
    String iTrangThai = Request.QueryString["iID_MaTrangThaiDuyet"];
    if (String.IsNullOrEmpty(iTrangThai))
        iTrangThai = dtTrangThai.Rows.Count > 0 ? Convert.ToString(dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"]) : Guid.Empty.ToString();
    using (Html.BeginForm("EditSubmit", "rptKeToanTongHop_CanDoiTaiKhoan", new { ParentID = ParentID }))
    { 
%>
<%=MyHtmlHelper.Hidden(ParentID,OK,"OK","") %>
<%if (String.IsNullOrEmpty(sThongBao) == false && String.IsNullOrEmpty(OK))
  { %>
    <script type="text/javascript">
        conf();
        function conf() {
            var cn = confirm('<%=sThongBao%>')
            if (cn) {
                document.getElementById('KeToan_OK').value = "OK";               
                window.location.href = '<%=urlsubmit%>&OK=OK';
            }
            else {
                document.getElementById('KeToan_OK').value = "CANCEL";
                window.location.href = '<%=urlsubmit%>&OK=CANCEL';
            }
            
        }
        </script>
        
 <%}%>
<style type="text/css">        
        div.login1{
            text-align : center;    
            background: transparent url(/Content/Report_Image/login.gif) no-repeat top center;
        }    
        div.login1 a,div.login1 span{
            color: white;
            text-decoration: none;
            font: bold 16px "Museo 700";
            display: block;
            height: 20px;
            padding:1px;
            width:90px;
            line-height: 20px;
            margin: 0px auto;
            -moz-border-radius:2px;
            -webkit-border-radius:2px;
            border-radius:2px;
            cursor:pointer;
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
	        opacity:0.1;	        
	        z-index:100000;
            display: none;   
            border:none;
            cursor:default;
        }
        #confirmBox{
	        background-color:#f1f1f1;
	        width:400px;	        
	        max-height:300px;	        
	        position:fixed;
	        left:50%;
	        top:50%;	       
	        margin:-100px 0 0 -200px;        
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
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span>Báo cáo cân đối tài khoản</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="table_form2" class="table_form2">            
        <div style="margin:0 auto; height:25px; text-align:center; cursor:pointer;" class="login1"><span>Xem báo cáo</span></div>
    </div><!---End #table_form2--->    
</div>
<div id="confirmOverlay"></div>
<div id="confirmBox">
    <h4>Bảng cân đối tài khoản năm <%=iNam %><a href="#" style="float:right; padding:1px 2px;">Thoát [Esc]</a></h4>    
    <div>
        <div id="left" style="float:left; width:226px; text-align:right; line-height:38px; padding:2px;">
            <p><span style="padding:2px; font-size:14px; font-family: Tahoma Arial;"><%=NgonNgu.LayXau("Thời gian") %></span><%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang, "iThang", "", "class=\"input1_2\" style=\"width: 100px; padding:2px;\" onchange=\"CheckKhoaSo(this.value)\" ")%></p>
            <p style="display:none;"><span style="padding:2px; font-size:14px; font-family: Tahoma Arial; "><%=NgonNgu.LayXau("Chọn trạng thái duyệt") %></span><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iTrangThai, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100px; padding:2px;\" ")%></p>
        </div>
        <div id="right" style="float:right; width:150px; text-align:left;">
            <fieldset>
                <legend><%=NgonNgu.LayXau("Chọn khổ giấy in") %></legend>
                <p><%=MyHtmlHelper.Option(ParentID, "2", KhoGiay, "rKhoGiay", "", "onchange=\"ChonKho()\"")%><span>In khổ giấy A4</span></p>
                <p><%=MyHtmlHelper.Option(ParentID, "1", KhoGiay, "rKhoGiay", "", "onchange=\"ChonKho()\"")%><span>In khổ giấy A3</span></p>
            </fieldset>
            <div style="display:none;">
                <%=MyHtmlHelper.TextBox(ParentID,KhoGiay,"KhoGiay","","") %>
            </div>
            <%=MyHtmlHelper.Hidden(ParentID, iNam, "iNam", "")%>
        </div>     
    </div>
    <p class="p"><input style="display:inline-block; margin-right:5px;" type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" /><input style="display:inline-block; margin-left:5px;" class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" /></p>    
</div>
<script type="text/javascript">
    CheckKhoaSo('<%=iThang %>');
    function CheckKhoaSo(value) {
        jQuery.ajaxSetup({ cache: false });
        var url = unescape('<%= Url.Action("CheckKhoaSo?iThang=#0&iNam=#1", "rptKeToanTongHop_CanDoiTaiKhoan") %>');
        url = unescape(url.replace("#0", value));
        url = unescape(url.replace("#1", '<%=iNam %>'));
        $.getJSON(url, function (sThangChuaKhoaSo) {
            if (sThangChuaKhoaSo !="") {
                alert('Tháng ' + sThangChuaKhoaSo + ' chưa được khóa sổ');
            }
        });
    }

    function Huy() {
        window.location.href = '<%=BackURL %>';
    }
    function ChonKho() {
        var TenPage = document.getElementsByName("<%=ParentID %>_rKhoGiay");
        var pages;
        var i = 0;
        for (i = 0; i < TenPage.length; i++) {
            if (TenPage[i].checked) {
                pages = TenPage[i].value;
            }
        }
        document.getElementById("<%= ParentID %>_KhoGiay").value = pages;
    }
    $(function () {
        $('*').keyup(function (e) {
            if (e.keyCode == '27') {
                Hide();
            }
        });
        $(".login1 span").click(function () {
            Show();
        });
        $("#confirmBox h4 a").click(function () {
            Hide();
        });
        if ("<%=Pageload %>" == "0") {
            Show();
        }
    });
    function Show() 
    {
        $("#confirmOverlay").show();
        $("#confirmBox").show();
        $("#table_form2").hide();
    }
    function Hide() {
        $("#confirmOverlay").hide();
        $("#confirmBox").hide();
        $("#table_form2").show();
    }
 </script>
<%} %>
<%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptKeToanTongHop_CanDoiTaiKhoan", new { iNam = iNam, iThang = iThang,KhoGiay=KhoGiay }), "ExportToExcel")%>
<div>
    <iframe  src="<%=urlReport%>"  height="600px" width="100%"></iframe>
</div>