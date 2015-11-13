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
    String sLayTuDuToan = Request.QueryString["sLayTuDuToan"];
    //Cập nhập các thông tin tìm kiếm
    String strDSTruong = MucLucNganSachModels.strDSTruong;
    String strDSTruongDoRong = MucLucNganSachModels.strDSTruongDoRong;
    String[] arrDSTruong = strDSTruong.Split(',');
    String[] arrDSTruongDoRong = strDSTruongDoRong.Split(',');
    String urlChiTiet = Url.Action("Index", "PhanBo_ChiTieuChiTiet", new { iID_MaChiTieu = iID_MaChiTieu });
    Dictionary<String, String> arrGiaTriTimKiem = new Dictionary<string, string>();
    for (int i = 0; i < arrDSTruong.Length; i++)
    {
        arrGiaTriTimKiem.Add(arrDSTruong[i], Request.QueryString[arrDSTruong[i]]);
    }
%>

<div class="box_tong">
    <div id="nhapform">
        <div id="form2">
            <table class="mGrid1">
            <tr>
                <%
                for (int j = 0; j < arrDSTruong.Length - 1; j++)
                {
                    int iColWidth = Convert.ToInt32(arrDSTruongDoRong[j]) + 4;
                    if (j == 0) iColWidth = iColWidth + 3;
                    String strAttr = String.Format("class='input1_4' onkeypress='jsChiTieu_Search_onkeypress(event)' search-control='1' search-field='{1}' style='width:{0}px;height:22px;'", iColWidth - 2, arrDSTruong[j]);
                    if (CoTabIndex)
                    {
                        strAttr += " tab-index='-1'";
                    }
                    %>
                    <td style="text-align:left;width:<%=iColWidth%>px;">
                        <%=MyHtmlHelper.TextBox(new { ParentID = ParentID, Value = arrGiaTriTimKiem[arrDSTruong[j]], TenTruong = arrDSTruong[j], LoaiTextBox = "2", Attributes = strAttr })%>
                    </td>
                    <%
                }
                %>
            </tr>
            </table>
            <iframe id="ifrChiTietChungTu" width="100%" height="530px" src="<%= Url.Action("ChiTieuChiTiet_Frame", "PhanBo_ChiTieuChiTiet", new {iID_MaChiTieu=iID_MaChiTieu,sLayTuDuToan=sLayTuDuToan})%>"></iframe>
            <script type="text/javascript">
                function LayDuToan() {
                    var url = '<%=urlChiTiet%>';
                    url = url + "&sLayTuDuToan=1";
                    location.href = url;
                }
            </script>
        </div>
    </div>
</div>