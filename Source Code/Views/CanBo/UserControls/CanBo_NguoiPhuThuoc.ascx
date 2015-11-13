<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="System.ComponentModel" %>
<%
    int i;
    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(Model);
    String MaND = Convert.ToString(props["MaND"].GetValue(Model));
    string iID_MaCanBo = Convert.ToString(props["iID_MaCanBo"].GetValue(Model));
    var dt = CanBo_DanhMucNhanSuModels.Get_DanhSach_NguoiPhuThuoc(iID_MaCanBo);
   
%>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span>Danh sách người phụ thuộc</span>
                </td>
                <td align="right">
                    <div style="float: right; margin-right: 5px;" onclick="OnInit_CT('Thêm người phụ thuộc');">
                        <%= Ajax.ActionLink("Thêm mới", "Index", "NhapNhanh", new { id = "NS_NGUOIPHUTHUOC", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaCanBo = iID_MaCanBo }, new AjaxOptions { }, new { @class = "button_title" })%>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <table class="mGrid">
        <tr>
            <th style="width: 3%;" align="center">
                STT
            </th>
            <th style="width: 20%;" align="center">
                Họ tên
            </th>
            <th style="width: 13%;" align="center">
                Quan hệ
            </th>
            <th  align="center">
                Lý do giảm trừ
            </th>
            <th style="width: 10%;" align="center">
                Từ ngày
            </th>
            <th align="center" style="width: 10%;">
                Đến ngày
            </th>
            <th style="width: 3%;" align="center">
                Sửa
            </th>
            <th style="width: 3%;" align="center">
                Xóa
            </th>
        </tr>
        <%
            for (i = 0; i < dt.Rows.Count; i++)
            {
                DataRow R = dt.Rows[i];
                String classtr = "";
                int STT = i + 1;



                String strDelete = "";
                if (LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHeNhanSu, MaND))
                {
                    strDelete = MyHtmlHelper.ActionLink(Url.Action("Delete", "CanBo_NguoiPhuThuoc", new { iID_MaNguoiPhuThuoc = R["iID_MaNguoiPhuThuoc"], iID_MaCanBo = R["iID_MaCanBo"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "");
                }

                //gioi tinh
                String iID_MaQuanHe = "", MaQuanHe = "", TuNgay = "", DenNgay = "";


                MaQuanHe = HamChung.ConvertToString(R["iID_MaQuanHe"]);
                if (String.IsNullOrEmpty(MaQuanHe) == false && MaQuanHe != "" && MaQuanHe != Convert.ToString(Guid.Empty))
                {
                    iID_MaQuanHe = Convert.ToString(DanhMucModels.GetRow_DanhMuc(Convert.ToString(MaQuanHe)).Rows[0]["sTen"]);
                }

                String dTuNgay = String.Format("{0:dd/MM/yyyy}", HamChung.ConvertDateTime(R["dTuNgay"]));
                if (dTuNgay == "01/01/0001") TuNgay = "";
                else TuNgay = dTuNgay;

                String dDenNgay = String.Format("{0:dd/MM/yyyy}", HamChung.ConvertDateTime(R["dDenNgay"]));
                if (dDenNgay == "01/01/0001") DenNgay = "";
                else DenNgay = dDenNgay;

                String iID_MaNguoiPhuThuoc = HamChung.ConvertToString(R["iID_MaNguoiPhuThuoc"]);
        %>
        <tr <%=classtr %>>
            <td align="center">
                <%=STT%>
            </td>
            <td align="left">
                <%= HttpUtility.HtmlEncode(R["sHoTen"])%>
            </td>
            <td align="left">
                <%=HttpUtility.HtmlEncode(iID_MaQuanHe)%>
            </td>
            <td align="left">
                <%=HttpUtility.HtmlEncode(R["sLyDoGiamTru"])%>
            </td>
            <td align="center">
                <%=TuNgay%>
            </td>
            <td align="center">
                <%=DenNgay%>
            </td>
            <td align="center">
                <div style="float: right; margin-right: 5px;" onclick="OnInit_CT('Sửa thông tin người phụ thuộc');">
                    <%= Ajax.ActionLink("s", "Index", "NhapNhanh", new { id = "NS_NGUOIPHUTHUOC", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaCanBo = iID_MaCanBo, iID_MaNguoiPhuThuoc = iID_MaNguoiPhuThuoc }, new AjaxOptions { }, new { @class = "icon_them" })%>
                </div>
            </td>
            <td align="center">
                <%=strDelete%>
            </td>
        </tr>
        <%} %>
    </table>
</div>
<% if (dt != null) dt.Dispose(); %>