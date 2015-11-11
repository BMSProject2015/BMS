<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<%@ Import Namespace="VIETTEL.Models" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=ConfigurationManager.AppSettings["TitleView"]%></title>
    <link rel="Stylesheet" type="text/css" href="<%= Url.Content("~/Content/Themes/css/style.css") %>" />
    <link rel="Stylesheet" type="text/css" href="<%= Url.Content("~/Content/Themes/css/form.css") %>" />
    <link rel="Stylesheet" type="text/css" href="<%= Url.Content("~/Content/Themes/css/dropdown_one.css") %>" />
    <link rel="Stylesheet" type="text/css" href="<%= Url.Content("~/Content/Themes/custom-theme/redmond/jquery-ui-1.8.2.custom.css") %>" />
    <script src="<%= Url.Content("~/Scripts/jsFunctions.js?id=") %><%=DateTime.Now.ToString("yyMMddHHmmss") %>"
        type="text/javascript"></script>
 
</head>
<body>
    <%
        String MaDiv = Request.QueryString["idDiv"];
        String MaDivDate = Request.QueryString["idDivDate"];
        //String iNamLamViec = Convert.ToString(NguoiDungCauHinhModels.iNamLamViec);
        //int sSoGhiSo = KeToanTongHop_ChungTuModels.GetMaxChungTu_CuoiCung(iNamLamViec);
        //String iID_MaChungTu = KeToanTongHop_ChungTuModels.LayMaChungTu(Convert.ToString(sSoGhiSo));
        //if (String.IsNullOrEmpty(iID_MaChungTu) || iID_MaChungTu == Guid.Empty.ToString())
        //{
        //    iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
        //}
        String iThang = Request.QueryString["iThang"];
        String iNamLamViec = Request.QueryString["iNam"];
        //String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
        String OnSuccess = "";
        OnSuccess = Request.QueryString["OnSuccess"];
        String ParentID = "Create";
        String iNgay = DateTime.Now.Day.ToString();
        if (String.IsNullOrEmpty(iNamLamViec))
        {
            iNamLamViec = DateTime.Now.Year.ToString();
        }
        String sSoChungTu = HamChung.ConvertToString(KeToanTongHop_ChungTuModels.GetMaxChungTu_CuoiCung(iNamLamViec) + 1);

        using (Html.BeginForm("Edit_Fast_Submit_Tao_CTGS", "KeToanTongHop_ChungTu", new { ParentID = ParentID, iNamLamViec = iNamLamViec }))
        {
    %>
    <div style="background-color: #ffffff; background-repeat: repeat">
        <div style="padding: 5px 1px 10px 1px;">
            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="mGrid">
                <tr>
                    <th colspan="6">
                        &nbsp;
                    </th>
                </tr>
                <tr>
                    <td class="td_label">
                        <div>
                            <b>Số C.T.G.S sao chép <span style="color: Red;">*</span></b>
                        </div>
                    </td>
                    <td>
                        <div>
                            <%=MyHtmlHelper.TextBox(ParentID, "", "sSoChungTu_SaoChep", "", "class=\"textbox_uploadbox\" style=\"width:99%;background-color: #f8e6d1;\" maxlength='50' tab-index=\"-1\"")%>
                        </div>
                    </td>
                    <td class="td_label" colspan="4">
                        <div>
                            <b>Lấy toàn bộ nội dung C.T.G.S sao chép</b>
                            <%=MyHtmlHelper.CheckBox(ParentID, "1", "iDisplay", "", "onclick=\"CheckDisplay(this.checked)\"")%>
                        </div>
                    </td>
                </tr>
                  <tr >
                    <td class="td_label">
                        <div>
                            <b>Thay đơn vị trên C.T.G.S mới</b>
                            <%=MyHtmlHelper.CheckBox(ParentID, "1", "iDonVi", "", "")%>
                        </div>
                    </td>
                    <td colspan="5">
                        <div>
                            <%=MyHtmlHelper.TextBox(ParentID, "", "sDonVi_Sua", "", "class=\"textbox_uploadbox\" style=\"width:99%;background-color: #f8e6d1;\" maxlength='50' tab-index=\"-1\"")%>
                        </div>
                    </td>
                    
                </tr>
                <tr>
                    <td class="td_label">
                        <div>
                            <b>Số C.T.G.S tạo <span style="color: Red;">*</span></b>
                        </div>
                    </td>
                    <td style="width: 70px">
                        <div>
                            <%=MyHtmlHelper.TextBox(ParentID, sSoChungTu, "sSoChungTu", "", "class=\"textbox_uploadbox\" style=\"width:97%;background-color: #f8e6d1;\" maxlength='50'")%>
                        </div>
                    </td>
                    <td style="width: 50px" class="td_label">
                        <div>
                            <b>Tháng <span style="color: Red;">*</span></b>
                        </div>
                    </td>
                    <td style="width: 50px">
                        <div>
                            <%=MyHtmlHelper.TextBox(ParentID, iThang, "iThang", "", "class=\"textbox_uploadbox\" style=\"width:97%;background-color: #f8e6d1;\" disabled =\"disabled\"")%>
                        </div>
                    </td>
                    <td class="td_label">
                        <div>
                            <b>Năm</b>
                        </div>
                    </td>
                    <td>
                        <div>
                            <%=MyHtmlHelper.TextBox(ParentID, iNamLamViec, "iNamLamViec", "", "class=\"textbox_uploadbox\" style=\"width:97%;background-color: #f8e6d1;\"  disabled =\"disabled\"")%>
                        </div>
                    </td>
                </tr>
                <tr id="tb_CanBo0">
                    <td class="td_label">
                        <div>
                            <b>Ngày <span style="color: Red;">*</span></b>
                        </div>
                    </td>
                    <td>
                        <div>
                            <%=MyHtmlHelper.TextBox(ParentID, iNgay, "iNgay", "", "class=\"textbox_uploadbox\" style=\"width:97%;\" maxlength='2'", 1)%>
                        </div>
                    </td>
                    <td class="td_label">
                        <div>
                            <b>Tập</b>
                        </div>
                    </td>
                    <td>
                        <div>
                            <%=MyHtmlHelper.TextBox(ParentID, "", "iTapSo", "", "class=\"textbox_uploadbox\" style=\"width:97%;\" maxlength='2' ",1)%>
                        </div>
                    </td>
                    <td class="td_label">
                        <div>
                            <b>Đơn vị <span style="color: Red;">*</span></b>
                        </div>
                    </td>
                    <td>
                        <div>
                            <%=MyHtmlHelper.TextBox(ParentID, "", "sDonVi", "", "class=\"textbox_uploadbox\" style=\"width:97%;\" maxlength='12' ")%>
                        </div>
                    </td>
                </tr>
                <tr id="tb_CanBo1">
                    <td class="td_label">
                        <div>
                            <b>Nội dung C.T.G.S <span style="color: Red;">*</span> </b>
                        </div>
                    </td>
                    <td colspan="5">
                        <div>
                            <%=MyHtmlHelper.TextBox(ParentID, "", "sNoiDung", "", "class=\"textbox_uploadbox\" style=\"width:99%;\" maxlength='50'")%>
                        </div>
                    </td>
                </tr>
            </table>
            <div style="height: 20px;">
                &nbsp;</div>
            <table border="0" cellpadding="0" cellspacing="0" style="padding-top: 10px;" align="center"
                width="100%">
                <tr>
                    <td align="right" style="width: 45%">
                        <input type="submit" class="button4" value="Thêm" />
                    </td>
                    <td style="width: 1%;">
                        &nbsp;
                    </td>
                    <td align="left" style="width: 45%">
                        <input type="button" class="button4" value="Hủy" onclick="Dialog_close('<%=ParentID %>');" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <%
        } 
    %>
    <script type="text/javascript">
        CheckDisplay('True');
        function CheckDisplay(value) {
            for (var i = 0; i < 2; i++) {
                if (value == true || value == 'True' || value == '1') {
                    document.getElementById('tb_CanBo' + i).style.display = 'none';
                } else {

                    document.getElementById('tb_CanBo' + i).style.display = '';
                }
            }
        }
      
        $(function () {
            document.getElementById("Create_sSoChungTu_SaoChep").focus();
        });
    </script>
</body>
</html>
