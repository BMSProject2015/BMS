<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient"%>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<%@ Import Namespace="VIETTEL.Models" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="Content-Style-Type" content="text/css" />
    <link rel="Stylesheet" type="text/css" href="<%= Url.Content("~/Content/Themes/css/style.css") %>" />
    <link rel="Stylesheet" type="text/css" href="<%= Url.Content("~/Content/Themes/css/form.css") %>" />
    <link rel="Stylesheet" type="text/css" href="<%= Url.Content("~/Content/Themes/css/dropdown_one.css") %>" />
    <link rel="Stylesheet" type="text/css" href="<%= Url.Content("~/Content/Themes/custom-theme/redmond/jquery-ui-1.8.2.custom.css") %>" />
    <!--[if lte IE 6]><link href="<%= Url.Content("~/Content/Themes/css/modal-window-ie6.css") %> type="text/css" rel="stylesheet" /><![endif]-->

    <script src="<%= Url.Content("~/Scripts/iepngfix_tilebg.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/MicrosoftAjax.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/MicrosoftMvcAjax.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-1.4.2.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-ui-1.8.21.custom.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.ui.datepicker-vi.js") %>" type="text/javascript"></script> 
    <script src="<%= Url.Content("~/Scripts/jquery.icolor.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/swfobject.js") %>" type="text/javascript"></script> 
    <script src="<%= Url.Content("~/Scripts/jquery.maskedinput-1.3.min.js") %>" type="text/javascript"></script> 
    <script src="<%= Url.Content("~/Scripts/ddaccordion.js") %>" type="text/javascript"></script> 
    <script src="<%=Url.Content("~/Scripts/ckeditor/ckeditor.js") %>" type="text/javascript"></script>
    
    <script src="<%= Url.Content("~/Scripts/jsUpload.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jsNumber.js?id=") %><%=DateTime.Now.ToString("yyMMddHHmmss") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jsDate.js?id=") %><%=DateTime.Now.ToString("yyMMddHHmmss") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jsString.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jsFunctions.js?id=") %><%=DateTime.Now.ToString("yyMMddHHmmss") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jsControl.js?id=") %><%=DateTime.Now.ToString("yyMMddHHmmss") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jsInit.js?id=") %><%=DateTime.Now.ToString("yyMMddHHmmss") %>" type="text/javascript"></script>
    <%--<script type="text/jscript">
//        $(document).ready(function () {
//            urlServerPath = '<%= Url.Content("~")%>';
//        })
//        document.ready( function (){
//            $('#save_button').onClick( function(){
//                alert(1);
//                $("#dialog_form").ajaxSubmit();
//                alert(2);
//            }
//        }
    </script>--%>
    <title><%=ConfigurationManager.AppSettings["TitleView"]%></title>
</head>
<body><div>
    <%
        String iID_MaSanPham = Convert.ToString(Request.QueryString["iID_MaSanPham"]);
        String iID_MaMauDanhMuc = Convert.ToString(Request.QueryString["iID_MaMauDanhMuc"]);
        String iID_MaMauDanhMuc_Cha = Convert.ToString(Request.QueryString["iID_MaMauDanhMuc_Cha"]);
        String iID_MaLoaiHinh = Convert.ToString(Request.QueryString["iID_MaLoaiHinh"]);
        String ParentID = "Edit";
        String sKyHieu = "", sTen = "", iLoai = "", iDM_MaDonViTinh = "";
        bool bLaHangCha = false;

        //chi tiết danh mục nếu trong trường hợp sửa
        DataTable dt = SanPham_DanhMucGiaModels.Get_ChiTietDanhMucGia_Row(iID_MaMauDanhMuc);
        DataRow R;
        if (dt != null)
        {
            if (dt.Rows.Count > 0 && iID_MaMauDanhMuc != null && iID_MaMauDanhMuc != "")
            {
                R = dt.Rows[0];
                sKyHieu = HamChung.ConvertToString(R["sKyHieu"]);
                sTen = HamChung.ConvertToString(R["sTen"]);
                iLoai = Convert.ToString(R["iLoai"]);
                bLaHangCha = Convert.ToBoolean(R["bLaHangCha"]);
                iDM_MaDonViTinh = HamChung.ConvertToString(R["iDM_MaDonViTinh"]);
            }
        }
        String tgLaHangCha = "";
        if (bLaHangCha == true)
        {
            tgLaHangCha = "on";
        }
        //lấy dữ liệu đưa vào Combobox
        //
        SqlCommand cmd;
        cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTen FROM DC_DanhMuc " +
                       "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                   "FROM DC_LoaiDanhMuc " +
                                                                   "WHERE sTenBang = 'DonViTinh') ORDER BY sTen");
        dt = Connection.GetDataTable(cmd);
        R = dt.NewRow();
        R["iID_MaDanhMuc"] = "dddddddd-dddd-dddd-dddd-dddddddddddd";
        R["sTen"] = "-- Đơn vị tính --";
        dt.Rows.InsertAt(R, 0);
        SelectOptionList slDonViTinh = new SelectOptionList(dt, "iID_MaDanhMuc", "sTen");
        cmd.Dispose();


        String DuLieuMoi = "0";
        if (String.IsNullOrEmpty(iID_MaMauDanhMuc))
        {
            DuLieuMoi = "1";
        }
        String urlSubmit = Url.Action("EditSubmit", "SanPham_MauDanhMuc", new { ParentID = ParentID, iID_MaMauDanhMuc = iID_MaMauDanhMuc });
        using (Html.BeginForm("EditSubmit", "SanPham_MauDanhMuc", new { ParentID = ParentID, iID_MaMauDanhMuc = iID_MaMauDanhMuc }, FormMethod.Post, new { }))
        {
    %>
    <%= Html.Hidden(ParentID + "_DuLieuMoi", DuLieuMoi)%>
    <%= Html.Hidden(ParentID + "_iID_MaSanPham", iID_MaSanPham)%>
    <%= Html.Hidden(ParentID + "_iID_MaLoaiHinh", iID_MaLoaiHinh)%>
    <%= Html.Hidden(ParentID + "_iID_MaMauDanhMuc_Cha", iID_MaMauDanhMuc_Cha)%>
    <div class="box_tong">
        <div id="nhapform">
            <div id="form2">
                <table cellpadding="5" cellspacing="5" width="100%">
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Mã khoản mục</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%
                                String strReadonly = "";
                                if (ViewData["DuLieuMoi"] == "0") {
                                    strReadonly = "readonly=\"readonly\""; 
                                }    
                                %>
                                <%=MyHtmlHelper.TextBox(ParentID, sKyHieu, "sKyHieu", "", " " + strReadonly + " class=\"input1_2\" tab-index='-1'", 2)%><br />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Tên khoản mục</b>&nbsp;<span  style="color:Red;">*</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sTen, "sTen", "", "class=\"input1_2\"", 2)%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_sTen")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Đơn vị tính</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                            <%=MyHtmlHelper.DropDownList(ParentID, slDonViTinh, iDM_MaDonViTinh, "iDM_MaDonViTinh", "", "class=\"input1_2\"")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1"><div><b>Là hàng cha</b></div></td>
                        <td class="td_form2_td5">
                            <div><%=MyHtmlHelper.CheckBox(ParentID, tgLaHangCha, "bLaHangCha", String.Format("value='{0}'", bLaHangCha))%></div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <br />
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td width="70%">
                &nbsp;
            </td>
            <td width="30%" align="right">
                <table cellpadding="0" cellspacing="0" border="0" align="right">
                    <tr>
                        <td>
                            <input type="submit" class="button" id="save_button" value="Lưu"/>
                        </td>
                        <td width="5px">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <%
        } if (dt != null) { dt.Dispose(); };    
    %>
</div>
</body>
</html>
