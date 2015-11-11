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
    var dt = CanBo_DanhMucNhanSuModels.Get_DanhSach_QuaTrinhCT(iID_MaCanBo);
   
%>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span>Danh sách quá trình Công tác</span>
                </td>
                <td align="right">
                    <div style="float: right; margin-right: 5px;" onclick="OnInit_CT('Thêm quá trình công tác');">
                        <%= Ajax.ActionLink("Thêm mới", "Index", "NhapNhanh", new { id = "NS_QUATRINHCONGTAC", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaCanBo = iID_MaCanBo }, new AjaxOptions { }, new { @class = "button_title" })%>
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
            <th style="width: 10%;" align="center">
                Từ ngày
            </th>
            <th style="width: 10%;" align="center">
                Đến ngày
            </th>
            <th style="width: 20%;" align="center">
                Chức vụ đảm nhận
            </th>
            <th  align="center">
                Đơn vị công tác
            </th>
            <th align="center" style="width: 10%;">
                Quyết định số
            </th>
            <th style="width: 10%;" align="center">
                Ngày hiệu lực
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
                    strDelete = MyHtmlHelper.ActionLink(Url.Action("Delete", "CanBo_QuaTrinhCongTac", new { iID_MaQuaTrinhCongTac = R["iID_MaQuaTrinhCongTac"], iID_MaCanBo = R["iID_MaCanBo"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "");
                }

                //gioi tinh
                String dNgayHieuLuc = "", NgayHieuLuc = "", TuNgay = "", DenNgay = "";

                String dTuNgay = String.Format("{0:dd/MM/yyyy}", HamChung.ConvertDateTime(R["dTuNgay"]));
                if (dTuNgay == "01/01/0001") TuNgay = "";
                else TuNgay = dTuNgay;

                String dDenNgay = String.Format("{0:dd/MM/yyyy}", HamChung.ConvertDateTime(R["dDenNgay"]));
                if (dDenNgay == "01/01/0001") DenNgay = "";
                else DenNgay = dDenNgay;



                dNgayHieuLuc = String.Format("{0:dd/MM/yyyy}", HamChung.ConvertDateTime(R["dNgayHieuLuc"]));
                if (dNgayHieuLuc == "01/01/0001") NgayHieuLuc = "";
                else NgayHieuLuc = dNgayHieuLuc;


                String iID_MaQuaTrinhCongTac = HamChung.ConvertToString(R["iID_MaQuaTrinhCongTac"]);
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
                <%= HttpUtility.HtmlEncode(R["sChucVuDamNhan"])%>
            </td>
            <td align="left">
                <%=HttpUtility.HtmlEncode( R["sDonViCongTac"])%>
            </td>
            <td align="left">
                <%=HttpUtility.HtmlEncode(R["sSoQD"])%>
            </td>
            <td align="left">
                <%=NgayHieuLuc%>
            </td>
            <td align="center">
                <div style="float: right; margin-right: 5px;" onclick="OnInit_CT('Sửa quá trình công tác');">
                    <%= Ajax.ActionLink("s", "Index", "NhapNhanh", new { id = "NS_QUATRINHCONGTAC", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaCanBo = iID_MaCanBo, iID_MaQuaTrinhCongTac = iID_MaQuaTrinhCongTac }, new AjaxOptions { }, new { @class = "icon_them" })%>
                </div>
            </td>
            <td align="center">
                <%=strDelete%>
            </td>
        </tr>
        <%} %>
    </table>
</div>
