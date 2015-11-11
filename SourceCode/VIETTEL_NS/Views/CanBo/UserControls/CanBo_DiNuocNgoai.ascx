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
    var dt = CanBo_DanhMucNhanSuModels.Get_DanhSach_DiNuocNgoai(iID_MaCanBo);
   
%>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span>Danh sách quá trình Công tác</span>
                </td>
                <td align="right">
                    <div style="float: right; margin-right: 5px;" onclick="OnInit_CT('Thêm quá trình đi nước ngoài');">
                        <%= Ajax.ActionLink("Thêm mới", "Index", "NhapNhanh", new { id = "NS_DINUOCNGOAI", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaCanBo = iID_MaCanBo }, new AjaxOptions { }, new { @class = "button_title" })%>
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
                Từ ngày
            </th>
            <th style="width: 13%;" align="center">
                Đến ngày
            </th>
            <th align="center">
                Nơi đến
            </th>
            <th style="width: 10%;" align="center">
                Lý do đến
            </th>
            <th align="center" style="width: 10%;">
                Kinh phí
            </th>
            <th align="center" style="width: 10%;">
                Nguồn kinh phí
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
                    strDelete = MyHtmlHelper.ActionLink(Url.Action("Delete", "CanBo_DiNuocNgoai", new { iID_MaDiNuocNgoai = R["iID_MaDiNuocNgoai"], iID_MaCanBo = R["iID_MaCanBo"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "");
                }

                //gioi tinh
                String TuNgay = "", DenNgay = "", iID_MaNuoc = "", MaNuoc = "", iID_MaNguonKinhPhi = "", MaNguonKinhPhi = "";

                String dTuNgay = String.Format("{0:dd/MM/yyyy}", HamChung.ConvertDateTime(R["dTuNgay"]));
                if (dTuNgay == "01/01/0001") TuNgay = "";
                else TuNgay = dTuNgay;

                String dDenNgay = String.Format("{0:dd/MM/yyyy}", HamChung.ConvertDateTime(R["dDenNgay"]));
                if (dDenNgay == "01/01/0001") DenNgay = "";
                else DenNgay = dDenNgay;

                String iID_MaDiNuocNgoai = HamChung.ConvertToString(R["iID_MaDiNuocNgoai"]);
                //lấy mã nước và mã nguồn kinh phí

                MaNuoc = HamChung.ConvertToString(R["iID_MaNuoc"]);
                MaNguonKinhPhi = HamChung.ConvertToString(R["iID_MaNguonKinhPhi"]);
                if (String.IsNullOrEmpty(MaNuoc) == false && MaNuoc != "" && MaNuoc != Convert.ToString(Guid.Empty))
                {
                    iID_MaNuoc = Convert.ToString(DanhMucModels.GetRow_DanhMuc(Convert.ToString(MaNuoc)).Rows[0]["sTen"]);
                }
                if (String.IsNullOrEmpty(MaNguonKinhPhi) == false && MaNguonKinhPhi != "" && MaNguonKinhPhi != Convert.ToString(Guid.Empty))
                {
                    iID_MaNguonKinhPhi = Convert.ToString(DanhMucModels.GetRow_DanhMuc(Convert.ToString(MaNguonKinhPhi)).Rows[0]["sTen"]);
                }
        %>
        <tr <%=classtr %>>
            <td align="center">
                <%=STT%>
            </td>
            <td align="center">
                <%=TuNgay%>
            </td>
            <td align="center">
                <%=DenNgay%>
            </td>
            <td align="left">
                <%=HttpUtility.HtmlEncode(iID_MaNuoc)%>
            </td>
            <td align="left">
                <%= HttpUtility.HtmlEncode(R["sLyDoDi"])%>
            </td>
            <td align="left">
                <%=  CommonFunction.DinhDangSo(R["rKinhPhi"])%>
            </td>
            <td align="left">
                <%=HttpUtility.HtmlEncode(iID_MaNguonKinhPhi)%>
            </td>
            <td align="center">
                <div style="float: right; margin-right: 5px;" onclick="OnInit_CT('Sửa quá trình đi nước ngoài');">
                    <%= Ajax.ActionLink("s", "Index", "NhapNhanh", new { id = "NS_DINUOCNGOAI", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaCanBo = iID_MaCanBo, iID_MaDiNuocNgoai = iID_MaDiNuocNgoai }, new AjaxOptions { }, new { @class = "icon_them" })%>
                </div>
            </td>
            <td align="center">
                <%=strDelete%>
            </td>
        </tr>
        <%} %>
    </table>
</div>
