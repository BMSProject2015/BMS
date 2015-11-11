<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%       
    PartialModel dlChuyen = (PartialModel)Model;
    String ParentID = dlChuyen.ControlID;
    Dictionary<string, object> dicData = dlChuyen.dicData;
    string TenBang = (string)dicData["TenBang"];
    string TruongKhoa = (string)dicData["TruongKhoa"];
    NameValueCollection data = (NameValueCollection)dicData["data"];
    string sDanhSachTruongCam = BaoMat.DanhSachTruongCam(User.Identity.Name, TenBang);
    string urlSua = Url.Action("Edit", "Luat", new { MaLuat = data[TruongKhoa] });
%>
<div class="box_tong">
    <div class="title_tong">
		<table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
                <td style="width: 5%;"><span>Luật:</span></td>
                <td>
                    <%= MyHtmlHelper.Label(data, "sTen", sDanhSachTruongCam)%>
                </td>
            	<td align="right">
                	 <span><%=NgonNgu.LayXau("Phân quyền chức năng cấm của các bảng")%></span>
                </td>
            </tr>
        </table>
	</div> 
    <%Html.RenderPartial("~/Views/CacBang/BaoMat/ChucNangCam/Partial_List.aspx", new { ControlID = ParentID + "_ChucNangCam", MaLuat = data[TruongKhoa] }); %>
</div>