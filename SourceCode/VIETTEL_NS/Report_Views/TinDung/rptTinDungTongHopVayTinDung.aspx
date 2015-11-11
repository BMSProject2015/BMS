<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.PhanBo" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <div>
     <%
        String ParentID = "TinDung";
        DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
        String iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
        String iThangLamViec = dtCauHinh.Rows[0]["iThangLamViec"].ToString();
        String pageload =Convert.ToString(ViewData["pageload"]);
        String iNam = Convert.ToString(ViewData["iNam"]);
        if (String.IsNullOrEmpty(iNam))
        {
            iNam = iNamLamViec;
        }
        DataTable dtNam = DanhMucModels.DT_Nam(true);
        SelectOptionList slNam = new SelectOptionList(dtNam,"MaNam","TenNam");
        dtNam.Dispose();
        String iThang = Convert.ToString(ViewData["iThang"]);
        if (String.IsNullOrEmpty(iThang))
        {
            iThang = iThangLamViec;
        }
        DataTable dtThang = DanhMucModels.DT_Thang(true);
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        dtThang.Dispose();

        String iThang_1 = Convert.ToString(ViewData["iThang_1"]);
        String iThang_2 = Convert.ToString(ViewData["iThang_2"]);
        String iThang_3 = Convert.ToString(ViewData["iThang_3"]);
        String iThang_4 = Convert.ToString(ViewData["iThang_4"]);
        String iThang_5 = Convert.ToString(ViewData["iThang_5"]);
        String iThang_6 = Convert.ToString(ViewData["iThang_6"]);
        String iThang_7 = Convert.ToString(ViewData["iThang_7"]);
        String iThang_8 = Convert.ToString(ViewData["iThang_8"]);
        String iThang_9 = Convert.ToString(ViewData["iThang_9"]);
        String iThang_10 = Convert.ToString(ViewData["iThang_10"]);

        String iNam_1 = Convert.ToString(ViewData["iNam_1"]);
        String iNam_2 = Convert.ToString(ViewData["iNam_2"]);
        String iNam_3 = Convert.ToString(ViewData["iNam_3"]);
        String iNam_4 = Convert.ToString(ViewData["iNam_4"]);
        String iNam_5 = Convert.ToString(ViewData["iNam_5"]);
        String iNam_6 = Convert.ToString(ViewData["iNam_6"]);
        String iNam_7 = Convert.ToString(ViewData["iNam_7"]);
        String iNam_8 = Convert.ToString(ViewData["iNam_8"]);
        String iNam_9 = Convert.ToString(ViewData["iNam_9"]);
        String iNam_10 = Convert.ToString(ViewData["iNam_10"]);
         
        if (String.IsNullOrEmpty(iThang_1)) iThang_1 = "12";
        if (String.IsNullOrEmpty(iThang_2)) iThang_2 = "3";
        if (String.IsNullOrEmpty(iThang_3)) iThang_3 = "6";
        if (String.IsNullOrEmpty(iThang_4)) iThang_4 = "12";
        if (String.IsNullOrEmpty(iThang_5)) iThang_5 = "12";
        if (String.IsNullOrEmpty(iThang_6)) iThang_6 = "12";
        if (String.IsNullOrEmpty(iThang_7)) iThang_7 = "12";
        if (String.IsNullOrEmpty(iThang_8)) iThang_8 = "12";
        if (String.IsNullOrEmpty(iThang_9)) iThang_9 = "12";
        if (String.IsNullOrEmpty(iThang_10)) iThang_10 = "12";

        if (String.IsNullOrEmpty(iNam_1)) iNam_1 = (Convert.ToInt32(iNamLamViec) + 3).ToString();
        if (String.IsNullOrEmpty(iNam_2)) iNam_2 = (Convert.ToInt32(iNamLamViec) + 4).ToString();
        if (String.IsNullOrEmpty(iNam_3)) iNam_3 = (Convert.ToInt32(iNamLamViec) + 4).ToString();
        if (String.IsNullOrEmpty(iNam_4)) iNam_4 = (Convert.ToInt32(iNamLamViec) + 4).ToString();
        if (String.IsNullOrEmpty(iNam_5)) iNam_5 = (Convert.ToInt32(iNamLamViec) + 4).ToString();
        if (String.IsNullOrEmpty(iNam_6)) iNam_6 = (Convert.ToInt32(iNamLamViec) + 5).ToString();
        if (String.IsNullOrEmpty(iNam_7)) iNam_7 = (Convert.ToInt32(iNamLamViec) + 6).ToString();
        if (String.IsNullOrEmpty(iNam_8)) iNam_8 = (Convert.ToInt32(iNamLamViec) + 7).ToString();
        if (String.IsNullOrEmpty(iNam_9)) iNam_9 = (Convert.ToInt32(iNamLamViec) + 8).ToString();
        if (String.IsNullOrEmpty(iNam_10)) iNam_10 = (Convert.ToInt32(iNamLamViec) + 9).ToString();
        
         
         
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
        {
            iID_MaTrangThaiDuyet = "0";
        }
        DataTable dtTrangThai = ReportModels.Get_dtDSTrangThaiDuyet(PhanHeModels.iID_MaPhanHeTinDung);
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
        String BackURL = Url.Action("Index", "TinDung_Report");
        String URL = "";
        if (pageload == "1") URL = Url.Action("ViewPDF", "rptTinDungTongHopVayTinDung", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iNam = iNam, iThang = iThang,iThang1=iThang_1,iNam1=iNam_1,
                                                                                              iThang2 = iThang_2,
                                                                                              iNam2 = iNam_2,
                                                                                              iThang3 = iThang_3,
                                                                                              iNam3 = iNam_3,
                                                                                              iThang4 = iThang_4,
                                                                                              iNam4 = iNam_4,
                                                                                              iThang5 = iThang_5,
                                                                                              iNam5 = iNam_5,
                                                                                              iThang6 = iThang_6,
                                                                                              iNam6 = iNam_6,
                                                                                              iThang7 = iThang_7,
                                                                                              iNam7 = iNam_7,
                                                                                              iThang8 = iThang_8,
                                                                                              iNam8 = iNam_8,
                                                                                              iThang9 = iThang_9,
                                                                                              iNam9 = iNam_9,
                                                                                              iThang10 = iThang_10,
                                                                                              iNam10 = iNam_10
        });
        using (Html.BeginForm("EditSubmit", "rptTinDungTongHopVayTinDung", new { ParentID = ParentID }))
        {%>
         <div class="box_tong">
          <div class="title_tong">
                 <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td>
                            <span>Báo cáo tình hình đầu tư tín dụng</span>
                         </td>
                    </tr>
                </table>
            </div>
                <div id="Div1">
                <div id="Div2">
                    <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td style="width: 15%;"></td>
                         <td class="td_form2_td1" style="width: 10%;">
                            <div>
                                <%=NgonNgu.LayXau("Năm làm việc:")%></div>
                        </td>
                       <td width="10%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slNam, iNam, "iNam", "", "class=\"input1_2\" style=\"width: 100%\" ")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%;">
                            <div>
                                <%=NgonNgu.LayXau("Tháng làm việc:")%></div>
                        </td>
                       <td width="10%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang, "iThang", "", "class=\"input1_2\" style=\"width: 100%\"  ")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%;">
                            <div>
                                <%=NgonNgu.LayXau("Trạng thái:")%></div>
                        </td>
                       <td width="10%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%\"")%>
                            </div>
                        </td>
                        <td></td>
                    </tr>
                    <tr></tr>
                   
    </table>
    <div style="border-style: solid; border-width: 1px; border-color: #000000">
    
    <table border="0" cellspacing="0" cellpadding="0" width="100%">
    	<tr>
         <td class="td_form2_td1" style="width: 10%;"></td>
    		   <td class="td_form2_td1" style="width: 10%;">
            Giai đoạn 1:</td>
            <td class="td_form2_td1" style="width: 30%; text-align:center">
             <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang_1, "iThang_1", "", "class=\"input1_2\" style=\"width: 20%\"  ")%>
                   /
                   <%=MyHtmlHelper.DropDownList(ParentID, slNam, iNam_1, "iNam_1", "", "class=\"input1_2\" style=\"width: 20%\" ")%>
            </td>
            <td class="td_form2_td1" style="width: 10%;">
            Giai đoạn 6:</td>
            <td class="td_form2_td1" style="width: 30%; text-align:center">
                 <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang_6, "iThang_6", "", "class=\"input1_2\" style=\"width: 20%\"  ")%>
                   /
                   <%=MyHtmlHelper.DropDownList(ParentID, slNam, iNam_6, "iNam_6", "", "class=\"input1_2\" style=\"width: 20%\" ")%>
            </td>
            <td class="td_form2_td1"></td>
    	</tr>
       <tr>
         <td class="td_form2_td1" style="width: 10%;"></td>
    		   <td class="td_form2_td1" style="width: 10%;">
            Giai đoạn 2:</td>
            <td class="td_form2_td1" style="width: 30%; text-align:center">
             <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang_2, "iThang_2", "", "class=\"input1_2\" style=\"width: 20%\"  ")%>
                   /
                   <%=MyHtmlHelper.DropDownList(ParentID, slNam, iNam_2, "iNam_2", "", "class=\"input1_2\" style=\"width: 20%\" ")%>
            </td>
            <td class="td_form2_td1" style="width: 10%;">
            Giai đoạn 7:</td>
            <td class="td_form2_td1" style="width: 30%; text-align:center">
                 <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang_7, "iThang_7", "", "class=\"input1_2\" style=\"width: 20%\"  ")%>
                   /
                   <%=MyHtmlHelper.DropDownList(ParentID, slNam, iNam_7, "iNam_7", "", "class=\"input1_2\" style=\"width: 20%\" ")%>
            </td>
              <td class="td_form2_td1"></td>
    	</tr>
        <tr>
         <td class="td_form2_td1" style="width: 10%;"></td>
    		   <td class="td_form2_td1" style="width: 10%;">
            Giai đoạn 3:</td>
            <td class="td_form2_td1" style="width: 30%; text-align:center">
             <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang_3, "iThang_3", "", "class=\"input1_2\" style=\"width: 20%\"  ")%>
                   /
                   <%=MyHtmlHelper.DropDownList(ParentID, slNam, iNam_3, "iNam_3", "", "class=\"input1_2\" style=\"width: 20%\" ")%>
            </td>
            <td class="td_form2_td1" style="width: 10%;">
            Giai đoạn 8:</td>
            <td class="td_form2_td1" style="width: 30%; text-align:center">
                 <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang_8, "iThang_8", "", "class=\"input1_2\" style=\"width: 20%\"  ")%>
                   /
                   <%=MyHtmlHelper.DropDownList(ParentID, slNam, iNam_8, "iNam_8", "", "class=\"input1_2\" style=\"width: 20%\" ")%>
            </td>
              <td class="td_form2_td1"></td>
    	</tr>
        <tr>
         <td class="td_form2_td1" style="width: 10%;"></td>
    		   <td class="td_form2_td1" style="width: 10%;">
            Giai đoạn 4:</td>
            <td class="td_form2_td1" style="width: 30%; text-align:center">
             <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang_4, "iThang_4", "", "class=\"input1_2\" style=\"width: 20%\"  ")%>
                   /
                   <%=MyHtmlHelper.DropDownList(ParentID, slNam, iNam_4, "iNam_4", "", "class=\"input1_2\" style=\"width: 20%\" ")%>
            </td>
            <td class="td_form2_td1" style="width: 10%;">
            Giai đoạn 9:</td>
            <td class="td_form2_td1" style="width: 30%; text-align:center">
                 <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang_9, "iThang_9", "", "class=\"input1_2\" style=\"width: 20%\"  ")%>
                   /
                   <%=MyHtmlHelper.DropDownList(ParentID, slNam, iNam_9, "iNam_9", "", "class=\"input1_2\" style=\"width: 20%\" ")%>
            </td>
             <td class="td_form2_td1"></td>
    	</tr>
        <tr>
         <td class="td_form2_td1" style="width: 10%;"></td>
    		   <td class="td_form2_td1" style="width: 10%;">
            Giai đoạn 5:</td>
            <td class="td_form2_td1" style="width: 30%; text-align:center">
             <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang_5, "iThang_5", "", "class=\"input1_2\" style=\"width: 20%\"  ")%>
                   /
                   <%=MyHtmlHelper.DropDownList(ParentID, slNam, iNam_5, "iNam_5", "", "class=\"input1_2\" style=\"width: 20%\" ")%>
            </td>
            <td class="td_form2_td1" style="width: 10%;">
            Giai đoạn 10:</td>
            <td class="td_form2_td1" style="width: 30%; text-align:center">
                 <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang_10, "iThang_10", "", "class=\"input1_2\" style=\"width: 20%\"  ")%>
                   /
                   <%=MyHtmlHelper.DropDownList(ParentID, slNam, iNam_10, "iNam_10", "", "class=\"input1_2\" style=\"width: 20%\" ")%>
            </td>
              <td class="td_form2_td1"></td>
    	</tr>
         <tr>
                        <td>
                        </td>
                        <td colspan="4"><table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
                            <tr>
                                    <td style="width: 49%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />                                    </td>
                                    <td width="2%">                                    </td>
                                    <td style="width: 49%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />                                    </td>
                                </tr>
                           </table></td> 
<td>
                        </td>
                    </tr>
    </table>
    </div>
    </div>
    
    </div>
    </div>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptTinDungTongHopVayTinDung", new
{
    iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet,
    iNam = iNam,
    iThang = iThang,
    iThang1 = iThang_1,
    iNam1 = iNam_1,
    iThang2 = iThang_2,
    iNam2 = iNam_2,
    iThang3 = iThang_3,
    iNam3 = iNam_3,
    iThang4 = iThang_4,
    iNam4 = iNam_4,
    iThang5 = iThang_5,
    iNam5 = iNam_5,
    iThang6 = iThang_6,
    iNam6 = iNam_6,
    iThang7 = iThang_7,
    iNam7 = iNam_7,
    iThang8 = iThang_8,
    iNam8 = iNam_8,
    iThang9 = iThang_9,
    iNam9 = iNam_9,
    iThang10 = iThang_10,
    iNam10 = iNam_10
}), "Xuất ra Excels")%>
      <script type="text/javascript">
          function Huy() {
              window.location.href = '<%=BackURL%>';
          }
    </script>
    <%}%>
    <iframe src="<%=URL%>" height="600px" width="100%"></iframe>
    </div>
</body>
</html>
