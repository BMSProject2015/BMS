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
    string urlSua = Url.Action("Edit", "LoaiDanhMuc", new { MaLoaiDanhMuc = data[TruongKhoa] });
    string urlSort = Url.Action("Sort", new { MaLoaiDanhMuc = data[TruongKhoa] });  
    string strPath = Url.RouteUrl("", "");
    %>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
	<tr>
		<td width="70%">
			<h3><%= MyHtmlHelper.Label(data, "sTen", sDanhSachTruongCam)%>&nbsp; &nbsp;
			<span class="color_two">
			    (Ngày tạo: <%= MyHtmlHelper.Label(data, "dNgayTao", sDanhSachTruongCam)%>,
			    ngày sửa cuối: <%= MyHtmlHelper.Label(data, "dNgaySua", sDanhSachTruongCam)%>,
                số lần sửa: <%= MyHtmlHelper.Label(data, "iSoLanSua", sDanhSachTruongCam)%>)
            </span></h3>
		</td>		
		<td  width="30%" align="right">	
		    <div style="padding-bottom:5px; margin-right:5px;"><a href="<%=urlSort %>" class="button"><%=NgonNgu.LayXau("Sắp xếp") %></a></div>	
		</td>
		<td  width="30%" align="right">			    
			<div style="padding-bottom:5px;"><a href="<%=urlSua%>" class="button"><%=NgonNgu.LayXau("Sửa")%></a></div>
		</td>
	</tr>
</table>
<div class="box_tong">
	<div class="title_tong">
		<table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span><%=NgonNgu.LayXauChuHoa("Loại danh mục")%></span>
                </td>
            </tr>
        </table>
	</div>
	<div class="nhapform">		
	    <div class="form2">
            <table  cellpadding="0"  cellspacing=""="0" border="0" class="table_form2">
                <tr>
                    <td class="td_form2_td1" style="padding-top:10px;"><div><%=NgonNgu.LayXau("Tên") %></div></td>
                    <td class="td_form2_td2" style="padding-top:10px;"><div>
                        <%= MyHtmlHelper.Label(data, "sTen", sDanhSachTruongCam)%>
                    </div></td>
                </tr>
                <tr>
                    <td class="td_form2_td1"><div><%=NgonNgu.LayXau("Tên bảng") %></div></td>
                    <td class="td_form2_td2"><div>
                         <%= MyHtmlHelper.Label(data, "sTenBang", sDanhSachTruongCam)%>
                    </div></td>
                </tr>
            </table>
        </div>
    </div>
</div>            
     <%Html.RenderPartial("~/Views/CacBang/DanhMuc/DanhMuc/Partial_List.aspx", new PartialModel("Partial_List", (Dictionary<string, object>)ViewData["DC_LoaiDanhMuc_dicData"])); %>
<% %>
