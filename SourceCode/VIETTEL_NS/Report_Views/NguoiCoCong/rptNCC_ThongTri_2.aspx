<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.NguoiCoCong" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
   <%
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "NguoiCoCong";
        String NamLamViec = Request.QueryString["NamLamViec"];
        String UserID = User.Identity.Name;
        String Thang_Quy = "",LoaiThangQuy="";
        String iID_MaDonVi = Request.QueryString["iID_MaDonVi"];
        String sLNS=Request.QueryString["sLNS"];
        if(String.IsNullOrEmpty(sLNS))
        {
            sLNS = Guid.Empty.ToString();
        }
      
        Thang_Quy = Convert.ToString(Request.QueryString["Thang_Quy"]);

        if (String.IsNullOrEmpty(Thang_Quy))
        {
            Thang_Quy = Guid.Empty.ToString();
        }
        LoaiThangQuy = Convert.ToString(Request.QueryString["LoaiThangQuy"]);
        if (String.IsNullOrEmpty(LoaiThangQuy))
        {
            LoaiThangQuy = Guid.Empty.ToString();
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
        dtNam.Rows[0]["TenNam"] = "-- Bạn chọn năm ngân sách --";
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        dtNam.Dispose();
        DataTable dtThang = DanhMucModels.DT_Thang();
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        dtThang.Dispose();
        DataTable dtQuy = DanhMucModels.DT_Quy();
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
        dtQuy.Dispose();
        DataTable dtLNS = DanhMucModels.NS_LoaiNganSachNguoiCoCong(true);
        SelectOptionList slLNS = new SelectOptionList(dtLNS, "sLNS", "TenHT");
        dtLNS.Dispose();
        DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(UserID);
        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
        dtDonVi.Dispose();
        String BackURL = Url.Action("Index", "NguoiCoCong_Report");
        String urlExport = Url.Action("ExportToExcel", "rptNCC_ThongTri_2", new { NamLamViec = NamLamViec, sLNS = sLNS, Thang_Quy = Thang_Quy, iID_MaDonVi = iID_MaDonVi });
        using (Html.BeginForm("EditSubmit", "rptNCC_ThongTri_2", new { ParentID = ParentID, NamLamViec = NamLamViec, sLNS = sLNS, Thang_Quy = Thang_Quy, iID_MaDonVi = iID_MaDonVi }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1" style="background-color:#F0F9FE;">
            <div id="Div2">

                <table width="1000" border="0" cellpadding="0" cellspacing="0" style="height: 104px">
  <tr>
    <td width="213" class="style1"><div align="right"> <b> <%=NgonNgu.LayXau("Chọn năm làm việc :")%></b> </div></td>
    <td width="272" class="style1"> <%=MyHtmlHelper.DropDownList(ParentID, slNam, NamHienTai, "iNamLamViec", "", "class=\"input1_2\" style=\"width: 100%;height:24px;\"")%></td>
    <td width="194" class="style1"><div align="right"><b>Tháng/Quý : </b></div></td>
    <td width="293" class="style1"> <%=MyHtmlHelper.Option(ParentID, "0","0", "ThangQuy", "")%>Tháng&nbsp;&nbsp;
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, "", "iThang", "", "class=\"input1_2\" style=\"width:80px;height:24px;\"")%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <%=MyHtmlHelper.Option(ParentID,"1", "","ThangQuy","")%>Quý&nbsp;&nbsp;
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, "", "iQuy", "", "class=\"input1_2\" style=\"width:85px;height:24px;\"")%><br /></td>
  </tr>
  <tr>
    <td class="style1"><div align="right"><b>Loại ngân sách :</b> </div></td>
    <td class="style1"><%=MyHtmlHelper.DropDownList(ParentID, slLNS, sLNS, "sLNS", "", "class=\"input1_2\"style=\"width: 100%;height:24px;\"")%></td>
    <td class="style1"><div align="right"><b><%=NgonNgu.LayXau("Chọn đơn vị :")%></b></div></td>
    <td class="style1"><%=MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 100%;height:24px;\"")%> </td>
  </tr>
  <tr>
    <td colspan="4" align="center"><table cellpadding="0" cellspacing="0" border="0" align="center" style="margin-left:140px;">
                                <tr>
                                    <td>
                                        <input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
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
                function Huy() {
                    window.location.href = '<%=BackURL%>';
                }
            </script>
        </div>
          
         <div>
    <%=MyHtmlHelper.ActionLink(urlExport, "Export To Excel") %>
    </div>
    </div>
    <%} %>
    <iframe src="<%=Url.Action("ViewPDF","rptNCC_ThongTri_2", new{NamLamViec=NamLamViec,LoaiThangQuy=LoaiThangQuy,sLNS=sLNS,Thang_Quy=Thang_Quy,iID_MaDonVi=iID_MaDonVi})%>" height="600px" width="100%">
    </iframe>
</body>
</html>
