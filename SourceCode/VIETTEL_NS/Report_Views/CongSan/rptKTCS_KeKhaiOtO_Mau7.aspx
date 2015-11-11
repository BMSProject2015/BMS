<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.CongSan" %>
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
     .style5
     {
         height: 28px;
     }
     .style6
     {
         width: 70px;
         height: 28px;
     }
     .style7
     {
         width: 70px;
     }
 </style>
</head>
<body>
   <%
       String srcFile = Convert.ToString(ViewData["srcFile"]);
       String ParentID = "KeToanCongSan";
       String Nam = DateTime.Now.Year.ToString();
       String iID_MaLoaiTaiSan = Request.QueryString["iID_MaLoaiTaiSan"];
       String TongHopDonVi = Request.QueryString["TongHopDonVi"];
       String TongHopLTS = Request.QueryString["TongHopLTS"];
       String NamChungTu = Request.QueryString["NamChungTu"];

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
       String LoaiBieu = Request.QueryString["LoaiBieu"];
       if (String.IsNullOrEmpty(LoaiBieu))
       {
           LoaiBieu = Convert.ToString(ViewData["LoaiBieu"]);
       }
       String[] arrLoaiBieu = { "rMaLoai4", "rMaLoai", "rTatCa" };
       if (String.IsNullOrEmpty(LoaiBieu))
           LoaiBieu = arrLoaiBieu[0];
       //ĐƠN VỊ
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

       //tai san
       DataTable dtTaiSan = rptKTCS_KeKhaiOtO_Mau7Controller.DT_LoaiTS();
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
      
       String BackURL = Url.Action("Index", "CongSan_Report");
       String urlExport = Url.Action("ExportToExcel", "rptKTCS_KeKhaiOtO_Mau7", new { NamChungTu = NamChungTu, iID_MaLoaiTaiSan = iID_MaLoaiTaiSan, iID_MaDonVi = iID_MaDonVi, LoaiBieu = LoaiBieu, TongHopDonVi = TongHopDonVi, TongHopLTS = TongHopLTS, KhoGiay = KhoGiay });
       using (Html.BeginForm("EditSubmit", "rptKTCS_KeKhaiOtO_Mau7", new { ParentID = ParentID, NamChungTu = NamChungTu, iID_MaLoaiTaiSan = iID_MaLoaiTaiSan, iID_MaDonVi = iID_MaDonVi, LoaiBieu = LoaiBieu, TongHopDonVi = TongHopDonVi, TongHopLTS = TongHopLTS, KhoGiay = KhoGiay }))
    {
%>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>BÁO CÁO KÊ KHAI TÀI SẢN Ô TÔ</span></td>
                 <td width="52%" style=" text-align:left;">
                           <div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
    <div id="form2" style="background-color:#F0F9FE;">
<div id="rptMain" style="width:100%;background-color:#F0F9FE;margin-left:80px;padding-top:5px;">
<table width="100%" border="0" cellpadding="0" cellspacing="0" 
        style="height: 184px">
  <tr>
    <td width="270" rowspan="5" valign="top"><fieldset style="text-align:left;padding:3px 6px;font-size:11px;width:300px; -moz-border-radius: 3px;-webkit-border-radius: 3px;-khtml-border-radius: 3px;border-radius: 3px;border:1px #C0C0C0 solid;">
                            <legend><b><%=NgonNgu.LayXau("Chọn loại tài sản") %></b></legend>
                             <%=MyHtmlHelper.DropDownList(ParentID, slTaiSan, iID_MaLoaiTaiSan, "iID_MaLoaiTaiSan", "", "class=\"input1_2\" style=\"width:300px;\" size='20' tabindex='-1'")%><br />
                             <b>Tất cả loại tài sản :</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%=MyHtmlHelper.CheckBox(ParentID, TongHopLTS, "TongHopLTS", "", "")%>
                           </fieldset></td>
    <td width="258" rowspan="5" valign="top"><fieldset style="text-align:left;padding:3px 6px;font-size:11px;margin-left:10px;width:280px;-moz-border-radius: 3px;-webkit-border-radius: 3px;-khtml-border-radius: 3px;border-radius: 3px;border:1px #C0C0C0 solid;"><legend><b><%=NgonNgu.LayXau("Chọn đơn vị") %></b></legend>
                             <%=MyHtmlHelper.DropDownList(ParentID, slTenDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width:280px;\" size='20' tabindex='-1'")%><br />
                             <b>Tất cả đơn vị :</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%=MyHtmlHelper.CheckBox(ParentID, TongHopDonVi, "TongHopDonVi", "", "")%>
                           </fieldset></td>
    <td width="23">&nbsp;</td>
    <td rowspan="3" class="style7"><b> Khổ giấy :</b> </td>
    <td rowspan="3"><%=MyHtmlHelper.DropDownList(ParentID, slKhoGiay, KhoGiay, "KhoGiay", "", "class=\"input1_2\" style=\"width: 60%\" size='2' tabindex='-1'")%></td>
  </tr>
  <tr>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td></td>
  </tr>
  <tr>
    <td class="style5"></td>
    <td class="style6" valign="bottom"><b>Năm :</b></td>
    <td width="314" class="style5" valign="bottom"> <%=MyHtmlHelper.DropDownList(ParentID, slNam, NamChungTu, "NamChungTu", "", "class=\"input1_2\" style=\"width: 60%\"")%></td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td class="style7">&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td colspan="5" align="center"><table cellpadding="0" cellspacing="0" border="0">
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
   <iframe src="<%=Url.Action("ViewPDF","rptKTCS_KeKhaiOtO_Mau7",new{NamChungTu=NamChungTu,iID_MaLoaiTaiSan=iID_MaLoaiTaiSan, iID_MaDonVi = iID_MaDonVi,LoaiBieu=LoaiBieu, TongHopDonVi = TongHopDonVi,TongHopLTS=TongHopLTS ,KhoGiay=KhoGiay })%>" height="600px" width="100%"></iframe>
</body>
</html>
