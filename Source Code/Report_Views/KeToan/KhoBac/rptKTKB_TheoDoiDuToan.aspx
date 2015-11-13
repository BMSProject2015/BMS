<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.KhoBac" %>
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
        }
        ul.inlineBlock li p{
            padding:1px;    
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
            background-position:  20px 1px;
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
      String MaND = User.Identity.Name;
        DataTable dtCauHinh=NguoiDungCauHinhModels.LayCauHinh(MaND);
      //Nguon NS
       String iID_MaNguonNganSach = Convert.ToString(ViewData["iID_MaNguonNganSach"]);
       DataTable dtNguonNganSach = rptKTKB_TheoDoiDuToanController.get_DanhSachNguon(MaND);
       SelectOptionList slNguonNganSach = new SelectOptionList(dtNguonNganSach, "iID_MaNguonNganSach", "sTen");
        if (String.IsNullOrEmpty(iID_MaNguonNganSach))
          iID_MaNguonNganSach = dtCauHinh.Rows[0]["iID_MaNguonNganSach"].ToString();
        dtNguonNganSach.Dispose();
      //Loai
      String Loai = Convert.ToString(ViewData["Loai"]);
      DataTable dtLoai = rptKTKB_TheoDoiDuToanController.get_DanhSachLoai_Khoan(MaND, iID_MaNguonNganSach);
      SelectOptionList slLoai = new SelectOptionList(dtLoai, "Loai", "TenHT");
      if (String.IsNullOrEmpty(Loai))
      {
          if (dtLoai.Rows.Count > 0)
              Loai = dtLoai.Rows[0]["Loai"].ToString();
          else
              Loai = "-1.-1";      
      }
      dtLoai.Dispose();
      // Thang
      String iThang_Quy = Convert.ToString(ViewData["iThang_Quy"]);
      if (String.IsNullOrEmpty(iThang_Quy))
          iThang_Quy = dtCauHinh.Rows[0]["iThangLamViec"].ToString();
      DataTable dtThang = DanhMucModels.DT_Thang();
      SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
      dtThang.Dispose();
      //Loai Thang Quy
      String bLoaiThang_Quy = Convert.ToString(ViewData["bLoaiThang_Quy"]);
      if (String.IsNullOrEmpty(bLoaiThang_Quy))
          bLoaiThang_Quy = "0";
      DataTable dtQuy = DanhMucModels.DT_Quy();
      SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
      dtQuy.Dispose();
      //Tap Hop
      String iTapHop = Convert.ToString(ViewData["iTapHop"]);
      if (String.IsNullOrEmpty(iTapHop)) iTapHop = "1";
      //Trang thai duyet
      String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
      DataTable dtTrangThai = Luong_ReportModel.DachSachTrangThai();
      SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
      if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet=="-100") ViewData["PageLoad"] = "0";   
      String URL = Url.Action("Index", "KeToan_ChiTiet_Report");
      String PageLoad = Convert.ToString(ViewData["PageLoad"]);
      String UrlReport = "";
      if (PageLoad == "1")
          UrlReport = Url.Action("ViewPDF", "rptKTKB_TheoDoiDuToan", new { MaND = MaND, iID_MaNguonNganSach = iID_MaNguonNganSach, Loai = Loai, iThang_Quy = iThang_Quy, bLoaiThang_Quy = bLoaiThang_Quy, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iTapHop = iTapHop });
     
        using (Html.BeginForm("EditSubmit", "rptKTKB_TheoDoiDuToan", new { ParentID = ParentID }))
        {
    %>
         <div class="box_tong">
            <div class="title_tong">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td width="47.9%">
                            <span>Báo cáo theo dõi rút dự toán </span>
                        </td>
                        <td width="52%" style=" text-align:left;">
                        <div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>
                    </td>
                    </tr>
                </table>
            </div>
            <div id="rptMain">
                <div id="Div2">
                    <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td width="10%"></td>
                        <td class="td_form2_td1" style="width: 10%;">
                            <div>
                                <%=NgonNgu.LayXau("Chọn nguồn:")%>
                            </div>
                        </td>
                        <td width="25%" class="td_form2_td1">
                            <div> <%=MyHtmlHelper.DropDownList(ParentID, slNguonNganSach, iID_MaNguonNganSach, "iID_MaNguonNganSach", "", "style=\"width:100%\" onchange=\"ChonNguon()\"")%>
                            </div>
                         </td>
                          <td class="td_form2_td1" style="width: 10%;">
                            <div>
                                <%=NgonNgu.LayXau("Chọn loại:")%>
                            </div>
                        </td>
                         <td width="20%" class="td_form2_td1" id="<%= ParentID %>_Loai">
                            <% rptKTKB_TheoDoiDuToanController rpt = new rptKTKB_TheoDoiDuToanController();
                            %>
                            <%=rpt.get_Loai(ParentID,MaND,iID_MaNguonNganSach,Loai)%>
                         </td>
                       <td class="td_form2_td1" style="width: 10%;">
                       Tập hơp số liệu: </td>
                         <td rowspan="2">
                            &nbsp&nbsp&nbsp
                             <%=MyHtmlHelper.Option(ParentID, "3", iTapHop, "iTapHop", "", "style=\"width:10%;\"")%>Đến ngày
                              <br />
                              &nbsp&nbsp&nbsp <%=MyHtmlHelper.Option(ParentID, "2", iTapHop, "iTapHop", "", "style=\"width:10%;\"")%>Đến số chứng từ
                              <br />
                              &nbsp&nbsp&nbsp  <%=MyHtmlHelper.Option(ParentID, "1", iTapHop, "iTapHop", "", "style=\"width:10%;\"")%>Đến chi tiết
                         
                         </td>
                    </tr>
                    <tr>
                            <td></td>
                          <td class="td_form2_td1">
                            <div>
                                <%=NgonNgu.LayXau("Loại tháng/quý:")%>
                            </div>
                        </td>
                         <td>
                                <%=MyHtmlHelper.Option(ParentID, "0", bLoaiThang_Quy, "bLoaiThang_Quy", "", "style=\"width:10%;\"")%>Tháng
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang_Quy, "iThang", "", "class=\"input1_2\" style=\"width:20%;\"")%> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp
                                <%=MyHtmlHelper.Option(ParentID, "1", bLoaiThang_Quy, "bLoaiThang_Quy", "", "style=\"width:10%;\"")%>Quý
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, iThang_Quy, "iQuy", "", "class=\"input1_2\" style=\"width:20%;\"")%>
                        </td>
                         <td class="td_form2_td1">
                            <div>
                                <%=NgonNgu.LayXau("Chọn trạng thái:")%>
                            </div>
                        </td>
                        <td>
                            <div> <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "style=\"width:100%\" onchange=\"ChonNguon()\"")%>
                            </div>
                         </td>
                            <td></td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan="6"><table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
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
                            </table></td>
                        <td></td>
                    </tr>
                    </table>
                </div>
            </div>
        </div>
    <%} %>
     <script type="text/javascript">
         function Huy() {
             window.location.href = '<%=URL %>';
         }
         $(function () {
             //         $("div#rptMain").hide();
             $('div.login1 a').click(function () {
                 $('div#rptMain').slideToggle('normal');
                 $(this).toggleClass('active');
                 return false;
             });
         });
     </script>
       <script type="text/javascript">
           function ChonNguon() {
               var iID_MaNguonNganSach = document.getElementById("<%=ParentID %>_iID_MaNguonNganSach").value;
               jQuery.ajaxSetup({ cache: false });
               var url = unescape('<%= Url.Action("Get_objNgayThang?ParentID=#0&MaND=#1&iID_MaNguonNganSach=#2&Loai=#3","rptKTKB_TheoDoiDuToan") %>');
               url = unescape(url.replace("#0", "<%= ParentID %>"));
               url = unescape(url.replace("#1", "<%=MaND%>"));
               url = unescape(url.replace("#2", iID_MaNguonNganSach));
               url = unescape(url.replace("#3", "<%=Loai%>"));
               $.getJSON(url, function (data) {
                   document.getElementById("<%= ParentID%>_Loai").innerHTML = data;
               });
           }                                            
     </script>
     <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptKTKB_TheoDoiDuToan", new { MaND = MaND, iID_MaNguonNganSach = iID_MaNguonNganSach, Loai = Loai, iThang_Quy = iThang_Quy, bLoaiThang_Quy = bLoaiThang_Quy,iID_MaTrangThaiDuyet=iID_MaTrangThaiDuyet,iTapHop=iTapHop }), "Xuất ra Excels")%>
   <iframe src="<%=UrlReport%>" height="600px" width="100%">
     </iframe>
</body>
</html>
