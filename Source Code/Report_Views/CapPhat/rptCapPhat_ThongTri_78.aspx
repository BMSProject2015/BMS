<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.CapPhat" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
  <%  
      
     
      String LuyKe = Request.QueryString["LuyKe"];
      if(String.IsNullOrEmpty(LuyKe))
      {
          LuyKe = "";
      }
      String srcFile = Convert.ToString(ViewData["srcFile"]);
      String ParentID = "CapPhat";

       String iID_MaCapPhat = Convert.ToString(ViewData["iID_MaCapPhat"]);

       String sMaDonVi = Convert.ToString(ViewData["sMaDonVi"]);
       String[] arrMaDonVi = sMaDonVi.Split(',');
       int ChiSo = Convert.ToInt16(ViewData["ChiSo"]);
       String iID_MaDonVi = arrMaDonVi[ChiSo];
       if (ChiSo + 1 < arrMaDonVi.Length)
       {
           iID_MaDonVi = arrMaDonVi[ChiSo];
           ChiSo = ChiSo + 1;
       }
       else{
           ChiSo=0;
       }
      String sLoaiThongTri = Convert.ToString(ViewData["sLoaiThongTri"]);
      if (String.IsNullOrEmpty(sLoaiThongTri))
      {
          sLoaiThongTri = "sNG";
      }

      String urlExportToExcel = Url.Action("ExportToExcel", "rptCapPhat_ThongTri_78", new { iID_MaDonVi = iID_MaDonVi, iID_MaCapPhat = iID_MaCapPhat, sLoaiThongTri = sLoaiThongTri });

      using (Html.BeginForm("EditSubmit_Next", "rptCapPhat_ThongTri_78", new { ParentID = ParentID, iID_MaCapPhat = iID_MaCapPhat, sLoaiThongTri = sLoaiThongTri, ChiSo = ChiSo }))
      {                  
   %>    
   <%=MyHtmlHelper.Hidden(ParentID,sMaDonVi,"sMaDonVi","") %>
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
            <div id="Div1">
                <div id="Div2">
                    <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">                       
                       <tr>
                        <td class="td_form2_td1">
                            <%
                            string _checked="";
                            for (int i = 0; i < arrMaDonVi.Length; i++)
                              {
                                    _checked = "";
                                  if(iID_MaDonVi==arrMaDonVi[i])
                                      _checked = "checked=\"checked\"";
                                  %>
                               <input type="radio" value="<%=arrMaDonVi[i]%>" <%=_checked %> id="iID_MaDonVi" name="iID_MaDonVi"/>
                               &nbsp;&nbsp;<%=DonViModels.Get_TenDonVi(arrMaDonVi[i])%>
                            <%} %>
                        </td>
                        <td>
                            <table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;">
                                <tr>
                                    <td>
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="5px">
                                    </td>
                                    <td>
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="history.go(-1)" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    </table>                   
                </div>
            </div>
        </div>
         
   <%} %>
   <%=MyHtmlHelper.ActionLink(urlExportToExcel,"Export to Excel") %>
    <iframe src="<%=Url.Action("ViewPDF","rptCapPhat_ThongTri_78",new{iID_MaDonVi=iID_MaDonVi,iID_MaCapPhat=iID_MaCapPhat,sLoaiThongTri=sLoaiThongTri})%>" height="600px" width="100%"></iframe>
</body>
</html>
