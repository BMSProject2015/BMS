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
    <title><%=ConfigurationManager.AppSettings["TitleView"]%></title>
</head>
<body><div>
    <%
        String iID_MaSanPham = Convert.ToString(Request.QueryString["iID_MaSanPham"]);
        String iID_MaLoaiHinh = Convert.ToString(Request.QueryString["iID_MaLoaiHinh"]);
        String iID_MaDanhMucGia = Convert.ToString(Request.QueryString["iID_MaDanhMucGia"]);
        String ParentID = "Edit";
        String sKyHieu = "", sTen = "";
        bool bLaHangCha = false;

        //chi tiết danh mục nếu trong trường hợp sửa
        DataTable dt = SanPham_DanhMucGiaModels.Get_ChiTietDanhMucGia_Row(iID_MaDanhMucGia);
        DataRow R;
        if (dt != null)
        {
            if (dt.Rows.Count > 0 && iID_MaDanhMucGia != null && iID_MaDanhMucGia != "")
            {
                R = dt.Rows[0];
                sKyHieu = HamChung.ConvertToString(R["sKyHieu"]);
                sTen = HamChung.ConvertToString(R["sTen"]);
                bLaHangCha = Convert.ToBoolean(R["bLaHangCha"]);
            }
        }
        //lấy dữ liệu đưa vào Combobox
        //
        dt.Dispose();
        DataTable dtChiTiet = SanPham_VatTuModels.Get_VatTuChuaChon(iID_MaDanhMucGia);
        String urlSubmit = Url.Action("EditSubmit", "SanPham_DanhMucGia", new { ParentID = ParentID, iID_MaDanhMucGia = iID_MaDanhMucGia });
        using (Html.BeginForm("ChooseSubmit", "SanPham_DanhMucGia", new { ParentID = ParentID, iID_MaDanhMucGia = iID_MaDanhMucGia, iID_MaSanPham = iID_MaSanPham, iID_MaLoaiHinh = iID_MaLoaiHinh }, FormMethod.Post, new { }))
        {
    %>
    <div class="box_tong">
        <div id="nhapform">
            <div id="form2">
                <table cellpadding="5" cellspacing="5" width="100%">
                    <tr>
                        <td class="td_form2_td1" align="right" style="width:25%">
                            <div><b>Mã khoản mục</b></div>
                        </td>
                        <td class="td_form2_td5" align="left" style="width:10%">
                            <div><b><%=sKyHieu%></b></div>
                        </td>
                        <td class="td_form2_td1" align="right" style="width:25%">
                            <div>
                                <b>Tên khoản mục</b></div>
                        </td>
                        <td class="td_form2_td5" align="left" style="width:40%">
                            <div><b><%=sTen%></b></div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <br />
    <div style="overflow:auto;height:300px">
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0" border="0" width="100%" class="mGrid">
                    <tr>
                        <%--<th style="width: 10%;" align="center">
                        </th--%>
                        <th style="width: 40%;" align="center">
                            Tên vật tư
                        </th>
                        <th style="width: 30%;" align="center">
                            Đơn vị tính
                        </th>
                        <th style="width: 15%;" align="center">
                            Mua ngoài
                        </th>
                        <th style="width: 15%;" align="center">
                            Ngân sách
                        </th>
                    </tr>
                <% foreach (DataRow row in dtChiTiet.Rows){
                %>
                    <tr>
<%--                        <td align="center">
                            <input type="checkbox" name="<%=ParentID %>_chonVatTu_<%=HamChung.ConvertToString(row["iID_MaVatTu"])%>" id="<%=ParentID %>_chonVatTu_<%=HamChung.ConvertToString(row["iID_MaVatTu"])%>" value="<%=HamChung.ConvertToString(row["iID_MaVatTu"])%>" group-index="1"/>
                            
                        </td>--%>
                        <td><%=HamChung.ConvertToString(row["sTen"])%></td>
                        <td><%=HamChung.ConvertToString(row["sTen_DonVi"])%></td>
                        <td align="center">
                        <%= Html.Hidden(ParentID + "_chonVatTu_" + HamChung.ConvertToString(row["iID_MaVatTu"]), HamChung.ConvertToString(row["iID_MaVatTu"]))%>
                        <% 
                            int bMuaNgoai =  SanPham_VatTuModels.Check_VatTu_NganSach(iID_MaDanhMucGia,HamChung.ConvertToString(row["iID_MaVatTu"]), "0");
                            int bNganSach = SanPham_VatTuModels.Check_VatTu_NganSach(iID_MaDanhMucGia, HamChung.ConvertToString(row["iID_MaVatTu"]), "1");
                        %>
                            <input type="checkbox" name="<%=ParentID %>_bMuaNgoai_<%=HamChung.ConvertToString(row["iID_MaVatTu"])%>" id="<%=ParentID %>_bMuaNgoai_<%=HamChung.ConvertToString(row["iID_MaVatTu"])%>" <%if(bMuaNgoai >0) {%>disabled="disabled"<%} %> value="1" />
                        </td>
                        <td align="center">
                            <input type="checkbox" name="<%=ParentID %>_bNganSach_<%=HamChung.ConvertToString(row["iID_MaVatTu"])%>" id="<%=ParentID %>_bNganSach_<%=HamChung.ConvertToString(row["iID_MaVatTu"])%>" <%if(bNganSach >0) {%>disabled="disabled"<%} %> value="1" />
                        </td>
                    </tr>
                <%      
                }
                %>
                </table>
            </td>
        </tr>
    </table>
    </div>
    <div><table cellpadding="0" cellspacing="0" border="0" align="right">
        <tr>
            <td>
                <input type="submit" class="button" id="save_button" value="Chọn"/>
            </td>
            <td width="5px">
            </td>
        </tr>
    </table></div>
    <%
        } if (dt != null) { dt.Dispose(); };    
    %>
</div>
</body>
</html>
