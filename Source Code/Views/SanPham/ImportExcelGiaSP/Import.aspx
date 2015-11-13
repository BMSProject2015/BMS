    <%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=NgonNgu.LayXau("Cổng thông tin điện tử BQP")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%  
    String ParentID = "Import";
    String iID_MaSanPham = Request.QueryString["iID_MaSanPham"];
    String FileName = Request.QueryString["FileName"];
    String FilePath = Request.QueryString["FilePath"];
    String KieuNhap = Request.QueryString["KieuNhap"];
    String isError = Request.QueryString["isError"];
    String iID_MaLoaiHinh = Request.QueryString["iID_MaLoaiHinh"];
        
    String UserID = User.Identity.Name;
    DataTable dtSP = SanPham_VatTuModels.Get_SanPham(iID_MaSanPham);
    DataRow R;
    string sTen = "", sMa = "", rSoLuong = "", iID_MaDonVi = "", strTenDonVi = "", sQuyCach = "", iDM_MaDonViTinh = "", sTen_DonViTinh = "", iID_LoaiDonVi = "", rThueGTGT = "", rLoiNhuan = "";
    if (dtSP.Rows.Count > 0)
    {
        R = dtSP.Rows[0];
        sTen = HamChung.ConvertToString(R["sTen"]);
        sMa = HamChung.ConvertToString(R["sMa"]);
        rSoLuong = HamChung.ConvertToString(R["rSoLuong"]);
        sQuyCach = HamChung.ConvertToString(R["sQuyCach"]);
        iID_MaDonVi = HamChung.ConvertToString(R["iID_MaDonVi"]);
        strTenDonVi = DonViModels.Get_TenDonVi(Convert.ToString(R["iID_MaDonVi"]));
        iDM_MaDonViTinh = HamChung.ConvertToString(R["iDM_MaDonViTinh"]);
        sTen_DonViTinh = SanPham_VatTuModels.Get_TenDonViTinh(iDM_MaDonViTinh);
    }
    DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(UserID);
    SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
    dtDonVi.Dispose();
    
    
    string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=Yes'";
    conStr = String.Format(conStr, FilePath);
    OleDbConnection connExcel = new OleDbConnection(conStr);
    OleDbCommand cmdExcel = new OleDbCommand();
    OleDbDataAdapter oda = new OleDbDataAdapter();
    cmdExcel.Connection = connExcel;
    connExcel.Open();

    DataTable dtSheet = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
    DataRow[] dataRow = null;
    dataRow = dtSheet.Select("TABLE_NAME IN ('SanPham$','ChiTietGia$')");
        
    DataTable dt = dtSheet.Clone();
    foreach (DataRow dr in dataRow)
    {
        dt.ImportRow(dr);
    }
    SelectOptionList slSheets = new SelectOptionList(dt, "TABLE_NAME", "TABLE_NAME");
    connExcel.Close();
    using (Html.BeginForm("EditSubmit", "ImportExcelGiaSP", new { ParentID = ParentID, KieuNhap = KieuNhap, iID_MaSanPham = iID_MaSanPham, iID_MaLoaiHinh = iID_MaLoaiHinh }))
    {          
%>
<%= Html.Hidden(ParentID + "_FileName", FileName)%>
<%= Html.Hidden(ParentID + "_FilePath", FilePath)%>
<%= Html.Hidden(ParentID + "_iID_MaSanPham", iID_MaSanPham)%>

<script type="text/javascript">
    function btnXem() {
        var FilePath, SheetName;
        var url = unescape('<%= Url.Action("get_dtSheet?ParentID=#0&FilePath=#1&SheetName=#2", "ImportExcelGiaSP") %>');
        FilePath = document.getElementById('<%= ParentID %>_FilePath').value;
        SheetName = document.getElementById('<%= ParentID %>_SheetName').value;
        url = unescape(url.replace("#0", "<%= ParentID %>"));
        url = unescape(url.replace("#1", FilePath));
        url = unescape(url.replace("#2", SheetName));
        $.getJSON(url, function (data) {
            document.getElementById("<%=ParentID%>_ChiTiet").style.display = 'block';
            document.getElementById("<%= ParentID %>_tdList").innerHTML = data.sData;
        });
    }
    function xemDuLieu(SheetName) {
        var FilePath;
        var url = unescape('<%= Url.Action("get_dtSheet?ParentID=#0&FilePath=#1&SheetName=#2", "ImportExcelGiaSP") %>');
        FilePath = document.getElementById('<%= ParentID %>_FilePath').value;
        url = unescape(url.replace("#0", "<%= ParentID %>"));
        url = unescape(url.replace("#1", FilePath));
        url = unescape(url.replace("#2", SheetName + "$"));
        $.getJSON(url, function (data) {
//            document.getElementById("<%=ParentID%>_ChiTiet").style.display = 'block';
            document.getElementById("<%= ParentID %>_td" + SheetName).innerHTML = data.sData;
        });
    }
    function setCheckboxes() {
        var cb = document.getElementById('<%= ParentID %>_tdChiTietGia').getElementsByTagName('input');

        for (var i = 0; i < cb.length; i++) {
            cb[i].checked = document.getElementById('checkall').checked;
        }
    }                                       
</script>
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
<div class="box_tong" id="<%= ParentID %>_DivSanPham">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span><%=NgonNgu.LayXauChuHoa("Thông tin sản phẩm")%></span>
                </td>
            </tr>
        </table>
    </div>
    <div id="form2">
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                    <div><b><%=NgonNgu.LayXau("Mã sản phẩm:")%></b></div>
                </td>
                <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                    <div style="float:left"><%=NgonNgu.LayXau(sMa)%></div>
                    <%= Html.Hidden(ParentID + "_sMa", sMa)%>
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
                    <%= Html.Hidden(ParentID + "_sTen", sTen)%>
                </td>
                <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
            </tr>
            <tr>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                        <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                            <div><b><%=NgonNgu.LayXau("Loại đơn vị:")%></b></div>
                        </td>
                        <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                            <select id="<%=ParentID %>_iID_LoaiDonVi" name="<%=ParentID %>_iID_LoaiDonVi" class="input1_2" style="width:99%">
                                <option value = "1" <%if(iID_LoaiDonVi == "1"){ %> selected="selected" <%} %> >Sản xuất</option>
                                <option value = "2" <%if(iID_LoaiDonVi == "2"){ %> selected="selected" <%} %> >Đặt hàng</option>
                                <option value = "3" <%if(iID_LoaiDonVi == "3"){ %> selected="selected" <%} %> >Cục tài chính</option>
                            </select>
                        </td>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                    </tr>
            <tr>
                <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                    <div><b><%=NgonNgu.LayXau("Đơn vị:")%></b></div>
                </td>
                <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                    <%=MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\"")%>
                </td>
                <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                    <div><b><%=NgonNgu.LayXau("Loại sửa chữa:")%></b></div>
                </td>
                <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                    <select class="input1_2" style="width:99%" disabled="disabled">
                        <option value = "1" <%if(iID_MaLoaiHinh == "1"){ %> selected="selected" <%} %> >Sửa chữa lớn</option>
                        <option value = "2" <%if(iID_MaLoaiHinh == "2"){ %> selected="selected" <%} %> >Sửa chữa vừa</option>
                        <option value = "3" <%if(iID_MaLoaiHinh == "3"){ %> selected="selected" <%} %> >Sửa chữa nhỏ</option>
                    </select>
                    <%= Html.Hidden(ParentID + "_iID_MaLoaiHinh", iID_MaLoaiHinh)%>
                </td>
                <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                    <div><b><%=NgonNgu.LayXau("Số lượng sản phẩm:")%></b></div>
                </td>
                <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                    <%=MyHtmlHelper.TextBox(ParentID, rSoLuong, "rSoLuong", "", "class=\"input1_2\" tab-index='-1' ")%>
                    <%= Html.Hidden(ParentID + "_iDM_MaDonViTinh", iDM_MaDonViTinh)%>
                </td>
                <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                    <div><b><%=NgonNgu.LayXau("Lợi nhuận dự kiến (%):")%></b></div>
                </td>
                <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                    <%=MyHtmlHelper.TextBox(ParentID, rLoiNhuan, "rLoiNhuan", "", "class=\"input1_2\" tab-index='-1' ")%>
                </td>
                <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                    <div><b><%=NgonNgu.LayXau("Thuế giá trị gia tăng (%):")%></b></div>
                </td>
                <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                    <%=MyHtmlHelper.TextBox(ParentID, rThueGTGT, "rThueGTGT", "", "class=\"input1_2\" tab-index='-1' ")%>
                </td>
                <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                    <div><b><%=NgonNgu.LayXau("Quy cách phẩm chất:")%></b></div>
                </td>
                <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                    <%=MyHtmlHelper.TextArea(ParentID, sQuyCach, "sQuyCach", "", "class=\"input1_2\" tab-index='-1' ")%>
                </td>
                <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
            </tr>
        </table>
    </div>
</div>
<div class="doancach">&nbsp;</div>
<div class="box_tong" id="<%= ParentID %>_ChiTiet">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span><%=NgonNgu.LayXauChuHoa("Thông tin chi tiết")%></span>
                </td>
            </tr>
        </table>
    </div>
    <div class="form_nhap">
        <div class="form2">
            <div class="content" style="overflow:auto" id="<%= ParentID %>_tdChiTietGia">                            
                <%ImportExcelGiaSPController.SheetData dataSheet2 = ImportExcelGiaSPController.get_objSheet(ParentID, FilePath, "ChiTietGia$",iID_MaSanPham, iID_MaLoaiHinh);%>
                <%= dataSheet2.sData%>
                <script type="text/javascript">
//                    xemDuLieu("ChiTietGia");
                </script>
            </div>
        </div>
    </div>
</div>
<br />
<table cellpadding="0" cellspacing="0" border="0" width="100%">
	<tr>
		<td width="70%">&nbsp;</td>
		<td width="30%" align="right">
            <table cellpadding="0" cellspacing="0" border="0" align="right">
        	    <tr>
            	    <td>
                        <%if (isError != "1")
                          { %>
            	        <input type="submit" class="button4" value="Lưu" />
                        <%} else { %>
                        <span style = "color:Red">File excel lựa chọn không đúng quy cách!</span>
                        <%} %>
            	    </td>
                    <td width="5px"></td>
                    <td>
                        <input type="button" class="button4" value="Quay lại" onclick="javascript:history.go(-1)" />
                    </td>
                </tr>
            </table>
		</td>
	</tr>
</table>
<%} %>
</asp:Content>
