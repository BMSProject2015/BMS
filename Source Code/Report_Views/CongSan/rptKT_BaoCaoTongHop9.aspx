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
    <style type="text/css">
        .style5
        {
            width: 89px;
        }
        .style6
        {
            width: 195px;
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
       DataTable dtNguonDonVi = DonViModels.Get_dtDonVi();
       if (String.IsNullOrEmpty(iID_MaDonVi))
       {
           iID_MaDonVi = Convert.ToString(dtNguonDonVi.Rows[0]["iID_MaDonVi"]);
       }
       dtNguonDonVi.Dispose();
       SelectOptionList slTenDonVi = new SelectOptionList(dtNguonDonVi, "iID_MaDonVi", "sTen");
       dtNguonDonVi.Rows.InsertAt(dtNguonDonVi.NewRow(), 0);
       dtNguonDonVi.Rows[0]["sTen"] = "--Chọn tất cả đơn vị--";
       /// <summary>
       /// DataTable lấy dữ liệu cho commbox tài sản
       /// </summary>
       /// <returns></returns>
       DataTable dtTaiSan = LoaiTaiSanModels.DT_LoaiTS(false, "--Chọn tất cả loại tài sản--");
       SelectOptionList slTaiSan = new SelectOptionList(dtTaiSan, "iID_MaLoaiTaiSan", "TenHT");
       dtTaiSan.Rows.InsertAt(dtTaiSan.NewRow(), 0);
       dtTaiSan.Rows[0]["TenHT"] = "--Chọn tất cả loại tài sản--";
       if (dtTaiSan.Rows.Count > 0)
       {
           iID_MaLoaiTaiSan = Convert.ToString(dtTaiSan.Rows[1]["iID_MaLoaiTaiSan"]);
       }
       else 
       {
           iID_MaLoaiTaiSan = Guid.Empty.ToString();
       }
       dtTaiSan.Dispose();
       /// <summary>
       /// Url Action thực hiện xuất dữ liệu ra file excel
       /// </summary>
       /// <returns></returns>
       String BackURL = Url.Action("Index", "CongSan_Report");
       String urlExport = Url.Action("ExportToExcel", "rptKT_BaoCaoTongHop9", new { NamChungTu = NamChungTu, LoaiBaoCao = LoaiBaoCao, iID_MaLoaiTaiSan = iID_MaLoaiTaiSan, iID_MaDonVi = iID_MaDonVi });
       using (Html.BeginForm("EditSubmit", "rptKT_BaoCaoTongHop9", new { ParentID = ParentID, NamChungTu = NamChungTu, LoaiBaoCao = LoaiBaoCao, iID_MaLoaiTaiSan = iID_MaLoaiTaiSan,  iID_MaDonVi = iID_MaDonVi }))
    {
%>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>BÁO CÁO</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">

        <div id="form2">
<div style="width:100%;float:left;padding-top:10px;background-color:#F0F9FE;padding-bottom:10px;">
 <table width="800" height="69" border="0" style="margin:0 auto;">
  <tr>
    <td width="143" style="text-align:right;"><b>Chọn loại báo cáo : </b></td>
    <td class="style6"> <%=MyHtmlHelper.DropDownList(ParentID, slLoaiBaoCao, LoaiBaoCao, "LoaiBaoCao", "", "class=\"input1_2\"style=\"width: 70%\"")%></td>
    <td class="style5" style="text-align:right;"><b>Chọn đơn vị :</b> </td>
    <td width="193"> <%=MyHtmlHelper.DropDownList(ParentID, slTenDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\"style=\"width: 70%\"")%></td>
  </tr>
  <tr>
    <td style="text-align:right;"><b>Chọn loại tài sản : </b></td>
    <td class="style6"> <%=MyHtmlHelper.DropDownList(ParentID, slTaiSan, iID_MaLoaiTaiSan, "iID_MaLoaiTaiSan", "", "class=\"input1_2\"style=\"width: 70%\"")%> </td>
    <td class="style5" style="text-align:right;"><b>chọn năm :</b> </td>
    <td><%=MyHtmlHelper.DropDownList(ParentID, slNam, NamChungTu, "NamChungTu", "", "class=\"input1_2\" style=\"width: 70%\"")%></td>
  </tr>
</table>
                   <table cellpadding="0" cellspacing="0" border="0" align="center" style="margin-left:500px;">
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
                            </table>                
   </div>
</div>
 </div>
  <script type="text/javascript">
      function Huy() {
          window.location.href = '<%=BackURL%>';
      }
    </script>
   <div>
    <%=MyHtmlHelper.ActionLink(urlExport, "Export To Excel") %>
    </div>
</div>
<%  } %>
<br />
   <iframe src="<%=Url.Action("ViewPDF","rptKT_BaoCaoTongHop9",new{NamChungTu=NamChungTu,LoaiBaoCao=LoaiBaoCao,iID_MaLoaiTaiSan=iID_MaLoaiTaiSan, iID_MaDonVi = iID_MaDonVi })%>" height="600px" width="100%"></iframe>
</body>
</html>
