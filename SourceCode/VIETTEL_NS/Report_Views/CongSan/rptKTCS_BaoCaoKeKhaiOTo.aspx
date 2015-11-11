<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.CongSan.ThongKe" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 406px;
            margin-left: 35px;
        }
        .style2
        {
            height: 30px;
            width: 138px;
        }
        .style3
        {
            width: 106px;
        }
        </style>
</head>
<body>
  <%
        //Báo cáo kê khai tài sản Ô Tô
       String srcFile = Convert.ToString(ViewData["srcFile"]);
       String ParentID = "KeToanCongSan";
       String Nam = DateTime.Now.Year.ToString();
       String iID_MaLoaiTaiSan = Request.QueryString["iID_MaLoaiTaiSan"];
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
       String[] arrLoaiBieu = { "rLoaiTaiSan", "rDonVi", "rLoaiTaiSanDonVi" };
       if (String.IsNullOrEmpty(LoaiBieu))
           LoaiBieu = arrLoaiBieu[0];
       /// <summary>
       /// DataTable lấy dữ liệu cho commbox đơn vị
       /// </summary>
       /// <returns></returns>
       DataTable dtNguonDonVi = rptKTCS_KeKhaiTaiSanController.ListDonVi();
       if (dtNguonDonVi.Rows.Count > 0)
       {
           iID_MaDonVi = Convert.ToString(dtNguonDonVi.Rows[0]["iID_MaDonVi"]);
       }
       else 
       {
           iID_MaDonVi = Guid.Empty.ToString();
       }
       dtNguonDonVi.Dispose();
       SelectOptionList slTenDonVi = new SelectOptionList(dtNguonDonVi, "iID_MaDonVi", "sTenDonVi");
       dtNguonDonVi.Rows.InsertAt(dtNguonDonVi.NewRow(), 0);
       dtNguonDonVi.Rows[0]["sTenDonVi"] = "--Chọn tất cả đơn vị--";
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
          iID_MaLoaiTaiSan=Guid.Empty.ToString(); 
       }
       dtTaiSan.Dispose();
       /// <summary>
       /// Action thực hiện xuất dữ liệu ra file excel
       /// </summary>
       /// <returns></returns>
       String BackURL = Url.Action("Index", "CongSan_Report");
       String urlExport = Url.Action("ExportToExcel", "rptKTCS_BaoCaoKeKhaiOtO", new { NamChungTu = NamChungTu, iID_MaLoaiTaiSan = iID_MaLoaiTaiSan, iID_MaDonVi = iID_MaDonVi, LoaiBieu = LoaiBieu });
       using (Html.BeginForm("EditSubmit", "rptKTCS_BaoCaoKeKhaiOtO", new { ParentID = ParentID, NamChungTu = NamChungTu, iID_MaLoaiTaiSan = iID_MaLoaiTaiSan, iID_MaDonVi = iID_MaDonVi, LoaiBieu = LoaiBieu }))
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
<div style="width:100%;float:left;background-color:#F0F9FE;padding-bottom:10px;">
<div style="width:1024px;margin-left:150px;">


<table width="800" border="0" cellpadding="0" cellspacing="0" style="height: 86px">
  <tr>
    <td><div align="right"><b>Chọn năm :  </div></td>
    <td><div align="left"><%=MyHtmlHelper.DropDownList(ParentID, slNam, NamChungTu, "NamChungTu", "", "class=\"input1_2\" style=\"width: 70%\"")%></div></td>
    <td><div align="right"><b>Chọn loại tài sản : </b></div></td>
    <td><div align="left"><%=MyHtmlHelper.DropDownList(ParentID, slTaiSan, iID_MaLoaiTaiSan, "iID_MaLoaiTaiSan", "", "class=\"input1_2\"style=\"width: 70%\"")%></div></td>
    <td><div align="right"><b>Chọn đơn vị :</b></div></td>
    <td><div align="left"><%=MyHtmlHelper.DropDownList(ParentID, slTenDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\"style=\"width: 80%\"")%></div></td>
  </tr>

</table>
</div>
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
   <iframe src="<%=Url.Action("ViewPDF","rptKTCS_BaoCaoKeKhaiOtO",new{NamChungTu=NamChungTu,iID_MaLoaiTaiSan=iID_MaLoaiTaiSan, iID_MaDonVi = iID_MaDonVi,LoaiBieu=LoaiBieu })%>" height="600px" width="100%"></iframe>
</body>
</html>
