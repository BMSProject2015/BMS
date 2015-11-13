<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        div.login1{ text-align : center; background: transparent url(/Content/Report_Image/login.gif) no-repeat top center;
            border-bottom-left-radius:10px;
            border-bottom-right-radius:10px;
            -moz-border-radius-bottomright:10px;
            -moz-border-radius-bottomleft:10px;
            -webkit-border-bottom-right-radius:10px;
            -webkit-border-bottom-left-radius:10px;        
            margin:0 auto; height:25px; cursor:pointer; width:90px; font-weight:bold; line-height:23px; padding:2px; font-size:15px; color:White; font-family:Tahoma Arial;    
        }    
        div.login1 a{ color: white;text-decoration: none;font: bold 16px "Museo 700";display: block;height: 20px;padding:2px;width:100px;line-height: 20px;
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
            opacity:0.1;
            z-index:100000;
            display: none;
            border:none;
            cursor:default;
        }
        #confirmBox{
	        background-color:#f1f1f1;
	        width:320px;	        
	        min-height:300px;       
	        /*position:fixed;*/
	        left:50%;
	        top:50%;	       
	        /*margin:-230px 0 0 -250px;        */
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
	        /*box-shadow: 0 0 4px 2px #888;*/
	        -webkit-box-shadow: rgba(200, 200, 200, 0.7) 0 4px 10px -1px;
            box-shadow: rgba(200, 200, 200, 0.7) 0 4px 10px -1px;
        }       
        #confirmBox h4{
	        letter-spacing:0.3px;padding:3px 3px 3px 5px; background-color:#dedede;font-size:12px;
	        font-family:Arial;color:Black; line-height:20px;border-bottom:1px solid #cacaca;	
        }    
        #confirmBox >div{
            padding:4px;                        
            overflow:hidden;  
        }    
        #confirmBox .p{
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
        #confirmBox #right,#confirmBox #left{
            padding:4px;
            font-size:13px;
            font-family: Tahoma Arial;
            color:Black;       
        }
        #confirmBox #right >fieldset,#confirmBox #left >fieldset{
            border:1px solid #cecece;
            padding:3px 5px;
            border-radius:3px;
            -moz-border-radius:3px;
            -webkit-border-radius:3px; 
            font-size:14px;
        } 
        #confirmBox #right >fieldset legend,#confirmBox #left >fieldset legend{padding:3px 3px 1px 5px;}
        #confirmBox #right >fieldset p {padding:1px 1px 1px 3px;}            
        #confirmBox #right >fieldset p span{padding:1px 1px 1px 5px;}   
        #right p label{font-size:14px;padding:4px;}    
    </style>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/jquery-latest.js") %>"></script>
    <script src="<%= Url.Content("~/Scripts/Report/jquery-1.8.0.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/Report/jquery-ui-1.8.23.custom.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/Report/ShowDialog.js") %>" type="text/javascript"></script>
</head>
<body>
    <% 
        String ParentID = "KeToan";
        String sLoaiMau="",sTenMau="";
        sLoaiMau = Request.QueryString["sLoaiMau"];
        sTenMau = Request.QueryString["sTenMau"];
        String NamLamViec = Request.QueryString["NamLamViec"];
        String UserID=User.Identity.Name;
        if (String.IsNullOrEmpty(NamLamViec))
        {
            NamLamViec = DanhMucModels.NamLamViec(UserID).ToString();
        }
        DateTime dNgayHienTai = DateTime.Now;
        String NamHienTai = Convert.ToString(dNgayHienTai.Year);
        DataTable dtNam = DanhMucModels.DT_Nam();
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        dtNam.Dispose();

        String ThangLamViec = Request.QueryString["ThangLamViec"];
        if (String.IsNullOrEmpty(ThangLamViec))
        {
            ThangLamViec = DanhMucModels.ThangLamViec(UserID).ToString();
        }      
        DataTable dtThang = DanhMucModels.DT_Thang();
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        dtThang.Dispose();
        
        String LoaiBiaSo = Request.QueryString["LoaiBiaSo"];
        if (String.IsNullOrEmpty(LoaiBiaSo))
        {
            LoaiBiaSo = "1";
        } 
        DataTable dtLoaiBiaSo = rptKeToan_InBiaController.LoaiBiaSo();
        SelectOptionList slLoaiBiaSo = new SelectOptionList(dtLoaiBiaSo, "MaLoaiBiaSo", "TenLoaiBiaSo");
        dtLoaiBiaSo.Dispose();
        
        String LoaiBia = Request.QueryString["LoaiBia"];
        if (String.IsNullOrEmpty(LoaiBia))
        {
            LoaiBia = "1";
        } 
        DataTable dtLoaiBia = rptKeToan_InBiaController.LoaiBia();
        SelectOptionList slLoaiBia = new SelectOptionList(dtLoaiBia, "MaLoaiBia", "TenLoaiBia");
        dtLoaiBia.Dispose();
        
        String TenDonVi = Request.QueryString["TenDonVi"];
        DataTable dtDonVi = DonViModels.DanhSach_DonVi();
        if (String.IsNullOrEmpty(TenDonVi))
        {
            TenDonVi = dtDonVi.Rows[0]["TenHT"].ToString();
        }
        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "sTen", "TenHT");
        dtDonVi.Dispose();
        
        String LoaiKhoGiay = Request.QueryString["LoaiKhoGiay"];
        if (String.IsNullOrEmpty(LoaiKhoGiay))
        {
            LoaiKhoGiay = "1";
        } 
        DataTable dtLoaiKhoGiay = rptKeToan_InBiaController.LoaiKhoGiay();
        SelectOptionList slLoaiKhoGiay = new SelectOptionList(dtLoaiKhoGiay, "MaLoaiGiay", "TenLoaiGiay");
        dtLoaiKhoGiay.Dispose();
        
        int TuTo =Convert.ToInt32(Request.QueryString["TuTo"]);
        if (TuTo==0)
        {
            TuTo = 1;
        }
        int DenTo = Convert.ToInt32(Request.QueryString["DenTo"]);
        if (DenTo==0)
        {
            DenTo = 2;
        }
        
        String LoaiNam_Thang =  Request.QueryString["LoaiNam_Thang"];
        if (String.IsNullOrEmpty(LoaiNam_Thang))
        {
            LoaiNam_Thang = "1";
        }
        String BackURL = Url.Action("SoDoLuong", "KeToanTongHop");
        String pageload = Convert.ToString(Request.QueryString["pageload"]);        
        using (Html.BeginForm("EditSubmit", "rptKeToan_InBia", new { ParentID = ParentID }))
        {          
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo sổ</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="table_form2" class="table_form2">            
            <div class="login1">Xem báo cáo</div>
        </div><!---End #table_form2--->          
    </div>
    <div id="confirmOverlay"></div>
    <div id="confirmBox" title="In bìa sổ">        
        <div>
            <div id="left" style="float:left; width:235px; text-align:left;">
                <fieldset style="height:150px;">
                    <legend><%=NgonNgu.LayXau("Chọn loại bìa sổ") %></legend>
                    <p><%=MyHtmlHelper.DropDownList(ParentID, slLoaiBiaSo, LoaiBiaSo, "LoaiBiaSo", "", "class=\"input1_2\" style=\"width: 200px; padding:4px 2px 4px 2px;\" size='5' tab-index='-1'")%></p>                                  
                    <p style="padding:4px; line-height:25px;">
                        <span style="padding:4px;"><%=NgonNgu.LayXau("Từ số: ") %></span><%=MyHtmlHelper.TextBox(ParentID, TuTo, "iTuTo", "", "class=\"input1_2\" style=\"width:40px; text-align:right;\"", 0)%>
                        <span style="padding:4px;"><%=NgonNgu.LayXau("Đến số: ") %></span><%=MyHtmlHelper.TextBox(ParentID, DenTo, "iDenTo", "", "class=\"input1_2\" style=\"width:40px; text-align:right;\"", 0)%>
                    </p>
                </fieldset>
            </div>
            <div id="right" style="float:right; width:235px; text-align:left;">
                <fieldset>
                    <legend><%=NgonNgu.LayXau("Chọn bìa cho") %></legend>
                    <p><%=MyHtmlHelper.DropDownList(ParentID, slLoaiBia, LoaiBia, "LoaiBia", "", "class=\"input1_2\" style=\"width: 200px; padding:2px;\" size='2' tab-index='-1'")%></p>                    
                </fieldset>                
                <fieldset>
                    <legend><%=NgonNgu.LayXau("Chọn thời gian") %></legend>
                    <p><%=MyHtmlHelper.Option(ParentID, "1", LoaiNam_Thang, "LoaiNam_Thang", "", "onchange=\"ChonThang_Nam\"")%><span>Tháng</span><%=MyHtmlHelper.DropDownList(ParentID, slThang, ThangLamViec, "ThangLamViec", "", "class=\"input1_2\" style=\"width:20%;\" ")%></p>
                    <p><%=MyHtmlHelper.Option(ParentID, "2", LoaiNam_Thang, "LoaiNam_Thang", "", "onchange=\"ChonThang_Nam\"")%><span>Quý</span></p>
                    <p><%=MyHtmlHelper.Option(ParentID, "0", LoaiNam_Thang, "LoaiNam_Thang", "", "onchange=\"ChonThang_Nam()\"")%><span>Năm</span><%=MyHtmlHelper.Hidden(ParentID, NamLamViec, "NamLamViec","")%></p>
                </fieldset>                
            </div>
        </div>
        <p style="padding:4px 8px 4px 8px;"><%=MyHtmlHelper.DropDownList(ParentID, slDonVi, TenDonVi, "TenDonVi", "", "class=\"input1_2\" style=\"width: 100%; padding:3px;\" size='9' tab-index='-1'")%></p>
        <p style="padding:4px 8px 4px 8px;">Loại mẫu:      <%=MyHtmlHelper.TextBox(ParentID,sLoaiMau,"sLoaiMau","","class=\"input1_2\" style=\"width: 80%\"") %></p>
        <p style="padding:4px 8px 4px 8px;">Tên mẫu :       <%=MyHtmlHelper.TextBox(ParentID, sTenMau, "sTenMau", "", "class=\"input1_2\" style=\"width: 80%\"")%></p>
        <p style="padding:4px 8px 4px 8px;"><%=MyHtmlHelper.DropDownList(ParentID, slLoaiKhoGiay, LoaiKhoGiay, "MaLoaiGiay", "", "class=\"input1_2\" style=\"width: 100%;padding:3px; height:45px;\" size='2' tab-index='-1'")%></p>
        <p class="p"><input style="display:inline-block; margin-right:5px;" type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" /><input style="display:inline-block; margin-left:5px;" class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" /></p>    
    </div><!--End #confirmBox-->
    <script type="text/javascript">
        $(function () {
            $('*').keyup(function (e) {
                if (e.keyCode == '27') {
                    Hide();
                }
            });
            $(".login1").click(function () {
                ShowDialog(500);
            });
            $("#confirmBox h4 a").click(function () {
                Hide();
            });
            if ("<%=pageload %>" == '') {
                ShowDialog(500);
            }          
        });        
        function Huy() {
            window.location.href = '<%=BackURL %>';
        }
    </script>
    <%} %>
    <iframe src="<%=Url.Action("ViewPDF","rptKeToan_InBia",new{NamLamViec=NamLamViec,ThangLamViec=ThangLamViec,LoaiNam_Thang=LoaiNam_Thang,LoaiBiaSo=LoaiBiaSo,LoaiBia=LoaiBia,TenDonVi=TenDonVi,LoaiKhoGiay=LoaiKhoGiay,TuTo=TuTo,DenTo=DenTo,sLoaiMau=sLoaiMau,sTenMau=sTenMau})%>" height="600px" width="100%"></iframe>

</body>
</html>
