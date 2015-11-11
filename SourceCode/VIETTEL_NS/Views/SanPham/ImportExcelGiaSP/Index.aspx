<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=NgonNgu.LayXau("Cổng thông tin điện tử BQP")%>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    //PartialModel dlChuyen = (PartialModel)Model;
    String ParentID = "Index";
    String UserID = User.Identity.Name;
    string iID_MaSanPham = HamChung.ConvertToString(ViewData["iID_MaSanPham"]);
    DataTable  dt = SanPham_VatTuModels.Get_SanPham(iID_MaSanPham);
    DataRow R;
    string sTen = "", sMa = "", rSoLuong = "", iID_MaDonVi = "", strTenDonVi = "", sQuyCach = "", iDM_MaDonViTinh = "", sTen_DonViTinh = "", iID_MaLoaiHinh = "";
    if (dt.Rows.Count > 0)
    {
        R = dt.Rows[0];
        sTen = HamChung.ConvertToString(R["sTen"]);
        sMa = HamChung.ConvertToString(R["sMa"]);
        rSoLuong = HamChung.ConvertToString(R["rSoLuong"]);
        sQuyCach = HamChung.ConvertToString(R["sQuyCach"]);
        iID_MaDonVi = HamChung.ConvertToString(R["iID_MaDonVi"]);
        strTenDonVi = DonViModels.Get_TenDonVi(Convert.ToString(R["iID_MaDonVi"]));
        iDM_MaDonViTinh = HamChung.ConvertToString(R["iDM_MaDonViTinh"]);
        sTen_DonViTinh = SanPham_VatTuModels.Get_TenDonViTinh(iDM_MaDonViTinh);
        iID_MaLoaiHinh = HamChung.ConvertToString(R["iID_MaLoaiHinh"]);
    }
    DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(UserID);
    SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
    dtDonVi.Dispose();
    //String Extension = Request.QueryString["Extension"];
    //String FilePath = Request.QueryString["FilePath"];
    string urlImport = Url.Action("Load", "ImportExcelGiaSP", new { ParentID = ParentID, iID_MaSanPham = iID_MaSanPham });

    using (Html.BeginForm("Load", "ImportExcelGiaSP", new { ParentID = ParentID, iID_MaSanPham = iID_MaSanPham }))
    {
%>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td style="width: 10%">
            <div style="padding-left: 20px; padding-top: 5px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                <b><%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
            </div>         
        </td>
        <td align="left">
            <%Html.RenderPartial("~/Views/Shared/LinkNhanhVattu.ascx"); %>
        </td>
    </tr>
</table>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span>
                        <%=NgonNgu.LayXau("Bước 1: Chọn file")%></span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="nhap_exel">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                 <tr>
                <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                    <div><b><%=NgonNgu.LayXau("Mã sản phẩm:")%></b></div>
                </td>
                <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                    <div style="float:left"><%=NgonNgu.LayXau(sMa)%></div>
                </td>
                <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                    <div><b><%=NgonNgu.LayXau("Tên sản phẩm:")%></b></div>
                </td>
                <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                    <div style="float:left"><%=NgonNgu.LayXau(sTen)%></div>
                </td>
                <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                    <div><b><%=NgonNgu.LayXau("Loại sửa chữa:")%></b></div>
                </td>
                <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                    <select id="<%=ParentID %>_iID_MaLoaiHinh" name="<%=ParentID %>_iID_MaLoaiHinh" class="input1_2" style="width:99%">
                        <option value = "1" >Sửa chữa lớn</option>
                        <option value = "2" >Sửa chữa vừa</option>
                        <option value = "3" >Sửa chữa nhỏ</option>
                    </select>
                </td>
                <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
            </tr>
                <tr>
                    <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                    <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                        <div><b><%=NgonNgu.LayXau("Kiểu nhập liệu:")%></b></div>
                    </td>
                    <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                        <select id ="<%= ParentID%>_KieuNhap" name ="<%= ParentID%>_KieuNhap" class="input1_2"  style="width:99%">
                            <option value="1">Nhập nhanh</option>
                            <option value="2">Nhập đầy đủ</option>
                        </select>
                    </td>
                    <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                </tr>
                <tr>
                    <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                    <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                        <div><b><%=NgonNgu.LayXau("Tải tệp excel:")%></b></div>
                    </td>
                    <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                        <% =MyHtmlHelper.UploadFile("upload", "Libraries/Excels", DateTime.Now.ToString("HHmmss"))%>
                        <%= Html.Hidden(ParentID + "_sFileName")%>
                        <%= Html.Hidden(ParentID + "_sDuongDan")%>

                        <script type="text/javascript">                            
                            upload.addFilter("Documents (*.xls)", "*.xls");                                        
                            upload.addListener(upload.UPLOAD_COMPLETE, <%= ParentID%>_uploadFile);
                                                                            
                            function <%=ParentID%>_uploadFile(filename, url) {                                                                                        
                                document.getElementById("<%= ParentID%>_sFileName").value = filename;
                                document.getElementById("<%= ParentID%>_sDuongDan").value = upload.serverPath + "/" + url;
                            }
                        </script>
                    </td>
                    <td valign="top" align="left" style="width: 10%;" class="td_form2_td1">
                    </td>
                </tr>
                <tr>
                <td valign="top" align="left" class="td_form2_td1">
                    </td>
                    <td valign="top" align="left" class="td_form2_td1">
                    </td>
                <td  class="td_form2_td1">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td width="50%" style="padding: 10px 10px;" align="left">
                                    <a href="../../Libraries/Excels/GIASPMAU/MauNhapExcel-Nhanh.xls">
                                    Mẫu nhập nhanh</a>
                                </td>
                                <td width="50%" style="padding: 10px 10px;" align="right">
                                    <a href="../../Libraries/Excels/GIASPMAU/MauNhapExcel-ChiTiet.xls">
                                    Mẫu nhập chi tiết
                                    </a>
                                </td>
                            </tr>
                        </table>
                </td>
                <td valign="top" align="left"  class="td_form2_td1">
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                </td>
                <td align="right">
                    <input type="submit" id="Submit1" class="button6" value="<%=NgonNgu.LayXau("Xác nhận")%>" onclick="return checkChoise();" />
                    <script type="text/javascript">
                        function checkChoise() {
                            if (document.getElementById("<%= ParentID%>_sFileName").value == "") {
                                alert('<%=NgonNgu.LayXau("Không tìm thấy tệp excel")%>');
                                return false;
                            }
                            else {
                                javascript: location.href = '<%=urlImport%>';
                                return true;
                            }
                        }
                    </script>
                </td>
            </tr>
        </table>
    </div>

</div>
<div class="cao5px">&nbsp;</div>
<%}%>
<div class="cao5px">&nbsp;</div>
</asp:Content>
