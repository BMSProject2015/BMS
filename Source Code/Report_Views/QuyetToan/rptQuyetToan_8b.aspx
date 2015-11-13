<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QuyetToan" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
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
         width: 230px;
     }
     .style2
     {
         width: 104px;
     }
     .style3
     {
         width: 204px;
     }
     .style4
     {
         width: 121px;
     }
    
 </style>
</head>
<body>
     <%
    String srcFile = Convert.ToString(ViewData["srcFile"]);
    String ParentID = "QuyetToanThongTri";
    String TruongTien = "";

    String MaND=User.Identity.Name;
    String Thang = "0", Quy = "0";
    String Thang_Quy = Convert.ToString(ViewData["Thang_Quy"]); 
    if (String.IsNullOrEmpty(Thang_Quy))
    {
        Thang_Quy = "1";
    }
    String LoaiThangQuy = Convert.ToString(ViewData["LoaiThangQuy"]); 
    if (String.IsNullOrEmpty(LoaiThangQuy))
    {
        LoaiThangQuy = "0";
    }
    if (LoaiThangQuy == "0")
    {
        Thang = Thang_Quy;
        Quy = "0";
    }
    else
    {
        Thang = "0";
        Quy = Thang_Quy;
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
    String UserID = User.Identity.Name;
    String iID_MaDanhMuc = Convert.ToString(ViewData["iID_MaDanhMuc"]);  
         //Trang Thai Duyet
    String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);   

    DataTable dtTrangThai = rptQuyetToan_8bController.tbTrangThai();
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

    // Đơn Vị
    DataTable dtDonVi = rptQuyetToan_8bController.Get_NhomDonVi();
    SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDanhMuc", "TenDM");
    if (String.IsNullOrEmpty(iID_MaDanhMuc))
    {
        if (dtDonVi.Rows.Count > 0)
            iID_MaDanhMuc = Convert.ToString(dtDonVi.Rows[0]["iID_MaDanhMuc"]);
        else
            iID_MaDanhMuc = Guid.Empty.ToString();
    }
    dtDonVi.Dispose();
    //Loai ngan sach
    DataTable dtThang = DanhMucModels.DT_Thang();
    SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
    dtThang.Dispose();
    DataTable dtQuy = DanhMucModels.DT_Quy();
    SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
    dtQuy.Dispose();
    String BackURL = Url.Action("Index", "QuyetToan_Report", new { Loai = "0" });
    String urlReport = "";
    String PageLoad = Convert.ToString(ViewData["PageLoad"]);
    if (PageLoad.Equals("1"))
    {
        urlReport = Url.Action("ViewPDF", "rptQuyetToan_8b", new { Thang_Quy = Thang_Quy, LoaiThangQuy = LoaiThangQuy, iID_MaDanhMuc = iID_MaDanhMuc, TruongTien = TruongTien, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
    }  
    using (Html.BeginForm("EditSubmit", "rptQuyetToan_8b", new { ParentID = ParentID}))
    {
    %>   
     <div class="box_tong" style="background-color:#F0F9FE;">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo Tổng hợp quyết toán</span>
                    </td>
                    <td width="52%" style=" text-align:left;">
                           <div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>
                </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="rptMain" style="padding:5px;">
  <table width="100%" border="0" cellpadding="0" cellspacing="0" style="height: 75px">
  <tr>
    <td align="right" class="style4">Tháng / Quý : </td>
    <td class="style1"><%=MyHtmlHelper.Option(ParentID, "0", LoaiThangQuy, "LoaiThangQuy", "", "onchange=Chon()")%>Tháng
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, Thang, "iThang", "onchange=Chon()", "class=\"input1_2\" style=\"width:27%;\"onchange=Chon() ")%>
                                <%=MyHtmlHelper.Option(ParentID, "1", LoaiThangQuy, "LoaiThangQuy", "", "onchange=Chon()")%>Quý
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, Quy, "iQuy", "", "class=\"input1_2\" style=\"width:27%;\"onchange=Chon()")%><br /></td>
    <td width="73" align="right">Trạng Thái : </td>
    <td class="style3"><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 80%;heigh:22px;\"onchange=Chon()")%> </td>
    
    <td width="76" align="right">chọn nhóm : </td>
    <td width="132"><div id="<%= ParentID %>_tdDonVi" style="width:150px;"><%rptQuyetToan_8bController rpt = new rptQuyetToan_8bController();%> 
                                <%=rpt.obj_Nhom(ParentID, Thang_Quy, LoaiThangQuy, iID_MaDanhMuc,iID_MaTrangThaiDuyet,MaND)%></div></td>
  </tr>
  <tr>
    <td colspan="6" align="center"><table cellpadding="0" cellspacing="0" border="0" align="center">
                                <tr>
                                    <td>
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="5px">
                                    </td>
                                    <td>
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                                    </td>
                                </tr>
                            </table></td>
  </tr>
</table>



            </div>
             <script type="text/javascript">
                 function Chon() {
                     var LoaiThang_QuyCheck = document.getElementById("<%= ParentID %>_LoaiThangQuy").checked;
                     var LoaiThangQuy
                     var Thang
                     if (LoaiThang_QuyCheck == true) {
                         Thang = document.getElementById("<%= ParentID %>_iThang").value;
                         LoaiThangQuy = 0;
                     }
                     else {
                         Thang = document.getElementById("<%= ParentID %>_iQuy").value;
                         LoaiThangQuy = 1;
                     }
                     
                     var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
                     jQuery.ajaxSetup({ cache: false });
                     var url = unescape('<%= Url.Action("Ds_Nhom?ParentID=#0&Thang_Quy=#1&LoaiThangQuy=#2&iID_MaDanhMuc=#3&iID_MaTrangThaiDuyet=#4", "rptQuyetToan_8b") %>');
                     url = unescape(url.replace("#0", "<%= ParentID %>"));
                     
                     url = unescape(url.replace("#1", Thang));
                     url = unescape(url.replace("#2", LoaiThangQuy));
                     url = unescape(url.replace("#3", "<%= iID_MaDanhMuc %>"));
                     url = unescape(url.replace("#4", iID_MaTrangThaiDuyet));
                     $.getJSON(url, function (data) {
                         document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data;
                     });
                 }                                            
     </script>
        </div>
    </div>
    <%} %>
    <div>
    </div>

     <script type="text/javascript">
         function Huy() {
             window.location.href = '<%=BackURL%>';
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
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptQuyetToan_8b", new { Thang_Quy = Thang_Quy, LoaiThangQuy = LoaiThangQuy, iID_MaDanhMuc = iID_MaDanhMuc, TruongTien = TruongTien, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet }), "Export To Excel")%>
     <iframe src="<%=urlReport%>" height="600px" width="100%">
     </iframe>
    
</body>
</html>
