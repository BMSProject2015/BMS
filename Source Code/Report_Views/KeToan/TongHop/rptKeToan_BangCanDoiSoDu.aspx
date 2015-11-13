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
            height:50px;
        }
        div.login1 {
            text-align : center;    
            background: transparent url(/Content/Report_Image/login.gif) no-repeat top center;
        }    
        div.login1 a {
            color: white;
            text-decoration: none;
            font: bold 16px "Museo 700";
            display: block;
            width: 50px; height: 20px;
            line-height: 20px;
            margin: 0px auto;
            background: transparent url(/Content/Report_Image/arrow.png) no-repeat 20px -29px;
            -webkit-border-radius:2px;
            border-radius:2px;
        }    
        div.login1 a.active {
            background-position: 20px 1px;
        }
        div.login1 a:active, a:focus {
            outline: none;
        }
        .errorafter
        {
           background-color:Yellow;
        }        
        ul.inlineBlock li fieldset .div
        {
            width:90px; height:23px; display:inline-block; text-align:center; padding:1px; font-size:14px;-moz-border-radius:3px;-webkit-border-radius:3px; border-radius:3px;
            font-family:Tahoma Arial; color:Black; line-height:23px; cursor:pointer; background-color:#dedede;  
        }
    </style>
    <script type="text/javascript" language="javascript" src="../../../Scripts/jquery-latest.js"></script>
</head>
<body>
    <% 
        String ParentID = "KeToan";
        String MaND=User.Identity.Name;
        DataTable dtND=NguoiDungCauHinhModels.LayCauHinh(MaND);
        String NamLamViec = Convert.ToString(ViewData["NamLamViec"]);
      
            NamLamViec = Convert.ToString(dtND.Rows[0]["iNamLamViec"]);
       
        DateTime dNgayHienTai = DateTime.Now;
        String NamHienTai = Convert.ToString(dNgayHienTai.Year);
        DataTable dtNam = DanhMucModels.DT_Nam();
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        dtNam.Dispose();
        String ThangLamViec = Convert.ToString(ViewData["ThangLamViec"]);
        if (String.IsNullOrEmpty(ThangLamViec)) ThangLamViec = Convert.ToString(dtND.Rows[0]["iThangLamViec"]);

        DataTable dtThang = DanhMucModels.DT_Thang(false);
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
       
        String TrangThai = Convert.ToString(ViewData["TrangThai"]);
        if (String.IsNullOrEmpty(TrangThai))
        {
            TrangThai = "0";
        }
      DataTable dtTrangThai = HamChung.GetTrangThai(PhanHeModels.iID_MaPhanHeKeToanTongHop, LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop), true, "--Tất cả--");
      SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
        String URL = Url.Action("SoDoLuong", "KeToanTongHop");
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String UrlReport = "";
        if (PageLoad == "1")
            UrlReport = Url.Action("ViewPDF", "rptKeToan_BangCanDoiSoDu", new { NamLamViec = NamLamViec, ThangLamViec=ThangLamViec,TrangThai=TrangThai });
        using (Html.BeginForm("EditSubmit", "rptKeToan_BangCanDoiSoDu", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td width="47.9%">
                        <span>Báo cáo bảng cân đối số dư </span>
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
            <div id="rptMain" style="width: 100%; margin: 0px auto; padding: 0px 0px; overflow: visible;">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td width="20%">
                            &nbsp;
                        </td>
                        <td class="td_form2_td1" style="width: 10%;">
                            Năm làm việc:&nbsp;
                        </td>
                        <td width="10%">
                            <%=MyHtmlHelper.DropDownList(ParentID, slNam, NamLamViec, "NamLamViec", "", "class=\"input1_2\" style=\"width: 100%\" disabled=\"disabled\" ")%>
                        </td>
                        <td class="td_form2_td1" style="width: 10%;">
                            Tháng làm việc:&nbsp;
                        </td>
                        <td width="10%">
                            <%=MyHtmlHelper.DropDownList(ParentID, slThang, ThangLamViec, "ThangLamViec", "", "class=\"input1_2\" style=\"width: 100%\"  ")%>
                        </td>
                        <td class="td_form2_td1" width="10%">
                            Trạng thái:
                        </td>
                        <td width="10%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, TrangThai, "TrangThai", "", "class=\"input1_2\" style=\"width: 100%\"")%>
                            </div>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan="6">
                            <table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;"
                                width="100%">
                                <tr>
                                    <td style="width: 49%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="2%">
                                    </td>
                                    <td style="width: 49%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=URL %>';
        }
        $(function () {
            $("ul.inlineBlock li span #<%=ParentID %>_LoaiBaoCao").css({ 'border-color': '#cecece' });
            $("div#rptMain").hide();
            $('div.login1 a').click(function () {
                $('div#rptMain').slideToggle('normal');
                $(this).toggleClass('active');
                return false;
            });
            $("div.div").bind('click', function () {
                $(this).children().removeAttr("checked", "checked").attr("checked", "checked");
                ChonReport();
                $('.div').css('background', '#dedede');
                $(this).css("background", "#F7F570");
            });
        });
       
    </script>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptKeToan_BangCanDoiSoDu", new { NamLamViec = NamLamViec, ThangLamViec = ThangLamViec,  TrangThai = TrangThai }), "Xuất ra Excel")%>
    <%} %>
    <iframe src="<%=UrlReport%>" height="600px" width="100%"></iframe>
</body>
</html>
