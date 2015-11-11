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
     .style6
     {
         width: 110px;
     }
     .style7
     {
         width: 287px;
     }
     .style8
     {
         width: 321px;
     }
     .style9
     {
         width: 110px;
         height: 37px;
     }
     .style10
     {
         height: 37px;
     }
     .style11
     {
         width: 110px;
         height: 54px;
     }
     .style12
     {
         height: 54px;
     }
 </style>
</head>
<body>
  <%
      //Báo cáo Hoạch toán tài sản cố định
       String ParentID = "KeToanCongSan";
       String Nam = DateTime.Now.Year.ToString();
       String TongHopDonVi = Convert.ToString(ViewData["TongHopDonVi"]);
       String TongHopLTS = Convert.ToString(ViewData["TongHopLTS"]);
      
      //Request mã loại tài sản
       String iID_MaLoaiTaiSan = Convert.ToString(ViewData["iID_MaLoaiTaiSan"]);
       String NamChungTu = Convert.ToString(ViewData["NamChungTu"]);
   
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
       String LoaiBieu = Convert.ToString(ViewData["LoaiBieu"]);
       if (String.IsNullOrEmpty(LoaiBieu))
       {
           LoaiBieu = Convert.ToString(ViewData["LoaiBieu"]);
       }
       String[] arrLoaiBieu = { "rLoaiTaiSan", "rDonVi", "rLoaiTaiSanDonVi" };
       if (String.IsNullOrEmpty(LoaiBieu))
           LoaiBieu = arrLoaiBieu[0];
       /// <summary>
       /// DataTable lấy dữ liệu cho commbox đơn vị
       /// </summary>
       /// <returns></returns>
       DataTable dtNguonDonVi = rptKTCS_HoachToanTSCDController.ListDonVi();
       SelectOptionList slTenDonVi = new SelectOptionList(dtNguonDonVi, "iID_MaDonVi", "TenHT");
       //dtNguonDonVi.Rows.InsertAt(dtNguonDonVi.NewRow(), 0);
       //dtNguonDonVi.Rows[0]["sTenDonVi"] = "--Chọn tất cả đơn vị--";
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
       /// DataTable lấy dữ liệu cho commbox Tài sản
       /// </summary>
       /// <returns></returns>
       DataTable dtTaiSan = rptKTCS_HoachToanTSCDController.DT_LoaiTS();
       SelectOptionList slTaiSan = new SelectOptionList(dtTaiSan, "iID_MaNhomTaiSan", "TenHT");
       //dtTaiSan.Rows.InsertAt(dtTaiSan.NewRow(), 0);
       //dtTaiSan.Rows[0]["TenHT"] = "--Chọn tất cả loại tài sản--";
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
       /// <summary>
       /// Action thực hiện xuất dữ liệu ra file excel
       /// </summary>
       /// <returns></returns>
       String PageLoad = Convert.ToString(ViewData["PageLoad"]);
       String URL = "";
       if (PageLoad == "1")
       {
           URL = Url.Action("ViewPDF", "rptKTCS_HoachToanTSCD", new { NamChungTu = NamChungTu, iID_MaLoaiTaiSan = iID_MaLoaiTaiSan, iID_MaDonVi = iID_MaDonVi, LoaiBieu = LoaiBieu, TongHopDonVi = TongHopDonVi, TongHopLTS = TongHopLTS });
       }
       String BackURL = Url.Action("Index", "CongSan_Report");
       String urlExport = Url.Action("ExportToExcel", "rptKTCS_HoachToanTSCD", new { NamChungTu = NamChungTu, iID_MaLoaiTaiSan = iID_MaLoaiTaiSan, iID_MaDonVi = iID_MaDonVi, LoaiBieu = LoaiBieu, TongHopDonVi = TongHopDonVi, TongHopLTS = TongHopLTS });
       using (Html.BeginForm("EditSubmit", "rptKTCS_HoachToanTSCD", new { ParentID = ParentID}))
    {
%>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>BÁO CÁO HẠCH TOÁN TÀI SẢN CỐ ĐỊNH</span>
                </td>
                 <td width="52%" style=" text-align:left;">
                           <div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>
                </td>
            </tr>
        </table>
    </div>
 <div id="nhapform">
    <div id="form2" style="background-color:#F0F9FE;">
<div id="rptMain" style="width:100%;background-color:#F0F9FE;margin-left:80px;padding-top:5px;">
<table width="100%" border="0" cellpadding="0" cellspacing="0">
  <tr>
    <td rowspan="5" valign="top" class="style8"><fieldset style="text-align:left;padding:3px 6px;font-size:11px;width:300px; -moz-border-radius: 3px;-webkit-border-radius: 3px;-khtml-border-radius: 3px;border-radius: 3px;border:1px #C0C0C0 solid;">
                            <legend><b><%=NgonNgu.LayXau("Chọn loại tài sản") %></b></legend>
                             <%=MyHtmlHelper.DropDownList(ParentID, slTaiSan, iID_MaLoaiTaiSan, "iID_MaNhomTaiSan", "", "class=\"input1_2\" style=\"width:300px;\" size='20' tabindex='-1'")%><br />
                             <b>Tất cả loại tài sản :</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%=MyHtmlHelper.CheckBox(ParentID, TongHopLTS, "TongHopLTS", "", "")%>
                           </fieldset></td>
    <td rowspan="5" valign="top" class="style7" align="right"><fieldset style="text-align:left;padding:3px 6px;font-size:11px;margin-left:10px;width:280px;-moz-border-radius: 3px;-webkit-border-radius: 3px;-khtml-border-radius: 3px;border-radius: 3px;border:1px #C0C0C0 solid;"><legend><b><%=NgonNgu.LayXau("Chọn đơn vị") %></b></legend>
                             <%=MyHtmlHelper.DropDownList(ParentID, slTenDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width:280px;\" size='20' tabindex='-1'")%><br />
                             <b>Tất cả đơn vị :</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%=MyHtmlHelper.CheckBox(ParentID, TongHopDonVi, "TongHopDonVi", "", "")%>
                           </fieldset></td>
    <td class="style6">&nbsp;</td>
    <td colspan="2" rowspan="3"><div style="width:310px;">
               <fieldset style="height:70px;padding-top:5px;width:270px;text-align:left;padding-left:5px;-moz-border-radius: 3px;-webkit-border-radius: 3px;-khtml-border-radius: 3px;border-radius: 3px;border:1px #C0C0C0 solid;">
                                    <legend><b>Loại biểu in ra</b></legend>
                                    &nbsp;&nbsp; <%=MyHtmlHelper.Option(ParentID, "rLoaiTaiSan", LoaiBieu, "LoaiBieu", "", "")%>&nbsp;<b>&nbsp;&nbsp;
                                       Loại tài sản</b>&nbsp;&nbsp<br />
                                    &nbsp;&nbsp;&nbsp;<%=MyHtmlHelper.Option(ParentID, "rDonVi", LoaiBieu, "LoaiBieu", "", "")%>&nbsp;<b>&nbsp;&nbsp;
                                       Theo đơn vị</b>&nbsp;&nbsp<br />
                                   &nbsp;&nbsp; <%=MyHtmlHelper.Option(ParentID, "rLoaiTaiSanDonVi", LoaiBieu, "LoaiBieu", "", "")%>&nbsp;&nbsp;<b>
                                        Loại tài sản + đơn vị</b>
                                </fieldset>
                                </div></td>
  </tr>
  <tr>
    <td class="style6">&nbsp;</td>
  </tr>
  <tr>
    <td class="style6">&nbsp;</td>
  </tr>
  <tr>
    <td class="style9"></td>
    <td class="style10" colspan="2" valign="bottom"><b>Năm :</b><%=MyHtmlHelper.DropDownList(ParentID, slNam, NamChungTu, "NamChungTu", "", "class=\"input1_2\" style=\"width: 47%\"")%></td>
  </tr>
  <tr>
    <td class="style11"></td>
    <td class="style12"></td>
    <td class="style12"></td>
  </tr>
  <tr>
    <td colspan="5" align="left" valign="bottom"><table cellpadding="0" cellspacing="0" border="0" style="margin-left:440px;">
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
</div>
 </div>
 <div>
    <%=MyHtmlHelper.ActionLink(urlExport, "Export To Excel") %>
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

<%  } %>
<br />
   <iframe src="<%=URL%>" height="600px" width="100%"></iframe>
</body>
</html>
