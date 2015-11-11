<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.TongHop" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 415px;
        }
        .style2
        {
            height: 30px;
            width: 103px;
        }
        .style3
        {
            width: 106px;
        }
        .style4
        {
            height: 30px;
            width: 106px;
        }
    </style>
</head>
<body>
  <%
       String srcFile = Convert.ToString(ViewData["srcFile"]);
       String ParentID = "KeToanTongHop";
       String Nam = DateTime.Now.Year.ToString();
       String iID_MaLoaiTaiSan = Request.QueryString["iID_MaLoaiTaiSan"];
       String NamLamViec = Request.QueryString["NamLamViec"];

       String LoaiBaoCao = Request.QueryString["LoaiBaoCao"];
       DataTable dtLoaiBaoCao = rptKT_BaoCaoTongHop9Controller.DTLoaiBaoCao();
       SelectOptionList slLoaiBaoCao = new SelectOptionList(dtLoaiBaoCao, "MaLoaiBaoCao", "TenLoaiBaoCao");
       if (String.IsNullOrEmpty(LoaiBaoCao))
       {
           LoaiBaoCao = Convert.ToString(dtLoaiBaoCao.Rows[0]["MaLoaiBaoCao"].ToString());
       }
       if (String.IsNullOrEmpty(NamLamViec))
       {
           NamLamViec = DateTime.Now.Year.ToString();
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
       String TuNgay = Request.QueryString["TuNgay"];
       String DenNgay = Request.QueryString["DenNgay"];
       String TuThang = Request.QueryString["TuThang"];
       String DenThang = Request.QueryString["DenThang"];
       String iID_MaPhongBan = Request.QueryString["iID_MaPhongBan"];
       String UserID = User.Identity.Name;
       if (String.IsNullOrEmpty(TuNgay) == true)
       {
           TuNgay = "1";
       }
       if (String.IsNullOrEmpty(TuThang) == true || TuThang == "")
       {
           TuThang = DateTime.Now.Month.ToString();
       }
       if (String.IsNullOrEmpty(DenThang) == true)
       {
           //DenThang = TuThang;
           DenThang = DateTime.Now.Month.ToString();
       }
       if (String.IsNullOrEmpty(DenNgay) == true)
       {
           DenNgay = HamChung.GetDaysInMonth(Convert.ToInt32(DenThang), Convert.ToInt32(Nam)).ToString();
       }
       //tháng     
       var dtThang = HamChung.getMonth(DateTime.Now, false, "", "Tháng");
       SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
       if (dtThang != null) dtThang.Dispose();

       ///ngày
       var dtNgay = HamChung.getDaysInMonths(DateTime.Now.Month, DateTime.Now.Year, false, "", "Ngày");
       SelectOptionList slNgay = new SelectOptionList(dtNgay, "MaNgay", "TenNgay");
       if (dtNgay != null) dtNgay.Dispose();
       DataTable dtPhongBan = NganSach_HamChungModels.DSPhongBanCuaNguoiDung(UserID);
       dtPhongBan.Rows.InsertAt(dtPhongBan.NewRow(), 0);
       dtPhongBan.Rows[0]["sTen"] = "--Chọn tất cả đơn vị--";
       SelectOptionList slPhongBan = new SelectOptionList(dtPhongBan, "iID_MaPhongBan", "sTen");
       if (dtPhongBan.Rows.Count > 0)
       {

           iID_MaPhongBan = Convert.ToString(dtPhongBan.Rows[1]["iID_MaPhongBan"]);

       }
       else
       {
           iID_MaPhongBan = Guid.Empty.ToString();
       }
       dtPhongBan.Dispose();
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
       using (Html.BeginForm("EditSubmit", "rptKT_BaoCaoTongHop9", new { ParentID = ParentID, NamLamViec = NamLamViec, LoaiBaoCao = LoaiBaoCao, iID_MaLoaiTaiSan = iID_MaLoaiTaiSan, TuNgay = TuNgay, DenNgay = DenNgay, TuThang = TuThang, DenThang = DenThang, iID_MaPhongBan = iID_MaPhongBan }))
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
   <div style="width:50%;float:left;margin:0; padding:0;">
       <table style="width: 100%;">
        <tr>
               <td style=" width: 126px;text-align: right;height:30px;">
                  <div><b>Chọn Năm Chứng từ:</b></div></td>
               <td>
                  <%=MyHtmlHelper.DropDownList(ParentID, slNam, NamLamViec, "iNamLamViec", "", "class=\"input1_2\" style=\"width: 30%\"")%>
           </tr>
          <tr>
               <td style=" width: 126px;text-align: right;height:30px;">
                  <div><b>Chọn báo cáo:</b></div></td>
               <td>
                  <%=MyHtmlHelper.DropDownList(ParentID, slLoaiBaoCao, LoaiBaoCao, "LoaiBaoCao", "", "class=\"input1_2\"style=\"width: 30%\"")%></td>
           </tr>
           <tr>
               <td style=" width: 126px;text-align: right;height:30px;">
                  <div><b>Chọn đơn vị:</b></div>
               </td>
               <td>
                    <%=MyHtmlHelper.DropDownList(ParentID, slPhongBan, iID_MaPhongBan, "iID_MaPhongBan", "", "class=\"input1_2\"style=\"width: 30%\"")%></td>
           </tr>
       </table>
   </div>
   <div style="width:49%;float:left;margin:0; padding:0;">
       <table class="style1">
         <tr>
               <td style=" text-align: right;" class="style2">
                  <div><b>Chọn Loại Tài Sản:</b></div></td>
               <td class="style3">
                  <%=MyHtmlHelper.DropDownList(ParentID, slTaiSan, iID_MaLoaiTaiSan, "iID_MaLoaiTaiSan", "", "class=\"input1_2\"style=\"width: 100%\"")%></td>
           </tr>
           <tr>
               <td style="text-align:right;" class="style2">
                   <div>
                                <%=NgonNgu.LayXau("Từ ngày:")%></div></td>
               <td class="style3">
                   <%=MyHtmlHelper.DropDownList(ParentID, slNgay, TuNgay, "TuNgay", "", "class=\"input1_2\"")%></td>
               <td style="width: 69px;height:30px;text-align:right;">
                  <div>
                                <%=NgonNgu.LayXau("tháng:")%></div></td>
               <td>
                    <%=MyHtmlHelper.DropDownList(ParentID, slThang, TuThang, "TuThang", "", "class=\"input1_2\"")%></td>
           </tr>
           <tr>
                  <td style="text-align:right;" class="style2">
                  <div>
                                <%=NgonNgu.LayXau("Đến ngày:")%></div></td>
                  <td style=" text-align:right;" class="style4">
                    <%=MyHtmlHelper.DropDownList(ParentID, slNgay, DenNgay, "DenNgay", "", "class=\"input1_2\" ")%></td>
                  <td style="width: 69px;height:30px;text-align:right;">
                   <div>
                   <%=NgonNgu.LayXau("tháng:")%></div></td>
                 <td>
                    <%=MyHtmlHelper.DropDownList(ParentID, slThang, DenThang, "DenThang", "", "class=\"input1_2\" ")%></td>
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
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="history.go(-1)" />
                                    </td>
                                </tr>
                            </table>                
   </div>
</div>
 </div>
</div>
<%  } %>
<br />
   <iframe src="<%=Url.Action("ViewPDF","rptKT_BaoCaoTongHop9",new{NamLamViec=NamLamViec,LoaiBaoCao=LoaiBaoCao,iID_MaLoaiTaiSan=iID_MaLoaiTaiSan,TuNgay = TuNgay, DenNgay = DenNgay, TuThang = TuThang,DenThang=DenThang, iID_MaPhongBan = iID_MaPhongBan })%>" height="600px" width="100%"></iframe>
</body>
</html>
