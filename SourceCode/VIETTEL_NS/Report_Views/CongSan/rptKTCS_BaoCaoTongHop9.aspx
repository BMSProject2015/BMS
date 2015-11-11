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
         .style3
     {
         height: 42px;
     }
     .style4
     {
         width: 149px;
     }
     .style5
     {
         height: 42px;
         width: 149px;
     }
     .style6
     {
         width: 248px;
     }
    
 </style>
</head>
<body>
  <%
      /// <summary>
      /// <prame name="NamLamViec">lấy năm làm việc</prame>
      /// <prame name="UserID">kiểm tra user theo phòng ban</prame>
      /// <prame name="iID_MaDonVi">Loại đơn vị/prame>
      /// <prame name="iID_MaLoaiTaiSan">mã loại tài san</prame>
      /// </summary>
      /// <returns></returns>
       String srcFile = Convert.ToString(ViewData["srcFile"]);
       String ParentID = "KeToanTongHop";
       String Nam = DateTime.Now.Year.ToString();
       String iID_MaLoaiTaiSan = Request.QueryString["iID_MaLoaiTaiSan"];
       String NamChungTu = Request.QueryString["NamChungTu"];
       String TongHopDonVi = Request.QueryString["TongHopDonVi"];
       String TongHopLTS = Request.QueryString["TongHopLTS"];
       String LoaiBaoCao = Request.QueryString["LoaiBaoCao"];
       DataTable dtLoaiBaoCao = rptKTCS_BaoCaoTongHop9Controller.DTLoaiBaoCao();
       SelectOptionList slLoaiBaoCao = new SelectOptionList(dtLoaiBaoCao, "MaLoaiBaoCao", "TenLoaiBaoCao");
       if (String.IsNullOrEmpty(LoaiBaoCao))
       {
           LoaiBaoCao = Convert.ToString(dtLoaiBaoCao.Rows[0]["MaLoaiBaoCao"].ToString());
       }
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
       String iID_MaDonVi = Request.QueryString["iID_MaDonVi"];
       String UserID = User.Identity.Name;

       /// <summary>
       /// DataTable lấy dữ liệu cho commbox đơn vị
       /// </summary>
       /// <returns></returns>
       DataTable dtNguonDonVi = KTCS_ReportModel.ListDonVi();
       SelectOptionList slTenDonVi = new SelectOptionList(dtNguonDonVi, "iID_MaDonVi", "TenHT");
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
       /// <summary>
       /// DataTable lấy dữ liệu cho commbox tài sản
       /// </summary>
       /// <returns></returns>
       DataTable dtTaiSan = rptKTCS_BaoCaoTongHop9Controller.DT_LoaiTS();
       SelectOptionList slTaiSan = new SelectOptionList(dtTaiSan, "iID_MaNhomTaiSan", "TenHT");
       if (String.IsNullOrEmpty(iID_MaLoaiTaiSan))
       {
           if (dtTaiSan.Rows.Count > 0)
           {
               iID_MaLoaiTaiSan = Convert.ToString(dtTaiSan.Rows[0]["iID_MaNhomTaiSan"]);
           }
           else
           {
               iID_MaLoaiTaiSan = Guid.Empty.ToString();
           }
       }
       dtTaiSan.Dispose();
       String KhoGiay = Request.QueryString["KhoGiay"];
       DataTable dtKhoGiay = ReportModels.LoaiKhoGiay();
       SelectOptionList slKhoGiay = new SelectOptionList(dtKhoGiay, "MaKhoGiay", "TenKhoGiay");
       if (string.IsNullOrEmpty(KhoGiay))
       {
           KhoGiay = "2";
       }
       dtKhoGiay.Dispose();
       /// <summary>
       /// Url Action thực hiện xuất dữ liệu ra file excel
       /// </summary>
       /// <returns></returns>
       String BackURL = Url.Action("Index", "CongSan_Report");
       String urlExport = Url.Action("ExportToExcel", "rptKTCS_BaoCaoTongHop9", new { NamChungTu = NamChungTu, LoaiBaoCao = LoaiBaoCao, iID_MaLoaiTaiSan = iID_MaLoaiTaiSan, iID_MaDonVi = iID_MaDonVi, TongHopDonVi = TongHopDonVi, TongHopLTS = TongHopLTS, KhoGiay = KhoGiay });
       using (Html.BeginForm("EditSubmit", "rptKTCS_BaoCaoTongHop9", new { ParentID = ParentID, NamChungTu = NamChungTu, LoaiBaoCao = LoaiBaoCao, iID_MaLoaiTaiSan = iID_MaLoaiTaiSan, iID_MaDonVi = iID_MaDonVi, TongHopDonVi = TongHopDonVi, TongHopLTS = TongHopLTS, KhoGiay = KhoGiay }))
    {
%>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>BÁO CÁO TỔNG HỢP NHÀ ĐẤT</span>
                </td>
                  <td width="52%" style=" text-align:left;">
                           <div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>
                </td>
            </tr>
        </table>
    </div>
<div id="nhapform" style="background-color:#F0F9FE;">
<div id="rptMain" style="width:100%;margin-left:80px;padding-top:10px;padding-bottom:10px;">                                    
<table width="100%"  border="0" cellpadding="0" cellspacing="0" 
        style="height: 165px">
  <tr>
    <td width="260" valign="bottom"><b>Chọn loại tài sản </b></td>
    <td valign="bottom" class="style6"><b>Chọn đơn vị</b> </td>
    <td class="style4">&nbsp;</td>
    <td width="380">&nbsp;</td>
  </tr>
  <tr>
    <td rowspan="3" valign="top"><%=MyHtmlHelper.DropDownList(ParentID, slTaiSan, iID_MaLoaiTaiSan, "iID_MaLoaiTaiSan", "", "class=\"input1_2\"style=\"width: 80%\" size='6' tabindex='-1'")%></td>
    <td rowspan="3" valign="top" class="style6"><%=MyHtmlHelper.DropDownList(ParentID, slTenDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\"style=\"width: 80%\"size='6' tabindex='-1'")%></td>
    <td class="style5"><b>Chọn mẫu báo cáo: </b></td>
    <td class="style3"> <%=MyHtmlHelper.DropDownList(ParentID, slLoaiBaoCao, LoaiBaoCao, "LoaiBaoCao", "", "class=\"input1_2\"style=\"width: 60%\"")%></td>
  </tr>
  <tr>
    <td class="style4"><b>Chọn năm : </b></td>
    <td><%=MyHtmlHelper.DropDownList(ParentID, slNam, NamChungTu, "NamChungTu", "", "class=\"input1_2\" style=\"width: 60%\"")%></td>
  </tr>
  <tr>
    <td class="style4"><b>Khổ Giấy : </b></td>
    <td><%=MyHtmlHelper.DropDownList(ParentID, slKhoGiay, KhoGiay, "KhoGiay", "", "class=\"input1_2\" style=\"width: 60%\"")%></td>
  </tr>
  <tr>
    <td><b>Tất cả loại tài sản :</b> &nbsp;&nbsp;<%=MyHtmlHelper.CheckBox(ParentID, TongHopLTS, "TongHopLTS", "", "")%></td>
    <td class="style6"><b>Tất cả đơn vị :</b>&nbsp;&nbsp;<%=MyHtmlHelper.CheckBox(ParentID, TongHopDonVi, "TongHopDonVi", "", "")%></td>
    <td class="style4">&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td colspan="4" align="left"><table cellpadding="0" cellspacing="0" border="0" style="margin-left:470px;padding-top:5px;">
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
                            </table></td>
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
   <div>
    <%=MyHtmlHelper.ActionLink(urlExport, "Export To Excel") %>
    </div>
</div>
<%  } %>
<br />
   <iframe src="<%=Url.Action("ViewPDF","rptKTCS_BaoCaoTongHop9",new{NamChungTu=NamChungTu,LoaiBaoCao=LoaiBaoCao,iID_MaLoaiTaiSan=iID_MaLoaiTaiSan, iID_MaDonVi = iID_MaDonVi, TongHopDonVi = TongHopDonVi, TongHopLTS = TongHopLTS, KhoGiay = KhoGiay })%>" height="600px" width="100%"></iframe>
</body>
</html>
