<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.CongSan" %>
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
            font: bold 12px "Museo 700";
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
     </style>
</head>
<body>
   <%
       String ParentID = "KeToanCongSan";
  
       String NamChungTu = Convert.ToString(ViewData["NamChungTu"]);
       String TongHopDonVi = Convert.ToString(ViewData["TongHopDonVi"]);
       if (String.IsNullOrEmpty(NamChungTu))
       {
           NamChungTu = NguoiDungCauHinhModels.iNamLamViec.ToString();
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
       dtNam.Rows[0]["TenNam"] = "-- Năm chứng từ kế toán --";
       SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
       String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
       String UserID = User.Identity.Name;
      //ĐƠN VỊ
       DataTable dtNguonDonVi = KTCS_ReportModel.ListDonVi();
       if (String.IsNullOrEmpty(iID_MaDonVi))
       {
           if (dtNguonDonVi.Rows.Count > 0)
           {
               iID_MaDonVi = Convert.ToString(dtNguonDonVi.Rows[0]["iID_MaDonVi"]);
           }
           else
           {
               iID_MaDonVi = Guid.Empty.ToString();
           }
       }
       dtNguonDonVi.Dispose();
       SelectOptionList slTenDonVi = new SelectOptionList(dtNguonDonVi, "iID_MaDonVi", "TenHT");
       String KhoGiay = Convert.ToString(ViewData["KhoGiay"]);
       DataTable dtKhoGiay = ReportModels.LoaiKhoGiay();
       SelectOptionList slKhoGiay = new SelectOptionList(dtKhoGiay, "MaKhoGiay", "TenKhoGiay");
       if (string.IsNullOrEmpty(KhoGiay))
       {
           KhoGiay = "2";
       }
       dtKhoGiay.Dispose();
       String PageLoad = Convert.ToString(ViewData["PageLoad"]);
       String URL = "";
       if(PageLoad=="1")
       {
           URL = Url.Action("ViewPDF","rptKTCS_MauC53",new{NamChungTu=NamChungTu, iID_MaDonVi = iID_MaDonVi, TongHopDonVi = TongHopDonVi,KhoGiay=KhoGiay});
       }
      //tai san
       String BackURL = Url.Action("Index", "CongSan_Report");
       String urlExport = Url.Action("ExportToExcel", "rptKTCS_MauC53", new { NamChungTu = NamChungTu, iID_MaDonVi = iID_MaDonVi, TongHopDonVi = TongHopDonVi, KhoGiay = KhoGiay });
       using (Html.BeginForm("EditSubmit", "rptKTCS_MauC53", new { ParentID = ParentID}))
    {
%>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>BÁO CÁO kiểm kê tài sản  </span>
                </td>
                <td width="52%" style=" text-align:left;">
                           <div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">

        <div id="form2">
<div style="width:100%;margin:0 auto;;background-color:#F0F9FE;">
<div id="rptMain" style="margin:0 auto;padding-top:5px;">
<table width="100%" border="0" cellpadding="0" cellspacing="0">
  <tr>
    <td width="142" align="right"><b>Chọn Năm : </b></td>
    <td width="170"><%=MyHtmlHelper.DropDownList(ParentID, slNam, NamChungTu, "NamChungTu", "", "class=\"input1_2\" style=\"width: 70%\"")%></td>
    <td width="93" align="right"><b>Chọn Đơn vị: </b></td>
    <td width="186"><%=MyHtmlHelper.DropDownList(ParentID, slTenDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\"style=\"width: 70%\"")%> </td>
    <td width="135"><b>Tất Cả đơn vị : </b><%=MyHtmlHelper.CheckBox(ParentID, TongHopDonVi, "TongHopDonVi", "", "")%></td>
    <td width="93" align="right"><b>Khổ Giấy : </b></td>
    <td width="165"><%=MyHtmlHelper.DropDownList(ParentID, slKhoGiay, KhoGiay, "KhoGiay", "", "class=\"input1_2\" style=\"width: 80%\"")%></td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td colspan="7" align="center"><table cellpadding="0" cellspacing="0" border="0" align="center">
                                <tr>
                                    <td>
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="25px">
                                    </td>
                                    <td>
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                                    </td>
                                </tr>
                            </table>       </td>
  </tr>
</table> 
</div>
          
   </div>
</div>
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
 <div>
    <%=MyHtmlHelper.ActionLink(urlExport, "Export To Excel") %>
    </div>
</div>
<%  } %>
<br />
   <iframe src="<%=URL%>" height="600px" width="100%"></iframe>
</body>
</html>
