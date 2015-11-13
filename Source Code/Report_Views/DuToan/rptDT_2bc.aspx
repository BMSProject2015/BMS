<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.DuToan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
   <%
    String srcFile = Convert.ToString(ViewData["srcFile"]);
    String ParentID = "BaoCaoNganSachNam";
    String MaND=User.Identity.Name;
    String iID_MaTrangThaiDuyet = Request.QueryString["iID_MaTrangThaiDuyet"];
    DataTable dtTrangThai = rtprptDuToanChiNganSachQuocPhongXDCBCongTrinhPhoThongController.tbTrangThai();
    SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "TenTrangThai");
    if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
    {
        if (dtTrangThai.Rows.Count > 0)
        {
            iID_MaTrangThaiDuyet = Convert.ToString(dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"]);
        }
        else
        {
            iID_MaTrangThaiDuyet = Guid.Empty.ToString();
        }
    }
    dtTrangThai.Dispose();
    
       
    String BackURL = Url.Action("Index", "DuToan_Report", new { sLoai="0"});

    using (Html.BeginForm("EditSubmit", "rptDT_2bc", new { ParentID = ParentID, MaND = MaND, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet }, FormMethod.Post, new { enctype = "multipart/form-data" }))
    {
    %>   
    <div class="box_tong">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Báo cáo dự toán chi ngân sách sử dụng ( Phần Nghiệp vụ 00 )</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
                 <table width="100%" cellpadding="0"  cellspacing="0" border="0" class="table_form2">
                    <tr>
                       <%-- <td class="td_form2_td1" style="text-align:center;width:40%">
                            <div><b>Chọn năm làm việc: </b>   <%=MyHtmlHelper.DropDownList(ParentID, slNam, NamLamViec, "iNamLamViec", "", "class=\"input1_2\" style=\"width: 30%;height:24px;\"")%>
                                 <%= Html.ValidationMessage(ParentID + "_" + "err_iNamLamViec")%>
                            </div>
                        </td>--%>
                        <td class="td_form2_td1" style="text-align:center;width:60%">
                            <div><b>Chọn trạng thái: </b>   <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 30%;height:24px;\"")%>    
                            </div>
                        </td>
                        
                    </tr>

                      <tr>
                                    <td class="td_form2_td1" style="width: 25%">
                                        <div>
                                            <%=NgonNgu.LayXau("File tài liệu ")%>
                                            <b style="color: Red">(*)</b></div>
                                    </td>
                                    <td class="td_form2_td5" style="width: 75%">
                                        <div>
                                            <%=MyHtmlHelper.Label("", "sFileName1", "")%>
                                            <%=MyHtmlHelper.Hidden(ParentID, "", "sFileName", "")%>
                                            <%=MyHtmlHelper.Hidden(ParentID, "", "sURL", "")%>
                                        </div>
                                        <div>
                                            <%--<% =MyHtmlHelper.UploadFile("uploadFile", path, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"))%>--%>
                                            <input type="file" name="uploadFile" id="uploadFile" style="width: 95%" onchange="checkFileExtension();" /><br />
                                             <%-- <%=MyHtmlHelper.Hidden(ParentID, data, "sFileName","")%>
                                            <br />
                                            <%= MyHtmlHelper.TextBox(ParentID, data, "sURL", "", "readonly style=\"width:10px; display:none;\"")%>--%>
                                          <%--  <script type="text/javascript">
                                                uploadFile.addListener(uploadFile.UPLOAD_COMPLETE, uploadFile_OnComplete);
                                                function uploadFile_OnComplete(FileName, url) {
                                                    document.getElementById("<%=ParentID%>_sFileName").value = FileName;
                                                    document.getElementById("<%=ParentID%>_sURL").value = uploadFile.serverPath + "/" + url;
                                                }
                                            </script>--%>
                                             <%= Html.ValidationMessage(ParentID + "_" + "err_sFileName")%>
                                        </div>
                                    </td>
                                </tr>
                    <tr>
                        <td class="td_form2_td1" style="margin:0 auto;" colspan = "2">
                            <table cellpadding="0" cellspacing="0" border="0" style="margin-left:500px;">
                                <tr>
                                    <td><input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" /></td>
                                    <td width="5px"></td>
                                    <td><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                 </table>
            </div>
        </div>
    </div>
    <%}
       
         %>
 
      <script type="text/javascript">
          function Huy() {
              window.location.href = '<%=BackURL%>';
          }
    </script> 
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptDT_2bc", new { MaND = MaND, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet }), "Export to Excel")%>
    <iframe src="<%=Url.Action("ViewPDF","rptDT_2bc",new{MaND=MaND, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet})%>" height="600px" width="100%"></iframe>
</body>
</html>
