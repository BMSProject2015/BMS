<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QuyetToan" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
     <script type="text/javascript" language="javascript" src="../../../Scripts/jquery-latest.js"></script>
 <style type="text/css">
     div.login1 {
            text-align : center;    
        }    
        div.login1 a {
            color: #545998;
            text-decoration: none;
            font: bold 10px "Museo 700";
            display: block;
            width: 250px; height: 20px;
            line-height: 20px;
            margin: 0px auto;
            -webkit-border-radius:2px;
            border-radius:2px;
        }
        div.login1 a:hover
        {
            text-decoration:underline;
            color:#471083;
        }    
        div.login1 a.active {
            background-position:  20px 1px;
        }
        div.login1 a:active, a:focus {
            outline: none;
        }
    
     .style1
     {
         width: 244px;
     }
     .style2
     {
         width: 52px;
     }
    
 </style>
</head>
<body>
    <%
    String srcFile = Convert.ToString(ViewData["srcFile"]);
    String MaND = User.Identity.Name;
    String ParentID = "QuyetToanThongTri";
    String iNamLamViec = Request.QueryString["iNamLamViec"];
    if (String.IsNullOrEmpty(iNamLamViec))
    {
        iNamLamViec = DateTime.Now.Year.ToString();
    }
    DateTime dNgayHienTai = DateTime.Now;
    //String NamHienTai = Convert.ToString(dNgayHienTai.Year);
    //int NamMin = Convert.ToInt32(dNgayHienTai.Year) - 10;
    //int NamMax = Convert.ToInt32(dNgayHienTai.Year) + 10;
    //DataTable dtNam = new DataTable();
    //dtNam.Columns.Add("MaNam", typeof(String));
    //dtNam.Columns.Add("TenNam", typeof(String));
    //DataRow R;
    //for (int i = NamMin; i < NamMax; i++)
    //{
    //    R = dtNam.NewRow();
    //    dtNam.Rows.Add(R);
    //    R[0] = Convert.ToString(i);
    //    R[1] = Convert.ToString(i);
    //}
    //dtNam.Rows.InsertAt(dtNam.NewRow(), 0);
    //dtNam.Rows[0]["TenNam"] = "-- Bạn chọn năm ngân sách --";
    //SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
    String UserID = User.Identity.Name;
    String iThang_Quy = Convert.ToString(ViewData["iThang_Quy"]);
    String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);

    // dt Trạng Thái duyệt
    String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
    DataTable dtTrangThai = rptQuyetToan_ThongTri_7Controller.tbTrangThai();
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
    //Loai thang
    DataTable dtThang = DanhMucModels.DT_Thang();
    SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
    if (String.IsNullOrEmpty(iThang_Quy))
    {
        if (dtThang.Rows.Count > 0)
        {
            iThang_Quy = Convert.ToString(dtThang.Rows[1]["TenThang"]);
        }
        else
        {
            iThang_Quy = Guid.Empty.ToString();
        }
    }
    dtThang.Dispose();
    // Đơn Vị
    DataTable dtDonVi = rptQuyetToan_ThongTri_7Controller.DanhSachDonVi(iThang_Quy,iID_MaTrangThaiDuyet,MaND);
    SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
    if (String.IsNullOrEmpty(iID_MaDonVi))
    {
        if (dtDonVi.Rows.Count > 0)
        {
            iID_MaDonVi = Convert.ToString(dtDonVi.Rows[0]["sTen"]);
        }
        else
        {
            iID_MaDonVi = Guid.Empty.ToString();
        }
    }
    dtDonVi.Dispose();
  
    String URL = Url.Action("Index", "QuyetToan_Report", new { Loai="0"});
    String urlExport = Url.Action("ExportToExcel", "rptQuyetToan_ThongTri_7", new { iThang_Quy = iThang_Quy, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iID_MaDonVi = iID_MaDonVi });
    String urlReport = "";
    String PageLoad = Convert.ToString(ViewData["PageLoad"]);
    if (PageLoad.Equals("1"))
    {
        urlReport = Url.Action("ViewPDF", "rptQuyetToan_ThongTri_7", new { iThang_Quy = iThang_Quy, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iID_MaDonVi = iID_MaDonVi });
    }  
        using (Html.BeginForm("EditSubmit", "rptQuyetToan_ThongTri_7", new { ParentID = ParentID }))
    {
    %>   
     <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo thông tri theo ngành</span>
                    </td>
                     <td width="52%" style=" text-align:left;">
                           <div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>
                </td>
                </tr>
            </table>
        </div>        
    <div id="Div3" style="background-color:#F0F9FE;">
           <div id="rptMain" style="margin:0 auto;">          
               <table width="100%" border="0" cellpadding="0" cellspacing="0" style="height: 71px">
  <tr>
    <td width="117" align="right"><b>Chọn Tháng :</b></td>
    <td width="131"><%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang_Quy, "iThang_Quy", "", "class=\"input1_2\" style=\"width: 100%\" onchange=ChonDV()")%></td>
    <td width="88" align="right"><b>Trạng Thái :</b> </td>
    <td width="171"><div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 80%;heigh:22px;\"onchange=ChonDV()")%>
                                <br /></div></td>
    <td width="82" align="right"><b>Đơn vị :</b> </td>
    <td class="style1"><div id="<%= ParentID %>_divDonVi">
                                     <%rptQuyetToan_ThongTri_7Controller rptTB1 = new rptQuyetToan_ThongTri_7Controller();%>                                    
                                     <%=rptTB1.obj_DSDonVi(ParentID, iThang_Quy, iID_MaTrangThaiDuyet, iID_MaDonVi,MaND)%>
                            </div></td>
    <td class="style2">&nbsp;</td>
  </tr>
  <tr>
    <td colspan="6" align="center"><table cellpadding="0" cellspacing="0" border="0" align="center" style="margin: 2px auto; width:200px;">
                                <tr>
                                    <td>
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="2px">
                                    </td>
                                    <td>
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                                    </td>
                                </tr>
                            </table> </td>
    <td align="center" class="style2">&nbsp;</td>
  </tr>
</table>
            </div>
        </div> 
        <div>    
        </div>
    </div>
    <%} %>
    <div>
    </div>
    <div class="box_tong">        
        <div>
            <%=MyHtmlHelper.ActionLink(urlExport, "Export To Excel") %>
        </div>
    </div>
    <%
        dtDonVi.Dispose();
        dtThang.Dispose();
    %>
     <script type="text/javascript">
         function ChonDV() {
             var Thang = document.getElementById("<%=ParentID %>_iThang_Quy").value;
             var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;

             jQuery.ajaxSetup({ cache: false });
             var url = unescape('<%= Url.Action("Get_dsDonVi?ParentID=#0&iThang_Quy=#1&iID_MaTrangThaiDuyet=#2&iID_MaDonVi=#3", "rptQuyetToan_ThongTri_7") %>');
             url = unescape(url.replace("#0", "<%= ParentID %>"));
             url = unescape(url.replace("#1", Thang));
             url = unescape(url.replace("#2", iID_MaTrangThaiDuyet));
             url = unescape(url.replace("#3", "<%= iID_MaDonVi %>"));
             $.getJSON(url, function (data) {
                 document.getElementById("<%= ParentID %>_divDonVi").innerHTML = data;
             });
         }                                            
     </script>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=URL %>';
        }
    </script>
      <script type="text/javascript">
          $(function () {
              $("div#rptMain").show();
              $('div.login1 a').click(function () {
                  $('div#rptMain').slideToggle('fast');
                  $(this).toggleClass('active');
                  return false;
              });
          });
    </script>
     <iframe src="<%=urlReport%>" height="600px" width="100%">
     </iframe>
    
</body>
</html>
