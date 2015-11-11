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
    <style type="text/css">
        #form2
        {
            height: 89px;
        }
        </style>
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
    
     .style7
     {
         height: 37px;
     }
     .style8
     {
         height: 29px;
     }
    
 </style>
</head>
<body>
  <%
      String ParentID = "KeToanTongHop";
      String Nam = DateTime.Now.Year.ToString();
      String iID_MaLoaiTaiSan = Convert.ToString(ViewData["iID_MaLoaiTaiSan"]);
      String NamChungTu = Convert.ToString(ViewData["NamChungTu"]);
      String ThangChungTu = Convert.ToString(ViewData["ThangChungTu"]);
      if (String.IsNullOrEmpty(ThangChungTu))
      {
          ThangChungTu = "1";
      }
      DataTable dtThang = DanhMucModels.DT_Thang();
      SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
      dtThang.Dispose();
      String TongHopDonVi = Convert.ToString(ViewData["TongHopDonVi"]);
      String TongHopLTS = Convert.ToString(ViewData["TongHopLTS"]);
      if (String.IsNullOrEmpty(NamChungTu))
      {
          NamChungTu = DateTime.Now.Year.ToString();
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
      //tai san
      DataTable dtTaiSan = KTCS_ReportModel.DT_LoaiTS();
      SelectOptionList slTaiSan = new SelectOptionList(dtTaiSan, "iID_MaNhomTaiSan", "TenHT");
      if (String.IsNullOrEmpty(iID_MaLoaiTaiSan))
      {
          if (dtTaiSan.Rows.Count > 0)
          {
              iID_MaLoaiTaiSan = Convert.ToString(dtTaiSan.Rows[1]["iID_MaNhomTaiSan"]);
          }
          else
          {
              iID_MaLoaiTaiSan = Guid.Empty.ToString();
          }
      }
      dtTaiSan.Dispose();
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
      if (PageLoad == "1")
      {
          URL = Url.Action("ViewPDF", "rptKTCS_SoTheoDoiTSCD", new { NamChungTu = NamChungTu,ThangChungTu=ThangChungTu, iID_MaLoaiTaiSan = iID_MaLoaiTaiSan, iID_MaDonVi = iID_MaDonVi, TongHopDonVi = TongHopDonVi, TongHopLTS = TongHopLTS, KhoGiay = KhoGiay });
      }
      
       String BackURL = Url.Action("Index", "CongSan_Report");
       String urlExport = Url.Action("ExportToExcel", "rptKTCS_SoTheoDoiTSCD", new { NamChungTu = NamChungTu,ThangChungTu=ThangChungTu, iID_MaLoaiTaiSan = iID_MaLoaiTaiSan, iID_MaDonVi = iID_MaDonVi, TongHopDonVi = TongHopDonVi, TongHopLTS = TongHopLTS, KhoGiay = KhoGiay });
       using (Html.BeginForm("EditSubmit", "rptKTCS_SoTheoDoiTSCD", new { ParentID = ParentID}))
    {
  %>
<div class="box_tong" >
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>BÁO CÁO SỔ THEO DÕI TÀI SẢN CỐ ĐỊNH</span>
                </td>
                <td width="52%" style=" text-align:left;">
                           <div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">

<div id="rptMain" style="background-color:#F0F9FE;margin:0 auto;">
<div style="margin:0 auto;">                       
<table width="100%" border="0" cellpadding="0" cellspacing="0" 
        style="height: 151px; margin-left:140px;">
  <tr>
    <td width="216" valign="bottom"><b>Chọn loại tài sản </b></td>
    <td width="24">&nbsp;</td>
    <td width="275" valign="bottom"><b>Chọn Đơn vị </b></td>
    <td width="95">&nbsp;</td>
    <td width="378">&nbsp;</td>
  </tr>
  <tr>
    <td rowspan="3"><%=MyHtmlHelper.DropDownList(ParentID, slTaiSan, iID_MaLoaiTaiSan, "iID_MaLoaiTaiSan", "", "class=\"input1_2\"style=\"width: 100%\" size='20' tabindex='-1'")%></td>
    <td rowspan="3">&nbsp;</td>
    <td rowspan="3"><%=MyHtmlHelper.DropDownList(ParentID, slTenDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\"style=\"width: 90%\"size='20' tabindex='-1'")%></td>
    <td align="right" class="style8"><b>Chọn năm :</b></td>
    <td class="style8"><%=MyHtmlHelper.DropDownList(ParentID, slNam, NamChungTu, "NamChungTu", "", "class=\"input1_2\" style=\"width: 30%\"")%></td>
  </tr>
    <tr>
    <td align="right" class="style7"><b>Chọn tháng : </b></td>
    <td class="style7"><%=MyHtmlHelper.DropDownList(ParentID, slThang, ThangChungTu, "ThangChungTu", "", "class=\"input1_2\" style=\"width: 30%\"")%></td>
  </tr>
  <tr>
    <td align="right" class="style7"><b>Khổ Giấy : </b></td>
    <td class="style7"><%=MyHtmlHelper.DropDownList(ParentID, slKhoGiay, KhoGiay, "KhoGiay", "", "class=\"input1_2\" style=\"width: 30%\"")%></td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td valign="top"><b>Tất cả loại tài sản : &nbsp;&nbsp;&nbsp;</b><%=MyHtmlHelper.CheckBox(ParentID, TongHopLTS, "TongHopLTS", "", "")%></td>
    <td>&nbsp;</td>
    <td valign="top"><b>Tất cả đơn vị :</b>&nbsp;&nbsp;&nbsp;<%=MyHtmlHelper.CheckBox(ParentID, TongHopDonVi, "TongHopDonVi", "", "")%></td>
    <td colspan="2" align="left"><table cellpadding="0" cellspacing="0" border="0">
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
                            </table> </td>
  </tr>
</table>
           
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
 </div>
 <div>
    <%=MyHtmlHelper.ActionLink(urlExport, "Export To Excel") %>
    </div>
</div>
<%  } %>
<br />
  <iframe src="<%=URL%>" height="600px" width="100%"></iframe>
</body>
</html>
