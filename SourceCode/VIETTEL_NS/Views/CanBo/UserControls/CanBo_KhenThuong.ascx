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
    var dt = CanBo_DanhMucNhanSuModels.Get_DanhSach_KhenThuong(iID_MaCanBo);
   
%>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span>Danh sách quá trình Khen thưởng</span>
                </td>
                <td align="right">
                    <div style="float: right; margin-right: 5px;" onclick="OnInit_CT('Thêm quá trình khen thưởng');">
                        <%= Ajax.ActionLink("Thêm mới", "Index", "NhapNhanh", new { id = "NS_KHENTHUONG", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaCanBo = iID_MaCanBo }, new AjaxOptions { }, new { @class = "button_title" })%>
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
            <th style="width: 7%;" align="center">
                Loại
            </th>
            <th style="width: 13%;" align="center">
                Cấp khen thưởng/phong tặng
            </th>
            <th align="center" style="width: 10%;">
                Hình thức khen thưởng/phong tặng
            </th>
            <th  align="center">
                Lý do khen thưởng/phong tặng
            </th>
            <th align="center" style="width: 10%;">
                Năm khen thưởng
            </th>
            <th align="center" style="width: 10%;">
                Số QĐ
            </th>
            <th align="center" style="width: 10%;">
                Ngày ký
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
                    strDelete = MyHtmlHelper.ActionLink(Url.Action("Delete", "CanBo_KhenThuong", new { iID_MaKhenThuong = R["iID_MaKhenThuong"], iID_MaCanBo = R["iID_MaCanBo"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "");
                }

                //gioi tinh
                String NgayKy = "", iID_MaHinhThuc = "", MaCapPhongTang = "", iID_MaCapPhongTang = "", MaHinhThuc = "", iLoai = "", iThang = "", iNam = "", sThoiGian = "";

                String dTuNgay = String.Format("{0:dd/MM/yyyy}", HamChung.ConvertDateTime(R["dNgayKy"]));
                if (dTuNgay == "01/01/0001") NgayKy = "";
                else NgayKy = dTuNgay;

                String iID_MaKhenThuong = HamChung.ConvertToString(R["iID_MaKhenThuong"]);
                //lấy mã nước và mã nguồn kinh phí

                if (HamChung.ConvertToString(R["iLoai"]) == "1") iLoai = "Khen thưởng";
                else iLoai = "Danh hiệu thi đua";
                MaCapPhongTang = HamChung.ConvertToString(R["iID_MaCapPhongTang"]);
                MaHinhThuc = HamChung.ConvertToString(R["iID_MaHinhThuc"]);
                if (String.IsNullOrEmpty(MaCapPhongTang) == false && MaCapPhongTang != "" && MaCapPhongTang != Convert.ToString(Guid.Empty))
                {
                    if ( (DanhMucModels.GetRow_DanhMuc(Convert.ToString(MaCapPhongTang)).Rows.Count>0))
                    {
                        iID_MaCapPhongTang =
                            Convert.ToString(
                                DanhMucModels.GetRow_DanhMuc(Convert.ToString(MaCapPhongTang)).Rows[0]["sTen"]);
                    }
                  
                }
                if (String.IsNullOrEmpty(MaHinhThuc) == false && MaHinhThuc != "" && MaHinhThuc != Convert.ToString(Guid.Empty))
                {
                     if ( (DanhMucModels.GetRow_DanhMuc(Convert.ToString(MaHinhThuc)).Rows.Count>0))
                    {
                        iID_MaHinhThuc =
                            Convert.ToString(DanhMucModels.GetRow_DanhMuc(Convert.ToString(MaHinhThuc)).Rows[0]["sTen"]);
                    }
                }

                //Thời gian
                iThang = HamChung.ConvertToString(R["iThang"]);
                iNam = HamChung.ConvertToString(R["iNam"]);
                if (iThang == "0") sThoiGian = iNam;
                else sThoiGian = iThang + " - " + iNam;
        %>
        <tr <%=classtr %>>
            <td align="center">
                <%=STT%>
            </td>
            <td align="center">
                <%=iLoai%>
            </td>
            <td align="left">
                <%=HttpUtility.HtmlEncode(iID_MaCapPhongTang)%>
            </td>
            <td align="left">
                <%= HttpUtility.HtmlEncode(iID_MaHinhThuc)%>
            </td>
            <td align="left">
                <%=  HttpUtility.HtmlEncode(R["sLyDo"])%>
            </td>
            <td align="left">
                <%=  HttpUtility.HtmlEncode(sThoiGian)%>
            </td>
            <td align="left">
                <%=  HttpUtility.HtmlEncode(R["sSoQD"])%>
            </td>
            <td align="center">
                <%=NgayKy%>
            </td>
            <td align="center">
                <div style="float: right; margin-right: 5px;" onclick="OnInit_CT('Sửa quá trình đi nước ngoài');">
                    <%= Ajax.ActionLink("s", "Index", "NhapNhanh", new { id = "NS_KHENTHUONG", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaCanBo = iID_MaCanBo, iID_MaKhenThuong = iID_MaKhenThuong }, new AjaxOptions { }, new { @class = "icon_them" })%>
                </div>
            </td>
            <td align="center">
                <%=strDelete%>
            </td>
        </tr>
        <%} %>
    </table>
</div>
