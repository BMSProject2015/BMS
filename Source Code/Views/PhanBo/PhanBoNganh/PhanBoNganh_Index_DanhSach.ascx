<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="System.ComponentModel" %>
<%
    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(Model);
    String ControlID = Convert.ToString(props["ControlID"].GetValue(Model));
    Boolean CoTabIndex = (props["CoTabIndex"] == null) ? false : Convert.ToBoolean(props["CoTabIndex"].GetValue(Model));
    String ParentID = ControlID + "_Search";
    String iID_MaChiTieu = Request.QueryString["iID_MaChiTieu"];

    //Cập nhập các thông tin tìm kiếm
    String strDSTruong = MucLucNganSachModels.strDSTruong + ",iID_MaDonVi";
    String strDSTruongDoRong = MucLucNganSachModels.strDSTruongDoRong + ",42";
    String[] arrDSTruong = strDSTruong.Split(',');
    String[] arrDSTruongDoRong = strDSTruongDoRong.Split(',');

    Dictionary<String, String> arrGiaTriTimKiem = new Dictionary<string, string>();
    for (int i = 0; i < arrDSTruong.Length; i++)
    {
        arrGiaTriTimKiem.Add(arrDSTruong[i], Request.QueryString[arrDSTruong[i]]);
    }
    NameValueCollection data = PhanBo_ChiTieuModels.LayThongTin(iID_MaChiTieu);
%>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span>Thông tin chứng từ</span>
                </td>
                <td align="right">
                    <span>F2 - Thêm</span>
                </td>
                <td align="right" style="width: 100px;">
                    <span>Delete - Xóa</span>
                </td>
                <td align="left">
                    <span>F10 - Lưu</span>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form3">
            <tr>
                <td class="td_form2_td1" style="width: 15%">
                    <div>
                        <b>Đợt chỉ tiêu</b></div>
                </td>
                <td class="td_form2_td5" style="width: 20%">
                    <div>
                        <%=String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(data["dNgayDotPhanBo"]))%></div>
                </td>
                <td class="td_form2_td1" style="width: 15%">
                    <div>
                        <b>Ngày chứng từ</b></div>
                </td>
                <td class="td_form2_td5" style="width: 20%">
                    <div>
                        <%=String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(data["dNgayChungTu"]))%></div>
                </td>
            </tr>
            <tr>
                <td class="td_form2_td1" style="width: 15%">
                    <div>
                        <b>Số phân bổ</b></div>
                </td>
                <td class="td_form2_td5" style="width: 20%">
                    <div>
                        <%=data["sTienToChungTu"]%><%=data["iSoChungTu"]%></div>
                </td>
                <td class="td_form2_td1" style="width: 15%">
                    <div>
                        <b>Nội dung</b></div>
                </td>
                <td class="td_form2_td5" style="width: 20%">
                    <div>
                        <%=data["sNoiDung"]%></div>
                </td>
            </tr>
        </table>
        <div id="form2">
            <table class="mGrid1">
                <tr>
                    <%
                        for (int j = 0; j < arrDSTruong.Length; j++)
                        {
                            int iColWidth = Convert.ToInt32(arrDSTruongDoRong[j]) + 4;
                           
                            if (j == 0) iColWidth = iColWidth + 3;
                            String strAttr = String.Format("class='input1_4' onkeypress='jsPhanBoNganh_Search_onkeypress(event)' search-control='1' search-field='{1}' style='width:{0}px;height:22px;'", iColWidth - 2, arrDSTruong[j]);
                            if (CoTabIndex)
                            {
                                strAttr += " tab-index='-1'";
                            }
                    %>
                    <td style="text-align: left; width: <%=iColWidth%>px;">
                        <%=MyHtmlHelper.TextBox(new { ParentID = ParentID, Value = arrGiaTriTimKiem[arrDSTruong[j]], TenTruong = arrDSTruong[j], LoaiTextBox = "2", Attributes = strAttr })%>
                    </td>
                    <%
                        }
                    %>
                </tr>
            </table>
            <iframe id="ifrChiTietChungTu" width="100%" height="530px" src="<%= Url.Action("PhanBoNganhChiTiet_Frame", "PhanBo_PhanBoNganh", new {iID_MaChiTieu=iID_MaChiTieu})%>">
            </iframe>
        </div>
    </div>
</div>
